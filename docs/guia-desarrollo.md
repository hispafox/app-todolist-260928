# Guía de desarrollo e instalación

> **Control documental**
> Código de proyecto: `app-todolist-260928`
> Fecha de actualización: `2026-09-28`
> Versión: `5`

## Estado de esta guía

**Aplicación MVP implementada.** La solución contiene el API, SQLite y la interfaz React. Los comandos de comprobación e inicio de esta guía corresponden a la estructura actual.

La API se ha compilado y probado; las pruebas frontend y el build de producción también pasan. No se han iniciado servidores automáticamente.

## 1. Tecnologías previstas

- Backend: ASP.NET Core 10.
- Frontend: React con Vite.
- Persistencia: SQLite mediante Entity Framework Core.
- Pruebas: xUnit en backend; Vitest y React Testing Library en frontend.
- Análisis de calidad: SonarQube con reglas locales básicas.

SonarQube aún no tiene versión ni configuración de ejecución definida. El archivo de bloqueo de npm fija las dependencias frontend instaladas.

## 2. Requisitos previos

Instala o prepara las siguientes herramientas en el equipo de desarrollo:

- Visual Studio Code.
- Git.
- .NET 10 SDK.
- Node.js 22.20.0 y npm 10.9.3 (versiones verificadas para este proyecto).
- SonarQube con reglas locales básicas cuando se definan su versión y forma de ejecución.

Puedes comprobar las herramientas de línea de comandos disponibles con:

```powershell
git --version
dotnet --list-sdks
node --version
npm --version
```

El frontend usa npm; sus dependencias quedan fijadas por `frontend/package-lock.json`.

## 3. Obtener y abrir el proyecto

Abre la carpeta raíz del repositorio en Visual Studio Code. La solución .NET está en `AppTodoList.sln`, el API en `src/AppTodoList.Api` y la aplicación React en `frontend`.

Comprueba que puedes ver el README y la carpeta `docs/` antes de ejecutar los comandos de esta guía.

## 4. Convenciones de desarrollo local

- Todas las explicaciones, la documentación, los comentarios y el código nuevo se escribirán en castellano. Solo se conservarán en su forma oficial los nombres de tecnologías, bibliotecas, comandos, palabras reservadas y contratos externos que lo requieran.
- El frontend debe servirse por HTTP en `http://localhost:5173`.
- La API ASP.NET Core debe servirse por HTTPS en `https://localhost:5001`, usando el certificado de desarrollo de .NET.
- No se deben añadir certificados autofirmados ni plugins SSL al frontend.
- El proxy de desarrollo de Vite hacia la API deberá usar `https://localhost:5001` y `secure: false` para aceptar el certificado de desarrollo local.
- La aplicación no incluye autenticación en el MVP. La asignación a usuarios de ejemplo no representa una sesión ni autorización.

Los puertos y el esquema HTTP/HTTPS son convenciones del entorno local; la configuración de despliegue queda fuera de esta guía inicial.

## 5. Restaurar, compilar y probar

Desde la raíz del repositorio, usa:

```powershell
dotnet restore AppTodoList.sln
dotnet build AppTodoList.sln
dotnet test AppTodoList.sln
npm --prefix frontend install
npm --prefix frontend test
npm --prefix frontend run build
```

Para iniciar localmente, ejecuta cada comando en una terminal separada. La API aplica sus migraciones al arrancar y no elimina la base de datos existente.

```powershell
dotnet run --project src/AppTodoList.Api --launch-profile https
npm --prefix frontend run dev
```

La API usa `https://localhost:5001`; Vite usa `http://localhost:5173` y su proxy acepta el certificado de desarrollo local de .NET. La base de datos se encuentra en `App_Data/tareas.db` bajo el directorio de contenido del API.

## 6. Base de datos SQLite

- Entity Framework Core gestiona el acceso a SQLite.
- La base de datos local está en `src/AppTodoList.Api/App_Data/tareas.db`.
- Los cambios de esquema se aplican mediante migraciones de EF Core, ejecutadas por la API al arrancar.
- Antes de un cambio de esquema, conserva una copia de la base de datos local si contiene datos que deban mantenerse.
- No borres el archivo SQLite para resolver errores de esquema. Investiga el cambio requerido y aplica una migración o una actualización de esquema compatible.
- La migración inicial precarga los usuarios de ejemplo Alex, Sam y Taylor de forma determinista.

## 7. Pruebas y SonarQube

Las pruebas automatizadas usan xUnit en backend y Vitest con React Testing Library en frontend. Están implementadas y sus comandos aparecen en la sección 5. La prueba manual guiada sigue pendiente.

SonarQube se ejecutará con reglas locales básicas. La versión, la configuración y el comando de análisis están pendientes; no se debe asumir una configuración local concreta. El análisis estático complementa las pruebas, pero no las sustituye.

## 8. Flujo recomendado con GitHub Copilot

1. Abre el requisito o la historia de usuario relacionada en `docs/analisis.md`.
2. Pide a Copilot un plan pequeño, redactado en castellano, y los archivos que propone modificar antes de solicitar la implementación.
3. Revisa el alcance del cambio y confirma que respeta las capas de interfaz, servicios, lógica de negocio y modelo.
4. Implementa una historia o parte comprobable cada vez, con sus pruebas y código en castellano según la convención del proyecto.
5. Revisa el diff y ejecuta la comprobación enfocada correspondiente antes de avanzar.
6. Actualiza la documentación si cambian los comandos, las decisiones técnicas o el comportamiento de usuario.

## 9. Decisiones necesarias para completar esta guía

- Definir la versión/configuración de SonarQube y ejecutar su análisis.
- Completar la prueba manual guiada y añadir cualquier ajuste derivado de esa comprobación.

## Documentación relacionada

- [README del proyecto](../README.md)
- [Análisis del MVP e historias de usuario](analisis.md)
- [Plan del proyecto](plan-proyecto.md)
- [Arquitectura y modelo de datos](arquitectura.md)
- [Manual de usuario](manual-usuario.md)
