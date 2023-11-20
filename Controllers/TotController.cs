// using OllamaToT.Dtos;
using OllamaTot.Utilities;
using Microsoft.AspNetCore.Mvc;
// using System.Net.Http;
// using Microsoft.EntityFrameworkCore;

namespace OllamaTot.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ToTController : ControllerBase
  {
    private readonly Tot _tot;

    public ToTController(Tot tot)
    {
      _tot = tot;
    }

    [HttpGet]
    public async Task<ActionResult<string>> Get()
    {
      var prompt = FileHelper.ReadPromptFile("Example.txt");
      var result = await _tot.Run(prompt);
      return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<string>> Post([FromBody] string value)
    {
      var prompt = FileHelper.ReadPromptFile("Example.txt");
      var result = await _tot.Run(prompt);
      return Ok(result);
    }
  }
}

