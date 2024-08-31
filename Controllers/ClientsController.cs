using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Dtos;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ClientsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllClients([FromQuery] int userId, [FromQuery] int page = 1, [FromQuery] int rowsPerPage = 10)
    {
        var now = DateTime.UtcNow;

        int skip = (page - 1) * rowsPerPage;

        var clients = await _context.Clients
            .Where(c => c.UserId == userId)
            .Include(c => c.Billings)
            .Select(c => new
            {
                id = c.Id,
                name = c.Name,
                document = c.Document,
                phone = c.Phone,
                address = c.Address,
                billings = c.Billings.Count(),
                lateBillingsCount = c.Billings.Count(b => !b.isPayed && b.dueDate < now),
                unpaidBillingsCount = c.Billings.Count(b => !b.isPayed),
                paidBillingsCount = c.Billings.Count(b => b.isPayed)
            })
            .Skip(skip)
            .Take(rowsPerPage)
            .ToListAsync();

        return Ok(clients);
    }


    [HttpGet("{clientId}/billings")]
    public async Task<IActionResult> GetClientBillings(int clientId, [FromQuery] int page = 1, [FromQuery] int rowsPerPage = 10)
    {
        var client = await _context.Clients
            .Include(c => c.Billings)
            .FirstOrDefaultAsync(c => c.Id == clientId);

        if (client == null)
            return NotFound();

        int skip = (page - 1) * rowsPerPage;
        
        var totalBillings = client.Billings.Count;
        var billings = client.Billings
            .Skip(skip)
            .Take(rowsPerPage)
            .Select(b => new 
            {
                b.Id,
                b.Description,
                b.Value,
                b.dueDate,
                b.isPayed,
                b.ClientId
            })
            .ToList();

        return Ok(billings);
    }


    [HttpPost]
    public async Task<IActionResult> AddClient([FromBody] ClientCreateDto clientDto)
    {
        if (clientDto == null)
        {
            return BadRequest(new { message = "Client data is required." });
        }

        var client = new Client
        {
            Name = clientDto.Name,
            Document = clientDto.Document,
            Phone = clientDto.Phone,
            Address = clientDto.Address,
            UserId = clientDto.UserId
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        
        return Ok(client);
    }

    [HttpPost("{clientId}/billings")]
    public async Task<IActionResult> AddBilling(int clientId, [FromBody] BillingCreateDto billingDto)
    {
        var client = await _context.Clients.FindAsync(clientId);

        if (client == null) return NotFound();

        if (billingDto == null)
        {
            return BadRequest(new { message = "Client data is required." });
        }

        var billing = new Billing
        {
            Description = billingDto.Description,
            Value = billingDto.Value,
            dueDate = billingDto.dueDate,
            isPayed = billingDto.isPayed,
            ClientId = clientId
        };

        _context.Billings.Add(billing);
        await _context.SaveChangesAsync();
        return Ok(billing);
    }



    [HttpPut("{clientId}")]
    public async Task<IActionResult> EditClient(int clientId, [FromBody] ClientCreateDto updatedClient)
    {
        if (updatedClient == null)
        {
            return BadRequest(new { message = "Client data is required." });
        }

        var client = await _context.Clients.FindAsync(clientId);

        if (client == null) return NotFound();

        client.Name = updatedClient.Name;
        client.Document = updatedClient.Document;
        client.Phone = updatedClient.Phone;
        client.Address = updatedClient.Address;

        await _context.SaveChangesAsync();
        return Ok(client);
    }

    [HttpDelete("{clientId}")]
    public async Task<IActionResult> DeleteClient(int clientId)
    {
        var client = await _context.Clients.FindAsync(clientId);

        if (client == null) return NotFound();

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{clientId}/billings/{billingId}")]
    public async Task<IActionResult> EditBilling(int clientId, int billingId, [FromBody] BillingCreateDto updatedBilling)
    {
        var billing = await _context.Billings
            .Where(b => b.ClientId == clientId && b.Id == billingId)
            .FirstOrDefaultAsync();

        if (billing == null) return NotFound();

        billing.Description = updatedBilling.Description;
        billing.Value = updatedBilling.Value;
        billing.dueDate = updatedBilling.dueDate;
        billing.isPayed = updatedBilling.isPayed;

        await _context.SaveChangesAsync();
        return Ok(billing);
    }

    [HttpDelete("{clientId}/billings/{billingId}")]
    public async Task<IActionResult> DeleteBilling(int clientId, int billingId)
    {
        var billing = await _context.Billings
            .Where(b => b.ClientId == clientId && b.Id == billingId)
            .FirstOrDefaultAsync();

        if (billing == null) return NotFound();

        _context.Billings.Remove(billing);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
