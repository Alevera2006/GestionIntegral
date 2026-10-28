namespace GestionIntegral.Application.DTOs;

public class BaseResponseDto<T>
{
    public bool Exitoso { get; set; }
    public string? Mensaje { get; set; }
    public T? Datos { get; set; }

    public static BaseResponseDto<T> Ok(T datos, string? mensaje = null) =>
        new() { Exitoso = true, Mensaje = mensaje, Datos = datos };

    public static BaseResponseDto<T> Error(string mensaje) =>
        new() { Exitoso = false, Mensaje = mensaje };
}
