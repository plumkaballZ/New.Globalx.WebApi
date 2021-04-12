using System.Collections.Generic;
using System.Linq;
using New.Globalx.WebApi.Models;

namespace New.Globalx.WebApi.Repos
{
    public class ProductRepo : BaseRepo
    {
        public List<Product> GetAllProduct()
        {
            return new List<Product>()
            {
                GetCardHolder(),
                GetXBelt()
            };
        }
        public Product GetCardHolder()
        {
            const string variantType = "Color";

            var variants = new List<Variant>()
            {
                new Variant()
                {
                    Id = 1, Type = variantType, Value = "Silver"
                },
                new Variant()
                {
                    Id = 2, Type = variantType, Value = "Antique"
                }
            };

            return new Product()
            {
                Name = "The Global X cardholder By Shevlin",
                ShortName = "The Global X cardholder",
                Description = @"
Vores absolut første produkt.Kortholderne er lavet med 100 % vegetabilsk garvet læder.
Læderet laver over tid sin egen unikke patina, alt afhængig af hvordan den bruges.
Kortholderne er dobbeltsidet og kan holde 8 - 10 kort samt sedler",
                Price = "359,95",
                DisplayPrice = "359,95 DKK",
                Slug = "prod_01",
                MetaDescription = "Hver og én er udført i hånden og vi har benyttet YKK lynlåse på begge modeller for at sikre den absolut højeste kvalitet. ",
                SmallImage = "https://shevlin.co/assets/api/_prod/theglobalx/small.png",
                Images = new List<string>()
                {
                    "https://shevlin.co/assets/api/_prod/theglobalx/21.jpg",
                    "https://shevlin.co/assets/api/_prod/theglobalx/22.jpg",
                    "https://shevlin.co/assets/api/_prod/theglobalx/23.jpg",
                    "https://scontent-cph2-1.xx.fbcdn.net/v/t1.6435-9/166342251_757031264943326_1875176381245870455_n.jpg?_nc_cat=102&ccb=1-3&_nc_sid=8bfeb9&_nc_ohc=kzvaQZmrl8cAX-JEEnb&_nc_ht=scontent-cph2-1.xx&oh=9e63fe9fad9c64d54c1fbff8e9bd9a63&oe=608FC011",
                    "https://scontent-cph2-1.xx.fbcdn.net/v/t1.6435-9/166482215_757031321609987_7571196016937052831_n.jpg?_nc_cat=108&ccb=1-3&_nc_sid=8bfeb9&_nc_ohc=6cTxTE4Hi1IAX_N3ji2&_nc_ht=scontent-cph2-1.xx&oh=9bd016be71f5cee5c06a0ef6f6bf4f45&oe=608FA927",
                    "https://scontent-cph2-1.xx.fbcdn.net/v/t1.6435-9/43368035_265029634143494_3935031070747000832_n.jpg?_nc_cat=102&ccb=1-3&_nc_sid=8bfeb9&_nc_ohc=UbEm3401DrQAX_ftC8r&_nc_ht=scontent-cph2-1.xx&oh=24ccdc3eed3bacb815fc0157c27f0a06&oe=6091F2E8",
                    "https://scontent-cph2-1.xx.fbcdn.net/v/t1.6435-9/41934622_259256991387425_7586443653980094464_n.jpg?_nc_cat=104&ccb=1-3&_nc_sid=8bfeb9&_nc_ohc=QS3EZQ0ZzqwAX84tnOX&_nc_ht=scontent-cph2-1.xx&oh=f5c94fd3d2a20122ffea423cf1f81764&oe=609289DC"
                },
                OptionsText = "(Size: small, Colour: red)",
                InStock = true,
                IsBackorderable = true,
                TotalOnHand = 10,
                IsDestroyed = false,
                Variants = variants,
                DefaultVariant = variants.FirstOrDefault(),
                LiArray = new List<string>()
                {
                    "100% vegetabilsk garvet læder",
                    "Utrolig holdbar",
                    "Får unik patina over tid"
                }
            };
        }
        public Product GetXBelt()
        {
            const string variantType = "Size";

            var variants = new List<Variant>()
            {
                new Variant()
                {
                    Id = 3, Type = variantType, Value = "80CM"
                },
                new Variant()
                {
                    Id = 4, Type = variantType, Value = "85CM"
                },
                new Variant()
                {
                    Id = 5, Type = variantType, Value = "90CM"
                },
                new Variant()
                {
                    Id = 6, Type = variantType, Value = "95CM"
                },
                new Variant()
                {
                    Id = 7, Type = variantType, Value = "100CM"
                }
            };

            return new Product()
            {
                Name = "Belt By Shevlin",
                ShortName = "Belt",
                Description = @"
Det første produkt i vores kommende kollektion “pure cognac”. Dette unikke bælte 
er chrom garvet under bæredygtige forhold og overholder alle EU regulationer.",
                Price = "349,95",
                DisplayPrice = "349,95 DKK",
                Slug = "prod_01",
                MetaDescription = @"
Vi har udviklet et bælte der ikke bare er bæredygtigt men samtidig utrolig 
smukt og klassisk i sit udtryk.",
                SmallImage = "https://shevlin.co/assets/api/_prod/xbelt/small.png",
                Images = new List<string>()
                {
                    "https://shevlin.co/assets/api/_prod/xbelt/3.jpg",
                    "https://shevlin.co/assets/api/_prod/xbelt/2.jpg",
                    "https://shevlin.co/assets/api/_prod/xbelt/1.jpg",
                },
                OptionsText = "(Size: small, Colour: red)",
                InStock = true,
                IsBackorderable = true,
                TotalOnHand = 10,
                IsDestroyed = false,
                Variants = variants,
                DefaultVariant = variants.FirstOrDefault(),
                LiArray = new List<string>()
                {
                    "Bæredygtig chrom-garvet læder",
                    "Håndlavet",
                    "Ansvarlig"
                }
            };
        }
    }
}
