# Análisis del encargo: fechas de inicio y fin de las tareas

> **Control documental**
> Código de proyecto: `app-todolist-260928`
> Fecha de actualización: `2026-09-29`
> Versión: `3`

## Encargo, referencia, prioridad y resultado esperado

- **Encargo:** analizar la incorporación de fechas de inicio y fin a las tareas.
- **Referencia:** no se proporcionó identificador externo. Identificador derivado: `fechas-tarea`.
- **Prioridad:** no indicada por quien encargó el análisis.
- **Resultado esperado:** una tarea podrá tener fecha de inicio, fecha de fin o ambas bajo las combinaciones válidas definidas en este informe. Se capturan en el formulario de creación/edición, se devuelven en la lectura que alimenta la edición y se muestran en el listado. Son fechas de calendario sin hora.

## Requisitos e historias aplicables

- `docs/analisis.md`, secciones 4, 6, 7 y 9: la gestión abarca creación, consulta, edición y persistencia. RF-01, RF-02, RF-03 y RF-09, y las historias HU-01, HU-02, HU-03 y HU-09 son las superficies relacionadas por analogía con los datos de una tarea. Actualmente no mencionan fechas.
- `docs/analisis.md`, sección 11: declara que el MVP se limita a título, estado, prioridad y responsable opcional, excluye expresamente fecha de vencimiento y no define inicio ni fin. La petición actual propone ampliar ese límite, pero el documento canónico no ha sido actualizado.
- Solicitud del usuario recibida por el Jefe de proyecto: «Las tareas no tienen fecha de inicio ni fecha de fin. A ver si podemos poner en marcha esta modificación. Además, el tema es que la fecha de inicio no podría ser posterior a la fecha de fin, por lógica.» La relación `inicio <= fin` queda definida por esta instrucción y permite igualdad.
- Respuestas del usuario: «fechas opcionales, se capturan en el formulario» y «Si se informa una fecha de fin, tendrá una fecha de inicio. Si se pone fecha de inicio, a lo mejor no sabemos cuándo es la fecha de fin. O podemos no poner ninguna. No deben incluir la hora. Sí, muéstralas en el listado también.» La matriz exacta es: ninguna; solo inicio; inicio y fin cuando inicio <= fin. Fin sin inicio no es válido. Son fechas de calendario sin hora, capturadas en creación/edición y mostradas en el listado.

## Situación actual comprobada y evidencia relevante

- `src/AppTodoList.Api/Dominio/Tarea.cs`: la entidad contiene título, estado, prioridad, responsable y relación a usuario; no tiene propiedades de fecha.
- `src/AppTodoList.Api/Aplicacion/ContratosTarea.cs`: `SolicitudTarea` sirve para crear y editar con título, prioridad y responsable; `TareaRespuesta` devuelve esos datos. No hay campos de fecha.
- `src/AppTodoList.Api/Aplicacion/ServicioTareas.cs`: creación y edición asignan dichos campos y validan título, prioridad y responsable. No hay validación de fechas.
- `src/AppTodoList.Api/Persistencia/ListaTareasDbContext.cs`: el mapeo configura los campos existentes; el proyecto aplica migraciones EF Core al iniciar (`src/AppTodoList.Api/Program.cs`). La fecha y valor que recibirían registros ya existentes no están definidos por el modelo actual.
- `frontend/src/types.ts` y `frontend/src/App.tsx`: el tipo de tarea, el formulario de creación/edición y el listado no contienen fechas. La edición se inicia desde la tarea obtenida al cargar el listado, por lo que la lectura que provee esos datos debe devolver las fechas para rellenar el formulario.
- `tests/AppTodoList.Tests/ServicioTareasTests.cs` y `tests/AppTodoList.Tests/EndpointsTareasTests.cs`: existen pruebas de servicio/persistencia y API para el comportamiento actual, pero no hay criterios ni cobertura de fechas.
- No se inspeccionó ni modificó el contenido de una base de datos local; por tanto, no se afirma cuántos registros existen ni el impacto concreto de la migración. La regla funcional sí está definida: las tareas existentes sin fechas conservan esa ausencia válida.

## Alcance, exclusiones, comportamientos y casos límite

**Alcance:** añadir fechas de calendario de inicio y fin, sin componente de hora, al ciclo de creación, consulta, edición y persistencia; capturarlas en el formulario de creación/edición, devolverlas en la lectura que rellena el formulario de edición y mostrar ambas en el listado. Aplicar exactamente estas reglas:

