import { apiBaseUrl } from "./config";
import type { Room } from "../types";

export async function getRooms(): Promise<Room[]> {
  const res = await fetch(`${apiBaseUrl}/rooms`);
  if (!res.ok) return [];

  return await res.json();
}

export async function createRoom(name: string): Promise<boolean> {
  const res = await fetch(`${apiBaseUrl}/rooms`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({ name })
  });

  return res.ok;
}

export async function removeRoom(roomId: number): Promise<boolean> {
  const res = await fetch(`${apiBaseUrl}/rooms/${roomId}`, {
    method: "DELETE"
  });

  return res.ok;
}