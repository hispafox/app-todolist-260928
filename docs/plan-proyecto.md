# Plan del proyecto: aplicación de lista de tareas

> **Control documental**
> Código de proyecto: `app-todolist-260928`
> Fecha de actualización: `2026-09-28`
> Versión: `1`

## Estado y propósito

**Estado:** plan propuesto; el proyecto todavía está en fase de análisis.

Este plan traduce el [análisis del MVP](analisis.md) en fases de trabajo pequeñas y verificables. No fija fechas ni selecciona herramientas que siguen pendientes de decisión.

## Resultado esperado

Una aplicación web en la que se puedan crear y gestionar tareas, asignarlas a usuarios de ejemplo y conservarlas localmente. El MVP se desarrollará con ASP.NET Core 10, React, Entity Framework Core y SQLite, e incluirá pruebas automatizadas, una prueba manual guiada y un análisis básico con SonarQube.

La asignación de tareas no implica autenticación ni colaboración real entre cuentas. El inicio de sesión, la autorización, la privacidad por usuario y la colaboración en tiempo real quedan fuera del alcance.

## Fases

### Fase 0: Cerrar decisiones y preparar el trabajo

**Objetivo:** resolver las decisiones abiertas en el análisis antes de fijar los detalles de implementación.

**Tareas:**

- Decidir si la asignación de responsable es obligatoria o puede quedar vacía.
- Definir la prioridad inicial de una tarea nueva.
- Confirmar los campos de tarea del MVP; descripción y fecha de vencimiento no están incluidas por ahora.
- Elegir la herramienta de construcción de React y los frameworks de pruebas para backend y frontend.
- Acordar cómo se ejecutará SonarQube y qué reglas básicas se revisarán.
- Confirmar la estructura general, separando interfaz, servicios, lógica de negocio y modelo de datos.

**Entregables:** decisiones registradas y estructura inicial acordada.

**Verificación:** requisitos, historias de usuario y criterios de aceptación no se contradicen; cada historia del MVP tiene una forma de probarse.

### Fase 1: Crear la estructura y configurar la calidad

**Objetivo:** establecer una base de desarrollo mínima para frontend, backend y pruebas.

**Tareas:**

- Crear la solución y los proyectos de ASP.NET Core 10 y React.
- Configurar Entity Framework Core con SQLite.
- Añadir proyectos o configuraciones de pruebas para las capas seleccionadas.
- Definir una configuración inicial de SonarQube conforme a las decisiones de la fase 0.
- Documentar los requisitos de herramientas y los pasos para ejecutar y probar cada parte una vez que la estructura exista.

**Entregables:** estructura compilable, configuración de base de datos y pruebas, y documentación básica de desarrollo.

**Verificación:** las partes iniciales compilan y la configuración de pruebas y análisis puede ejecutarse. No se inicia la aplicación como parte de este plan.

### Fase 2: Modelar tareas y usuarios de ejemplo

**Objetivo:** definir y persistir los datos mínimos del dominio.

**Tareas:**

- Modelar tarea, estado, prioridad y usuario de ejemplo.
- Configurar sus relaciones y reglas en Entity Framework Core.
- Añadir datos iniciales de usuarios de ejemplo.
- Crear la base SQLite y el mecanismo de actualización del esquema que se acuerde para el proyecto.

**Entregables:** modelo de datos y persistencia local configurados.

**Verificación:** pruebas comprueban que se pueden guardar y recuperar tareas y usuarios; las decisiones de campos y asignación se cumplen.

### Fase 3: Implementar la primera funcionalidad vertical

**Objetivo:** completar un flujo pequeño desde la API hasta SQLite.

**Tareas:**

- Implementar la creación y consulta/listado de tareas.
- Separar el manejo HTTP de los servicios y reglas de negocio.
- Añadir pruebas automatizadas del flujo y sus resultados persistidos.

**Entregables:** API capaz de crear y listar tareas.

**Verificación:** las pruebas demuestran que una tarea guardada aparece al volver a consultarla y que el comportamiento acordado para prioridad y responsable se respeta.

### Fase 4: Completar las operaciones y reglas de tareas

**Objetivo:** cubrir el resto de los comportamientos del MVP en cambios pequeños.

**Orden sugerido:**

1. Editar una tarea.
2. Marcarla como completada y reabrirla.
3. Eliminarla.
4. Asignar y cambiar el usuario responsable.
5. Establecer y cambiar la prioridad.
6. Filtrar por todas, pendientes o completadas.

**Entregables:** API y lógica de negocio completas para las historias HU-03 a HU-08.

**Verificación:** cada operación incluye pruebas automatizadas antes de avanzar; los cambios y eliminaciones se reflejan en las consultas posteriores y sobreviven a un reinicio cuando corresponda.

### Fase 5: Construir la interfaz React

**Objetivo:** permitir completar los flujos desde la pantalla de usuario.

**Tareas:**

- Crear la pantalla inicial de listado, incluyendo estados vacío y con tareas.
- Añadir formularios y controles para crear, editar, eliminar, asignar responsable y seleccionar prioridad.
- Añadir el cambio de estado y los filtros.
- Conectar las interacciones con la API y mostrar resultados y errores de forma comprensible.
- Añadir pruebas del comportamiento de interfaz según el framework acordado.

**Entregables:** interfaz conectada a datos persistidos, sin datos simulados como fuente de la aplicación.

**Verificación:** las interacciones principales producen el resultado esperado y las pruebas del frontend pasan.

### Fase 6: Validar el MVP y cerrar la entrega

**Objetivo:** comprobar el sistema integrado frente a los criterios de aceptación.

**Tareas:**

- Ejecutar las pruebas automatizadas de backend y frontend.
- Seguir la prueba manual guiada para crear, editar, asignar, priorizar, completar, filtrar y eliminar tareas.
- Confirmar que los datos persisten al reiniciar la aplicación.
- Ejecutar SonarQube, revisar hallazgos pertinentes y corregir los que correspondan al alcance.
- Actualizar el README y la documentación con el estado real de implementación y las instrucciones confirmadas.

**Entregables:** MVP verificado y documentación coherente con el código.

**Criterios de salida:** historias del MVP verificadas, pruebas relevantes aprobadas, prueba manual completada y hallazgos de calidad revisados.

## Forma de trabajo con GitHub Copilot

Para cada fase o historia:

1. Proporcionar a Copilot el requisito y los criterios de aceptación relacionados.
2. Pedir un plan breve antes de generar o modificar código.
3. Revisar la propuesta y limitar el cambio a un incremento.
4. Añadir o actualizar las pruebas de la funcionalidad en el mismo incremento.
5. Revisar el diff y ejecutar la comprobación enfocada antes de continuar.

Copilot apoya el trabajo, pero las decisiones de alcance, la revisión del código y la aceptación de los resultados corresponden al equipo.

## Riesgos y controles

- **Alcance creciente:** autenticación y colaboración real están fuera del MVP; mantenerlas como evolución separada.
- **Decisiones técnicas pendientes:** cerrar las herramientas de frontend, pruebas y SonarQube en la fase 0 para evitar configuraciones incompatibles.
- **Confundir asignación con identidad:** los usuarios precargados son datos de referencia, no cuentas autenticadas.
- **Persistencia incompleta:** verificar con pruebas que los cambios están en SQLite y no dependen solo del estado de la interfaz.
- **Confiar únicamente en análisis estático:** SonarQube no reemplaza pruebas automatizadas ni validación manual.

## Documentos relacionados

- [README del proyecto](../README.md)
- [Análisis del MVP, requisitos e historias de usuario](analisis.md)
