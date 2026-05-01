# Análisis Funcional (v1.0)

## 1. Visión General del Negocio
La aplicación web provee funcionalidades enfocadas en la gestión de identificación de usuarios (Autenticación y Registro). Contiene flujos específicos para registrar un nuevo perfil utilizando información de contacto y credenciales, además de los módulos base para iniciar y validar una futura sesión de acceso público y privado.

## 2. Flujos Principales e Interacciones

### 2.1. Módulo de Registro (Register)
Responsable de captar la información para generar nuevas cuentas de usuario en el sistema.
- **Rutas asociadas:** 
  - `GET /Register/Create` (o `/Register/Register`): Devuelve la vista principal del formulario del registro.
  - `POST /Register/Create`: Endpoint encargado del procesamiento de datos.
- **Modelo de Datos:**
  - `nombre` (Obligatorio, máx. 50 caracteres).
  - `clave` (Obligatorio, de 4 a 100 caracteres, tratada como contraseña oculta).
  - `correo` (Opcional bajo reglas o validado puramente con formato `@`).
- **Proceso Interno:**
  1. El sistema valida si los campos obligatorios cumplen el formato esperado (Data Annotations en el lado del servidor).
  2. Si el modelo no es válido, se recarga la vista mostrando mensajes al usuario indicando el error (ej: *"Ingresa un nombre válido..."*).
  3. Si la validación es exitosa, se invoca a base de datos de manera asíncrona (`AgregarRegisterAsync`) insertando los datos en la tabla `Register`.
  4. Finalizado el proceso sin errores, redirecciona al usuario al `Home` (Index) del sistema.

### 2.2. Módulo de Autenticación (Login)
Destinado a permitir el ingreso al sistema de usuarios con perfiles ya existentes.
- **Ruta asociada:**
  - `GET /Login/Login`: Despliega el formulario de inicio de sesión.
  - `POST /Login/Login`: Endpoint de recepción de credenciales de ingreso.
- **Modelo de Datos:**
  - `nombre` y `clave` usando las mismas restricciones y mensajería de error del registro para consistencia.
- **Proceso Interno (Actual / Mock):**
  1. Realiza las mismas validaciones de input establecidas en el modelo.
  2. **Validación Simulada (Hardcodeada):** Actualmente, las credenciales no se cruzan con la base de datos SQL. En lugar de ello, el sistema tiene una lógica de demostración de que si el usuario es `karim` y la contraseña `1234`, el inicio de sesión será considerado válido.
  3. De ser válido el inicio, guarda un mensaje de bienvenida temporal (`TempData["Message"]`) para la confirmación de la interfaz gráfica y redirige hacia `Home/Index`.
  4. De ser denegado, añade un error local al estado del modelo de MVC y pide repetir el inicio de sesión al cliente.

## 3. Integración a Nivel de Usuario
Las funcionalidades logran su continuidad inter-páginas a través de diferentes estructuras de MVC. Aunque aún se debe robustecer el Login conectándolo directamente al patrón *Repository* análogo al del Registro, la validación integral y los redireccionamientos ya componen el flujo central para el cliente o usuario del entorno.
