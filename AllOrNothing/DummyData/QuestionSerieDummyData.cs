
using AllOrNothing.Data;
using System;
using System.Collections.Generic;

namespace AllOrNothing.DummyData
{
    public static class QuestionSerieDummyData
    {
        private static Player _author = new Player
        {
            Institue = "",
            NickNames = new List<string> { "Csabi, ÖCS" },
            Name = "Csaba",
        };

        private static int _currentId = 0;
        public static int CurrentId => _currentId;
        public static QuestionSerie QS1 => new QuestionSerie
        {
            Id = _currentId++,
            Topics = new List<Topic>
            {
                new Topic
                {
                    Id = 0,
                    Author = _author,
                    Name = "Matek",
                    Competences = new List<Competence>
                    {
                        new Competence
                        {
                            Id = 0,
                            Name = "Matematika",
                        },
                    },
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Id = 0,
                            Text = "Mennyi 2+2?",
                            Answer = "4",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 1000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Mennyi 4+4?",
                            Answer = "8",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 2000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Mennyi 10+2?",
                            Answer = "12",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 3000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Mennyi 3-2?",
                            Answer = "1",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 4000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Mennyi -5+4?",
                            Answer = "-1",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 5000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Mennyi 22+22?",
                            Answer = "44",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 8000
                        },
                    }
                },
                new Topic
                {
                    Id = 0,
                    Author = _author,
                    Name = "Lol",
                    Competences = new List<Competence>
                    {
                        new Competence
                        {
                            Id = 0,
                            Name = "LOL",
                        },
                    },

                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Id = 0,
                            Text = "Melyik champion a legjobb?",
                            Answer = "AAtrox",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 1000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Süt a nap?",
                            Answer = "nem",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 2000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Mit egyek ma?",
                            Answer = "igen",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 3000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Mennyi a hidrogén atomtömege?",
                            Answer = "1",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 4000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Kevés a minimálbér?",
                            Answer = "túl sok",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 5000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Szabolcs mondott egy kérdést vajon?",
                            Answer = "",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 8000
                        },
                    }
                },
                 new Topic
                {
                    Id = 0,
                    Author = _author,
                    Name = "Matek",
                    Competences = new List<Competence>
                    {
                        new Competence
                        {
                            Id = 0,
                            Name = "Matematika",
                        },
                    },

                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Id = 0,
                            Text = "Mennyi 2+2?",
                            Answer = "4",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 1000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Mennyi 4+4?",
                            Answer = "8",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 2000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Mennyi 10+2?",
                            Answer = "12",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 3000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Mennyi 3-2?",
                            Answer = "1",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 4000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Mennyi -5+4?",
                            Answer = "-1",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 5000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Mennyi 22+22?",
                            Answer = "44",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 8000
                        },
                    }
                },
                new Topic
                {
                    Id = 0,
                    Author = _author,
                    Name = "Lol",
                    Competences = new List<Competence>
                    {
                        new Competence
                        {
                            Id = 0,
                            Name = "LOL",
                        },
                    },

                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Id = 0,
                            Text = "Melyik champion a legjobb?",
                            Answer = "AAtrox",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 1000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Süt a nap?",
                            Answer = "nem",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 2000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Mit egyek ma?",
                            Answer = "igen",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 3000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Mennyi a hidrogén atomtömege?",
                            Answer = "1",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 4000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Kevés a minimálbér?",
                            Answer = "túl sok",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 5000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Szabolcs mondott egy kérdést vajon?",
                            Answer = "",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 8000
                        },
                    }
                },
                new Topic
                {
                    Id = 0,
                    Author = _author,
                    Name = "Matek",
                    Competences = new List<Competence>
                    {
                        new Competence
                        {
                            Id = 0,
                            Name = "Matematika",
                        },
                    },

                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Id = 0,
                            Text = "Mennyi 2+2?",
                            Answer = "4",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 1000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Mennyi 4+4?",
                            Answer = "8",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 2000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Mennyi 10+2?",
                            Answer = "12",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 3000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Mennyi 3-2?",
                            Answer = "1",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 4000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Mennyi -5+4?",
                            Answer = "-1",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 5000
                        },
                        new Question
                        {
                            Id = 0,
                            Text = "Mennyi 22+22?",
                            Answer = "44",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 8000
                        },
                    }
                },
            },

        };
    }
}
