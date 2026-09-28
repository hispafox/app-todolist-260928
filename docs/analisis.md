# Análisis del MVP: aplicación de lista de tareas

## 1. Propósito

Crear una aplicación web sencilla para practicar el desarrollo iterativo con GitHub Copilot en Visual Studio Code. El MVP permitirá gestionar tareas persistidas en una base de datos local, asignarlas a usuarios de ejemplo y comprobar el comportamiento mediante pruebas automatizadas y una prueba manual guiada.

## 2. Objetivos de aprendizaje

- Convertir una necesidad expresada en lenguaje natural en requisitos y criterios de aceptación.
- Usar Copilot para analizar y planificar antes de generar código.
- Implementar funcionalidades en incrementos pequeños y revisables.
- Incorporar pruebas y análisis de calidad durante el desarrollo.

## 3. Tecnologías acordadas

- Backend: ASP.NET Core 10.
- Frontend: React.
- Persistencia: SQLite mediante Entity Framework Core.
- Calidad: SonarQube con reglas locales básicas.

No se ha elegido todavía una herramienta concreta de construcción para React ni un framework de pruebas. Se seleccionarán al iniciar el proyecto, teniendo en cuenta las versiones compatibles y los objetivos del curso.

## 4. Alcance funcional del MVP

La aplicación tendrá una pantalla inicial con el listado de tareas y permitirá:

1. Crear una tarea.
2. Consultar las tareas existentes.
3. Editar una tarea.
4. Eliminar una tarea.
5. Marcar una tarea como completada y volver a dejarla pendiente.
6. Filtrar tareas por estado: todas, pendientes o completadas.
7. Asignar una tarea a un usuario elegido de una lista de usuarios de ejemplo.
8. Establecer prioridad baja, media o alta.

La persistencia será local mediante SQLite; las tareas no deben desaparecer al cerrar y volver a iniciar la aplicación.

## 5. Usuarios y límites de colaboración

Para este MVP habrá usuarios de ejemplo precargados. El usuario que crea o modifica tareas seleccionará una persona de esa lista para la asignación.

Quedan fuera de alcance el registro, el inicio de sesión, la autorización, la privacidad por usuario, la colaboración en tiempo real y el despliegue multiusuario. Por tanto, la asignación representa el dato de responsable de una tarea, no una identidad autenticada ni un espacio compartido con control de acceso.

## 6. Requisitos funcionales

- **RF-01: Crear tarea.** El sistema permitirá añadir una tarea con título, prioridad y usuario asignado.
- **RF-02: Listar tareas.** La pantalla inicial mostrará las tareas guardadas, su estado, prioridad y responsable.
- **RF-03: Editar tarea.** El usuario podrá modificar los datos de una tarea existente.
- **RF-04: Eliminar tarea.** El usuario podrá eliminar una tarea y esta dejará de aparecer en el listado.
- **RF-05: Cambiar estado.** El usuario podrá completar una tarea pendiente y reabrir una completada.
- **RF-06: Filtrar por estado.** El usuario podrá mostrar todas las tareas, solo las pendientes o solo las completadas.
- **RF-07: Asignar responsable.** El usuario podrá elegir un usuario de ejemplo como responsable de una tarea.
- **RF-08: Priorizar tarea.** Cada tarea tendrá una prioridad baja, media o alta.
- **RF-09: Persistir datos.** Los cambios se guardarán en SQLite y estarán disponibles al reiniciar la aplicación.

## 7. Historias de usuario

### HU-01: Crear una tarea

**Como** persona usuaria, **quiero** crear una tarea, **para** recordar algo que debo hacer.

- **Dado** que estoy en la pantalla de tareas, **cuando** introduzco los datos de una tarea y la guardo, **entonces** aparece en el listado.
- La tarea guardada incluye un título; la prioridad y la asignación se gestionan según HU-06 y HU-07.
- La tarea sigue disponible después de reiniciar la aplicación (HU-09).

Relacionada con: RF-01, RF-09.

### HU-02: Consultar las tareas

**Como** persona usuaria, **quiero** ver las tareas guardadas y sus datos principales, **para** saber qué está pendiente y quién es responsable.

- **Dado** que hay tareas guardadas, **cuando** abro la pantalla inicial, **entonces** veo cada tarea con su estado, prioridad y responsable cuando esté asignado.
- Si todavía no hay tareas, la pantalla muestra el listado vacío sin errores.

Relacionada con: RF-02.

### HU-03: Editar una tarea

**Como** persona usuaria, **quiero** modificar una tarea, **para** mantener su información actualizada.

- **Dado** que existe una tarea, **cuando** cambio sus datos y guardo, **entonces** el listado muestra los valores actualizados.
- Los cambios se conservan al volver a consultar la tarea.

Relacionada con: RF-03.

### HU-04: Completar o reabrir una tarea

**Como** persona usuaria, **quiero** cambiar el estado de una tarea, **para** reflejar mi progreso.

- **Dado** que una tarea está pendiente, **cuando** la marco como completada, **entonces** su estado cambia a completada.
- **Dado** que una tarea está completada, **cuando** la reabro, **entonces** vuelve al estado pendiente.

