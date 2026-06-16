<script lang="ts">
  import { onMount } from "svelte";

  import Hero from "./components/Hero.svelte";
  import FilterCard from "./components/FilterCard.svelte";
  import Footer from "./components/Footer.svelte";
  import RoomCard from "./components/RoomCard.svelte";
  import AddRoomCard from "./components/AddRoomCard.svelte";
  import AddDeviceCard from "./components/AddDeviceCard.svelte";
  import UserBar from "./components/UserBar.svelte";
  import ProfilePage from "./components/ProfilePage.svelte";

  //Login
  import LoginCard from "./components/LoginCard.svelte";
  import { getUser, clearAuth } from "./api/authStorage";
  import type { AuthUser } from "./api/authApi";

  import type { Room, Device, HomeAssistantEntity, Weather } from "./types";



  

  import { getRooms, createRoom, removeRoom } from "./api/roomsApi";
  import {getDevices, createDevice, toggleDeviceById, removeDevice} from "./api/deviceApi";
  import { getHomeAssistantEntities } from "./api/homeAssistantApi";
  import { getWeatherByCurrentLocation } from "./api/weatherApi";




  let currentView = $state<"dashboard" | "profile">("dashboard");

  let currentUser = $state<AuthUser | null>(getUser());
  let weather = $state<Weather | null>(null);

  let devices = $state<Device[]>([]);
  let dbRooms = $state<Room[]>([]);
  let haEntities = $state<HomeAssistantEntity[]>([]);

  let newDeviceName = $state("");
  let newDeviceRoom = $state(0);
  let newRoomName = $state("");

  let selectedHaEntityId = $state("");
  let newDeviceIntegrationType = $state("Simulation");

  let roomSearch = $state("");
  let deviceSearch = $state("");

  const MAX_ROOM_NAME_LENGTH = 40;

  let canAddRoom = $derived(newRoomName.trim().length > 0 && newRoomName.trim().length <= MAX_ROOM_NAME_LENGTH);

  let availableHaEntities = $derived(
    haEntities.filter(
      (entity) =>
        !devices.some(
          (device) =>
            device.integrationType === "HomeAssistant" &&
            device.externalId === entity.entityId
        )
    )
  );

  let filteredRooms = $derived(
    dbRooms.filter((room) => {
      const matchesRoom = room.name.toLowerCase().includes(roomSearch.toLowerCase().trim());

      const matchesDevice =
        deviceSearch.trim() === "" || getDevicesForRoom(room.id).some((device) => device.name.toLowerCase().includes(deviceSearch.toLowerCase().trim()));

      return matchesRoom && matchesDevice;
    })
  );



async function loadWeather() {
  weather = await getWeatherByCurrentLocation();
}

async function loadRooms() {
  dbRooms = await getRooms();

  if (dbRooms.length > 0 && newDeviceRoom === 0) {
    newDeviceRoom = dbRooms[0].id;
  }
}

async function loadDevices() {
  devices = await getDevices();
}

async function loadHomeAssistantEntities() {
  haEntities = await getHomeAssistantEntities();

  if (availableHaEntities.length > 0 && selectedHaEntityId === "") {
    selectedHaEntityId = availableHaEntities[0].entityId;
  }
}

async function addRoom() {
  if (!canAddRoom) return;

  const success = await createRoom(newRoomName.trim());
  if (!success) return;

  newRoomName = "";
  await loadRooms();
}

async function deleteRoom(roomId: number) {
  const success = await removeRoom(roomId);
  if (!success) return;

  await loadRooms();
  await loadDevices();
}

async function addDevice() {
  if (newDeviceName.trim() === "") return;
  if (newDeviceRoom === 0) return;

  const isHomeAssistant = newDeviceIntegrationType === "HomeAssistant";

  if (isHomeAssistant && selectedHaEntityId === "") return;

  const success = await createDevice({
    name: newDeviceName.trim(),
    roomId: newDeviceRoom,
    type: "Light",
    isOn: false,
    integrationType: newDeviceIntegrationType,
    externalId: isHomeAssistant ? selectedHaEntityId : null
  });

  if (!success) return;

  newDeviceName = "";
  await loadDevices();
}

async function toggleDevice(id: number) {
  const success = await toggleDeviceById(id);
  if (!success) return;

  await loadDevices();
}

async function deleteDevice(id: number) {
  const success = await removeDevice(id);
  if (!success) return;

  devices = devices.filter((device) => device.id !== id);
}




  function getDevicesForRoom(roomId: number) {
    return devices.filter((device) => device.roomId === roomId);
  }

  function getActiveDevicesForRoom(roomId: number) {
    return devices.filter((device) => device.roomId === roomId && device.isOn);
  }


  //Login-Logik
  function handleLogin(user: AuthUser) {
  currentUser = user;
  loadDashboardData();
}

function logout() {
  clearAuth();
  currentUser = null;
  currentView = "dashboard";
  devices = [];
  dbRooms = [];
  haEntities = [];
}

async function loadDashboardData() {
  await loadRooms();
  await loadDevices();
  await loadHomeAssistantEntities();
  await loadWeather();
}

  onMount(async () => {
  if (currentUser) {
    await loadDashboardData();
  }
});
</script>





{#if !currentUser}
  <LoginCard onLogin={handleLogin} />

{:else if currentView === "profile"}

  <ProfilePage
    user={currentUser}
    onBack={() => (currentView = "dashboard")}
  />

{:else}

  <main class="app">
    <UserBar
      user={currentUser}
      onLogout={logout}
      onProfileClick={() => (currentView = "profile")}
    />

    <Hero {weather} deviceCount={devices.length} />

    <FilterCard bind:roomSearch bind:deviceSearch />

    <AddDeviceCard
      rooms={dbRooms}
      {availableHaEntities}
      bind:newDeviceName
      bind:newDeviceRoom
      bind:newDeviceIntegrationType
      bind:selectedHaEntityId
      onAddDevice={addDevice}
    />

    <AddRoomCard
      bind:newRoomName
      {canAddRoom}
      onAddRoom={addRoom}
    />

    <section class="rooms-grid">
      {#each filteredRooms as room}
        <RoomCard
          {room}
          devices={getDevicesForRoom(room.id)}
          activeDeviceCount={getActiveDevicesForRoom(room.id).length}
          onDeleteRoom={deleteRoom}
          onToggleDevice={toggleDevice}
          onDeleteDevice={deleteDevice}
        />
      {/each}
    </section>
  </main>

  <Footer />

{/if}









<style>
  :global(body) {
    margin: 0;
    min-height: 100vh;
    background:
      radial-gradient(circle at top left, rgba(0, 255, 255, 0.14), transparent 35%),
      radial-gradient(circle at top right, rgba(160, 32, 240, 0.16), transparent 35%),
      #070a12;
    color: white;
    font-family: Inter, system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif;

  }

:global(body) {
  animation: none;
}
  @keyframes gradientMove {
    0% {
      background-position: 0% 50%;
    }

    50% {
      background-position: 100% 50%;
    }

    100% {
      background-position: 0% 50%;
    }
  }

  .app {
    padding: 32px;
    max-width: 1200px;
    margin: 0 auto;
  }

  .rooms-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(330px, 1fr));
    gap: 22px;
  }

  @media (max-width: 720px) {
    .app {
      padding: 20px;
    }

    .rooms-grid {
      grid-template-columns: 1fr;
    }
  }

 :global(h1),
:global(h2),
:global(h3) {
  color: white;
}
</style>