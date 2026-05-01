# Análisis de Arquitectura (v1.0)

## 1. Visión General
El proyecto es una aplicación web desarrollada utilizando el framework **ASP.NET Core 8.0**. Sigue el patrón arquitectónico **Modelo-Vista-Controlador (MVC)**, el cual ayuda a separar las preocupaciones entre la lógica de negocio, la lógica de presentación y el control del flujo de la aplicación.

## 2. Tecnologías y Herramientas
- **Framework Principal:** .NET 8.0
- **Patrón de Diseño Web:** ASP.NET Core MVC
- **Acceso a Datos:** ADO.NET (mediante `Microsoft.Data.SqlClient`)
- **Base de Datos:** SQL Server (LocalDB orientado a entornos de desarrollo, instancia `(localdb)\MSSQLLocalDB`)
- **Motor de Plantillas:** Razor

## 3. Modelo-Vista-Controlador (MVC) y Organización de Capas
El proyecto está estructurado con las siguientes correspondencias funcionales:

### 3.1. Controladores (`/Controllers`)
Son el punto de entrada de las peticiones HTTP y manejan el ruteo interno de la aplicación.
- Se identifican controladores claramente segregados por responsabilidad: `HomeController` (vistas principales), `LoginController` (autenticación) y `RegisterController` (registro de usuarios).
- Realizan validaciones rápidas basadas en el estado del modelo (`ModelState.IsValid`).
- Emplean prevención contra ataques XSRF/CSRF aplicando el atributo `[ValidateAntiForgeryToken]` en el recibimiento de datos POST.

### 3.2. Modelos (`/Models`)
Actúan como Objetos de Transferencia de Datos (DTO) o ViewModels.
- Se utilizan clases en C# puras (POCO) como `LoginModel` y `RegisterModel`.
- **Validaciones Integradas:** Hacen uso intensivo de **Data Annotations** (`[Required]`, `[StringLength]`, `[EmailAddress]`) para declarar reglas de negocio y restricciones directamente en el modelo de forma declarativa.

### 3.3. Vistas (`/Views` implícito)
A pesar de no haber analizado directamente los archivos `.cshtml`, la presencia de llamadas como `return View()` junto a archivos en `/wwwroot` indican la arquitectura estándar de renderizado del lado del servidor de Microsoft con Razor.

## 4. Acceso a Datos e Integración (Capa Repository)
La aplicación implementa un patrón **Repository** básico para abstraer el acceso a la información.
- **Implementación DB:** En la carpeta `/Repository` se ubica la clase `RegisterRepository`.
- **Agnóstico a ORM:** El proyecto descarta el uso de ORMs como Entity Framework en favor de ADO.NET plano (`SqlConnection`, `SqlCommand`), optimizando las consultas SQL crudas en la base de datos local `proyecto_dsw1`.
- **Ejecución Asíncrona:** Se aprovecha la naturaleza asíncrona de C# empleando Tasks (`async/await`) en métodos como `AgregarRegisterAsync`, mejorando el uso de recursos e hilos en el servidor al interactuar de manera I/O orientada con la DB.

## 5. Decisiones de Configuración e Inversión de Control (IoC)
- El ensamblado e inicio de la app se configura en el archivo central de configuración `Program.cs`.
- Se registra `AddControllersWithViews()` habilitando así la inyección de MVC dentro del ciclo de vida de la aplicación.
- El repositorio (`RegisterRepository`) requiere e inyecta la interfaz `IConfiguration` directamente desde el contenedor DI nativo de .NET, de donde extrae la cadena de conexión especificada de forma segura en `appsettings.json`.

## 6. Observaciones
La estructura actual mantiene una fuerte solidez para un proyecto a escala de curso o prueba de concepto. Debido a que las configuraciones locales y base de datos apuntan a `LocalDB`, si se decide pasar a Producción, será necesario cambiar o parametrizar dichos valores y reemplazar simulaciones lógicas temporales.
