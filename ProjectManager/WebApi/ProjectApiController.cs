using Microsoft.AspNetCore.Mvc;
using ProjectManager.Models;
using ProjectManager.WebApi.DataHelpers;
using System.Collections.Generic;
using System;
using ProjectManager.WebApi.Helpers;

namespace ProjectManager.WebApi
{
    /// <summary>
    /// Validates and routes the CRUD requests to the FileOperations class
    /// </summary>
    [Produces("application/json")]
    [Route("api/Project")]
    public class ProjectApiController : Controller
    {
        public FileOperations FileOps;
        public InputValidator _inputValidator;

        public ProjectApiController()
        {
            FileOps = new FileOperations();
            _inputValidator = new InputValidator();
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
            var validationResult = _inputValidator.ValidateProject(project);

            if (validationResult.IsSuccessful)
            {
                return FileOps.Add(project);
            }
            else
            {
                return validationResult;
            }
        }

        [HttpPut]
        public ResponseModel<ProjectViewModel> Put([FromBody] ProjectViewModel project)
        {
            var validationResult = _inputValidator.ValidateProject(project);

            if(validationResult.IsSuccessful)
            {
                return FileOps.Update(project);
            }
            else
            {
                return validationResult;
            }          
        }
        
        [HttpDelete("{id}")]
        public ResponseModel<ProjectViewModel> Delete(Int64 id)
        {
            return FileOps.Delete(id);
        }
    }
}
