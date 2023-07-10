using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace book_store_server_side.Models
{
	public class Book
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string ? Description { get; set; }

        [Required]
        public int Price { get; set; }

        
        [Required]
        public string Image { get; set; }

    }

}

