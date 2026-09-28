# Guía de desarrollo e instalación

> **Control documental**
> Código de proyecto: `app-todolist-260928`
> Fecha de actualización: `2026-09-28`
> Versión: `2`

## Estado de esta guía

**Borrador inicial.** El proyecto está todavía en fase de análisis y no se ha creado la estructura ejecutable. Las versiones de las herramientas, las rutas y los comandos de restauración, prueba e inicio deben completarse después de cerrar las decisiones de la fase 0 del [plan del proyecto](plan-proyecto.md).

Esta guía recoge las tecnologías y convenciones ya acordadas para que la configuración futura sea coherente. No se han ejecutado comandos ni iniciado aplicaciones.

## 1. Tecnologías previstas

- Backend: ASP.NET Core 10.
- Frontend: React.
- Persistencia: SQLite mediante Entity Framework Core.
- Análisis de calidad: SonarQube con reglas locales básicas.

Siguen pendientes la herramienta de construcción del frontend, el gestor de paquetes, los frameworks de pruebas y la configuración concreta de SonarQube.

## 2. Requisitos previos

Instala o prepara las siguientes herramientas en el equipo de desarrollo:

- Visual Studio Code.
- Git.
- .NET 10 SDK.
- Node.js y el gestor de paquetes compatible con la herramienta de React que se elija.
- Acceso a la instancia o configuración local de SonarQube que se acuerde para el curso.

Puedes comprobar las herramientas de línea de comandos disponibles con:

```powershell
git --version
dotnet --list-sdks
node --version
npm --version
```

`npm` solo será necesario si se selecciona como gestor de paquetes. La versión requerida de Node.js se concretará con la herramienta de React.

## 3. Obtener y abrir el proyecto

Cuando el repositorio tenga una URL remota definida, clónalo y abre la carpeta raíz del repositorio en Visual Studio Code. No se fija aquí una URL ni una estructura de carpetas porque todavía no se han creado los proyectos de frontend y backend.

Comprueba que puedes ver el README y la carpeta `docs/`. Las instrucciones definitivas indicarán las rutas de la solución .NET y del frontend una vez creado el scaffold.

## 4. Convenciones de desarrollo local

- Todas las explicaciones, la documentación, los comentarios y el código nuevo se escribirán en castellano. Solo se conservarán en su forma oficial los nombres de tecnologías, bibliotecas, comandos, palabras reservadas y contratos externos que lo requieran.
- El frontend debe servirse por HTTP en `http://localhost:5173`.
- La API ASP.NET Core debe servirse por HTTPS en `https://localhost:5001`, usando el certificado de desarrollo de .NET.
- No se deben añadir certificados autofirmados ni plugins SSL al frontend.
- Si se elige Vite, el proxy de desarrollo hacia la API deberá usar `https://localhost:5001` y `secure: false` para aceptar el certificado de desarrollo local. Si se elige otra herramienta, se configurará el proxy equivalente sin cambiar las URLs acordadas.
- La aplicación no incluye autenticación en el MVP. La asignación a usuarios de ejemplo no representa una sesión ni autorización.

Los puertos y el esquema HTTP/HTTPS son convenciones del entorno local; la configuración de despliegue queda fuera de esta guía inicial.

## 5. Restaurar, compilar y probar

Los comandos concretos dependen de las rutas y scripts que se creen durante el scaffold. Cuando estén definidos, esta sección deberá incluir los comandos exactos para cada proyecto.

Como referencia, la solución .NET deberá admitir restauración, compilación y pruebas con el SDK correspondiente. El frontend deberá instalar dependencias y ejecutar los scripts declarados por su `package.json`, si se elige un gestor compatible con npm. Los marcos de prueba y nombres de scripts todavía no están decididos.

No ejecutes comandos con rutas o scripts de ejemplo como si ya existieran. Completar esta sección es parte de la fase de preparación.

## 6. Base de datos SQLite

- Entity Framework Core gestionará el acceso a SQLite.
- La ubicación del archivo de base de datos se documentará cuando se configure el proyecto.
- Los cambios de esquema deben aplicarse con el mecanismo de migración que se acuerde para Entity Framework Core.
- Antes de un cambio de esquema, conserva una copia de la base de datos local si contiene datos que deban mantenerse.
- No borres el archivo SQLite para resolver errores de esquema. Investiga el cambio requerido y aplica una migración o una actualización de esquema compatible.
- Los usuarios de ejemplo se precargarán; el mecanismo exacto se implementará y documentará durante la fase de persistencia.

## 7. Pruebas y SonarQube

Cada funcionalidad deberá incorporar pruebas automatizadas. La selección de frameworks y los comandos exactos se añadirán una vez creada la solución. La prueba manual guiada se mantendrá en el manual de usuario o en un documento de pruebas cuando se defina el procedimiento.

SonarQube se ejecutará con las reglas básicas que se acuerden. La versión, la configuración, el acceso al servidor y el comando de análisis están pendientes; no se debe asumir una configuración local concreta. El análisis estático complementa las pruebas, pero no las sustituye.

## 8. Flujo recomendado con GitHub Copilot

1. Abre el requisito o la historia de usuario relacionada en `docs/analisis.md`.
2. Pide a Copilot un plan pequeño, redactado en castellano, y los archivos que propone modificar antes de solicitar la implementación.
3. Revisa el alcance del cambio y confirma que respeta las capas de interfaz, servicios, lógica de negocio y modelo.
4. Implementa una historia o parte comprobable cada vez, con sus pruebas y código en castellano según la convención del proyecto.
5. Revisa el diff y ejecuta la comprobación enfocada correspondiente antes de avanzar.
6. Actualiza la documentación si cambian los comandos, las decisiones técnicas o el comportamiento de usuario.

## 9. Decisiones necesarias para completar esta guía

- Estructura y nombres de los proyectos y de la solución.
- Herramienta de construcción React, gestor de paquetes y versión compatible de Node.js.
- Frameworks de pruebas y scripts para frontend y backend.
- Ruta del archivo SQLite y comandos de migración.
- Configuración y forma de ejecutar SonarQube.
- Comandos exactos para restaurar, compilar, probar y ejecutar cada parte.

## Documentación relacionada

- [README del proyecto](../README.md)
- [Análisis del MVP e historias de usuario](analisis.md)
- [Plan del proyecto](plan-proyecto.md)
- [Arquitectura y modelo de datos](arquitectura.md)
- [Manual de usuario](manual-usuario.md)
