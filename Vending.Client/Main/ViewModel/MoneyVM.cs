using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Vending.Model.Logic;

namespace Vending.Client.Main.ViewModel
{
    public class MoneyVM : BindableBase
    {
        public MoneyVM(MoneyStack moneyStack, PurchaseManager manager = null)
        {
            MoneyStack = moneyStack;
            moneyStack.PropertyChanged += (s, a) => { RaisePropertyChanged(nameof(Amount)); };

            if (manager != null)
            {
                InsertCommand = new DelegateCommand(() => { manager.InsertMoney(MoneyStack.Banknote); });
            }
        }

        public int Amount => MoneyStack.Amount;
        public string Icon => MoneyStack.Banknote.IsCoin ? "\\..\\Images\\coin.jpg" : "\\..\\Images\\banknote.png";
        public DelegateCommand InsertCommand { get; }

        public Visibility IsInsertVisible => InsertCommand == null ? Visibility.Collapsed : Visibility.Visible;
        public MoneyStack MoneyStack { get; }
        public string Name => MoneyStack.Banknote.Name;
    }
}