DROP SCHEMA IF EXISTS public CASCADE;
CREATE SCHEMA public;

-- Tabla base de personas
CREATE TABLE public.person (
    id SERIAL PRIMARY KEY,
    created_at TIMESTAMP DEFAULT now() NOT NULL,
    last_modification TIMESTAMP DEFAULT NULL,
    is_active BOOLEAN DEFAULT true,
    name VARCHAR(50),
    first_lastname VARCHAR(50),
    second_lastname VARCHAR(50),
    date_birth DATE,
    ci VARCHAR(20)
);

-- Tabla de clientes
CREATE TABLE public.client (
    id_person INT PRIMARY KEY,
    fitness_level TEXT CHECK (fitness_level IN ('Principiante','Intermedio','Avanzado')),
    initial_weight_kg NUMERIC(5,2),
    current_weight_kg NUMERIC(5,2),
    emergency_contact_phone VARCHAR(20),
    CONSTRAINT fk_client_person FOREIGN KEY (id_person) REFERENCES public.person(id) ON DELETE CASCADE
);

-- Tabla de usuarios (empleados o administradores)
CREATE TABLE public.user (
    id_person INT PRIMARY KEY,
    role VARCHAR(20),
    hire_date DATE,
    monthly_salary NUMERIC(10,2),
    specialization VARCHAR(100),
    password VARCHAR(255) NOT NULL,
    email VARCHAR(100) UNIQUE,
    must_change_password BOOLEAN DEFAULT true,
    CONSTRAINT fk_person_user FOREIGN KEY (id_person) REFERENCES public.person(id) ON DELETE CASCADE
);

-- Tabla de membresías
CREATE TABLE public.membership (
    id SMALLSERIAL PRIMARY KEY,
    created_at TIMESTAMP DEFAULT now() NOT NULL,
    last_modification TIMESTAMP DEFAULT NULL,
    is_active BOOLEAN DEFAULT true,
    name VARCHAR(50),
    price NUMERIC(10,2),
    description TEXT,
    monthly_sessions SMALLINT
);

-- Tabla de disciplinas
CREATE TABLE public.discipline (
    id SMALLSERIAL PRIMARY KEY,
    created_at TIMESTAMP DEFAULT now() NOT NULL,
    last_modification TIMESTAMP DEFAULT NULL,
    is_active BOOLEAN DEFAULT true,
    name VARCHAR(50),
    id_user INT,
    start_time TIME,
    end_time TIME,
    CONSTRAINT fk_discipline_user FOREIGN KEY (id_user) REFERENCES public.user(id_person) ON DELETE SET NULL
);

-- Tabla relacional membresía-disciplina
CREATE TABLE public.membership_disciplines (
    id_membership SMALLINT,
    id_discipline SMALLINT,
    CONSTRAINT fk_membership FOREIGN KEY (id_membership) REFERENCES public.membership(id) ON DELETE CASCADE,
    CONSTRAINT fk_discipline FOREIGN KEY (id_discipline) REFERENCES public.discipline(id) ON DELETE CASCADE
);

-- Tabla que asocia cliente con membresía
CREATE TABLE public.client_membership (
    id SERIAL PRIMARY KEY,
    created_at TIMESTAMP DEFAULT now() NOT NULL,
    last_modification TIMESTAMP DEFAULT NULL,
    is_active BOOLEAN DEFAULT true,
    id_client INT,
    id_membership SMALLINT,
    start_date DATE,
    end_date DATE,
    sessions_left SMALLINT,
    CONSTRAINT fk_client FOREIGN KEY (id_client) REFERENCES public.client(id_person) ON DELETE CASCADE,
    CONSTRAINT fk_membership FOREIGN KEY (id_membership) REFERENCES public.membership(id) ON DELETE SET NULL
);

-- Tabla de logs
CREATE TABLE public.logs (
    id SERIAL PRIMARY KEY,
    created_at TIMESTAMP DEFAULT now() NOT NULL,
    level VARCHAR(20),
    message TEXT,
    client_identifier VARCHAR(20)
);

-----------------------------------------------------
-- INSERCIÓN DE DATOS
-----------------------------------------------------

