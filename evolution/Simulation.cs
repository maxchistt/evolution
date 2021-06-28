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
            //refresh match arr and set animals list ref to refreshed matches
            matchField.refreshAndSetRef(ref population);

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

        public void fillMatches(ref Match[] matchArr)
        {
            //fill animals to match arr
            for (int i = 0; i < animalArr.Count; i++)
            {
                List<int> numArr = new List<int>();
                for (int ind = 0; ind < matchArr.Length; ind++)
                {
                    if (matchArr[ind].getCount() < 2)
                    {
                        numArr.Add(ind);
                    }
                }
                if (numArr.Count > 0)
                {
                    int mNum = numArr[rnd.Next(numArr.Count)];
                    matchArr[mNum].add(i);

                }
            }
        }

        public void shuffle()
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
        public int food;

        public Animal(bool Angry)
        {
            rnd = new Random();
            angryMod = Angry;
            food = 0;
        }

        public List<Animal> returnAfterDay()
        {
            List<Animal> res = new List<Animal>();
            switch (food)
            {
                case 0:
                    break;
                case 1:
                    if (rnd.Next(2) > 0) res.Add(this);
                    break;
                case 2:
                    res.Add(this);
                    break;
                case 3:
                    res.Add(this);
                    if (rnd.Next(2) > 0) res.Add(new Animal(angryMod));
                    break;
                case 4:
                    res.Add(this);
                    res.Add(new Animal(angryMod));
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            food = 0;
            return res;
        }

    }

    public class MatchField
    {
        private Random rnd;
        public Match[] matchArr;

        public MatchField(int food_amount)
        {
            rnd = new Random();
            matchArr = new Match[food_amount];
        }

        public void refreshAndSetRef(ref Population population)
        {
            //refresh match arr and set animals list ref to refreshed matches
            for (int i = 0; i < matchArr.Length; i++)
            {
                matchArr[i] = new Match();
                matchArr[i].setAnimalsRef(ref population.animalArr);
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
        private List<int> matchAnimalIndexes;
        private List<Animal> animalArrRef;
        public int food;

        public Match()
        {
            food = 4;
            matchAnimalIndexes = new List<int>();
            animalArrRef = new List<Animal>();
        }

        public void setAnimalsRef(ref List<Animal> arrRef)
        {
            animalArrRef = arrRef;
        }

        public void add(int animalIndex)
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
                animalArrRef[matchAnimalIndexes[0]].food += food;
            }
            else if (getCount() > 1)
            {
                bool aMod0 = animalArrRef[matchAnimalIndexes[0]].angryMod;
                bool aMod1 = animalArrRef[matchAnimalIndexes[1]].angryMod;

                if (aMod0 != aMod1)
                {
                    int winnerNum = (aMod0) ? 0 : 1;
                    int looserNum = (winnerNum == 1) ? 0 : 1;
                    animalArrRef[matchAnimalIndexes[winnerNum]].food += (food * 3) / 4;
                    animalArrRef[matchAnimalIndexes[looserNum]].food += food / 4;
                }
                else if (aMod0 == true)
                {
                    //animalArrRef[matchAnimalIndexes[0]].food += food / 4;
                    //animalArrRef[matchAnimalIndexes[1]].food += food / 4;
                }
                else if (aMod0 == false)
                {
                    animalArrRef[matchAnimalIndexes[0]].food += food / 2;
                    animalArrRef[matchAnimalIndexes[1]].food += food / 2;
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
