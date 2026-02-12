public class RoomResponseDto
{
    //updated
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Location { get; set; }
    public int Capacity { get; set; }
    public bool IsAvailable { get; set; }
}
