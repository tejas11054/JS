using TimeSheet.Models;

namespace TimeSheet.Services
{
    public interface ITimesheetService
    {
        IEnumerable<Timesheet> GetAll();
        Timesheet? GetById(int id);
        Timesheet Create(Timesheet entry);
        Timesheet Update(Timesheet entry);
        void Delete(int id);

        IEnumerable<Timesheet> GetByUser(int userId);
        IEnumerable<Timesheet> GetByProject(int projectId);
        IEnumerable<Timesheet> GetByDateRange(DateTime from, DateTime to, int? userId = null, int? projectId = null);
    }
}
