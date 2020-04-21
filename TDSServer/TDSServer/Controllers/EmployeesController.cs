using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using TDSServer.Models;
using DTO = TDSDTO.References;

namespace TDSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private AppDbContext dbContext;
        public EmployeesController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("user/{userId}")]
        public Task<Employee> EmployeesByUser(int userId) =>
            dbContext.Employees
                .FirstOrDefaultAsync(x => x.UserId == userId);

        [HttpGet]
        [Authorize(Roles = "EmployeeRead")]
        public Task<List<DTO.Employee>> GetEmployees() =>
            dbContext.Employees
                .Include(x => x.User)
                .Include(x => x.Position)
                .Select(x => new DTO.Employee
                {
                    Name = x.Name,
                    FullName = x.FullName,
                    UserId = x.UserId,
                    PositionId = x.PositionId,
                    PositionName = x.Position.Name,
                    UserName = x.User.FullName,
                    Id = x.Id,  
                    IsDeleted = x.IsDeleted
                })
                .ToListAsync();

        [HttpPost]
        [Authorize(Roles = "EmployeeEdit")]
        public async Task<ApiResult<bool>> SaveEmployee([FromBody] DTO.Employee employee)
        {
            try
            {
                Employee em;
                if (employee.Id != default)
                {
                    em = await dbContext.Employees.FirstOrDefaultAsync(x => x.Id == employee.Id);
                    if (em == null)
                    {
                        return new ApiResult<bool>(false, "Не найден элемент справочника!");
                    }
                }
                else
                {
                    em = new Employee();
                    dbContext.Employees.Add(em);
                }

                em.FullName = employee.FullName;
                em.Name = employee.Name;
                em.PositionId = employee.PositionId;
                em.UserId = employee.UserId;

                await dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return new ApiResult<bool>(false, ex.Message);
            }
            return new ApiResult<bool>(true);
        }
    }
}