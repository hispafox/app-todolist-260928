# Manual de usuario: lista de tareas

## Estado de esta guía

**Borrador inicial.** Describe el uso previsto según el análisis del MVP; la aplicación todavía no está implementada. Los nombres de controles, el diseño de pantallas y los pasos exactos de inicio se completarán cuando exista la interfaz y se confirmen las herramientas del proyecto.

## 1. Acerca de la aplicación

La aplicación permitirá gestionar una lista de tareas desde una interfaz web. Podrás crear y modificar tareas, cambiar su estado, filtrarlas, establecer una prioridad y asignarlas a usuarios de ejemplo. Los datos se guardarán localmente en SQLite y deberán conservarse al reiniciar la aplicación.

## 2. Límites del MVP

- No hay registro ni inicio de sesión.
- Los usuarios disponibles son ejemplos precargados. Asignar una tarea a alguien no crea una cuenta ni controla quién puede verla o modificarla.
- El MVP no incluye colaboración en tiempo real, fechas de vencimiento ni descripción de tarea.
- La prioridad inicial y si es obligatorio asignar responsable aún están por decidir.

## 3. Consultar la lista

Al abrir la aplicación, se mostrará la lista de tareas guardadas. Cada tarea mostrará su estado, prioridad y responsable cuando tenga uno asignado. Si todavía no hay tareas, la lista estará vacía.

Podrás elegir uno de estos filtros:

- **Todas:** muestra todas las tareas.
- **Pendientes:** muestra las tareas que no están completadas.
- **Completadas:** muestra las tareas finalizadas.

## 4. Crear una tarea

1. Desde la pantalla de tareas, inicia la creación de una tarea.
2. Escribe el título.
3. Elige una prioridad entre baja, media y alta.
4. Selecciona un usuario de ejemplo como responsable si la asignación se configura como opcional o requerida según la decisión final del proyecto.
5. Guarda la tarea.

La tarea aparecerá en la lista. El diseño final determinará los nombres de botones, la validación del título y los valores iniciales de los campos.

## 5. Editar una tarea

Abre las acciones de la tarea que quieras cambiar, modifica los datos disponibles y guarda los cambios. El listado deberá mostrar la información actualizada. Los campos editables concretos se confirmarán al implementar la interfaz.

## 6. Completar o reabrir una tarea

Cambia el estado de una tarea pendiente a completada cuando termines el trabajo. Si necesitas volver a trabajar en ella, podrás reabrirla para que vuelva a figurar como pendiente. El filtro de estado permitirá localizarla en cada caso.

## 7. Cambiar prioridad o responsable

Al crear o editar una tarea, podrás elegir prioridad baja, media o alta y seleccionar un responsable de la lista de usuarios de ejemplo. La asignación es solo informativa: no envía notificaciones ni limita el acceso.

## 8. Eliminar una tarea

Usa la acción de eliminación de la tarea correspondiente. La tarea dejará de aparecer en la lista y, según el alcance acordado, no se recuperará al reiniciar la aplicación. El comportamiento de confirmación antes de eliminar está pendiente de diseño.

## 9. Datos y privacidad

Las tareas se guardarán en una base SQLite local y deberán persistir entre reinicios. Como el MVP no incluye autenticación ni separación de datos por persona, los usuarios de ejemplo son responsables asignados y no perfiles privados.

## 10. Ayuda y problemas conocidos

Esta sección se completará después de implementar y probar la aplicación. Se añadirán aquí los pasos de inicio, posibles mensajes de error y soluciones verificadas. Por ahora, no hay una versión ejecutable ni una URL de acceso.

## Documentación relacionada

- [Análisis del MVP e historias de usuario](analisis.md)
- [Plan del proyecto](plan-proyecto.md)
- [Arquitectura y modelo de datos](arquitectura.md)
