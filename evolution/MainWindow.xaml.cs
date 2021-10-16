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

        private void catchInputDataErr(Exception e)
        {
            INPUTdata.Header = "INPUT data" + "     Error: " + e.Message;
        }

        private void simData_setNewFromInput()
        {
            try
            {
                int ang, pea, foodAmount;
                ang = Convert.ToInt32(input_Angry.Text);
                pea = Convert.ToInt32(input_Peaceful.Text);
                foodAmount = Convert.ToInt32(input_Food.Text);
                simulation.setNewSim(ang, pea, foodAmount);
            }
            catch (Exception e)
            {
                catchInputDataErr(e);
            }
        }

        public void simData_PushAnimals()
        {
            try
            {
                int ang, pea;
                ang = Convert.ToInt32(input_Angry.Text);
                pea = Convert.ToInt32(input_Peaceful.Text);
                simulation.pushAnimals(ang, pea);
            }
            catch (Exception e)
            {
                catchInputDataErr(e);
            }
        }

        public void simData_updateDynamic()
        {
            try
            {
                int foodAmount = Convert.ToInt32(input_Food.Text);
                double feedInMatchFood, angryAngryFightHarmPercent, angryPecefulAngrysPartPercent;
                feedInMatchFood = Convert.ToDouble(input_Feed.Text);
                angryAngryFightHarmPercent = Convert.ToDouble(input_Harm.Text);
                angryPecefulAngrysPartPercent = Convert.ToDouble(input_Part.Text);

                simulation.updateDinamicData(foodAmount, feedInMatchFood, angryAngryFightHarmPercent, angryPecefulAngrysPartPercent);
            }
            catch (Exception e)
            {
                catchInputDataErr(e);
            }
        }

        public void sim_New()
        {
            simulation.Pause();
            graph_Peaceful.Values.Clear();
            graph_Angry.Values.Clear();
            simData_setNewFromInput();
            simData_updateDynamic();
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

        private void btn_PushAnimals_Click(object sender, RoutedEventArgs e)
        {
            simData_PushAnimals();
        }

        private void btn_Update_Click(object sender, RoutedEventArgs e)
        {
            simData_updateDynamic();
        }
    }
}
