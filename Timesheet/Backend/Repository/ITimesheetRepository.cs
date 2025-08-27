using TimeSheet.Models;

namespace TimeSheet.Repository
{
    public interface ITimesheetRepository
    {
        IEnumerable<Timesheet> GetAll();
        Timesheet? GetById(int id);
        Timesheet Add(Timesheet entry);
        Timesheet Update(Timesheet entry);
        void Delete(int id);

        IEnumerable<Timesheet> GetByUser(int userId);
        IEnumerable<Timesheet> GetByProject(int projectId);
        IEnumerable<Timesheet> GetByDateRange(DateTime from, DateTime to, int? userId = null, int? projectId = null);
        bool Exists(int id);
    }
}
