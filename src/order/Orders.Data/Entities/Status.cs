using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Data.Entities
{
    public enum Status
    {
        LoadPayment,
        LoadAccept,
        LoadPacking,
        LoadShip,
        Delivered,
        Cancelled,
        Refunded
    }
}
