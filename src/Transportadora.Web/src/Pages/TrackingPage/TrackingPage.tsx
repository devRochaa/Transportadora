import { useEffect, useRef, useState } from "react";
import { api } from "@/Pages/TrackingPage/api";
import type { VehiclePositionArgs } from "@/args";

import L from "leaflet";
import "leaflet/dist/leaflet.css";

const ROUTE_ID = "65c5b2ac-fefc-49fb-a46e-fd2343aa7593";
const INTERVAL_MS = 200;

const customIcon = L.icon({
  iconUrl:
    "https://assets.streamlinehq.com/image/private/w_300,h_300,ar_1/f_auto/v1/icons/map-location/map-arrow-up-8k25gf4o22x7q90qr2e44b.png/map-arrow-up-qcn1lc5qxcspcmq9g6rsh.png?_a=DATAg1AAZAA0",
  iconSize: [32, 32],
  iconAnchor: [16, 16],
});

const startIcon = L.icon({
  iconUrl: "https://maps.gstatic.com/mapfiles/ms2/micons/flag.png",
  iconSize: [32, 32],
  iconAnchor: [16, 32],
});

async function loadRouteTimeline(routeId: string) {
  const { data } = await api.get<
    {
      latitude: number;
      longitude: number;
      capturedAt: string;
    }[]
  >(`/routes/${routeId}/timeline`);

  return data.map((p) => [p.latitude, p.longitude] as L.LatLngTuple);
}

export default function TrackingPage() {
  const startMarkerRef = useRef<L.Marker | null>(null);
  const [centerMap, setCenterMap] = useState(true);

  const lastPositionRef = useRef<GeolocationCoordinates | null>(null);
  const lastConfirmedRef = useRef<{
    latitude: number;
    longitude: number;
    heading?: number | null;
    speed?: number | null;
  } | null>(null);

  const mapRef = useRef<L.Map | null>(null);
  const markerRef = useRef<L.Marker | null>(null);
  const polylineRef = useRef<L.Polyline | null>(null);
  const pathRef = useRef<L.LatLngTuple[]>([]);

  // Flag para impedir inicialização dupla do mapa
  const mapInitializedRef = useRef(false);

  // =========================
  // Timeline (rota já percorrida)
  // =========================
  useEffect(() => {
    (async () => {
      const timeline = await loadRouteTimeline(ROUTE_ID);
      if (timeline.length === 0) return;

      const lastPoint = timeline[timeline.length - 1];

      if (mapInitializedRef.current) return;

      mapRef.current = L.map("map").setView(lastPoint, 16);

      L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
        attribution: "© OpenStreetMap",
      }).addTo(mapRef.current);

      markerRef.current = L.marker(lastPoint, {
        icon: customIcon,
      }).addTo(mapRef.current);

      pathRef.current = [...timeline];

      // Marker de início da rota (primeiro ponto)
      const startPoint = timeline[0];

      startMarkerRef.current = L.marker(startPoint, {
        icon: startIcon,
      }).addTo(mapRef.current);

      polylineRef.current = L.polyline(pathRef.current, {
        weight: 4,
        color: "#2563eb",
      }).addTo(mapRef.current);

      mapInitializedRef.current = true;
    })();
  }, []);

  // =========================
  // GPS cru
  // =========================
  useEffect(() => {
    if (!navigator.geolocation) {
      alert("Geolocalização não suportada");
      return;
    }

    const watchId = navigator.geolocation.watchPosition(
      (position) => {
        lastPositionRef.current = position.coords;
      },
      (error) => {
        if (error.code === error.TIMEOUT) {
          console.warn("GPS demorou, aguardando novo fix...");
          window.location.reload();
          return;
        }
        console.error("Erro GPS crítico", error);
      },
      {
        enableHighAccuracy: true,
        maximumAge: 10_000,
        timeout: 30_000,
      }
    );

    return () => navigator.geolocation.clearWatch(watchId);
  }, []);

  // =========================
  // Envio periódico
  // =========================
  useEffect(() => {
    const interval = setInterval(async () => {
      const coords = lastPositionRef.current;
      if (!coords) return;

      const payload: VehiclePositionArgs = {
        latitude: coords.latitude,
        longitude: coords.longitude,
        speed: coords.speed ? coords.speed * 3.6 : 0,
        heading: coords.heading ?? 0,
      };

      try {
        await api.post(`/route-tracking/routes/${ROUTE_ID}/positions`, payload);

        lastConfirmedRef.current = {
          latitude: coords.latitude,
          longitude: coords.longitude,
          heading: coords.heading,
          speed: coords.speed,
        };
      } catch (err) {
        console.error("Falha ao enviar posição", err);
      }
    }, INTERVAL_MS);

    return () => clearInterval(interval);
  }, []);

  // =========================
  // Mapa + marker + rota (apenas posições confirmadas)
  // =========================
  useEffect(() => {
    const interval = setInterval(() => {
      const confirmed = lastConfirmedRef.current;
      if (!confirmed) return;

      const latLng: L.LatLngTuple = [confirmed.latitude, confirmed.longitude];

      // Inicializa mapa via GPS caso não exista timeline
      if (!mapRef.current && !mapInitializedRef.current) {
        mapRef.current = L.map("map").setView(latLng, 16);

        L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
          attribution: "© OpenStreetMap",
        }).addTo(mapRef.current);

        markerRef.current = L.marker(latLng, {
          icon: customIcon,
        }).addTo(mapRef.current);

        // Marker de início da rota
        startMarkerRef.current = L.marker(latLng, {
          icon: startIcon,
        }).addTo(mapRef.current);

        pathRef.current = [latLng];

        polylineRef.current = L.polyline(pathRef.current, {
          weight: 4,
          color: "#2563eb",
        }).addTo(mapRef.current);

        mapInitializedRef.current = true;
        return;
      }

      // Atualiza marker
      markerRef.current?.setLatLng(latLng);

      // Evita duplicar ponto
      const last = pathRef.current[pathRef.current.length - 1];
      if (!last || last[0] !== latLng[0] || last[1] !== latLng[1]) {
        pathRef.current.push(latLng);
        polylineRef.current?.setLatLngs(pathRef.current);
      }

      // Rotação por heading
      if (
        confirmed.heading != null &&
        confirmed.speed != null &&
        confirmed.speed > 1 &&
        markerRef.current
      ) {
        const el = markerRef.current.getElement();
        if (el) {
          el.style.transformOrigin = "center";
          el.style.transform = `rotate(${confirmed.heading}deg)`;
        }
      }

      if (centerMap) {
        mapRef.current?.setView(latLng);
      }
    }, 500);

    return () => clearInterval(interval);
  }, [centerMap]);

  return (
    <div style={{ padding: 20 }}>
      <h1>Route Tracker</h1>

      <label style={{ display: "block", marginTop: 10 }}>
        <input
          type="checkbox"
          checked={centerMap}
          onChange={(e) => setCenterMap(e.target.checked)}
          style={{ marginRight: 6 }}
        />
        Centralizar no meu local
      </label>

      <div
        id="map"
        style={{
          height: 400,
          marginTop: 20,
          borderRadius: 8,
          overflow: "hidden",
        }}
      />
    </div>
  );
}
