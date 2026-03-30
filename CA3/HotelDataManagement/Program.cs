using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CA3_S00274698_ThiagoBraz;

namespace HotelDataManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create the database context
            using (HotelDB db = new HotelDB())
            {
                try
                {
                    // Add initial rooms to the DB
                    if (!db.Rooms.Any())
                    {
                        db.Rooms.AddRange(new[]
                        {
                        // Single Rooms
                        new Room { RoomNumber = "101", Category = "Single", PricePerNight = 120.00m, Description = "Single room with city view" },
                        new Room { RoomNumber = "301", Category = "Single", PricePerNight = 130.00m, Description = "Single room, top floor" },

                        // Double Rooms
                        new Room { RoomNumber = "102", Category = "Double", PricePerNight = 180.00m, Description = "Double room, balcony, garden view" },
                        new Room { RoomNumber = "202", Category = "Double", PricePerNight = 185.00m, Description = "Double room, pool view" },

                        // Twin Rooms
                        new Room { RoomNumber = "201", Category = "Twin", PricePerNight = 170.00m, Description = "Twin room, quiet floor" },
                        new Room { RoomNumber = "302", Category = "Twin", PricePerNight = 175.00m, Description = "Twin room, city view" },

                        // Suite Rooms
                        new Room { RoomNumber = "401", Category = "Suite", PricePerNight = 300.00m, Description = "Luxury suite with jacuzzi" },
                        new Room { RoomNumber = "402", Category = "Suite", PricePerNight = 320.00m, Description = "Presidential suite with panoramic view" }
                                            });
                        db.SaveChanges();
                        Console.WriteLine("Added rooms successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Rooms already exist in the database.");
                    }

                    //method to add a booking with validation
                    void AddBooking(string roomNumber, string guestName, string guestEmail, DateTime checkIn, DateTime checkOut)
                    {
                        // Check if the room exists
                        var room = db.Rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
                        if (room == null)
                        {
                            Console.WriteLine($"Room {roomNumber} not found!");
                            return;
                        }

                        // Validate booking dates
                        if (checkIn >= checkOut)
                        {
                            throw new Exception("Check-in date must be before check-out date");
                        }

                        // Add the booking
                        db.Bookings.Add(new Booking
                        {
                            RoomId = room.RoomId,
                            GuestName = guestName,
                            GuestEmail = guestEmail,
                            CheckInDate = checkIn,
                            CheckOutDate = checkOut
                        });
                        Console.WriteLine($"Booking for room {roomNumber} added successfully.");
                    }

                    // Add some bookings to the DB
                    if (!db.Bookings.Any())
                    {
                        AddBooking("101", "Alice Johnson", "alice.johnson@email.com", new DateTime(2025, 5, 23), new DateTime(2025, 5, 25));
                        AddBooking("102", "Bob Smith", "bob.smith@email.com", new DateTime(2025, 5, 27), new DateTime(2025, 5, 31));
                        AddBooking("201", "Carla Dias", "carla.dias@email.com", new DateTime(2025, 5, 24), new DateTime(2025, 6, 1));
                        AddBooking("202", "David Lee", "david.lee@email.com", new DateTime(2025, 6, 2), new DateTime(2025, 6, 6));
                        AddBooking("301", "Elena Costa", "elena.costa@email.com", new DateTime(2025, 5, 29), new DateTime(2025, 6, 3));

                        db.SaveChanges();
                        Console.WriteLine("Added bookings successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Bookings already exist in the database.");
                    }

                    Console.WriteLine("Saved to Database");
                }
                catch (Exception ex)
                {
                    // Print any errors to the console
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
