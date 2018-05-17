using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using ProjectManager.Models;
using ProjectManager.WebApi;
using Microsoft.Extensions.Logging;

namespace ProjectManager.Controllers
{
    public class ProjectController : Controller
    {
        private ILogger _logger;
        private readonly string LANDING_PAGE_ACTION_NAME = "Index";
        private readonly string ERROR_ACTION_NAME = "Error";
        private readonly string INVALID_MODEL_MESSAGE = "Invalid Project Details.Please check and try again";
        private ProjectApiController _apiObject;

        public ProjectController(ILogger<ProjectController> logger)
        {
            _logger = logger;
            _apiObject = new ProjectApiController();
        }

        public IActionResult Index()
        {
            try
            {
                // Get all the projects that have been added 
                List<ProjectViewModel> lstProjects = _apiObject.GetAll().ResultSet;
                if(lstProjects.Count == 0)
                {
                    _logger.LogInformation("There are no projects added yet.");
                }
                return View(lstProjects);
            }
            catch(Exception ex)
            {
                return GetErrorResponse("Index", ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetProject(Int64 id, string operation)
        {
            try
            {
                ProjectViewModel project = _apiObject.Get(id).ResultSet;
                if(project == null)
                {
                    _logger.LogInformation(String.Format("Could not find any project with the Id: {0}", id));
                }
                ViewBag.Operation = operation;
                return PartialView("_AddProjectPartial", project);
            }
            catch(Exception ex)
            {
                return GetErrorResponse("GetProject", ex.Message);
            }

        }

        [HttpPost]
        public IActionResult Add(ProjectViewModel project)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _apiObject.Post(project);
                    return RedirectToAction(LANDING_PAGE_ACTION_NAME);
                }
                else
                {
                    return GetErrorResponse("Add", INVALID_MODEL_MESSAGE);
                }
            }
            catch(Exception ex)
            {
                return GetErrorResponse("Add", ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Update(ProjectViewModel project)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _apiObject.Put(project);
                    return RedirectToAction(LANDING_PAGE_ACTION_NAME);
                }
                else
                {
                    return GetErrorResponse("Update", INVALID_MODEL_MESSAGE);
                }
            }
            catch (Exception ex)
            {
                return GetErrorResponse("Update", ex.Message);
            }
        }

        public IActionResult DeleteProject(Int64 id)
        {
            try
            {
                _apiObject.Delete(id);
                return RedirectToAction(LANDING_PAGE_ACTION_NAME);
            }
            catch (Exception ex)
            {
                return GetErrorResponse("DeleteProject", ex.Message);
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? 
                        HttpContext.TraceIdentifier });
        }

        private IActionResult GetErrorResponse(string actionName, string errorMessage)
        {
            _logger.LogError(String.Format("Action: {0}; Message: {1}", actionName, errorMessage));
            return RedirectToAction(ERROR_ACTION_NAME);
        }
    }
}
