// import { useEffect, useRef, useState } from "react";
// import { api } from "./api";
// import type { VehiclePositionArgs } from "./args";

import { Outlet } from "react-router-dom";

// import L from "leaflet";
// import "leaflet/dist/leaflet.css";
// import { Outlet } from "react-router-dom";

// const ROUTE_ID = "65c5b2ac-fefc-49fb-a46e-fd2343aa7593";
// const INTERVAL_MS = 2_000;

// const customIcon = L.icon({
//   iconUrl:
//     "https://assets.streamlinehq.com/image/private/w_300,h_300,ar_1/f_auto/v1/icons/map-location/map-arrow-up-8k25gf4o22x7q90qr2e44b.png/map-arrow-up-qcn1lc5qxcspcmq9g6rsh.png?_a=DATAg1AAZAA0",
//   iconSize: [32, 32],
//   iconAnchor: [16, 16],
// });

export default function App() {
  // // Flag de UX: mapa acompanha ou não o usuário
  // const [centerMap, setCenterMap] = useState<boolean>(true);

  // // =========================
  // // GPS cru (vem do dispositivo)
  // // =========================
  // const lastPositionRef = useRef<GeolocationCoordinates | null>(null);

  // // =========================
  // // GPS confirmado (só após sucesso na API)
  // // Fonte da verdade para o mapa e rota
  // // =========================
  // const lastConfirmedRef = useRef<{
  //   latitude: number;
  //   longitude: number;
  //   heading?: number | null;
  //   speed?: number | null;
  // } | null>(null);

  // // =========================
  // // CONTROLES DO MAPA
  // // =========================
  // const mapRef = useRef<L.Map | null>(null);
  // const markerRef = useRef<L.Marker | null>(null);

  // // Linha da rota percorrida (confirmada pela API)
  // const pathRef = useRef<L.LatLngTuple[]>([]);
  // const polylineRef = useRef<L.Polyline | null>(null);

  // // Captura GPS
  // useEffect(() => {
  //   if (!navigator.geolocation) {
  //     alert("Geolocalização não suportada");
  //     return;
  //   }

  //   const watchId = navigator.geolocation.watchPosition(
  //     (position) => {
  //       lastPositionRef.current = position.coords;
  //     },
  //     (error) => {
  //       if (error.code === error.TIMEOUT) {
  //         console.warn("GPS demorou, aguardando novo fix...");
  //         // reload para tentar recuperar o GPS
  //         window.location.reload();
  //         return;
  //       }
  //       console.error("Erro GPS crítico", error);
  //     },
  //     {
  //       enableHighAccuracy: true,
  //       maximumAge: 10_000,
  //       timeout: 30_000,
  //     }
  //   );

  //   return () => navigator.geolocation.clearWatch(watchId);
  // }, []);

  // // Envio periódico
  // useEffect(() => {
  //   const interval = setInterval(async () => {
  //     const coords = lastPositionRef.current;
  //     if (!coords) return;

  //     const payload: VehiclePositionArgs = {
  //       latitude: coords.latitude,
  //       longitude: coords.longitude,
  //       speed: coords.speed ? coords.speed * 3.6 : 0,
  //       heading: coords.heading ?? 0,
  //     };

  //     try {
  //       await api.post(`/route-tracking/routes/${ROUTE_ID}/positions`, payload);

  //       lastConfirmedRef.current = {
  //         latitude: coords.latitude,
  //         longitude: coords.longitude,
  //         heading: coords.heading,
  //         speed: coords.speed,
  //       };

  //       console.log("Posição enviada", payload);
  //     } catch (err) {
  //       console.error("Falha ao enviar posição", err);
  //     }
  //   }, INTERVAL_MS);

  //   return () => clearInterval(interval);
  // }, []);

  // // =========================
  // // 🗺️ Observador de GPS
  // // MAPA + MARKER + ROTA
  // // CONSOME APENAS POSIÇÕES CONFIRMADAS
  // // =========================
  // useEffect(() => {
  //   const interval = setInterval(() => {
  //     const confirmed = lastConfirmedRef.current;
  //     if (!confirmed) return;

  //     const latLng: [number, number] = [
  //       confirmed.latitude,
  //       confirmed.longitude,
  //     ];

  //     // Inicializa o mapa na primeira vez
  //     if (!mapRef.current) {
  //       mapRef.current = L.map("map").setView(latLng, 16);

  //       L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
  //         attribution: "© OpenStreetMap",
  //       }).addTo(mapRef.current);

  //       markerRef.current = L.marker(latLng, {
  //         icon: customIcon,
  //       }).addTo(mapRef.current);

  //       // Primeiro ponto da rota confirmada
  //       pathRef.current.push(latLng);
  //       polylineRef.current = L.polyline(pathRef.current, {
  //         weight: 4,
  //       }).addTo(mapRef.current);
  //       return;
  //     }

  //     // Atualiza a posição do marker
  //     markerRef.current?.setLatLng(latLng);

  //     // Atualiza a rota somente com pontos confirmados
  //     pathRef.current.push(latLng);
  //     polylineRef.current?.setLatLngs(pathRef.current);

  //     // Rotação do marker pelo heading (se fizer sentido)
  //     if (
  //       confirmed.heading != null &&
  //       confirmed.speed != null &&
  //       confirmed.speed > 1 && // evita girar parado
  //       markerRef.current
  //     ) {
  //       const el = markerRef.current.getElement();
  //       if (el) {
  //         el.style.transformOrigin = "center";
  //         el.style.transform = `rotate(${confirmed.heading}deg)`;
  //       }
  //     }

  //     // Centraliza o mapa se a flag estiver ativa
  //     if (centerMap) {
  //       mapRef.current.setView(latLng);
  //     }
  //   }, 500);

  //   return () => clearInterval(interval);
  // }, [centerMap]);

  return <Outlet />;
}
