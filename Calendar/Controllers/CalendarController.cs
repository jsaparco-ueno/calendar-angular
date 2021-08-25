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
            return await CalendarMethods.GetAllEventsAsync(connectionString);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<CalendarEvent> GetOne(Guid id)
        {
            var result = new CalendarEvent();
            await using (var db = new SqlConnection(connectionString))
            {
                result = (await db.QueryFirstAsync<CalendarEvent>(@"SELECT TOP 1 * FROM CalendarEvent WHERE id = @id", new {id}));
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
        public async Task<IActionResult> Create(CalendarEvent calendarEvent)
        {
            if (calendarEvent.Title == string.Empty)
                return BadRequest("The event title cannot be empty.");

            if (CalendarMethods.CheckForOverlaps(calendarEvent, await CalendarMethods.GetAllEventsAsync(connectionString)) == true)
                return BadRequest("The event overlaps one or more other events.  Events may not overlap.");

            if (calendarEvent.EndTime < calendarEvent.StartTime)
                return BadRequest("The event's end date is before its start date.  Events cannot end before they begin.");

            await using(var db = new SqlConnection(connectionString))
            {
                await db.QueryAsync(@"INSERT INTO [dbo].[CalendarEvent]
                    ([Id],[Title],[Notes],[StartTime],[EndTime],[IsDeleted])
                    VALUES
                    (NEWID(),@title,@notes,@startTime,@endTime,0)",
                    new {calendarEvent.Title, calendarEvent.Notes, calendarEvent.StartTime, calendarEvent.EndTime});
            }

            return Ok();
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update(CalendarEvent calendarEvent)
        {
            if (CalendarMethods.CheckForOverlaps(calendarEvent, await CalendarMethods.GetAllEventsAsync(connectionString)) == true)
            return BadRequest("The event overlaps one or more other events.  Events may not overlap.");
            {
                await using(var db =new SqlConnection(connectionString))
                {
                    await db.QueryAsync(@"UPDATE [dbo].[CalendarEvent] SET
                        Title = @title, Notes = @notes, StartTime = @startTime, EndTime = @endTime
                        WHERE Id = @id",
                        new {calendarEvent.Title, calendarEvent.Notes, calendarEvent.StartTime, calendarEvent.EndTime, calendarEvent.Id});
                }
            }
            return Ok();
        }
    }

    public static class CalendarMethods
    {
        public static async Task<IEnumerable<CalendarEvent>> GetAllEventsAsync(string connectionString)
        {
            var result = new List<CalendarEvent>();
            await using (var db = new SqlConnection(connectionString))
            {
                result = (await db.QueryAsync<CalendarEvent>(@"SELECT * FROM CalendarEvent where IsDeleted = 0")).ToList();
            }
            return result;
        }
        public static bool CheckForOverlaps(CalendarEvent calendarEvent, IEnumerable<CalendarEvent> allEvents)
        {
             return allEvents.FirstOrDefault<CalendarEvent>(ce => ce.Id != calendarEvent.Id && ce.StartTime <= calendarEvent.EndTime && ce.EndTime >= calendarEvent.StartTime) != null;
        }
    }
}
