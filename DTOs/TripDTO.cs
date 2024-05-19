using Zadanie5.Models;

namespace Zadanie5.DTOs;

public class TripDTO
{
    public int Idtrip { get; set; }

    public string Name { get; set; } 

    public string Description { get; set; } 

    public DateOnly Datefrom { get; set; }

    public DateOnly Dateto { get; set; }

    public int Maxpeople { get; set; }
    
    public IEnumerable<ClientDTO> Clients { get; set; }
    public IEnumerable<CountryDTO> Countries { get; set; }
}

public class ClientDTO
{

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;


}

public class CountryDTO
{
    public string Name { get; set; } = null!;
}

public class ClientTripDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Telephone { get; set; }
    public string Pesel { get; set; }
    public int IdTrip { get; set; }
    public string TripName { get; set; }
    public DateTime PaymentDate { get; set; }

    
} 