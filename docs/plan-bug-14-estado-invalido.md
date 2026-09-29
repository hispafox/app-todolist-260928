# Plan de resolución del bug #14: estado no definido devuelve 500

> **Control documental**
> Código de proyecto: `app-todolist-260928`
> Fecha de actualización: `2026-09-29`
> Versión: `1`

## Objetivo y alcance

Corregir que `PUT /api/tareas/{id}/estado` devuelva `500 Internal Server Error` en vez de `400 Bad Request` cuando el valor de `Estado` recibido no está definido en el enum `EstadoTarea` (por ejemplo, un entero fuera de rango). Este plan prepara el desarrollo; no implementa todavía el cambio ni da el bug por resuelto.

El alcance se limita al endpoint en `src/AppTodoList.Api/Program.cs`. No se propone modificar `ServicioTareas.CambiarEstadoAsync`, `ContratosTarea.cs` ni `EstadoTarea.cs`, porque la regla de negocio ya existe y es correcta; falta únicamente que el endpoint la traduzca a una respuesta HTTP adecuada.

## Comportamiento actual confirmado (leído en el código)

- `ServicioTareas.CambiarEstadoAsync` ([src/AppTodoList.Api/Aplicacion/ServicioTareas.cs](../src/AppTodoList.Api/Aplicacion/ServicioTareas.cs#L72-L86)) valida con `Enum.IsDefined(estado)` y lanza `ArgumentException("El estado indicado no es válido.")` si el valor no está definido. Este comportamiento es correcto y no requiere cambios.
- El endpoint `PUT /api/tareas/{id}/estado` ([src/AppTodoList.Api/Program.cs](../src/AppTodoList.Api/Program.cs#L88-L96)) invoca `servicio.CambiarEstadoAsync` **sin** try/catch. Los endpoints `POST /api/tareas` y `PUT /api/tareas/{id}` sí envuelven la llamada equivalente en `try { ... } catch (ArgumentException error) { return ErrorValidacion(error.Message); }`, donde `ErrorValidacion` devuelve `Results.ValidationProblem` (400). El endpoint de estado carece de este bloque, por lo que la `ArgumentException` queda sin capturar y el pipeline por defecto de ASP.NET Core la traduce en `500`.
- **Diagnóstico confirmado**: coincide exactamente con la hipótesis de partida.

## Criterio de aceptación aplicable

- `docs/arquitectura.md` línea 98 establece explícitamente: *"Las entradas no válidas devuelven errores de validación HTTP 400; los identificadores inexistentes devuelven 404."* Este criterio ya está documentado y el endpoint de estado lo incumple hoy. No hace falta añadir ni negociar un criterio nuevo; el cambio corrige una implementación que no sigue una decisión ya acordada.
- `docs/analisis.md` (RF-05, línea 64) describe la funcionalidad de cambio de estado sin detallar el código HTTP; no contradice el criterio de arquitectura.md.
- No se requiere modificar la documentación funcional salvo que la implementación revele un comportamiento distinto al aquí previsto.

## Matiz sobre `JsonStringEnumConverter` (hipótesis a verificar en implementación)

`Program.cs` registra `JsonStringEnumConverter` globalmente vía `ConfigureHttpJsonOptions`. Esto tiene dos efectos distintos que conviene distinguir y comprobar por separado, ya que **no se han ejecutado pruebas en esta sesión**:

1. **String no reconocido** (p. ej. `{"estado": "Cancelada"}`): previsiblemente falla la deserialización del cuerpo JSON durante el model binding, antes de llegar al handler. El comportamiento por defecto de las minimal APIs ante un fallo de deserialización del body es devolver `400 Bad Request` automáticamente. Es una hipótesis razonable dado el comportamiento estándar del framework, pero debe confirmarse con una prueba concreta; no se debe asumir sin verificar.
2. **Entero fuera de rango** (p. ej. `{"estado": 5}`): `System.Text.Json` deserializa enteros a un enum sin exigir que el valor esté definido, incluso con `JsonStringEnumConverter` activo (que solo intercepta representaciones en texto). La deserialización tiene éxito y el valor fuera de rango llega al servicio, que lo rechaza con `ArgumentException`. Este es el caso que hoy produce el `500` y el que principalmente debe cubrir la corrección y sus pruebas.

Si al implementar se comprueba que el caso 1 ya devuelve 400 por sí solo, la corrección puede limitarse al caso 2; si no fuera así, habría que ampliar el alcance, lo cual debe registrarse como cambio de plan, no asumirse aquí.

## Incremento propuesto

1. En `src/AppTodoList.Api/Program.cs`, envolver la llamada `await servicio.CambiarEstadoAsync(...)` del endpoint `PUT /api/tareas/{id}/estado` en un `try { ... } catch (ArgumentException error) { return ErrorValidacion(error.Message); }`, replicando el patrón ya usado en `POST /api/tareas` y `PUT /api/tareas/{id}`. Es un cambio de una sola sección del archivo.
2. No tocar `ServicioTareas.cs`, `ContratosTarea.cs` ni `EstadoTarea.cs`: la regla de negocio ya es correcta.
3. Verificar durante la implementación el matiz del apartado anterior (string no reconocido vs. entero fuera de rango) con una prueba concreta antes de dar el incremento por completo.
4. Si la verificación confirma que el criterio de `docs/arquitectura.md` ya queda satisfecho con este cambio, no es necesario modificar esa documentación (ya refleja el comportamiento esperado). Solo actualizarla si aparece un matiz no documentado (por ejemplo, un comportamiento distinto entre string y entero que se considere relevante para el lector).

## Archivos candidatos

- `src/AppTodoList.Api/Program.cs`: cambio principal (try/catch en el endpoint de estado).
- `tests/AppTodoList.Tests/ServicioTareasTests.cs`: prueba de servicio (ver más abajo).
- Posible nuevo archivo de pruebas de endpoint (por ejemplo `tests/AppTodoList.Tests/EndpointsTareasTests.cs`), si se decide cubrir el caso a nivel HTTP.

## Pruebas a añadir o actualizar

- **Nivel de servicio** (ya cubierto en parte, falta un caso explícito): añadir en `ServicioTareasTests.cs` una prueba que llame a `_servicio.CambiarEstadoAsync(id, (EstadoTarea)99, default)` sobre una tarea existente y compruebe que lanza `ArgumentException` con un mensaje que mencione "estado". Esto ya se puede inferir del código de `ServicioTareas`, pero no hay una prueba dedicada; sin ella, una regresión futura en la validación del servicio no se detectaría.
- **Nivel de endpoint (gap detectado)**: el proyecto de pruebas (`tests/AppTodoList.Tests/AppTodoList.Tests.csproj`) no referencia `Microsoft.AspNetCore.Mvc.Testing` ni existe hoy ninguna prueba que ejercite `Program.cs` a través de HTTP (`WebApplicationFactory` u otro mecanismo). Todas las pruebas actuales llaman directamente a `ServicioTareas`. **Esto es relevante porque el propio bug está en el cableado del endpoint, no en el servicio**: una prueba de servicio que ya pasaba no habría detectado este defecto, y no lo detectaría tampoco tras la corrección si algo se rompe de nuevo en `Program.cs`.
  - Antes de implementar, decidir si se añade infraestructura de pruebas de integración HTTP (nueva dependencia `Microsoft.AspNetCore.Mvc.Testing`, nuevo archivo de pruebas) para verificar que `PUT /api/tareas/{id}/estado` con un valor de estado fuera de rango devuelve `400` con el cuerpo de `ValidationProblem`, o si la verificación de este caso concreto se limita a comprobación manual guiada (por ejemplo, con el archivo `AppTodoList.Api.http`).
  - Esta decisión no se resuelve en este plan porque implica añadir una dependencia y un patrón de pruebas nuevo en el proyecto, más allá del alcance mínimo de la corrección puntual.

## Preguntas bloqueantes / decisiones pendientes

1. ¿Se autoriza añadir `Microsoft.AspNetCore.Mvc.Testing` y un primer archivo de pruebas de integración HTTP, o se prefiere verificar el endpoint únicamente con comprobación manual guiada (`.http` file) para este incremento?
2. Confirmar en la implementación si el caso "string no reconocido" ya devuelve 400 sin cambios (hipótesis del apartado anterior) o si también requiere ajuste; no se ha ejecutado ninguna prueba en esta sesión que lo confirme.

## Verificación prevista (a ejecutar en la implementación, no en este plan)

- `dotnet test` sobre `tests/AppTodoList.Tests/AppTodoList.Tests.csproj`, comprobando que la nueva prueba de servicio pasa.
- Si se autoriza la pregunta 1, ejecutar la prueba de endpoint nueva y comprobar que devuelve 400 con el cuerpo de validación esperado para un estado entero fuera de rango.
- Comprobación manual guiada con `AppTodoList.Api.http`: enviar `PUT /api/tareas/{id}/estado` con un valor de estado fuera de rango y otro con un string no reconocido, y registrar el código HTTP obtenido en cada caso.
- Revisar el diff para confirmar que el cambio se limita al endpoint de estado y no introduce otras modificaciones.
