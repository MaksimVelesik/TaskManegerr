using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace TaskManager
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<MyTask> AllTasks { get; } = new ObservableCollection<MyTask>();

        public ICommand AddTaskCommand { get; }
        public ICommand EditTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand ExitCommand { get; }

        public MainWindow()
        {
            InitializeComponent();
            InitializeTasks();

            AddTaskCommand = new RelayCommand(AddTask);
            EditTaskCommand = new RelayCommand(EditTask, CanEditOrDelete);
            DeleteTaskCommand = new RelayCommand(DeleteTask, CanEditOrDelete);
            ExitCommand = new RelayCommand(ExitApplication);

            DataContext = this;

            taskListView.SelectionChanged += (s, e) => UpdateCommandStates();
        }

        private void InitializeTasks()
        {
            AllTasks.Add(new MyTask { Title = "Задача 1", Description = "Описание 1", Priority = "Высокий", Status = "В работе" });
            AllTasks.Add(new MyTask { Title = "Задача 2", Description = "Описание 2", Priority = "Низкий", Status = "Выполнено" });
            taskListView.ItemsSource = AllTasks;
        }

        private void AddTask()
        {
            TaskDialog taskDialog = new TaskDialog();
            if (taskDialog.ShowDialog() == true)
            {
                AllTasks.Add(taskDialog.NewTask);
                UpdateCommandStates();
            }
        }

        private void EditTask()
        {
            if (taskListView.SelectedItem is MyTask selectedTask)
            {
                TaskDialog taskDialog = new TaskDialog(selectedTask);
                if (taskDialog.ShowDialog() == true)
                {
                    var index = AllTasks.IndexOf(selectedTask);
                    AllTasks[index] = taskDialog.NewTask;
                    UpdateCommandStates();
                }
            }
        }

        private void DeleteTask()
        {
            if (taskListView.SelectedItem is MyTask selectedTask)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить эту задачу?", "Подтверждение удаления", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    AllTasks.Remove(selectedTask);
                    UpdateCommandStates();
                }
            }
        }

        private bool CanEditOrDelete()
        {
            return taskListView.SelectedItem != null;
        }

        private void UpdateCommandStates()
        {
            ((RelayCommand)EditTaskCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DeleteTaskCommand).RaiseCanExecuteChanged();
        }

        private void FilterTasks(string status)
        {
            var filteredTasks = new ObservableCollection<MyTask>(AllTasks.Where(t => t.Status == status));
            taskListView.ItemsSource = filteredTasks;
        }

        private void FilterAll_Click(object sender, RoutedEventArgs e)
        {
            taskListView.ItemsSource = AllTasks;
        }

        private void FilterInProgress_Click(object sender, RoutedEventArgs e)
        {
            FilterTasks("В работе");
        }

        private void FilterCompleted_Click(object sender, RoutedEventArgs e)
        {
            FilterTasks("Выполнено");
        }

        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        public void Execute(object parameter) => _execute();

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}