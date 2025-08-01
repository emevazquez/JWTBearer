# Copilot Instructions for JWTBearer (.NET 8 API)

## Arquitectura y Componentes
- Proyecto API en ASP.NET Core 8, estructura tradicional de controllers.
- Endpoints principales:
  - `POST /api/auth/login`: Genera un JWT con roles embebidos ("Admin", "User").
  - `GET /api/auth/admin`: Protegido, requiere rol `Admin`.
  - `GET /api/auth/user`: Protegido, requiere autenticación.
- Toda la lógica de autenticación y autorización está en `AuthController.cs`.
- El token JWT se firma y valida con una clave secreta larga definida en ambos: `Program.cs` y `AuthController.cs` (¡deben coincidir!).

## Configuración y Flujo
- La clave secreta debe ser de al menos 256 bits para HS256.
- La configuración de JWT está en `Program.cs` bajo `AddAuthentication` y `TokenValidationParameters`.
- Los endpoints protegidos usan `[Authorize]` y `[Authorize(Roles = "Admin")]`.
- El login permite dos usuarios "hardcodeados": `admin`/`admin` y `user`/`user`.

## Build, Run y Debug
- Compilar: `dotnet build JwtAuthApi`
- Ejecutar: `dotnet run --project JwtAuthApi`
- Swagger UI: disponible en `/swagger` en modo desarrollo.
- Probar login y endpoints protegidos desde Swagger o con curl/postman.

## Convenciones y Patrones
- Rutas de controllers: `[Route("api/[controller]")]`.
- Los endpoints de controllers deben ser registrados con `AddControllers()` y `MapControllers()` en `Program.cs`.
- No hay base de datos ni almacenamiento persistente: todo es en memoria y simulado.
- No hay tests automatizados por defecto.

## Ejemplo de flujo JWT
1. `POST /api/auth/login` con usuario y contraseña válidos.
2. Copiar el token JWT de la respuesta.
3. Usar el token en el header `Authorization: Bearer <token>` para acceder a `/api/auth/admin` o `/api/auth/user`.

## Archivos clave
- `JwtAuthApi/Program.cs`: configuración de servicios, autenticación y pipeline.
- `JwtAuthApi/Controllers/AuthController.cs`: lógica de login, generación de JWT y endpoints protegidos.

## Notas
- Si cambias la clave secreta, actualízala en ambos archivos.
- Si Swagger no muestra los endpoints, asegúrate de tener `AddControllers()` y `MapControllers()` en `Program.cs` y que los endpoints no estén definidos como minimal APIs.
