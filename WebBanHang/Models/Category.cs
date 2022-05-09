using System;
using System.Collections.Generic;

#nullable disable

namespace WebBanHang.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
            TinTucs = new HashSet<TinTuc>();
        }

        public int CateId { get; set; }
        public string CateName { get; set; }
        public string Description { get; set; }
        public int? ParentCateId { get; set; }
        public int? Levels { get; set; }
        public int? Ordering { get; set; }
        public bool Published { get; set; }
        public string Image { get; set; }
        public string Tittle { get; set; }
        public string Alias { get; set; }
        public string MetaDesc { get; set; }
        public string MetaKey { get; set; }
        public string Cover { get; set; }
        public string SchemaMarkup { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<TinTuc> TinTucs { get; set; }
    }
}
