# BackCoding Challenge - Backend

## 🚀 Descripción

El proyecto **BackCoding Challenge** implementa una API REST desarrollada en .NET 8 (Clean Architecture) con base de datos PostgreSQL. Su objetivo es gestionar clientes, productos, sucursales y suscripciones, además de generar reportes consolidados mediante una función SQL optimizada.

### Funcionalidades

- Suscribirse a un fondo (apertura)
- Cancelar suscripciones
- Consultar historial de transacciones
- Validación de monto mínimo y saldo del cliente
- Notificaciones vía Email/SMS (simulado)
- Testing unitario e integración

Se implementa siguiendo **Clean Architecture**, **CQRS con MediatR**, y principios **SOLID**.

## 🧩 Arquitectura y Decisiones de Diseño

### Capas y Propósito

- **Domain**: Entidades y lógica de negocio pura (Balance, reglas de suscripción, transacciones)
- **Application**: 
  - Comandos/Queries CQRS con MediatR  
  - DTOs y Validators (FluentValidation)  
  - Interfaces de servicios externos (INotificationService)
- **Infrastructure**:  
  - Persistencia EF Core con PostgreSQL  
  - Repositorios genéricos y UnitOfWork  
  - Servicios externos simulados (Email/SMS)
- **WebApi**:  
  - Endpoints REST  
  - Configuración de Serilog, Swagger, DI y CORS

### Decisiones clave

- **CQRS**: Separa comandos (Write) de queries (Read) para claridad y escalabilidad.  
- **Strategy Pattern**: Para envío de notificaciones (Email/SMS).  
- **EF Core + Repository + UnitOfWork**: Facilita testeo e integración con DbContext.  
- **Testing**: xUnit + Moq + InMemoryDatabase para tests unitarios e integración.  


## 🧩 Stack Tecnológico

| Componente       | Tecnología                                   |
|------------------|----------------------------------------------|
| Backend          | .NET 8 (ASP.NET Core Web API)                |
| ORM              | Entity Framework Core 8                      |
| SQL Dinámico     | Dapper                                       |
| BD               | PostgreSQL 16                                |
| Auth             | JWT Bearer + Roles                           |
| Infraestructura  | Docker + Terraform (AWS ECS + RDS)           |
| Logging          | Serilog + AWS CloudWatch                     |
| Tests            | xUnit + Moq                                  |



## 🧪 Endpoints Principales

- **POST** /api/subscriptions → Suscribir cliente a un fondo  
- **POST** /api/subscriptions/cancel → Cancelar suscripción  
- **GET** /api/transactions → Historial de transacciones  

### Ejemplo de request

**Subscribe:**
json
{
  "clientId": 1,
  "productId": 2,
  "amount": 150000
}

**Cancel:**

{
  "clientId": 1,
  "productId": 2
}

## 🧪 Cómo Probar la API

**Autenticación**

Para registrarte con el rol indicado (Administrador o Cliente), puedes enviar una solicitud:

**Registrar:**


POST http://localhost:8080/api/Auth/register

{
  "username": "alexander",
  "password": "123456",
  "role": "Administrador"
}

Luego inicias sesión:

POST http://localhost:8080/api/Auth/login

{
  "username": "alexander",
  "password": "123456"
}

Copia el token y agrégalo en la colección para autorización y puedes probar los demás endpoints.


## Levantar Docker
Para levantar el contenedor de PostgreSQL y la aplicación, utiliza los siguientes comandos en la consola:


# Navegar al directorio del proyecto
cd D:\Proyectos\Consulting\technical-test\backend

# Levantar el contenedor
docker compose up --build

# Para acceder a la consola de PostgreSQL
docker exec -it postgres-btg psql -U postgres -d DB_BackCodingChallenge

## Consultar Funciones SQL
Para ejecutar la función SQL que obtiene clientes y productos por sucursal, usa el siguiente comando en la consola de PostgreSQL:

