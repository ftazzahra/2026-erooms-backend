namespace erooms.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Role { get; set; }
    }
}