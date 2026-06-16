<script lang="ts">
  import type { AuthUser } from "../api/authApi";

  let {
    user,
    onLogout,
    onProfileClick
  } = $props<{
    user: AuthUser;
    onLogout: () => void;
    onProfileClick: () => void;
  }>();
</script>

<div class="user-bar">
  <button class="profile-button" onclick={onProfileClick}>
    <span class="avatar">{user.displayName.charAt(0).toUpperCase()}</span>
    <span>{user.displayName}</span>
  </button>

  <button class="logout-button" onclick={onLogout}>
    Logout
  </button>
</div>

<style>
  .user-bar {
    display: flex;
    justify-content: flex-end;
    align-items: center;
    gap: 12px;
    margin-bottom: 20px;
  }

  .profile-button,
  .logout-button {
    border: 1px solid rgba(255,255,255,0.16);
    border-radius: 999px;
    padding: 10px 14px;
    font-weight: 800;
    cursor: pointer;
    color: white;
    background: rgba(3, 7, 18, 0.55);
    backdrop-filter: blur(10px);
  }

  .profile-button {
    display: flex;
    align-items: center;
    gap: 10px;
  }

  .avatar {
    width: 28px;
    height: 28px;
    border-radius: 999px;
    display: grid;
    place-items: center;
    color: #020617;
    background: linear-gradient(135deg, #67e8f9, #a78bfa);
  }

  .logout-button {
    color: #fecaca;
    border-color: rgba(239, 68, 68, 0.35);
  }
</style>