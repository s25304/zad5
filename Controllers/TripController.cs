using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zadanie5.Context;
using Zadanie5.DTOs;
using Zadanie5.Models;

namespace Zadanie5.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TripController: ControllerBase
{
   private readonly ApbdContext _context;

   public TripController(ApbdContext context)
   {
      _context = context;
   }
   
   [HttpGet("/trips")]
   public async Task<IActionResult> getAll()
   {
       Console.WriteLine("inside get");
       var trips = await _context.Trips
           .Include(t => t.ClientTrips)
           .ThenInclude(tc => tc.IdclientNavigation)
           .Select(t => new TripDTO
           {
               Idtrip= t.Idtrip,
               Name = t.Name,
               Description = t.Description,
               Datefrom = t.Datefrom,
               Dateto = t.Dateto,
               Maxpeople = t.Maxpeople,
               Clients = t.ClientTrips.Select(ct => new ClientDTO
               {
                   Firstname = ct.IdclientNavigation.Firstname,
                   Lastname = ct.IdclientNavigation.Lastname
               }),
               Countries = t.Idcountries.Select(c => new CountryDTO
               {
                   Name = c.Name
               })
           })
           .ToListAsync();
       
       return Ok(trips);
   }
    [HttpDelete("/clients/{idClient}")]
   public async Task<IActionResult> deleteClient(int idClient)
   {
       var toDelete = _context.Clients
           .Find(idClient);
       if (_context.ClientTrips.Where(e => e.Idclient == toDelete.Idclient).Any())
       {
           return BadRequest(new {msg = "client is assign with a trip"});
       }

       _context.Clients.Remove(toDelete);
       await _context.SaveChangesAsync();
       return NoContent();
   }

    [HttpPost("/trips/{idTrip}/clients")]
    public async Task<IActionResult> addClientToTrip(int idTrip, ClientTripDTO clientTrip )
    {
        
        if (_context.Clients.Where(c => c.Pesel == clientTrip.Pesel).Count() == 0)
        {
            var newClient = new Client
            {
                Firstname = clientTrip.FirstName,
                Lastname = clientTrip.LastName,
                Email = clientTrip.Email,
                Telephone = clientTrip.Telephone,
                Pesel = clientTrip.Pesel
            };
           await _context.Clients.AddAsync(newClient);
           await _context.SaveChangesAsync();
        }

        var client = _context.Clients
            .Where(c => c.Pesel == clientTrip.Pesel).First();

        if (_context.ClientTrips
                .Where(ct => ct.Idclient == client.Idclient 
                             && ct.Idtrip == clientTrip.IdTrip)
                .Count() != 0
           )
        {
            return BadRequest(new {message = "client alr3eady in a trip"});
        }

        if (!_context.Trips.Any(t => t.Idtrip == clientTrip.IdTrip))
        {
            return BadRequest(new {message = "there is no trip with given id"});
        }

        var trip = _context.Trips
            .Find(clientTrip.IdTrip);                     ;

        var clientTripEntity = new ClientTrip
        {
            Idclient = client.Idclient,
            Idtrip = clientTrip.IdTrip,
            //Registeredat = DateTime.Now()
        };
       await _context.AddAsync(clientTripEntity);
       await _context.SaveChangesAsync();
        
      return Created();
    }
}