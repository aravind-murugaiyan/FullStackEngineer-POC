using System;
using System.Data.Entity;
using System.Linq;

namespace ProjectManager.Models
{
    public class Project
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
        public int Priority { get; set; }
        public long? ManagerUserId { get; set; }

        #region Virtual Methods
        public virtual User User
        {
            get
            {
                User userDetail = null;

                if (ManagerUserId.HasValue)
                {
                    var usr = DbContext.Users.FirstOrDefault(u => u.Id == ManagerUserId);
                    if (usr != null)
                    {
                        userDetail = new User()
                        {
                            Id = usr.Id,
                            FirstName = usr.FirstName,
                            LastName = usr.LastName,
                            EmployeeId = usr.EmployeeId
                        };
                    }
                }
                return userDetail;
            }
        }

        public bool IsCompleted
        {
            get
            {
                bool isProjectCompleted = false;
                isProjectCompleted = DbContext.Tasks.Where(p => p.ProjectId == Id).All(t => t.Status == 1);
                return isProjectCompleted;
            }
        }
        public int NoOfCompletedTasks
        {
            get
            {
                int count = 0;
                count = DbContext.Tasks.Where(t => t.ProjectId == Id && t.Status == 1).Count();
                return count;
            }
        }

        public int NoOfTasks
        {
            get
            {
                int count = 0;
                count = DbContext.Tasks.Where(t => t.ProjectId == Id).Count();
                return count;
            }
        }
        #endregion
    }
}