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
    public class EnrollmentController : BaseController<Enrollment>, iBaseController<Enrollment>
    {
        public EnrollmentController(SWARMOracleContext context,
            IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Enrollment> lst_t = await _context.Enrollments.OrderBy(x => x.SectionId).ThenBy(x => x.StudentId).ToListAsync();
            return Ok(lst_t);
        }

        //need these methods because they are in iBaseController but cannot just have 1 parameter bc uses composite key
        [HttpGet]
        [Route("Get/{t_no}")]
        public async Task<IActionResult> Get(int t_no)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Insufficient number of parameters for composite key, cannot get");
        }

        [HttpGet]
        [Route("Get/{section_no}/{student_no}")]
        public async Task<IActionResult> Get(int section_no, int student_no)
        {
            Enrollment itm_t = await _context.Enrollments.Where(x => x.SectionId == section_no && x.StudentId == student_no).FirstOrDefaultAsync();
            return Ok(itm_t);
        }

        //need these methods because they are in iBaseController but cannot just have 1 parameter bc uses composite key
        [HttpDelete]
        [Route("Delete/{t_no}")]
        public async Task<IActionResult> Delete(int t_no)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Insufficient number of parameters for composite key, cannot delete");
        }

        [HttpDelete]
        [Route("Delete/{section_no}/{student_no}")]
        public async Task<IActionResult> Delete(int section_no, int student_no)
        {
            Enrollment itm_t = await _context.Enrollments.Where(x => x.SectionId == section_no && x.StudentId == student_no).FirstOrDefaultAsync();
            _context.Remove(itm_t);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Enrollment t_dto)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist_t = await _context.Enrollments.Where(x => x.SectionId == t_dto.SectionId && x.StudentId == t_dto.StudentId).FirstOrDefaultAsync();

                if (exist_t == null)
                {
                    bExist = false;
                    exist_t = new Enrollment();
                }
                else
                {
                    bExist = true;
                }

                exist_t.SectionId = t_dto.SectionId;
                exist_t.StudentId = t_dto.StudentId;
                exist_t.EnrollDate = t_dto.EnrollDate;
                exist_t.SchoolId = t_dto.SchoolId;
                exist_t.FinalGrade = t_dto.FinalGrade;
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
                return Ok(t_dto.SectionId + ", " + t_dto.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Enrollment t_dto)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist_t = await _context.Enrollments.Where(x => x.SectionId == t_dto.SectionId && x.StudentId == t_dto.StudentId).FirstOrDefaultAsync();

                if (exist_t != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record exists, cannot insert");
                }
                exist_t = new Enrollment();
                exist_t.SectionId = t_dto.SectionId;
                exist_t.StudentId = t_dto.StudentId;
                exist_t.EnrollDate = t_dto.EnrollDate;
                exist_t.SchoolId = t_dto.SchoolId;
                exist_t.FinalGrade = t_dto.FinalGrade;
                _context.Add(exist_t);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(t_dto.SectionId + ", " + t_dto.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}