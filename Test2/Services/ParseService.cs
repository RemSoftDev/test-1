using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Test2.Models;
using Test2.Services.Parsers;

namespace Test2.Services
{
    public class ParseService : IParseService
    {
        private readonly List<Func<IParser>> _factories = new List<Func<IParser>>();

        public ParseService(IEnumerable<Type> parsersTypes)
        {
            foreach (var parserType in parsersTypes)
            {
                _factories.Add(() => Activator.CreateInstance(parserType) as IParser);
            }
        }

        private IEnumerable<IParser> GetParsers()
        {
            return _factories.Select(factory => factory());
        }

        public async Task<ParseResult> Parse(string fileName, Stream data)
        {
            foreach (var parser in GetParsers())
            {
                data.Position = 0;
                if ((await parser.ParseAsync(fileName, data)) != ReportStatus.UnknownFile)
                {
                    return new ParseResult
                    {
                        Report = parser.Report,
                        Transactions = parser.Transactions
                    };
                }
            }
            return new ParseResult {Report = new Report()};
        }
    }
}
