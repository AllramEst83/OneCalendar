using System;

namespace OneCalendar.Models
{
    public class EditedByUser
    {
        public int Id { get; set; }
        public DateTime DateOfEdit { get; set; }
        public int EditedByUserId { get; set; } //<--Manually insert UserId from Auth DB
    }
}