- **Válido:** inicio ausente y fin ausente.
- **Válido:** inicio presente y fin ausente.
- **Válido:** inicio y fin presentes si `inicio <= fin`; se permite que sean iguales.
- **Inválido:** fin presente e inicio ausente.
- **Inválido:** inicio y fin presentes si `inicio > fin`.

Las tareas existentes sin fechas siguen siendo válidas y deben permanecer sin fechas inventadas. Este análisis define el comportamiento esperado, no prescribe la implementación.

**Exclusiones:** fecha/hora (no se captura ni conserva hora), filtrado u ordenación por fecha, vistas de calendario, recordatorios, historial, reglas según estado y cualquier capacidad no especificada aquí. No se asignan fechas por defecto a tareas existentes.

La captura se realiza en el formulario existente de tareas, que se usa tanto para crear como para editar. La lectura que proporciona la tarea para la edición debe devolver los dos valores, y el listado debe mostrarlos. No se amplía la captura a otras superficies.

## Criterios de aceptación verificables

Los criterios se vinculan con RF-01, RF-02, RF-03 y RF-09 de `docs/analisis.md`. El Programador actualizará primero los documentos canónicos afectados conforme a las instrucciones del proyecto; este informe no los modifica.

- **CA-01 (RF-01, RF-03):** el formulario de creación y edición permite capturar inicio y fin como fechas de calendario sin hora. Guardar sin ambas fechas está permitido.
- **CA-02 (RF-01, RF-03):** guardar solo una fecha de inicio está permitido y conserva el inicio; guardar una fecha de fin sin inicio se rechaza con un error comprensible.
- **CA-03 (regla explícita del encargo):** si ambas fechas están presentes, `inicio < fin` y `inicio == fin` se aceptan; `inicio > fin` se rechaza y se comunica un error comprensible. La comparación corresponde a fechas de calendario, sin hora.
- **CA-04 (RF-02, RF-03):** la lectura usada para cargar la tarea en el formulario de edición devuelve inicio y fin, incluidos los valores ausentes, de modo que se muestran los valores actuales y se conservan al guardar sin cambios.
- **CA-05 (RF-02):** cada tarea del listado muestra las fechas presentes de inicio y fin; una fecha ausente no se sustituye por un valor inventado.
- **CA-06 (RF-09):** las fechas introducidas se conservan al volver a consultar y tras reiniciar la aplicación. Las tareas preexistentes sin fechas siguen disponibles y permanecen sin fechas asignadas.
- **CA-07 (cobertura de combinaciones):** las pruebas automatizadas cubren las cuatro combinaciones presencia/ausencia (ninguna válida, solo inicio válido, solo fin inválida, ambas presentes sujetas a orden), incluyendo igualdad permitida, orden invertido rechazado, edición/lectura, listado y conservación de tareas existentes sin fechas.

No se ejecutaron pruebas ni comandos durante este análisis. La ejecución de las pruebas automatizadas y la comprobación manual pertinente corresponden al flujo de implementación/verificación.

## Dependencias, riesgos, contradicciones y decisiones pendientes

- **Documentación canónica pendiente para el flujo de implementación:** la nueva capacidad amplía el alcance y contradice las exclusiones actuales sobre fechas en `docs/analisis.md` y la arquitectura relacionada. El Programador debe actualizar primero los documentos canónicos afectados conforme a las instrucciones del proyecto. El Analista no los modifica.
- **Riesgo de datos:** no se inspeccionó la base de datos local; no se afirma cuántos registros existen. La regla acordada exige que los existentes sin fechas sigan válidos y no reciban fechas inventadas.
- **Decisiones funcionales pendientes:** ninguna. Las reglas de presencia, precisión, presentación, igualdad y datos existentes están determinadas.

## Traspaso al Programador

- **Resumen funcional:** implementar las dos fechas de calendario sin hora, con la matriz de presencia y la regla de orden especificadas, captura en creación/edición, lectura de valores para rellenar edición y presentación en el listado.
- **Criterios:** CA-01 a CA-07 de este informe. Las pruebas deben cubrir todas las combinaciones de fechas, incluida igualdad, rechazo de fin sin inicio y de inicio posterior a fin, lectura/edición, listado y tareas existentes sin fechas.
- **Límites:** no incorporar hora, filtrado, ordenación, calendario, recordatorios ni fechas inventadas para registros existentes.
- **Estado:** `listo`.
- **Traspaso:** se encarga al Programador actualizar primero los documentos canónicos afectados conforme a `.github/copilot-instructions.md` e implementar y probar el alcance. Rutas de referencia: `docs/analisis-encargo-fechas-tarea.md` y `docs/auditoria-flujo-general-fechas-tarea.md`. No se ejecutaron pruebas ni comandos en esta intervención.