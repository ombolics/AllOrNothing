using System.ComponentModel.DataAnnotations;

namespace AllOrNothing.Data
{
    public class Competence
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}