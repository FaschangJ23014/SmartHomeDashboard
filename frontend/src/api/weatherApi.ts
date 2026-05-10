import type { Weather } from "../types";

export async function getWeatherByCurrentLocation(): Promise<Weather | null> {
  if (!navigator.geolocation) return null;

  return new Promise((resolve) => {
    navigator.geolocation.getCurrentPosition(
      async (position) => {
        const latitude = position.coords.latitude;
        const longitude = position.coords.longitude;

        const res = await fetch(
          `https://api.open-meteo.com/v1/forecast?latitude=${latitude}&longitude=${longitude}&current_weather=true`
        );

        if (!res.ok) {
          resolve(null);
          return;
        }

        const data = await res.json();

        resolve({
          temperature: data.current_weather.temperature,
          windspeed: data.current_weather.windspeed
        });
      },
      () => resolve(null)
    );
  });
}