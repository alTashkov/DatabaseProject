using DatabaseProject.Data;
using Microsoft.Extensions.Logging;

namespace DatabaseProject.Services
{
    public class BulkInsertService<T> where T : class
    {
        private readonly ILogger<BulkInsertService<T>> _logger;

        private readonly SocialMediaContext _context;

        public BulkInsertService(SocialMediaContext context, ILogger<BulkInsertService<T>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void InsertInBatches(List<T> entities, int batchSize = 10)
        {
            for (int i = 0; i < entities.Count; i += batchSize)
            {
                var batch = entities.Skip(i).Take(batchSize).ToList();
                _context.Set<T>().AddRange(batch);
                _context.SaveChanges();
            }

            _logger.LogInformation("Entities ({EntityCount}) were inserted in batches.", entities.Count);
        }
    }
}
