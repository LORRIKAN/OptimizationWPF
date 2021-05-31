using DevExpress.Xpf.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Math;

namespace OptimizationWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string NL = Environment.NewLine;
        public MainWindow()
        {
            InitializeComponent();
            restrictionsTB.IsReadOnly = true;
            formulaTB.IsReadOnly = true;
            restrictionsTB.Text = $", где: a=a1=B=B1=u=u1=1{NL}N=2{NL}V1=11{NL}V2=7{NL}1⩽A1⩽10{NL}1⩽A2⩽10{NL}4*A1+5*A2⩽20";
        }

        const double a = 1, a1 = 1, B = 1, B1 = 1, u = 1, u1 = 1, N = 2, V1 = 11, V2 = 7;

        Vertex[] vx;
        private async void calculateBtn_Click(object sender, RoutedEventArgs e)
        {
            pointStore.Points.Clear();

            if (!double.TryParse(solutionAccuracy.Text, out double accuracy) || accuracy <= 0)
            {
                MessageBox.Show("Точность решения задана неверно или не задана вовсе.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool RestrictionOfSecondKind(Variable[] variables)
            {
                double A1 = variables.First(v => v.VariableInfo.Name == "A1");
                double A2 = variables.First(v => v.VariableInfo.Name == "A2");

                if (4 * A1 + 5 * A2 <= 20)
                    return true;
                else
                    return false;
            }

            double C(Variable[] variables)
            {
                double A1 = variables.First(v => v.VariableInfo.Name == "A1");
                double A2 = variables.First(v => v.VariableInfo.Name == "A2");

                return 1000 * (a * Pow(Pow(A1, 2) + B * A2 - u * V1, N) +
                    a1 * Pow(B1 * A1 + Pow(A2, 2) - u1 * V2, N));
            }

            int BestValue(IEnumerable<double> values)
            {
                double bestValue = values.ElementAt(0);
                int bestValueIndex = 0;

                for (int i = 0; i < values.Count(); i++)
                {
                    double value = values.ElementAt(i);

                    if (value < bestValue)
                    {
                        bestValue = value;
                        bestValueIndex = i;
                    }
                }

                return bestValueIndex;
            }

            int WorstValue(IEnumerable<double> values)
            {
                double worstValue = values.ElementAt(0);
                int worstValueIndex = 0;

                for (int i = 0; i < values.Count(); i++)
                {
                    double value = values.ElementAt(i);

                    if (value > worstValue)
                    {
                        worstValue = value;
                        worstValueIndex = i;
                    }
                }

                return worstValueIndex;
            }

            ITargetFunction targetFunction = new TargetFunction(
                new Func<Variable[], bool>[]
                {
                RestrictionOfSecondKind
                },
                new VariableInfo[]
                {
                    new VariableInfo { Name = "A1", ValueLowerBound = 1, ValueUpperBound = 10 },
                    new VariableInfo { Name = "A2", ValueLowerBound = 1, ValueUpperBound = 10 }
                },
                C,
                BestValue,
                WorstValue
                );

            ISolutionMethod methodOfBox = SolutionMethodsFactory.ConstructSolutionMethodOfBox();

            var solution = methodOfBox.GetSolution(targetFunction, 0.1);

            pointStore.Points.AddRange(vx.Select(v => new SeriesPoint3D(v.x, v.y, v.z)));

            MessageBox.Show($"MinC={minC} при A1={minA1} и A2={minA2}");

            chart2DBtn.IsEnabled = true;
            tableBtn.IsEnabled = true;
        }

        private void GoAdminBtn_Click(object sender, RoutedEventArgs e)
        {
            Authorize authorize = new Authorize();
            authorize.Show();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combobox.SelectedIndex != 0)
            {
                MessageBox.Show("Приносим свои извинения, функция находиться в разработке.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
                combobox.SelectedIndex = 0;
            }
        }

        private void chart2DBtn_Click(object sender, RoutedEventArgs e)
        {
            Chart2D crt = new Chart2D(vx);
            crt.Show();
        }

        private void tableBtn_Click(object sender, RoutedEventArgs e)
        {
            Table tb = new Table(vx);
            tb.Show();
        }

        private void reportBtn_Click(object sender, RoutedEventArgs e)
        {
            chart.Print();
        }
    }
}
