export type EstadoTarea = "Pendiente" | "Completada";
export type PrioridadTarea = "Baja" | "Media" | "Alta";

export interface Tarea {
  id: number;
  titulo: string;
  estado: EstadoTarea;
  prioridad: PrioridadTarea;
  responsableId: number | null;
  responsable: string | null;
  fechaInicio: string | null;
  fechaFin: string | null;
}

export interface Usuario {
  id: number;
  nombre: string;
}

export type FiltroTareas = "Todas" | EstadoTarea;