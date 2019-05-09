using System;

namespace OneCalendar.ViewModels
{
    public class RemoveUserFromGroupViewModel
    {
        public Guid UserId { get; set; }
        public int GroupId { get; set; }
    }
}