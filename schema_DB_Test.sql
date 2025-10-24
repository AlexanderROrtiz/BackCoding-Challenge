-- =========================================================
-- SCRIPT DE BASE DE DATOS: BTG PACTUAL
-- Descripción: Estructura completa y datos de prueba
-- Autor: Roberth Alexander Ortiz Chavaco
-- Fecha: 2025-10-20
-- =========================================================

-- Crear base de datos (ejecutar una vez desde postgres)
CREATE DATABASE DB_Test;

-- =========================================================
-- Usar esquema público
-- =========================================================
SET search_path TO public;

-- =========================================================
-- Tablas base
-- =========================================================

-- Tabla: cliente
CREATE TABLE IF NOT EXISTS cliente (
    id_cliente      INTEGER PRIMARY KEY,
    nombre          VARCHAR(100) NOT NULL,
    apellidos       VARCHAR(100) NOT NULL,
    ciudad          VARCHAR(100) NOT NULL
);

-- Tabla: sucursal
CREATE TABLE IF NOT EXISTS sucursal (
    id_sucursal     INTEGER PRIMARY KEY,
    nombre          VARCHAR(100) NOT NULL,
    ciudad          VARCHAR(100) NOT NULL
);

-- Tabla: producto
CREATE TABLE IF NOT EXISTS producto (
    id_producto     INTEGER PRIMARY KEY,
    nombre          VARCHAR(200) NOT NULL,
    tipo_producto   VARCHAR(50) NOT NULL,
    min_amount      NUMERIC(12,2) DEFAULT 0
);

-- Tabla: inscripcion (cliente <-> producto)
CREATE TABLE IF NOT EXISTS inscripcion (
    id_producto     INTEGER NOT NULL,
    id_cliente      INTEGER NOT NULL,
    PRIMARY KEY (id_producto, id_cliente),
    CONSTRAINT fk_inscripcion_producto FOREIGN KEY (id_producto)
        REFERENCES producto(id_producto) ON DELETE CASCADE,
    CONSTRAINT fk_inscripcion_cliente FOREIGN KEY (id_cliente)
        REFERENCES cliente(id_cliente) ON DELETE CASCADE
);

-- Tabla: disponibilidad (sucursal <-> producto)
CREATE TABLE IF NOT EXISTS disponibilidad (
    id_sucursal     INTEGER NOT NULL,
    id_producto     INTEGER NOT NULL,
    PRIMARY KEY (id_sucursal, id_producto),
    CONSTRAINT fk_disponibilidad_sucursal FOREIGN KEY (id_sucursal)
        REFERENCES sucursal(id_sucursal) ON DELETE CASCADE,
    CONSTRAINT fk_disponibilidad_producto FOREIGN KEY (id_producto)
        REFERENCES producto(id_producto) ON DELETE CASCADE
);

-- Tabla: visitan (cliente <-> sucursal)
CREATE TABLE IF NOT EXISTS visitan (
    id_sucursal     INTEGER NOT NULL,
    id_cliente      INTEGER NOT NULL,
    fecha_visita    DATE NOT NULL,
    PRIMARY KEY (id_sucursal, id_cliente, fecha_visita),
    CONSTRAINT fk_visitan_sucursal FOREIGN KEY (id_sucursal)
        REFERENCES sucursal(id_sucursal) ON DELETE CASCADE,
    CONSTRAINT fk_visitan_cliente FOREIGN KEY (id_cliente)
        REFERENCES cliente(id_cliente) ON DELETE CASCADE
);

-- =========================================================
-- Índices para optimización
-- =========================================================
CREATE INDEX IF NOT EXISTS idx_inscripcion_id_cliente ON inscripcion (id_cliente);
CREATE INDEX IF NOT EXISTS idx_disponibilidad_id_producto ON disponibilidad (id_producto);
CREATE INDEX IF NOT EXISTS idx_visitan_id_cliente ON visitan (id_cliente);

-- =========================================================
-- Datos iniciales
-- =========================================================

-- Clientes
INSERT INTO cliente (id_cliente, nombre, apellidos, ciudad) VALUES
(1, 'Juan', 'Pérez', 'Bogotá'),
(2, 'María', 'Gómez', 'Medellín'),
(3, 'Carlos', 'Ruiz', 'Cali')
ON CONFLICT DO NOTHING;

-- Sucursales
INSERT INTO sucursal (id_sucursal, nombre, ciudad) VALUES
(1, 'Sucursal Centro', 'Bogotá'),
(2, 'Sucursal Norte', 'Bogotá'),
(3, 'Sucursal Laureles', 'Medellín'),
(4, 'Sucursal Valle', 'Cali')
ON CONFLICT DO NOTHING;

-- Productos
INSERT INTO producto (id_producto, nombre, tipo_producto, min_amount) VALUES
(1, 'FPV_BTG_PACTUAL_RECAUDADORA', 'FPV', 75000.00),
(2, 'FPV_BTG_PACTUAL_ECOPETROL',   'FPV', 125000.00),
(3, 'DEUDAPRIVADA',                'FIC', 50000.00),
(4, 'FDO-ACCIONES',                'FIC', 250000.00),
(5, 'FPV_BTG_PACTUAL_DINAMICA',    'FPV', 100000.00)
ON CONFLICT DO NOTHING;

-- Disponibilidad
INSERT INTO disponibilidad (id_sucursal, id_producto) VALUES
(1, 1),
(1, 2),
(2, 2),
(3, 1),
(3, 5),
(4, 3),
(4, 4)
ON CONFLICT DO NOTHING;

-- Inscripciones
INSERT INTO inscripcion (id_producto, id_cliente) VALUES
(1, 1),
(2, 1),
(3, 3),
(5, 2)
ON CONFLICT DO NOTHING;

-- Visitas
INSERT INTO visitan (id_sucursal, id_cliente, fecha_visita) VALUES
(1, 1, '2025-10-01'),
(2, 1, '2025-09-15'),
(3, 2, '2025-08-20'),
(4, 3, '2025-07-10'),
(3, 1, '2025-10-05')
ON CONFLICT DO NOTHING;

-- =========================================================
-- Consulta de ejemplo solicitada en la prueba
-- =========================================================
SELECT DISTINCT c.id_cliente, c.nombre, c.apellidos
FROM cliente c
JOIN inscripcion i ON i.id_cliente = c.id_cliente
WHERE EXISTS (
  SELECT 1
  FROM producto p
  WHERE p.id_producto = i.id_producto
    AND NOT EXISTS (
      SELECT 1
      FROM disponibilidad d
      LEFT JOIN visitan v
        ON v.id_sucursal = d.id_sucursal AND v.id_cliente = c.id_cliente
      WHERE d.id_producto = p.id_producto
        AND v.id_cliente IS NULL
    )
);
