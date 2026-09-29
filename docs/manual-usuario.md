# Manual de usuario: lista de tareas

> **Control documental**
> Código de proyecto: `app-todolist-260928`
> Fecha de actualización: `2026-09-29`
> Versión: `6`

## Estado de esta guía

**Aplicación implementada.** Esta guía describe la interfaz disponible. Las pruebas automatizadas existen; queda pendiente completar el recorrido manual de aceptación.

## 1. Acerca de la aplicación

La aplicación web **hecho.** permite gestionar tareas guardadas localmente en SQLite. Desde la pantalla **Mis tareas** puedes crear, editar y eliminar tareas, cambiar su estado, filtrarlas por estado, asignarles prioridad y fechas de inicio/fin, y seleccionar un responsable de ejemplo.

La versión actual de la aplicación aparece en el pie de la pantalla y se toma del archivo `frontend/package.json` para mantenerla sincronizada con la versión publicada.

## 2. Límites del MVP

- No hay registro ni inicio de sesión.
- Los usuarios disponibles son ejemplos precargados. Asignar una tarea a alguien no crea una cuenta ni controla quién puede verla o modificarla.
- El MVP no incluye colaboración en tiempo real, fecha de vencimiento independiente de las fechas de inicio/fin ni descripción de tarea.
- Las fechas son de calendario, sin hora. Puedes dejar ambas vacías o indicar solo el inicio. Para indicar un fin debes indicar también el inicio; el inicio puede ser igual o anterior al fin.
- El responsable es opcional y la prioridad inicial es media.

## 3. Consultar la lista

Al abrir la aplicación se cargan las tareas guardadas. El resumen superior muestra el total, las pendientes y las completadas. Cada fila presenta el título, el estado, la prioridad y, si existe, el responsable. Si aún no hay tareas, verás **Tu lista empieza aquí**. También puede aparecer una vista vacía cuando el filtro seleccionado no tenga resultados.

En **Tu lista**, elige uno de estos filtros:

- **Todas:** muestra todas las tareas.
- **Pendientes:** muestra las tareas que no están completadas.
- **Completadas:** muestra las tareas finalizadas.

## 4. Crear una tarea

1. En el formulario **Añadir tarea**, escribe un título de hasta 200 caracteres.
2. La prioridad inicial es media; puedes elegir baja, media o alta.
3. En **Responsable**, selecciona Alex, Sam o Taylor, o deja **Sin asignar**.
4. Si corresponde, indica la fecha de inicio y la fecha de fin. No se puede guardar un fin sin inicio ni un inicio posterior al fin.
5. Pulsa **Añadir tarea**.

El título es obligatorio. Al guardar, se recortan los espacios iniciales y finales y la tarea aparece en la lista como pendiente.

## 5. Editar una tarea

Pulsa el icono de edición de la tarea. Sus valores actuales, incluidas las fechas, aparecerán en el formulario; puedes cambiarlos y seleccionar **Guardar cambios**. Se aplican las mismas reglas de fechas que al crear una tarea. Para salir sin guardar, pulsa **Cancelar**.

## 6. Completar o reabrir una tarea

Pulsa el círculo junto a una tarea pendiente para marcarla como completada. Pulsa la marca de verificación para reabrirla. Los contadores se actualizan y, si tienes activo un filtro de estado, la tarea puede dejar de aparecer en esa vista al cambiar de estado.

## 7. Cambiar prioridad o responsable

En el formulario de creación o edición, selecciona prioridad baja, media o alta y un responsable de la lista de ejemplo (Alex, Sam o Taylor). La asignación es informativa: no envía notificaciones ni limita el acceso.

## 8. Eliminar una tarea

Pulsa el icono de papelera de la tarea. En el diálogo del navegador, confirma la eliminación; la tarea desaparece y no se puede recuperar desde la aplicación.

## 9. Datos y privacidad

Las tareas se guardan en la base SQLite local (`src/AppTodoList.Api/App_Data/tareas.db`) y la API aplica las migraciones al iniciarse. Los cambios guardados permanecen al reiniciar la aplicación. Como el MVP no incluye autenticación ni separación de datos por persona, los usuarios de ejemplo son responsables asignados, no perfiles privados.

## 10. Ayuda y problemas conocidos

La interfaz necesita que estén iniciados tanto el API como el servidor de desarrollo de Vite. Consulta [la guía de desarrollo](guia-desarrollo.md) para los comandos. Si no se cargan los datos, comprueba que ambos procesos estén activos. La comprobación manual completa de todos los flujos sigue pendiente.

## Documentación relacionada

- [Análisis del MVP e historias de usuario](analisis.md)
- [Plan del proyecto](plan-proyecto.md)
- [Arquitectura y modelo de datos](arquitectura.md)
