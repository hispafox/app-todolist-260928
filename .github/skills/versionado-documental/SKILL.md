---
name: versionado-documental
description: 'Versiona y mantiene actualizados los documentos del proyecto. Usar al crear o modificar README.md o archivos Markdown de docs/, o cuando se solicite actualizar código de proyecto, fecha o versión documental.'
---

# Versionado documental

Mantén un bloque de control documental inmediatamente después del título principal de cada documento incluido:

```markdown
> **Control documental**
> Código de proyecto: `app-todolist-260928`
> Fecha de actualización: `AAAA-MM-DD`
> Versión: `N`
```

## Alcance

- Incluye `README.md` y los archivos Markdown situados en `docs/` y sus subdirectorios.
- Excluye `labs/`, porque sus materiales formativos tienen un versionado editorial propio.
- Excluye archivos de configuración, código fuente y personalizaciones de agentes, aunque utilicen Markdown.

## Procedimiento

1. Comprueba si el documento ya contiene el bloque de control documental.
2. Conserva siempre el código de proyecto `app-todolist-260928`.
3. Usa la fecha local del día en formato ISO `AAAA-MM-DD`.
4. Si se crea un documento incluido, asigna la versión `1`.
5. Si se modifica el contenido de un documento existente, incrementa su versión entera en una unidad y actualiza la fecha.
6. Si la única operación pendiente es corregir el propio bloque tras haber modificado el contenido en la misma sesión, no vuelvas a incrementar la versión.
7. No cambies la versión de documentos que no hayan sido modificados.
8. Antes de terminar, verifica que cada documento modificado tenga un único bloque, que la fecha sea la actual y que la nueva versión sea exactamente la anterior más uno.

## Restricciones

- No reinicies la versión ni uses versiones decimales o semánticas.
- No inventes una versión previa si el documento carece de bloque: inicialízala en `1`.
- No actualices únicamente la fecha sin incrementar la versión cuando haya cambiado el contenido.
- No alteres el versionado editorial propio de documentos excluidos.