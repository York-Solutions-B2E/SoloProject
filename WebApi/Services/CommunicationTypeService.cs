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
    
    public async Task<List<CommunicationTypeStatusDto>> GetStatusesForTypeAsync(string typeCode)
    {
        var allGlobalStatuses = await _db.GlobalStatuses // assume you have a table for all status codes
            .Select(s => new CommunicationTypeStatusDto
            {
                TypeCode = typeCode, // <-- satisfies the required member
                StatusCode = s.StatusCode,
                Description = s.Description,
                IsValid = false // default, will flip if exists for type
            })
            .ToListAsync();

        var typeStatuses = await _db.CommunicationTypeStatuses
            .Where(ts => ts.TypeCode == typeCode)
            .ToListAsync();

        foreach (var dto in allGlobalStatuses)
        {
            var match = typeStatuses.FirstOrDefault(ts => ts.StatusCode == dto.StatusCode);
            if (match != null)
            {
                dto.IsValid = true;
                dto.Description = match.Description;
            }
        }
        Console.WriteLine($"Global statuses found: {allGlobalStatuses.Count}");
        foreach (var s in allGlobalStatuses)
        {
            Console.WriteLine($" - {s.StatusCode} ({s.Description})");
        }


        return allGlobalStatuses;
    }

    public async Task<bool> UpdateStatusesAsync(string typeCode, List<CommunicationTypeStatusDto> statuses)
    {
        var existing = await _db.CommunicationTypeStatuses
            .Where(ts => ts.TypeCode == typeCode)
            .ToListAsync();

        _db.CommunicationTypeStatuses.RemoveRange(existing);

        var communicationType = await _db.CommunicationTypes.FindAsync(typeCode);

        var toAdd = statuses
            .Where(s => s.IsValid)
            .Select(s => new CommunicationTypeStatus
            {
                TypeCode = typeCode,
                StatusCode = s.StatusCode,
                Description = s.Description,
                CommunicationType = communicationType
            })
            .ToList();

        _db.CommunicationTypeStatuses.AddRange(toAdd);
        await _db.SaveChangesAsync();

        return true;
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
