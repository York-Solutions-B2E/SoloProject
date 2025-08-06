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

        public async Task<CommunicationDetailsDto?> GetCommunicationDetailsAsync(Guid communicationId)
        {
            var communication = await _db.Communications
                .Include(c => c.CommunicationType)
                .Include(c => c.StatusHistory)
                .FirstOrDefaultAsync(c => c.Id == communicationId);


            Console.WriteLine($"History count for {communicationId}: {communication?.StatusHistory?.Count}");

            
            

            if (communication == null)
                return null;
            
            return new CommunicationDetailsDto
            {
                Id = communication.Id,
                Title = communication.Title,
                TypeCode = communication.CommunicationType.TypeCode,
                CurrentStatusCode = communication.CurrentStatus,
                CreatedAt = communication.LastUpdatedUtc,
                SourceFileUrl = communication.SourceFileUrl,
                StatusHistory = communication.StatusHistory.Select(sh => new CommunicationStatusHistoryDto
                {
                    StatusCode = sh.StatusCode,
                    OccurredUtc = sh.OccurredUtc
                }).ToList()
            };
        }

        public async Task<Guid> CreateCommunicationAsync(CreateCommunicationDto dto)
        {
            // Find the CommunicationType entity by TypeCode
            var communicationType = await _db.CommunicationTypes
                .FirstOrDefaultAsync(ct => ct.TypeCode == dto.TypeCode);

            if (communicationType == null)
                throw new ArgumentException($"Invalid TypeCode: {dto.TypeCode}");

            var entity = new Communication
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                CommunicationTypeId = communicationType.Id,   // use surrogate key here
                CurrentStatus = dto.InitialStatusCode,
                LastUpdatedUtc = DateTime.UtcNow,
                // Assign other properties as needed
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
                    TypeCode = c.CommunicationType.TypeCode,
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
            var entity = await _db.Communications
                .Include(c => c.CommunicationType)
                .FirstOrDefaultAsync(c => c.Id == id);

            return entity == null ? null : new CommunicationDto
            {
                Id = entity.Id,
                Title = entity.Title,
                TypeCode = entity.CommunicationType.TypeCode,
                CurrentStatus = entity.CurrentStatus,
                LastUpdatedUtc = entity.LastUpdatedUtc
            };
        }

        public async Task<List<CommunicationDto>> GetAllAsync()
        {
            return await _db.Communications
                .Select(c => new CommunicationDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    TypeCode = c.CommunicationType.TypeCode,
                    CurrentStatus = c.CurrentStatus,
                    LastUpdatedUtc = c.LastUpdatedUtc
                })
                .ToListAsync();
        }


    }
}
