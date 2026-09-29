import { afterEach, describe, expect, it, vi } from "vitest";
import { cleanup, fireEvent, render, screen, waitFor } from "@testing-library/react";
import packageJson from "../package.json";
import App from "./App";

afterEach(() => {
  cleanup();
  vi.unstubAllGlobals();
});

describe("tablero de tareas", () => {
  it("muestra la versión de la aplicación en la interfaz y coincide con el paquete", async () => {
    vi.stubGlobal("fetch", vi.fn(async (entrada: RequestInfo | URL) => {
      const ruta = entrada.toString();
      return Response.json(ruta.includes("usuarios") ? [{ id: 1, nombre: "Alex" }] : []);
    }));

    render(<App />);

    await screen.findByText("Tu lista empieza aquí");

    const versionEsperada = packageJson.version.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
    expect(screen.getByText(new RegExp(`v?${versionEsperada}`))).toBeInTheDocument();
  });

  it("muestra un estado vacío cuando todavía no hay tareas", async () => {
    vi.stubGlobal("fetch", vi.fn(async (entrada: RequestInfo | URL) => {
      const ruta = entrada.toString();
      return Response.json(ruta.includes("usuarios") ? [{ id: 1, nombre: "Alex" }] : []);
    }));

    render(<App />);

    expect(await screen.findByText("Tu lista empieza aquí")).toBeInTheDocument();
    expect(screen.getByRole("option", { name: "Alex" })).toBeInTheDocument();
    expect(screen.getByLabelText("Responsable")).toBeInTheDocument();
  });

  it("crea una tarea con los valores del formulario y la muestra en la lista", async () => {
    let tareas: Array<Record<string, unknown>> = [];
    vi.stubGlobal("fetch", vi.fn(async (entrada: RequestInfo | URL, opciones?: RequestInit) => {
      const ruta = entrada.toString();
      if (ruta.includes("usuarios")) {
        return Response.json([{ id: 1, nombre: "Alex" }]);
      }
      if (opciones?.method === "POST") {
        const solicitud = JSON.parse(opciones.body as string) as Record<string, unknown>;
        const creada = {
          id: 1,
          titulo: solicitud.titulo,
          estado: "Pendiente",
          prioridad: solicitud.prioridad,
          responsableId: solicitud.responsableId,
          responsable: solicitud.responsableId === 1 ? "Alex" : null,
        };
        tareas = [...tareas, creada];
        return Response.json(creada, { status: 201 });
      }
      return Response.json(tareas);
    }));

    render(<App />);
    await screen.findByText("Tu lista empieza aquí");
    fireEvent.change(screen.getByLabelText("¿Qué tienes que hacer?"), { target: { value: "Preparar entrega" } });
    fireEvent.change(screen.getByLabelText("Prioridad"), { target: { value: "Alta" } });
    fireEvent.change(screen.getByLabelText("Responsable"), { target: { value: "1" } });
    fireEvent.click(screen.getByRole("button", { name: "Añadir tarea" }));

    expect(await screen.findByText("Preparar entrega")).toBeInTheDocument();
    expect(screen.getByText("Alta", { selector: ".priority-label" })).toBeInTheDocument();
    expect(screen.getByText("Alex", { selector: ".assignee-label" })).toBeInTheDocument();
    await waitFor(() => expect(screen.getByLabelText("¿Qué tienes que hacer?")).toHaveValue(""));
  });

  it("filtra la lista por estado", async () => {
    const tareas = [
      { id: 1, titulo: "Pendiente", estado: "Pendiente", prioridad: "Media", responsableId: null, responsable: null },
      { id: 2, titulo: "Terminada", estado: "Completada", prioridad: "Baja", responsableId: null, responsable: null },
    ];
    vi.stubGlobal("fetch", vi.fn(async (entrada: RequestInfo | URL) =>
      Response.json(entrada.toString().includes("usuarios") ? [] : tareas)));

    render(<App />);
    expect(await screen.findByText("Pendiente")).toBeInTheDocument();
    fireEvent.click(screen.getByRole("button", { name: "Completadas" }));

    expect(screen.getByText("Terminada")).toBeInTheDocument();
    expect(screen.queryByText("Pendiente")).not.toBeInTheDocument();
  });
});