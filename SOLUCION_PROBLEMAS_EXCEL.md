# ?? Guía de Solución de Problemas - Importación Excel

## Problema Reportado
- ? Archivos Excel no se importan correctamente
- ? Errores al intentar importar .xlsx

## Verificaciones Necesarias

### 1. Verificar que EPPlus esté instalado

```powershell
dotnet list package | findstr EPPlus
```

Debe mostrar:
```
EPPlus    7.5.2
```

### 2. Formato del Archivo Excel

El archivo Excel DEBE tener esta estructura EXACTA:

#### Fila 1 (Encabezados):
```
A1: Name
B1: Description  
C1: Price
D1: Quantity
E1: Category
F1: Barcode
```

#### Filas 2+ (Datos):
```
A2: Laptop HP Pavilion 15
B2: Intel Core i5 8GB RAM 256GB SSD
C2: 599.99        (SIN símbolo $)
D2: 15            (número entero)
E2: Computadoras
F2: 7501234567890
```

## Errores Comunes

### Error 1: "El archivo Excel está vacío o corrupto"

**Causas:**
- Archivo sin hojas
- Archivo dañado
- Formato incorrecto

**Solución:**
1. Abrir el archivo en Excel
2. Verificar que tenga al menos 1 hoja
3. Guardar como nuevo archivo .xlsx
4. Intentar de nuevo

### Error 2: "Precio inválido"

**Causas:**
- Celda con formato de moneda ($599.99)
- Celda con texto ("Quinientos")
- Celda vacía

**Solución:**
1. Seleccionar columna C (Price)
2. Click derecho ? Formato de celdas
3. Categoría: Número
4. Decimales: 2
5. SIN símbolo de moneda
6. Guardar

### Error 3: "Cantidad inválida"

**Causas:**
- Decimales (15.5)
- Texto
- Celdas vacías

**Solución:**
1. Seleccionar columna D (Quantity)
2. Formato de celdas ? Número
3. Decimales: 0
4. Asegurar que son números enteros

### Error 4: "No se puede leer el archivo"

**Causas:**
- Archivo abierto en Excel
- Sin permisos
- Archivo protegido

**Solución:**
1. CERRAR Excel completamente
2. Verificar que el archivo no esté en uso
3. Verificar permisos de lectura
4. Si está en red, copiar a disco local

## Crear Archivo de Prueba Correcto

### Paso a Paso:

1. **Abrir Excel** ? Libro nuevo

2. **En la Fila 1**, escribir encabezados:
   ```
   A1: Name
   B1: Description
   C1: Price
   D1: Quantity
   E1: Category
   F1: Barcode
   ```

3. **En la Fila 2**, agregar producto de prueba:
   ```
   A2: Producto de Prueba
   B2: Descripción de prueba
   C2: 100        (SOLO el número, sin $)
   D2: 10         (número entero)
   E2: General
   F2: 123456
   ```

4. **Formatear columnas**:
   - Columna C: Número, 2 decimales, SIN $
   - Columna D: Número, 0 decimales

5. **Guardar**:
   - Archivo ? Guardar como
   - Tipo: Libro de Excel (*.xlsx)
   - Nombre: prueba_productos.xlsx

6. **Cerrar Excel**

7. **Importar en SEKAI POS**:
   - Inventario ? Importar
   - Seleccionar prueba_productos.xlsx
   - Elegir opción (Sí/No)
   - Verificar resultado

## Formato Recomendado por Columna

### Columna A - Name (Texto)
```
Formato: Texto
Ejemplo: "Laptop HP Pavilion 15"
Máximo: 255 caracteres
Requerido: SÍ
```

### Columna B - Description (Texto)
```
Formato: Texto
Ejemplo: "Intel Core i5, 8GB RAM, 256GB SSD"
Máximo: 500 caracteres
Requerido: NO (puede estar vacío)
```

### Columna C - Price (Número)
```
Formato: Número
Decimales: 2
Símbolo: NO
Ejemplo: 599.99
Mínimo: 0.01
Requerido: SÍ
```

