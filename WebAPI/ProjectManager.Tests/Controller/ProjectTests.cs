using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectManager;
using System.Net.Http;
using ProjectManager.Controllers;
using System.Data.Entity;

namespace ProjectManager.Tests.Controller
{
    [TestClass]
    public class ProjectTests
    {
        List<DAC.Project> ProjectList = null;
        List<DAC.Task> TaskList = null;
        ProjectController projectController;
        public ProjectTests()
        {
            SetupTestData();
        }

        public void SetupTestData()
        {
            ProjectList = new List<DAC.Project>()
                {
                    new DAC.Project
                    {
                        Id=1,
                        Name="Project 1",
                        StartDate = DateTime.Today,
                        EndDate=DateTime.Today.AddDays(5),
                        Priority = 1,
                        ManagerUserId=1,
                        User = new DAC.User
                        {
                            Id=1,
                            EmployeeId="Emp1",
                            FirstName="User 1",
                            LastName="Last Name"
                        }
                    },
                    new DAC.Project
                    {
                        Id=2,
                        Name="Project 2",
                        StartDate = DateTime.Today.AddDays(1),
                        EndDate=DateTime.Today.AddDays(6),
                        Priority = 3,
                        ManagerUserId=2,
                        User = new DAC.User
                        {
                            Id=2,
                            EmployeeId="Emp2",
                            FirstName="User 2",
                            LastName="Last Name"
                        }
                    }
                };

            TaskList = new List<DAC.Task>()
            {
                new DAC.Task
                {
                    Id=1,
                    Name="Task 1",
                    Status=1,
                    ProjectId=1
                },
                new DAC.Task
                {
                    Id=2,
                    Name="Task 2",
                    Status=0,
                    ProjectId=2
                }
            };

            var projects = ProjectList.AsQueryable();
            var projectMock = new Mock<DbSet<DAC.Project>>();
            var taskMock = new Mock<DbSet<DAC.Task>>();
            var tasks = TaskList.AsQueryable();

            projectMock.Setup(x => x.Add(It.IsAny<DAC.Project>())).Returns((DAC.Project proj) =>
            {
                ProjectList.Add(proj);
                return proj;
            });
            projectMock.Setup(x => x.Remove(It.IsAny<DAC.Project>())).Returns((DAC.Project proj) =>
            {
                var p = ProjectList.FirstOrDefault(x => x.Id == proj.Id);
                ProjectList.Remove(p);
                return proj;
            });
            projectMock.As<IQueryable<DAC.Project>>().Setup(m => m.Provider).Returns(projects.Provider);
            projectMock.As<IQueryable<DAC.Project>>().Setup(m => m.Expression).Returns(projects.Expression);
            projectMock.As<IQueryable<DAC.Project>>().Setup(m => m.ElementType).Returns(projects.ElementType);
            projectMock.As<IQueryable<DAC.Project>>().Setup(m => m.GetEnumerator()).Returns(projects.GetEnumerator());

            taskMock.As<IQueryable<DAC.Task>>().Setup(m => m.Provider).Returns(tasks.Provider);
            taskMock.As<IQueryable<DAC.Task>>().Setup(m => m.Expression).Returns(tasks.Expression);
            taskMock.As<IQueryable<DAC.Task>>().Setup(m => m.ElementType).Returns(tasks.ElementType);
            taskMock.As<IQueryable<DAC.Task>>().Setup(m => m.GetEnumerator()).Returns(tasks.GetEnumerator());


            var dbContext = new Mock<DAC.Entities>();

            dbContext.Setup(ctx => ctx.Projects).Returns(projectMock.Object);
            dbContext.Setup(ctx => ctx.Tasks).Returns(taskMock.Object);
            projectController = new ProjectController(dbContext.Object)
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new System.Web.Http.HttpConfiguration()
            };
        }

