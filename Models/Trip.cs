using System;
using System.Collections.Generic;

namespace Zadanie5.Models;

public partial class Trip
{
    public int Idtrip { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateOnly Datefrom { get; set; }

    public DateOnly Dateto { get; set; }

    public int Maxpeople { get; set; }

    public virtual ICollection<ClientTrip> ClientTrips { get; set; } = new List<ClientTrip>();

    public virtual ICollection<Country> Idcountries { get; set; } = new List<Country>();
}
