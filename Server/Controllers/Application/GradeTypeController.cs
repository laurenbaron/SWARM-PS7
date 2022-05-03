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
    public class GradeTypeController : BaseController<GradeType>, iBaseController<GradeType>
    {
        public GradeTypeController(SWARMOracleContext context,
            IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<GradeType> lst_t = await _context.GradeTypes.OrderBy(x => x.SchoolId).ThenBy(x => x.GradeTypeCode).ToListAsync();
            return Ok(lst_t);
        }

        [HttpGet]
        [Route("Get/{t_no}")]
        public async Task<IActionResult> Get(int t_no)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Insufficient number of parameters for composite key, cannot get");
        }

        [HttpGet]
        [Route("Get/{school_no}/{GTC_no}")]
        public async Task<IActionResult> Get(int school_no, string GTC_no)
        {
            GradeType itm_t = await _context.GradeTypes.Where(x => x.SchoolId == school_no && x.GradeTypeCode == GTC_no).FirstOrDefaultAsync();
            return Ok(itm_t);
        }

        [HttpDelete]
        [Route("Delete/{t_no}")]
        public async Task<IActionResult> Delete(int t_no)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Insufficient number of parameters for composite key, cannot delete");
        }

        [HttpDelete]
        [Route("Delete/{school_no}/{GTC_no}")]
        public async Task<IActionResult> Delete(int school_no, string GTC_no)
        {
            GradeType itm_t = await _context.GradeTypes.Where(x => x.SchoolId == school_no && x.GradeTypeCode == GTC_no).FirstOrDefaultAsync();
            _context.Remove(itm_t);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeType t_dto)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist_t = await _context.GradeTypes.Where(x => x.SchoolId == t_dto.SchoolId && x.GradeTypeCode == t_dto.GradeTypeCode).FirstOrDefaultAsync();

                if (exist_t == null)
                {
                    bExist = false;
                    exist_t = new GradeType();
                }
                else
                {
                    bExist = true;
                }

                exist_t.SchoolId = t_dto.SchoolId;
                exist_t.GradeTypeCode = t_dto.GradeTypeCode;
                exist_t.Description = t_dto.Description;

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
                return Ok(t_dto.SchoolId + ", " + t_dto.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeType t_dto)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist_t = await _context.GradeTypes.Where(x => x.SchoolId == t_dto.SchoolId && x.GradeTypeCode == t_dto.GradeTypeCode).FirstOrDefaultAsync();

                if (exist_t != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record exists, cannot insert");
                }
                exist_t = new GradeType();
                exist_t.SchoolId = t_dto.SchoolId;
                exist_t.GradeTypeCode = t_dto.GradeTypeCode;
                exist_t.Description = t_dto.Description;
                _context.Add(exist_t);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(t_dto.SchoolId + ", " + t_dto.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}