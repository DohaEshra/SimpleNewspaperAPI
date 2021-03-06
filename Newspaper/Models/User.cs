// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Newspaper.Models
{
    public partial class User
    {
        public User()
        {
            Articles = new HashSet<Article>();
            Categories = new HashSet<Category>();
        }

        [Key]
        public int UsrID { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Username { get; set; }
        [StringLength(50)]
        public string Password { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string Role { get; set; }

        [InverseProperty("Writer")]
        public virtual ICollection<Article> Articles { get; set; }
        [InverseProperty("Admin")]
        public virtual ICollection<Category> Categories { get; set; }
    }
}