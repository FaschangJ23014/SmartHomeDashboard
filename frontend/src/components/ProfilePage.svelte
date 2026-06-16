<script lang="ts">
  import type { AuthUser } from "../api/authApi";

  let {
    user,
    onBack
  } = $props<{
    user: AuthUser;
    onBack: () => void;
  }>();

  let homeAssistantUrl = $state("");
  let homeAssistantToken = $state("");
</script>

<main class="profile-page">
  <section class="profile-card">
    <button class="back-button" onclick={onBack}>
      ← Back to Dashboard
    </button>

    <p class="eyebrow">USER PROFILE</p>
    <h1>{user.displayName}</h1>
    <p class="email">{user.email}</p>

    <div class="divider"></div>

    <p class="eyebrow">HOME ASSISTANT</p>
    <h2>Connection Settings</h2>

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

    <div class="actions">
      <button class="secondary-button">
        Test Connection
      </button>

      <button>
        Save Configuration
      </button>
    </div>
  </section>
</main>

<style>
  .profile-page {
    padding: 32px;
    max-width: 900px;
    margin: 0 auto;
  }

  .profile-card {
    display: grid;
    gap: 16px;
    padding: 28px;
    border-radius: 28px;
    background: linear-gradient(145deg, rgba(255,255,255,0.12), rgba(255,255,255,0.04));
    border: 1px solid rgba(255,255,255,0.18);
    box-shadow: 0 24px 80px rgba(0,0,0,0.35);
    backdrop-filter: blur(10px);
  }

  .eyebrow {
    color: #67e8f9;
    letter-spacing: 0.18em;
    font-size: 12px;
    font-weight: 700;
    margin: 0;
  }

  h1 {
    margin: 0;
    font-size: clamp(38px, 7vw, 72px);
  }

  h2 {
    margin: 0;
    font-size: 28px;
  }

  .email {
    color: #aab4cf;
    margin: 0;
  }

  .divider {
    height: 1px;
    background: rgba(255,255,255,0.12);
    margin: 10px 0;
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

  .secondary-button,
  .back-button {
    color: white;
    background: rgba(255,255,255,0.08);
    border: 1px solid rgba(255,255,255,0.16);
  }

  .back-button {
    justify-self: start;
  }
</style>