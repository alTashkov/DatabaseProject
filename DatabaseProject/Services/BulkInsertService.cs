using DatabaseProject.Data;

namespace DatabaseProject.Services
{
    public class BulkInsertService<T> where T : class
    {
        private readonly SocialMediaContext _context;

        public BulkInsertService(SocialMediaContext context)
        {
            _context = context;
        }
        public void InsertInBatches(List<T> entities, int batchSize = 10)
        {
            for (int i = 0; i < entities.Count; i += batchSize)
            {
                var batch = entities.Skip(i).Take(batchSize).ToList();
                _context.Set<T>().AddRange(batch);
                _context.SaveChanges();
            }
        }
    }
}
