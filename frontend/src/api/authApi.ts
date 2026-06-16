import { apiBaseUrl } from "./config";

export type AuthUser = {
  id: number;
  displayName: string;
  email: string;
};

export type AuthResponse = {
  token: string;
  user: AuthUser;
};

export async function login(
  email: string,
  password: string
): Promise<AuthResponse | null> {
  const res = await fetch(`${apiBaseUrl}/auth/login`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({ email, password })
  });

  if (!res.ok) return null;

  return await res.json();
}

export async function register(
  displayName: string,
  email: string,
  password: string
): Promise<AuthResponse | null> {
  const res = await fetch(`${apiBaseUrl}/auth/register`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({ displayName, email, password })
  });

  if (!res.ok) return null;

  return await res.json();
}