using System;
using Geev.Timing;

namespace Geev.AspNetCore.App.Models
{
    public class SimpleDateModel
    {
        public DateTime Date { get; set; }
    }

    public class SimpleDateModel2
    {
        [DisableDateTimeNormalization]
        public DateTime Date { get; set; }
    }

    [DisableDateTimeNormalization]
    public class SimpleDateModel3
    {
        public DateTime Date { get; set; }
    }

    public class SimpleDateModel4
    {
        public DateTime Date { get; set; }
    }
}