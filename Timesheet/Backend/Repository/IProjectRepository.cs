using TimeSheet.Models;

namespace TimeSheet.Repository
{
    public interface IProjectRepository
    {
        IEnumerable<Project> GetAll();
        Project? GetById(int id);
        Project Add(Project project);
        Project Update(Project project);
        void Delete(int id);
        bool Exists(int id);
    }
}
