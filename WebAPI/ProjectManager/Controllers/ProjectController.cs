using ProjectManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ProjectManager.ActionFilters;
using System.Web.Http.Cors;
using System.Net.Http;
using System.Net;
using System.Data.Entity;

namespace ProjectManager.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProjectController : ApiController
    {
        private DbContext dbContext = null;

        public DAC.Entities DbContext
        {
            get
            {
                if (dbContext == null)
                {
                    dbContext = new DAC.Entities();
                }
                return dbContext as DAC.Entities;
            }
        }

        public ProjectController()
        {

        }

        public ProjectController(DAC.Entities dbContext)
        {
            this.dbContext = dbContext;
            Project.DbContext = DbContext;
        }

        [HttpGet]
        [ProjectManagerLogFilter]
        [Route("api/projects")]
        public HttpResponseMessage GetProjects()
        {
            var projectList = DbContext.Projects.Select(p => new Project
            {
                Id = p.Id,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Name = p.Name,
                ManagerUserId = p.ManagerUserId,
                Priority = p.Priority
            }).ToList();
            return Request.CreateResponse(projectList);
        }

        [HttpGet]
        [ProjectManagerLogFilter]
        [Route("api/projects/sort/{propertyName}/{order}")]
        public HttpResponseMessage GetSortedProjects(string propertyName, string order)
        {
            List<Project> projectList = null;
            projectList = DbContext.Projects.Select(p => new Project
            {
                Id = p.Id,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Name = p.Name,
                ManagerUserId = p.ManagerUserId,
                Priority = p.Priority
            }).ToList();
            if (order == "asc")
            {
                switch (propertyName)
                {
                    case "startDate":
                        projectList = projectList.OrderBy(a => a.StartDate).ToList();
                        break;
                    case "endDate":
                        projectList = projectList.OrderBy(a => a.EndDate).ToList();
                        break;
                    case "priority":
                        projectList = projectList.OrderBy(a => a.Priority).ToList();
                        break;
                    case "status":
                        projectList = projectList.OrderBy(a => a.IsCompleted).ToList();
                        break;
                }
            }
            else
            {
                switch (propertyName)
                {
                    case "startDate":
                        projectList = projectList.OrderByDescending(a => a.StartDate).ToList();
                        break;
                    case "endDate":
                        projectList = projectList.OrderByDescending(a => a.EndDate).ToList();
                        break;
                    case "priority":
                        projectList = projectList.OrderByDescending(a => a.Priority).ToList();
                        break;
                    case "status":
                        projectList = projectList.OrderByDescending(a => a.IsCompleted).ToList();
                        break;
                }
            }
            return Request.CreateResponse(projectList);
        }

        [HttpPost]
        [ProjectManagerLogFilter]
        [Route("api/projects/add")]
        public HttpResponseMessage InsertProjectDetails(Project project)
        {
            if (project == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Project is null");
            }
            else if (project.Id < 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Project ID cannot be negative");
            }

            try
            {
                var proj = new DAC.Project
                {
                    Id = project.Id,
                    Name = project.Name,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    Priority = project.Priority,
                    ManagerUserId = project.ManagerUserId
                };
                DbContext.Projects.Add(proj);
                return Request.CreateResponse(dbContext.SaveChanges());
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error in adding the project:" + ex.Message);
            }
        }


        [HttpPost]
        [Route("api/projects/update")]
        [ProjectManagerLogFilter]
        public HttpResponseMessage UpdateProjectDetails(DAC.Project project)
        {
            if (project == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Project is null");
            }
            if (project.Id <= 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Project ID cannot be negative");
            }
            var proj = DbContext.Projects.FirstOrDefault(a => a.Id == project.Id);
            if (proj != null)
            {
                proj.ManagerUserId = project.ManagerUserId;
                proj.Name = project.Name;
                proj.Priority = project.Priority;
                proj.StartDate = project.StartDate;
                proj.EndDate = project.EndDate;
                return Request.CreateResponse(dbContext.SaveChanges());
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Project with project id " + project.Id + " was not found.");
            }
        }

        [HttpPost]
        [Route("api/projects/delete/{projectId}")]
        [ProjectManagerLogFilter]
        public HttpResponseMessage DeleteProjectDetails(long projectId)
        {
            if (projectId <= 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Project ID should be greater than 0");
            }
            var proj = DbContext.Projects.FirstOrDefault(p => p.Id == projectId);
            if (proj != null)
            {
                DbContext.Projects.Remove(proj);
                return Request.CreateResponse(dbContext.SaveChanges());
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Project with project id " + projectId + " not found.");
            }
        }

    }
}