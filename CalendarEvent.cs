using System;

namespace calendar5
{
    public class CalendarEvent
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
