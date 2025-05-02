namespace sunflower.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public int Price { set; get; }
        public int Quantity { set; get; }
        public string PathOfImage { set; get; }
    
        public int Catid { set; get; }
    }
}
