﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCalendar.ResponseModels
{
    public class TaskResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool AllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

    }
}
