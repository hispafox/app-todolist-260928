---
name: mensajes-commit
description: 'Redacta mensajes de commit concretos para los cambios de este proyecto. Usar al pedir un mensaje de commit, preparar un commit o revisar su resumen y cuerpo.'
---

# Mensajes de commit

## Procedimiento

1. Consulta el estado de Git y el diff de los cambios preparados (`git diff --cached`). Si se va a crear un commit, describe solo esos cambios. Si no hay cambios preparados y solo se pide una propuesta de mensaje, consulta el diff sin preparar y aclara que la propuesta corresponde a esos cambios. Incluye los archivos nuevos sin seguimiento solo si se conoce su contenido; no supongas que entraran en el commit.
2. Identifica el cambio concreto y su proposito. No infieras funcionalidades, correcciones o resultados que no se vean en los cambios.
3. Escribe la primera linea en castellano y minusculas, con el formato `tipo(ambito): descripcion corta`. Usa un tipo acorde con el cambio, como `feat`, `fix`, `docs`, `test`, `refactor` o `chore`; omite `(ambito)` cuando no aporte precision. El resumen debe decir que cambio, no solo que archivo se toco.
4. Anade un cuerpo separado por una linea en blanco cuando ayude a explicar decisiones, alcance o detalles que no caben en el resumen. Para cambios triviales, deja solo la primera linea. Evita repetir el resumen en el cuerpo.
5. Entrega el mensaje propuesto sin ejecutar `git add` ni `git commit`, salvo que el usuario los haya solicitado expresamente.

## Criterio de redaccion

- Se especifico sin perder brevedad: `docs: cambiar idioma de los ejemplos de codigo al castellano` describe el cambio; `docs: actualizar instrucciones` no.
- Prioriza el efecto observable sobre frases genericas como "actualizar fichero", "hacer cambios" o "mejoras varias".
- Si el commit incluye cambios distintos, resume el proposito que los une y usa el cuerpo para distinguirlos. Si no existe un proposito comun, senala que conviene separarlos en commits en lugar de inventar uno.