-- Personas
INSERT INTO public.person (name, first_lastname, second_lastname, date_birth, ci)
VALUES
('Juan', 'Perez', 'Quispe', '2000-07-14', '12345678'),
('Andrés', 'Salvatierra', 'Ramírez', '2003-11-21', '0000000'),
('Elad', 'Minist', 'Trador', '2000-12-20', '99999999');

-- Usuarios (empleados/admins)
INSERT INTO public.user (id_person, role, hire_date, monthly_salary, specialization, email, password, must_change_password)
VALUES
(1, 'Instructor', '2025-09-30', 2000, 'Body Combat', 'instructor@gmail.com', '$2a$11$lkjVho0mJHPWs8Cn.8wEhOtJP1ZvDtwQ01qpXaxLVSVPjQVjL2rnm', true),
(3, 'Admin', '2025-09-30', 2500, 'Administrar Sistema', 'admin@gmail.com', '$2a$11$W/CTSKdsb0e0hTYsCUvFH.pA0BzYdUjqmwT/.fpREeCXbGN1qfyim', true);

-- Cliente (solo persona sin registro en "user")
INSERT INTO public.client (id_person, fitness_level, initial_weight_kg, current_weight_kg, emergency_contact_phone)
VALUES
(2, 'Intermedio', 55, 57, '77967341');

-- Membresías
INSERT INTO public.membership (name, price, description, monthly_sessions)
VALUES
('GOLD', 460, 'Incluye todas las disciplinas del gimnasio y acceso a los saunas gratis. Uso ilimitado de las máquinas de cardio.', 15),
('SILVER', 300, 'Incluye solo las disciplinas spinning, zumba, body combat. Acceso ilimitado a máquinas de cardio.', 15),
('Zumba', 49.89, 'Acceso únicamente a clases e instructores de Zumba', 30),
('Test', 10, 'Nada especial', 15);

-- Disciplinas
INSERT INTO public.discipline (name, id_user, start_time, end_time)
VALUES
('Crossfit', 1, '08:30:00', '09:30:00'),
('Zumba', 1, '08:00:00', '09:00:00'),
('Test Dis', 1, '10:10:00', '12:00:00');

-- Asociación membresía-disciplinas
INSERT INTO public.membership_disciplines (id_membership, id_discipline)
VALUES
(1, 1),
(1, 2),
(2, 2),
(3, 2);

-- Asociación cliente-membresía
INSERT INTO public.client_membership (id_client, id_membership, start_date, end_date, sessions_left)
VALUES
(2, 2, '2025-01-01', '2026-02-22', 30),
(2, 2, '2025-01-01', '2026-01-01', 15);

-----------------------------------------------------
-- VISTAS
-----------------------------------------------------

-- Vista de instructores
CREATE OR REPLACE VIEW public.instructor_view AS
SELECT 
    p.id AS Id,
    p.name AS Name,
    p.first_lastname AS FirstLastname,
    p.second_lastname AS SecondLastname,
    p.date_birth AS DateBirth,
    p.ci AS Ci,
    u.role AS Role,
    p.created_at AS CreatedAt,
    p.last_modification AS LastModification,
    p.is_active AS IsActive,
    u.hire_date AS HireDate,
    u.monthly_salary AS MonthlySalary,
    u.specialization AS Specialization,
    u.email AS Email,
    u.password AS Password,
    u.must_change_password AS MustChangePassword
FROM public.person p
INNER JOIN public.user u ON p.id = u.id_person
WHERE p.is_active = true;

-- Vista de clientes
CREATE OR REPLACE VIEW public.client_view AS
SELECT
    p.id AS ClientId,
    p.created_at AS CreatedAt,
    p.last_modification AS LastModification,
    p.is_active AS IsActive,
    p.name AS Name,
    p.first_lastname AS FirstLastname,
    p.second_lastname AS SecondLastname,
    p.date_birth AS DateBirth,
    p.ci AS Ci,
    'Client' AS Role,
    c.fitness_level AS FitnessLevel,
    c.initial_weight_kg AS InitialWeightKg,
    c.current_weight_kg AS CurrentWeightKg,
    c.emergency_contact_phone AS EmergencyContactPhone
FROM public.person p
INNER JOIN public.client c ON p.id = c.id_person;
