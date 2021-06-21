using System;
using System.Collections.Generic;

#nullable disable

namespace AdminShoesStore.Data
{
    public partial class Order
    {
        public Order()
        {
            DetailOrders = new HashSet<DetailOrder>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public int Amount { get; set; }
        public int? StatusId { get; set; }
        public DateTime? CreateAt { get; set; }

        public virtual Status Status { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<DetailOrder> DetailOrders { get; set; }
    }
}
