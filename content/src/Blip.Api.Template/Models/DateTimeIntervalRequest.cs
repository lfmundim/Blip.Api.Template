using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blip.Api.Template.Models
{
    public class DateTimeIntervalRequest
    {
        [Required]
        public HashSet<DayOfWeek> DaysOfWeek { get; set; }

        [Required]
        public TimeSpan BeginHour { get; set; }

        [Required]
        public TimeSpan EndHour { get; set; }
    }
}
