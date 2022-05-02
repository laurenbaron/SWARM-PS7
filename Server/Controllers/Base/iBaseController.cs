using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SWARM.Server.Controllers.Base
{
    public interface iBaseController<T>
    { 
        Task<IActionResult> Get();
        Task<IActionResult> Get(int t_no);
        Task<IActionResult> Delete(int t_no);
        Task<IActionResult> Put([FromBody] T t_dto);
        Task<IActionResult> Post([FromBody] T t_dto);
    }
}
