using System.ComponentModel.DataAnnotations;

namespace AllOrNothing.Data
{
    public class Player
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Institue { get; set; }
        public string NickName { get; set; }
        //public ICommand RemoveCommand { get; set; }
        //public ICommand TestCommand { get; set; }
        //public override string ToString()
        //{
        //    return Name;
        //}
    }
}