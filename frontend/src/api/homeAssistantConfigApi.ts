import { apiBaseUrl } from "./config";
import { getAuthHeaders, getJsonAuthHeaders } from "./apiClient";

export type HomeAssistantConfig = {
  baseUrl: string;
  hasToken: boolean;
};

export async function getHomeAssistantConfig(): Promise<HomeAssistantConfig | null> {
  const res = await fetch(`${apiBaseUrl}/homeassistantconfig`, {
    headers: getAuthHeaders()
  });

  if (!res.ok) {
    return null;
  }

  return await res.json();
}

export async function saveHomeAssistantConfig(
  baseUrl: string,
  token: string
): Promise<boolean> {
  const res = await fetch(`${apiBaseUrl}/homeassistantconfig`, {
    method: "POST",
    headers: getJsonAuthHeaders(),
    body: JSON.stringify({
      baseUrl,
      token
    })
  });

  return res.ok;
}

export async function testHomeAssistantConfig(
  baseUrl: string,
  token: string
): Promise<boolean> {
  const res = await fetch(`${apiBaseUrl}/homeassistantconfig/test`, {
    method: "POST",
    headers: getJsonAuthHeaders(),
    body: JSON.stringify({
      baseUrl,
      token
    })
  });

  return res.ok;
}