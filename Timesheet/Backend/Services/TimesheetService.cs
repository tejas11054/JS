using TimeSheet.Models;
using TimeSheet.Repository;

namespace TimeSheet.Services
{
    public class TimesheetService : ITimesheetService
    {
        private readonly ITimesheetRepository _repo;
        public TimesheetService(ITimesheetRepository repo) => _repo = repo;

        public IEnumerable<Timesheet> GetAll() => _repo.GetAll();

        public Timesheet? GetById(int id) => _repo.GetById(id);

        public Timesheet Create(Timesheet entry) => _repo.Add(entry);

        public Timesheet Update(Timesheet entry)
        {
            if (!_repo.Exists(entry.Id)) throw new KeyNotFoundException("Timesheet not found");
            return _repo.Update(entry);
        }

        public void Delete(int id) => _repo.Delete(id);

        public IEnumerable<Timesheet> GetByUser(int userId) => _repo.GetByUser(userId);

        public IEnumerable<Timesheet> GetByProject(int projectId) => _repo.GetByProject(projectId);

        public IEnumerable<Timesheet> GetByDateRange(DateTime from, DateTime to, int? userId = null, int? projectId = null)
            => _repo.GetByDateRange(from, to, userId, projectId);
    }
}
