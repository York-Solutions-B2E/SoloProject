using App.Shared.Dtos;
using WebApi.Entities;
using Microsoft.EntityFrameworkCore;
using WebApi.CommunicationDbContext;


namespace WebApi.Services {



    public class CommunicationRepository : ICommunicationRepository
    {
        private readonly AppDbContext _db;
        

        public CommunicationRepository(AppDbContext db)
        {
            
            _db = db;
        }

        public async Task<CommunicationDetailsDto?> GetCommunicationDetailsAsync(Guid communicationId)
        {
            var communication = await _db.Communications
                .Include(c => c.CommunicationType)
                .Include(c => c.StatusHistory)
                .FirstOrDefaultAsync(c => c.Id == communicationId);

            if (communication == null)
                return null;
            
            
            
            return new CommunicationDetailsDto
            {
                Id = communication.Id,
                Title = communication.Title,
                TypeCode = communication.CommunicationType.TypeCode,
                CurrentStatusCode = communication.CurrentStatus,
                LastUpdatedUtc = communication.LastUpdatedUtc,
                CreatedAt = communication.CreatedAt, //grab initial creation date
                SourceFileUrl = communication.SourceFileUrl,
                StatusHistory = communication.StatusHistory.Select(sh => new CommunicationStatusHistoryDto //Create history of Comms
                {
                    StatusCode = sh.StatusCode,
                    OccurredUtc = sh.OccurredUtc
                }).ToList()
            };
        }

        public async Task<Guid> CreateCommunicationAsync(CreateCommunicationDto dto)
        {
            
            var communicationType = await _db.CommunicationTypes // Find the CommunicationType entity by TypeCode
                .FirstOrDefaultAsync(ct => ct.TypeCode == dto.TypeCode);

            if (communicationType == null)
                throw new ArgumentException($"Invalid TypeCode: {dto.TypeCode}");

            var entity = new Communication
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                CommunicationTypeId = communicationType.Id, // use surrogate key here
                CurrentStatus = dto.InitialStatusCode,
                LastUpdatedUtc = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
                
            };

            
            var historyEntry = new CommunicationStatusHistory //start tracking status history
            {
                CommunicationId = entity.Id,
                Communication = entity,
                StatusCode = dto.InitialStatusCode,
                OccurredUtc = DateTime.UtcNow
            };


            _db.Communications.Add(entity);
            _db.CommunicationStatusHistories.Add(historyEntry);
            await _db.SaveChangesAsync();

            return entity.Id;
        }

        /* Assign a number of communications for each page based on page size
           Skip over values if pageNumber is over 1 to display next page Comms.
           Lastly Provide total number of elements to keep pageSize Dynamic.
        */
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

            
            return entity == null ? null : new CommunicationDto //return if null otherwise return new DTO
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
            return await _db.Communications //return all existing Communications
                .Select(c => new CommunicationDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    TypeCode = c.CommunicationType.TypeCode,
                    CurrentStatus = c.CurrentStatus,
                    LastUpdatedUtc = c.LastUpdatedUtc,
                    AllowedStatusCodes = c.CommunicationType.Statuses
                        .Select(s => s.StatusCode)
                        .ToList()
                })
                .ToListAsync();
        }


    }
}
