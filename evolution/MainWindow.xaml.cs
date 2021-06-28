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

namespace evolution
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private Simulation simulation;

        public MainWindow()
        {
            InitializeComponent();
            simulation = new Simulation();
            simulation.addTimerHandler(simData_displayToOutput);
        }

        private void simData_displayToOutput()
        {
            int angry = simulation.getAmountEntities(true);
            int peaceful = simulation.getAmountEntities(false);
            int day = simulation.getDay();

            label_Angry.Content = angry.ToString();
            label_Peaceful.Content = peaceful.ToString();
            label_Day.Content = day.ToString();
        }

        private void simData_setNewFromInput()
        {
            int ang, pea, fam;
            ang = Convert.ToInt32(input_Angry.Text);
            pea = Convert.ToInt32(input_Peaceful.Text);
            fam = Convert.ToInt32(input_Food.Text);
            simulation.setNewSim(ang, pea, fam);
        }

        public void sim_New()
        {
            simulation.Pause();
            simData_setNewFromInput();
            simData_displayToOutput();
        }

        public void sim_Start()
        {
            if (simulation.getDay() == 0)
            {
                sim_New();
            }

            simulation.Start();
        }

        public void sim_Pause()
        {
            simulation.Pause();
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
}
