using Microsoft.AspNetCore.Mvc;
using MyDotnetProject.Models;
using Microsoft.EntityFrameworkCore;


[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly DemoContext _context;

    public EmployeeController(DemoContext context)
    {
        _context = context;
    }

//Sync Get Method:
[HttpGet]
public List<Employee>  GetEmployees()
{
    var employees =  _context.Employees.ToList();
    return employees;
}

[HttpGet("getEmployees")]
public String getEmployees()
{
    var employees = "Checking My Pipeline Testing Now For Panda ";
    return employees;
}

[HttpPost]
public async Task<IActionResult> AddEmployee(Employee employee)
{
    if (employee == null)
    {
        return BadRequest("Employee data is null.");
    }
    await _context.Employees.AddAsync(employee);
    await _context.SaveChangesAsync();
    return CreatedAtAction(nameof(GetEmployees), new { id = employee.Id }, employee);
}


[HttpPost("add")]
public Object AddEmployees(Employee employee)
{
    if (employee == null)
    {
        return BadRequest("Employee data is null.");
    }
     _context.Employees.Add(employee);
     _context.SaveChanges();

    return employee;
}
}