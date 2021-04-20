using System.Collections.Generic;

namespace Ckode.Dapper.Repository
{
    internal interface IRepository<TPrimaryKeyRecord, TRecord>
        where
            TPrimaryKeyRecord : TableRecord
        where TRecord : TPrimaryKeyRecord
    {
        TRecord Get(TPrimaryKeyRecord record);
        TRecord Insert(TRecord record);
        TRecord Update(TRecord record);
        TRecord Delete(TPrimaryKeyRecord record);
        IEnumerable<TRecord> GetAll();
    }
}
