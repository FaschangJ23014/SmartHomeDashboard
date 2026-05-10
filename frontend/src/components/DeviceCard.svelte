<script lang="ts">
  import type { Device } from "../types";

  let {
    device,
    onToggle,
    onDelete
  } = $props<{
    device: Device;
    onToggle: (id: number) => void;
    onDelete: (id: number) => void;
  }>();
</script>



<div class:active={device.isOn} class="device-card">
  <div class="device-info">
    <p class="device-type">{device.type}</p>
    <h3>{device.name}</h3>
  </div>

  <div class="device-actions">
    <button on:click={() => onToggle(device.id)}>
      {device.isOn ? "ON" : "OFF"}
    </button>

    <button class="delete-button" on:click={() => onDelete(device.id)}>
      Delete
    </button>
  </div>
</div>





<style>
  .device-card {
    display: flex;
    justify-content: space-between;
    align-items: center;
    gap: 14px;
    padding: 16px;
    border-radius: 20px;
    background: rgba(3, 7, 18, 0.65);
    border: 1px solid rgba(255, 255, 255, 0.1);
    transition: 0.2s ease;
  }

  .device-card.active {
    border-color: rgba(34, 211, 238, 0.5);
    box-shadow: 0 0 24px rgba(34, 211, 238, 0.16);
  }

  .device-info {
    min-width: 0;
  }

  .device-info h3 {
    max-width: 150px;
    word-break: break-word;
    margin: 0;
    font-size: 16px;
  }

  .device-type {
    margin: 0 0 4px;
    color: #818cf8;
    font-size: 12px;
    text-transform: uppercase;
    letter-spacing: 0.12em;
  }

  .device-actions {
    display: flex;
    gap: 10px;
    align-items: center;
    flex-shrink: 0;
  }

  button {
    border: none;
    border-radius: 999px;
    padding: 12px 18px;
    font-weight: 800;
    cursor: pointer;
    color: #020617;
    background: linear-gradient(135deg, #67e8f9, #a78bfa);
    box-shadow: 0 0 18px rgba(103, 232, 249, 0.25);
    transition: 0.2s ease;
  }

  button:hover:not(:disabled) {
    transform: translateY(-1px);
    box-shadow: 0 0 28px rgba(103, 232, 249, 0.35);
  }

  button:active {
    transform: scale(0.96);
  }

  .delete-button {
    color: white;
    background: rgba(239, 68, 68, 0.16);
    border: 1px solid rgba(239, 68, 68, 0.35);
    box-shadow: none;
  }

  .delete-button:hover {
    box-shadow: 0 0 24px rgba(239, 68, 68, 0.28);
  }

  .device-card:not(.active) button:not(.delete-button) {
    color: white;
    background: rgba(255, 255, 255, 0.08);
    box-shadow: none;
  }

  @media (max-width: 720px) {
    .device-card {
      flex-direction: column;
      align-items: stretch;
    }

    .device-actions {
      justify-content: space-between;
    }

    .device-info h3 {
      max-width: none;
    }
  }
</style>