
using Microsoft.AspNetCore.Mvc;
using Web_Programming_Assignment_5.Entities;
using Web_Programming_Assignment_5.Services;

namespace Web_Programming_Assignment_5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarioController : Controller
    {
        private readonly IMarioService marioService;

        public MarioController(IMarioService marioService)
        {
            this.marioService = marioService;
        }

        [HttpGet("{move}")]
        public async Task<IActionResult> MoveAsync(string move)
        {
            if (IsValidAction(move))
            {
                try
                {
                    return Json(await marioService.MakeActionAsync(move));
                }
                catch (HttpRequestException ex)
                {
                    return Ok(new { Message = "Mario died" });
                }
            }
            else
            {
                return BadRequest("Invalid move");
            }
        }

        private bool IsValidAction(string move)
        {
            return move == "walk" || move == "jump" || move == "wait" || move == "run";
        }
    }

}