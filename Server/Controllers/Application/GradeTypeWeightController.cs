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
    public class GradeTypeWeightController : BaseController<GradeTypeWeight>, iBaseController<GradeTypeWeight>
    {
        public GradeTypeWeightController(SWARMOracleContext context,
            IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<GradeTypeWeight> lst_t = await _context.GradeTypeWeights.OrderBy(x => x.SchoolId).ThenBy(x => x.SectionId).ThenBy(x => x.GradeTypeCode).ToListAsync();
            return Ok(lst_t);
        }

        [HttpGet]
        [Route("Get/{t_no}")]
        public async Task<IActionResult> Get(int t_no)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Insufficient number of parameters for composite key, cannot get");
        }

        [HttpGet]
        [Route("Get/{school_no}/{section_no}/{GTC_no}")]
        public async Task<IActionResult> Get(int school_no, int section_no, string GTC_no)
        {
            GradeTypeWeight itm_t = await _context.GradeTypeWeights.Where(x => x.SchoolId == school_no && x.SectionId == section_no && x.GradeTypeCode == GTC_no).FirstOrDefaultAsync();
            return Ok(itm_t);
        }

        [HttpDelete]
        [Route("Delete/{t_no}")]
        public async Task<IActionResult> Delete(int t_no)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Insufficient number of parameters for composite key, cannot delete");
        }

        [HttpDelete]
        [Route("Delete/{school_no}/{section_no}/{GTC_no}")]
        public async Task<IActionResult> Delete(int school_no, int section_no, string GTC_no)
        {
            GradeTypeWeight itm_t = await _context.GradeTypeWeights.Where(x => x.SchoolId == school_no && x.SectionId == section_no && x.GradeTypeCode == GTC_no).FirstOrDefaultAsync();
            _context.Remove(itm_t);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeTypeWeight t_dto)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist_t = await _context.GradeTypeWeights.Where(x => x.SchoolId == t_dto.SchoolId && x.SectionId == t_dto.SectionId && x.GradeTypeCode == t_dto.GradeTypeCode).FirstOrDefaultAsync();

                if (exist_t == null)
                {
                    bExist = false;
                    exist_t = new GradeTypeWeight();
                }
                else
                {
                    bExist = true;
                }

                exist_t.SchoolId = t_dto.SchoolId;
                exist_t.SectionId = t_dto.SectionId;
                exist_t.GradeTypeCode = t_dto.GradeTypeCode;
                exist_t.NumberPerSection = t_dto.NumberPerSection;
                exist_t.PercentOfFinalGrade = t_dto.PercentOfFinalGrade;
                exist_t.DropLowest = t_dto.DropLowest;

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
                return Ok(t_dto.SchoolId + ", " + t_dto.SectionId + ", " + t_dto.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeTypeWeight t_dto)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist_t = await _context.GradeTypeWeights.Where(x => x.SchoolId == t_dto.SchoolId && x.SectionId == t_dto.SectionId && x.GradeTypeCode == t_dto.GradeTypeCode).FirstOrDefaultAsync();

                if (exist_t != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record exists, cannot insert");
                }
                exist_t = new GradeTypeWeight();
                exist_t.SchoolId = t_dto.SchoolId;
                exist_t.SectionId = t_dto.SectionId;
                exist_t.GradeTypeCode = t_dto.GradeTypeCode;
                exist_t.NumberPerSection = t_dto.NumberPerSection;
                exist_t.PercentOfFinalGrade = t_dto.PercentOfFinalGrade;
                exist_t.DropLowest = t_dto.DropLowest;
                _context.Add(exist_t);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(t_dto.SchoolId + ", " + t_dto.SectionId + ", " + t_dto.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}