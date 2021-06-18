using DevExpress.Xpf.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        Dictionary<List<Variable>, double> FuncResults { get; set; }

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

            foreach (Button button in FindVisualChildren<Button>(this))
            {
                button.IsEnabled = false;
            }

            var solutionTask = Task.Run(() => methodOfBox.GetSolution(targetFunction, accuracy));

            var funcResultsTask = Task.Run(() =>
            {
                bool CheckVariablesValues(List<Variable> variables)
                {
                    if (variables.Any(vsv => vsv.Value < vsv.VariableInfo.ValueLowerBound
                        || vsv.Value > vsv.VariableInfo.ValueUpperBound)
                        || targetFunction.RestrictionsOfSecondKind
                            .Any(r => r(variables.ToArray()) is false))
                        return false;
                    else
                        return true;
                }

                var funcResults = new Dictionary<List<Variable>, double>();

                for (int i = 0; i < targetFunction.VariablesInfo.Length - 1; i++)
                {
                    List<Variable> variablesValues = targetFunction.VariablesInfo
                    .Select(vi => new Variable(vi, vi.ValueLowerBound + accuracy)).ToList();

                    while (true)
                    {
                        while (true)
                        {
                            if (CheckVariablesValues(variablesValues) is true)
                            {
                                funcResults.Add(variablesValues, targetFunction.Func(variablesValues.ToArray()));
                            }
                            else
                                break;

                            variablesValues = variablesValues
                                .Select((Variable v, int j) => new { j, v })
                                .Select(vi =>
                                {
                                    if (vi.j == i)
                                        return vi.v;
                                    else
                                        return new Variable(vi.v.VariableInfo, vi.v.Value + accuracy);
                                }).ToList();
                        }

                        variablesValues = variablesValues
                            .Select((Variable v, int j) => new { j, v })
                            .Select(vi =>
                            {
                                if (vi.j == i)
                                    return new Variable(vi.v.VariableInfo, vi.v.Value + accuracy);
                                else
                                    return new Variable(vi.v.VariableInfo, vi.v.VariableInfo.ValueLowerBound
                                        + accuracy);
                            }).ToList();

                        if (CheckVariablesValues(variablesValues) is false)
                            break;
                    }
                }

                return funcResults;
            });

            var solution = await solutionTask;

            FuncResults = await funcResultsTask;

            var points3DList = await Task.Run(() =>

            {
                if (FuncResults.Keys.All(l => l.Count == 2))
                    return FuncResults
                        .Select(f => new SeriesPoint3D(f.Key[0], f.Key[1], f.Value)).ToList();
                else
                    return new List<SeriesPoint3D>();
            }
            );

            double targetValue = solution.Item2;

            Variable[] targetVariables = solution.Item1;

            string resultStr = $"MinS={targetValue:0.00} при";

            foreach (Variable variable in targetVariables)
            {
                resultStr += $" {variable.VariableInfo.Name}={variable.Value:0.00}";
            }

            if (FuncResults.Keys.All(f => f.Count == 2))
                pointStore.Points.AddRange(points3DList);

            chart3D.XAxis = new XAxis3D
            {
                Label =
                    new AxisLabel { Name = FuncResults.Keys.ElementAt(0)[0].VariableInfo.Name }
            };

            chart3D.YAxis = new YAxis3D
            {
                Label =
                    new AxisLabel { Name = FuncResults.Keys.ElementAt(0)[1].VariableInfo.Name }
            };

            chart3D.ZAxis = new ZAxis3D
            {
                Label =
                    new AxisLabel { Name = "S" }
            };

            MessageBox.Show(resultStr);

            foreach (Button button in FindVisualChildren<Button>(this))
            {
                button.IsEnabled = true;
            }

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
            if (FuncResults.Any(fr => fr.Key.Count != 2))
            {
                MessageBox.Show("2D-график для функции не двух переменных недоступен");
                return;
            }

            var points = new List<Tuple<double, double, double>>();

            foreach (KeyValuePair<List<Variable>, double> result in FuncResults)
            {
                points.Add(new Tuple<double, double, double>(
                    result.Key[0], result.Key[1], result.Value));
            }

            var crt = new Plot2D(points, points.Select(r => r.Item3).OrderBy(f => f).ToList());
            crt.Show();
        }

        private void tableBtn_Click(object sender, RoutedEventArgs e)
        {
            Table tb = new Table(FuncResults);
            tb.Show();
        }

        private void reportBtn_Click(object sender, RoutedEventArgs e)
        {
            chart3D.Print();
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T t)
                    {
                        yield return t;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}