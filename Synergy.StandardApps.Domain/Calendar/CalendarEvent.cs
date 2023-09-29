using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Domain.Calendar
{
    public class CalendarEvent
    {
        public long Id { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public int Color { get; set; }
        public int Month { get; set; } = 1;
        public int Day { get; set; } = 1;
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
