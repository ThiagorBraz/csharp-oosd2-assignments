using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA3_S00274698_ThiagoBraz
{
    public class Booking
    {
        public int BookingId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string GuestName { get; set; }
        public string GuestEmail { get; set; }


        //Changes to allow for EntityFramework
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }
    }
}
