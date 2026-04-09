# Prueba Técnica – API de Pacientes

Este proyecto es una API básica para la gestión de pacientes. Se desarrolló aplicando una estructura inspirada en arquitectura limpia, buscando mantener una buena organización y separación de responsabilidades.

Decidí estructurar el proyecto en varias capas:

* Domain
* Application
* Infrastructure
* Presentation (API)(PruebaTecnica)

La idea principal fue desacoplar la lógica y que cada capa tenga una responsabilidad clara.

---

## Decisiones de diseño

Para este caso opté por usar servicios en la capa de Application en lugar de implementar casos de uso (CQRS). Esto porque el problema es relativamente simple, aunque sé que en escenarios más grandes lo ideal sería usar commands y queries.

---

## Organización del proyecto

### Domain

Aquí se definieron:

* Entidades
* Excepciones personalizadas
* Interfaces de repositorios

Esta capa no depende de ninguna otra.

---

### Application

Contiene la lógica de negocio:

* Servicios e interfaces
* Validaciones con FluentValidation

Las validaciones se usan para evitar errores como:

* Formatos incorrectos en documentos
* Inconsistencias entre edad y tipo de documento
* Campos obligatorios vacíos

---

### Infrastructure

Aquí está toda la implementación técnica:

* Repositorios
* DbContext
* Configuración de entidades en OnModelCreating

También se implementó un procedimiento almacenado para el reporte de pacientes.
Este procedimiento se agregó mediante una migración vacía para mantenerlo versionado dentro del proyecto.

---

### Presentation (API)(PruebaTecnica)

Contiene los controllers:

* Manejo de endpoints HTTP
* Respuestas del sistema
* Comunicación con los servicios

---

## Manejo de errores

Se implementó un middleware global para el manejo de excepciones.

Este middleware:

* Captura errores
* Maneja las excepciones de validación
* Retorna respuestas HTTP adecuadas

De esta forma se evita usar try-catch en los controllers y se mantiene el código más limpio.

---

## Tests

Se creó un proyecto de pruebas donde se validan los principales comportamientos de los controllers.

Los tests se enfocan en asegurar que las respuestas sean las esperadas en distintos escenarios.

---

## Cómo ejecutar el proyecto

1. Clonar el repositorio

git clone https://github.com/AbrahamBass/prueba-tecnica-back.git

2. Entrar a la carpeta del proyecto

cd prueba-tecnica-back

3. Restaurar dependencias

dotnet restore

4. Aplicar migraciones

dotnet ef database update

5. Ejecutar el proyecto

dotnet run

---

## Notas

* Se utilizó .NET 10
* La configuración de la base de datos se encuentra en appsettings.Development.json

---

## Ejecución con Docker

Si no deseas clonar este repositorio, también puedes usar directamente la imagen publicada en Docker Hub:

docker.io/abrahambass/asp-prueba-tecnica

Adicionalmente, se dejó un repositorio separado con un docker-compose ya configurado que levanta todo el entorno (API + base de datos) listo para usar:

https://github.com/AbrahamBass/prueba-tecnica-compose.git

En ese repositorio encontrarás:

El archivo docker-compose.yml
La configuración necesaria para la base de datos
Los pasos para levantar el entorno completo
Cómo usarlo
Clonar el repositorio del compose:

git clone https://github.com/AbrahamBass/prueba-tecnica-compose.git

Entrar a la carpeta:

cd prueba-tecnica-compose

Ejecutar:

docker-compose up

Con esto se levantará automáticamente:

La API
La base de datos
Todo listo para probar el sistema sin configuraciones adicionales

## Ejecutar solo la API

También puedes probar unicamente la API usando la imagen en Docker Hub:

docker.io/abrahambass/asp-prueba-tecnica

Pasos:

docker pull abrahambass/asp-prueba-tecnica
docker run -p 8080:8080 abrahambass/asp-prueba-tecnica

Luego puedes acceder desde:

http://localhost:8080

---

## Consideraciones finales

Se buscó mantener el código lo más claro y organizado posible, aplicando buenas prácticas sin sobrecomplicar la solución.

La idea fue dejar una base limpia que pueda escalar fácilmente en caso de necesitar más funcionalidades.
