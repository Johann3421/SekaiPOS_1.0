# ?? MANUAL DE USUARIO - SEKAI POS v1.0

## Sistema de Punto de Venta para Tiendas de Tecnología

---

## ?? TABLA DE CONTENIDOS

1. [Introducción](#introducción)
2. [Inicio de Sesión](#inicio-de-sesión)
3. [Dashboard](#dashboard)
4. [Módulo de Inventario](#módulo-de-inventario)
5. [Módulo de Ventas](#módulo-de-ventas)
6. [Módulo de Reportes](#módulo-de-reportes)
7. [Módulo de Configuración](#módulo-de-configuración)
8. [Preguntas Frecuentes](#preguntas-frecuentes)
9. [Soporte Técnico](#soporte-técnico)

---

## ?? INTRODUCCIÓN

**SEKAI POS** es un sistema moderno de punto de venta diseñado específicamente para tiendas de tecnología. Ofrece una interfaz intuitiva con tema oscuro profesional y todas las funcionalidades necesarias para gestionar tu negocio.

### Características Principales:
- ? Gestión de inventario completa
- ? Punto de venta con cálculo automático de IVA
- ? Reportes y estadísticas en tiempo real
- ? Gestión de usuarios con permisos
- ? Interfaz moderna y fácil de usar

### Requisitos del Sistema:
- Windows 10 o superior
- .NET 10 Runtime (incluido en instalador)
- 2GB RAM mínimo (4GB recomendado)
- 100MB de espacio en disco

---

## ?? INICIO DE SESIÓN

### Paso 1: Ejecutar la Aplicación
1. Doble clic en el icono **SEKAI POS** en el escritorio
2. Espera a que cargue la pantalla de login

### Paso 2: Ingresar Credenciales

**Usuario Administrador por Defecto:**
- **Usuario:** `admin`
- **Contraseña:** `admin123`

**Importante:** ?? Cambia la contraseña del administrador después del primer inicio de sesión.

### Paso 3: Iniciar Sesión
1. Escribe tu usuario
2. Escribe tu contraseña
3. Haz clic en el botón **"Iniciar Sesión"**

### Tipos de Usuarios:
- **Administrador:** Acceso completo a todos los módulos
- **Vendedor:** Acceso a Ventas e Inventario (solo visualización)

---

## ?? DASHBOARD

Al iniciar sesión, verás el **Dashboard** con información importante:

### Tarjetas de Información:

#### 1. Productos en Stock
- Muestra el total de productos registrados
- Color: Azul

#### 2. Ventas Hoy
- Total de dinero vendido hoy
- Color: Verde
- Formato: $X,XXX.XX

#### 3. Total Ventas
- Número total de transacciones
- Color: Naranja

#### 4. Promedio de Venta
- Promedio por transacción
- Color: Púrpura
- Cálculo automático

### Panel Superior:
- **Reloj:** Fecha y hora actual (actualización en tiempo real)
- **Usuario:** Tu nombre de usuario y rol
- **Cerrar Sesión:** Botón rojo para salir

### Menú Lateral (Izquierda):
- Dashboard
- Inventario
- Ventas
- Reportes
- Configuración

---

## ?? MÓDULO DE INVENTARIO

### Ver Productos

Al entrar al módulo, verás una tabla con todos los productos:
- **ID:** Identificador único
- **Nombre:** Nombre del producto
- **Descripción:** Detalles del producto
- **Precio:** Precio de venta
- **Stock:** Cantidad disponible
- **Categoría:** Clasificación del producto
- **Código de Barras:** Para escaneo rápido

### Buscar Productos

En el cuadro de búsqueda (?? Buscar):
1. Escribe el nombre, descripción o categoría
2. La tabla se filtra automáticamente
3. Borra el texto para ver todos los productos

### Agregar Nuevo Producto

1. Haz clic en el botón verde **"Agregar"**
2. Llena el formulario:
   - Nombre (requerido)
   - Descripción
   - Precio (requerido, mayor a 0)
   - Cantidad (stock inicial)
   - Categoría
   - Código de barras
3. Haz clic en **"Guardar"**

**Ejemplo:**
```
Nombre: Laptop Dell XPS 13
Descripción: Intel i7, 16GB RAM, 512GB SSD
Precio: 1299.99
Cantidad: 5
Categoría: Computadoras
Código de Barras: 7501234560001
```

### Editar Producto

1. Selecciona un producto de la tabla (clic en la fila)
2. Haz clic en el botón azul **"Editar"**
3. Modifica los campos que necesites
4. Haz clic en **"Guardar"**

### Eliminar Producto

1. Selecciona un producto de la tabla
2. Haz clic en el botón rojo **"Eliminar"**
3. Confirma la eliminación

?? **Advertencia:** Esta acción no se puede deshacer.

### Actualizar Lista

Haz clic en el botón gris **"Actualizar"** para recargar la lista de productos.

---

## ?? MÓDULO DE VENTAS

### Interfaz de Ventas

La pantalla se divide en dos secciones:

#### Izquierda: Catálogo de Productos
- Lista de todos los productos disponibles
- Cuadro de búsqueda
- Cuadro para código de barras

#### Derecha: Carrito de Compras
- Productos agregados
- Cálculos (Subtotal, IVA, Total)
- Botones de acción

### Realizar una Venta - Paso a Paso

#### 1. Buscar el Producto

**Opción A: Búsqueda por Nombre**
1. Escribe en el cuadro "Buscar"
2. Selecciona el producto de la lista
3. Haz doble clic o usa el botón "Agregar"

**Opción B: Código de Barras**
1. Haz clic en el cuadro "Código"
2. Escanea el código de barras
3. Presiona Enter

#### 2. Especificar Cantidad
1. Usa el control de "Cantidad" (botones + y -)
2. O escribe el número directamente
3. Haz clic en **"Agregar"**

#### 3. Verificar Carrito
- Revisa los productos agregados
- Verifica las cantidades
- Verifica el total

**Para quitar un producto:**
- Selecciona el producto en el carrito
- Haz clic en **"Quitar"** (botón rojo)

**Para vaciar el carrito completo:**
- Haz clic en **"Limpiar"** (botón gris)

#### 4. Seleccionar Método de Pago
- Efectivo
- Tarjeta de Débito
- Tarjeta de Crédito
- Transferencia

#### 5. Completar la Venta
1. Haz clic en **"Completar Venta"** (botón azul)
2. Aparecerá un mensaje de confirmación
3. Se mostrará la boleta

#### 6. Boleta de Venta
La boleta muestra:
- Nombre de la tienda
- Dirección y teléfono
- Fecha y hora
- Productos vendidos
- Subtotal
- IVA (16%)
- Total
- Método de pago

**Opciones:**
- Imprimir boleta
- Cerrar

### Cálculos Automáticos

El sistema calcula automáticamente:
- **Subtotal:** Suma de (Precio × Cantidad) de cada producto
- **IVA:** Subtotal × 16%
- **Total:** Subtotal + IVA

**Ejemplo:**
```
Producto 1: Mouse ($50) × 2 = $100
Producto 2: Teclado ($80) × 1 = $80
---
Subtotal: $180
IVA (16%): $28.80
TOTAL: $208.80
```

### Validaciones de Venta

El sistema NO permitirá:
- ? Vender productos sin stock
- ? Vender más unidades de las disponibles
- ? Completar venta con carrito vacío

---

## ?? MÓDULO DE REPORTES

### Visualizar Reportes

Al entrar al módulo, verás:

#### Tarjetas de Estadísticas:
1. **Total de Ventas:** Número de transacciones
2. **Ingresos Totales:** Dinero total recaudado
3. **Ticket Promedio:** Promedio por venta
4. **Producto Más Vendido:** El que más se vende

#### Tabla de Ventas:
- ID Venta
- Fecha y Hora
- Total
- Usuario que vendió
- Método de Pago

### Filtrar por Fechas

1. Selecciona **"Desde:"** (fecha inicio)
2. Selecciona **"Hasta:"** (fecha fin)
3. Haz clic en **"Filtrar"**
4. Los datos se actualizarán automáticamente

**Por defecto:** Se muestran las ventas del último mes.

### Exportar Reporte a CSV

1. Filtra los datos que quieres exportar
2. Haz clic en **"Exportar a CSV"** (botón verde)
3. Elige dónde guardar el archivo
4. El archivo se abrirá en Excel automáticamente

**Formato del CSV:**
```
ID,Fecha,Total,Usuario,Método de Pago
1,2025-01-15 14:30:00,208.80,admin,Efectivo
2,2025-01-15 15:45:00,599.99,vendedor1,Tarjeta
```

---

## ?? MÓDULO DE CONFIGURACIÓN

?? **Solo usuarios Administradores** tienen acceso a este módulo.

### Tab 1: General

Configuración de la Tienda:

#### Campos Editables:
- **Nombre de la Tienda**
  - Aparece en las boletas
  - Ejemplo: "SEKAI Tech Store"

- **Dirección**
  - Dirección física del negocio
  - Ejemplo: "Av. Principal #123, Ciudad"

- **Teléfono**
  - Número de contacto
  - Ejemplo: "+1 234 567 8900"

- **IVA (%)**
  - Porcentaje de impuesto
  - Por defecto: 16%
  - Rango: 0% - 100%

#### Guardar Cambios:
1. Modifica los campos que necesites
2. Haz clic en **"Guardar Cambios"**
3. Verás un mensaje de confirmación

### Tab 2: Usuarios

Gestión de Usuarios del Sistema:

#### Agregar Usuario:
1. Haz clic en **"Agregar Usuario"** (verde)
2. Llena el formulario:
   - Usuario (único, sin espacios)
   - Contraseña (mínimo 4 caracteres)
   - Confirmar contraseña
   - ? Administrador (marcar si tiene permisos completos)
3. Haz clic en **"Crear"**

**Ejemplo:**
```
Usuario: vendedor1
Contraseña: vend1234
Confirmar: vend1234
? Administrador: No
```

#### Eliminar Usuario:
1. Selecciona un usuario de la tabla
2. Haz clic en **"Eliminar Usuario"** (rojo)
3. Confirma la eliminación

?? **Nota:** No se puede eliminar el usuario 'admin'.

#### Cambiar Contraseña:
1. Haz clic en **"Cambiar Contraseña"** (naranja)
2. Ingresa:
   - Nombre de usuario
   - Nueva contraseña
   - Confirmar contraseña
3. Haz clic en **"Cambiar"**

### Tab 3: Apariencia

Información sobre el tema visual:
- Tema actual: Oscuro (Tech Style)
- Color de acento: Verde Neón (#00FF7F)

*La personalización avanzada estará disponible en futuras versiones.*

### Tab 4: Acerca de

Información del sistema:
- Logo de la aplicación
- Versión: 1.0.0
- Tecnologías utilizadas
- Copyright
- Enlace a GitHub

---

## ? PREGUNTAS FRECUENTES

### 1. ¿Olvidé mi contraseña?

**Para Administradores:**
- Contacta al soporte técnico
- Se requiere acceso a la base de datos

**Para Vendedores:**
- Solicita al administrador que cambie tu contraseña
- Configuración ? Usuarios ? Cambiar Contraseña

### 2. ¿Cómo hago un backup de mis datos?

1. Cierra la aplicación SEKAI POS
2. Navega a la carpeta de instalación
3. Copia el archivo `pos.db`
4. Guárdalo en un lugar seguro (USB, nube, etc.)

**Frecuencia recomendada:** Diaria o semanal

### 3. ¿Puedo usar el sistema en múltiples computadoras?

**Versión actual (1.0):**
- Solo una computadora a la vez
- Base de datos local

**Futuras versiones:**
- Multi-terminal con servidor centralizado
- Sincronización en tiempo real

### 4. El stock no se actualiza después de una venta

**Solución:**
1. Ve a Inventario
2. Haz clic en "Actualizar"
3. Si persiste, cierra y vuelve a abrir la aplicación

### 5. Error al completar una venta

**Posibles causas:**
1. Stock insuficiente ? Verifica disponibilidad
2. Producto eliminado ? Actualiza el catálogo
3. Error de conexión a BD ? Reinicia la aplicación

### 6. ¿Cómo imprimo las boletas?

**Actualmente:**
- La boleta se muestra en pantalla
- Usa Ctrl+P para imprimir desde la ventana

**Futuras versiones:**
- Impresión automática en impresora térmica
- Formato personalizable

### 7. ¿Puedo cambiar el porcentaje de IVA?

Sí, como administrador:
1. Ve a Configuración ? General
2. Modifica el campo "IVA (%)"
3. Haz clic en "Guardar Cambios"

?? **Nota:** El cambio aplica para todas las ventas futuras.

### 8. ¿Cómo recupero productos eliminados?

**No es posible** recuperar productos eliminados desde la aplicación.

**Solución preventiva:**
- Realiza backups frecuentes
- Confirma siempre antes de eliminar

### 9. El sistema está lento

**Soluciones:**
1. Cierra otros programas
2. Verifica que tengas mínimo 2GB RAM disponibles
3. Reinicia la computadora
4. Compacta la base de datos (contacta soporte)

### 10. ¿Puedo personalizar las categorías?

Sí, al agregar/editar productos:
1. En el campo "Categoría"
2. Escribe el nombre de la nueva categoría
3. Se creará automáticamente

**Categorías sugeridas:**
- Computadoras
- Accesorios
- Monitores
- Audio
- Almacenamiento
- Componentes
- Redes

---

## ?? SOPORTE TÉCNICO

### Contacto:

**GitHub:** [https://github.com/Johann3421/SekaiPOS_1.0](https://github.com/Johann3421/SekaiPOS_1.0)

**Documentación:** Revisa el archivo `README.md` en el repositorio

**Reporte de Bugs:** Crea un "Issue" en GitHub con:
- Descripción del problema
- Pasos para reproducirlo
- Capturas de pantalla (si aplica)
- Versión del sistema

### Información del Sistema:

Para reportar un problema, incluye:
- Versión de SEKAI POS: 1.0.0
- Sistema Operativo: Windows X
- Versión de .NET: 10
- Memoria RAM: XGB

### Actualizaciones:

Revisa periódicamente el repositorio de GitHub para:
- Nuevas versiones
- Parches de seguridad
- Nuevas funcionalidades

---

## ?? NOTAS IMPORTANTES

### Seguridad:
- ? Cambia las contraseñas por defecto
- ? No compartas credenciales de administrador
- ? Cierra sesión al terminar
- ? Realiza backups regulares

### Mejores Prácticas:
- ? Capacita a tus empleados antes de usar el sistema
- ? Verifica el stock periódicamente
- ? Revisa los reportes diariamente
- ? Mantén actualizados los precios
- ? Limpia productos obsoletos

### Mantenimiento:
- ? Backup semanal de pos.db
- ? Revisar logs de errores mensualmente
- ? Actualizar cuando haya nuevas versiones
- ? Limpiar registros antiguos anualmente

---

## ?? CAPACITACIÓN RECOMENDADA

### Para Administradores (2 horas):
1. Login y navegación (15 min)
2. Dashboard y estadísticas (15 min)
3. Gestión de inventario (30 min)
4. Realización de ventas (30 min)
5. Reportes y exportación (15 min)
6. Configuración y usuarios (15 min)

### Para Vendedores (1 hora):
1. Login básico (10 min)
2. Visualizar inventario (15 min)
3. Realizar ventas (30 min)
4. Manejo de errores comunes (5 min)

---

**¡Gracias por usar SEKAI POS!**

*Sistema de Punto de Venta para Tiendas de Tecnología*
*Versión 1.0.0*
*© 2025 - Todos los derechos reservados*
