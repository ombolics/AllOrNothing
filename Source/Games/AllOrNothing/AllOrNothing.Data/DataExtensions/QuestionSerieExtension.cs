namespace AllOrNothing.Data.DataExtensions
{
    public static class QuestionSerieExtension
    {
        public static void SetValue(this QuestionSerie originalSerie, QuestionSerie newSerie)
        {
            originalSerie.Name = newSerie.Name;
            originalSerie.Id = newSerie.Id;
            originalSerie.IsDeleted = newSerie.IsDeleted;
            for (int i = 0; i < originalSerie.Topics.Count; i++)
            {
                originalSerie.Topics[i].SetValue(newSerie.Topics[i]);
            }

        }
    }
}
