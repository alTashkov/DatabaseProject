using DatabaseProject.Data;
using Microsoft.Extensions.Logging;

namespace DatabaseProject.Services
{
    public class BulkInsertService<T> : IServiceWithLogger where T : class
    {
        private readonly SocialMediaContext _context;

        public ILogger Logger { get; }

        public BulkInsertService(SocialMediaContext context, ILogger<BulkInsertService<T>> logger)
        {
            _context = context;
            Logger = logger;
        }

        /// <summary>
        /// Inserts a fixed amount of entities into the database.
        /// </summary>
        /// <param name="entities">The data list of entities to be inserted into the database.</param>
        /// <param name="batchSize">The amount of entities to be inserted from the list.</param>
        /// <returns>
        /// <b>True</b> if the entities were successfully inserted into the database;<br></br>
        /// <b>False</b> if the data list was invalid.
        /// </returns>
        public bool InsertInBatches(List<T> entities, int batchSize = 10)
        {
            for (int i = 0; i < entities.Count; i += batchSize)
            {
                var batch = entities.Skip(i).Take(batchSize).ToList();
                _context.Set<T>().AddRange(batch);
                _context.SaveChanges();
                return true;
            }

            Logger.LogInformation("Entities ({EntityCount}) were inserted in batches.", entities.Count);
            return true;
        }
    }
}
