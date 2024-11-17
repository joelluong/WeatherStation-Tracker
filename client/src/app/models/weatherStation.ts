export interface VariableData {
  id: string;
  timestamp: string;
  value: number;
}

export interface Variable {
  id: number;
  name: string;
  unit: string;
  longName: string;
  data: VariableData[];
}

export interface weatherStation {
  id: number;
  name: string;
  site: string;
  portfolio: string;
  state: string;
  latitude: number;
  longitude: number;
  variables: Variable[];
}
