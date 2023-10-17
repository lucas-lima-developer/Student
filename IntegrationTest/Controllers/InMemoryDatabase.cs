using API.DbContexts;
using API.Models;
using FluentAssertions;
using IntegrationTest.Factory;
using IntegrationTest.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTest.Controllers
{
    public class InMemoryDatabase
    {
        private WebApplicationFactory<Program> _factory;

        public InMemoryDatabase()
        {
            //_factory = new WebApplicationFactory<Program>()
            //    .WithWebHostBuilder(builder =>
            //    {
            //        builder.ConfigureTestServices(services =>
            //        {
            //            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
            //            services.AddDbContext<AppDbContext>(options =>
            //            {
            //                options.UseInMemoryDatabase("test");
            //            });
            //        });
            //    });
            _factory = new InMemoryDatabaseFactory();
        }

        [Fact]
        public async void OnGetStudent_WhenExecuteApi_ShouldReturnExpectedStudent()
        {
            // Arrange

            using (var scope = _factory.Services.CreateScope())
            {
                var scopService = scope.ServiceProvider;
                var dbContext = scopService.GetRequiredService<AppDbContext>();

                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
                dbContext.Students.Add(new Student()
                {
                    FirstName = "name1",
                    LastName = "family1",
                    Address = "address1",
                    BirthDay = new DateTime(1970, 05, 20)
                });

                dbContext.SaveChanges();
            }

            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(HttpHelper.Urls.GetAllStudents);
            var result = await response.Content.ReadFromJsonAsync<List<Student>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result!.Count.Should().Be(1);

            result[0].FirstName.Should().Be("name1");
            result[0].LastName.Should().Be("family1");
            result[0].Address.Should().Be("address1");
            result[0].BirthDay.Should().Be(new DateTime(1970, 05, 20));

        }

        [Fact]
        public async void OnGetStudent_WithNoStudentInDatabase_ShouldReturnArrayEmpty()
        {
            // Arrange

            using (var scope = _factory.Services.CreateScope())
            {
                var scopService = scope.ServiceProvider;
                var dbContext = scopService.GetRequiredService<AppDbContext>();

                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();

                dbContext.SaveChanges();
            }

            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(HttpHelper.Urls.GetAllStudents);
            var result = await response.Content.ReadFromJsonAsync<List<Student>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result!.Count.Should().Be(0);

            result.ToArray().Should().BeEmpty();
        }
    }
}
