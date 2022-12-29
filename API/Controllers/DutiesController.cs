using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers;

public class DutiesController : BaseApiController
{
    private readonly DataContext _context;

    public DutiesController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Duty>>> GetDuties()
    {
        return await _context.Duties.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Duty>> GetDuty(Guid id)
    {
        return await _context.Duties.FindAsync(id);
    }
}