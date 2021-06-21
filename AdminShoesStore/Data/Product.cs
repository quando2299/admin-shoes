using System;
using System.Collections.Generic;

#nullable disable

namespace AdminShoesStore.Data
{
    public partial class Product
    {
        public Product()
        {
            DetailOrders = new HashSet<DetailOrder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Descriptions { get; set; }
        public int? BranchId { get; set; }
        public int? CategoryId { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<DetailOrder> DetailOrders { get; set; }
    }
}
