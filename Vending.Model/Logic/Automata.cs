using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Mvvm;
using Vending.Model.Data;

namespace Vending.Model.Logic
{
    public class Automata : BindableBase
    {
        private readonly ObservableCollection<MoneyStack> _automataBank;

        private readonly ObservableCollection<ProductStack> _productsInAutomata;

        private int credit;

        public Automata()
        {
            _automataBank = new ObservableCollection<MoneyStack>
                (Banknote.Banknotes.Select(b => new MoneyStack(b, 100)));
            AutomataBank = new ReadOnlyObservableCollection<MoneyStack>(_automataBank);

            _productsInAutomata =
                new ObservableCollection<ProductStack>(Product.Products.Select(p => new ProductStack(p, 100)));
            ProductsInAutomata = new ReadOnlyObservableCollection<ProductStack>(_productsInAutomata);
        }

        public ReadOnlyObservableCollection<MoneyStack> AutomataBank { get; }

        public int Credit
        {
            get => credit;
            set => SetProperty(ref credit, value);
        }

        public ReadOnlyObservableCollection<ProductStack> ProductsInAutomata { get; }

        internal bool BuyProduct(Product product)
        {
            if (Credit >= product.Price && _productsInAutomata.First(p => p.Product.Equals(product)).PullOne())
            {
                Credit -= product.Price;
                return true;
            }

            return false;
        }

        internal bool GetChange(out IEnumerable<MoneyStack> change)
        {
            change = new List<MoneyStack>();
            if (Credit == 0)
            {
                return false;
            }

            var creditToReturn = Credit;
            var toReturn = new List<MoneyStack>();
            foreach (var ms in _automataBank.OrderByDescending(m => m.Banknote.Nominal))
            {
                if (creditToReturn >= ms.Banknote.Nominal)
                {
                    toReturn.Add(new MoneyStack(ms.Banknote, creditToReturn / ms.Banknote.Nominal));
                    creditToReturn -= creditToReturn / ms.Banknote.Nominal * ms.Banknote.Nominal;
                }
            }

            if (creditToReturn != 0)
            {
                return false; //денег не набирается, ничего не возвращаем
            }

            foreach (var ms in toReturn) //возвращаем
            {
                for (var i = 0; i < ms.Amount; ++i) //по одной монетке правда
                {
                    _automataBank.First(m => Equals(m.Banknote, ms.Banknote)).PullOne();
                }
            }

            change = toReturn;
            Credit = 0;
            return true;
        }

        internal void InsertBanknote(Banknote banknote)
        {
            _automataBank.First(ms => ms.Banknote.Equals(banknote)).PushOne();
            Credit += banknote.Nominal;
        }
    }
}