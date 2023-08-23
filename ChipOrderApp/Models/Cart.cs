namespace ChipOrderApp.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public List<CartItem> Items { get; set; }
        public double TotalPrice { get; set; }
    }
}
