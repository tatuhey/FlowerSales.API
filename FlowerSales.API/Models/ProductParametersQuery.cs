﻿namespace FlowerSales.API.Models
{
    public class ProductParametersQuery:QueryParameters
    {
        public decimal ? MinPrice { get; set; }
        public decimal? MaxPrice { get; set;}

        public string Name { get; set; } = String.Empty;



    }
}
