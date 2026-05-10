import { apiBaseUrl } from "./config";
import type { HomeAssistantEntity } from "../types";

export async function getHomeAssistantEntities(): Promise<HomeAssistantEntity[]> {
  try {
    const res = await fetch(`${apiBaseUrl}/homeassistant/entities`);
    if (!res.ok) return [];

    return await res.json();
  } catch {
    return [];
  }
}