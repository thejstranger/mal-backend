using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAL.DAL
{
    [Table("Users")]
    public class Users
    { 
        public Users(){}
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(20)]
        public string Firstname { get; set; }

        [MaxLength(50)]
        public string Lastname { get; set; }

        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MaxLength(20)]
        public string Password { get; set; }

        public string Token { get; set; }

    }
}
