using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Test2.Data;
using Test2.Data.Models;
using Test2.Data.Repositories;
using Test2.Models;

namespace Test2.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly IRepository<Transaction, string> _repository;
        private readonly DataContext _context;

        public TransactionsService(IRepository<Transaction, string> repository, DataContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IReadOnlyCollection<Transaction>> GetAsync(FilterModel filter)
        {
            var query = _repository.Query;

            if (filter.Status.HasValue)
                query = query.Where(t => t.Status == filter.Status.Value);
            if (!string.IsNullOrWhiteSpace(filter.Currency))
                query = query.Where(t => t.Currency == filter.Currency);
            if (filter.FromDate.HasValue)
                query = query.Where(t => t.Date >= filter.FromDate.Value);
            if (filter.ToDate.HasValue)
                query = query.Where(t => t.Date <= filter.ToDate.Value);

            query = query.OrderByDescending(t => t.Date);
            if (filter.PageSize.HasValue)
            {
                if (filter.Page.HasValue && filter.Page.Value > 1)
                    query = query.Skip((filter.Page.Value - 1) * filter.PageSize.Value);
                query = query.Take(filter.PageSize.Value);
            }

            return await query.ToListAsync();
        }

        public async Task Upload(IReadOnlyCollection<Transaction> transactions)
        {
            var exists = await _repository.CheckExists(transactions.Select(t => t.Id));

            foreach (var transaction in transactions)
            {
                if (exists.Contains(transaction.Id))
                    _repository.Update(transaction);
                else
                    _repository.Add(transaction);
            }

            await _context.SaveChangesAsync();
        }
    }
}
