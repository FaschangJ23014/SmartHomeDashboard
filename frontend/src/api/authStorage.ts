import type { AuthUser } from "./authApi";

const TOKEN_KEY = "smartHomeToken";
const USER_KEY = "smartHomeUser";

export function saveAuth(token: string, user: AuthUser) {
  localStorage.setItem(TOKEN_KEY, token);
  localStorage.setItem(USER_KEY, JSON.stringify(user));
}

export function getToken() {
  return localStorage.getItem(TOKEN_KEY);
}

export function getUser(): AuthUser | null {
  const userJson = localStorage.getItem(USER_KEY);

  if (!userJson) return null;

  return JSON.parse(userJson);
}

export function clearAuth() {
  localStorage.removeItem(TOKEN_KEY);
  localStorage.removeItem(USER_KEY);
}