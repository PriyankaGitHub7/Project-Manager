using Microsoft.AspNetCore.Mvc;
using ProjectManager.Models;
using ProjectManager.WebApi.DataHelpers;
using System.Collections.Generic;
using System;

namespace ProjectManager.WebApi
{
    /// <summary>
    /// Routes the CRUD requests to the FileOperations class
    /// </summary>
    [Produces("application/json")]
    [Route("api/Project")]
    public class ProjectApiController : Controller
    {
        public FileOperations FileOps;

        public ProjectApiController()
        {
            FileOps = new FileOperations();
        }

        [HttpGet(Name = "GetAll")]
        public ResponseModel<List<ProjectViewModel>> GetAll()
        {
           return FileOps.GetAll();
        }

        [HttpGet("{id}", Name = "Get")]
        public ResponseModel<ProjectViewModel> Get(Int64 id)
        {
            return FileOps.GetProject(id);
        }

        [HttpPost]
        public ResponseModel<ProjectViewModel> Post([FromBody] ProjectViewModel project)
        {
            return FileOps.Add(project);
        }

        [HttpPut]
        public ResponseModel<ProjectViewModel> Put([FromBody] ProjectViewModel project)
        {
            return FileOps.Update(project);
        }
        
        [HttpDelete("{id}")]
        public ResponseModel<ProjectViewModel> Delete(Int64 id)
        {
            return FileOps.Delete(id);
        }
    }
}
