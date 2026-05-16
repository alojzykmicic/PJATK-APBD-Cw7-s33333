
using WebApplication2.DTOs;

namespace WebApplication2.Services;

public interface IPCService
{
    Task<IEnumerable<PcDto>> GetAllPCsAsync();
    Task<IEnumerable<PcComponentDetailDto>> GetPCComponentsAsync(int id);
    Task<PcDto> CreatePCAsync(PCRequestDto dto);
    Task<bool> UpdatePCAsync(int id, PCRequestDto dto);
    Task<bool> DeletePCAsync(int id);
}