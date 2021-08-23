using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace calendar5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly ILogger<CalendarController> _logger;
        private readonly IConfiguration _configuration;

        public CalendarController(ILogger<CalendarController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<CalendarEvent>> Get()
        {
            var result = new List<CalendarEvent>();
            await using (var db = new SqlConnection(_configuration["ConnectionStrings:CalendarDB"]))
            {
                result = (await db.QueryAsync<CalendarEvent>(@"select * from CalendarEvent")).ToList();
            }

            return result;
        }
    }
}
