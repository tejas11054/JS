using Microsoft.EntityFrameworkCore;
using TimeSheet.Data;
using TimeSheet.Models;

namespace TimeSheet.Repository
{
    public class TimesheetRepository : ITimesheetRepository
    {
        private readonly AppDbContext _context;
        public TimesheetRepository(AppDbContext context) => _context = context;

        public IEnumerable<Timesheet> GetAll() =>
            _context.Timesheets
                    .Include(t => t.User)
                    .Include(t => t.Project)
                    .OrderByDescending(t => t.Date)
                    .ToList();

        public Timesheet? GetById(int id) =>
            _context.Timesheets
                    .Include(t => t.User)
                    .Include(t => t.Project)
                    .FirstOrDefault(t => t.Id == id);

        public Timesheet Add(Timesheet entry)
        {
            _context.Timesheets.Add(entry);
            _context.SaveChanges();
            return entry;
        }

        public Timesheet Update(Timesheet entry)
        {
            _context.Timesheets.Update(entry);
            _context.SaveChanges();
            return entry;
        }

        public void Delete(int id)
        {
            var item = _context.Timesheets.Find(id);
            if (item != null)
            {
                _context.Timesheets.Remove(item);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Timesheet> GetByUser(int userId) =>
            _context.Timesheets
                    .Include(t => t.User)
                    .Include(t => t.Project)
                    .Where(t => t.UserId == userId)
                    .OrderByDescending(t => t.Date)
                    .ToList();

        public IEnumerable<Timesheet> GetByProject(int projectId) =>
            _context.Timesheets
                    .Include(t => t.User)
                    .Include(t => t.Project)
                    .Where(t => t.ProjectId == projectId)
                    .OrderByDescending(t => t.Date)
                    .ToList();

        public IEnumerable<Timesheet> GetByDateRange(DateTime from, DateTime to, int? userId = null, int? projectId = null)
        {
            var q = _context.Timesheets
                            .Include(t => t.User)
                            .Include(t => t.Project)
                            .Where(t => t.Date.Date >= from.Date && t.Date.Date <= to.Date);

            if (userId.HasValue) q = q.Where(t => t.UserId == userId.Value);
            if (projectId.HasValue) q = q.Where(t => t.ProjectId == projectId.Value);

            return q.OrderByDescending(t => t.Date).ToList();
        }

        public bool Exists(int id) => _context.Timesheets.Any(t => t.Id == id);
    }
}
