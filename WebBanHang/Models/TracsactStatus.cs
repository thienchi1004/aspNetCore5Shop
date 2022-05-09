using System;
using System.Collections.Generic;

#nullable disable

namespace WebBanHang.Models
{
    public partial class TracsactStatus
    {
        public TracsactStatus()
        {
            Orders = new HashSet<Order>();
        }

        public int TracsactStatusId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
