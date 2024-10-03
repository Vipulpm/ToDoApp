using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ToDoMobile.Model;

namespace ToDoMobile.Services
{
    public class ToDoService
    {
        private readonly HttpClient _httpClient;

        public ToDoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ToDoItem>> GetToDoItems()
        {
            var response = await _httpClient.GetAsync("todo");
            response.EnsureSuccessStatusCode();
            var items = await response.Content.ReadFromJsonAsync<List<ToDoItem>>();
            return items;
        }

        public async Task<ToDoItem> AddToDoItem(ToDoItem item)
        {
            var response = await _httpClient.PostAsJsonAsync("todo", item);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ToDoItem>();
        }
        public async Task UpdateToDoItem(int id, ToDoItem updatedItem)
        {
            var json = JsonSerializer.Serialize(updatedItem);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"todo/{id}", content);

            response.EnsureSuccessStatusCode(); // Throw an exception if the status code is not successful
        }
        public async Task DeleteToDoItem(int id)
        {
            var response = await _httpClient.DeleteAsync($"todo/{id}");
            response.EnsureSuccessStatusCode(); // Throw an exception if the status code is not successful
        }
    }
}
