using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;

namespace CA3_S00274698_ThiagoBraz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HotelDB _context = new HotelDB();

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            LoadCategories();
        }

        private void LoadData()
        {
            try
            {

                // Load rooms into context and bind to grid
                _context.Rooms.Include("Bookings").Load();
                var roomsWithBookingCount = _context.Rooms.Local
    .Select(r => new
    {
        r.RoomNumber,
        r.Category,
        r.PricePerNight,
        r.Description,
        Bookings = r.Bookings.Count
    })
    .ToList();
                AllRoomsGrid.ItemsSource = roomsWithBookingCount;

                // Load bookings (with related rooms) into context and bind to grid
                _context.Bookings.Include("Room").Load();
                BookingsGrid.ItemsSource = _context.Bookings.Local;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }
        //method to Refresh the all rooms grid after adding or deleting a booking
        private void RefreshAllRoomsGrid()
        {
            _context.Rooms.Include("Bookings").Load();

            var roomsWithBookingCount = _context.Rooms.Local
                .Select(r => new
                {
                    r.RoomNumber,
                    r.Category,
                    r.PricePerNight,
                    r.Description,
                    Bookings = r.Bookings.Count
                })
                .ToList();

            AllRoomsGrid.ItemsSource = null;
            AllRoomsGrid.ItemsSource = roomsWithBookingCount;
        }

        private void LoadCategories()
        {
            try
            {
                // Make sure rooms are loaded
                if (!_context.Rooms.Local.Any())
                    _context.Rooms.Load();

                // Get distinct categories and add to ComboBox
                var categories = _context.Rooms.Local
                    .Select(r => r.Category)
                    .Distinct()
                    .ToList();

                // Add "All Categories" option
                categories.Insert(0, "All Categories");

                // Bind to ComboBox
                CategoryComboBox.ItemsSource = categories;
                CategoryComboBox.SelectedIndex = 0; 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}");
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get selected category from ComboBox
                string selectedCategory = CategoryComboBox.SelectedItem as string;
                bool allCategories = selectedCategory == "All Categories" || string.IsNullOrEmpty(selectedCategory);

                // Get selected dates from DatePickers
                DateTime? checkIn = CheckInDate.SelectedDate;
                DateTime? checkOut = CheckOutDate.SelectedDate;

                if (checkIn == null || checkOut == null)
                {
                    MessageBox.Show("Please select both check-in and check-out dates.");
                    return;
                }

                if (checkIn >= checkOut)
                {
                    MessageBox.Show("Check-in date must be before check-out date.");
                    return;
                }

                // Query available rooms for the selected date range and category
                var availableRooms = _context.Rooms
                    .Where(r => (allCategories || r.Category == selectedCategory) &&
                                !r.Bookings.Any(b =>
                                    (checkIn < b.CheckOutDate) &&
                                    (checkOut > b.CheckInDate)))
                    .ToList();

                RoomsList.ItemsSource = availableRooms;

                if (!availableRooms.Any())
                {
                    MessageBox.Show("No available rooms found for the selected dates and category.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching rooms: {ex.Message}");
            }
        }

        private void RoomsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRoom = RoomsList.SelectedItem as Room;
            if (selectedRoom != null)

            // Display the details of the selected room
            {
                RoomDetails.Text = $"Room Number: {selectedRoom.RoomNumber}\n" +
                                   $"Category: {selectedRoom.Category}\n" +
                                   $"Price per Night: {selectedRoom.PricePerNight:C}\n" +
                                   $"Description: {selectedRoom.Description}";

                // Select the appropriate image based on the room category
                string imageName = null;
                string category = selectedRoom.Category.ToLower();

                if (category == "single")
                    imageName = "single.png";
                else if (category == "double")
                    imageName = "double.png";
                else if (category == "twin")
                    imageName = "twin.png";
                else if (category == "suite")
                    imageName = "suite.png";

                if (imageName != null)
                {
                    RoomTypeImage.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/{imageName}"));
                }
                else
                {
                    RoomTypeImage.Source = null;
                }
            }
            
            else
            {
                // Clear the details and image if no room is selected
                RoomDetails.Text = string.Empty;
                RoomTypeImage.Source = null;
            }
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get selected room
                var selectedRoom = RoomsList.SelectedItem as Room;
                if (selectedRoom == null)
                {
                    MessageBox.Show("Please select a room to book.");
                    return;
                }

                // Get selected dates
                DateTime? checkIn = CheckInDate.SelectedDate;
                DateTime? checkOut = CheckOutDate.SelectedDate;

                if (checkIn == null || checkOut == null)
                {
                    MessageBox.Show("Please select both check-in and check-out dates.");
                    return;
                }

                if (checkIn >= checkOut)
                {
                    MessageBox.Show("Check-in date must be before check-out date.");
                    return;
                }

                // Get guest details
                string guestName = GuestNameTextBox.Text;
                string guestEmail = GuestEmailTextBox.Text;

                if (string.IsNullOrWhiteSpace(guestName) || string.IsNullOrWhiteSpace(guestEmail))
                {
                    MessageBox.Show("Please enter guest name and email.");
                    return;
                }

                // Create new booking
                var newBooking = new Booking
                {
                    RoomId = selectedRoom.RoomId,
                    CheckInDate = checkIn.Value,
                    CheckOutDate = checkOut.Value,
                    GuestName = guestName,
                    GuestEmail = guestEmail
                };

                _context.Bookings.Add(newBooking);
                _context.SaveChanges();

                RefreshAllRoomsGrid();

                // Build the booking details string
                string bookingDetails = $"Booking confirmed!\n\n" +
                                       $"Room Number: {selectedRoom.RoomNumber}\n" +
                                       $"Category: {selectedRoom.Category}\n" +
                                       $"Price per Night: {selectedRoom.PricePerNight:C}\n" +
                                       $"Description: {selectedRoom.Description}\n\n" +
                                       $"Check-in: {checkIn.Value:dd/MM/yyyy}\n" +
                                       $"Check-out: {checkOut.Value:dd/MM/yyyy}\n" +
                                       $"Guest Name: {guestName}\n" +
                                       $"Guest Email: {guestEmail}";

                // Show message box with details
                MessageBox.Show(bookingDetails, "Booking Confirmed", MessageBoxButton.OK, MessageBoxImage.Information);

                // Refresh bookings grid - better approach than recreating list each time
                _context.Bookings.Include("Room").Load(); // Refresh data

                // Clear form
                CategoryComboBox.SelectedIndex = 0;
                CheckInDate.SelectedDate = null;
                CheckOutDate.SelectedDate = null;
                RoomsList.ItemsSource = null;
                RoomsList.SelectedItem = null;
                RoomDetails.Text = string.Empty;
                GuestNameTextBox.Text = string.Empty;
                GuestEmailTextBox.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error booking room: {ex.Message}");
            }
        }

        //click event for the delete button
        private void DeleteBooking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get selected booking from the grid
                var booking = (sender as FrameworkElement)?.DataContext as Booking;
                if (booking != null)
                {
                    if (MessageBox.Show("Are you sure you want to delete this booking?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        _context.Bookings.Remove(booking);
                        _context.SaveChanges();

                        RefreshAllRoomsGrid();

                        // Refresh data correctly
                        _context.Bookings.Include("Room").Load();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting booking: {ex.Message}");
            }
        }
    }
}
