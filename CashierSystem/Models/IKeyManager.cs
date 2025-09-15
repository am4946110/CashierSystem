namespace CashierSystem.Models
{
    public interface IKeyManager
    {
        string GetKey();
        void SetKey(string key);
    }
}
