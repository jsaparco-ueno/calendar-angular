using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace calendar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly ILogger<CalendarController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public CalendarController(ILogger<CalendarController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            connectionString = _configuration["ConnectionStrings:CalendarDB"];
        }

        [HttpGet]
        public async Task<IEnumerable<CalendarEvent>> Get()
        {
            var result = new List<CalendarEvent>();
            await using (var db = new SqlConnection(connectionString))
            {
                result = (await db.QueryAsync<CalendarEvent>(@"select * from CalendarEvent")).ToList();
            }

            return result;
        }

        [HttpDelete]
        [Route("delete/{id:Guid}")]
        public async Task Delete(Guid id)
        {
            await using (var db = new SqlConnection(connectionString))
            {
                await db.QueryAsync(@"update CalendarEvent set isDeleted = 1 where Id = @id", new {id});
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task Create(CalendarEvent calendarEvent)
        {
            await using(var db = new SqlConnection(connectionString))
            {
                await db.QueryAsync(@"INSERT INTO[dbo].[CalendarEvent]
                    ([Id],[Title],[Notes],[StartTime],[EndTime],[IsDeleted])
                    VALUES
                    (NEWID(),@title,@notes,@startTime,@endTime,0)",
                    new {calendarEvent.Title, calendarEvent.Notes, calendarEvent.StartTime, calendarEvent.EndTime});
            }
        }
    }
}
