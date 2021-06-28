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

}
