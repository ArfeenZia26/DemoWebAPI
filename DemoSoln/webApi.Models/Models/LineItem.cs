﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace webApi.Models
{
    public partial class LineItem
    {
        public long Id { get; set; }
        public long InvoiceId { get; set; }
        public decimal? ItemPrice { get; set; }
        public string ProductType { get; set; }

        public virtual Customer Invoice { get; set; }
    }
}