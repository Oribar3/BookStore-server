using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace book_store_server_side.Models
{
	public class CartItems
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

		public string AppUserId { get; set; }
		public int BookId { get; set; }
		public int Amount { get; set; } = 0;
	}
}

