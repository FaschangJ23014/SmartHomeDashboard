<script lang="ts">
  import DeviceCard from "./DeviceCard.svelte";
  import type { Device, Room } from "../types";

  let {
    room,
    devices,
    activeDeviceCount,
    onDeleteRoom,
    onToggleDevice,
    onDeleteDevice
  } = $props<{
    room: Room;
    devices: Device[];
    activeDeviceCount: number;
    onDeleteRoom: (roomId: number) => void;
    onToggleDevice: (deviceId: number) => void;
    onDeleteDevice: (deviceId: number) => void;
  }>();
</script>




<article class="room-card">
  <div class="room-header">
    <div>
      <p class="room-label">Room</p>
      <h2>{room.name}</h2>
    </div>

    <div class="room-actions">
      <div class="room-badge">
        {activeDeviceCount}/{devices.length} on
      </div>

      <button class="delete-button small-delete" on:click={() => onDeleteRoom(room.id)}>
        Delete
      </button>
    </div>
  </div>

  <div class="devices">
    {#if devices.length === 0}
      <p class="empty-state">Derzeit keine Geräte im Raum verfügbar</p>
    {:else}
      {#each devices as device (device.id)}
        <DeviceCard
          {device}
          onToggle={onToggleDevice}
          onDelete={onDeleteDevice}
        />
      {/each}
    {/if}
  </div>
</article>




<style>
  .room-card {
    padding: 22px;
    border-radius: 28px;
    overflow: hidden;
    background:
      linear-gradient(145deg, rgba(255,255,255,0.12), rgba(255,255,255,0.04));
    border: 1px solid rgba(255,255,255,0.18);
    box-shadow:
      0 24px 80px rgba(0, 0, 0, 0.35),
      inset 0 1px 0 rgba(255, 255, 255, 0.08);
    backdrop-filter: blur(10px);
    transition:
      transform 0.25s ease,
      box-shadow 0.25s ease;
  }

  .room-card:hover {
    transform: translateY(-4px) scale(1.01);
  }

  .room-header {
    display: grid;
    grid-template-columns: minmax(0, 1fr) auto;
    align-items: start;
    gap: 16px;
    margin-bottom: 18px;
  }

  .room-header > div:first-child {
    min-width: 0;
  }

  .room-header h2 {
    max-width: 100%;
    white-space: normal;
    overflow-wrap: normal;
    word-break: normal;
    hyphens: none;
    margin: 0;
    font-size: 28px;
  }

  .room-actions {
    display: flex;
    align-items: flex-start;
    gap: 10px;
    flex-shrink: 0;
  }

  .room-label {
    margin: 0 0 6px;
    color: #67e8f9;
    text-transform: uppercase;
    font-size: 11px;
    letter-spacing: 0.16em;
  }

  .room-badge {
    padding: 8px 12px;
    border-radius: 999px;
    color: #67e8f9;
    background: rgba(103, 232, 249, 0.1);
    border: 1px solid rgba(103, 232, 249, 0.25);
    font-size: 13px;
    white-space: nowrap;
    transform: translateY(-5px);
  }

  .devices {
    display: grid;
    gap: 12px;
  }

  .empty-state {
    margin: 0;
    padding: 16px;
    border-radius: 18px;
    color: #aab4cf;
    background: rgba(3, 7, 18, 0.42);
    border: 1px dashed rgba(255, 255, 255, 0.14);
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

  .delete-button {
    color: white;
    background: rgba(239, 68, 68, 0.16);
    border: 1px solid rgba(239, 68, 68, 0.35);
    box-shadow: none;
  }

  .delete-button:hover {
    box-shadow: 0 0 24px rgba(239, 68, 68, 0.28);
  }

  .small-delete {
    padding: 8px 12px;
    font-size: 13px;
  }

  @media (max-width: 720px) {
    .room-header {
      grid-template-columns: 1fr;
    }

    .room-actions {
      justify-content: space-between;
    }
  }
</style>