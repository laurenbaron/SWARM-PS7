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
    public class SectionController : BaseController<Section>, iBaseController<Section>
    {
        public SectionController(SWARMOracleContext context,
            IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<Section> lst_t = await _context.Sections.OrderBy(x => x.SectionId).ToListAsync();
            return Ok(lst_t);
        }

        [HttpGet]
        [Route("Get/{t_no}")]
        public async Task<IActionResult> Get(int t_no)
        {
            Section itm_t = await _context.Sections.Where(x => x.SectionId == t_no).FirstOrDefaultAsync();
            return Ok(itm_t);
        }

        [HttpDelete]
        [Route("Delete/{t_no}")]
        public async Task<IActionResult> Delete(int t_no)
        {
            Section itm_t = await _context.Sections.Where(x => x.SectionId == t_no).FirstOrDefaultAsync();
            _context.Remove(itm_t);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Section t_dto)
        {
            bool bExist = false;
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist_t = await _context.Sections.Where(x => x.SectionId == t_dto.SectionId).FirstOrDefaultAsync();

                if (exist_t == null)
                {
                    bExist = false;
                    exist_t = new Section();
                }
                else
                {
                    bExist = true;
                }

                exist_t.SectionId = t_dto.SectionId;
                exist_t.CourseNo = t_dto.CourseNo;
                exist_t.SectionNo = t_dto.SectionNo;
                exist_t.StartDateTime = t_dto.StartDateTime;
                exist_t.Location = t_dto.Location;
                exist_t.InstructorId = t_dto.InstructorId;
                exist_t.Capacity = t_dto.Capacity;
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
                return Ok(t_dto.SectionId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Section t_dto)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exist_t = await _context.Sections.Where(x => x.SectionId == t_dto.SectionId).FirstOrDefaultAsync();

                if (exist_t != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record exists, cannot insert");
                }

                exist_t = new Section();
                //   exist_t.SectionId = t_dto.SectionId;
                exist_t.CourseNo = t_dto.CourseNo;
                exist_t.SectionNo = t_dto.SectionNo;
                exist_t.StartDateTime = t_dto.StartDateTime;
                exist_t.Location = t_dto.Location;
                exist_t.InstructorId = t_dto.InstructorId;
                exist_t.Capacity = t_dto.Capacity;
                exist_t.SchoolId = t_dto.SchoolId;

                _context.Add(exist_t);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(t_dto.SectionId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}