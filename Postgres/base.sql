DROP SCHEMA IF EXISTS public CASCADE;
CREATE SCHEMA public;

-- Tabla base de usuarios
CREATE TABLE public.user (
    id SERIAL PRIMARY KEY,
    created_at TIMESTAMP DEFAULT now() NOT NULL,
    last_modification TIMESTAMP DEFAULT NULL,
    is_active BOOLEAN DEFAULT true,
    name VARCHAR(50),
    first_lastname VARCHAR(50),
    second_lastname VARCHAR(50),
    date_birth DATE,
    ci VARCHAR(20),
    role VARCHAR(20)
);

-- Tabla de clientes
CREATE TABLE public.client (
    id_user INT PRIMARY KEY,
    fitness_level TEXT CHECK (fitness_level IN ('Principiante','Intermedio','Avanzado')),
    initial_weight_kg NUMERIC(5,2),
    current_weight_kg NUMERIC(5,2),
    emergency_contact_phone VARCHAR(20),
    CONSTRAINT fk_client_user FOREIGN KEY (id_user) REFERENCES public.user(id) ON DELETE CASCADE
);

-- Tabla de instructores
CREATE TABLE public.instructor (
    id_user INT PRIMARY KEY,
    hire_date DATE,
    monthly_salary NUMERIC(10,2),
    specialization VARCHAR(100),
    CONSTRAINT fk_instructor_user FOREIGN KEY (id_user) REFERENCES public.user(id) ON DELETE CASCADE
);

-- Tabla de membresias
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
    id_instructor INT,
    start_time TIME,
    end_time TIME,
    CONSTRAINT fk_discipline_instructor FOREIGN KEY (id_instructor) REFERENCES public.instructor(id_user) ON DELETE SET NULL
);

-- Tabla de detalles de membresía
CREATE TABLE public.details_membership (
    id_membership SMALLINT,
    id_discipline SMALLINT,
    CONSTRAINT fk_details_membership_membership FOREIGN KEY (id_membership) REFERENCES public.membership(id) ON DELETE CASCADE,
    CONSTRAINT fk_details_membership_discipline FOREIGN KEY (id_discipline) REFERENCES public.discipline(id) ON DELETE CASCADE
);

-- Tabla de detalles de usuario
CREATE TABLE public.details_user (
    id SERIAL PRIMARY KEY,
    created_at TIMESTAMP DEFAULT now() NOT NULL,
    last_modification TIMESTAMP DEFAULT NULL,
    is_active BOOLEAN DEFAULT true,
    id_user INT,
    id_membership SMALLINT,
    start_date DATE,
    end_date DATE,
    sessions_left SMALLINT,
    CONSTRAINT fk_details_user_user FOREIGN KEY (id_user) REFERENCES public.user(id) ON DELETE CASCADE,
    CONSTRAINT fk_details_user_membership FOREIGN KEY (id_membership) REFERENCES public.membership(id) ON DELETE SET NULL
);

-- Tabla de logs
CREATE TABLE public.logs (
    id SERIAL PRIMARY KEY,
    created_at TIMESTAMP DEFAULT now() NOT NULL,
    level VARCHAR(20),
    message TEXT,
    client_identifier VARCHAR(20)
);

-- Usuarios
INSERT INTO public.user (created_at, name, first_lastname, second_lastname, date_birth, ci, "role")
OVERRIDING SYSTEM VALUE VALUES
('2025-09-28 21:38:33', 'Juan', 'Perez', 'Quispe', '2000-07-14', '12345678', 'Instructor'),
('2025-09-30 12:59:41', 'Andrés', 'Salvatierra', 'Ramírez', '2003-11-21', '0000000', 'Client');

-- Clientes
INSERT INTO public.client (id_user, fitness_level, initial_weight_kg, current_weight_kg, emergency_contact_phone)
VALUES (2, 'Intermedio', 55, 57, '77967341');

-- Instructores
INSERT INTO public.instructor (id_user, hire_date, monthly_salary, specialization)
VALUES (1, '2025-09-30', 2000, 'Body Combat');

-- Membresías
INSERT INTO public.membership (created_at, name, price, description, monthly_sessions)
OVERRIDING SYSTEM VALUE VALUES
('2025-09-28 12:49:38', 'GOLD', 460, 'Incluye todas las disciplinas del gimnasio y acceso a los saunas gratis. Uso ilimitado de las máquinas de cardio.', 15),
('2025-09-28 13:23:38', 'SILVER', 300, 'Incluye solo las disciplinas spinning, zumba, body combat. Acceso ilimitado a máquinas de cardio.', 15),
('2025-09-28 20:59:19', 'Zumba', 49.89, 'Acceso únicamente a clases e instructores de Zumba', 30),
('2025-10-05 14:57:37', 'Test', 10, 'Nada especial', 15);

-- Disciplinas
INSERT INTO public.discipline (created_at, name, id_instructor, start_time, end_time)
OVERRIDING SYSTEM VALUE VALUES
('2025-09-28 20:29:27', 'Crossfit', NULL, '07:30:00', '09:30:00'),
('2025-09-28 21:05:31', 'Zumba', NULL, '07:00:00', '09:00:00'),
('2025-10-05 16:48:10', 'Test Dis', NULL, '00:00:00', '13:00:00');

-- Detalles de usuario
INSERT INTO public.details_user (created_at, id_user, id_membership, start_date, end_date, sessions_left)
OVERRIDING SYSTEM VALUE VALUES
('2025-10-04 16:15:40', 2, 2, '2025-01-01', '2026-02-22', 30),
('2025-10-05 17:17:11', 2, 2, '2025-01-01', '2026-01-01', 150);
