// Services/CommunicationTypeService.cs
using Microsoft.EntityFrameworkCore;
using WebApi.CommunicationDbContext;
using App.Shared.Dtos;
using WebApi.Entities;

public class CommunicationTypeService : ICommunicationTypeService
{
    private readonly AppDbContext _db;

    public CommunicationTypeService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CommunicationTypeDto>> GetAllAsync()
    {
        return await _db.CommunicationTypes
            .Include(t => t.Statuses)
            .Select(t => new CommunicationTypeDto
            {
                TypeCode = t.TypeCode,
                DisplayName = t.DisplayName
            })
            .ToListAsync();
    }

    public async Task<CommunicationTypeDto?> CreateAsync(CommunicationTypeDto dto)
    {
        var entity = new CommunicationType
        {
            TypeCode = dto.TypeCode,
            DisplayName = dto.DisplayName
        };

        _db.CommunicationTypes.Add(entity);
        await _db.SaveChangesAsync();

        return new CommunicationTypeDto
        {
            TypeCode = entity.TypeCode,
            DisplayName = dto.DisplayName
        };
    }
    public async Task<bool> UpdateAsync(CommunicationTypeDto dto)
    {
        var entity = await _db.CommunicationTypes
            .FirstOrDefaultAsync(c => c.TypeCode == dto.TypeCode);

        if (entity == null)
            return false;

        entity.DisplayName = dto.DisplayName;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string typeCode)
    {
        var entity = await _db.CommunicationTypes.FindAsync(typeCode);
        if (entity == null) return false;

        _db.CommunicationTypes.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
