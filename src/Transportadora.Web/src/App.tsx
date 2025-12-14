import { useEffect, useRef, useState } from "react";
import { api } from "./api";
import type { VehiclePositionArgs } from "./args";

import L from "leaflet";
import "leaflet/dist/leaflet.css";

const ROUTE_ID = "314f93f3-1000-4c06-a392-a158d20ba309";
const INTERVAL_MS = 2_000;

export default function App() {
  const [centerMap, setCenterMap] = useState<boolean>(true);

  const lastPositionRef = useRef<GeolocationCoordinates | null>(null);

  const mapRef = useRef<L.Map | null>(null);
  const markerRef = useRef<L.Marker | null>(null);

  // Captura GPS
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
          // reload para tentar recuperar o GPS
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

  // Envio periódico
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
        console.log("Posição enviada", payload);
      } catch (err) {
        console.error("Falha ao enviar posição", err);
      }
    }, INTERVAL_MS);

    return () => clearInterval(interval);
  }, []);

  // 🗺️ Observador de GPS → atualiza mapa
  useEffect(() => {
    const interval = setInterval(() => {
      const coords = lastPositionRef.current;
      if (!coords) return;

      const latLng: [number, number] = [coords.latitude, coords.longitude];

      if (!mapRef.current) {
        mapRef.current = L.map("map").setView(latLng, 16);

        L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
          attribution: "© OpenStreetMap",
        }).addTo(mapRef.current);

        markerRef.current = L.marker(latLng).addTo(mapRef.current);
        return;
      }
      markerRef.current?.setLatLng(latLng);

      if (centerMap) {
        mapRef.current.setView(latLng);
      }
    }, 500);

    return () => clearInterval(interval);
  }, [centerMap]);

  return (
    <div style={{ padding: 20 }}>
      <h1>Route Tracker</h1>
      <p>Enviando localização a cada {INTERVAL_MS / 1000} segundos...</p>

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
