using Prism.Mvvm;
using Vending.Model.Data;

namespace Vending.Model.Logic
{
    public class MoneyStack : BindableBase
    {
        private int _amount;

        public MoneyStack(Banknote banknote, int amount)
        {
            Banknote = banknote;
            Amount = amount;
        }

        public int Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        public Banknote Banknote { get; }

        internal bool PullOne()
        {
            if (Amount > 0)
            {
                --Amount;
                return true;
            }

            return false;
        }

        internal void PushOne()
        {
            ++Amount;
        }
    }
}