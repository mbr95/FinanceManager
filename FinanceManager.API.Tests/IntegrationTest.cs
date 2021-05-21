using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FinanceManager.API.IntegrationTests
{
    public class IntegrationTest : IClassFixture<FinanceManagerAppFactory<Startup>>
    {
        protected readonly FinanceManagerAppFactory<Startup> _financeManagerAppFactory;

        protected IntegrationTest(FinanceManagerAppFactory<Startup> financeManagerAppFactory)
        {
            _financeManagerAppFactory = financeManagerAppFactory;
        }
    }
}
