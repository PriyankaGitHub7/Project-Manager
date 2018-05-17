using ProjectManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManager.WebApi.Helpers
{
    /// <summary>
    /// This class contains methods to perform validations on the input 
    /// project data.
    /// </summary>
    public class InputValidator
    {
        public ResponseModel<ProjectViewModel> ValidateProject(ProjectViewModel project)
        {
            var responseModel = new ResponseModel<ProjectViewModel>();
            try
            {               
                var context = new ValidationContext(project);
                var results = new List<ValidationResult>();
                responseModel.IsSuccessful = Validator.TryValidateObject(project, context, results, true);
                if (!responseModel.IsSuccessful)
                {
                    responseModel.Message = "Project could not be saved due to invalid input. Messages: " + Environment.NewLine + String.Join<ValidationResult>(";", results?.ToArray());
                }               
            }
            catch(Exception ex)
            {
                responseModel.IsSuccessful = false;
                responseModel.Message = "An error occured while trying to validate the given project";
            }
            return responseModel;
        }
    }
}
