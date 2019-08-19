using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjectManager.Controllers;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;

namespace ProjectManager.Tests.Controller
{
    /// <summary>
    /// Summary description for UserTests
    /// </summary>
    [TestClass]
    public class UserTests
    {
        List<DAC.User> UserList = null;
        UserController userControllers;
        public UserTests()
        {
            SetupTestData();
        }

        public void SetupTestData()
        {
            UserList = new List<DAC.User>()
                {
                    new DAC.User
                    {
                        EmployeeId = "EMP1",
                        FirstName = "Aravind",
                        LastName = "Murugaiyan",
                        Id = 1
                    },
                    new DAC.User
                    {
                        EmployeeId = "EMP2",
                        FirstName = "Murugaiyan",
                        LastName = "Aravind",
                        Id = 2
                    }
                };
            var users = UserList.AsQueryable();
            var usersMock = new Mock<DbSet<DAC.User>>();
            usersMock.Setup(x => x.Add(It.IsAny<DAC.User>())).Returns((DAC.User usr) =>
            {
                UserList.Add(usr);
                return usr;
            });
            usersMock.Setup(x => x.Remove(It.IsAny<DAC.User>())).Returns((DAC.User usr) =>
            {
                var u = UserList.FirstOrDefault(x => x.Id == usr.Id);
                UserList.Remove(u);
                return usr;
            });
            usersMock.As<IQueryable<DAC.User>>().Setup(m => m.Provider).Returns(users.Provider);
            usersMock.As<IQueryable<DAC.User>>().Setup(m => m.Expression).Returns(users.Expression);
            usersMock.As<IQueryable<DAC.User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            usersMock.As<IQueryable<DAC.User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            var dbContext = new Mock<DAC.Entities>();
            dbContext.Setup(usr => usr.Users).Returns(usersMock.Object);
            userControllers = new UserController(dbContext.Object)
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new System.Web.Http.HttpConfiguration()
            };
        }

        [TestMethod]
        public void GetAllUsersTest()
        {
            try
            {
                var httpReponse = userControllers.GetUsers();
                List<Models.User> userList = null;
                if (httpReponse.TryGetContentValue<List<Models.User>>(out userList))
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
        public void GetSortedUsersByEmpIdAscTest()
        {
            try
            {
                var httpReponse = userControllers.GetUsersSorted("EMPID","asc");
                IQueryable<Models.User> userList = null;
                if (httpReponse.TryGetContentValue<IQueryable<Models.User>>(out userList))
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
        public void GetSortedUsersByEmpIdDescTest()
        {
            try
            {
                var httpReponse = userControllers.GetUsersSorted("EMPID","desc");
                IQueryable<Models.User> userList = null;
                if (httpReponse.TryGetContentValue<IQueryable<Models.User>>(out userList))
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
        public void GetSortedUsersByFirstNameAscTest()
        {
            try
            {
                var httpReponse = userControllers.GetUsersSorted("FIRSTNAME|","asc");
                IQueryable<Models.User> userList = null;
                if (httpReponse.TryGetContentValue<IQueryable<Models.User>>(out userList))
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
        public void GetSortedUsersByFirstNameDscTest()
        {
            try
            {
                var httpReponse = userControllers.GetUsersSorted("FIRSTNAME","desc");
                IQueryable<Models.User> userList = null;
                if (httpReponse.TryGetContentValue<IQueryable<Models.User>>(out userList))
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
        public void GetSortedUsersByLastNameAscTest()
        {
            try
            {
                var httpReponse = userControllers.GetUsersSorted("LASTNAME", "asc");
                IQueryable<Models.User> userList = null;
                if (httpReponse.TryGetContentValue<IQueryable<Models.User>>(out userList))
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
        public void AddUserTest()
        {
            try
            {
                int userListCount = UserList.Count;
                var httpReponse = userControllers.AddUser(new DAC.User
                {
                    EmployeeId = "267127",
                    FirstName = "Aravind",
                    LastName = "Murugaiyan",
                    Id = 3
                });
                if (httpReponse.IsSuccessStatusCode && UserList.Count == userListCount + 1)
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
        public void DeleteUserTest()
        {
            try
            {
                int userListCount = UserList.Count;
                var httpReponse = userControllers.DeleteUserDetails(new Models.User
                {
                    Id = 2
                });
                if (httpReponse.IsSuccessStatusCode && UserList.Count == userListCount - 1)
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
        public void UpdateUserTest()
        {
            try
            {
                int userListCount = UserList.Count;
                var httpReponse = userControllers.UpdateUserDetails(new DAC.User
                {
                    EmployeeId = "267127",
                    FirstName = "Aravind",
                    LastName = "Murugaiyan",
                    Id = 2
                });
                if (httpReponse.IsSuccessStatusCode && UserList.Count == userListCount &&
                    UserList.FirstOrDefault(x => x.Id == 2).EmployeeId == "267127")
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
