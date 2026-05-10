<script lang="ts">
  import type { Weather } from "../types";

  let {
    weather,
    deviceCount
  } = $props<{
    weather: Weather | null;
    deviceCount: number;
  }>();
</script>

<section class="hero">
  <div>
    <p class="eyebrow">LOCAL SMART HOME</p>
    <h1>SmartHome Dashboard</h1>
    <p class="subtitle">
      Local Smart Home Dashboard
    </p>
  </div>

  <div class="hero-right">
    {#if weather}
      <div class="weather-card">
        <p>Weather</p>
        <strong>{weather.temperature}°C</strong>
        <span>{weather.windspeed} km/h wind</span>
      </div>
    {/if}

    <div class="status-card">
      <span class="status-dot"></span>
      <div>
        <p>System online</p>
        <strong>{deviceCount} Devices</strong>
      </div>
    </div>
  </div>
</section>

<style>
  .hero {
    display: flex;
    justify-content: space-between;
    gap: 24px;
    align-items: center;
    margin-bottom: 28px;
  }

  .eyebrow {
    color: #67e8f9;
    letter-spacing: 0.18em;
    font-size: 12px;
    font-weight: 700;
    text-transform: uppercase;
    margin: 0 0 8px;
  }

  h1 {
    font-size: clamp(42px, 8vw, 84px);
    line-height: 0.9;
    margin: 0;
    text-shadow: 0 0 30px rgba(103, 232, 249, 0.35);
  }

  .subtitle {
    color: #aab4cf;
    font-size: 18px;
    margin-top: 16px;
  }

  .hero-right {
    display: flex;
    gap: 14px;
    align-items: center;
    flex-wrap: wrap;
    justify-content: flex-end;
  }

  .status-card,
  .weather-card {
    background:
      linear-gradient(145deg, rgba(255,255,255,0.12), rgba(255,255,255,0.04));
    border: 1px solid rgba(255,255,255,0.18);
    box-shadow:
      0 24px 80px rgba(0, 0, 0, 0.35),
      inset 0 1px 0 rgba(255, 255, 255, 0.08);
    backdrop-filter: blur(18px);
  }

  .status-card {
    min-width: 190px;
    display: flex;
    align-items: center;
    gap: 14px;
    padding: 18px;
    border-radius: 22px;
  }

  .status-card p,
  .weather-card p {
    margin: 0;
    color: #aab4cf;
  }

  .status-dot {
    width: 14px;
    height: 14px;
    border-radius: 999px;
    background: #22c55e;
    animation: pulse 2s infinite;
  }

  .weather-card {
    min-width: 150px;
    padding: 18px;
    border-radius: 22px;
  }

  .weather-card strong {
    display: block;
    font-size: 28px;
    margin-top: 4px;
  }

  .weather-card span {
    color: #67e8f9;
    font-size: 13px;
  }

  @keyframes pulse {
    0% {
      box-shadow: 0 0 0 0 rgba(34,197,94,0.7);
    }

    70% {
      box-shadow: 0 0 0 12px rgba(34,197,94,0);
    }

    100% {
      box-shadow: 0 0 0 0 rgba(34,197,94,0);
    }
  }

  @media (max-width: 720px) {
    .hero {
      flex-direction: column;
      align-items: stretch;
    }
  }
</style>