using Vending.Model.Data;

namespace Vending.Model.Logic
{
    public class PurchaseManager
    {
        public Automata Automata { get; } = new Automata();
        public User User { get; } = new User();

        public void BuyProduct(Product product)
        {
            if (Automata.BuyProduct(product))
            {
                User.AddProduct(product);
            }
        }

        public void GetChange()
        {
            if (Automata.GetChange(out var change))
            {
                User.AppendMoney(change);
            }
        }

        public void InsertMoney(Banknote banknote)
        {
            if (User.GetBanknote(banknote))
            {
                Automata.InsertBanknote(banknote);
            }
        }
    }
}