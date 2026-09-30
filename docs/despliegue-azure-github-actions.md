# Despliegue en Azure y GitHub Actions

> **Control documental**
> Código de proyecto: `app-todolist-260928`
> Fecha de actualización: `2026-09-30`
> Versión: `1`

## Estado del proyecto

El repositorio ya incluye una base funcional para la integración continua y un flujo de despliegue preparado para Azure App Service. La implementación actual se ha diseñado como una demo realista y segura: la compilación, pruebas y publicación de artefactos están automatizadas; el despliegue real queda desactivado por defecto para evitar una publicación accidental.

## Objetivo

Establecer una base de CI/CD que:

- valide el backend y el frontend en cada cambio,
- compile la solución y la aplicación React,
- prepare artefactos listos para despliegue,
- deje un flujo de Azure preparado para ejecutarse cuando existan credenciales y permisos reales.

## Qué se ha implementado

### 1. Integración continua en GitHub Actions

Se creó el workflow [.github/workflows/ci.yml](../.github/workflows/ci.yml), con estas tareas:

- restauración de la solución .NET,
- ejecución de pruebas del backend,
- instalación de dependencias del frontend,
- ejecución de pruebas del frontend,
- compilación del frontend en producción,
- publicación del API en un artefacto para la etapa de despliegue.

Este workflow se dispara en:

- pushes a la rama principal,
- pull requests,
- ejecución manual desde la interfaz de GitHub.

### 2. Despliegue Azure preparado

Se añadió el workflow [.github/workflows/azure-deploy.yml](../.github/workflows/azure-deploy.yml), que incluye:

- validación previa de compilación y pruebas,
- preparación del artefacto ZIP del backend,
- despliegue a Azure App Service cuando el usuario activa la opción real,
- modo de simulación por defecto para evitar despliegues accidentales.

### 3. Infraestructura mínima con Bicep

La carpeta [infra](../infra) contiene:

- [infra/main.bicep](../infra/main.bicep): definición mínima de App Service Linux con runtime .NET 10,
- [infra/main.parameters.json](../infra/main.parameters.json): parámetros para la creación del recurso.

Este punto de partida es suficiente para un entorno de demostración y para ajustar la configuración real cuando se disponga de la suscripción y del nombre del recurso.

## Alcance real del despliegue

La solución actual está preparada para una demo de integración continua y para cerrar el primer ciclo de despliegue con supervisión manual. No se ha ejecutado un despliegue real en Azure desde este entorno porque falta:

- credenciales Azure válidas,
- un resource group o suscripción preparada,
- permisos para crear o actualizar recursos,
- la decisión de si se desplegará la API sola o con frontend.

## Requisitos para activar el despliegue real

Para completar la publicación real, será necesario:

1. configurar el secreto `AZURE_CREDENTIALS` en GitHub,
2. definir el nombre del recurso Azure y el grupo de recursos,
3. activar el flujo manual con `simulate_only=false`,
4. verificar que el runtime .NET 10 está disponible en la región elegida,
5. comprobar que el plan y el App Service cumplen con los requisitos del proyecto.

## Verificación ejecutada

Se ha validado localmente lo siguiente:

- `dotnet test AppTodoList.sln --nologo -v q`
- `npm --prefix frontend test -- --run --reporter=basic`
- `npm --prefix frontend run build -- --emptyOutDir`

La ejecución terminó con resultado correcto y la salida final del comando mostró `ALL_CHECKS_OK`.

## Conclusión

Este punto deja preparado un primer ciclo profesional de CI/CD para la aplicación: compilación, verificación automática y preparación para despliegue en Azure. El estado actual es una demostración funcional y segura de integración continua, con la posibilidad de ampliar a despliegue real cuando se disponga de acceso Azure y configuración de secretos.
