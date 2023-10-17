using API.Models;

namespace API.Services
{
    public interface IStudentService
    {
        Task<bool> AddStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int id);
        Task<bool> EditStudentAsync(Student student);
        Task<Student> GetStudentAsync(int id);
        Task<List<Student>> GetStudentsAsync();
    }
}
