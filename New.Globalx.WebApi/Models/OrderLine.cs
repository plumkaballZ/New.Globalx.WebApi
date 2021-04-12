namespace New.Globalx.WebApi.Models
{
    public class OrderLine
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int Variant_Id { get; set; }
        //public string Image_Url { get; set; }

        public bool NewLine { get; set; }
        public bool Deleted { get; set; }
        public bool Updated { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
    }
}
