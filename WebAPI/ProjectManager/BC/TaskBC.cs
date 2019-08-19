using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectManager.Models;
using DAC = ProjectManager.DAC;

namespace ProjectManager.BC
{
    public class TaskBC
    {
        DAC.ProjectManagerEntities1 dbContext = null;
        public TaskBC()
        {
            dbContext = new DAC.ProjectManagerEntities1();
        }

        public TaskBC(DAC.ProjectManagerEntities1 context)
        {
            dbContext = context;
        }
        public List<Task> RetrieveTaskByProjectId(int projectId)
        {
            //using (dbContext)
            //{
            //    return dbContext.Tasks.Where(z => z.Project_ID == projectId).Select(x => new Task()
            //    {
            //        Id = x.Task_ID,
            //        Name = x.Task_Name,
            //        ParentTask = x.Parent_ID != null ? dbContext.Tasks.Where(pt => pt.Task_ID == x.Parent_ID).First().Select(dtoTask => new Task
            //        {
            //            Id = dtoTask.Task_ID,
            //            Name = dtoTask.Task_Name,
            //            StartDate = dtoTask.Start_Date,
            //            EndDate = dtoTask.End_Date,
            //            Priority = dtoTask.Priority,
            //            IsParentTask = !dtoTask.Parent_ID.HasValue,
            //            ParentTask = GetTaskById(task_ID),
            //            Project = GetProjectById(dtoTask.Project_ID),
            //            User = GetUserByTaskId(dtoTask.Task_ID, dtoTask.Project_ID)
            //        }
            //            ) : null,
            //        //ParentTaskName = dbContext.ParentTasks.Where(y => y.Parent_ID == x.Parent_ID).FirstOrDefault().Parent_Task_Name,
            //        StartDate = x.Start_Date,
            //        EndDate = x.End_Date,
            //        Priority = x.Priority,
            //        Status = x.Status,
            //        User = dbContext.Users.Where(y => y.Task_ID == x.Task_ID).Select(z => new User()
            //        {
            //            Id = z.User_ID,
            //            FirstName = z.First_Name
            //        }).FirstOrDefault(),
            //    }).ToList();
            //}
            return null;
        }

        public List<Task> RetrieveTasks()
        {
            List<Task> taskList = null;
            using (dbContext)
            {
                taskList = dbContext.Tasks.Select(t => new Task
                {
                    Id = t.Task_ID,
                    Name = t.Task_Name
                }).ToList();
            }
            return taskList;
        }

        private Task GetTaskById(int task_ID)
        {
            Task task = null;
            var dtoTask = dbContext.Tasks.FirstOrDefault(t => t.Task_ID == task_ID);
            if (dtoTask != null)
            {
                task = new Task
                {
                    Id = dtoTask.Task_ID,
                    Name = dtoTask.Task_Name,
                    StartDate = dtoTask.Start_Date,
                    EndDate = dtoTask.End_Date,
                    Priority = dtoTask.Priority,
                    IsParentTask = !dtoTask.Parent_ID.HasValue,
                    ParentTask = GetTaskById(task_ID),
                    Project = GetProjectById(dtoTask.Project_ID),
                    User = GetUserByTaskId(dtoTask.Task_ID, dtoTask.Project_ID)
                };
            }
            return task;
        }

        private User GetUserByTaskId(int taskId, int? projectId)
        {
            User usr = null;
            var dtoUser = dbContext.Users.FirstOrDefault(a => (a.Task_ID == taskId && a.Project_ID == projectId) || a.Task_ID == taskId);
            if (dtoUser != null)
            {
                usr = new User
                {
                    Id = dtoUser.User_ID,
                    EmployeeId = dtoUser.Employee_ID,
                    FirstName = dtoUser.First_Name,
                    LastName = dtoUser.Last_Name,
                    ProjectId = dtoUser.Project_ID
                };
            }
            return usr;
        }

