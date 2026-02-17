using System;

namespace erooms.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;

        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public string Status { get; set; } = "Pending";

        // add field purpose
        public string Purpose { get; set; } = string.Empty;
    }
}
