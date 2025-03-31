using Kraft_Back_CS.Services.DevBoard;
using Kraft_Back_CS.Services.DevBoard.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kraft_Back_CS.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DevBoardController : ControllerBase
    {
        private readonly IDevBoardService DevBoardService;

        public DevBoardController(IDevBoardService DevBoardService)
        {
            this.DevBoardService = DevBoardService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ReadExcelToJson(String Path)
        {
            Path = Path.Contains('"') ? Path.Remove('"') : Path;
            return Ok(this.DevBoardService.ConvertExcelToJSON(Path));
        }
    }
}