using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeneralStoreAPI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(0,double.MaxValue)]
        [Required]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        [Range(0,int.MaxValue)]
        public int QuantityOnHand { get; set; }
    }
}