SELECT * FROM fn_clientes_productos_sucursales();

## Inserciones de Clientes y Productos
Si deseas insertar manualmente más clientes y productos, puedes utilizar los siguientes comandos:

**Insertar Clientes:**

INSERT INTO cliente (nombre, apellidos, ciudad, balance, telefono) VALUES
('Roberth', 'Ortiz', 'Bogota', 500000, '3108889999'),
('Camila', 'Ramirez', 'Medellin', 500000, '3112223344'),
('Daniel', 'Garcia', 'Cali', 500000, '3125556677');

**Insertar Productos:**

INSERT INTO producto (nombre, tipo_producto, min_amount) VALUES
('FPV_BTG_PACTUAL_RECAUDADORA', 'FPV', 75000),
('FPV_BTG_PACTUAL_ECOPETROL', 'FPV', 125000),
('DEUDAPRIVADA', 'FIC', 50000),
('FDO_ACCIONES', 'FIC', 250000),
('FPV_BTG_PACTUAL_DINAMICA', 'FPV', 100000);

**Instrucciones para Terraform**

Para desplegar la infraestructura en AWS con Terraform, sigue estos pasos:

cd infra/terraform

terraform init
terraform plan
terraform apply

## Colección de Postman
Para facilitar la prueba de los endpoints, se incluye una colección de Postman. Puedes importar el archivo Postman_Collection.json que se encuentra en el directorio raíz del proyecto para tener acceso a todos los endpoints y ejemplos de solicitudes.

## ⚙️ Configuración

appsettings.json: ConnectionStrings y configuración de Serilog
Serilog: Logging a consola y archivo
FluentValidation: Valida comandos antes de MediatR
CORS: Permite AllowAnyOrigin para pruebas

## ✅ Testing
Ejecutar desde la raíz del proyecto:

dotnet test

## 📝 Notas finales

Lógica de negocio central en Domain/Entities
Reglas de validación en Application/Validators
Patrón Strategy para envío de notificaciones
Arquitectura modular y escalable para agregar más servicios en el futuro

## ⚙️ Configuración Local

**Clonar el repositorio:**

https://github.com/AlexanderROrtiz/BackCoding-Challenge.git
cd BackCoding-Challenge/backend

**Levantar contenedores:**

docker-compose up --build

**Una vez completado el build, abre:**
👉 http://localhost:8080/swagger

## 📊 Endpoints Principales

## 🔐 Autenticación
**Método	Ruta	Descripción**

POST	/api/Auth/register	Registrar usuario
POST	/api/Auth/login	Autenticar y obtener token JWT

## 👤 Clientes
**Método	Ruta	Descripción**

POST	/api/Client/create	Crear cliente
POST	/api/Client/bulk	Crear múltiples clientes
GET	/api/Client	Listar clientes

## 📦 Productos
**Método	Ruta	Descripción**

POST	/api/Product/create	Crear producto
GET	/api/Product	Listar productos

## 📈 Reportes
**Método	Ruta	Descripción**

GET	/api/Reports/client-products	Reporte consolidado desde función SQL


## ☁️ Despliegue en AWS con Terraform

## Inicializar y aplicar

terraform init
terraform plan
terraform apply

**resultado final con la configuración correcta:**

http://backcoding-api-alb-1746810151.us-east-1.elb.amazonaws.com/swagger


## Build y push de imagen


aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin <aws_account_id>.dkr.ecr.us-east-1.amazonaws.com
docker build -t backcoding-api .
docker tag backcoding-api:latest <aws_account_id>.dkr.ecr.us-east-1.amazonaws.com/backcoding-api:latest
docker push <aws_account_id>.dkr.ecr.us-east-1.amazonaws.com/backcoding-api:latest

## Contacto
Cualquier duda o inquietud sobre el proyecto estare atento al chat en Linkedin: https://www.linkedin.com/in/roberth-ortiz-b526331a9/

¡Gracias por visitar este proyecto!