﻿using Microsoft.AspNetCore.Mvc;
using WebWhisperer.Services;

namespace WebWhisperer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }

    [Route("api/whisper")]
    [ApiController]
    public class WhisperController : ControllerBase
    {
        private readonly WhisperService _whisperService;

        public WhisperController(WhisperService whisperService)
        {
            _whisperService = whisperService;
        }

        [HttpPost]
        [Route("process")]
        public ActionResult<List<string>> ProcessInput([FromBody] string querySoFar)
        {
            List<string> whisperText = _whisperService.ProcessInput(querySoFar).ToList();
            return Ok(whisperText);
        }

        [HttpPost]
        [Route("upload")]
        public ActionResult LoadUserInput([FromBody] string userInput)
        {
            _whisperService.LoadUserInput(userInput);
            return Ok();
        }
    }
}