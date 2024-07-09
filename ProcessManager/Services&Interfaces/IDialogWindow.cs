namespace ProcessManager.Services_Interfaces;

public interface IDialogWindow
{
    public bool? DialogResult { get; set; }
    public Object DataContext { get; set; }
    public bool? ShowDialog();
}
