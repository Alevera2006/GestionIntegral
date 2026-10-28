using GestionIntegral.Application.DTOs.ConfiguracionBase;

namespace GestionIntegral.Application.DTOs.MotivosAnulacion;

public class ActualizarMotivoAnulacionRequestDto : ActualizarConfiguracionRequestDto
{
    public int IdMotivoAnulacion { get; set; }
}
