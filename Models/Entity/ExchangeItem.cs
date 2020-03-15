namespace FinanceApi.Models.Entity
{
    /// <summary>
    /// Exchange Item
    /// </summary>
    public class ExchangeItem
    {
        /// <summary>
        /// Exchange id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Exchange value
        /// </summary>
        public decimal Value { get; set; }
    }
}