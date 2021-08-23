using System;
using Xunit;
using calendar.Controllers;
using calendar;
using System.Collections.Generic;

namespace CalendarTests
{
    public class UnitTest1
    {
        [Fact]
        public void FindOverlap()
        {
            var result = CalendarMethods.CheckForOverlaps(SampleEvent1, new List<CalendarEvent> {SampleEvent2,SampleEvent3});
            Assert.True(result);
        }

        private readonly CalendarEvent SampleEvent1 = new CalendarEvent {
            Id = new Guid("580B1474-5E04-4AE1-A02E-ADF7BA6331DD"),
            Title = "Sample Event 1",
            Notes = "Leverage agile frameworks to provide a robust synopsis for high level overviews. Iterative approaches to corporate strategy foster collaborative thinking to further the overall value proposition. Organically grow the holistic world view of disruptive innovation via workplace diversity and empowerment.",
            StartTime = new DateTime(2021, 8, 22, 17, 0, 0),
            EndTime = new DateTime(2021, 8, 22, 20, 0, 0),
            IsDeleted = false
        };

        private readonly CalendarEvent SampleEvent2 = new CalendarEvent {
            Id = new Guid("7758681A-264E-4350-B6FB-2681097A55F9"),
            Title = "Sample Event 2",
            Notes = "Bring to the table win-win survival strategies to ensure proactive domination. At the end of the day, going forward, a new normal that has evolved from generation X is on the runway heading towards a streamlined cloud solution. User generated content in real-time will have multiple touchpoints for offshoring.",
            StartTime = new DateTime(2021, 8, 24, 17, 0, 0),
            EndTime = new DateTime(2021, 8, 24, 18, 0, 0),
            IsDeleted = false
        };

        private readonly CalendarEvent SampleEvent3 = new CalendarEvent {
            Id = new Guid("9BD9E370-081B-4747-AE1C-2B25C19BAB97"),
            Title = "Sample Event 3",
            Notes = "Capitalize on low hanging fruit to identify a ballpark value added activity to beta test. Override the digital divide with additional clickthroughs from DevOps. Nanotechnology immersion along the information highway will close the loop on focusing solely on the bottom line.",
            StartTime = new DateTime(2021, 8, 22, 16, 0, 0),
            EndTime = new DateTime(2021, 8, 22, 18, 0, 0),
            IsDeleted = false
        };
    }
}
