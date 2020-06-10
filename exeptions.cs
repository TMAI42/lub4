using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace final {
    class gameEx : Exception {
        public gameEx(string Messenge) : base(Messenge) {}
    }

    class Player {
        public string Name { get; }
        public int Age { get; }
        public int Lvl { get; }


        public Player(string Name) {
            this.Name = Name;
            Random r = new Random();
            this.Age = r.Next(18, 100);
            this.Lvl = r.Next(0, 100);
        }

    }

    class Trainer {
        public string secondname { get; }
        public double luck { get; }

        public Trainer(string Name) {
            this.secondname = Name;
            Random r2 = new Random();
            this.luck = r2.Next(-5, 5) / 10.0 + 1;
        }

    }

    class Team {
        public string Name { get; }
        public Trainer trainer;
        public double TeamLvl { get; set; }
        public List<Player> Players = new List<Player>();
        public Team(string Name) {
            this.Name = Name;
            this.TeamLvl = 0;
        }
        public void Add(Player p) {
            if (trainer != null) {
                this.Players.Add(p);
                this.TeamLvl = ((this.TeamLvl) / trainer.luck + p.Lvl) * trainer.luck;
            }
            else {
                this.Players.Add(p);
                this.TeamLvl += p.Lvl;
            }
        }
        public void AddTrainer(Trainer t) {
            this.trainer = t;
            this.TeamLvl *= t.luck;
            Console.WriteLine(TeamLvl);
        }

        public void PlayersSort() {
            var temp = from p in Players
                       orderby p.Name
                       select p.Name;
            foreach (string p in temp) {
                Console.WriteLine(p);
            }

        }

        public void PlayerOld() {
            var temp = from p in Players
                       where p.Age >= 30
                       orderby p.Lvl descending
                       select p;
            foreach (Player p in temp) {
                Console.WriteLine(p.Name + " " + p.Age + " " + p.Lvl);
            }
        }

    }

    class Game {
        private List<Team> teams = new List<Team>();

        public Judge judge;

        public event EventHandler Goal;
        public event EventHandler Cheating;

        public Game(Team team1, Team team2) {
            this.teams.Add(team1);
            this.teams.Add(team2);
            this.teams.Sort((a, b) => { return (a.TeamLvl < b.TeamLvl) ? 0 : 1; });
        }

        public void AddJudge(Judge j) {
            this.judge = j;
            if (j.pref == 1) {
                teams[0].TeamLvl += 10;
            }
            else if (j.pref == 2) {
                teams[1].TeamLvl += 10;

            }
        }

        public void Play() {
            if(judge == null) { throw new gameEx("судья пропал"); }
            if (!(teams[0].Players.Count() == teams[1].Players.Count())) {
                Console.WriteLine("Количество игроков не равно между собой!");
            }
            else {
                Console.WriteLine("команда " + teams[0].Name + " уровень " + teams[0].TeamLvl);
                Console.WriteLine("команда " + teams[1].Name + " уровень " + teams[1].TeamLvl);

                if (Convert.ToDouble(teams[0].TeamLvl) / (Convert.ToDouble(teams[1].TeamLvl)) >= 0.9) {
                    Console.WriteLine("ничья");
                }
                else {
                    Console.WriteLine("победила команда " + teams[1].Name);
                }

            }
        }

        public void GameStart() {
            EventArgs e = new EventArgs();
            Goal?.Invoke(this, e);
            Cheating?.Invoke(this, e);
            throw new gameEx("мяч лопнул!");
        }


    }

    class Judge {
        public string SecondName { get; }

        public int pref { get; }
        public Judge(string SecondName) {
            this.SecondName = SecondName;
            Random r3 = new Random();
            this.pref = r3.Next(0, 2);
        }

        public void Straf(object sender, EventArgs e) {
            Console.WriteLine("желтая карточка!!");
        }

        public void gg(object sender, EventArgs e) {
            Console.WriteLine("Гол!!");
        }
    }

    class Program {
        static void Main(string[] args) {
            try {
                Player a = new Player("a");
                Player b = new Player("b");
                Player c = new Player("c");
                Player d = new Player("d");
                Player e = new Player("e");
                Player f = new Player("f");
                Player g = new Player("g");
                Player h = new Player("h");
                Trainer ssss = new Trainer("ssss");
                Trainer eeee = new Trainer("eeee");
                Team first = new Team("First");
                Team second = new Team("second");
                first.Add(a);
                first.Add(b);
                first.Add(c);
                first.Add(d);
                second.Add(e);
                second.Add(f);
                second.Add(g);
                second.Add(h);
                first.AddTrainer(ssss);
                second.AddTrainer(eeee);
                Judge j = new Judge("j");
                Game qqq = new Game(first, second);
                qqq.AddJudge(j);
                qqq.Cheating += j.Straf;
                qqq.Goal += j.gg;
                first.PlayerOld();
                second.PlayersSort();
                qqq.Play();
                qqq.GameStart(); 
            }
            catch (gameEx e) {
                Console.WriteLine("игра приостановленая, " + e.Message);
            }
            //first.PlayerOld();
            //second.PlayersSort();



        }
    }
}
