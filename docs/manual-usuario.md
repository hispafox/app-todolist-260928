# Manual de usuario: lista de tareas

> **Control documental**
> Código de proyecto: `app-todolist-260928`
> Fecha de actualización: `2026-09-28`
> Versión: `3`

## Estado de esta guía

**Primera versión implementada.** Los pasos describen la interfaz existente; el recorrido manual completo queda pendiente de validación.

## 1. Acerca de la aplicación

La aplicación permitirá gestionar una lista de tareas desde una interfaz web. Podrás crear y modificar tareas, cambiar su estado, filtrarlas, establecer una prioridad y asignarlas a usuarios de ejemplo. Los datos se guardarán localmente en SQLite y deberán conservarse al reiniciar la aplicación.

## 2. Límites del MVP

- No hay registro ni inicio de sesión.
- Los usuarios disponibles son ejemplos precargados. Asignar una tarea a alguien no crea una cuenta ni controla quién puede verla o modificarla.
- El MVP no incluye colaboración en tiempo real, fechas de vencimiento ni descripción de tarea.
- El responsable es opcional y la prioridad inicial es media.

## 3. Consultar la lista

Al abrir la aplicación, se mostrará la lista de tareas guardadas. Cada tarea mostrará su estado, prioridad y responsable cuando tenga uno asignado. Si todavía no hay tareas, la lista estará vacía.

Podrás elegir uno de estos filtros:

- **Todas:** muestra todas las tareas.
- **Pendientes:** muestra las tareas que no están completadas.
- **Completadas:** muestra las tareas finalizadas.

## 4. Crear una tarea

1. Desde la pantalla de tareas, inicia la creación de una tarea.
2. Escribe el título.
3. La prioridad inicial es media; puedes elegir baja, media o alta.
4. Selecciona un usuario de ejemplo como responsable o deja la tarea sin asignar.
5. Pulsa **Añadir tarea**.

La tarea aparecerá en la lista después de guardarla. El título es obligatorio y admite hasta 200 caracteres.

## 5. Editar una tarea

Abre las acciones de la tarea que quieras cambiar, modifica los datos disponibles y guarda los cambios. El listado deberá mostrar la información actualizada. Los campos editables concretos se confirmarán al implementar la interfaz.

## 6. Completar o reabrir una tarea

Cambia el estado de una tarea pendiente a completada cuando termines el trabajo. Si necesitas volver a trabajar en ella, podrás reabrirla para que vuelva a figurar como pendiente. El filtro de estado permitirá localizarla en cada caso.

## 7. Cambiar prioridad o responsable

Al crear o editar una tarea, podrás elegir prioridad baja, media o alta y seleccionar un responsable de la lista de usuarios de ejemplo. La asignación es solo informativa: no envía notificaciones ni limita el acceso.

## 8. Eliminar una tarea

Usa el botón de eliminar de la tarea. Confirma la acción en el diálogo; la tarea dejará de aparecer y no se recuperará al reiniciar la aplicación.

## 9. Datos y privacidad

Las tareas se guardarán en una base SQLite local y deberán persistir entre reinicios. Como el MVP no incluye autenticación ni separación de datos por persona, los usuarios de ejemplo son responsables asignados y no perfiles privados.

## 10. Ayuda y problemas conocidos

Para iniciar la API y la interfaz, consulta [la guía de desarrollo](guia-desarrollo.md). La comprobación manual de todos los flujos sigue pendiente.

## Documentación relacionada

- [Análisis del MVP e historias de usuario](analisis.md)
- [Plan del proyecto](plan-proyecto.md)
- [Arquitectura y modelo de datos](arquitectura.md)
