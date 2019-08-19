using ProjectManager.Models;
using System.Web.Http;

using ProjectManager.ActionFilters;
using System.Collections.Generic;
using System;
using System.Web.Http.Cors;
using System.Net.Http;
using System.Linq;
using System.Net;
using System.Data.Entity;

namespace ProjectManager.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TaskController : ApiController
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

        public TaskController()
        {

        }

        public TaskController(DAC.Entities dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [ProjectManagerLogFilter]
        [Route("api/tasks")]
        public HttpResponseMessage GetTasks()
        {
            List<Task> taskList = null;
            taskList = DbContext.Tasks.Select(t => new Task
            {
                Id = t.Id,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                IsParentTask = !t.ParentTaskId.HasValue,
                Name = t.Name,
                Priority = t.Priority,
            }).ToList();
            return Request.CreateResponse(taskList);
        }

        [HttpGet]
        [ProjectManagerLogFilter]
        [Route("api/tasks/sort/{projectId}/{propertyName}/{order}")]
        public HttpResponseMessage GetSortedTasks(long projectId, string propertyName, string order)
        {
            List<Task> taskList = null;
            IOrderedQueryable<DAC.Task> sortedTasks = null;
            if (order == "asc")
            {
                switch (propertyName)
                {
                    case "startDate":
                        sortedTasks = DbContext.Tasks.OrderBy(a => a.StartDate);
                        break;
                    case "endDate":
                        sortedTasks = DbContext.Tasks.OrderBy(a => a.EndDate);
                        break;
                    case "priority":
                        sortedTasks = DbContext.Tasks.OrderBy(a => a.Priority);
                        break;
                    case "status":
                        sortedTasks = DbContext.Tasks.OrderBy(a => a.Status);
                        break;
                }
            }
            else
            {
                switch (propertyName)
                {
                    case "startDate":
                        sortedTasks = DbContext.Tasks.OrderByDescending(a => a.StartDate);
                        break;
                    case "endDate":
                        sortedTasks = DbContext.Tasks.OrderByDescending(a => a.EndDate);
                        break;
                    case "priority":
                        sortedTasks = DbContext.Tasks.OrderByDescending(a => a.Priority);
                        break;
                    case "status":
                        sortedTasks = DbContext.Tasks.OrderByDescending(a => a.Status);
                        break;
                }
            }


            taskList = sortedTasks.Select(t => new Task
            {
                Id = t.Id,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                IsParentTask = !t.ParentTaskId.HasValue,
                Name = t.Name,
                Priority = t.Priority,
                Status = t.Status
            }).ToList();
            return Request.CreateResponse(taskList);
        }

        [HttpGet]
        [Route("api/task/project/{projectId}")]
        [ProjectManagerLogFilter]
        public HttpResponseMessage GetTaskByProjectId(int projectId)
        {
            List<Task> taskList = null;
            taskList = DbContext.Tasks.Where(t => t.ProjectId == projectId).Select(
                t => new Task
                {
                    Id = t.Id,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    IsParentTask = !t.ParentTaskId.HasValue,
                    ParentTaskId = t.ParentTaskId,
                    Name = t.Name,
                    ProjectId = t.ProjectId,
                    AssignedToUserId = t.AssignedToUserId,
                    Priority = t.Priority
                }).ToList();
            return Request.CreateResponse(taskList);
        }

        [HttpPost]
        [ProjectManagerLogFilter]
        [Route("api/task/add")]
        public HttpResponseMessage AddTask(Task task)
        {
            if (task == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Task cannot be null");
            }
            try
            {
                var dbTask = new DAC.Task();
                dbTask = new DAC.Task
                {
                    AssignedToUserId = task.AssignedToUserId,
                    Name = task.Name,
                    StartDate = task.StartDate,
                    EndDate = task.EndDate,
                    Priority = task.Priority,
                    ProjectId = task.ProjectId,
                    ParentTaskId = task.ParentTaskId
                };
                DbContext.Tasks.Add(dbTask);
                return Request.CreateResponse(dbContext.SaveChanges());
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error in adding Task: " + ex.Message);
            }
        }

        [HttpPost]
        [ProjectManagerLogFilter]
        [Route("api/task/update")]
        public HttpResponseMessage UpdateTaskDetails(Task task)
        {
            var taskToUpdate = DbContext.Tasks.FirstOrDefault(t => t.Id == task.Id);
            if (taskToUpdate != null)
            {
                taskToUpdate.Name = task.Name;
                taskToUpdate.Priority = task.Priority;
                taskToUpdate.StartDate = task.StartDate;
                taskToUpdate.EndDate = task.EndDate;
                taskToUpdate.ParentTaskId = task.ParentTaskId;
                taskToUpdate.ProjectId = task.ProjectId;
                taskToUpdate.AssignedToUserId = task.AssignedToUserId;
                return Request.CreateResponse(dbContext.SaveChanges());
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Task not found");
            }
        }

        [HttpPost]
        [ProjectManagerLogFilter]
        [Route("api/task/delete")]
        public HttpResponseMessage EndTask(Task task)
        {
            var taskToUpdate = DbContext.Tasks.FirstOrDefault(t => t.Id == task.Id);
            if (taskToUpdate != null)
            {
                taskToUpdate.Status = 1;
                return Request.CreateResponse(dbContext.SaveChanges());
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Task not found");
            }
        }


    }
}