using ProjectManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ProjectManager.ActionFilters;
using DAC = ProjectManager.DAC;
using System.Web.Http.Cors;
using System.Net.Http;
using System.Net;
using System.Data.Entity;

namespace ProjectManager.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
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

        public UserController()
        {

        }

        public UserController(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [ProjectManagerLogFilter]
        [Route("api/users")]
        public HttpResponseMessage GetUsers()

        {
            List<User> userList = null;
            userList = DbContext.Users.Select(u => new User
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                EmployeeId = u.EmployeeId
            }).ToList();
            return Request.CreateResponse(userList);
        }

        [HttpGet]
        [ProjectManagerLogFilter]
        [Route("api/users/{sortBy}/{sortOrder}")]
        public HttpResponseMessage GetUsersSorted(string sortBy, string sortOrder)

        {
            IQueryable<User> userList = null;
            userList = DbContext.Users.Select(u => new User
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                EmployeeId = u.EmployeeId
            });
            switch (sortBy.ToUpper().Trim())
            {
                case "FIRSTNAME":
                    if (sortOrder == "asc")
                    {
                        userList = userList.OrderBy(u => u.FirstName);
                    }
                    else if (sortOrder == "desc")
                    {
                        userList = userList.OrderByDescending(u => u.FirstName);
                    }
                    break;
                case "LASTNAME":
                    if (sortOrder == "asc")
                    {
                        userList = userList.OrderBy(u => u.LastName);
                    }
                    else if (sortOrder == "desc")
                    {
                        userList = userList.OrderByDescending(u => u.LastName);
                    }
                    break;
                case "EMPID":
                    if (sortOrder == "asc")
                    {
                        userList = userList.OrderBy(u => u.EmployeeId);
                    }
                    else if (sortOrder == "desc")
                    {
                        userList = userList.OrderByDescending(u => u.EmployeeId);
                    }
                    break;
            }

            return Request.CreateResponse(userList);
        }
        [HttpGet]
        [ProjectManagerLogFilter]
        [Route("api/user/{userId}")]
        public HttpResponseMessage GetUserById(long userId)
        {
            HttpResponseMessage response = null;
            var user = DbContext.Users.FirstOrDefault(usr => usr.Id == userId);
            if (user != null)
            {
                response = Request.CreateResponse<DAC.User>(user);
            }
            else
            {
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User not found with Id " + userId);
            }
            return response;
        }

        [HttpPost]
        [ProjectManagerLogFilter]
        [Route("api/user/add")]
        public HttpResponseMessage AddUser(DAC.User user)
        {
            if (user == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User cannot be null.");
            }
            try
            {
                DbContext.Users.Add(user);
                return Request.CreateResponse(dbContext.SaveChanges());
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error in adding user : " + ex.Message);
            }
        }

        [HttpPost]
        [Route("api/user/update")]
        [ProjectManagerLogFilter]
        public HttpResponseMessage UpdateUserDetails(DAC.User user)
        {
            var usr = DbContext.Users.FirstOrDefault(u => u.Id == user.Id);
            if (usr != null)
            {
                usr.FirstName = user.FirstName;
                usr.LastName = user.LastName;
                usr.EmployeeId = user.EmployeeId;
                return Request.CreateResponse(dbContext.SaveChanges());
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "User with id " + user.Id + " not found.");
            }
        }

        [HttpPost]
        [Route("api/user/delete")]
        [ProjectManagerLogFilter]
        public HttpResponseMessage DeleteUserDetails(User user)
        {
            if (user == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User cannot be null");
            }
            else if (!DbContext.Users.Any(usr => usr.Id == user.Id))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, String.Format("User with id {0} not found.", user.Id));
            }
            try
            {
                var usr = DbContext.Users.FirstOrDefault(u => u.Id == user.Id);
                if (usr != null)
                {
                    DbContext.Users.Remove(usr);
                    return Request.CreateResponse(dbContext.SaveChanges());
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, String.Format("User with id {0} not found.", user.Id));
                }
            }
            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Unable to delete user with id " + user.Id);
            }
        }
    }
}