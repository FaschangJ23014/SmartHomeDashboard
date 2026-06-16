<script lang="ts">
  import { login, register } from "../api/authApi";
  import { saveAuth } from "../api/authStorage";
  import type { AuthUser } from "../api/authApi";

  let { onLogin } = $props<{
    onLogin: (user: AuthUser) => void;
  }>();

  let isRegister = $state(false);
  let displayName = $state("");
  let email = $state("");
  let password = $state("");
  let errorMessage = $state("");

  async function submitAuth() {
    errorMessage = "";

    const result = isRegister
      ? await register(displayName, email, password)
      : await login(email, password);

    if (!result) {
      errorMessage = "Login/Register failed.";
      return;
    }

    saveAuth(result.token, result.user);
    onLogin(result.user);
  }
</script>

<main class="login-page">
  <section class="login-card">
    <p class="eyebrow">SMART HOME LOGIN</p>
    <h1>{isRegister ? "Create Account" : "Welcome Back"}</h1>

    {#if isRegister}
      <input placeholder="Display name" bind:value={displayName} />
    {/if}

    <input placeholder="Email" bind:value={email} />
    <input placeholder="Password" type="password" bind:value={password} />

    {#if errorMessage}
      <p class="error">{errorMessage}</p>
    {/if}

    <button onclick={submitAuth}>
      {isRegister ? "Register" : "Login"}
    </button>

    <button class="switch-button" onclick={() => (isRegister = !isRegister)}>
      {isRegister ? "Already have an account?" : "Create new account"}
    </button>
  </section>
</main>

<style>
  .login-page {
    min-height: 100vh;
    display: grid;
    place-items: center;
    padding: 24px;
  }

  .login-card {
    width: min(420px, 100%);
    display: grid;
    gap: 14px;
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
    margin: 0 0 12px;
    font-size: 42px;
  }

  input {
    padding: 14px;
    border-radius: 16px;
    border: 1px solid rgba(255,255,255,0.18);
    background: rgba(3, 7, 18, 0.65);
    color: white;
    outline: none;
  }

  button {
    border: none;
    border-radius: 999px;
    padding: 13px 18px;
    font-weight: 800;
    cursor: pointer;
    background: linear-gradient(135deg, #67e8f9, #a78bfa);
    color: #020617;
  }

  .switch-button {
    background: transparent;
    color: #67e8f9;
  }

  .error {
    color: #f87171;
    margin: 0;
  }
</style>