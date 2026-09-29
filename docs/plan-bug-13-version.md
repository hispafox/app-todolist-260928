# Plan de resolución del bug #13: versión visible

> **Control documental**
> Código de proyecto: `app-todolist-260928`
> Fecha de actualización: `2026-09-29`
> Versión: `1`

## Objetivo y alcance

Resolver la [issue #13](https://github.com/hispafox/app-todolist-260928/issues/13): la página principal no muestra el número de versión publicado, lo que impide identificar con precisión la versión utilizada en las pruebas. Este plan prepara el desarrollo; no implementa todavía el cambio ni da el bug por resuelto.

La fuente de verdad para la versión de la aplicación será `frontend/package.json`, que actualmente declara `1.0.0`. La versión `1` de este documento identifica únicamente su revisión documental: no es la versión de la aplicación. No se propone modificar la API ni la base SQLite, porque el defecto y los criterios de aceptación se limitan a la página principal.

## Evidencia y criterios de aceptación

- La issue indica que el número debe verse claramente en la página principal, coincidir con la versión definida y quedar cubierto por una prueba de interfaz.
- `frontend/src/App.tsx` renderiza el encabezado y el pie sin versión; `frontend/src/App.test.tsx` no comprueba ese dato.
- La versión declarada en el paquete no debe copiarse como literal independiente en la interfaz: cuando cambie el paquete, el valor visible debe seguirlo.

## Incremento propuesto

1. Añadir a `frontend/src/App.test.tsx` una prueba que renderice la página y compruebe la versión visible frente a `frontend/package.json`. Confirmar primero que falla con la interfaz actual.
2. En `frontend/src/App.tsx`, leer la versión del paquete y mostrarla con una etiqueta accesible y discreta en el pie existente. Mantenerla visible también si las tareas están vacías o falla su carga. Si la importación del JSON exige configuración de TypeScript, ajustar solo `frontend/tsconfig.app.json`.
3. Ajustar `frontend/src/styles.css` únicamente si el texto nuevo necesita espacio o contraste en móvil y escritorio. No crear un endpoint ni introducir una segunda constante de versión.
4. Cuando se implemente el cambio, actualizar `README.md` y `docs/manual-usuario.md` para indicar dónde consultar la versión, aplicando a ambos su versionado documental. No marcar la issue como cerrada antes de la verificación.

## Verificación y evidencia de pruebas

- Ejecutar `npm --prefix frontend test` y `npm --prefix frontend run build`; comprobar que la prueba nueva pasa y que el valor procede del paquete.
- Revisar el diff para detectar versiones duplicadas, cambios ajenos al bug o modificaciones innecesarias de backend y persistencia.
- El desarrollador inicia la aplicación manualmente: comprobar en la página principal que el número se lee en el pie con tareas, sin tareas y a anchura móvil. Registrar en la evidencia de cada prueba el número visible, la fecha y el resultado; asociarla a la issue #13.
- Criterio de salida: versión visible = versión declarada en `frontend/package.json`, prueba de interfaz aprobada, compilación aprobada y comprobación manual registrada. Solo entonces evaluar el cierre de la issue.