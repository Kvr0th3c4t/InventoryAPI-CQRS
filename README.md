# 📦 InventoryAPI - Sistema de Gestión de Inventario

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-9.0-512BD4)](https://docs.microsoft.com/en-us/aspnet/core/)
[![Docker](https://img.shields.io/badge/Docker-Enabled-2496ED?logo=docker)](https://www.docker.com/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?logo=microsoft-sql-server)](https://www.microsoft.com/sql-server)
[![Tests](https://img.shields.io/badge/Tests-25%20Passing-success)](https://github.com/Kvr0th3c4t/InventoryAPI-CQRS)

> API RESTful moderna para gestión de inventarios implementada con **CQRS Pattern**, **MediatR**, **Repository Pattern** y **Docker**. Proyecto educativo demostrando arquitectura limpia y buenas prácticas en ASP.NET Core.

---

## 📋 Tabla de Contenidos

- [Características Principales](#-características-principales)
- [Arquitectura y Patrones](#️-arquitectura-y-patrones)
- [Tecnologías Utilizadas](#-tecnologías-utilizadas)
- [Modelo de Dominio](#-modelo-de-dominio)
- [Instalación y Configuración](#-instalación-y-configuración)
- [Uso con Docker](#-uso-con-docker)
- [Ejecutar Tests](#-ejecutar-tests)
- [Documentación de API](#-documentación-de-api)
- [Endpoints Disponibles](#-endpoints-disponibles)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Roadmap](#-roadmap)

---

## ✨ Características Principales

- ✅ **CQRS con MediatR** - Separación completa de Commands y Queries
- ✅ **Repository Pattern** - Abstracción de acceso a datos
- ✅ **Event Publisher** - Sistema de eventos de dominio (ej: Stock Bajo)
- ✅ **Docker Ready** - Dockerfile multi-stage optimizado
- ✅ **Integration Tests** - 25 tests con Testcontainers
- ✅ **Entity Framework Core** - Code-First con Migrations
- ✅ **Swagger/OpenAPI** - Documentación automática de API
- ✅ **DTOs separados** - Request/Response con validación
- ✅ **Manejo de excepciones** - Códigos HTTP apropiados
- ✅ **Datos persistentes** - Volúmenes Docker para SQL Server

---

## 🏗️ Arquitectura y Patrones

### **CQRS (Command Query Responsibility Segregation)**

La aplicación separa completamente las operaciones de **lectura** (Queries) y **escritura** (Commands):

```
Features/
├── Productos/
│   ├── Commands/
│   │   ├── CreateProducto/
│   │   │   ├── CreateProductoCommand.cs
│   │   │   └── CreateProductoCommandHandler.cs
│   │   ├── UpdateProducto/
│   │   └── DeleteProducto/
│   └── Queries/
│       ├── GetProductoById/
│       │   ├── GetProductoByIdQuery.cs
│       │   └── GetProductoByIdQueryHandler.cs
│       └── GetAllProductos/
```

**Ventajas:**
- 📊 Optimización independiente de lecturas y escrituras
- 🧩 Código más mantenible y testeable
- 🔍 Separación clara de responsabilidades
- 🚀 Escalabilidad mejorada

### **Patrones Implementados**

| Patrón | Implementación | Beneficio |
|--------|----------------|-----------|
| **CQRS** | MediatR | Separación Commands/Queries |
| **Repository** | Interfaces + EF Core | Abstracción de datos |
| **Event Publisher** | IEventPublisher | Desacoplamiento de eventos |
| **DTO Pattern** | Request/Response DTOs | Validación y mapeo |
| **Dependency Injection** | Built-in DI | Inversión de dependencias |

---

## 🛠️ Tecnologías Utilizadas

### **Backend**
- **ASP.NET Core 9.0** - Framework web
- **C# 12** - Lenguaje de programación
- **Entity Framework Core 9.0** - ORM
- **MediatR 13.1** - Implementación CQRS
- **SQL Server 2022** - Base de datos

### **Testing**
- **xUnit** - Framework de testing
- **Testcontainers** - Contenedores para integration tests
- **FluentAssertions** - Assertions expresivas

### **DevOps**
- **Docker** - Contenedorización
- **Docker Compose** - Orquestación multi-contenedor

---

## 📊 Modelo de Dominio

### **Entidades Principales**

```
┌─────────────┐         ┌──────────────┐
│  Categoria  │◄────────│  Producto    │
└─────────────┘         └──────────────┘
                              │
                              │ 1:N
                              ▼
                        ┌──────────────────┐         ┌─────────────┐
                        │ MovimientoStock  │────────►│  Proveedor  │
                        └──────────────────┘         └─────────────┘
```

### **1. Producto**
```csharp
- Id: int
- Nombre: string
- SKU: string (auto-generado)
- Descripcion: string?
- StockActual: int
- StockMinimo: int
- Precio: decimal
- FechaCreacion: DateTime
- CategoriaId: int (FK)
- ProveedorId: int? (nullable)
```

**Lógica de negocio:**
- SKU se genera automáticamente: `PROD-{GUID}`
- Si `StockActual < StockMinimo` → Dispara evento `StockBajoEvent`

### **2. Categoria**
```csharp
- Id: int
- Nombre: string
- Descripcion: string?
```

### **3. Proveedor**
```csharp
- Id: int
- Nombre: string
- Email: string?
- Telefono: string?
```

### **4. MovimientoStock**
```csharp
- Id: int
- ProductoId: int (FK)
- ProveedorId: int? (FK, nullable)
- Tipo: TipoMovimiento (enum)
- Cantidad: int
- FechaMovimiento: DateTime
- Razon: string?
```

**Tipos de Movimiento:**
- `Entrada` (1) - Suma stock, **requiere ProveedorId**
- `Salida` (2) - Resta stock
- `AjustePositivo` (3) - Suma stock
- `AjusteNegativo` (4) - Resta stock

**Características:**
- ✅ Inmutables (no tienen Update/Delete)
- ✅ Actualizan automáticamente el stock del producto
- ✅ Validación de stock suficiente en salidas

---

## 🚀 Instalación y Configuración

### **Requisitos Previos**

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com/)

### **Clonar el Repositorio**

```bash
git clone https://github.com/Kvr0th3c4t/InventoryAPI-CQRS.git
cd InventoryAPI-CQRS
```

---

## 🐳 Uso con Docker

### **Opción 1: Docker Compose (Recomendado)**

La forma más rápida de ejecutar la aplicación:

```bash
# Arrancar API + SQL Server
docker-compose up

# O en modo detached (segundo plano)
docker-compose up -d
```

**Acceder a la aplicación:**
- API: `http://localhost:5000`
- Swagger: `http://localhost:5000/swagger`
- SQL Server: `localhost:1433`

**Credenciales SQL Server:**
- Usuario: `sa`
- Password: `YourStrong@Passw0rd`

### **Comandos Útiles**

```bash
# Ver logs en tiempo real
docker-compose logs -f

# Ver logs solo de la API
docker-compose logs -f api

# Parar contenedores (conserva datos)
docker-compose down

# Parar y eliminar volúmenes (resetea BBDD)
docker-compose down -v

# Ver estado de contenedores
docker-compose ps

# Reconstruir imágenes
docker-compose up --build
```

### **Opción 2: Ejecución Local (Sin Docker)**

**1. Configurar SQL Server local:**

Editar `src/InventoryAPI/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=InventoryDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**2. Aplicar migrations:**
```bash
cd src/InventoryAPI
dotnet ef database update
```

**3. Ejecutar la API:**
```bash
dotnet run
```

---

## 🧪 Ejecutar Tests

La suite de tests incluye **25 integration tests** usando **Testcontainers** (cada test usa su propio SQL Server temporal).

### **Ejecutar todos los tests:**

```bash
dotnet test
```

**Salida esperada:**
```
Resumen de pruebas: total: 25; con errores: 0; correcto: 25; omitido: 0
```

### **Ejecutar tests por entidad:**

```bash
# Solo tests de Productos (7 tests)
dotnet test --filter ProductoTests

# Solo tests de Categorías (5 tests)
dotnet test --filter CategoriaTests

# Solo tests de Proveedores (6 tests)
dotnet test --filter ProveedorTests

# Solo tests de MovimientosStock (7 tests)
dotnet test --filter MovimientoStockTests
```

### **Tests con más detalle:**

```bash
dotnet test --logger "console;verbosity=detailed"
```

### **Cobertura de Tests**

| Entidad | Tests | Cobertura |
|---------|-------|-----------|
| **Productos** | 7 | ✅ CRUD + Validaciones + SKU |
| **Categorías** | 5 | ✅ CRUD Completo |
| **Proveedores** | 6 | ✅ CRUD + Validación integridad |
| **MovimientosStock** | 7 | ✅ Create + Queries + Validaciones |
| **TOTAL** | **25** | **100%** |

---

## 📚 Documentación de API

### **Swagger UI**

La documentación interactiva está disponible en:

```
http://localhost:5000/swagger
```

Desde Swagger puedes:
- 📖 Ver todos los endpoints disponibles
- 🧪 Probar cada endpoint directamente
- 📋 Ver los modelos de Request/Response
- 🔍 Explorar los códigos de respuesta HTTP

---

## 📡 Endpoints Disponibles

### **🏷️ Categorías**

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/Categoria` | Obtener todas las categorías |
| `GET` | `/api/Categoria/{id}` | Obtener categoría por ID |
| `POST` | `/api/Categoria` | Crear nueva categoría |
| `PUT` | `/api/Categoria/{id}` | Actualizar categoría |
| `DELETE` | `/api/Categoria/{id}` | Eliminar categoría |

**Ejemplo - Crear Categoría:**
```http
POST /api/Categoria
Content-Type: application/json

{
  "nombre": "Electrónica",
  "descripcion": "Productos electrónicos y tecnológicos"
}
```

**Respuesta:**
```json
{
  "id": 1,
  "nombre": "Electrónica",
  "descripcion": "Productos electrónicos y tecnológicos"
}
```

---

### **📦 Productos**

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/Productos` | Obtener todos los productos |
| `GET` | `/api/Productos/{id}` | Obtener producto por ID |
| `POST` | `/api/Productos` | Crear nuevo producto |
| `PUT` | `/api/Productos/{id}` | Actualizar producto |
| `DELETE` | `/api/Productos/{id}` | Eliminar producto |

**Ejemplo - Crear Producto:**
```http
POST /api/Productos
Content-Type: application/json

{
  "nombre": "Laptop Dell XPS 15",
  "descripcion": "Laptop de alta gama para desarrollo",
  "categoriaId": 1,
  "stockActual": 10,
  "stockMinimo": 5,
  "precio": 1499.99
}
```

**Respuesta:**
```json
{
  "id": 1,
  "nombre": "Laptop Dell XPS 15",
  "descripcion": "Laptop de alta gama para desarrollo",
  "sku": "PROD-A3F5B2C1",
  "categoriaId": 1,
  "categoriaNombre": "Electrónica",
  "stockActual": 10,
  "precio": 1499.99
}
```

---

### **🚚 Proveedores**

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/Proveedor` | Obtener todos los proveedores |
| `GET` | `/api/Proveedor/{id}` | Obtener proveedor por ID |
| `POST` | `/api/Proveedor` | Crear nuevo proveedor |
| `PUT` | `/api/Proveedor/{id}` | Actualizar proveedor |
| `DELETE` | `/api/Proveedor/{id}` | Eliminar proveedor |

**Ejemplo - Crear Proveedor:**
```http
POST /api/Proveedor
Content-Type: application/json

{
  "nombre": "Tech Suppliers Inc.",
  "email": "ventas@techsuppliers.com",
  "telefono": "+34 912 345 678"
}
```

---

### **📊 Movimientos de Stock**

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/MovimientosStock` | Obtener todos los movimientos |
| `GET` | `/api/MovimientosStock/{id}` | Obtener movimiento por ID |
| `POST` | `/api/MovimientosStock` | Registrar nuevo movimiento |

**Ejemplo - Entrada de Stock:**
```http
POST /api/MovimientosStock
Content-Type: application/json

{
  "productoId": 1,
  "proveedorId": 1,
  "tipo": 1,
  "cantidad": 50,
  "razon": "Compra mensual"
}
```

**Tipos de Movimiento:**
- `1` - Entrada (requiere ProveedorId)
- `2` - Salida
- `3` - Ajuste Positivo
- `4` - Ajuste Negativo

**Respuesta:**
```json
{
  "id": 1,
  "productoId": 1,
  "productoNombre": "Laptop Dell XPS 15",
  "proveedorId": 1,
  "proveedorNombre": "Tech Suppliers Inc.",
  "tipo": 1,
  "cantidad": 50,
  "razon": "Compra mensual",
  "fecha": "2024-11-18T00:15:30"
}
```

---

## 📁 Estructura del Proyecto

```
InventoryAPI-CQRS/
├── src/
│   └── InventoryAPI/
│       ├── Controllers/           # Controladores API
│       ├── Features/             # CQRS - Commands y Queries
│       │   ├── Productos/
│       │   ├── Categorias/
│       │   ├── Proveedores/
│       │   └── MovimientosStock/
│       ├── DTOs/                 # Data Transfer Objects
│       ├── Models/               # Entidades de dominio
│       ├── Repositories/         # Repository Pattern
│       ├── Events/               # Event Publisher
│       ├── Data/                 # DbContext
│       ├── Migrations/           # EF Core Migrations
│       └── Program.cs
├── tests/
│   └── InventoryAPI.IntegrationTests/
│       ├── Infrastructure/       # TestBase con Testcontainers
│       └── Tests/               # Tests por entidad
├── Dockerfile                    # Multi-stage build
├── docker-compose.yml           # Orquestación
├── .dockerignore
└── README.md
```

---

## 🔒 Decisiones de Diseño

### **1. CQRS sin Service Layer**

Se eliminó la capa de servicios tradicional en favor de **Handlers dedicados** para cada comando/query. Esto proporciona:
- Mayor cohesión
- Código más testeable
- Separación clara de responsabilidades

### **2. Repository Pattern con EF Core**

Aunque EF Core ya implementa Unit of Work y Repository, se mantienen repositorios explícitos para:
- Facilitar testing con mocks
- Abstracción adicional sobre EF Core
- Posible cambio de ORM en el futuro

### **3. DTOs Separados**

- **Request DTOs**: Validación de entrada
- **Response DTOs**: Control de datos expuestos
- **Beneficio**: Desacoplamiento entre modelo de dominio y API

### **4. Event Publisher Abstracto**

```csharp
public interface IEventPublisher
{
    void Publish<T>(T evento) where T : class;
}
```

Diseño **plug & play** que permite cambiar la implementación sin modificar handlers:
- Actual: `ConsoleEventPublisher` (desarrollo)
- Futuro: Azure Service Bus, RabbitMQ, etc.

### **5. MovimientosStock Inmutables**

Los movimientos NO tienen operaciones de Update/Delete para mantener:
- ✅ Integridad del historial
- ✅ Auditoría completa
- ✅ Trazabilidad de cambios

---

## 🎯 Validaciones Implementadas

### **Productos**
- ✅ Precio no puede ser negativo
- ✅ Stock actual no puede ser negativo
- ✅ Stock mínimo no puede ser negativo
- ✅ Categoría debe existir
- ✅ SKU se genera automáticamente

### **MovimientosStock**
- ✅ Cantidad debe ser > 0
- ✅ Producto debe existir
- ✅ Entradas requieren ProveedorId obligatorio
- ✅ Salidas verifican stock suficiente
- ✅ Proveedor debe existir (si se proporciona)

### **Proveedores**
- ✅ No se puede eliminar si tiene movimientos asociados

---

## 📈 Roadmap

### **Fase Actual - Completado ✅**
- [x] Implementación CQRS con MediatR
- [x] Repository Pattern
- [x] Integration Tests (25 tests)
- [x] Dockerización completa
- [x] Event Publisher

### **Próximas Mejoras**

#### **v2.0 - Vertical Slice Architecture**
- [ ] Migración a Wolverine (sucesor de MediatR)
- [ ] Organización por features verticales
- [ ] Minimal APIs

#### **v2.1 - Seguridad**
- [ ] Autenticación JWT
- [ ] Autorización basada en roles
- [ ] Rate limiting

#### **v2.2 - Observabilidad**
- [ ] Logging estructurado (Serilog)
- [ ] Health checks
- [ ] Métricas con Prometheus

#### **v2.3 - Cache**
- [ ] Redis para cache distribuido
- [ ] Cache de queries frecuentes

---

## 🤝 Contribuciones

Este es un proyecto educativo personal, pero sugerencias y feedback son bienvenidos.

---

## 👨‍💻 Autor

**Adrián** - [GitHub](https://github.com/Kvr0th3c4t)

Desarrollador Full-Stack con experiencia en:
- ASP.NET Core & Angular
- CQRS & Clean Architecture
- Docker & Azure
- Testing & TDD

---

## 📄 Licencia

Este proyecto es de código abierto y está disponible bajo la licencia MIT.

---

## 🙏 Agradecimientos

Este proyecto fue desarrollado como parte de mi aprendizaje continuo en arquitecturas modernas de backend y mejores prácticas en .NET.

---

**⭐ Si este proyecto te resultó útil, considera darle una estrella en GitHub!**
