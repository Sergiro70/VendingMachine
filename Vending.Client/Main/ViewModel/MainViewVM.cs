using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Vending.Model.Logic;

namespace Vending.Client.Main.ViewModel
{
    public class MainViewVM : BindableBase
    {
        private Automata _automata;
        private PurchaseManager _manager = new PurchaseManager();
        private User _user;

        public MainViewVM()
        {
            _user = _manager.User;
            _automata = _manager.Automata;
            _user.PropertyChanged += (s, a) => { RaisePropertyChanged(nameof(CheckAmount)); };
            _automata.PropertyChanged += (s, a) => { RaisePropertyChanged(nameof(Credit)); };

            //Кошелек пользователя
            UserWallet = new ObservableCollection<MoneyVM>(_user.UserWallet.Select(ms => new MoneyVM(ms)));
            Watch(_user.UserWallet, UserWallet, um => um.MoneyStack);
            //покупки пользователя
            CustomPurchases =
                new ObservableCollection<ProductVM>(_user.CustomPurchases.Select(ub => new ProductVM(ub)));
            Watch(_user.CustomPurchases, CustomPurchases, ub => ub.ProductStack);

            //деньги автомата
            AutomataBank =
                new ObservableCollection<MoneyVM>(_automata.AutomataBank.Select(a => new MoneyVM(a, _manager)));
            Watch(_automata.AutomataBank, AutomataBank, a => a.MoneyStack);
            //товары автомата
            ProductsInAutomata =
                new ObservableCollection<ProductVM>(
                    _automata.ProductsInAutomata.Select(ap => new ProductVM(ap, _manager)));
            Watch(_automata.ProductsInAutomata, ProductsInAutomata, p => p.ProductStack);

            GetChange = new DelegateCommand(() => _manager.GetChange());
        }

        public ObservableCollection<MoneyVM> AutomataBank { get; }

        public int CheckAmount => _user.CheckAmount;
        public int Credit => _automata.Credit;
        public ObservableCollection<ProductVM> CustomPurchases { get; }
        public DelegateCommand GetChange { get; }
        public ObservableCollection<ProductVM> ProductsInAutomata { get; }
        public ObservableCollection<MoneyVM> UserWallet { get; }

        //функция синхронизации ReadOnly коллекции элементов модели и соответствующей коллекции VM, 
        //в конструкторы которых передается эти экземпляры модели, указываемые в делегате
        private static void Watch<T, T2>(ReadOnlyObservableCollection<T> collToWatch,
            ObservableCollection<T2> collToUpdate,
            Func<T2, object> modelProperty)
        {
            ((INotifyCollectionChanged)collToWatch).CollectionChanged += (s, a) =>
           {
               if (a.NewItems?.Count == 1)
               {
                   collToUpdate.Add((T2)Activator.CreateInstance(typeof(T2), (T)a.NewItems[0], null));
               }

               if (a.OldItems?.Count == 1)
               {
                   collToUpdate.Remove(collToUpdate.First(mv => modelProperty(mv) == a.NewItems[0]));
               }
           };
        }
    }
}