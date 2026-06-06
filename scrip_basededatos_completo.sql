

DROP DATABASE IF EXISTS chef_recetas;

CREATE DATABASE chef_recetas
CHARACTER SET utf8mb4
COLLATE utf8mb4_spanish_ci;

USE chef_recetas;

-- =====================================================
-- TABLA CATEGORIAS
-- =====================================================

CREATE TABLE categorias (
    id_categoria INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(80) NOT NULL,
    descripcion TEXT,
    icono VARCHAR(50),
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_creacion DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- =====================================================
-- TABLA INGREDIENTES
-- =====================================================

CREATE TABLE ingredientes (
    id_ingrediente INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    unidad_medida VARCHAR(30),
    calorias_por_u DECIMAL(8,2),
    imagen MEDIUMBLOB,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_creacion DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- =====================================================
-- TABLA RECETAS
-- =====================================================

CREATE TABLE recetas (
    id_receta INT AUTO_INCREMENT PRIMARY KEY,
    id_categoria INT NOT NULL,
    nombre VARCHAR(150) NOT NULL,
    descripcion TEXT,
    tiempo_prep_min INT,
    porciones INT,
    dificultad ENUM('fácil','media','difícil') NOT NULL,
    foto MEDIUMBLOB,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_creacion DATETIME DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_recetas_categoria
    FOREIGN KEY (id_categoria)
    REFERENCES categorias(id_categoria)
);

-- =====================================================
-- TABLA RECETA_INGREDIENTES
-- =====================================================

CREATE TABLE receta_ingredientes (
    id_ri INT AUTO_INCREMENT PRIMARY KEY,
    id_receta INT NOT NULL,
    id_ingrediente INT NOT NULL,
    cantidad DECIMAL(10,2) NOT NULL,
    unidad VARCHAR(30),

    activo BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_creacion DATETIME DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_ri_receta
    FOREIGN KEY (id_receta)
    REFERENCES recetas(id_receta),

    CONSTRAINT fk_ri_ingrediente
    FOREIGN KEY (id_ingrediente)
    REFERENCES ingredientes(id_ingrediente)
);

-- =====================================================
-- TABLA PASOS_PREPARACION
-- =====================================================

CREATE TABLE pasos_preparacion (
    id_paso INT AUTO_INCREMENT PRIMARY KEY,
    id_receta INT NOT NULL,
    numero_paso INT NOT NULL,
    instruccion TEXT NOT NULL,

    activo BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_creacion DATETIME DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_pasos_receta
    FOREIGN KEY (id_receta)
    REFERENCES recetas(id_receta)
);

-- =====================================================
-- TABLA USUARIOS
-- =====================================================

CREATE TABLE usuarios (
    id_usuario INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    usuario VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(64) NOT NULL,

    rol ENUM(
        'admin',
        'operador',
        'consultor'
    ) NOT NULL,

    activo BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_creacion DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- =====================================================
-- TABLA AUDITORIA
-- =====================================================

CREATE TABLE auditoria (
    id_auditoria INT AUTO_INCREMENT PRIMARY KEY,
    tabla_afectada VARCHAR(50) NOT NULL,
    accion VARCHAR(20) NOT NULL,
    usuario_bd VARCHAR(100) NOT NULL,
    fecha DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- =====================================================
-- USUARIO ADMINISTRADOR INICIAL
-- =====================================================

INSERT INTO usuarios
(
    nombre,
    usuario,
    password,
    rol,
    activo
)
VALUES
(
    'Administrador General',
    'admin',
    SHA2('admin123',256),
    'admin',
    TRUE
);
-- =====================================================
-- PARTE 2
-- ROLES Y PERMISOS
-- =====================================================

CREATE ROLE IF NOT EXISTS rol_admin;
CREATE ROLE IF NOT EXISTS rol_operador;
CREATE ROLE IF NOT EXISTS rol_consultor;

GRANT ALL PRIVILEGES
ON chef_recetas.*
TO rol_admin;

GRANT SELECT, INSERT, UPDATE
ON chef_recetas.*
TO rol_operador;

GRANT SELECT
ON chef_recetas.*
TO rol_consultor;

-- =====================================================
-- PROCEDIMIENTOS ALMACENADOS
-- =====================================================

DELIMITER $$

-- Listar recetas activas
CREATE PROCEDURE sp_listar_recetas()
BEGIN
    SELECT *
    FROM recetas
    WHERE activo = 1;
END$$

-- Buscar receta por ID
CREATE PROCEDURE sp_buscar_receta
(
    IN p_id INT
)
BEGIN
    SELECT *
    FROM recetas
    WHERE id_receta = p_id;
END$$

-- Contar recetas por categoría
CREATE PROCEDURE sp_total_recetas_categoria
(
    IN p_categoria INT,
    OUT p_total INT
)
BEGIN
    SELECT COUNT(*)
    INTO p_total
    FROM recetas
    WHERE id_categoria = p_categoria
    AND activo = 1;
END$$

-- Listar ingredientes de una receta
CREATE PROCEDURE sp_ingredientes_receta
(
    IN p_receta INT
)
BEGIN
    SELECT
        r.nombre AS receta,
        i.nombre AS ingrediente,
        ri.cantidad,
        ri.unidad
    FROM receta_ingredientes ri
    INNER JOIN recetas r
        ON ri.id_receta = r.id_receta
    INNER JOIN ingredientes i
        ON ri.id_ingrediente = i.id_ingrediente
    WHERE r.id_receta = p_receta;
END$$

-- Buscar usuarios por rol
CREATE PROCEDURE sp_usuarios_por_rol
(
    IN p_rol VARCHAR(20)
)
BEGIN
    SELECT *
    FROM usuarios
    WHERE rol = p_rol;
END$$

DELIMITER ;

-- =====================================================
-- VISTAS
-- =====================================================

-- Vista de usuarios

CREATE OR REPLACE VIEW vw_usuarios AS
SELECT
    id_usuario,
    nombre,
    usuario,
    rol,
    activo,
    fecha_creacion
FROM usuarios;

-- Vista de categorías

CREATE OR REPLACE VIEW vw_categorias AS
SELECT
    id_categoria,
    nombre,
    descripcion,
    icono,
    activo,
    fecha_creacion
FROM categorias;

-- Vista de ingredientes

CREATE OR REPLACE VIEW vw_ingredientes AS
SELECT
    id_ingrediente,
    nombre,
    unidad_medida,
    calorias_por_u,
    activo,
    fecha_creacion
FROM ingredientes;

-- Vista recetas completas

CREATE OR REPLACE VIEW vw_recetas AS
SELECT
    r.id_receta,
    r.nombre,
    r.descripcion,
    r.tiempo_prep_min,
    r.porciones,
    r.dificultad,
    c.nombre AS categoria,
    r.activo,
    r.fecha_creacion
FROM recetas r
INNER JOIN categorias c
    ON r.id_categoria = c.id_categoria;

-- Vista receta ingredientes

CREATE OR REPLACE VIEW vw_receta_ingredientes AS
SELECT
    ri.id_ri,
    r.id_receta,
    r.nombre AS receta,
    i.id_ingrediente,
    i.nombre AS ingrediente,
    ri.cantidad,
    ri.unidad,
    ri.fecha_creacion
FROM receta_ingredientes ri
INNER JOIN recetas r
    ON ri.id_receta = r.id_receta
INNER JOIN ingredientes i
    ON ri.id_ingrediente = i.id_ingrediente;

-- Vista pasos preparación

CREATE OR REPLACE VIEW vw_pasos_preparacion AS
SELECT
    p.id_paso,
    p.id_receta,
    r.nombre AS receta,
    p.numero_paso,
    p.instruccion,
    p.fecha_creacion
FROM pasos_preparacion p
INNER JOIN recetas r
    ON p.id_receta = r.id_receta;

-- Vista estadísticas por categoría

CREATE OR REPLACE VIEW vw_estadistica_recetas_categoria AS
SELECT
    c.id_categoria,
    c.nombre AS categoria,
    COUNT(r.id_receta) AS total_recetas
FROM categorias c
LEFT JOIN recetas r
    ON c.id_categoria = r.id_categoria
GROUP BY
    c.id_categoria,
    c.nombre;

-- Vista estadísticas por dificultad

CREATE OR REPLACE VIEW vw_estadistica_dificultad AS
SELECT
    dificultad,
    COUNT(*) AS total_recetas
FROM recetas
GROUP BY dificultad;
-- =====================================================
-- PARTE 3
-- TRIGGERS Y AUDITORÍA
-- =====================================================

DELIMITER $$

-- ==========================================
-- VALIDAR PORCIONES AL INSERTAR
-- ==========================================

CREATE TRIGGER tr_validar_porciones_insert
BEFORE INSERT
ON recetas
FOR EACH ROW
BEGIN
    IF NEW.porciones <= 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'Las porciones deben ser mayores que cero';
    END IF;
END$$

-- ==========================================
-- VALIDAR PORCIONES AL ACTUALIZAR
-- ==========================================

CREATE TRIGGER tr_validar_porciones_update
BEFORE UPDATE
ON recetas
FOR EACH ROW
BEGIN
    IF NEW.porciones <= 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'Las porciones deben ser mayores que cero';
    END IF;
END$$

-- ==========================================
-- AUDITORIA INSERT
-- ==========================================

CREATE TRIGGER tr_receta_insert
AFTER INSERT
ON recetas
FOR EACH ROW
BEGIN

    INSERT INTO auditoria
    (
        tabla_afectada,
        accion,
        usuario_bd
    )
    VALUES
    (
        'recetas',
        'INSERT',
        CURRENT_USER()
    );

END$$

-- ==========================================
-- AUDITORIA UPDATE
-- ==========================================

CREATE TRIGGER tr_receta_update
AFTER UPDATE
ON recetas
FOR EACH ROW
BEGIN

    INSERT INTO auditoria
    (
        tabla_afectada,
        accion,
        usuario_bd
    )
    VALUES
    (
        'recetas',
        'UPDATE',
        CURRENT_USER()
    );

END$$

-- ==========================================
-- AUDITORIA DELETE
-- ==========================================

CREATE TRIGGER tr_receta_delete
AFTER DELETE
ON recetas
FOR EACH ROW
BEGIN

    INSERT INTO auditoria
    (
        tabla_afectada,
        accion,
        usuario_bd
    )
    VALUES
    (
        'recetas',
        'DELETE',
        CURRENT_USER()
    );

END$$

DELIMITER ;

-- =====================================================
-- TRANSACCIONES
-- =====================================================

START TRANSACTION;

INSERT INTO recetas
(
    id_categoria,
    nombre,
    descripcion,
    tiempo_prep_min,
    porciones,
    dificultad,
    activo
)
VALUES
(
    2,
    'Tacos de Carne',
    'Receta de prueba',
    35,
    4,
    'media',
    1
);

UPDATE recetas
SET porciones = 6
WHERE nombre = 'Tacos de Carne';

COMMIT;

-- =====================================================
-- EJEMPLO ROLLBACK
-- =====================================================

START TRANSACTION;

INSERT INTO recetas
(
    id_categoria,
    nombre,
    descripcion,
    tiempo_prep_min,
    porciones,
    dificultad,
    activo
)
VALUES
(
    2,
    'Enchiladas Verdes',
    'Receta de prueba',
    40,
    5,
    'media',
    1
);

ROLLBACK;

-- =====================================================
-- SAVEPOINT
-- =====================================================

START TRANSACTION;

INSERT INTO recetas
(
    id_categoria,
    nombre,
    descripcion,
    tiempo_prep_min,
    porciones,
    dificultad,
    activo
)
VALUES
(
    2,
    'Mole Poblano',
    'Receta de prueba',
    60,
    8,
    'dificil',
    1
);

SAVEPOINT sp_receta;

UPDATE recetas
SET porciones = 10
WHERE nombre = 'Mole Poblano';

ROLLBACK TO SAVEPOINT sp_receta;

COMMIT;

-- =====================================================
-- NIVELES DE AISLAMIENTO
-- =====================================================

SELECT @@transaction_isolation;

SET SESSION TRANSACTION ISOLATION LEVEL
READ COMMITTED;

SET SESSION TRANSACTION ISOLATION LEVEL
REPEATABLE READ;

SET SESSION TRANSACTION ISOLATION LEVEL
SERIALIZABLE;
-- =====================================================
-- PARTE 4
-- DATOS DE PRUEBA
-- =====================================================

-- =====================================================
-- CATEGORIAS
-- =====================================================

INSERT INTO categorias
(nombre, descripcion, icono)
VALUES
('Desayunos','Recetas para desayuno','desayuno.png'),
('Comidas','Platillos principales','comida.png'),
('Postres','Postres variados','postre.png'),
('Bebidas','Bebidas naturales','bebida.png'),
('Ensaladas','Recetas saludables','ensalada.png'),
('Sopas','Sopas y caldos','sopa.png'),
('Carnes','Platillos con carne','carne.png'),
('Vegetarianos','Comida vegetariana','vegetariano.png'),
('Mariscos','Platillos del mar','mariscos.png'),
('Antojitos','Comida mexicana','antojitos.png');

-- =====================================================
-- INGREDIENTES
-- =====================================================

INSERT INTO ingredientes
(nombre, unidad_medida, calorias_por_u)
VALUES
('Huevo','pieza',70),
('Harina','gramos',364),
('Leche','ml',42),
('Azucar','gramos',387),
('Carne de Res','gramos',250),
('Pollo','gramos',239),
('Tomate','gramos',18),
('Cebolla','gramos',40),
('Limon','pieza',17),
('Fresa','gramos',32);

-- =====================================================
-- USUARIOS
-- =====================================================

INSERT INTO usuarios
(nombre, usuario, password, rol, activo)
VALUES
('Juan Perez','jperez',SHA2('1234',256),'admin',1),
('Ana Lopez','alopez',SHA2('1234',256),'operador',1),
('Carlos Ruiz','cruiz',SHA2('1234',256),'consultor',1),
('Maria Torres','mtorres',SHA2('1234',256),'operador',1),
('Pedro Gomez','pgomez',SHA2('1234',256),'consultor',1),
('Laura Martinez','lmartinez',SHA2('1234',256),'operador',1),
('Jose Hernandez','jhernandez',SHA2('1234',256),'consultor',1),
('Sofia Ramirez','sramirez',SHA2('1234',256),'operador',1),
('Miguel Flores','mflores',SHA2('1234',256),'consultor',1),
('Daniela Cruz','dcruz',SHA2('1234',256),'operador',1);

-- =====================================================
-- RECETAS
-- =====================================================

INSERT INTO recetas
(id_categoria,nombre,descripcion,tiempo_prep_min,porciones,dificultad,activo)
VALUES

(1,'Huevos Rancheros',
'Desayuno tradicional mexicano',
20,2,'fácil',1),

(1,'Hot Cakes',
'Hot cakes caseros',
25,4,'fácil',1),

(2,'Tacos de Carne',
'Tacos de res',
35,4,'media',1),

(2,'Enchiladas Verdes',
'Enchiladas con salsa verde',
40,4,'media',1),

(2,'Arroz Rojo',
'Arroz mexicano',
30,6,'fácil',1),

(3,'Pastel de Chocolate',
'Pastel casero',
90,8,'difícil',1),

(3,'Gelatina de Fresa',
'Gelatina natural',
25,6,'fácil',1),

(4,'Limonada',
'Bebida refrescante',
10,4,'fácil',1),

(5,'Ensalada Cesar',
'Ensalada saludable',
20,2,'fácil',1),

(6,'Sopa de Verduras',
'Sopa nutritiva',
35,5,'media',1);

-- =====================================================
-- RECETA_INGREDIENTES
-- =====================================================

INSERT INTO receta_ingredientes
(id_receta,id_ingrediente,cantidad,unidad)
VALUES
(1,1,2,'pieza'),
(2,2,250,'gramos'),
(3,5,500,'gramos'),
(4,6,500,'gramos'),
(5,7,300,'gramos'),
(6,2,400,'gramos'),
(7,10,250,'gramos'),
(8,9,6,'pieza'),
(9,8,100,'gramos'),
(10,7,150,'gramos');

-- =====================================================
-- PASOS_PREPARACION
-- =====================================================

INSERT INTO pasos_preparacion
(id_receta,numero_paso,instruccion)
VALUES

(1,1,'Freír tortillas y preparar huevos.'),
(2,1,'Mezclar ingredientes y cocinar en sartén.'),
(3,1,'Cocinar carne y servir en tortillas.'),
(4,1,'Preparar salsa y rellenar tortillas.'),
(5,1,'Freír arroz y cocinar con caldo.'),
(6,1,'Preparar mezcla y hornear.'),
(7,1,'Disolver gelatina y refrigerar.'),
(8,1,'Mezclar agua, limón y azúcar.'),
(9,1,'Mezclar ingredientes de la ensalada.'),
(10,1,'Hervir verduras hasta suavizar.');

