using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace evolution
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public Timer timer;
        public Simulation sim;

        public MainWindow()
        {
            InitializeComponent();
            timer = new Timer(timer_Tick);
        }

        public void simData_displayToOutput()
        {
            int angry = sim.getInfo(true);
            int peaceful = sim.getInfo(false);
            int day = sim.day;

            label_Angry.Content = angry.ToString();
            label_Peaceful.Content = peaceful.ToString();
            label_Day.Content = day.ToString();
        }

        public void simData_setNewFromInput()
        {
            int ang, pea, fam;
            ang = Convert.ToInt32(input_Angry.Text);
            pea = Convert.ToInt32(input_Peaceful.Text);
            fam = Convert.ToInt32(input_Food.Text);
            sim = new Simulation(ang, pea, fam);
        }

        public void sim_PlayDay()
        {
            sim.playDay();
            simData_displayToOutput();
        }

        public void sim_New()
        {
            timer.Stop();
            simData_setNewFromInput();
            simData_displayToOutput();
        }

        public void sim_Start()
        {
            if (sim == null)
            {
                timer.Stop();
                simData_setNewFromInput();
                simData_displayToOutput();
            }
            timer.Start();
        }

        public void sim_Pause()
        {
            timer.Stop();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            sim_PlayDay();
        }

        private void btn_NewSim_Click(object sender, RoutedEventArgs e)
        {
            sim_New();
        }

        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            sim_Start();
        }

        private void btn_Pause_Click(object sender, RoutedEventArgs e)
        {
            sim_Pause();
        }

    }

    public class Timer : DispatcherTimer
    {
        public Timer(EventHandler handler)
        {
            Tick += new EventHandler(handler);
            Interval = new TimeSpan(0, 0, 1);
            Stop();
        }
    }

    public class Simulation
    {
        Random rnd;
        Match[] matchArr;
        List<Animal> animalArr;

        public int day = 0;

        public Simulation(int angry_amount, int peaceful_amount, int food_amount)
        {
            rnd = new Random();
            matchArr = new Match[food_amount];
            animalArr = new List<Animal>();

            //first fill of animal arr
            for (int i = 0; i < peaceful_amount + angry_amount; i++)
            {
                animalArr.Add(new Animal(i < angry_amount ? true : false));
            }
        }

        public void playDay()
        {
            List<Animal> newDayAnimalArr = new List<Animal>();

            //refresh match arr
            for (int i = 0; i < matchArr.Length; i++)
            {
                matchArr[i] = new Match();
            }

            //fill matches
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < matchArr.Length; i++)
                {
                    if (animalArr.Count > 0)
                    {
                        int aNum = rnd.Next(animalArr.Count);
                        matchArr[i].add(animalArr[aNum]);
                        animalArr.RemoveAt(aNum);

                    }
                }
            }

            //get results
            for (int i = 0; i < matchArr.Length; i++)
            {
                List<Animal> addList = matchArr[i].getResult();
                newDayAnimalArr.AddRange(addList);
            }

            //apply results
            this.animalArr = newDayAnimalArr;
            day++;
        }

        public int getInfo(bool angry)
        {
            int amount = 0;
            foreach (var item in animalArr)
            {
                if (item.angryMod == angry) amount++;
            }
            return amount;
        }
    }

    public class Animal
    {
        public bool angryMod;
        public Animal(bool Angry)
        {
            this.angryMod = Angry;
        }
    }

    public class Match
    {
        List<Animal> matchAnimals = new List<Animal>();
        Random rnd = new Random();
        public Match()
        {

        }

        public void add(Animal animal)
        {
            matchAnimals.Add(animal);
        }

        public List<Animal> getResult()
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
                    if (rnd.Next(2) > 0) res.Add(matchAnimals[winnerNum]);//winner children

                }
                else if (aMod0 == true & aMod1 == true)
                {
                    res.Add(matchAnimals[rnd.Next(2)]);//fight winner

                }
                else if (aMod0 == false & aMod1 == false)
                {
                    res.Add(matchAnimals[0]);//animal1
                    if (rnd.Next(2) > 0) res.Add(matchAnimals[0]);//children1
                    res.Add(matchAnimals[1]);//animal2
                    if (rnd.Next(2) > 0) res.Add(matchAnimals[1]);//children2
                }
            };
            return res;
        }
    }
}
