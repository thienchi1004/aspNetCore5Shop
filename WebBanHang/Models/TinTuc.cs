using System;
using System.Collections.Generic;

#nullable disable

namespace WebBanHang.Models
{
    public partial class TinTuc
    {
        public int PostId { get; set; }
        public string Tittle { get; set; }
        public string ShortContents { get; set; }
        public string Contents { get; set; }
        public string Image { get; set; }
        public bool Published { get; set; }
        public string Alias { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Author { get; set; }
        public int? AccountId { get; set; }
        public string Tags { get; set; }
        public int? CateId { get; set; }
        public bool? IsHot { get; set; }
        public bool? IsNewfeed { get; set; }
        public string MetaDesc { get; set; }
        public string MetaKey { get; set; }
        public int? Views { get; set; }

        public virtual Account Account { get; set; }
        public virtual Category Cate { get; set; }
    }
}
