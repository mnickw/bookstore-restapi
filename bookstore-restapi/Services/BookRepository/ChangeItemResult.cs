using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookstore_restapi.Services.BookRepository
{
    public enum ChangeItemResult
    {
        Ok,
        ConcurrencyException_TryAgain,
        NoItemInDB
    }
}
