using System.ComponentModel.DataAnnotations;

namespace AllOrNothing.Data
{
    public enum QuestionResourceType
    {
        TEXT,
        IMAGE,
        VIDEO,
        MUSIC
    }
    public enum QuestionType
    {
        THEMATICAL,
        LIGHTNING,
    }
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public QuestionResourceType ResourceType { get; set; }
        public QuestionType Type { get; set; }
        public byte[] Resource { get; set; }
        [Required]
        public string Text { get; set; }
        public string Answer { get; set; }
        public int Value { get; set; }
    }
}