Relacionada con: RF-05.

### HU-05: Filtrar por estado

**Como** persona usuaria, **quiero** filtrar las tareas por estado, **para** concentrarme en las que necesito revisar.

- **Dado** que hay tareas en distintos estados, **cuando** elijo "todas", "pendientes" o "completadas", **entonces** solo se muestran las tareas correspondientes a ese filtro.
- Elegir "todas" vuelve a mostrar la lista completa.

Relacionada con: RF-06.

### HU-06: Asignar una persona responsable

**Como** persona usuaria, **quiero** asignar una tarea a una persona de la lista disponible, **para** indicar quién debe hacerse cargo.

- **Dado** que estoy creando o editando una tarea, **cuando** selecciono un usuario de ejemplo y guardo, **entonces** el usuario aparece como responsable de esa tarea.
- La selección procede únicamente de los usuarios de ejemplo disponibles.

Relacionada con: RF-07.

### HU-07: Establecer la prioridad

**Como** persona usuaria, **quiero** establecer la prioridad de una tarea, **para** distinguir su importancia relativa.

- **Dado** que estoy creando o editando una tarea, **cuando** elijo prioridad baja, media o alta y guardo, **entonces** el valor elegido se muestra en el listado.

Relacionada con: RF-08.

### HU-08: Eliminar una tarea

**Como** persona usuaria, **quiero** eliminar una tarea que ya no necesito, **para** mantener útil el listado.

- **Dado** que una tarea aparece en el listado, **cuando** la elimino, **entonces** deja de aparecer y ya no se recupera al reiniciar la aplicación.

Relacionada con: RF-04, RF-09.

### HU-09: Conservar las tareas

**Como** persona usuaria, **quiero** que mis cambios se conserven entre sesiones, **para** no perder el trabajo realizado.

- **Dado** que he creado o modificado tareas, **cuando** cierro y vuelvo a iniciar la aplicación, **entonces** los cambios guardados siguen disponibles.

Relacionada con: RF-09.

## 8. Requisitos de calidad y verificación

- Añadir pruebas automatizadas para las operaciones y reglas principales.
- Probar manualmente el flujo de creación, edición, asignación, cambio de prioridad, cambio de estado, filtrado y eliminación.
- Ejecutar SonarQube con las reglas básicas acordadas y revisar sus hallazgos; el análisis no sustituye a las pruebas.
- Mantener separadas las responsabilidades de interfaz, servicios, lógica de negocio y modelo de datos.
- No considerar validada una funcionalidad solo porque Copilot haya generado código: debe revisarse y comprobarse.

## 9. Criterios de aceptación

- Se puede crear una tarea y verla en el listado después de guardarla.
- Una tarea puede editarse y eliminarse.
- Una tarea puede pasar de pendiente a completada y volver a pendiente.
- Los filtros muestran únicamente las tareas que corresponden al estado seleccionado.
- Una tarea muestra la prioridad elegida y permite seleccionar un usuario de ejemplo como responsable.
- Los datos permanecen disponibles después de cerrar y reiniciar la aplicación.
- Las pruebas automatizadas relevantes pasan y la prueba manual guiada puede completarse.
- Se ejecuta el análisis básico de SonarQube y se documentan o corrigen los hallazgos pertinentes.

## 10. Incrementos propuestos

1. **Preparar el proyecto.** Acordar estructura, versiones y herramientas de pruebas; guardar este análisis como referencia.
2. **Modelar y persistir.** Definir las entidades de tarea y usuario de ejemplo, configurar Entity Framework Core y crear la base SQLite.
3. **Crear una primera funcionalidad vertical.** Implementar crear y listar tareas desde la API hasta la base de datos, con pruebas para el flujo.
4. **Completar la gestión básica.** Añadir edición, eliminación y cambio de estado, con pruebas correspondientes.
5. **Añadir filtros y asignación.** Incorporar filtros de estado, usuarios de ejemplo y prioridad baja/media/alta.
6. **Construir la interfaz React.** Conectar la pantalla inicial y sus controles con la API.
7. **Verificar calidad.** Ejecutar las pruebas automatizadas, seguir la prueba manual guiada y analizar el código con SonarQube.

En cada incremento, pedir a Copilot que explique su propuesta, revisar el diff y comprobar el comportamiento antes de continuar.

## 11. Decisiones pendientes

- ¿La asignación de responsable será obligatoria o podrá quedar vacía?
- ¿Cuál será la prioridad inicial de una tarea nueva?
- ¿Qué campos tendrá una tarea además del título, estado, prioridad y responsable? No se han solicitado descripción ni fecha de vencimiento.
- ¿Qué herramientas concretas se usarán para crear y probar la aplicación React?
- ¿Qué framework y estrategia de pruebas se emplearán en backend y frontend?
- ¿Qué versión y configuración de SonarQube estarán disponibles para el ejercicio?

Estas decisiones deben cerrarse antes de convertir el análisis en tareas de implementación detalladas.
