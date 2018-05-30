using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using EmployeeDataAccess;

namespace EmpService.Controllers
{
    [EnableCors("*", "*", "*")]
    public class EmployeesController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetSomeThing()
        {
            using (var enitties = new EmployeeDBEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, enitties.Employees.AsNoTracking().ToList());
            }
        }

        [HttpGet]
        public HttpResponseMessage EmployeeById(int id)
        {
            using (var entities = new EmployeeDBEntities())
            {
                var entity = entities.Employees.AsNoTracking().FirstOrDefault(e => e.ID == id);
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with ID " + id + " Not Found");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
            }
        }


        public HttpResponseMessage Put([FromUri]int id, [FromBody]Employee employee)
        {
            try
            {
                using (var entities = new EmployeeDBEntities())
                {
                    var emp = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (emp == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The Employee with Id " + id + " Not found to Update");
                    }
                    else {
                        emp.FirstName = employee.FirstName;
                        emp.LastName = employee.LastName;
                        emp.Salary = employee.Salary;
                        emp.Gender = employee.Gender;

                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, emp);
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Post([FromBody]Employee employee)
        {
            try
            {
                using(var entities = new EmployeeDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + "/ "+ employee.ID.ToString());
                    return message;

                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Delete([FromUri]int id)
        {
            try
            {
                using(var entities = new EmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if(entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,"The Employee with ID " + id + "Not found to delete");
                    }
                    else
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "The Employee with Id " + entity.ID + " has deleted");
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}