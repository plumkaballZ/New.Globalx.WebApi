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
                GetGlobalXCardHolder(),
                GetXBelt(),
                GetKeyHanger(),
                GetBambooSocks(),
                GetCardHolder()
            };
        }
        public Product GetGlobalXCardHolder()
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
                SmallImage = "pics/theglobalx/small.png",
                Images = new List<string>()
                {
                    "pics/theglobalx/21.jpg",
                    "pics/theglobalx/22.jpg",
                    "pics/theglobalx/23.jpg",
                    "pics/theglobalx/24.jpg",
                    "pics/theglobalx/25.jpg",
                    "pics/theglobalx/26.jpg",
                    "pics/theglobalx/27.jpg"
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
                },
                HasVariants = true,
                FilterCategory = "Kortholdere"
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
                SmallImage = "pics/xbelt/small.png",
                Images = new List<string>()
                {
                    "pics/xbelt/1.jpg",
                    "pics/xbelt/2.jpg",
                    "pics/xbelt/3.jpg",
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
                },
                HasVariants = true,
                FilterCategory = "Bælter"
            };
        }
        public Product GetKeyHanger()
        {
            var variants = new List<Variant>()
            {
                new Variant()
                {
                    Id = 8
                }
            };

            return new Product()
            {
                Name = "Keyhanger By Shevlin",
                ShortName = "Keyhanger",
                Description = @"
Keyhangeren er lavet af samme vegetabilsk garvede lædertype som vores kortholder. 
Med denne nøglering har du altid dine nøgler med dig på ansvarlig vis, både for din egen skyld men også for miljøets.",
                Price = "129,95",
                DisplayPrice = "129,95 DKK",
                Slug = "prod_01",
                MetaDescription = "",
                SmallImage = "pics/keyhanger/small.png",
                Images = new List<string>()
                {
                    "pics/keyhanger/1.jpg",
                    "pics/keyhanger/2.jpg",
                    "pics/keyhanger/3.jpg",
                    "pics/keyhanger/4.jpg",
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
                    "Håndlavet",
                    "Stilrent design"
                },
                HasVariants = false,
                FilterCategory = "Nøgleringe"
            };
        }
        public Product GetBambooSocks()
        {
            const string variantType = "Size";

            var variants = new List<Variant>()
            {
                new Variant()
                {
                    Id = 9, Type = variantType, Value = "35-40 EU"
                },
                new Variant()
                {
                    Id = 10, Type = variantType, Value = "41-48 EU"
                }
            };

            return new Product()
            {
                Name = "Bamboo socks by Shevlin",
                ShortName = "Bamboo Socks",
                Description = @"
Vi har lavet en sok der ikke bare er behagelig, men samtidig bæredygtig. Den er antibakteriel, super blød, 
god til sart hud og temperatur regulerende og mindsker fugtige fødder.",
                Price = "129,95",
                DisplayPrice = "129,95 DKK",
                Slug = "prod_01",
                MetaDescription = @"
Sokkerne er lavet af bambus for et mere bæredygtigt produkt. 
Sokkerne sidder tæt til huden og det føles næsten som om man ikke har sokker på.",
                SmallImage = "pics/socks/small.png",
                Images = new List<string>()
                {
                    "pics/socks/1.jpg",
                    "pics/socks/2.jpg"
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
                    "75% Bambus",
                    "20% Polyester",
                    "5% Elastin"
                },
                HasVariants = true,
                FilterCategory = "Sokker"
            };
        }
        public Product GetCardHolder()
        {
            var variants = new List<Variant>()
            {
                new Variant()
                {
                    Id = 11
                }
            };

            return new Product()
            {
                Name = "Card Holder By Shevlin",
                ShortName = "Card Holder",
                Description = @"
Flot vegetabilsk garvet kortholder. Håndsyet for de flotteste resultater og for den bedste holdbarhed.",
                Price = "249,95",
                DisplayPrice = "249,95 DKK",
                Slug = "prod_01",
                MetaDescription = @"
Den er dobbeltsiddet med plads til op til 8 kort. Samtidig har du mulighed for at have dine sedler og kvitteringer med på farten.",
                SmallImage = "pics/cardholder/small.png",
                Images = new List<string>()
                {
                    "pics/cardholder/1.jpeg"
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
                    "Simpel",
                    "Unisex"
                },
                HasVariants = false,
                FilterCategory = "Kortholdere"
            };
        }
    }
}
