using AllOrNothing.Data;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace AllOrNothing.DummyData
{
    public static class DummyData
    {
        // private static Mapper _mapper = new Mapper(new MapperConfiguration());
        private static ICommand _testCommand;
        public static ICommand TestCommand => _testCommand ??= new RelayCommand(Asd);

        public static void Asd()
        {

        }

        private static Player _author = new Player
        {
            Institue = "PSEG",
            NickName = "Csabi",
            Name = " Ömböli Csaba",
        };

        private static int _currentId = 0;
        public static int CurrentId => _currentId;

        public static List<QuestionSerie> TestSeries => new List<QuestionSerie>
        {
            QS1,
            QS1,
            QS1,
            QS1,
            QS1,
            QS1,
            QS1,
        };

        public static QuestionSerie QS1 => new QuestionSerie
        {
            Topics = new List<Topic>
            {
                new Topic
                {
                    Author = _author,
                    Name = "Egyiptomitológia",
                    Competences = new List<Competence>
                    {
                        new Competence
                        {
                            Name = "történelem",
                        },
                    },
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Text = "Az alvilág, és a holtak oltalmazója, a holttestek bebalzsamozóinak istene. Ozirisz kultusza előtt ő vezette a holtakat az alvilágba. Sakál fejjel ábrázolják.",
                            Answer = "Anubis",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 1000
                        },
                        new Question
                        {
                            Text = "Egyiptom egyik legfontosabb istene, a királyok isteni megszemélyesítője. Az idők során alakja egybeolvadt Ré-vel. Az ég sólyomfejű istene",
                            Answer = "Hórusz",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 2000
                        },
                        new Question
                        {
                            Text = "A sivatag a zűrzavar és a pusztítás istene. Geb és Nut fia, Ozirisz gyilkosa. Ember testtel és állati fejjel ábrázolják, de feje nem beazonosítható, leginkább hiénára hasonlít.",
                            Answer = "Szeth",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 3000
                        },
                        new Question
                        {
                            Text = "A sivatag a zűrzavar és a pusztítás istene. Geb és Nut fia, Ozirisz gyilkosa. Ember testtel és állati fejjel ábrázolják, de feje nem beazonosítható, leginkább hiénára hasonlít.",
                            Answer = "Thot",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 4000
                        },
                        new Question
                        {
                            Text = "A Nílus krokodilfejű istene. Ő biztosította a rendszeres áradást. A fáraó hadseregének védelmezője. Egyes források az emberek és az istenek védelmezőjeként utalnak rá.",
                            Answer = "Szobek",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 5000
                        },
                        new Question
                        {
                            Text = "A termékenység, és a teremtés istene. A kosfejű lény másik neve a „nagy fazekas”, mert fazekaskorongján ő formázta meg az első embereket és a lelküket.",
                            Answer = "Khnum",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 8000
                        },
                    }
                },
                new Topic
                {
                    Author = _author,
                    Name = "Kakukktojás",
                    Competences = new List<Competence>
                    {
                        new Competence
                        {
                            Id = 0,
                            Name = "logika",
                        },
                    },

                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Text = "Párizs, Róma, Budapest, London, Tunisz",
                            Answer = "Tunisz nem európai főváros",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 1000
                        },
                        new Question
                        {
                            Text = "128, 64, 254, 1024, 1",
                            Answer = "A 254 nem 2 hatvány",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 2000
                        },
                        new Question
                        {
                            Text = "Nagy Lajos, Luxemburgi Zsigmond, Hunyadi Mátyás, Szapolyai János, II. Ulászló",
                            Answer = "Szapolyai csak magyar király volt",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 3000
                        },
                        new Question
                        {
                            Text = "lajbi, szedett ing, priccses nadrág, pendely, krecsány, rékli	",
                            Answer = "a pendely nem felsőruházat",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 4000
                        },
                        new Question
                        {
                            Text = "Ernest Rutherford, Niels Bohr, Lénárd Fülöp, Albert Einstein, Pierre Curie, Gábor Dénes",
                            Answer = "Rutherford nem kapott fizikai Nobel-díjat",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 5000
                        },
                        new Question
                        {
                            Text = "Apple, Disney, IBM, Nike, General Electric, McDonald’s, Coca-Cola, Boeing",
                            Answer = "General Electric (nem része a Dow Jones indexnek) ",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 8000
                        },
                    }
                },
                 new Topic
                {
                    Author = _author,
                    Name = "Csacsi-pacsi",
                    Competences = new List<Competence>
                    {
                        new Competence
                        {
                            Name = "logika",
                        },
                    },

                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Text = "Keretbe foglalt dróthálóból készült használati tárgy, itt tudjuk feladni a leveleket ",
                            Answer = "rosta-posta",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 1000
                        },
                        new Question
                        {
                            Text = "Szavazati joggal rendelkező személy, egy balkáni országból származó",
                            Answer = "polgár-bolgár",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 2000
                        },
                        new Question
                        {
                            Text = "Ókori találmánnyal rendelkező, életkorát meghazudtolóan negatívan viselkedő",
                            Answer = "kerekes-gyerekes",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 3000
                        },
                        new Question
                        {
                            Text = "Börtön ”szobája”, fizetőeszköz szlenges neve ",
                            Answer = "cella-della",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 4000
                        },
                        new Question
                        {
                            Text = "A Kárpátok legmagasabb hegyvonulata, szétszedhető ideiglenes lakása ",
                            Answer = "Tátra-sátra",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 5000
                        },
                        new Question
                        {
                            Text = "Magánál értéktelenebbnek, alacsonyabb rendűnek tart, a betelepítéseket irányító közösségi vezető a középkori Magyarországon",
                            Answer = "lenéz-kenéz",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 8000
                        },
                    }
                },
                new Topic
                {
                    Author = _author,
                    Name = "Orosz irodalom",
                    Competences = new List<Competence>
                    {
                        new Competence
                        {
                            Name = "irodalom",
                        },
                    },

                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Text = "Mely ruhadarab szerepel Gogol egy művének a címében?",
                            Answer = "köpönyeg",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 1000
                        },
                        new Question
                        {
                            Text = "Hány könyvből áll a Háború és béke? ",
                            Answer = "4",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 2000
                        },
                        new Question
                        {
                            Text = "Ki írt levelet Anyeginnek?",
                            Answer = "Tatjána",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 3000
                        },
                        new Question
                        {
                            Text = "Hogy hívják a Bűn és bűnhődés főszereplőjét?",
                            Answer = "Rogyion Romanovics Raszkolnyikov",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 4000
                        },
                        new Question
                        {
                            Text = "A lónevű, Ellenségek, Kaméleon, Vologya, A tokba bújt ember, A 6-os számú kórterem. Ki a novellák szerzője?",
                            Answer = "Anton Pavlovics Csehov",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 5000
                        },
                        new Question
                        {
                            Text = "Nobeldíjas orosz író. Az 1973-ban írt A Gulag szigetvilág című regénye miatt a Legfelsőbb Tanács megfosztotta szovjet állampolgárságától és kiutasította az országból.",
                            Answer = "Alekszandr Iszajevics Szolzsenyicin",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 8000
                        },
                    }
                },
                new Topic
                {
                    Author = _author,
                    Name = "Anyag",
                    Competences = new List<Competence>
                    {
                        new Competence
                        {
                            Name = "orvosi",
                        },
                    },

                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Text = "Mi a kokain alapanyaga?",
                            Answer = "kokacserje",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 1000
                        },
                        new Question
                        {
                            Text = "Minek a rövidítése az LSD?",
                            Answer = "lizergsav-dietilamid",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 2000
                        },
                        new Question
                        {
                            Text = "Hol fordul elő a természetben a mescalin?",
                            Answer = "kaktuszfélékben",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 3000
                        },
                        new Question
                        {
                            Text = "Miből készül a heroin?",
                            Answer = "mák, ópium",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 4000
                        },
                        new Question
                        {
                            Text = "Milyen drogot hívnak kristálynak?",
                            Answer = "metamfetamin",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 5000
                        },
                        new Question
                        {
                            Text = "A dél-amerikai őserdőkben honos indákat felaprítva lassú tűzön főzik. Az így kapott hallucinogén italt bennszülöttek sámán szertartásaihoz használták fel. Mi az ital neve?",
                            Answer = "ayahusca",
                            Resource = null,
                            Type = QuestionType.THEMATICAL,
                            Value = 8000
                        },
                    }
                },
            },
            Authors = new List<Player>
            {
                _author,
            },
            CreatedOn = DateTime.Now,
        };

        public static List<Player> PLayers => new List<Player>
        {
            new Player
            {
                Id = 0,
                Institue = "PSEG",
                Name = "Ömböli Csaba",
                NickName = "Lajos",
                //TestCommand = TestCommand,
            },
            new Player
            {
                Id = 1,
                Institue = "PSEG",
                Name = "Zareczky András",
                NickName = "Lajos",
                //TestCommand = TestCommand,
            },
            new Player
            {
                Id = 2,
                Institue = "PSEG",
                Name = "Zell Julian Maxim",
                NickName = "Lajos",
                //TestCommand = TestCommand,
            },

            new Player
            {
                Id = 3,
                Institue = "PSEG",
                Name = "Kiss Géza Ferenc Péter János",
                NickName = "Lajos",
                //TestCommand = TestCommand,
            },
            new Player
            {
                Id = 4,
                Institue = "PSEG",
                Name = "Németh Zoltán",
                NickName = "Lajos",
                //TestCommand = TestCommand,
            },
            new Player
            {
                Id = 5,
                Institue = "PSEG",
                Name = "Nagy Lajos",
                NickName = "Lajos",
                //TestCommand = TestCommand,
            },
            new Player
            {
                Id = 6,
                Institue = "PSEG",
                Name = "Kis Pippin",
                NickName = "Lajos",
                //TestCommand = TestCommand,
            },
            new Player
            {
                Id = 7,
                Institue = "PSEG",
                Name = "XVII. Lajos",
                NickName = "Lajos",
                //TestCommand = TestCommand,
            },
            new Player
            {
                Id = 8,
                Institue = "PSEG",
                Name = "Loius Bonaparte",
                NickName = "Lajos",
                //TestCommand = TestCommand,
            },
        };

        public static List<Team> Teams => new List<Team>()
        {
            new Team
            {
                Id = 0,
                Players = PLayers.GetRange(0,2),
                TeamName = "Csapat1",
            },
            new Team
            {
                Id = 0,
                Players = PLayers.GetRange(1,2),
                TeamName = "Csapat2",
            },
            new Team
            {
                Id = 0,
                Players = PLayers.GetRange(0,2),
                TeamName = "Csapat3",
            },
            new Team
            {
                Id = 0,
                Players = PLayers.GetRange(0,2),
                TeamName = "Csapat4",
            },
            new Team
            {
                Id = 0,
                Players = PLayers.GetRange(2,2),
                TeamName = "Csapat5",
            },
            new Team
            {
                Id = 0,
                Players = PLayers.GetRange(2,2),
                TeamName = "Csapat6",
            },
            new Team
            {
                Id = 0,
                Players = PLayers.GetRange(3,2),
                TeamName = "Csapat7",
            },
            new Team
            {
                Id = 0,
                Players = PLayers.GetRange(1,3),
                TeamName = "Csapat8",
            },
            new Team
            {
                Id = 0,
                Players = PLayers.GetRange(4,2),
                TeamName = "Csapat9",
            },
        };

    }
}