        [TestMethod]
        public void GetAllProjectsTest()
        {
            try
            {
                var httpReponse = projectController.GetProjects();
                List<Models.Project> projectLst = null;
                if (httpReponse.TryGetContentValue<List<Models.Project>>(out projectLst))
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK && projectLst.Count == 2);
                }
                else
                {
                    Assert.Fail();
                }
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void GetSortedProjectsByStartDateAscTest()
        {
            try
            {
                var httpReponse = projectController.GetSortedProjects("startDate", "asc");
                List<Models.Project> projectLst = null;
                if (httpReponse.TryGetContentValue<List<Models.Project>>(out projectLst))
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK && projectLst.Count == 2 && projectLst[0].StartDate < projectLst[1].StartDate);
                }
                else
                {
                    Assert.Fail();
                }
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void GetSortedProjectsByStartDateDescTest()
        {
            try
            {
                var httpReponse = projectController.GetSortedProjects("startDate", "desc");
                List<Models.Project> projectLst = null;
                if (httpReponse.TryGetContentValue<List<Models.Project>>(out projectLst))
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK && projectLst.Count == 2 && projectLst[0].StartDate > projectLst[1].StartDate);
                }
                else
                {
                    Assert.Fail();
                }
            }
            catch
            {
                Assert.Fail();
            }
        }


        [TestMethod]
        public void GetSortedProjectsByEndDateAscTest()
        {
            try
            {
                var httpReponse = projectController.GetSortedProjects("endDate", "asc");
                List<Models.Project> projectLst = null;
                if (httpReponse.TryGetContentValue<List<Models.Project>>(out projectLst))
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK && projectLst.Count == 2 && projectLst[0].EndDate < projectLst[1].EndDate);
                }
                else
                {
                    Assert.Fail();
                }
            }
            catch
            {

                Assert.Fail();
            }
        }

        [TestMethod]
        public void GetSortedProjectsByEndDateDescTest()
        {
            try
            {
                var httpReponse = projectController.GetSortedProjects("endDate", "desc");
                List<Models.Project> projectLst = null;
                if (httpReponse.TryGetContentValue<List<Models.Project>>(out projectLst))
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK && projectLst.Count == 2 && projectLst[0].EndDate > projectLst[1].EndDate);
                }
                else
                {
                    Assert.Fail();
                }
            }
            catch
            {

                Assert.Fail();
            }
        }

        [TestMethod]
        public void GetSortedProjectsByPriorityAscTest()
        {
            try
            {
                var httpReponse = projectController.GetSortedProjects("priority", "asc");
                List<Models.Project> projectLst = null;
                if (httpReponse.TryGetContentValue<List<Models.Project>>(out projectLst))
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK && projectLst.Count == 2 && projectLst[0].Priority < projectLst[1].Priority);
                }
                else
                {
                    Assert.Fail();
                }
            }
            catch
            {

                Assert.Fail();
            }
        }

        [TestMethod]
        public void GetSortedProjectsByPriorityDescTest()
        {
            try
            {
                var httpReponse = projectController.GetSortedProjects("priority", "desc");
                List<Models.Project> projectLst = null;
                if (httpReponse.TryGetContentValue<List<Models.Project>>(out projectLst))
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK && projectLst.Count == 2 && projectLst[0].Priority > projectLst[1].Priority);
                }
                else
                {
                    Assert.Fail();
                }
            }
            catch
            {

                Assert.Fail();
            }
        }

        [TestMethod]
        public void GetSortedProjectsByStatusAscTest()
        {
            try
            {
                var httpReponse = projectController.GetSortedProjects("status", "asc");
                List<Models.Project> projectLst = null;
                if (httpReponse.TryGetContentValue<List<Models.Project>>(out projectLst))
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK && projectLst.Count == 2 && projectLst[1].IsCompleted);
                }
                else
                {
                    Assert.Fail();
                }
            }
            catch
            {

                Assert.Fail();
            }
        }

        [TestMethod]
        public void GetSortedProjectsByStatusDescTest()
        {
            try
            {
                var httpReponse = projectController.GetSortedProjects("status", "desc");
                List<Models.Project> projectLst = null;
                if (httpReponse.TryGetContentValue<List<Models.Project>>(out projectLst))
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK && projectLst.Count == 2 && projectLst[0].IsCompleted);
                }
                else
                {
                    Assert.Fail();
                }
            }
            catch
            {

                Assert.Fail();
            }
        }

        [TestMethod]
        public void AddProjectTest()
        {
            try
            {
                int projectListCount = ProjectList.Count;
                var httpReponse = projectController.InsertProjectDetails(new Models.Project
                {
                    Id = 3,
                    Name = "Project 3",
                    StartDate = DateTime.Today.AddDays(4),
                    EndDate = DateTime.Today.AddDays(7),
                    Priority = 3,
                    ManagerUserId = 1,
                });
                if (httpReponse.IsSuccessStatusCode && ProjectList.Count == projectListCount + 1)
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK);
                }
                else
                {
                    Assert.Fail();
                }
            }
            catch
            {

                Assert.Fail();
            }
        }

        [TestMethod]
        public void DeleteProjectTest()
        {
            try
            {
                int projectListCount = ProjectList.Count;
                var httpReponse = projectController.DeleteProjectDetails(2);
                if (httpReponse.IsSuccessStatusCode && ProjectList.Count == projectListCount - 1)
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK);
                }
                else
                {
                    Assert.Fail();
                }
            }
            catch
            {

                Assert.Fail();
            }
        }

        [TestMethod]
        public void UpdateProjectTest()
        {
            try
            {
                int projectListCount = ProjectList.Count;
                var httpReponse = projectController.UpdateProjectDetails(new DAC.Project
                {
                    Id = 2,
                    Name = "Project 02",
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(5),
                    Priority = 6,
                    ManagerUserId = 1,
                    User = new DAC.User
                    {
                        Id = 1,
                        EmployeeId = "Emp1",
                        FirstName = "User 1",
                        LastName = "Last Name"
                    }
                });
                if (httpReponse.IsSuccessStatusCode && ProjectList.Count == projectListCount &&
                    ProjectList.FirstOrDefault(x => x.Id == 2).Name == "Project 02")
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK);
                }
                else
                {
                    Assert.Fail();
                }
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}
