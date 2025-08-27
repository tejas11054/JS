using TimeSheet.Data;
using TimeSheet.Models;

namespace TimeSheet.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;
        public ProjectRepository(AppDbContext context) => _context = context;

        public IEnumerable<Project> GetAll() => _context.Projects.ToList();

        public Project? GetById(int id) => _context.Projects.Find(id);

        public Project Add(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
            return project;
        }

        public Project Update(Project project)
        {
            _context.Projects.Update(project);
            _context.SaveChanges();
            return project;
        }

        public void Delete(int id)
        {
            var proj = _context.Projects.Find(id);
            if (proj != null)
            {
                _context.Projects.Remove(proj);
                _context.SaveChanges();
            }
        }

        public bool Exists(int id) => _context.Projects.Any(p => p.Id == id);
    }
}
