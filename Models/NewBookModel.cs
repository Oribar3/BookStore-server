using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace book_store_server_side.Models
{
    public class NewBookModel
    {
   

        [Required(ErrorMessage = "Please add a title")]
        public string Title { get; set; }

        public string Description { get; set; }


        [Required(ErrorMessage = "Please add a price")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Please add an image")]
        public string Image { get; set; }
    }
}

