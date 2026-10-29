# Generar hash BCrypt para el SuperAdmin

La API incluye un endpoint de utilidad para generar hashes BCrypt. Este endpoint solo funciona en entorno de **desarrollo**.

## Requisitos

- Tener la API corriendo en modo Development.
- Tener un token JWT valido (loguearse con un usuario existente).

## Endpoint

```http
POST /api/utilidades/generar-hash
```

## Body

```json
{
  "clave": "SuperAdmin123!"
}
```

## Response

```json
{
  "exitoso": true,
  "mensaje": "Hash generado correctamente.",
  "datos": "$2a$11$OEcVjIdhJRAEGYdJt8SKLeBcbL0SWLcU1NU6bL4IMZ4AARg1kqLeC"
}
```

## Uso tipico

1. Loguearse con algun usuario valido para obtener un token JWT:
   ```http
   POST /api/auth/login
   {
     "usuario": "SuperAdmin",
     "clave": "SuperAdmin123!"
   }
   ```

2. Con el token en el header `Authorization: Bearer {token}`, llamar a:
   ```http
   POST /api/utilidades/generar-hash
   {
     "clave": "NuevaClaveSegura123!"
   }
   ```

3. Copiar el hash del campo `datos` y usarlo en el script SQL:
   ```sql
   DECLARE @SuperAdminClaveHash NVARCHAR(500) = N'$2a$11$...hash...';
   ```

   O pasarlo como variable SQLCMD:
   ```cmd
   sqlcmd -S localhost -d GestionIntegralDB -i CrearUsuariosYTiposUsuario.sql -v SuperAdminClaveHash="$2a$11$...hash..."
   ```

## Seguridad

- El endpoint `/api/utilidades/generar-hash` retorna `404 Not Found` si la API no esta corriendo en entorno de desarrollo.
- De todas formas, esta protegido con `[Authorize]`, por lo que se requiere un token valido.
- No dejar habilitado en produccion. Si es necesario generar hashes en produccion, hacerlo offline con la misma libreria `BCrypt.Net-Next`.
