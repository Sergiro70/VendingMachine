using System.Windows;
using Vending.Client.Main.ViewModel;

namespace Vending.Client.Main.View
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewVM();
        }
    }
}