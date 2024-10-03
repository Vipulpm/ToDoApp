using ToDoMobile.Model;
using ToDoMobile.Services;

namespace ToDoMobile
{
    public partial class MainPage : ContentPage
    {
        private readonly ToDoService _toDoService;
        public MainPage()
        {
            InitializeComponent();
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7212/api/")
            };
            _toDoService = new ToDoService(httpClient);

            LoadToDoItems();
        }

        private async void LoadToDoItems()
        {
            var items = await _toDoService.GetToDoItems();
            ToDoList.ItemsSource = items;
        }

        private async void OnAddToDoClicked(object sender, EventArgs e)
        {
            var newItem = new ToDoItem
            {
                Title = TitleEntry.Text,
                Description = DescriptionEntry.Text,
                IsCompleted = false
            };

            await _toDoService.AddToDoItem(newItem);
            TitleEntry.Text = string.Empty;  // Clear the input after adding
            DescriptionEntry.Text = string.Empty;
            LoadToDoItems(); // Refresh the list
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tappedItem = e.Item as ToDoItem;
            if (tappedItem != null)
            {
                tappedItem.IsCompleted = !tappedItem.IsCompleted; // Toggle completion status

                await _toDoService.UpdateToDoItem(tappedItem.Id, tappedItem); // Update the task in API
                LoadToDoItems(); // Refresh the list
            }
        }
        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            // Get the ID of the to-do item to delete
            var button = sender as Button;
            var id = (int)button.CommandParameter;

            // Confirm deletion with the user (optional)
            bool confirm = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this item?", "Yes", "No");
            if (confirm)
            {
                await _toDoService.DeleteToDoItem(id); // Call the delete method in the service
                LoadToDoItems(); // Refresh the list
            }
        }
        private async void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            // Get the CheckBox that triggered the event
            var checkBox = sender as CheckBox;

            // Get the ToDoItem from the CheckBox's BindingContext
            var item = checkBox.BindingContext as ToDoItem;

            if (item != null)
            {
                // Update the IsCompleted property based on the CheckBox's state
                item.IsCompleted = e.Value;

                // Update the item in the service
                await _toDoService.UpdateToDoItem(item.Id, item);

                // Refresh the list to reflect the changes
                LoadToDoItems();
            }
        }
    }

}
