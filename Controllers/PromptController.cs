using OllamaTot.Dtos;
using OllamaTot.Models;
using OllamaTot.Utilities;
using Microsoft.AspNetCore.Mvc;
// using System.Net.Http;
using Microsoft.EntityFrameworkCore;

namespace OllamaTot.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PromptController : ControllerBase
  {
    private readonly ILogger<Tot> _logger;
    private readonly OllamaTotContext _context;

    public PromptController(ILogger<Tot> logger, OllamaTotContext context)
    {
      _logger = logger;
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<PromptResponseDto>>> GetAll()
    {
      var prompts = await _context.Prompts
        .Select(p => new PromptResponseDto
          {
            Id = p.Id,
            Content = p.Content
          })
        .ToListAsync();

      return Ok(prompts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PromptResponseDto>> GetOne(int id)
    {
      var prompt = await _context.Prompts
        .Where(p => p.Id == id)
        .Select(p => new PromptResponseDto
            {
              Id = p.Id,
              Content = p.Content
            })
        .FirstOrDefaultAsync();

      if (prompt == null)
      {
        return NotFound();
      }
      return Ok(prompt);
    }

    [HttpPost]
    public async Task<ActionResult<Prompt>> Post([FromBody] string content)
    {
      var prompt = new Prompt
      {
        Content = content
      };
      _context.Prompts.Add(prompt);
      await _context.SaveChangesAsync();

      var responseDto = new PromptResponseDto
      {
        Id = prompt.Id,
        Content = prompt.Content,
      };
      return CreatedAtAction(nameof(Post), new { id = prompt.Id }, responseDto);
    }
  }
}


