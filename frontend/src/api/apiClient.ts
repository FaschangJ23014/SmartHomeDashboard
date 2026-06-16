import { getToken } from "./authStorage";

export function getAuthHeaders(): HeadersInit {
  const token = getToken();

  if (!token) {
    return {};
  }

  return {
    Authorization: `Bearer ${token}`
  };
}

export function getJsonAuthHeaders(): HeadersInit {
  return {
    "Content-Type": "application/json",
    ...getAuthHeaders()
  };
}