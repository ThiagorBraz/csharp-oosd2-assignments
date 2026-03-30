using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA3_S00274698_ThiagoBraz
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string Category { get; set; }
        public decimal PricePerNight { get; set; }
        public string Description { get; set; }

        //Changes to allow for EntityFramework
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}

