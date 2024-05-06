using KaibaSystem_Back_End.Services.DevBoard;
using KaibaSystem_Back_End.Services.DevBoard.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KaibaSystem_Back_End.Controllers
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
            return Ok(this.DevBoardService.ConvertExcelToJSON(Path));
        }
    }
}