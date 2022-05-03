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
    public class GradeConversionController : BaseController<GradeConversion>, iBaseController<GradeConversion>
    {
        public GradeConversionController(SWARMOracleContext context,
            IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<GradeConversion> lst_t = await _context.GradeConversions.OrderBy(x => x.SchoolId).ThenBy(x => x.LetterGrade).ToListAsync();
            return Ok(lst_t);
        }

        [HttpGet]
        [Route("Get/{t_no}")]
        public async Task<IActionResult> Get(int t_no)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Insufficient number of parameters for composite key, cannot get");
        }

        [HttpGet]
        [Route("Get/{school_no}/{letter_grade}")]
        public async Task<IActionResult> Get(int school_no, string letter_grade)
        {
            GradeConversion itm_t = await _context.GradeConversions.Where(x => x.SchoolId == school_no && x.LetterGrade == letter_grade).FirstOrDefaultAsync();
            return Ok(itm_t);
        }

        [HttpDelete]
        [Route("Delete/{t_no}")]
        public async Task<IActionResult> Delete(int t_no)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Insufficient number of parameters for composite key, cannot delete");
        }

        [HttpDelete]
        [Route("Delete/{school_no}/{letter_grade}")]
        public async Task<IActionResult> Delete(int school_no, string letter_grade)
        {
            GradeConversion itm_t = await _context.GradeConversions.Where(x => x.SchoolId == school_no && x.LetterGrade == letter_grade).FirstOrDefaultAsync();
            _context.Remove(itm_t);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeConversion t_dto)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist_t = await _context.GradeConversions.Where(x => x.SchoolId == t_dto.SchoolId && x.LetterGrade == t_dto.LetterGrade).FirstOrDefaultAsync();

                if (exist_t == null)
                {
                    bExist = false;
                    exist_t = new GradeConversion();
                }
                else
                {
                    bExist = true;
                }

                exist_t.SchoolId = t_dto.SchoolId;
                exist_t.LetterGrade = t_dto.LetterGrade;
                exist_t.GradePoint = t_dto.GradePoint;
                exist_t.MaxGrade = t_dto.MaxGrade;
                exist_t.MinGrade = t_dto.MinGrade;

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
                return Ok(t_dto.SchoolId + ", " + t_dto.LetterGrade);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeConversion t_dto)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist_t = await _context.GradeConversions.Where(x => x.SchoolId == t_dto.SchoolId && x.LetterGrade == t_dto.LetterGrade).FirstOrDefaultAsync();

                if (exist_t != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record exists, cannot insert");
                }
                exist_t = new GradeConversion();
                exist_t.SchoolId = t_dto.SchoolId;
                exist_t.LetterGrade = t_dto.LetterGrade;
                exist_t.GradePoint = t_dto.GradePoint;
                exist_t.MaxGrade = t_dto.MaxGrade;
                exist_t.MinGrade = t_dto.MinGrade;
                _context.Add(exist_t);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(t_dto.SchoolId + ", " + t_dto.LetterGrade);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}