using App.Shared.Dtos;
using WebApi.Entities;
using Microsoft.EntityFrameworkCore;
using WebApi.CommunicationDbContext;


namespace WebApi.Services {



    public class CommunicationService : ICommunicationService
    {
        private readonly AppDbContext _db;

        public CommunicationService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Guid> CreateCommunicationAsync(CreateCommunicationDto dto)
        {
            var communicationTypeExists = await _db.CommunicationTypes
            .AnyAsync(ct => ct.TypeCode == dto.TypeCode);

            if (!communicationTypeExists)
                throw new ArgumentException($"Invalid TypeCode: {dto.TypeCode}");

            var entity = new Communication
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                TypeCode = dto.TypeCode,
                CurrentStatus = dto.InitialStatusCode,
                LastUpdatedUtc = DateTime.UtcNow
            };

            _db.Communications.Add(entity);
            await _db.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<PaginatedResult<CommunicationDto>> GetPaginatedCommunicationsAsync(int pageNumber, int pageSize)
        {
            var query = _db.Communications
                .OrderByDescending(c => c.LastUpdatedUtc); // sort as needed

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CommunicationDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    TypeCode = c.TypeCode,
                    CurrentStatus = c.CurrentStatus,
                    LastUpdatedUtc = c.LastUpdatedUtc
                })
                .ToListAsync();

            return new PaginatedResult<CommunicationDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }



        public async Task<CommunicationDto?> GetCommunicationAsync(Guid id)
        {
            var entity = await _db.Communications.FindAsync(id);

            return entity == null ? null : new CommunicationDto
            {
                Id = entity.Id,
                Title = entity.Title,
                TypeCode = entity.TypeCode,
                CurrentStatus = entity.CurrentStatus,
                LastUpdatedUtc = entity.LastUpdatedUtc
            };
        }
    }
}