        public Project GetProjectById(int? projectId)
        {
            Project proj = null;
            var dtoProject = dbContext.Projects.FirstOrDefault(a => a.Project_ID == projectId);
            if (dtoProject != null)
            {
                proj = new Project
                {
                    Id = dtoProject.Project_ID,
                    StartDate = dtoProject.Start_Date,
                    EndDate = dtoProject.End_Date,
                    Name = dtoProject.Project_Name,
                    Priority = dtoProject.Priority
                };
            }
            return proj;
        }

        public List<ParentTask> RetrieveParentTasks()
        {
            using (dbContext)
            {
                return dbContext.ParentTasks.Select(x => new ParentTask()
                {
                    ParentTaskId = x.Parent_ID,
                    ParentTaskName = x.Parent_Task_Name
                }).ToList();
            }
        }


        public int InsertTaskDetails(Task task)
        {
            using (dbContext)
            {
                DAC.Task dbTask = null;
                if (task.IsParentTask)
                {
                    dbTask = new DAC.Task
                    {
                        Task_Name = task.Name,
                        Project_ID = task.Project.Id
                    };
                }
                else
                {
                    dbTask = new DAC.Task
                    {
                        Task_Name = task.Name,
                        Project_ID = task.Project != null ? task.Project.Id : (int?)null,
                        Start_Date = task.StartDate,
                        End_Date = task.EndDate,
                        Parent_ID = task.ParentTask != null ? task.ParentTask.Id : (int?)null,
                        Priority = task.Priority
                    };
                }
                dbContext.Tasks.Add(dbTask);
                dbContext.SaveChanges();
                //if (task.Priority == 0)
                //{
                //    dbContext.ParentTasks.Add(new DAC.ParentTask()
                //    {
                //        Parent_Task_Name = task.Name

                //    });
                //}
                //else
                //{
                //    DAC.Task taskDetail = new DAC.Task()
                //    {
                //        Task_Name = task.Name,
                //        Project_ID = task.Project.Id,
                //        Start_Date = task.StartDate,
                //        End_Date = task.EndDate,
                //        Parent_ID = task.ParentTask.Id,
                //        Priority = task.Priority,
                //        Status = task.Status
                //    };
                //    dbContext.Tasks.Add(taskDetail);
                //    dbContext.SaveChanges();

                //    var editDetails = (from editUser in dbContext.Users
                //                       where editUser.User_ID.ToString().Contains(task.User.Id.ToString())
                //                       select editUser).ToList();
                //    // Modify existing records
                //    if (editDetails != null && editDetails.Count>0)
                //    {
                //        editDetails.First().Task_ID = taskDetail.Task_ID;
                //    }
                //}
                return dbContext.SaveChanges();
            }
        }

        public int UpdateTaskDetails(Task task)
        {
            using (dbContext)
            {
                var editDetails = (from editTask in dbContext.Tasks
                                   where editTask.Task_ID.ToString().Contains(task.Id.ToString())
                                   select editTask).First();
                // Modify existing records
                if (editDetails != null)
                {
                    editDetails.Task_Name = task.Name;
                    editDetails.Start_Date = task.StartDate;
                    editDetails.End_Date = task.EndDate;
                    editDetails.Status = task.Status;
                    editDetails.Priority = task.Priority;

                }
                var editDetailsUser = (from editUser in dbContext.Users
                                       where editUser.User_ID.ToString().Contains(task.User.Id.ToString())
                                       select editUser).First();
                // Modify existing records
                if (editDetailsUser != null)
                {
                    editDetails.Task_ID = task.Id;
                }
                return dbContext.SaveChanges();
            }

        }

        public int DeleteTaskDetails(Task task)
        {
            using (dbContext)
            {
                var deleteTask = (from editTask in dbContext.Tasks
                                  where editTask.Task_ID.ToString().Contains(task.Id.ToString())
                                  select editTask).First();
                // Delete existing record
                if (deleteTask != null)
                {
                    deleteTask.Status = 1;
                }
                return dbContext.SaveChanges();
            }

        }


    }
}