using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;

namespace evolution
{
    public class Simulation
    {
        private Timer timer;
        private Random rnd;
        private Match[] matchArr;
        private List<Animal> animalArr;
        private int day;

        public Simulation()
        {
            day = 0;
            timer = new Timer();
            rnd = new Random();
            addTimerHandler(playDay);
        }

        private void playDay()
        {
            //refresh match arr
            for (int i = 0; i < matchArr.Length; i++)
            {
                matchArr[i] = new Match();
            }

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

            //set animals list ref to refreshed matches
            for (int i = 0; i < matchArr.Length; i++)
            {
                matchArr[i].setAnimalsRef(ref animalArr);
            }

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

            //fight matches
            for (int i = 0; i < matchArr.Length; i++)
            {
                matchArr[i].fight();
            }

            //fill new day aninal list
            List<Animal> newDayAnimalArr = new List<Animal>();
            for (int i = 0; i < animalArr.Count; i++)
            {
                newDayAnimalArr.AddRange(animalArr[i].returnAfterDay());
            }

            //apply results
            animalArr = newDayAnimalArr;
            day++;
        }

        public void setNewSim(int angry_amount, int peaceful_amount, int food_amount)
        {
            day = 0;
            matchArr = new Match[food_amount];
            animalArr = new List<Animal>();
            //first fill of animal arr
            for (int i = 0; i < peaceful_amount + angry_amount; i++)
            {
                animalArr.Add(new Animal(i < angry_amount ? true : false));
            }
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
            foreach (var item in animalArr)
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
