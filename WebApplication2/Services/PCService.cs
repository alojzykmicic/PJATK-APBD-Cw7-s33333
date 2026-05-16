using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Services;
using WebApplication2.Models;
using WebApplication2.DTOs;
using WebApplication2.Infrastructure;

namespace WebApplication2.Services;

public class PCService : IPCService
{
    private readonly AppDbContext _context;
    
    public PCService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PcDto>> GetAllPCsAsync()
    {
        return await _context.PCs
            .Select(pc => new PcDto
            {
                Id = pc.Id,
                Name = pc.Name,
                Weight = pc.Weight,
                Warranty = pc.Warranty,
                CreatedAt = pc.CreatedAt,
                Stock = pc.Stock
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<PcComponentDetailDto>> GetPCComponentsAsync(int id)
    {
        var pcExists = await _context.PCs.AnyAsync(p => p.Id == id);
        if (!pcExists) return null;
        
        return await _context.PcComponents
            .Include(pc => pc.Component)
                .ThenInclude(c => c.Manufacturer)
            .Include(pc => pc.Component)
                .ThenInclude(c => c.Type)
            .Where(pc => pc.PCId == id)
            .Select(pc => new PcComponentDetailDto
            {
                ComponentCode = pc.ComponentCode,
                ComponentName = pc.Component.Name,
                Amount = pc.Amount,
                Manufacturer = pc.Component.Manufacturer.FullName,
                Type = pc.Component.Type.Name
            })
            .ToListAsync();
    }
    
    public async Task<PcDto> CreatePCAsync(PCRequestDto dto)
    {
        var pc = new PC
        {
            Name = dto.Name,
            Weight = dto.Weight,
            Warranty = dto.Warranty,
            CreatedAt = dto.CreatedAt,
            Stock = dto.Stock
        };

        _context.PCs.Add(pc);
        await _context.SaveChangesAsync();

        return new PcDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        };
    }
    
    public async Task<bool> UpdatePCAsync(int id, PCRequestDto dto)
    {
        var pc = await _context.PCs.FindAsync(id);
        if (pc == null) return false;

        pc.Name = dto.Name;
        pc.Weight = dto.Weight;
        pc.Warranty = dto.Warranty;
        pc.CreatedAt = dto.CreatedAt;
        pc.Stock = dto.Stock;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePCAsync(int id)
    {
        var pc = await _context.PCs.FindAsync(id);
        if (pc == null) return false;

        _context.PCs.Remove(pc);
        await _context.SaveChangesAsync();
        return true;
    }
}