using WebApi.Dtos;
using WebApi.Entities;
using Microsoft.EntityFrameworkCore;
using WebApi.CommunicationDbContext;
using Shared;

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
                CurrentStatus = "ReadyForRelease",
                LastUpdatedUtc = DateTime.UtcNow
            };

            _db.Communications.Add(entity);
            await _db.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<IEnumerable<CommunicationDto>> GetAllCommunicationsAsync()
        {
            var comms = await _db.Communications
                .Include(c => c.CommunicationType)
                .Include(c => c.StatusHistory)
                .ToListAsync();

            return comms.Select(c => new CommunicationDto
            {
                Id = c.Id,
                Title = c.Title,
                TypeCode = c.TypeCode,
                CurrentStatus = c.CurrentStatus,
                LastUpdatedUtc = c.LastUpdatedUtc
                // Map other needed properties
            });
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
