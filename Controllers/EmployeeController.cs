using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApi1.Models;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using ZstdSharp.Unsafe;

namespace WebApi1.Controllers
{
    [RoutePrefix("api/timesheet")]
    public class EmployeeController : ApiController
    {
        [HttpGet]
        [Route("getemployee")]
        public IHttpActionResult GetEmployee()
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    string query = "select Employee_Id,Employee_Name,Employee_Task,Employee_Notes,EntryDateTime from timesheetdetails";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    con.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())

                    {
                        while (reader.Read())
                        {
                            Employee emp = new Employee
                            {
                                Employeename = reader["Employee_Name"].ToString(),
                                Employeeid = reader["Employee_Id"].ToString(),
                                Employeetask = reader["Employee_Task"].ToString(),
                                Employeenotes = reader["Employee_Notes"].ToString(),
                                Datetime = reader["EntryDateTime"].ToString()

                            };
                            employees.Add(emp);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return Ok(employees);
        }
        [HttpPost]
        [Route("postemployee")]
        public IHttpActionResult PostEmployee([FromBody]Employee emp) {
            string Employeename= Environment.UserName;
            if (emp == null) { 
                return BadRequest("Invalid post");
            }
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            try
            {
                using(MySqlConnection con = new MySqlConnection(connectionString)){
                    con.Open();
                    string query = "Insert into timesheetdetails(Employee_Name,Employee_Task,Employee_Notes,EntryDateTime) values(@Employee_Name,@Employee_Task,@Employee_Notes,@EntryDateTime)";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Employee_Name", Employeename);
                    cmd.Parameters.AddWithValue("@Employee_Task", emp.Employeetask);
                    cmd.Parameters.AddWithValue("@Employee_Notes", emp.Employeenotes);
                    cmd.Parameters.AddWithValue("@EntryDateTime", emp.Datetime);
                    int res=cmd.ExecuteNonQuery();
                    if (res > 0) {
                        Console.WriteLine("User Created Successfully.");
                        return Ok(new
                        {
                            Message = "User Created Successfully.",
                            Employee=emp,
                            WindowsUserName=Employeename
                        });
                    }
                    else
                    {
                        return BadRequest("An error occured storing in database");
                    }
                }
                
                
            }
            catch (Exception ex) {
            }
            return Ok();
        }
    }
}