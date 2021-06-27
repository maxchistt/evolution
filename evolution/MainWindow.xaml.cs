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
        public DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    public class Simulation
    {
        Random rnd = new Random();
        Match[] matchArr;
        List<Animal> animalArr = new List<Animal>();

        public int day = 0;

        public Simulation(int angry_amount, int peaceful_amount, int food_amount)
        {
            this.matchArr = new Match[food_amount];

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

/*
namespace evolution_OLD
{
    public partial class Form1 : Form
    {
        public Simulation sim;

        public Form1()
        {
            InitializeComponent();
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            sim.playDay();
            updSimInfo();
            //Refresh();
        }

        void updSimInfo()
        {
            int angry = sim.getInfo(true);
            int peaceful = sim.getInfo(false);
            int day = sim.day;

            this.label1.Text = angry.ToString();
            this.label2.Text = peaceful.ToString();
            this.label10.Text = day.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void newSimFromInput()
        {
            int ang, pea, fam;
            ang = Convert.ToInt32(textBox1.Text);
            pea = Convert.ToInt32(textBox2.Text);
            fam = Convert.ToInt32(textBox3.Text);
            sim = new Simulation(ang, pea, fam);
        }

        private void button_new_sim_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            newSimFromInput();
            updSimInfo();
        }
        private void button_start_Click(object sender, EventArgs e)
        {
            if (sim == null)
            {
                timer1.Stop();
                newSimFromInput();
                updSimInfo();
            }

            timer1.Start();
        }

        private void button_pause_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
    }


}
*/
