<script lang="ts">
  import type { AuthUser } from "../api/authApi";
  import { onMount } from "svelte";

  import {
  getHomeAssistantConfig,
  saveHomeAssistantConfig,
  testHomeAssistantConfig
} from "../api/homeAssistantConfigApi";

  let {
    user,
    onBack
  } = $props<{
    user: AuthUser;
    onBack: () => void;
  }>();

  let homeAssistantUrl = $state("");
  let homeAssistantToken = $state("");
  let isSaving = $state(false);
  let statusMessage = $state("");
  let statusType = $state<"success" | "error" | "">("");

  async function save() {
    statusMessage = "";
    statusType = "";
    isSaving = true;

    const success = await saveHomeAssistantConfig(
      homeAssistantUrl,
      homeAssistantToken
    );

    isSaving = false;

    if (success) {
      statusType = "success";
      statusMessage = "Configuration saved";
      homeAssistantToken = "";
    } else {
      statusType = "error";
      statusMessage = "Failed to save configuration";
    }
  }

  async function testConnection() {
  statusMessage = "";
  statusType = "";

  const success = await testHomeAssistantConfig(
    homeAssistantUrl,
    homeAssistantToken
  );

  if (success) {
    statusType = "success";
    statusMessage = "Home Assistant connection successful";
  } else {
    statusType = "error";
    statusMessage = "Home Assistant connection failed";
  }
}

  onMount(async () => {
    const config = await getHomeAssistantConfig();

    if (!config) return;

    homeAssistantUrl = config.baseUrl;
  });
</script>

<main class="profile-page">
  <section class="profile-card">
    <button class="back-button" onclick={onBack}>
      ← Back to Dashboard
    </button>

    <div class="profile-header">
      <div class="avatar-large">
        {user.displayName.charAt(0).toUpperCase()}
      </div>

      <div>
        <p class="eyebrow">USER PROFILE</p>
        <h1>{user.displayName}</h1>
        <p class="email">{user.email}</p>
      </div>
    </div>

    <div class="settings-section">
      <div>
        <p class="eyebrow">HOME ASSISTANT</p>
        <h2>Connection Settings</h2>
        <p class="section-description">
          Connect your personal Home Assistant instance to load and control your smart devices.
        </p>
      </div>

      <label>
        Home Assistant URL
        <input
          placeholder="http://192.168.31.8:8123"
          bind:value={homeAssistantUrl}
          autocomplete="off"
        />
      </label>

      <label>
        Home Assistant Token
        <input
          placeholder="Long-Lived Access Token"
          type="password"
          bind:value={homeAssistantToken}
          autocomplete="off"
        />
      </label>

      {#if statusMessage}
        <div class:success={statusType === "success"} class:error={statusType === "error"} class="status-message">
          {statusType === "success" ? "✅" : "❌"} {statusMessage}
        </div>
      {/if}

      <div class="actions">
        <button class="secondary-button" onclick={testConnection}>
          Test Connection
        </button>

        <button onclick={save} disabled={isSaving}>
          {isSaving ? "Saving..." : "Save Configuration"}
        </button>
      </div>
    </div>
  </section>
</main>

<style>
  .profile-page {
    padding: 32px;
    max-width: 960px;
    margin: 0 auto;
  }

  .profile-card {
    display: grid;
    gap: 24px;
    padding: 28px;
    border-radius: 32px;
    background:
      linear-gradient(145deg, rgba(255,255,255,0.13), rgba(255,255,255,0.04));
    border: 1px solid rgba(255,255,255,0.18);
    box-shadow: 0 24px 80px rgba(0,0,0,0.35);
    backdrop-filter: blur(10px);
  }

  .profile-header {
    display: flex;
    align-items: center;
    gap: 20px;
    padding: 22px;
    border-radius: 26px;
    background: rgba(3, 7, 18, 0.45);
    border: 1px solid rgba(255,255,255,0.1);
  }

  .avatar-large {
    width: 76px;
    height: 76px;
    border-radius: 24px;
    display: grid;
    place-items: center;
    font-size: 34px;
    font-weight: 900;
    color: #020617;
    background: linear-gradient(135deg, #67e8f9, #a78bfa);
    box-shadow: 0 0 30px rgba(103, 232, 249, 0.22);
    flex-shrink: 0;
  }

  .settings-section {
    display: grid;
    gap: 16px;
    padding: 22px;
    border-radius: 26px;
    background: rgba(3, 7, 18, 0.36);
    border: 1px solid rgba(103, 232, 249, 0.14);
  }

  .eyebrow {
    color: #67e8f9;
    letter-spacing: 0.18em;
    font-size: 12px;
    font-weight: 700;
    margin: 0 0 6px;
  }

  h1 {
    margin: 0;
    font-size: clamp(36px, 7vw, 64px);
    line-height: 0.95;
  }

  h2 {
    margin: 0;
    font-size: 28px;
  }

  .email,
  .section-description {
    color: #aab4cf;
    margin: 0;
  }

  label {
    display: grid;
    gap: 8px;
    color: #aab4cf;
    font-weight: 700;
  }

  input {
    padding: 14px;
    border-radius: 16px;
    border: 1px solid rgba(255,255,255,0.18);
    background: rgba(3, 7, 18, 0.65);
    color: white;
    outline: none;
  }

  input:focus {
    border-color: rgba(103, 232, 249, 0.7);
    box-shadow: 0 0 0 3px rgba(103, 232, 249, 0.12);
  }

  .status-message {
    padding: 12px 14px;
    border-radius: 16px;
    font-weight: 800;
  }

  .status-message.success {
    color: #bbf7d0;
    background: rgba(34, 197, 94, 0.12);
    border: 1px solid rgba(34, 197, 94, 0.25);
  }

  .status-message.error {
    color: #fecaca;
    background: rgba(239, 68, 68, 0.12);
    border: 1px solid rgba(239, 68, 68, 0.25);
  }

  .actions {
    display: flex;
    gap: 12px;
    flex-wrap: wrap;
    justify-content: flex-end;
    margin-top: 8px;
  }

  button {
    border: none;
    border-radius: 999px;
    padding: 12px 18px;
    font-weight: 800;
    cursor: pointer;
    color: #020617;
    background: linear-gradient(135deg, #67e8f9, #a78bfa);
  }

  button:disabled {
    opacity: 0.55;
    cursor: not-allowed;
  }

  .secondary-button,
  .back-button {
    color: white;
    background: rgba(255,255,255,0.08);
    border: 1px solid rgba(255,255,255,0.16);
  }

  .back-button {
    justify-self: start;
  }

  @media (max-width: 720px) {
    .profile-page {
      padding: 20px;
    }

    .profile-header {
      flex-direction: column;
      align-items: flex-start;
    }

    .actions {
      flex-direction: column;
    }

    .actions button {
      width: 100%;
    }
  }
</style>