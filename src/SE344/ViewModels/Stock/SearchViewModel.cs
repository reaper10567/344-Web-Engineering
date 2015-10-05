namespace SE344.ViewModels.Stock
{
    public class SearchViewModel
    {
        public string Symbol { get; set; }
        public decimal? CurrentPrice { get; set; }
        public decimal? DaysHigh { get; set; }
        public decimal? DaysLow { get; set; }
        public decimal? YearsHigh { get; set; }
        public decimal? YearsLow { get; set; }
    }
}
