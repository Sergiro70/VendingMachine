using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Vending.Model.Logic;

namespace Vending.Client.Main.ViewModel
{
    public class ProductVM : BindableBase
    {
        public ProductVM(ProductStack productStack, PurchaseManager manager = null)
        {
            ProductStack = productStack;
            productStack.PropertyChanged += (s, a) => { RaisePropertyChanged(nameof(Amount)); };

            if (manager != null)
            {
                BuyCommand = new DelegateCommand(() => { manager.BuyProduct(ProductStack.Product); });
            }
        }

        public int Amount => ProductStack.Amount;
        public DelegateCommand BuyCommand { get; }

        public Visibility IsBuyVisible => BuyCommand == null ? Visibility.Collapsed : Visibility.Visible;
        public string Name => ProductStack.Product.Name;
        public string Price => $"({ProductStack.Product.Price} руб.)";
        public ProductStack ProductStack { get; }
    }
}