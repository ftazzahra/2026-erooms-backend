namespace erooms.DTOs
{
    public class RoomRequestDto
    {
        //updated
        public required string Name { get; set; }
        public int Capacity { get; set; }
        public required string Location { get; set; }
        public bool IsAvailable { get; set; }
    }
}
