using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Mvvm;
using Vending.Model.Data;

namespace Vending.Model.Logic
{
    public class User : BindableBase
    {
        private readonly ObservableCollection<ProductStack> _customPurchases = new ObservableCollection<ProductStack>();
        private readonly ObservableCollection<MoneyStack> _customWallet;

        public User()
        {
            //кошелек пользователя
            _customWallet = new ObservableCollection<MoneyStack>
                (Banknote.Banknotes.Select(b => new MoneyStack(b, 10)));
            UserWallet = new ReadOnlyObservableCollection<MoneyStack>(_customWallet);

            //продукты пользователя
            CustomPurchases = new ReadOnlyObservableCollection<ProductStack>(_customPurchases);
        }

        public int CheckAmount
        {
            get { return _customWallet.Select(b => b.Banknote.Nominal * b.Amount).Sum(); }
        }

        public ReadOnlyObservableCollection<ProductStack> CustomPurchases { get; }

        public ReadOnlyObservableCollection<MoneyStack> UserWallet { get; }

        internal void AddProduct(Product product)
        {
            var stack = _customPurchases.FirstOrDefault(b => b.Product == product);
            if (stack == null)
            {
                _customPurchases.Add(new ProductStack(product, 1));
            }
            else
            {
                stack.PushOne();
            }
        }

        internal void AppendMoney(IEnumerable<MoneyStack> change)
        {
            foreach (var ms in change)
            {
                for (var i = 0; i < ms.Amount; ++i)
                {
                    UserWallet.First(m => Equals(m.Banknote.Nominal, ms.Banknote.Nominal)).PushOne();
                }
            }

            RaisePropertyChanged(nameof(CheckAmount));
        }

        internal bool GetBanknote(Banknote banknote)
        {
            if (_customWallet.FirstOrDefault(ms => ms.Banknote.Equals(banknote))?.PullOne() ?? false)
            {
                RaisePropertyChanged(nameof(CheckAmount));
                return true;
            }

            return false;
        }
    }
}