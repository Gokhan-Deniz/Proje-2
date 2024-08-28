using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Envanter.Models
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int StockQuantity { get; set; }
        public string Barcode { get; set; }
        public string Details { get; set; }
        public string Category { get; set; }
        public DateTime AddedDate { get; set; }
    }
}