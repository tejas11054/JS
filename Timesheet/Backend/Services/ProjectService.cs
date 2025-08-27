using TimeSheet.Models;
using TimeSheet.Repository;

namespace TimeSheet.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repo;
        public ProjectService(IProjectRepository repo) => _repo = repo;

        public IEnumerable<Project> GetAll() => _repo.GetAll();

        public Project? GetById(int id) => _repo.GetById(id);

        public Project Create(Project project) => _repo.Add(project);

        public Project Update(Project project)
        {
            if (!_repo.Exists(project.Id)) throw new KeyNotFoundException("Project not found");
            return _repo.Update(project);
        }

        public void Delete(int id) => _repo.Delete(id);
    }
}
