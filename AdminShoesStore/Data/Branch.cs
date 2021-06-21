using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AdminShoesStore.Data
{
    public partial class Branch
    {
        public Branch()
        {
            Products = new HashSet<Product>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
