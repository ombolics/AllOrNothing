using AllOrNothing.Data;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace AllOrNothing.DummyData
{
    public static class DummyData
    {
        private static ICommand _testCommand;
        public static ICommand TestCommand => _testCommand ??= new RelayCommand(Asd);

        public static void Asd()
        {

        }


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
                            Text = "A matematika, tárgyát és módszereit tekintve, sajátos tudomány, mely részben a többi tudomány által vizsgált, részben pedig a matematika „belső” fejlődéséből adódóan létrejött (felfedezett, ill. feltalált) rendszereket, struktúrákat, azok absztrakt, közösen meglévő tulajdonságait vizsgálja.Régebben a „mennyiség és a tér tudományaként” (vagyis a számok és geometriai alakzatok tanaként) határozták meg, a múlt század elejétől kezdve pedig a",
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

        public static List<Player> PLayers => new List<Player>
        {
            new Player
            {
                Id = 0,
                Institue = "PSEG",
                Name = "Ömböli Csaba",
                NickNames = new List<string> {"Csabi", "ÖCS"},
                TestCommand = TestCommand,
            },
            new Player
            {
                Id = 1,
                Institue = "PSEG",
                Name = "Zareczky András",
                NickNames = new List<string> {"Zarkó", "Bandi"},
                TestCommand = TestCommand,
            },
            new Player
            {
                Id = 2,
                Institue = "PSEG",
                Name = "Zell Julian Maxim",
                NickNames = new List<string> {},
                TestCommand = TestCommand,
            },

            new Player
            {
                Id = 3,
                Institue = "PSEG",
                Name = "Kiss Géza Ferenc Péter János",
                NickNames = new List<string> {},
                TestCommand = TestCommand,
            },
            new Player
            {
                Id = 4,
                Institue = "PSEG",
                Name = "Németh Zoltán",
                NickNames = new List<string> {},
                TestCommand = TestCommand,
            },
            new Player
            {
                Id = 5,
                Institue = "PSEG",
                Name = "Nagy Lajos",
                NickNames = new List<string> {},
                TestCommand = TestCommand,
            },
            new Player
            {
                Id = 6,
                Institue = "PSEG",
                Name = "Kis Pippin",
                NickNames = new List<string> {},
                TestCommand = TestCommand,
            },
            new Player
            {
                Id = 7,
                Institue = "PSEG",
                Name = "XVII. Lajos",
                NickNames = new List<string> {},
                TestCommand = TestCommand,
            },
            new Player
            {
                Id = 8,
                Institue = "PSEG",
                Name = "Loius Bonaparte",
                NickNames = new List<string> {},
                TestCommand = TestCommand,
            },
        };

        public static List<Team> Teams => new List<Team>()
        {
            new Team
            {
                Id = 0,
                Players = PLayers.GetRange(0,2),
                TeamName = "Csapat1",
                TestCommand = TestCommand,
            },
            new Team
            {
                Id = 0,
                Players = PLayers.GetRange(1,2),
                TeamName = "Csapat2",
                TestCommand = TestCommand,
            },
            new Team
            {
                Id = 0,
                Players = PLayers.GetRange(0,2),
                TeamName = "Csapat3",
                TestCommand = TestCommand,
            },
            new Team
            {
                Id = 0,
                Players = PLayers.GetRange(0,2),
                TeamName = "Csapat4",
                TestCommand = TestCommand,
            },
            new Team
            {
                Id = 0,
                Players = PLayers.GetRange(2,2),
                TeamName = "Csapat5",
                TestCommand = TestCommand,
            },
            new Team
            {
                Id = 0,
                Players = PLayers.GetRange(2,2),
                TeamName = "Csapat6",
                TestCommand = TestCommand,
            },
            new Team
            {
                Id = 0,
                Players = PLayers.GetRange(3,2),
                TeamName = "Csapat7",
                TestCommand = TestCommand,
            },
            new Team
            {
                Id = 0,
                Players = PLayers.GetRange(1,3),
                TeamName = "Csapat8",
                TestCommand = TestCommand,
            },
            new Team
            {
                Id = 0,
                Players = PLayers.GetRange(4,2),
                TeamName = "Csapat9",
                TestCommand = TestCommand,
            },
        };
        
    }
}