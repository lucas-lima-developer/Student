using API.DbContexts;
using API.Models;
using API.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace UnitTest
{
    public class UnitTest1
    {
        private async Task<AppDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new AppDbContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Students.CountAsync() <= 0)
            {
                databaseContext.Students.Add(
                    new Student
                    {
                        StudentId = 1,
                        FirstName = "Lucas",
                        LastName = "Lima",
                        BirthDay = new DateTime(1998, 11, 15),
                        Address = "Morada do Parque"
                    });
                await databaseContext.SaveChangesAsync();
            }

            return databaseContext;
        }

        [Fact]
        public async Task StudentService_GetStudentAsync_ReturnStudent()
        {
            var dbContext = await GetDatabaseContext();
            var service = new StudentService(dbContext);

            var result = await service.GetStudentAsync(1);

            result.Should()
                .BeEquivalentTo(
                    new Student {
                        StudentId = 1,
                        FirstName = "Lucas",
                        LastName = "Lima",
                        BirthDay = new DateTime(1998, 11, 15),
                        Address = "Morada do Parque"
                    }
                 );
        }
    }
}