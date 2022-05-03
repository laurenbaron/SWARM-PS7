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
    public class InstructorController : BaseController<Instructor>, iBaseController<Instructor>
    {
        public InstructorController(SWARMOracleContext context,
            IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Instructor> lst_t = await _context.Instructors.OrderBy(x => x.SchoolId).ThenBy(x => x.InstructorId).ToListAsync();
            return Ok(lst_t);
        }

        [HttpGet]
        [Route("Get/{t_no}")]
        public async Task<IActionResult> Get(int t_no)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Insufficient number of parameters for composite key, cannot get");
        }

        [HttpGet]
        [Route("Get/{school_no}/{instructor_no}")]
        public async Task<IActionResult> Get(int school_no, int instructor_no)
        {
            Instructor itm_t = await _context.Instructors.Where(x => x.SchoolId == school_no && x.InstructorId == instructor_no).FirstOrDefaultAsync();
            return Ok(itm_t);
        }

        [HttpDelete]
        [Route("Delete/{t_no}")]
        public async Task<IActionResult> Delete(int t_no)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Insufficient number of parameters for composite key, cannot delete");
        }

        [HttpDelete]
        [Route("Delete/{school_no}/{instructor_no}")]
        public async Task<IActionResult> Delete(int school_no, int instructor_no)
        {
            Instructor itm_t = await _context.Instructors.Where(x => x.SchoolId == school_no && x.InstructorId == instructor_no).FirstOrDefaultAsync();
            _context.Remove(itm_t);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Instructor t_dto)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist_t = await _context.Instructors.Where(x => x.SchoolId == t_dto.SchoolId && x.InstructorId == t_dto.InstructorId).FirstOrDefaultAsync();

                if (exist_t == null)
                {
                    bExist = false;
                    exist_t = new Instructor();
                }
                else
                {
                    bExist = true;
                }

                exist_t.SchoolId = t_dto.SchoolId;
                exist_t.InstructorId = t_dto.InstructorId;
                exist_t.Salutation = t_dto.Salutation;
                exist_t.FirstName = t_dto.FirstName;
                exist_t.LastName = t_dto.LastName;
                exist_t.StreetAddress = t_dto.StreetAddress;
                exist_t.Zip = t_dto.Zip;
                exist_t.Phone = t_dto.Phone;

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
                return Ok(t_dto.SchoolId + ", " + t_dto.InstructorId );
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Instructor t_dto)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist_t = await _context.Instructors.Where(x => x.SchoolId == t_dto.SchoolId && x.InstructorId == t_dto.InstructorId).FirstOrDefaultAsync();

                if (exist_t != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record exists, cannot insert");
                }
                exist_t = new Instructor();
                //PK is set by trigger 01 for course, instructor, section, student using sequence numbers
                // exist_t.SchoolId = t_dto.SchoolId;
                // exist_t.InstructorId = t_dto.InstructorId;
                exist_t.Salutation = t_dto.Salutation;
                exist_t.FirstName = t_dto.FirstName;
                exist_t.LastName = t_dto.LastName;
                exist_t.StreetAddress = t_dto.StreetAddress;
                exist_t.Zip = t_dto.Zip;
                exist_t.Phone = t_dto.Phone;
                
                _context.Add(exist_t);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(t_dto.SchoolId + ", " + t_dto.InstructorId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}