using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Messaging;
using RedmineAutoLogTime.Messages;

namespace RedmineAutoLogTime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                var message = new TextChangedMessage("IssueId", textBox.Text);
                WeakReferenceMessenger.Default.Send(message);
            }
        }
    }
}