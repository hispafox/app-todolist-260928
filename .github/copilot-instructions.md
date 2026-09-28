# Instrucciones del proyecto

## Contexto

Este repositorio contiene una aplicación web de lista de tareas cuyo objetivo es desarrollar un MVP de forma iterativa con GitHub Copilot y Visual Studio Code.

Tecnologías acordadas:

- Backend: ASP.NET Core 10.
- Frontend: React.
- Persistencia: Entity Framework Core con SQLite.
- Calidad: pruebas automatizadas, prueba manual guiada y SonarQube con reglas locales básicas.

El proyecto tiene una primera estructura ejecutable y el MVP está en implementación. Comprueba el estado real en el código y la documentación antes de proponer cambios; no inventes rutas, scripts, contratos ni comandos que no estén definidos.

## Documentación de referencia

Antes de implementar una funcionalidad, revisa el requisito correspondiente y sus criterios de aceptación en `docs/analisis.md`. Usa también estos documentos como fuente de verdad:

- `README.md`: alcance, tecnologías, comandos y estado general.
- `docs/analisis.md`: requisitos funcionales, historias de usuario, criterios de aceptación y decisiones acordadas o pendientes.
- `docs/arquitectura.md`: responsabilidades, arquitectura lógica y modelo de datos.
- `docs/plan-proyecto.md`: fases, incrementos y verificaciones.
- `docs/guia-desarrollo.md`: convenciones locales, persistencia, pruebas y flujo de trabajo.
- `docs/manual-usuario.md`: comportamiento previsto y límites del MVP.

Si el código contradice la documentación, identifica la discrepancia y actualiza la documentación cuando el cambio sea intencionado. No des por resuelta una decisión pendiente sin registrarla primero.

## Forma de trabajo

- Trabaja en incrementos pequeños, verticales y verificables.
- Antes de modificar código, explica brevemente el requisito, el plan, los archivos afectados y cómo se comprobará el cambio.
- Revisa el diff y ejecuta una comprobación enfocada después de cada cambio.
- Cada funcionalidad debe incluir o actualizar sus pruebas automatizadas en el mismo incremento.
- No consideres una funcionalidad validada porque Copilot haya generado el código: revisa el resultado y comprueba el comportamiento.
- No ejecutes aplicaciones, servidores ni watchers automáticamente. El desarrollador los inicia manualmente.
- No inventes datos, contratos, campos o reglas que no formen parte del alcance o de una decisión documentada.
- Mantén los cambios centrados en la tarea y evita refactorizaciones no relacionadas.

## Arquitectura y responsabilidades

Mantén separadas las responsabilidades de interfaz, API, servicios de aplicación, lógica de negocio, modelo de dominio y persistencia:

- React presenta el listado, formularios, filtros y acciones, y se comunica con la API mediante HTTP y JSON.
- ASP.NET Core expone endpoints HTTP y traduce peticiones y respuestas.
- Los servicios de aplicación coordinan los casos de uso.
- La lógica de negocio aplica reglas y validaciones del dominio.
- El modelo representa tareas, usuarios de ejemplo, estados y prioridades.
- `DbContext` y las configuraciones de EF Core se ocupan del acceso y mapeo de datos.
- Los controladores o endpoints no deben contener consultas de persistencia ni reglas de negocio complejas.
- No mezcles lógica de presentación, acceso a datos y reglas de negocio en una misma clase.

Respeta la estructura real del proyecto cuando se cree. No añadas capas, proyectos o abstracciones por anticipado si no aportan una responsabilidad clara.

## Alcance funcional del MVP

La aplicación debe permitir:

- Crear, consultar, editar y eliminar tareas.
- Marcar tareas como completadas y reabrirlas.
- Filtrar por todas, pendientes o completadas.
- Asignar una tarea a un usuario de ejemplo precargado.
- Establecer prioridad baja, media o alta.
- Persistir los cambios en SQLite entre reinicios.

Una tarea contiene como mínimo título, estado, prioridad y una asignación opcional o obligatoria según la decisión documentada. No añadas descripción, fecha de vencimiento, historial ni otros campos sin requisito.

Los usuarios precargados son datos de ejemplo y responsables informativos. El MVP no incluye registro, inicio de sesión, autenticación, autorización, privacidad por usuario, notificaciones, colaboración en tiempo real ni despliegue multiusuario.

## Entity Framework Core y SQLite

- Usa Entity Framework Core como única vía de acceso a SQLite.
- Centraliza el modelo persistido en un `DbContext` y configuraciones de entidad coherentes con la arquitectura del proyecto.
- Define explícitamente las relaciones, nulabilidad, claves y restricciones del modelo.
- Trata `Status` y `Priority` como conceptos del dominio con valores limitados a los estados y prioridades acordados. La representación concreta en SQLite debe ser consistente y estar documentada.
- Precarga únicamente los usuarios de ejemplo acordados. El seeding debe ser determinista y no duplicar registros en cada arranque.
- Usa migraciones o el mecanismo de actualización de esquema configurado por el proyecto.
- No borres la base de datos SQLite para resolver errores de esquema o desarrollo. Investiga la causa y aplica una migración o actualización compatible; conserva una copia si contiene datos que deban mantenerse.
- No ocultes errores de persistencia ni dependas únicamente del estado en memoria para afirmar que los datos se guardan.
- Prueba tanto las operaciones de persistencia como las reglas que dependen de ellas.

## API y validación

- Diseña contratos HTTP claros y coherentes con los casos de uso del MVP.
- Valida las entradas en el límite de la API y vuelve a proteger las invariantes en la lógica de negocio cuando corresponda.
- Devuelve códigos HTTP y errores comprensibles y consistentes con el comportamiento acordado.
- No expongas directamente entidades de persistencia si eso acopla el contrato público al esquema interno; usa modelos de entrada y salida cuando sea necesario.
- No añadas autenticación ni autorización: están fuera del alcance actual.

## Pruebas y calidad

- Añade pruebas para crear, listar, editar, eliminar, completar, reabrir, filtrar, asignar, priorizar y persistir tareas según el incremento implementado.
- Cubre los casos vacíos, entradas inválidas y reglas de nulabilidad que estén definidos.
- Verifica que los cambios sobreviven a una nueva consulta o reinicio cuando el criterio de aceptación lo exige.
- Mantén separadas las pruebas de servicios/reglas, API/persistencia e interfaz según los frameworks elegidos.
- Ejecuta SonarQube únicamente con la configuración acordada en el repositorio; su análisis complementa las pruebas y no las sustituye.
- No marques una historia como completada hasta que sus pruebas automatizadas y la comprobación manual pertinente estén realizadas.

## Desarrollo local

- El frontend debe usar HTTP en `http://localhost:5173`.
- La API debe usar HTTPS en `https://localhost:5001` con el certificado de desarrollo de .NET.
- No añadas certificados autofirmados ni plugins SSL al frontend.
- Si se usa Vite, su proxy debe apuntar a `https://localhost:5001` con `secure: false`; configura el equivalente si se elige otra herramienta.
- Los puertos y comandos definitivos deben tomarse de la configuración real del proyecto, no asumirse desde esta instrucción.

## Documentación y entrega

Cuando cambien el comportamiento, los comandos, las decisiones técnicas o la estructura, actualiza `README.md` y el documento de `docs/` correspondiente. Mantén documentado el estado real del MVP, incluyendo lo que sigue pendiente.

Al finalizar un incremento, resume el requisito cubierto, los archivos modificados, las pruebas ejecutadas y cualquier decisión pendiente o limitación restante.
