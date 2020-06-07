namespace Payments_API.Controllers
{
    public class PurchaseOrder
    {
        public int AmountToPay { get; set; }
        public string PONumber { get; set; }
        public string CompanyName { get; set; }
        public int PaymentDayTerms { get; set; }
    }
}