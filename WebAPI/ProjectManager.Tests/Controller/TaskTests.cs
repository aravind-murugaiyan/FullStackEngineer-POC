using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjectManager.Controllers;

namespace ProjectManager.Tests.Controller
{
    [TestClass]
    public class TaskTests
    {
        List<DAC.Task> TaskList = null;
        TaskController taskController;
        public TaskTests()
        {
            SetupTestData();
        }

        public void SetupTestData()
        {
            TaskList = new List<DAC.Task>()
            {
                new DAC.Task
                {
                    Id = 1,
                    StartDate = DateTime.Today,
                    EndDate=DateTime.Today.AddDays(2),
                    Name="Task 1",
                    Priority=1,
                    Status=0,
                    ProjectId=1
                },
                new DAC.Task
                {
                    Id = 2,
                    StartDate = DateTime.Today.AddDays(1),
                    EndDate=DateTime.Today.AddDays(3),
                    Name="Task 2",
                    Priority=5,
                    Status=1,
                    ProjectId=1
                },
                new DAC.Task
                {
                    Id = 3,
                    StartDate = DateTime.Today.AddDays(2),
                    EndDate=DateTime.Today.AddDays(4),
                    Name="Task 3",
                    Priority=4,
                    Status=3,
                    ProjectId=2
                }
            };
            var tasks = TaskList.AsQueryable();
            var taskMock = new Mock<DbSet<DAC.Task>>();
            taskMock.Setup(x => x.Add(It.IsAny<DAC.Task>())).Returns((DAC.Task tsk) =>
            {
                TaskList.Add(tsk);
                return tsk;
            });
            taskMock.Setup(x => x.Remove(It.IsAny<DAC.Task>())).Returns((DAC.Task tsk) =>
            {
                var u = TaskList.FirstOrDefault(x => x.Id == tsk.Id);
                TaskList.Remove(u);
                return tsk;
            });
            taskMock.As<IQueryable<DAC.Task>>().Setup(m => m.Provider).Returns(tasks.Provider);
            taskMock.As<IQueryable<DAC.Task>>().Setup(m => m.Expression).Returns(tasks.Expression);
            taskMock.As<IQueryable<DAC.Task>>().Setup(m => m.ElementType).Returns(tasks.ElementType);
            taskMock.As<IQueryable<DAC.Task>>().Setup(m => m.GetEnumerator()).Returns(tasks.GetEnumerator());

            var dbContext = new Mock<DAC.Entities>();
            dbContext.Setup(usr => usr.Tasks).Returns(taskMock.Object);
            taskController = new TaskController(dbContext.Object)
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new System.Web.Http.HttpConfiguration()
            };
        }

