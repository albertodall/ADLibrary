using System.Data;
using System.Data.Entity;

namespace AD.ORM.EntityFramework
{
    public class HiLoGenerator<TDbContext> : IHiLoGenerator<long> where TDbContext : DbContext, new()
    {
        private readonly int _maxLo; 
        private int _currentLo; 
        private long _currentHi = -1; 
        private static readonly object ConcurrencyLock = new object(); 
    
        public HiLoGenerator(int maxLo)
        {
            _maxLo = maxLo;
        } 
    
        public long GetIdentifier()
        {
            long result; 
            lock (ConcurrencyLock)
            {
                if (_currentHi == -1)
                {
                    MoveNextHi();
                } 
                
                if (_currentLo == _maxLo)
                {
                    _currentLo = 0; 
                    MoveNextHi();
                } 
                
                result = (_currentHi * _maxLo) + _currentLo; 
                _currentLo++;
            } 
            return result;
        } 
        
        private void MoveNextHi()
        {
            using (var dbContext = new TDbContext())
            {
                if (dbContext.Database.Connection.State == ConnectionState.Closed)
                {
                    dbContext.Database.Connection.Open();
                }

                using (var tx = dbContext.Database.Connection.BeginTransaction())
                {
                    using (var updateNextHiCommand = dbContext.Database.Connection.CreateCommand())
                    {
                        using (var getCurrentHiCommand = dbContext.Database.Connection.CreateCommand())
                        {
                            updateNextHiCommand.Transaction = tx;
                            updateNextHiCommand.CommandText = "SELECT next_hi FROM entityframework_unique_key";
                            _currentHi = (int) updateNextHiCommand.ExecuteScalar();
                            getCurrentHiCommand.Transaction = tx;
                            getCurrentHiCommand.CommandText = "UPDATE entityframework_unique_key SET next_hi = next_hi + 1";
                            getCurrentHiCommand.ExecuteNonQuery();
                            tx.Commit();
                        }
                    }
                }
            }
        }
    }
}