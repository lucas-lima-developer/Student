using API.DbContexts;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddStudentAsync(Student student)
        {
            _context.Students.Add(student);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            
            if (student == null)
            {
                return false;
            }

            _context.Students.Remove(student);

            var result = await _context.SaveChangesAsync();
            
            return result > 0;
        }

        public async Task<bool> EditStudentAsync(Student student)
        {
            _context.Update(student);

            var result = await _context.SaveChangesAsync();

            return result > 1;
        }

        public async Task<Student> GetStudentAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }
    }
}
