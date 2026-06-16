import { apiBaseUrl } from "./config";
import type { Device } from "../types";
import { getAuthHeaders, getJsonAuthHeaders } from "./apiClient";

export type CreateDeviceRequest = {
  name: string;
  roomId: number;
  type: string;
  isOn: boolean;
  integrationType: string;
  externalId: string | null;
};

export async function getDevices(): Promise<Device[]> {
  const res = await fetch(`${apiBaseUrl}/devices`, {
  headers: getAuthHeaders()
});
  if (!res.ok) return [];

  return await res.json();
}

export async function createDevice(device: CreateDeviceRequest): Promise<boolean> {
  const res = await fetch(`${apiBaseUrl}/devices`, {
    method: "POST",
    headers: getJsonAuthHeaders(),
    body: JSON.stringify(device)
  });

  return res.ok;
}

export async function toggleDeviceById(id: number): Promise<boolean> {
  const res = await fetch(`${apiBaseUrl}/devices/${id}/toggle`, {
  method: "POST",
  headers: getAuthHeaders()
});

  return res.ok;
}

export async function removeDevice(id: number): Promise<boolean> {
  const res = await fetch(`${apiBaseUrl}/devices/${id}`, {
  method: "DELETE",
  headers: getAuthHeaders()
});

  return res.ok;
}