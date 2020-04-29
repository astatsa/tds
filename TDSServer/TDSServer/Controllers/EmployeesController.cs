using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using TDSDTO;
using TDSServer.Models;
using DTO = TDSDTO.References;

namespace TDSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : BaseReferenceController<Models.Employee, DTO.Employee>
    {
        public EmployeesController(AppDbContext dbContext) : base(dbContext)
        {
        }

        [HttpGet("user/{userId}")]
        public Task<Employee> EmployeesByUser(int userId) =>
            dbContext.Employees
                .FirstOrDefaultAsync(x => x.UserId == userId);

        [HttpGet]
        [Authorize(Roles = "ReferenceRead")]
        public async Task<ApiResult<List<DTO.Employee>>> GetEmployees()
        {
            try
            {
                return ApiResult(
                    await dbContext.Employees
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
                        .ToListAsync());
            }
            catch(Exception ex)
            {
                return ApiResult<List<DTO.Employee>>(null, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "ReferenceEdit")]
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
            catch(DbUpdateException ex)
            {
                return new ApiResult<bool>(false, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
            catch(Exception ex)
            {
                return new ApiResult<bool>(false, ex.Message);
            }
            return new ApiResult<bool>(true);
        }
    }
}