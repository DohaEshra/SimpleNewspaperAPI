﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Newspaper.Models
{
    [Table("Category")]
    public partial class Category
    {
        public Category()
        {
            Articles = new HashSet<Article>();
        }

        [Key]
        [StringLength(50)]
        [Unicode(false)]
        public string Category_Name { get; set; }
        public int AdminID { get; set; }

        [ForeignKey("AdminID")]
        [InverseProperty("Categories")]
        public virtual User Admin { get; set; }
        [InverseProperty("Category_NameNavigation")]
        public virtual ICollection<Article> Articles { get; set; }
    }
}