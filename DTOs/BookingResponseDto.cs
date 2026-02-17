using System;

namespace erooms.DTOs
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;

        //get location and capacity data from room
        public string RoomLocation { get; set; } = string.Empty; 
        public int RoomCapacity { get; set; } 
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = string.Empty;

        //add field purpose
        public string Purpose { get; set; } = string.Empty;
    }
}
