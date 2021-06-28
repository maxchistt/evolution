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
using LiveCharts;
using LiveCharts.Wpf;

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

            graph_Angry.Values = new ChartValues<int>();
            graph_Peaceful.Values = new ChartValues<int>();
        }

        private void simData_displayToOutput()
        {
            int angry = simulation.getAmountEntities(true);
            int peaceful = simulation.getAmountEntities(false);
            int day = simulation.getDay();

            graph_Peaceful.Values.Add(peaceful);
            graph_Angry.Values.Add(angry);

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
            simData_updateDynamic();
        }

        public void simData_PushAnimals()
        {
            int ang, pea;
            ang = Convert.ToInt32(input_Angry.Text);
            pea = Convert.ToInt32(input_Peaceful.Text);
            simulation.pushAnimals(ang, pea);
            //добавить кнопку пушер
        }

        public void simData_updateDynamic()
        {
            int fam = Convert.ToInt32(input_Food.Text);
            double foodInMatch, angryAngryFightHarmPercent, angryPecefulAngrysPartPercent;
            //добавить инпуты, и обработку данных

            //simulation.updateDinamicData(fam, foodInMatch, angryAngryFightHarmPercent, angryPecefulAngrysPartPercent);
        }

        public void sim_New()
        {
            simulation.Pause();
            graph_Peaceful.Values.Clear();
            graph_Angry.Values.Clear();
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
