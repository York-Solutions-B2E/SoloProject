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
    
    public async Task<List<CommunicationTypeStatusDto>> GetStatusesForTypeAsync(Guid id)
    {
        var communicationType = await _db.CommunicationTypes
            .Include(ct => ct.Statuses)
            .FirstOrDefaultAsync(ct => ct.Id == id);

        var allGlobalStatuses = await _db.GlobalStatuses // assume you have a table for all status codes
            .Select(s => new CommunicationTypeStatusDto
            {
                CommunicationTypeId = id,
                TypeCode = communicationType.TypeCode, // <-- satisfies the required member
                StatusCode = s.StatusCode,
                Description = s.Description,
                IsValid = false // default, will flip if exists for type
            })
            .ToListAsync();

        


        if (communicationType != null)
        {
            foreach (var dto in allGlobalStatuses)
            {
                var match = communicationType.Statuses.FirstOrDefault(ts => ts.StatusCode == dto.StatusCode);
                if (match != null)
                {
                    dto.IsValid = true;
                    dto.Description = match.Description;
                }
            }
        }

        return allGlobalStatuses;
    }

    public async Task<bool> UpdateStatusesAsync(Guid id, List<CommunicationTypeStatusDto> statuses)
    {
        var communicationType = await _db.CommunicationTypes
            .Include(ct => ct.Statuses)
            .FirstOrDefaultAsync(ct => ct.Id == id);

        if (communicationType == null) return false;

        // Remove old statuses
        _db.CommunicationTypeStatuses.RemoveRange(communicationType.Statuses);

        // Add new ones
        var toAdd = statuses
            .Where(s => s.IsValid)
            .Select(s => new CommunicationTypeStatus
            {
                CommunicationTypeId = communicationType.Id, // FK by surrogate key
                StatusCode = s.StatusCode,
                Description = s.Description,
            })
            .ToList();

        communicationType.Statuses = toAdd;

        await _db.SaveChangesAsync();
        return true;
    }
    
    public async Task<IEnumerable<CommunicationTypeDto>> GetAllAsync()
    {
        return await _db.CommunicationTypes
            .Include(t => t.Statuses)
            .Select(t => new CommunicationTypeDto
            {
                Id = t.Id,
                TypeCode = t.TypeCode,
                DisplayName = t.DisplayName
            })
            .ToListAsync();
    }

    public async Task<CommunicationTypeDto?> CreateAsync(CommunicationTypeDto dto)
    {
        var entity = new CommunicationType
        {
            Id = Guid.NewGuid(),
            TypeCode = dto.TypeCode,
            DisplayName = dto.DisplayName
        };

        _db.CommunicationTypes.Add(entity);
        await _db.SaveChangesAsync();

        return new CommunicationTypeDto
        {
            Id = entity.Id,
            TypeCode = entity.TypeCode,
            DisplayName = dto.DisplayName
        };
    }
    public async Task<bool> UpdateAsync(CommunicationTypeDto dto)
    {
        var entity = await _db.CommunicationTypes
            .FirstOrDefaultAsync(c => c.Id == dto.Id);

        if (entity == null)
            return false;

        // Allow updating TypeCode and DisplayName
        // If you want to allow changing TypeCode (string), update that only:
        if (!string.Equals(entity.TypeCode, dto.TypeCode, StringComparison.OrdinalIgnoreCase))
        {
            
            entity.TypeCode = dto.TypeCode;
            // No need to update Communications — they reference by CommunicationTypeId (surrogate key)
        }

        entity.DisplayName = dto.DisplayName;

        await _db.SaveChangesAsync();
        return true;
    }


    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _db.CommunicationTypes.FindAsync(id);
        if (entity == null) return false;

        _db.CommunicationTypes.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
