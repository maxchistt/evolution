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
            List<Animal> newDayAnimalArr = new List<Animal>();

            //refresh match arr
            for (int i = 0; i < matchArr.Length; i++)
            {
                matchArr[i] = new Match();
            }

            //shuffle animal arr
            List<Animal> data = new List<Animal>();
            foreach (Animal s in animalArr)
            {
                int j = rnd.Next(data.Count + 1);
                if (j == data.Count)
                {
                    data.Add(s);
                }
                else
                {
                    data.Add(data[j]);
                    data[j] = s;
                }
            }

            //fill matches
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
                if (numArr.Count>0)
                {
                    int mNum = numArr[rnd.Next(numArr.Count)];
                    matchArr[mNum].add(animalArr[i]);
                }
            }

            //get results
            for (int i = 0; i < matchArr.Length; i++)
            {
                List<Animal> addList = matchArr[i].fightAndGetResult();
                newDayAnimalArr.AddRange(addList);
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
        public bool angryMod;
        public Animal(bool Angry)
        {
            angryMod = Angry;
        }
    }

    public class Match
    {
        private List<Animal> matchAnimals;
        private Random rnd;

        public Match()
        {
            matchAnimals = new List<Animal>();
            rnd = new Random();
        }

        public void add(Animal animal)
        {
            matchAnimals.Add(animal);
        }

        public int getCount()
        {
            return matchAnimals.Count;
        }

        public List<Animal> fightAndGetResult()
        {
            List<Animal> res = new List<Animal>();

            if (matchAnimals.Count == 1)
            {
                res.Add(matchAnimals[0]);
                res.Add(matchAnimals[0]);//children
            }
            else if (matchAnimals.Count > 1)
            {
                bool aMod0 = matchAnimals[0].angryMod;
                bool aMod1 = matchAnimals[1].angryMod;

                if (aMod0 != aMod1)
                {
                    int winnerNum = (aMod0) ? 0 : 1;
                    int loserNum = (aMod0) ? 1 : 0;

                    if (rnd.Next(2) > 0) res.Add(matchAnimals[loserNum]);//loser
                    res.Add(matchAnimals[winnerNum]);//winner 
                    if (rnd.Next(2) > 0) res.Add(new Animal(matchAnimals[winnerNum].angryMod));//winner children

                }
                else if (aMod0 == true)
                {
                    res.Add(matchAnimals[rnd.Next(2)]);//fight winner

                }
                else if (aMod0 == false)
                {
                    res.Add(matchAnimals[0]);//animal1
                    if (rnd.Next(2) > 0) res.Add(new Animal(matchAnimals[0].angryMod));//children1
                    res.Add(matchAnimals[1]);//animal2
                    if (rnd.Next(2) > 0) res.Add(new Animal(matchAnimals[1].angryMod));//children2
                }
            };
            return res;
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
