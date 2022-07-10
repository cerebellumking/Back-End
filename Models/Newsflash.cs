using System;
using System.Collections.Generic;

#nullable disable

namespace Back_End.Models
{
    public partial class Newsflash
    {
        public int NewsFlashId { get; set; }
        public string NewsFlashTitle { get; set; }
        public DateTime NewsFlashDate { get; set; }
        public string NewsFlashTag { get; set; }
        public string NewsFlashContent { get; set; }
        public string NewsFlashImage { get; set; }
        public string NewsFlashRegion { get; set; }
        public bool? NewsFlashVisible { get; set; }
        public string NewsFlashSummary { get; set; }
    }
}
