using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test2.Data.Models;
using Test2.Models;

namespace Test2.Services
{
    public interface ITransactionsService
    {
        Task<IReadOnlyCollection<Transaction>> GetAsync(FilterModel filter);
        Task Upload(IReadOnlyCollection<Transaction> transactions);
    }
}
