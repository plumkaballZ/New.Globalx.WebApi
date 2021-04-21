using System.Collections.Generic;

namespace New.Globalx.WebApi.Models
{
    public class Product
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Price { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public int TotalOnHand { get; set; }
        public string DisplayPrice { get; set; }
        public string OptionsText { get; set; }
        public bool InStock { get; set; }
        public bool IsBackorderable { get; set; }
        public bool IsDestroyed { get; set; }
        public string SmallImage { get; set; }
        public List<string> Images { get; set; }

        public string MetaDescription { get; set; }
        public List<string> LiArray { get; set; }

        //Variants
        public List<Variant> Variants { get; set; }
        public Variant DefaultVariant { get; set; }
        public bool HasVariants { get; set; }

        public string FilterCategory { get; set; }
    }
}
