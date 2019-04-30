using OneCalendar.Context;

namespace OneCalendar.Interfaces
{
    public interface ISeedService
    {
        CalenderContext _calenderContext { get; }

        void SeedCalenderTasks();
    }
}