
namespace SE344.Models
{
    public class StockNote
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public string StockTicker { get; set; }
        public string Note { get; set; }
    }
}