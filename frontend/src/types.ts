export type Room = {
  id: number;
  name: string;
};

export type Device = {
  id: number;
  name: string;
  type: string;
  isOn: boolean;
  roomId: number;
  room?: Room;
  integrationType: string;
  externalId?: string | null;
};

export type HomeAssistantEntity = {
  entityId: string;
  name: string;
  state: string;
};

export type Weather = {
  temperature: number;
  windspeed: number;
};