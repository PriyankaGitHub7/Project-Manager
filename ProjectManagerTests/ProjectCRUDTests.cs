using System;
using Xunit;
using ProjectManager.Models;
using ProjectManager.WebApi;

namespace ProjectManagerTests
{
    public class ProjectCRUDTests
    {
        [Fact]
        public void Test_Add_Project_Valid_Values()
        {
            var result = new ProjectApiController().Post(
                         new ProjectViewModel()
                         {
                             Name = "Project #1",
                             Description = "This aims to promote education",
                             StartDate = DateTime.Now,
                             EndDate = DateTime.Now.AddDays(3)
                         });

            Assert.True(result.IsSuccessful);
            Assert.NotNull(result.ResultSet);
        }

        [Fact]
        public void Test_Get_All_Projects()
        {
            var result = new ProjectApiController().GetAll();
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Test_Add_Project_Invalid_Values()
        {
            var result = new ProjectApiController().Post(
                         new ProjectViewModel() { Name = "" });
            Assert.False(result.IsSuccessful);
            Assert.False(String.IsNullOrEmpty(result.Message));
        }

        [Fact]
        public void Test_Get_Project_Found()
        {
            var result = new ProjectApiController().Get(1);
            Assert.True(result.IsSuccessful);          
        }

        [Fact]
        public void Test_Get_Project_Not_Found()
        {
            var result = new ProjectApiController().Get(100);
            Assert.False(result.IsSuccessful);
            Assert.Equal("No Project found with the given Id", result.Message);
        }
        
        [Fact]
        public void Test_Update_Project_Valid_Values()
        {
            var result = new ProjectApiController().Put(
                         new ProjectViewModel()
                         {
                             Id = 1,
                             Name = "Project One",
                             Description = "This aims to promote education",
                             StartDate = DateTime.Now,
                             EndDate = DateTime.Now.AddDays(3)
                         });

            Assert.True(result.IsSuccessful);
            Assert.Equal("Project One", result.ResultSet.Name);
        }

        [Fact]
        public void Test_Update_Project_Invalid_Values()
        {
            var result = new ProjectApiController().Put(
                         new ProjectViewModel()
                         {
                             Id = 1,
                             Name = "",
                             Description = "This aims to promote education",
                             StartDate = DateTime.Now,
                             EndDate = DateTime.Now.AddDays(3)
                         });

            Assert.False(result.IsSuccessful);
            Assert.False(String.IsNullOrEmpty(result.Message));
        }

        [Fact]
        public void Delete_Project_Found()
        {
            var result = new ProjectApiController().Delete(1);
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Delete_Project_Not_Found()
        {
            var result = new ProjectApiController().Delete(200);
            Assert.False(result.IsSuccessful);
            Assert.Equal("No Project found with the given Id", result.Message);
        }
    }
}
