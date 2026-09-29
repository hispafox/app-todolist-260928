# Lista de tareas

> **Control documental**
> Código de proyecto: `app-todolist-260928`
> Fecha de actualización: `2026-09-29`
> Versión: `10`

> **Versión visible de la interfaz:** la app lee la versión del paquete `frontend/package.json` y la muestra en el pie de pantalla con etiqueta accesible. Consulta el pie de la aplicación o el archivo `frontend/package.json` para comprobar la versión publicada.

Aplicación web de lista de tareas planteada como proyecto práctico para aprender a desarrollar de forma iterativa con GitHub Copilot y Visual Studio Code.

> **Estado:** aplicación MVP implementada. El backend ASP.NET Core y la interfaz React están conectados; las pruebas automatizadas y la compilación frontend están verificadas. Falta la prueba manual guiada y la configuración/ejecución de SonarQube.

## Objetivo

Practicar cómo convertir una necesidad en requisitos verificables, planificar antes de programar, implementar cambios pequeños con ayuda de Copilot y comprobar el resultado con pruebas y análisis de calidad.

## Convención lingüística

Todas las explicaciones, la documentación, los comentarios y el código del proyecto se escribirán en castellano. Los nombres propios de tecnologías, bibliotecas, comandos, palabras reservadas y contratos externos conservarán su forma oficial cuando sea necesario para que el software funcione correctamente.

## Funcionalidad implementada

El MVP permitirá:

- Crear, consultar, editar y eliminar tareas.
- Marcar tareas como completadas y reabrirlas.
- Filtrar la lista por todas, pendientes o completadas.
- Asignar tareas a usuarios de ejemplo precargados.
- Establecer prioridad baja, media o alta.
- Establecer fechas de inicio y fin de calendario, sin hora. La fecha de fin requiere una fecha de inicio y no puede ser anterior a ella.
- Conservar los cambios en una base de datos SQLite entre sesiones.

La asignación indica quién es responsable de una tarea, pero el MVP no incluye cuentas, autenticación, autorización, privacidad por usuario, colaboración en tiempo real ni despliegue multiusuario.

## Tecnologías

- **Backend:** ASP.NET Core 10.
- **Frontend:** React con Vite.
- **Acceso a datos:** Entity Framework Core.
- **Base de datos:** SQLite.
- **Análisis de calidad:** SonarQube con reglas locales básicas; versión y configuración concreta pendientes.
- **Pruebas:** xUnit en backend; Vitest y React Testing Library para componentes frontend.
- **Pruebas E2E:** Playwright Test con Chromium para flujos de interfaz; las respuestas de la API se simulan.

Las tareas pueden quedar sin responsable y tienen prioridad media inicialmente. Las fechas de inicio y fin son opcionales: se permite no informar ninguna o informar solo el inicio; si se informa el fin, también se requiere el inicio y este debe ser anterior o igual al fin. No se añade una hora ni se asignan fechas a tareas preexistentes. El MVP no incluye descripción ni fecha de vencimiento independiente de estas fechas. Los estados y prioridades se guardan como texto en SQLite; la aplicación crea y actualiza el esquema mediante migraciones de EF Core. Los usuarios precargados son Alex, Sam y Taylor.

## Calidad y verificación

La solución incluye pruebas xUnit del servicio y la persistencia, pruebas Vitest de componentes y un flujo E2E con Playwright Test. El flujo E2E simula la API: valida la interacción de la interfaz, no el backend. Requiere Chromium instalado e iniciar manualmente Vite; Playwright no inicia servidores. La prueba manual guiada y el análisis de SonarQube quedan pendientes.

## Inicio y comprobaciones

Requisitos probados: .NET SDK 10.0.401, Node.js 22.20.0 y npm 10.9.3.

```powershell
dotnet restore AppTodoList.sln
dotnet test AppTodoList.sln
npm --prefix frontend install
npm --prefix frontend test
npm --prefix frontend run build
npx --prefix frontend playwright install chromium
npm --prefix frontend run test:e2e
```

Para probar la aplicación, inicia manualmente cada parte desde la raíz del repositorio:

```powershell
dotnet run --project src/AppTodoList.Api --launch-profile https
npm --prefix frontend run dev
```

La API queda en `https://localhost:5001` y Vite en `http://localhost:5173`. SQLite se crea en `App_Data/tareas.db` relativa al directorio de contenido del API.

## Trabajo con GitHub Copilot

El proyecto se desarrollará en incrementos pequeños. Antes de aceptar una propuesta de código, se revisará el plan y el diff; cada comportamiento se comprobará mediante sus pruebas o la prueba manual correspondiente.

El workspace incluye cuatro agentes locales para resolver issues: **Orquestador**, **Planificador**, **Desarrollador** y **Verificador**. Selecciona Orquestador en GitHub Copilot Chat para coordinar el ciclo de planificación, implementación y verificación. Sus perfiles y límites están descritos en [`docs/plan-agentes.md`](docs/plan-agentes.md) y sus definiciones viven en `.github/agents/`.

El agente **Jefe de proyecto** prioriza el trabajo pendiente y mantiene el cuadro de mando [`docs/cuadro-mando.html`](docs/cuadro-mando.html), que se abre directamente en el navegador.

## Plan inicial

1. Estructura, herramientas y decisiones funcionales acordadas.
2. Modelo, migración inicial, datos de ejemplo y persistencia SQLite implementados.
3. API CRUD y filtros implementados con validaciones y pruebas.
4. Interfaz React conectada a la API, con pruebas Vitest.
5. Aplicación implementada. Pendiente: prueba manual guiada y análisis de SonarQube.

## Documentación

- [Análisis del MVP](docs/analisis.md): requisitos funcionales, historias de usuario, criterios de aceptación y decisiones técnicas restantes.
- [Plan del proyecto](docs/plan-proyecto.md): fases, tareas, entregables y verificaciones propuestas.
- [Arquitectura y modelo de datos](docs/arquitectura.md): diagramas Mermaid de arquitectura y ERD, responsabilidades y decisiones técnicas restantes.
- [Manual de usuario](docs/manual-usuario.md): instrucciones de los flujos implementados y límites del MVP.
- [Guía de desarrollo e instalación](docs/guia-desarrollo.md): requisitos del entorno, convenciones locales, pruebas y configuración pendiente.
