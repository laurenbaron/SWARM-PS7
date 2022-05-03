using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SWARM.EF.Data;
using SWARM.EF.Models;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWARM.Server.Controllers.Base;

namespace SWARM.Server.Controllers.Application
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : BaseController<Student>, iBaseController<Student>
    {
        public StudentController(SWARMOracleContext context,
            IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Student> lst_t = await _context.Students.OrderBy(x => x.StudentId).ToListAsync();
            return Ok(lst_t);
        }

        [HttpGet]
        [Route("Get/{t_no}")]
        public async Task<IActionResult> Get(int t_no)
        {
            Student itm_t = await _context.Students.Where(x => x.StudentId == t_no).FirstOrDefaultAsync();
            return Ok(itm_t);
        }

        [HttpDelete]
        [Route("Delete/{t_no}")]
        public async Task<IActionResult> Delete(int t_no)
        {
            Student itm_t = await _context.Students.Where(x => x.StudentId == t_no).FirstOrDefaultAsync();
            _context.Remove(itm_t);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Student t_dto)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist_t = await _context.Students.Where(x => x.StudentId == t_dto.StudentId).FirstOrDefaultAsync();

                if (exist_t == null)
                {
                    bExist = false;
                    exist_t = new Student();
                }
                else
                {
                    bExist = true;
                }

                exist_t.StudentId = t_dto.StudentId;
                exist_t.Salutation = t_dto.Salutation;
                exist_t.FirstName = t_dto.FirstName;
                exist_t.LastName = t_dto.LastName;
                exist_t.StreetAddress = t_dto.StreetAddress;
                exist_t.Zip = t_dto.Zip;
                exist_t.Phone = t_dto.Phone;
                exist_t.Employer = t_dto.Employer;
                exist_t.RegistrationDate = t_dto.RegistrationDate;
                exist_t.SchoolId = t_dto.SchoolId;

                if (bExist)
                {
                    _context.Update(exist_t);
                }
                else
                {
                    _context.Add(exist_t);
                }
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(t_dto.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student t_dto)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist_t = await _context.Students.Where(x => x.StudentId == t_dto.StudentId).FirstOrDefaultAsync();

                if (exist_t != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record exists, cannot insert");
                }

                exist_t = new Student();
                //    exist_t.StudentId = t_dto.StudentId;
                exist_t.Salutation = t_dto.Salutation;
                exist_t.FirstName = t_dto.FirstName;
                exist_t.LastName = t_dto.LastName;
                exist_t.StreetAddress = t_dto.StreetAddress;
                exist_t.Zip = t_dto.Zip;
                exist_t.Phone = t_dto.Phone;
                exist_t.Employer = t_dto.Employer;
                exist_t.RegistrationDate = t_dto.RegistrationDate;
                exist_t.SchoolId = t_dto.SchoolId;

                _context.Add(exist_t);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(t_dto.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}