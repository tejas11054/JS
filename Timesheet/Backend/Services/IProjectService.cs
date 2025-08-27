using TimeSheet.Models;

namespace TimeSheet.Services
{
    public interface IProjectService
    {
        IEnumerable<Project> GetAll();
        Project? GetById(int id);
        Project Create(Project project);
        Project Update(Project project);
        void Delete(int id);
    }
}
