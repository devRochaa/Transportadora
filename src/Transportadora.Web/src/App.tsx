import { useEffect, useRef } from "react";
import { api } from "./api";
import type { VehiclePositionArgs } from "./args";

const ROUTE_ID = "D710ED84-5D76-4ACF-A185-3495AB968FDF";
const INTERVAL_MS = 5_000;

export default function App() {
  const lastPositionRef = useRef<GeolocationCoordinates | null>(null);

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

  return (
    <div style={{ padding: 20 }}>
      <h1>Route Tracker</h1>
      <p>Enviando localização a cada {INTERVAL_MS / 1000} segundos...</p>
    </div>
  );
}