### Columna D - Quantity (Número Entero)
```
Formato: Número
Decimales: 0
Ejemplo: 15
Mínimo: 0
Requerido: SÍ
```

### Columna E - Category (Texto)
```
Formato: Texto
Ejemplo: "Computadoras"
Por defecto: "General"
Requerido: NO
```

### Columna F - Barcode (Texto)
```
Formato: Texto
Ejemplo: "7501234567890"
Longitud: Variable
Requerido: NO
```

## Ejemplo Visual en Excel

```
???????????????????????????????????????????????????????????????????????????????????????????????????
? Name                    ? Description      ? Price  ? Quantity ? Category     ? Barcode         ?
???????????????????????????????????????????????????????????????????????????????????????????????????
? Laptop HP Pavilion 15   ? Intel Core i5... ? 599.99 ?    15    ? Computadoras ? 7501234567890  ?
? Mouse Logitech MX       ? Mouse wireless   ?  99.99 ?    30    ? Accesorios   ? 7501234567891  ?
? Teclado Mecánico        ? RGB Cherry MX    ? 179.99 ?    20    ? Accesorios   ? 7501234567892  ?
???????????????????????????????????????????????????????????????????????????????????????????????????
```

## Conversión desde CSV a Excel

Si ya tienes un CSV que funciona:

1. Abrir el CSV en Excel
2. Los datos se cargarán correctamente
3. Archivo ? Guardar como ? .xlsx
4. Ya tienes un archivo Excel válido

## Diagnóstico de Errores

### Si el archivo NO se importa:

1. **Verificar extensión**:
   ```
   ¿Es .xlsx o .xls? ?
   ¿No es .xlsm o .xlsb? ?
   ```

2. **Verificar contenido**:
   ```
   ¿Tiene encabezados en fila 1? ?
   ¿Tiene datos en fila 2+? ?
   ¿Columnas A-D tienen datos? ?
   ```

3. **Verificar formato**:
   ```
   ¿Precios son números? ?
   ¿Cantidades son enteros? ?
   ¿No hay símbolos en números? ?
   ```

4. **Verificar acceso**:
   ```
   ¿Excel está cerrado? ?
   ¿Archivo no está protegido? ?
   ¿Tienes permisos de lectura? ?
   ```

## Mensajes de Error y Soluciones

### "Línea X: Precio inválido 'Y'"

```
Problema: El precio en la fila X no es válido
Solución:
1. Ir a fila X en Excel
2. Verificar columna C (Price)
3. Quitar cualquier símbolo ($, €, etc.)
4. Asegurar que es un número (599.99)
5. Guardar y reimportar
```

### "Línea X: Cantidad inválida 'Y'"

```
Problema: La cantidad en la fila X no es un entero
Solución:
1. Ir a fila X en Excel
2. Verificar columna D (Quantity)
3. Convertir a número entero (15.5 ? 15 o 16)
4. Guardar y reimportar
```

### "Línea X: Nombre vacío"

```
Problema: La columna Name está vacía
Solución:
1. Ir a fila X en Excel
2. Agregar nombre en columna A
3. O eliminar la fila si no es necesaria
4. Guardar y reimportar
```

## Plantilla Excel Recomendada

Crear un archivo llamado `PLANTILLA_PRODUCTOS.xlsx`:

```
Fila 1: Name | Description | Price | Quantity | Category | Barcode
Fila 2: (ejemplo) | (ejemplo) | 99.99 | 10 | General | ABC123
Fila 3: (vacía para empezar a llenar)
```

Uso:
1. Hacer copia de PLANTILLA_PRODUCTOS.xlsx
2. Nombrar: mis_productos.xlsx
3. Llenar desde fila 3 en adelante
4. NO modificar encabezados (fila 1)
5. Guardar
6. Importar

## Contacto de Soporte

Si después de seguir todos los pasos el problema persiste:

1. Enviar el archivo Excel problemático
2. Captura de pantalla del error
3. Descripción de los pasos seguidos

---

**Última actualización**: Enero 2025
**Versión**: 1.1
