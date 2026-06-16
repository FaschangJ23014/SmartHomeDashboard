<script lang="ts">
  import type { HomeAssistantEntity, Room } from "../types";

  let {
    rooms,
    availableHaEntities,
    newDeviceName = $bindable(),
    newDeviceRoom = $bindable(),
    newDeviceIntegrationType = $bindable(),
    selectedHaEntityId = $bindable(),
    onAddDevice
  } = $props<{
    rooms: Room[];
    availableHaEntities: HomeAssistantEntity[];
    newDeviceName: string;
    newDeviceRoom: number;
    newDeviceIntegrationType: string;
    selectedHaEntityId: string;
    onAddDevice: () => void;
  }>();

  let canAddDevice = $derived(
    rooms.length > 0 &&
    newDeviceName.trim() !== "" &&
    !(
      newDeviceIntegrationType === "HomeAssistant" &&
      availableHaEntities.length === 0
    )
  );
</script>

<section class="add-card">
  <div>
    <p class="section-label">New Device</p>
    <h2>Add Smart Device</h2>
  </div>

  <div class="add-form">
    <input placeholder="Device name" bind:value={newDeviceName} />

    <select bind:value={newDeviceRoom} disabled={rooms.length === 0}>
      {#if rooms.length === 0}
        <option value={0}>Create a room first</option>
      {:else}
        {#each rooms as room}
          <option value={room.id}>{room.name}</option>
        {/each}
      {/if}
    </select>

    <select bind:value={newDeviceIntegrationType}>
      <option value="Simulation">Simulation</option>
      <option value="HomeAssistant">Home Assistant</option>
    </select>

    {#if newDeviceIntegrationType === "HomeAssistant"}
      <select bind:value={selectedHaEntityId}>
        {#each availableHaEntities as entity}
          <option value={entity.entityId}>
            {entity.name} ({entity.entityId})
          </option>
        {/each}
      </select>
    {/if}

    <button onclick={onAddDevice} disabled={!canAddDevice}>
      Add Device
    </button>
  </div>
</section>

<style>
  .add-card {
    display: flex;
    justify-content: space-between;
    align-items: end;
    gap: 20px;
    margin-bottom: 28px;
    padding: 22px;
    border-radius: 28px;
    background:
      linear-gradient(
        135deg,
        rgba(103, 232, 249, 0.16),
        rgba(167, 139, 250, 0.12)
      );
    border: 1px solid rgba(103, 232, 249, 0.25);
    box-shadow:
      0 0 40px rgba(103, 232, 249, 0.08),
      0 0 80px rgba(167, 139, 250, 0.08);
    backdrop-filter: blur(10px);
  }

  .section-label {
    color: #67e8f9;
    letter-spacing: 0.18em;
    font-size: 12px;
    font-weight: 700;
    text-transform: uppercase;
    margin: 0 0 8px;
  }

  h2 {
    margin: 0;
    font-size: 28px;
  }

  .add-form {
    display: flex;
    gap: 12px;
    flex-wrap: wrap;
    justify-content: flex-end;
  }

  input,
  select {
    min-width: 180px;
    padding: 12px 14px;
    border-radius: 16px;
    border: 1px solid rgba(255,255,255,0.18);
    background: rgba(3, 7, 18, 0.65);
    color: white;
    font-size: 15px;
    outline: none;
  }

  input::placeholder {
    color: #7c86a6;
  }

  input:focus,
  select:focus {
    border-color: rgba(103, 232, 249, 0.7);
    box-shadow: 0 0 0 3px rgba(103, 232, 249, 0.12);
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

  button:disabled,
  select:disabled {
    opacity: 0.45;
    cursor: not-allowed;
  }

  @media (max-width: 720px) {
    .add-card {
      flex-direction: column;
      align-items: stretch;
    }

    .add-form {
      flex-direction: column;
    }

    input,
    select,
    button {
      width: 100%;
      box-sizing: border-box;
    }
  }
</style>