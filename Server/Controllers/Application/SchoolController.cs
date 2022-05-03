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
    public class SchoolController : BaseController<School>, iBaseController<School>
    {
        public SchoolController(SWARMOracleContext context,
            IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<School> lst_t = await _context.Schools.OrderBy(x => x.SchoolId).ToListAsync();
            return Ok(lst_t);
        }

        [HttpGet]
        [Route("Get/{t_no}")]
        public async Task<IActionResult> Get(int t_no)
        {
            School itm_t = await _context.Schools.Where(x => x.SchoolId == t_no).FirstOrDefaultAsync();
            return Ok(itm_t);
        }

        [HttpDelete]
        [Route("Delete/{t_no}")]
        public async Task<IActionResult> Delete(int t_no)
        {
            School itm_t = await _context.Schools.Where(x => x.SchoolId == t_no).FirstOrDefaultAsync();
            _context.Remove(itm_t);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] School t_dto)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist_t = await _context.Schools.Where(x => x.SchoolId == t_dto.SchoolId).FirstOrDefaultAsync();

                if (exist_t == null)
                {
                    bExist = false;
                    exist_t = new School();
                }
                else
                {
                    bExist = true;
                }

                exist_t.SchoolId = t_dto.SchoolId;
                exist_t.SchoolName = t_dto.SchoolName;

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
                return Ok(t_dto.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] School t_dto)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist_t = await _context.Schools.Where(x => x.SchoolId == t_dto.SchoolId).FirstOrDefaultAsync();

                if (exist_t != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record exists, cannot insert");
                }

                exist_t = new School();
                exist_t.SchoolId = t_dto.SchoolId;
                exist_t.SchoolName = t_dto.SchoolName;

                _context.Add(exist_t);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(t_dto.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}