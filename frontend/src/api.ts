import type { EstadoTarea, PrioridadTarea, Tarea, Usuario } from "./types";

interface SolicitudTarea {
  titulo: string;
  prioridad: PrioridadTarea;
  responsableId: number | null;
}

async function solicitar<T>(ruta: string, opciones?: RequestInit): Promise<T> {
  const respuesta = await fetch(ruta, {
    ...opciones,
    headers: {
      "Content-Type": "application/json",
      ...opciones?.headers,
    },
  });

  if (!respuesta.ok) {
    const detalle = (await respuesta.json().catch(() => null)) as {
      title?: string;
      errors?: Record<string, string[]>;
    } | null;
    const mensaje = detalle?.errors
      ? Object.values(detalle.errors).flat().join(" ")
      : detalle?.title;
    throw new Error(mensaje || "No se pudo completar la operación.");
  }

  if (respuesta.status === 204) {
    return undefined as T;
  }

  return (await respuesta.json()) as T;
}

export const api = {
  listarTareas: () => solicitar<Tarea[]>('/api/tareas'),
  listarUsuarios: () => solicitar<Usuario[]>('/api/usuarios'),
  crearTarea: (solicitud: SolicitudTarea) =>
    solicitar<Tarea>('/api/tareas', {
      method: "POST",
      body: JSON.stringify(solicitud),
    }),
  editarTarea: (id: number, solicitud: SolicitudTarea) =>
    solicitar<Tarea>(`/api/tareas/${id}`, {
      method: "PUT",
      body: JSON.stringify(solicitud),
    }),
  cambiarEstado: (id: number, estado: EstadoTarea) =>
    solicitar<Tarea>(`/api/tareas/${id}/estado`, {
      method: "PUT",
      body: JSON.stringify({ estado }),
    }),
  eliminarTarea: (id: number) =>
    solicitar<void>(`/api/tareas/${id}`, { method: "DELETE" }),
};