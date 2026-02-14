using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace erooms.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Location { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;

        [JsonIgnore] 
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        
    }
}
