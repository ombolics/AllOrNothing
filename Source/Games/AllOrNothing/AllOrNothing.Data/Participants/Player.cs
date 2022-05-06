using System.ComponentModel.DataAnnotations;

namespace AllOrNothing.Data
{
    public class Player
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Institute { get; set; }
        public string NickName { get; set; }
        public bool IsDeleted { get; set; }
    }
}