using AutoMapper;
using GestionIntegral.Application.DTOs;
using GestionIntegral.Application.DTOs.ValoresCartera;
using GestionIntegral.Application.Interfaces.Services;
using GestionIntegral.Domain.Interfaces.Repositories;

namespace GestionIntegral.Application.Services;

public class ValorCarteraService : IValorCarteraService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ValorCarteraService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponseDto<IEnumerable<ValorCarteraResponseDto>>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var valores = await _unitOfWork.ValoresCartera.GetAllWithDetailsAsync(cancellationToken);
        return BaseResponseDto<IEnumerable<ValorCarteraResponseDto>>.Ok(_mapper.Map<IEnumerable<ValorCarteraResponseDto>>(valores));
    }
}
