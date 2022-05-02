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
    public class GradeController : BaseController<Grade>, iBaseController<Grade>
    {
        public GradeController(SWARMOracleContext context,
            IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List< Grade > lst_t = await _context.Grades.OrderBy(x => x.SchoolId).ThenBy(x => x.StudentId).ThenBy(x => x.SectionId).ThenBy(x => x.GradeTypeCode).ThenBy(x => x.GradeCodeOccurrence).ToListAsync();
            return Ok(lst_t);
        }

        [HttpGet]
        [Route("Get/{t_no}")]
        public async Task<IActionResult> Get(int t_no)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Insufficient number of parameters for composite key, cannot get");
        }

        [HttpGet]
        [Route("Get/{school_no}/{student_no}/{section_no}/{GTC_no}/{GCO_no}")]
        public async Task<IActionResult> Get(int school_no, int student_no, int section_no, string GTC_no, int GCO_no)
        {
            Grade itm_t = await _context.Grades.Where(x => x.SchoolId == school_no && x.StudentId == student_no && x.SectionId == section_no && x.GradeTypeCode == GTC_no && x.GradeCodeOccurrence == GCO_no ).FirstOrDefaultAsync();
            return Ok(itm_t);
        }

        [HttpDelete]
        [Route("Delete/{t_no}")]
        public async Task<IActionResult> Delete(int t_no)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Insufficient number of parameters for composite key, cannot delete");
        }

        [HttpDelete]
        [Route("Delete/{school_no}/{student_no}/{section_no}/{GTC_no}/{GCO_no}")]
        public async Task<IActionResult> Delete(int school_no, int student_no, int section_no, string GTC_no, int GCO_no)
        {
            Grade itm_t = await _context.Grades.Where(x => x.SchoolId == school_no && x.StudentId == student_no && x.SectionId == section_no && x.GradeTypeCode == GTC_no && x.GradeCodeOccurrence == GCO_no).FirstOrDefaultAsync();
            _context.Remove(itm_t);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Grade t_dto)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist_t = await _context.Grades.Where(x => x.SchoolId == t_dto.SchoolId && x.StudentId == t_dto.StudentId && x.SectionId == t_dto.SectionId && x.GradeTypeCode == t_dto.GradeTypeCode && x.GradeCodeOccurrence == t_dto.GradeCodeOccurrence).FirstOrDefaultAsync();

                if (exist_t == null)
                {
                    bExist = false;
                    exist_t = new Grade();
                }
                else
                {
                    bExist = true;
                }

                exist_t.SchoolId = t_dto.SchoolId;
                exist_t.StudentId = t_dto.StudentId;
                exist_t.SectionId = t_dto.SectionId;
                exist_t.GradeTypeCode = t_dto.GradeTypeCode;
                exist_t.GradeCodeOccurrence = t_dto.GradeCodeOccurrence;               
                exist_t.NumericGrade = t_dto.NumericGrade;
                exist_t.Comments = t_dto.Comments;

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
                return Ok(t_dto.SchoolId + ", " + t_dto.StudentId + ", " + t_dto.SectionId + ", " + t_dto.GradeTypeCode + ", " + t_dto.GradeCodeOccurrence);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Grade t_dto)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist_t = await _context.Grades.Where(x => x.SchoolId == t_dto.SchoolId && x.StudentId == t_dto.StudentId && x.SectionId == t_dto.SectionId && x.GradeTypeCode == t_dto.GradeTypeCode && x.GradeCodeOccurrence == t_dto.GradeCodeOccurrence).FirstOrDefaultAsync();

                if (exist_t != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record exists, cannot insert");
                }
                exist_t.SchoolId = t_dto.SchoolId;
                exist_t.StudentId = t_dto.StudentId;
                exist_t.SectionId = t_dto.SectionId;
                exist_t.GradeTypeCode = t_dto.GradeTypeCode;
                exist_t.GradeCodeOccurrence = t_dto.GradeCodeOccurrence;
                exist_t.NumericGrade = t_dto.NumericGrade;
                exist_t.Comments = t_dto.Comments;
                _context.Add(exist_t);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(t_dto.SchoolId + ", " + t_dto.StudentId + ", " + t_dto.SectionId + ", " + t_dto.GradeTypeCode + ", " + t_dto.GradeCodeOccurrence);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}