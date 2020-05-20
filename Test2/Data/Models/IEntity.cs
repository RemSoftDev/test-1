using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test2.Data.Models
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
