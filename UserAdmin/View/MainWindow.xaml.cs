using System.Windows;
using ViewModel;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainWinViewModel mainViewModel = new MainWinViewModel(Mediator.Instance);
            DataContext = mainViewModel;

        }

        private void NewBtn_Click(object sender, RoutedEventArgs e)
        {
            NewAndEditWindow newWindow = new NewAndEditWindow();
            newWindow.DataContext = new NewAndEditViewModel(Mediator.Instance);
            newWindow.ShowDialog();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWinViewModel viewModel = (MainWinViewModel)DataContext;
            NewAndEditWindow editWindow = new NewAndEditWindow();
            editWindow.DataContext = new NewAndEditViewModel(viewModel.CurrentUser.Clone(), Mediator.Instance);
            editWindow.ShowDialog();
        }
    }
}


