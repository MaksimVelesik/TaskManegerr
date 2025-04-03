using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskManager;

namespace TaskManegerr
{
    public class TaskManagerViewModel : ViewModelBase 
    {
        private readonly TaskService _taskService;

        public ObservableCollection<TaskModel> Tasks { get; set; }
        public ObservableCollection<TaskCategoryModel> Categories { get; set; }

        public ICommand AddTaskCommand { get; }
        public ICommand RemoveTaskCommand { get; }

        public TaskManagerViewModel()
        {
            _taskService = new TaskService();
            Tasks = new ObservableCollection<TaskModel>();
            Categories = new ObservableCollection<TaskCategoryModel>();

            AddTaskCommand = new RelayCommand(AddTask);
            RemoveTaskCommand = new RelayCommand<TaskModel>(RemoveTask);

            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            await Task.Delay(2000); 

            var tasks = await _taskService.GetTasksAsync();
            var categories = await _taskService.GetCategoriesAsync();

            foreach (var task in tasks)
                Tasks.Add(task);

            foreach (var category in categories)
                Categories.Add(category);
        }

        private void AddTask()
        {
            var newTask = new TaskModel { Title = "Новая задача" };
            _taskService.AddTask(newTask);
            Tasks.Add(newTask);
        }

        private void RemoveTask(TaskModel task)
        {
            if (task != null)
            {
                _taskService.RemoveTask(task);
                Tasks.Remove(task);
            }
        }
    }
}
