using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace CA3_S00274698_ThiagoBraz
{
    //Data that is going to create the basis of the database
    public class HotelDB : DbContext
    {
        public HotelDB() : base("BrazHotelData") { }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }

    }
}
