using System;
using Web.Framework.Mvc;

namespace Web.Models.Home
{
    public partial class TeklifTakvimiModel : TemelTSModel
    {
        public int TaskID { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public string StartTimezone { get; set; }
        public DateTime End { get; set; }
        public string EndTimezone { get; set; }
        public string Description { get; set; }
        public string RecurrenceID { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
        public int OwnerID { get; set; }
        public bool IsAllDay { get; set; }
    }
}