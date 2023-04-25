using Common;
using Common.Validator;
using EmpwebAPI;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EmployeeDbContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeDB")));
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IValidator<Employee>, EmployeeValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    //Global Exception Handler
    app.UseMiddleware<ExceptionHandler>();
}

//Search Employee with id
app.MapGet("/employees/{id:int}", async (IEmployeeRepository empRepo,int id) =>
{    
    if(id<=0) return Results.NotFound();

    var employee = await empRepo.GetEmployeeByIdAsync(id);
    return employee != null ? Results.Ok(employee) : Results.NotFound();    
});

//Search Employee with Name
app.MapGet("/employees/{name}", async (IEmployeeRepository empRepo, string name) =>
{
    if (string.IsNullOrWhiteSpace(name)) return Results.NotFound();

    var employee = await empRepo.GetEmployeeByNameAsync(name);
    return employee == null ? Results.NotFound() : Results.Ok(employee);
});

//Search with query parameter 
app.MapGet("/employees", async (IEmployeeRepository empRepo,int pageNumber, int pageSize) =>
{
    if (pageNumber < 0 || pageSize < 0)
        return Results.NotFound();
    
    return Results.Ok(await empRepo.GetEmployeesAsync(pageNumber, pageSize));        
});

//Create New employee
app.MapPost("/employees", async (IEmployeeRepository empRepo, Employee emp) =>
{
    await empRepo.AddEmployeeAsync(emp);
    await empRepo.SaveAsync();
    
    return Results.Ok();
});

//Update Employee
app.MapPut("/employees", async (IValidator<Employee> employeeValidator, IEmployeeRepository empRepo, Employee emp) =>
{
    var validationResult = await employeeValidator.ValidateAsync(emp);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }
    await empRepo.UpdateEmployeeAsync(emp);
    await empRepo.SaveAsync();

    return Results.Ok();
});

//Remove Employee by id
app.MapDelete("/employees/{id}", async (IEmployeeRepository empRepo, int id) =>
{
    if (id<=0) return Results.NotFound();

    await empRepo.DeleteEmployeeAsync(id);
    await empRepo.SaveAsync();
    
    return Results.Ok();
});

//Remove Employee by name
app.MapDelete("/employees/{name}", async (IEmployeeRepository empRepo,string name) =>
{
    if(string.IsNullOrEmpty(name)) return Results.NotFound();
    
    await empRepo.DeleteEmployeeByNameAsync(name);
    await empRepo.SaveAsync();

    return Results.Ok();
});

app.MapGet("/", () => "Employee API!");

app.Run();
