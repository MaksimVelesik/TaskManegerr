using System.ComponentModel;

public class MyTask : INotifyPropertyChanged
{
    private string title;
    private string description;
    private string priority;
    private string status;

    public string Title
    {
        get => title;
        set
        {
            title = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    public string Description
    {
        get => description;
        set
        {
            description = value;
            OnPropertyChanged(nameof(Description));
        }
    }

    public string Priority
    {
        get => priority;
        set
        {
            priority = value;
            OnPropertyChanged(nameof(Priority));
        }
    }

    public string Status
    {
        get => status;
        set
        {
            status = value;
            OnPropertyChanged(nameof(Status));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}