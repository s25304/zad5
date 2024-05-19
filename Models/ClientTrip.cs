using System;
using System.Collections.Generic;

namespace Zadanie5.Models;

public partial class ClientTrip
{
    public int Idclient { get; set; }

    public int Idtrip { get; set; }

    public DateOnly Registeredat { get; set; }

    public DateOnly? Paymentdate { get; set; }

    public virtual Client IdclientNavigation { get; set; } = null!;

    public virtual Trip IdtripNavigation { get; set; } = null!;
}
