using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;

namespace evolution
{
    public class Simulation
    {
        private Timer timer;
        private MatchField matchField;
        private Population population;
        private int day;

        public Simulation()
        {
            day = 0;
            timer = new Timer();
            addTimerHandler(playDay);
        }

        private void playDay()
        {
            //refresh match arr
            matchField.refresh();

            //fill animals to match arr
            population.fillMatches(ref matchField.matchArr);

            //fight matches
            matchField.fightMatches();

            //new day, hungry animals die, feeded animals reproduse
            population.lifecicle();

            //count new day
            day++;
        }

        public void setNewSim(int angry_amount, int peaceful_amount, int food_amount)
        {
            day = 0;
            matchField = new MatchField(food_amount);
            population = new Population(angry_amount, peaceful_amount);
        }

        public void addTimerHandler(Action onNewDay)
        {
            timer.addHandler(onNewDay);
        }

        public void Start()
        {
            timer.Start();
        }

        public void Pause()
        {
            timer.Stop();
        }

        public int getAmountEntities(bool angryType)
        {
            int amount = 0;
            foreach (var item in population.animalArr)
            {
                if (item.angryMod == angryType) amount++;
            }
            return amount;
        }

        public int getDay()
        {
            return day;
        }
    }

    public class Population
    {
        private Random rnd;
        public List<Animal> animalArr;

        public Population(int angry_amount, int peaceful_amount)
        {
            rnd = new Random();
            animalArr = new List<Animal>();
            //first fill of animal arr
            for (int i = 0; i < peaceful_amount; i++)
            {
                animalArr.Add(new Animal(false));
            }
            for (int i = 0; i < angry_amount; i++)
            {
                animalArr.Add(new Animal(true));
            }
            //shuffle animals
            shuffle();
        }

        public void lifecicle()
        {
            //fill new day aninal list
            List<Animal> newDayAnimalArr = new List<Animal>();
            for (int i = 0; i < animalArr.Count; i++)
            {
                newDayAnimalArr.AddRange(animalArr[i].returnAfterDay());
            }
            animalArr = newDayAnimalArr;
            //shuffle animals
            shuffle();
        }

        public void fillMatches(ref Match[] matchArrRef)
        {
            //set refs
            for (int ind = 0; ind < matchArrRef.Length; ind++)
            {
                matchArrRef[ind].setAnimalArrRef(ref animalArr);
            }
            //fill animals to match arr
            for (int i = 0; i < animalArr.Count; i++)
            {
                //get index list of unfilled matches
                List<int> numArr = new List<int>();
                for (int ind = 0; ind < matchArrRef.Length; ind++)
                {
                    if (matchArrRef[ind].getCount() < 2)
                    {
                        numArr.Add(ind);
                    }
                }
                //takes up unfilled match
                if (numArr.Count > 0)
                {
                    int mNum = numArr[rnd.Next(numArr.Count)];
                    matchArrRef[mNum].addAnimalIndex(i);
                }
            }
        }

        private void shuffle()
        {
            //shuffle animals
            List<Animal> shuffled = new List<Animal>();
            foreach (Animal s in animalArr)
            {
                int j = rnd.Next(shuffled.Count + 1);
                if (j == shuffled.Count)
                {
                    shuffled.Add(s);
                }
                else
                {
                    shuffled.Add(shuffled[j]);
                    shuffled[j] = s;
                }
            }
            animalArr = shuffled;
        }

    }

    public class Animal
    {
        private Random rnd;
        public bool angryMod;
        public double food;

        public double food_to_feed = 2;

        public Animal(bool Angry)
        {
            rnd = new Random();
            angryMod = Angry;
            food = 0;
        }

        public List<Animal> returnAfterDay()
        {
            List<Animal> res = new List<Animal>();

            double step1 = food_to_feed / 2;
            double step2 = food_to_feed;

            //шанс выживания
            double probability1 = food / step1 * 100;
            bool chanse1 = rnd.Next(100) < probability1;
            if (chanse1) res.Add(this);

            //шанс размножения
            double probability2 = (food - step1) / (step2 - step1) * 100;
            bool chanse2 = rnd.Next(100) < probability2;
            if (chanse2) res.Add(new Animal(angryMod));

            food = 0;
            return res;
        }

    }

    public class MatchField
    {
        public Match[] matchArr;

        public MatchField(int food_amount)
        {
            matchArr = new Match[food_amount];
        }

        public void refresh()
        {
            //refresh match arr and set animals list ref to refreshed matches
            for (int i = 0; i < matchArr.Length; i++)
            {
                matchArr[i] = new Match();
            }
        }

        public void fightMatches()
        {
            //fight matches
            for (int i = 0; i < matchArr.Length; i++)
            {
                matchArr[i].fight();
            }
        }
    }

    public class Match
    {
        private List<int> matchAnimalIndexes; /// поменять
        private List<Animal> animalArrRef;

        public double food_in_match = 2;
        public double angry_angry_FightWinersPartPercent = 0;
        public double angry_peceful_AngrysPartPercent = 75;

        public Match()
        {
            matchAnimalIndexes = new List<int>();
        }

        public void setAnimalArrRef(ref List<Animal> animArrRef)
        {
            animalArrRef = animArrRef;
        }

        public void addAnimalIndex(int animalIndex)
        {
            if (getCount() < 2) { matchAnimalIndexes.Add(animalIndex); }
        }

        public int getCount()
        {
            return matchAnimalIndexes.Count;
        }

        public void fight()
        {
            if (getCount() == 1)
            {
                animalArrRef[matchAnimalIndexes[0]].food += food_in_match;
            }
            else if (getCount() > 1)
            {
                bool aMod0 = animalArrRef[matchAnimalIndexes[0]].angryMod;
                bool aMod1 = animalArrRef[matchAnimalIndexes[1]].angryMod;

                if (aMod0 != aMod1)
                {
                    int winnerNum = (aMod0) ? 0 : 1;
                    int looserNum = (winnerNum == 1) ? 0 : 1;
                    animalArrRef[matchAnimalIndexes[winnerNum]].food += food_in_match * angry_peceful_AngrysPartPercent / 100;
                    animalArrRef[matchAnimalIndexes[looserNum]].food += food_in_match * (100 - angry_peceful_AngrysPartPercent) / 100;
                }
                else if (aMod0 == true)
                {
                    animalArrRef[matchAnimalIndexes[0]].food += food_in_match / 100 * angry_angry_FightWinersPartPercent;
                }
                else if (aMod0 == false)
                {
                    animalArrRef[matchAnimalIndexes[0]].food += food_in_match / 2;
                    animalArrRef[matchAnimalIndexes[1]].food += food_in_match / 2;
                }
            };
        }
    }

    public class Timer : DispatcherTimer
    {
        public Timer()
        {
            Interval = new TimeSpan(0, 0, 0, 0, 500);
            Stop();
        }

        public void addHandler(Action handler)
        {
            Tick += new EventHandler((object sender, EventArgs e) =>
            {
                handler();
            });
        }
    }
}
