namespace erooms.DTOs
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }   // ← TAMBAHKAN INI
        public string RoomName { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}