# ??? SEKAI POS - Sistema de Punto de Venta

![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-14.0-239120?logo=csharp)
![Windows](https://img.shields.io/badge/Windows-10%2F11-0078D6?logo=windows)
![SQLite](https://img.shields.io/badge/SQLite-3-003B57?logo=sqlite)
![License](https://img.shields.io/badge/License-MIT-green)

Sistema moderno de Punto de Venta diseñado específicamente para tiendas de tecnología, desarrollado con .NET 10 y Windows Forms.

![SEKAI POS Preview](https://via.placeholder.com/800x450/0F0F0F/00FF7F?text=SEKAI+POS+v1.0)

---

## ?? Tabla de Contenidos

- [Características](#-características)
- [Tecnologías](#?-tecnologías-utilizadas)
- [Instalación](#-instalación)
- [Uso Rápido](#-uso-rápido)
- [Módulos del Sistema](#-módulos-del-sistema)
- [Capturas de Pantalla](#-capturas-de-pantalla)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Base de Datos](#?-base-de-datos)
- [Roadmap](#-roadmap)
- [Contribución](#-contribución)
- [Licencia](#-licencia)
- [Contacto](#-contacto)

---

## ? Características

### ?? Autenticación y Seguridad
- Sistema de login con roles (Administrador/Vendedor)
- Gestión de usuarios y permisos
- Cambio de contraseñas seguro
- Validaciones de entrada

### ?? Gestión de Inventario
- CRUD completo de productos
- Búsqueda en tiempo real
- Soporte para códigos de barras
- Categorización de productos
- Control de stock automático

### ?? Punto de Venta
- Interfaz intuitiva de venta
- Cálculo automático de IVA (configurable)
- Múltiples métodos de pago
- Generación de boletas
- Actualización automática de inventario

### ?? Reportes y Estadísticas
- Dashboard con métricas en tiempo real
- Reportes de ventas con filtros por fecha
- Exportación a CSV
- Estadísticas de productos más vendidos
- Análisis de ticket promedio

### ?? Configuración
- Personalización de datos de la tienda
- Gestión de usuarios
- Configuración de impuestos
- Temas visuales (Dark Theme por defecto)

---

## ??? Tecnologías Utilizadas

| Tecnología | Versión | Propósito |
|-----------|---------|-----------|
| .NET | 10 | Framework principal |
| C# | 14.0 | Lenguaje de programación |
| Windows Forms | 10 | Interfaz de usuario |
| SQLite | 3.x | Base de datos local |
| FontAwesome.Sharp | 6.3.0 | Iconografía |
| Microsoft.Data.Sqlite | 8.x | Conector de BD |

---

## ?? Instalación

### Requisitos Previos

- Windows 10 o superior (64-bit)
- .NET 10 Runtime ([Descargar](https://dotnet.microsoft.com/download/dotnet/10.0))
- 2GB RAM mínimo (4GB recomendado)
- 100MB de espacio en disco

### Método 1: Ejecutable Pre-compilado

1. Descarga la última versión desde [Releases](https://github.com/Johann3421/SekaiPOS_1.0/releases)
2. Extrae el archivo ZIP
3. Ejecuta `SekaiPOS_1.0.exe`

### Método 2: Compilar desde Código Fuente

```bash
# Clonar el repositorio
git clone https://github.com/Johann3421/SekaiPOS_1.0.git

# Navegar al directorio
cd SekaiPOS_1.0/SekaiPOS_1.0

# Restaurar paquetes NuGet
dotnet restore

# Compilar el proyecto
dotnet build --configuration Release

# Ejecutar
dotnet run
```

---

## ?? Uso Rápido

### 1. Primer Inicio

Al ejecutar la aplicación por primera vez:
- **Usuario:** `admin`
- **Contraseña:** `admin123`

?? **Importante:** Cambia la contraseña del administrador después del primer login.

### 2. Flujo Básico de Venta

```
1. Login ? Dashboard
2. Ir a Ventas
3. Buscar producto (por nombre o código de barras)
4. Agregar al carrito
5. Seleccionar método de pago
6. Completar venta
7. Generar boleta
```

### 3. Gestionar Inventario

```
1. Login ? Dashboard
2. Ir a Inventario
3. Click en "Agregar" para nuevo producto
4. Llenar formulario (Nombre, Precio, Stock, etc.)
5. Guardar
```

---

## ?? Módulos del Sistema

### 1. ?? Dashboard
- Tarjetas con estadísticas en tiempo real
- Total de productos en stock
- Ventas del día
- Promedio de venta
- Navegación rápida a otros módulos

### 2. ?? Inventario
- Listar todos los productos
- Búsqueda y filtrado
- Agregar nuevos productos
- Editar productos existentes
- Eliminar productos
- Gestión de códigos de barras

### 3. ?? Ventas
- Catálogo de productos disponibles
- Carrito de compras
- Cálculo automático de IVA
- Múltiples métodos de pago
- Generación de boletas
- Actualización automática de stock

### 4. ?? Reportes
- Estadísticas generales
- Historial de ventas
- Filtros por rango de fechas
- Exportación a CSV
- Producto más vendido

### 5. ?? Configuración
- **General:** Datos de la tienda (nombre, dirección, teléfono, IVA)
- **Usuarios:** Agregar, eliminar, cambiar contraseñas
- **Apariencia:** Información del tema
- **Acerca de:** Información del sistema

---

## ?? Capturas de Pantalla

### Login
```
??????????????????????????????
?     ?? SEKAI POS Login     ?
?                            ?
?  Usuario: [_____________] ?
?  Pass:    [_____________] ?
?                            ?
?      [Iniciar Sesión]      ?
??????????????????????????????
```

### Dashboard
```
????????????????????????????????????????????????
?       ?  ?? Dashboard                        ?
? Menu  ?                                      ?
?       ?  Bienvenido, admin!                  ?
? ?Dash ?                                      ?
?  Inv  ?  ???????? ???????? ???????? ?????????
?  Ven  ?  ?  20  ? ?$1,500? ?  15  ? ? $100 ??
?  Rep  ?  ?Prods ? ?Ventas? ?Trans ? ?Prom  ??
?  Cfg  ?  ???????? ???????? ???????? ?????????
????????????????????????????????????????????????
```

### Punto de Venta
```
?????????????????????????????????????
?  Productos      ?  Carrito        ?
?                 ?                 ?
? ??[Buscar...]   ? Mouse    x2 $100?
?                 ? Teclado  x1 $80 ?
? ??????????????? ?                 ?
? ?Mouse   $50  ? ? Subtotal: $180  ?
? ?Teclado $80  ? ? IVA 16%:  $28.8 ?
? ?Monitor $350 ? ? TOTAL:    $208.8?
? ??????????????? ?                 ?
?                 ? [Completar Venta]?
?????????????????????????????????????
```

---

## ?? Estructura del Proyecto

```
SekaiPOS_1.0/
??? SekaiPOS_1.0/
?   ??? Program.cs                      # Punto de entrada
?   ??? DatabaseHelper.cs               # Gestión de BD
?   ??? CurrentUser.cs                  # Estado del usuario
?   ?
?   ??? Forms/
?   ?   ??? LoginForm.cs               # Autenticación
?   ?   ??? MainDashboardFinal.cs      # Dashboard principal
?   ?   ??? InventoryFormFinal.cs      # Gestión de inventario
?   ?   ??? SalesFormFixed.cs          # Punto de venta
?   ?   ??? ReportsForm.cs             # Reportes
?   ?   ??? SettingsFormFunctional.cs  # Configuración
?   ?   ??? ProductEditForm.cs         # Edición de productos
?   ?   ??? ReceiptForm.cs             # Boleta de venta
?   ?
?   ??? pos.db                          # Base de datos SQLite
?
??? VALIDATION_CHECKLIST.md             # Lista de validación
??? USER_MANUAL.md                      # Manual de usuario
??? README.md                           # Este archivo
??? LICENSE                             # Licencia MIT
```

---

## ??? Base de Datos

### Esquema de Tablas

#### Products
```sql
CREATE TABLE Products (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Description TEXT,
    Price REAL NOT NULL,
    Quantity INTEGER NOT NULL DEFAULT 0,
    Category TEXT,
    Barcode TEXT
);
```

#### Users
```sql
CREATE TABLE Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE,
    Password TEXT NOT NULL,
    IsAdmin INTEGER NOT NULL DEFAULT 0
);
```

#### Sales
```sql
CREATE TABLE Sales (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    SaleDate TEXT NOT NULL,
    Total REAL NOT NULL,
    Username TEXT NOT NULL,
    PaymentMethod TEXT
);
```

#### SaleItems
```sql
CREATE TABLE SaleItems (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    SaleId INTEGER NOT NULL,
    ProductId INTEGER NOT NULL,
    ProductName TEXT NOT NULL,
    Quantity INTEGER NOT NULL,
    Price REAL NOT NULL,
    Subtotal REAL NOT NULL,
    FOREIGN KEY (SaleId) REFERENCES Sales(Id)
);
```

#### Settings
```sql
CREATE TABLE Settings (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    StoreName TEXT NOT NULL DEFAULT 'SEKAI Tech Store',
    Address TEXT NOT NULL DEFAULT 'Av. Principal #123',
    Phone TEXT NOT NULL DEFAULT '+1 234 567 8900',
    TaxRate REAL NOT NULL DEFAULT 0.16,
    Theme TEXT NOT NULL DEFAULT 'Dark',
    AccentColor TEXT NOT NULL DEFAULT '#00FF7F'
);
```

### Backup de Base de Datos

Para hacer backup de tus datos:
1. Cierra la aplicación
2. Copia el archivo `pos.db`
3. Guárdalo en un lugar seguro

Para restaurar:
1. Reemplaza el archivo `pos.db` con tu backup
2. Inicia la aplicación

---

## ??? Roadmap

### Versión 1.1 (Próximamente)
- [ ] Impresión directa en impresoras térmicas
- [ ] Soporte para escáner de código de barras USB
- [ ] Sistema de descuentos y promociones
- [ ] Gráficas avanzadas en reportes
- [ ] Temas de color personalizables

### Versión 2.0 (Futuro)
- [ ] Arquitectura cliente-servidor
- [ ] Múltiples terminales sincronizadas
- [ ] Base de datos SQL Server
- [ ] API REST para integraciones
- [ ] Aplicación móvil complementaria
- [ ] Facturación electrónica

### Versión 3.0 (Largo Plazo)
- [ ] Sistema en la nube
- [ ] E-commerce integrado
- [ ] CRM para clientes
- [ ] Inteligencia artificial para predicción de ventas
- [ ] Sistema de fidelización de clientes

---

## ?? Contribución

¡Las contribuciones son bienvenidas! Si deseas contribuir:

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add: Amazing Feature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

### Guía de Estilo
- Usa nombres descriptivos para variables y métodos
- Comenta código complejo
- Sigue las convenciones de C#
- Prueba tu código antes de hacer commit

### Reporte de Bugs
Crea un Issue incluyendo:
- Descripción del problema
- Pasos para reproducir
- Comportamiento esperado vs actual
- Capturas de pantalla (si aplica)
- Información del sistema

---

## ?? Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo [LICENSE](LICENSE) para más detalles.

```
MIT License

Copyright (c) 2025 Johann3421

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

---

## ?? Contacto

**Desarrollador:** Johann3421

**GitHub:** [@Johann3421](https://github.com/Johann3421)

**Repositorio:** [https://github.com/Johann3421/SekaiPOS_1.0](https://github.com/Johann3421/SekaiPOS_1.0)

**Issues:** [Reportar un problema](https://github.com/Johann3421/SekaiPOS_1.0/issues)

---

## ?? Agradecimientos

- [FontAwesome](https://fontawesome.com/) por los increíbles iconos
- [Microsoft](https://dotnet.microsoft.com/) por .NET Framework
- [SQLite](https://www.sqlite.org/) por la base de datos embebida
- La comunidad de C# y .NET

---

## ?? Estadísticas del Proyecto

![GitHub stars](https://img.shields.io/github/stars/Johann3421/SekaiPOS_1.0?style=social)
![GitHub forks](https://img.shields.io/github/forks/Johann3421/SekaiPOS_1.0?style=social)
![GitHub watchers](https://img.shields.io/github/watchers/Johann3421/SekaiPOS_1.0?style=social)

![GitHub last commit](https://img.shields.io/github/last-commit/Johann3421/SekaiPOS_1.0)
![GitHub repo size](https://img.shields.io/github/repo-size/Johann3421/SekaiPOS_1.0)
![GitHub language count](https://img.shields.io/github/languages/count/Johann3421/SekaiPOS_1.0)
![GitHub top language](https://img.shields.io/github/languages/top/Johann3421/SekaiPOS_1.0)

---

<div align="center">

**? Si te gusta este proyecto, considera darle una estrella ?**

**Hecho con ?? y C#**

[Volver arriba](#?-sekai-pos---sistema-de-punto-de-venta)

</div>
