using Prism.Mvvm;
using Vending.Model.Data;

namespace Vending.Model.Logic
{
    public class ProductStack : BindableBase
    {
        private int _amount;

        public ProductStack(Product product, int amount)
        {
            Product = product;
            _amount = amount;
        }

        public int Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        public Product Product { get; }

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