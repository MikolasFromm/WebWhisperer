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

        [HttpGet]
        [Route("getCurrent")]
        public ActionResult GetCurrentTable()
        {
            // Call your service to get the current table data
            string csvData = _whisperService.GetCurrentTable(); // You need to implement this method in your service

            if (string.IsNullOrEmpty(csvData))
            {
                return NotFound(); // Return an appropriate response if the data is not found
            }

            // Assuming you want to return the CSV data as plain text
            return Content(csvData, "text/plain");
        }
    }
}