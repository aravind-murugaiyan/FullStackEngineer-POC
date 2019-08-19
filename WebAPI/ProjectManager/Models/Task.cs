using ProjectManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjectManager.Models
{
    public class Task
    {
        private static DbContext dbContext = null;

        public static DAC.Entities DbContext
        {
            get
            {
                if (dbContext == null)
                {
                    dbContext = new DAC.Entities();
                }
                return dbContext as DAC.Entities;
            }
            set
            {
                dbContext = value;
            }
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Priority { get; set; }
        public int Status { get; set; }




        public bool IsParentTask { get; set; }
        public long ProjectId { get; set; }
        public long? AssignedToUserId { get; set; }
        public long? ParentTaskId { get; set; }

        public Task ParentTask
        {
            get
            {
                Task parentTask = null;
                if (ParentTaskId.HasValue)
                {
                    var task = DbContext.Tasks.FirstOrDefault(t => t.Id == ParentTaskId);
                    if (task != null)
                    {
                        parentTask = new Task
                        {
                            Id = task.Id,
                            Name = task.Name
                        };
                    }
                }
                return parentTask;
            }
        }

        public Project Project
        {
            get
            {
                Project prj = null;
                if (ProjectId > 0)
                {
                    var proj = DbContext.Projects.FirstOrDefault(p => p.Id == ProjectId);
                    prj = new Project
                    {
                        Id = proj.Id,
                        Name = proj.Name
                    };
                }
                return prj;
            }
        }

        public User AssignedToUser
        {
            get
            {
                User usr = null;
                if (AssignedToUserId.HasValue)
                {
                    var user = DbContext.Users.FirstOrDefault(u => u.Id == AssignedToUserId);
                    if (user != null)
                    {
                        return new User
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            EmployeeId = user.EmployeeId
                        };
                    }
                }
                return usr;
            }
        }
    }
}