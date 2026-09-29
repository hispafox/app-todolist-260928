import { expect, test } from "@playwright/test";

type TareaPrueba = {
  id: number;
  titulo: string;
  estado: "Pendiente" | "Completada";
  prioridad: "Baja" | "Media" | "Alta";
  responsableId: number | null;
  responsable: string | null;
};

test("crea, completa y filtra una tarea", async ({ page }) => {
  const tareas: TareaPrueba[] = [];

  await page.route("**/api/usuarios", (route) =>
    route.fulfill({ json: [{ id: 1, nombre: "Alex" }] }),
  );

  await page.route("**/api/tareas**", async (route) => {
    const solicitud = route.request();
    const ruta = new URL(solicitud.url()).pathname;

    if (solicitud.method() === "GET") {
      await route.fulfill({ json: tareas });
      return;
    }

    if (solicitud.method() === "POST") {
      const datos = solicitud.postDataJSON() as Omit<TareaPrueba, "id" | "estado" | "responsable">;
      const tarea: TareaPrueba = {
        ...datos,
        id: 1,
        estado: "Pendiente",
        responsable: datos.responsableId === 1 ? "Alex" : null,
      };
      tareas.push(tarea);
      await route.fulfill({ status: 201, json: tarea });
      return;
    }

    if (solicitud.method() === "PUT" && ruta.endsWith("/estado")) {
      const id = Number(ruta.split("/")[3]);
      const datos = solicitud.postDataJSON() as { estado: TareaPrueba["estado"] };
      const tarea = tareas.find((elemento) => elemento.id === id);
      if (tarea) tarea.estado = datos.estado;
      await route.fulfill({ json: tarea });
      return;
    }

    await route.fulfill({ status: 404, json: { title: "No encontrado" } });
  });

  await page.goto("/");
  await expect(page.getByText("Tu lista empieza aquí")).toBeVisible();
  await page.getByLabel("¿Qué tienes que hacer?").fill("Preparar demo");
  await page.getByLabel("Prioridad").selectOption("Alta");
  await page.getByLabel("Responsable").selectOption("1");
  await page.getByRole("button", { name: "Añadir tarea" }).click();

  await expect(page.getByText("Preparar demo")).toBeVisible();
  await expect(page.locator(".priority-label")).toHaveText("Alta");
  await expect(page.locator(".assignee-label")).toHaveText("Alex");

  await page.getByRole("button", { name: "Completar Preparar demo" }).click();
  await expect(page.getByRole("button", { name: "Reabrir Preparar demo" })).toBeVisible();
  await page.getByRole("button", { name: "Completadas" }).click();
  await expect(page.getByText("Preparar demo")).toBeVisible();
  await page.getByRole("button", { name: "Pendientes" }).click();
  await expect(page.getByText("Preparar demo")).not.toBeVisible();
});