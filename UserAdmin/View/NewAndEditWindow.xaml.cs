using System.Windows;
using ViewModel;

namespace View
{
    /// <summary>
    /// Interaction logic for NewAndEditWindow.xaml
    /// </summary>
    public partial class NewAndEditWindow : Window
    {
        public NewAndEditWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NewAndEditViewModel viewModel = (NewAndEditViewModel)DataContext;
            viewModel.Done += ViewModel_Done;
        }

        private void ViewModel_Done(object sender, NewAndEditViewModel.DoneEventArgs e)
        {
            MessageBox.Show(e.Message);
        }
    }
}
