using FinanceManager.API.Data;
using FinanceManager.API.Requests.v1;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FinanceManager.API.Responses.v1;
using System.Text;
using System.Linq;
using FinanceManager.API.Domain.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using FinanceManager.API.Extensions;
using Xunit;

namespace FinanceManager.API.IntegrationTests
{
    public class IntegrationTest : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private readonly WebApplicationFactory<Startup> _webApplicationFactory;

        protected IntegrationTest(WebApplicationFactory<Startup> webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }
        
        public void Dispose()
        {
            
        }
    }
}
