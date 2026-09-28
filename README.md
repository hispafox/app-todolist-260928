# Lista de tareas

Aplicación web de lista de tareas planteada como proyecto práctico para aprender a desarrollar de forma iterativa con GitHub Copilot y Visual Studio Code.

> **Estado:** fase de análisis. Las funcionalidades descritas son el alcance previsto del MVP; todavía no hay una implementación ejecutable ni comandos de inicio definidos.

## Objetivo

Practicar cómo convertir una necesidad en requisitos verificables, planificar antes de programar, implementar cambios pequeños con ayuda de Copilot y comprobar el resultado con pruebas y análisis de calidad.

## Alcance previsto

El MVP permitirá:

- Crear, consultar, editar y eliminar tareas.
- Marcar tareas como completadas y reabrirlas.
- Filtrar la lista por todas, pendientes o completadas.
- Asignar tareas a usuarios de ejemplo precargados.
- Establecer prioridad baja, media o alta.
- Conservar los cambios en una base de datos SQLite entre sesiones.

La asignación indica quién es responsable de una tarea, pero el MVP no incluye cuentas, autenticación, autorización, privacidad por usuario, colaboración en tiempo real ni despliegue multiusuario.

## Tecnologías previstas

- **Backend:** ASP.NET Core 10.
- **Frontend:** React.
- **Acceso a datos:** Entity Framework Core.
- **Base de datos:** SQLite.
- **Análisis de calidad:** SonarQube con reglas locales básicas.

La herramienta de construcción de React, los frameworks de pruebas y la configuración concreta de SonarQube están pendientes de decisión.

## Calidad y verificación

El desarrollo incluirá pruebas automatizadas para las operaciones y reglas principales, además de una prueba manual guiada del flujo de tareas. También se ejecutará SonarQube y se revisarán sus hallazgos. El análisis estático complementa las pruebas; no las sustituye.

## Trabajo con GitHub Copilot

El proyecto se desarrollará en incrementos pequeños. Antes de aceptar una propuesta de código, se revisará el plan y el diff; cada comportamiento se comprobará mediante sus pruebas o la prueba manual correspondiente.

## Plan inicial

1. Acordar la estructura del proyecto y las herramientas de desarrollo y pruebas.
2. Definir los modelos de tarea y usuario de ejemplo y configurar SQLite con Entity Framework Core.
3. Implementar una primera funcionalidad vertical: crear y listar tareas desde la API hasta la base de datos.
4. Añadir edición, eliminación y cambio de estado, con sus pruebas.
5. Incorporar filtros, asignación de responsables y prioridades.
6. Construir la interfaz React y conectarla con la API.
7. Ejecutar pruebas automatizadas, completar la prueba manual y revisar SonarQube.

## Documentación

- [Análisis del MVP](docs/analisis.md): requisitos funcionales, historias de usuario, criterios de aceptación y decisiones pendientes.
- [Plan del proyecto](docs/plan-proyecto.md): fases, tareas, entregables y verificaciones propuestas.
- [Arquitectura y modelo de datos](docs/arquitectura.md): diagramas Mermaid de arquitectura y ERD, responsabilidades y decisiones pendientes.
- [Manual de usuario](docs/manual-usuario.md): guía inicial de los flujos previstos y límites del MVP.
- [Guía de desarrollo e instalación](docs/guia-desarrollo.md): requisitos del entorno, convenciones locales, pruebas y decisiones pendientes.
