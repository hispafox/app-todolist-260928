---
name: "Versionado documental"
description: "Usar al crear o modificar documentación funcional del proyecto para mantener su código, fecha y versión incremental."
applyTo: ["README.md", "docs/**/*.md"]
---

# Versionado documental

Al crear o modificar un archivo incluido, aplica el procedimiento de la skill `versionado-documental` situada en `.github/skills/versionado-documental/SKILL.md` dentro del mismo cambio.

- El código de proyecto es siempre `app-todolist-260928`.
- La fecha usa el día local en formato `AAAA-MM-DD`.
- Un documento nuevo comienza en versión `1`; uno modificado incrementa en una unidad su versión anterior.
- La actualización del bloque forma parte de la modificación y no provoca un segundo incremento.