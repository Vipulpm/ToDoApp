using System.Text.Json.Serialization;

namespace ToDoApp.DTOs
{
    public class ToDoItemDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
