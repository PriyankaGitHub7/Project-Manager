using ProjectManager.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

namespace ProjectManager.WebApi.DataHelpers
{
    public class FileOperations
    {
        public string FILE_PATH = Directory.GetCurrentDirectory() + "//projects.json";
        public string PROJECT_NOT_FOUND_MESSAGE = "No Project found with the given Id";

        /// <summary>
        /// Adds the given project to the file and returns success or failure
        /// </summary>
        /// <param name="project"></param>
        /// <returns>ResponseModel that contains the status of the Add Operation 
        /// and an error message, if any</returns>
        public ResponseModel<ProjectViewModel> Add(ProjectViewModel project)
        {
            ResponseModel<ProjectViewModel> result = 
                new ResponseModel<ProjectViewModel>();
            try
            {
                var projectList = GetAllProjects();
                // For the first project, set Id to be 1
                if (!projectList.Any())
                {
                    project.Id = 1;
                    projectList.Add(project);
                }
                else
                {
                    /* Since Id is the unique project identifier, 
                    calculate it based on the last Id value */
                    var nextId = projectList.Last().Id + 1;
                    project.Id = nextId;
                    projectList.Add(project);
                }
     
                WriteUpdatedProjectList(projectList);
                return GetResponse<ProjectViewModel>(true, project, "");
            }
            catch(Exception ex)
            {
                return GetResponse<ProjectViewModel>(false,null, ex.Message);
            }
        }

        /// <summary>
        /// Reads all the projects present in the file and returns the result as a list
        /// </summary>
        /// <returns>ResponseModel with the list of projects, read status and error
        /// message, if any</returns>
        public ResponseModel<List<ProjectViewModel>> GetAll()
        {
            try
            {
                var projects = GetAllProjects();
                return GetResponse<List<ProjectViewModel>>(true, projects, "");
            }
            catch(Exception ex)
            {
                return GetResponse<List<ProjectViewModel>>(false, null, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the project with the given Id 
        /// </summary>
        /// <param name="Id">Project Id for which </param>
        /// <returns>Response Model with the project object, read status and error 
        /// message, if any</returns>
        public ResponseModel<ProjectViewModel> GetProject(Int64 Id)
        {
            try
            {
                if(GetSingleProject(Id).Any())
                {
                    return GetResponse<ProjectViewModel>(true, GetSingleProject(Id).Single(), "");
                }
                else
                {
                    return GetResponse<ProjectViewModel>(false, null, PROJECT_NOT_FOUND_MESSAGE);
                }
               
            }
            catch(Exception ex)
            {
                return GetResponse<ProjectViewModel>(false, null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the given project in the data file
        /// </summary>
        /// <param name="project">Project to be updated</param>
        /// <returns>Response model with the updated project, update status 
        /// and error message, if any</returns>
        public ResponseModel<ProjectViewModel> Update(ProjectViewModel project)
        {
            try
            {
                if(GetSingleProject(project.Id).Any())
                {
                    var allProjects = GetAllProjects();
                    var indexToUpdate = allProjects.FindIndex(a => a.Id == project.Id);
                    // Remove the project with old data and add the new one at the same place
                    allProjects.RemoveAt(indexToUpdate);
                    allProjects.Insert(indexToUpdate, project);
                    WriteUpdatedProjectList(allProjects);
                    return GetResponse<ProjectViewModel>(true, project, "");
                }
                else
                {
                    return GetResponse<ProjectViewModel>(false, null, PROJECT_NOT_FOUND_MESSAGE);
                }
            }
            catch (Exception ex)
            {
                return GetResponse<ProjectViewModel>(false, null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the project with the given Id from the file
        /// </summary>
        /// <param name="Id">Project Id to be deleted</param>
        /// <returns>ResponseModel with delete status and error message, if any</returns>
        public ResponseModel<ProjectViewModel> Delete(Int64 Id)
        {
            try
            {
                if(GetSingleProject(Id).Any())
                {
                    var allProjects = GetAllProjects();
                    allProjects.RemoveAll(a => a.Id == Id);
                    WriteUpdatedProjectList(allProjects);
                    return GetResponse<ProjectViewModel>(true, null, "");
                }
                else
                {
                    return GetResponse<ProjectViewModel>(false, null, PROJECT_NOT_FOUND_MESSAGE);
                }
            }
            catch(Exception ex)
            {
                return GetResponse<ProjectViewModel>(false, null, ex.Message);
            }
        }

        // Filters the projects based on the given Id and returns the matched entry
        private IEnumerable<ProjectViewModel> GetSingleProject(Int64 Id)
        {
            return GetAllProjects().Where(a => a.Id == Id);
        }

        // Reads the json project data from the file and deserializes it 
        // into a list of projects.
        private List<ProjectViewModel> GetAllProjects()
        {
            var projects = new List<ProjectViewModel>();

            if (File.Exists(FILE_PATH))
            {
                var jsonProjects = File.ReadAllText(FILE_PATH);
                projects = JsonConvert.DeserializeObject<List<ProjectViewModel>>
               (jsonProjects);
            }        

            return projects;
        }

        // Writes the modified list of projects back to the file
        private void WriteUpdatedProjectList(List<ProjectViewModel> projects)
        {
            var json = JsonConvert.SerializeObject(projects);
            File.WriteAllText(FILE_PATH, json);
        }

        // Creates, populates and returns the response object
        private ResponseModel<T> GetResponse<T>(bool IsSucccessful, T result, 
                                                string message)
        {
            ResponseModel<T> response = new ResponseModel<T>();
            response.IsSuccessful = IsSucccessful;
            response.ResultSet = result;
            response.Message = message;
            return response;
        }
    }
}