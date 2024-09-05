using EmployeeLogin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
            private readonly MydbContext _dbContext;
            public EmployeeController(MydbContext mydbContext)
            {
                _dbContext = mydbContext;
            }

            //here we register the employee details
            [HttpPost]
            [Route("Employee Registration")]
            public IActionResult Registration(EmployeeDTO employeeDTO)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var employee = _dbContext.Employeees.FirstOrDefault(x => x.Email == employeeDTO.Email);
                if (employee == null)
                {
                    _dbContext.Employeees.Add(new Employee
                    {
                        FirstName = employeeDTO.FirstName,
                        lastName = employeeDTO.lastName,
                        Email = employeeDTO.Email,
                        Password = employeeDTO.Password
                    });
                    _dbContext.SaveChanges();
                    return Ok("Employee Registration Successfully");
                }
                else
                {
                    return BadRequest("Employee allredy Exist, with Same EMail address. ");

                }
            }

            //Here we create Login for all employee are allready exist
            [HttpPost]
            [Route("Empployee Login")]
            public IActionResult Login(LoginDTO loginDTO)
            {
                var employee = _dbContext.Employeees.FirstOrDefault(x => x.Email == loginDTO.Email && x.Password == loginDTO.Password);
                if (employee != null)
                {
                    return Ok(employee);
                }
                return BadRequest();
            }


        //Here we upadate the existing data of employee

        [HttpPut("{id}")]
        //[Route("Update Employee")]
        public IActionResult UpadateEmployee(int id, [FromBody] EmployeeDTO employeeDTO)
        {
            //chheck if the user exist
            var existinguser = _dbContext.Employeees.Find(id);
            if (existinguser == null)
            {
                return NotFound("!! Employee Not Found");
            }

            //Update the properties
            existinguser.FirstName = employeeDTO.FirstName;
            existinguser.lastName = employeeDTO.lastName;
            existinguser.Email = employeeDTO.Email;
            existinguser.Password = employeeDTO.Password;
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                //Handle Concurrency isues if the record was updated by another processs
                if (!_dbContext.Employeees.Any(e => e.EmployeeId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok("Employee Updated Successfully"); //4 No content 

        }


        //Here we delete the data or records 
        [HttpDelete("{id}")]
        //[Route("Delete Employee")]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = _dbContext.Employeees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            _dbContext.Employeees.Remove(employee);
            _dbContext.SaveChanges();
            return Ok("Employee Deleted Successfully");
        }

        //here get all Employee

        [HttpGet]
            [Route("Get all Employee")]
            public IActionResult GetEmployee()
            {
                return Ok(_dbContext.Employeees.ToList());
            }

            //Here get emloyee by specific condition
            [HttpGet]
            [Route("Get Specific Employee")]
            public IActionResult GetUser(int id)
            {
                var employee = _dbContext.Employeees.FirstOrDefault(x => x.EmployeeId == id);
                if (employee != null)
                    return Ok(employee);

                else
                    return NoContent();
            }
    } 
} 

