using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Test2.Models;

namespace Test2.Services
{
    public interface IParseService
    {
        Task<ParseResult> Parse(string fileName, Stream data);
    }
}