        [TestMethod]
        public void GetTasksTest()
        {
            try
            {
                var httpReponse = taskController.GetTasks();
                List<Models.Task> taskList = null;
                if (httpReponse.TryGetContentValue<List<Models.Task>>(out taskList))
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK && taskList.Count == 3);
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
        public void GetSortedTasksByStartDateAscTest()
        {
            try
            {
                var httpReponse = taskController.GetSortedTasks(1, "startDate", "asc");
                List<Models.Task> taskList = null;
                if (httpReponse.TryGetContentValue<List<Models.Task>>(out taskList))
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK && taskList.Count == 3 && taskList[0].StartDate < taskList[1].StartDate);
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
        public void GetSortedTasksByStartDateDescTest()
        {
            try
            {
                var httpReponse = taskController.GetSortedTasks(1, "startDate", "desc");
                List<Models.Task> taskList = null;
                if (httpReponse.TryGetContentValue<List<Models.Task>>(out taskList))
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK && taskList.Count == 3 && taskList[0].StartDate > taskList[1].StartDate);
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
        public void GetSortedTasksByEndDateAscTest()
        {
            try
            {
                var httpReponse = taskController.GetSortedTasks(1, "endDate", "asc");
                List<Models.Task> taskList = null;
                if (httpReponse.TryGetContentValue<List<Models.Task>>(out taskList))
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK && taskList.Count == 3 && taskList[0].EndDate < taskList[1].EndDate);
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
        public void GetSortedTasksByEndDateDescTest()
        {
            try
            {
                var httpReponse = taskController.GetSortedTasks(1, "endDate", "desc");
                List<Models.Task> taskList = null;
                if (httpReponse.TryGetContentValue<List<Models.Task>>(out taskList))
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK && taskList.Count == 3 && taskList[0].EndDate > taskList[1].EndDate);
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
        public void GetSortedTasksByPriorityAscTest()
        {
            try
            {
                var httpReponse = taskController.GetSortedTasks(1, "priority", "asc");
                List<Models.Task> taskList = null;
                if (httpReponse.TryGetContentValue<List<Models.Task>>(out taskList))
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK && taskList.Count == 3 && taskList[0].Priority < taskList[1].Priority);
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
        public void GetSortedTasksByPriorityDescTest()
        {
            try
            {
                var httpReponse = taskController.GetSortedTasks(1, "priority", "desc");
                List<Models.Task> taskList = null;
                if (httpReponse.TryGetContentValue<List<Models.Task>>(out taskList))
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK && taskList.Count == 3 && taskList[0].Priority > taskList[1].Priority);
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
        public void GetSortedTasksByStatusAscTest()
        {
            try
            {
                var httpReponse = taskController.GetSortedTasks(1, "status", "asc");
                List<Models.Task> taskList = null;
                if (httpReponse.TryGetContentValue<List<Models.Task>>(out taskList))
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK && taskList.Count == 3 && taskList[0].Status < taskList[1].Status);
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
        public void GetSortedTasksByStatusDescTest()
        {
            try
            {
                var httpReponse = taskController.GetSortedTasks(1, "status", "desc");
                List<Models.Task> taskList = null;
                if (httpReponse.TryGetContentValue<List<Models.Task>>(out taskList))
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK && taskList.Count == 3 && taskList[0].Status > taskList[1].Status);
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
        public void GetTaskByProjectIdTest()
        {
            try
            {
                var httpReponse = taskController.GetTaskByProjectId(1);
                int totalTaskCount = TaskList.Count;
                List<Models.Task> taskList = null;
                if (httpReponse.TryGetContentValue<List<Models.Task>>(out taskList))
                {
                    Assert.IsTrue(httpReponse.StatusCode == System.Net.HttpStatusCode.OK && totalTaskCount == 3 && taskList.Count == 2);
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
        public void AddTaskTest()
        {
            try
            {
                int totalTaskListCount = TaskList.Count;
                var httpReponse = taskController.AddTask(new Models.Task
                {
                    Id = 4,
                    StartDate = DateTime.Today.AddDays(5),
                    EndDate = DateTime.Today.AddDays(9),
                    Name = "Task 4",
                    Priority = 5,
                    Status = 1,
                    ProjectId = 3
                });
                if (httpReponse.IsSuccessStatusCode && TaskList.Count == totalTaskListCount + 1)
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
        public void UpdateTaskTest()
        {
            try
            {
                int taskListCount = TaskList.Count;
                var httpReponse = taskController.UpdateTaskDetails(new Models.Task
                {
                    Id = 2,
                    StartDate = DateTime.Today.AddDays(5),
                    EndDate = DateTime.Today.AddDays(9),
                    Name = "Task 02",
                    Priority = 5,
                    Status = 1,
                    ProjectId = 3
                });
                if (httpReponse.IsSuccessStatusCode && TaskList.Count == taskListCount &&
                    TaskList.FirstOrDefault(x => x.Id == 2).Name == "Task 02")
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
        public void DeleteTaskTest()
        {
            try
            {
                int taskListCount = TaskList.Count;
                var httpReponse = taskController.EndTask(new Models.Task
                {
                    Id = 1
                });
                if (httpReponse.IsSuccessStatusCode && TaskList.FirstOrDefault(t => t.Id == 1).Status == 1)
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
