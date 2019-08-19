using System.Collections.Generic;
using System.Linq;
using MODEL = ProjectManager.Models;
using DAC = ProjectManager.DAC;

namespace ProjectManager.BC
{
    public class ProjectBC
    {
        DAC.ProjectManagerEntities1 dbContext = null;
        public ProjectBC()
        {
            dbContext = new DAC.ProjectManagerEntities1();
        }

        public ProjectBC(DAC.ProjectManagerEntities1 context)
        {
            dbContext = context;
        }
        public List<MODEL.Project> RetrieveProjects()
        {
            using (dbContext)
            {
                return dbContext.Projects.Select(x => new MODEL.Project()
                {
                    Id = x.Project_ID,
                    Name = x.Project_Name,
                    EndDate = x.End_Date,
                    StartDate = x.Start_Date,
                    Priority = x.Priority,
                    User = dbContext.Users.Where(y => y.Project_ID == x.Project_ID).Select(z => new MODEL.User()
                    {
                        Id = z.User_ID,
                        EmployeeId = z.Employee_ID,
                        FirstName=z.First_Name,
                        LastName=z.Last_Name,
                        ProjectId=z.Project_ID
                    }).FirstOrDefault(),
                    NoOfTasks = dbContext.Tasks.Where(y => y.Project_ID == x.Project_ID).Count(),
                    NoOfCompletedTasks = dbContext.Tasks.Where(y => y.Project_ID == x.Project_ID && y.Status == 1).Count(),
                }).ToList();
            }
        }

        public int InsertProjectDetails(MODEL.Project project)
        {
            using (dbContext)
            {
                DAC.Project proj = new DAC.Project()
                {
                    Project_Name = project.Name,
                    Start_Date = project.StartDate,
                    End_Date = project.EndDate,
                    Priority = project.Priority
                };
                dbContext.Projects.Add(proj);
                dbContext.SaveChanges();
                var editDetails = (from editUser in dbContext.Users
                                   where editUser.User_ID.ToString().Contains(project.User.Id.ToString())
                                   select editUser).First();
                // Modify existing records
                if (editDetails != null)
                {
                    editDetails.Project_ID = proj.Project_ID;
                }
                return dbContext.SaveChanges();
            }
        }


        //public int InsertProjectDetails(MODEL.Project project)
        //{
        //    using (dbContext)
        //    {
        //        DAC.Project proj = new DAC.Project()
        //        {
        //            Project_Name = project.ProjectName,
        //            Start_Date = project.ProjectStartDate,
        //            End_Date = project.ProjectEndDate,
        //            Priority = project.Priority
        //        };
        //        dbContext.Projects.Add(proj);
        //        dbContext.SaveChanges();
        //        var editDetails = (from editUser in dbContext.Users
        //                           where editUser.User_ID.ToString().Contains(project.User.UserId.ToString())
        //                           select editUser).First();
        //        // Modify existing records
        //        if (editDetails != null)
        //        {
        //            editDetails.Project_ID = proj.Project_ID;
        //        }
        //        return dbContext.SaveChanges();
        //    }
        //}

        public int UpdateProjectDetails(MODEL.Project project)
        {
            using (dbContext)
            {
                var editProjDetails = (from editProject in dbContext.Projects
                                       where editProject.Project_ID.ToString().Contains(project.Id.ToString())
                                       select editProject).First();
                // Modify existing records
                if (editProjDetails != null)
                {
                    editProjDetails.Project_Name = project.Name;
                    editProjDetails.Start_Date = project.StartDate;
                    editProjDetails.End_Date = project.EndDate;
                    editProjDetails.Priority = project.Priority;
                }


                var editDetails = (from editUser in dbContext.Users
                                   where editUser.User_ID.ToString().Contains(project.User.Id.ToString())
                                   select editUser).First();
                // Modify existing records
                if (editDetails != null)
                {
                    editDetails.Project_ID = project.Id;
                }
                return dbContext.SaveChanges();
            }

        }
        public int DeleteProjectDetails(MODEL.Project project)
        {
            using (dbContext)
            {

                var editDetails = (from proj in dbContext.Projects
                                   where proj.Project_ID == project.Id
                                   select proj).First();
                // Delete existing record
                if (editDetails != null)
                {
                    dbContext.Projects.Remove(editDetails);
                }
                return dbContext.SaveChanges();
            }

        }

    }
}