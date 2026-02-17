using System;

namespace erooms.DTOs
{
    public class BookingRequestDto
    {
        public int RoomId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        // add field purpose
        public string Purpose { get; set; } = string.Empty;
    }
}
