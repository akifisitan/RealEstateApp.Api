namespace RealEstateApp.Api.Entity
{
    public class Currency : PropertyFieldBase
    {
        public Currency() { }
        public Currency(string currency)
        {
            base.Value = currency;
        }
    }
}
