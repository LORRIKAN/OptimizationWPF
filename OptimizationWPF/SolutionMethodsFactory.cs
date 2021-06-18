using System;
using System.Collections.Generic;
using System.Linq;

namespace OptimizationWPF
{
    public static class SolutionMethodsFactory
    {
        private static Tuple<Variable[], double> MethodOfBox(ITargetFunction targetFunction, double accuracy)
        {
            // 1)
            // Формирование исходного комплекса

            int n = targetFunction.VariablesInfo.Length;

            int N = n <= 5 ? 2 * n : n + 1;

            var randomizer = new Random();

            Points GenerateStartingPoints()
            {
                var coordinatesByVariables = new Variable[n][];

                for (int i = 0; i < n; i++)
                {
                    VariableInfo variableInfo = targetFunction.VariablesInfo[i];

                    coordinatesByVariables[i] = new Variable[N];

                    double gi = variableInfo.ValueLowerBound;
                    double hi = variableInfo.ValueUpperBound;

                    for (int j = 0; j < N; j++)
                    {
                        double rij = randomizer.NextDouble();

                        double xij = gi + rij * (hi - gi);

                        coordinatesByVariables[i][j] = new Variable(variableInfo, xij);
                    }
                }

                return new Points(coordinatesByVariables, Points.MatrixTranspose(coordinatesByVariables));
            }

            // Генерация начальных точек
            Points x = GenerateStartingPoints();

            // Проход по ограничениям второго рода и популяция списка неверных точек

            Points GetWrongPoints(Variable[][] coordinatesByPoints)
            {
                var wrongPointsLst = new List<Variable[]>();
                for (int i = 0; i < coordinatesByPoints.GetLength(0); i++)
                {
                    Variable[] point = coordinatesByPoints[i];

                    bool allChecksSucceeded =
                        targetFunction.RestrictionsOfSecondKind.All(r => r(point));

                    if (allChecksSucceeded is false)
                        wrongPointsLst.Add(point);
                }

                if (wrongPointsLst.All(p => p is null))
                    return null;

                Variable[][] wrongPoints = wrongPointsLst.ToArray();

                return new Points(Points.MatrixTranspose(wrongPoints), wrongPoints);
            }

            // Заведение списка точек, не отвечающих ограничениям второго рода
            Points wrongStartingPoints = GetWrongPoints(x.CoordinatesByPoints);

            // Смещение точки
            void PointOffset(Variable[] point, Variable[][] correctPoints, int PInternal)
            {
                for (int i = 0; i < n; i++)
                {
                    point[i].Value = 1.0 / 2 * (point[i] +
                        1.0 / PInternal * correctPoints[i].Sum(v => v));
                }
            }

            while (!(wrongStartingPoints is null) &&
                x.CoordinatesByPoints.GetLength(0)
                == wrongStartingPoints.CoordinatesByPoints.GetLength(0))
            {
                x = GenerateStartingPoints();

                wrongStartingPoints = GetWrongPoints(x.CoordinatesByPoints);
            }

            if (!(wrongStartingPoints is null))
            {
                int P = N - wrongStartingPoints.CoordinatesByPoints.GetLength(0);

                var pointsToCheck = new Variable[1][];

                Variable[][] correctCoordinatesByPoints = x.CoordinatesByPoints.Except(wrongStartingPoints.CoordinatesByPoints).ToArray();

                var correctPoints = new Points(Points.MatrixTranspose(correctCoordinatesByPoints), correctCoordinatesByPoints);

                for (int i = 0; i < wrongStartingPoints.CoordinatesByPoints.GetLength(0); i++)
                {
                    Variable[] wrongPoint = wrongStartingPoints.CoordinatesByPoints[i];

                    pointsToCheck[0] = wrongPoint;

                    do
                    {
                        PointOffset(wrongPoint, correctPoints.CoordinatesByVariables, P);
                    } while (!(GetWrongPoints(pointsToCheck) is null));

                    correctPoints.CoordinatesByPoints = correctPoints.CoordinatesByPoints.Concat(pointsToCheck).ToArray();

                    for (int j = 0; j < n; j++)
                    {
                        correctPoints.CoordinatesByVariables[j] = correctPoints.CoordinatesByVariables[j].Concat(new List<Variable> { wrongPoint[j] })
                            .ToArray();
                    }

                    P++;
                }
            }

            // 2) Вычисление значений целевой функции Fj для всех N вершин Комплекса
            var funcResults = new Dictionary<Variable[], double>();

            foreach (Variable[] point in x.CoordinatesByPoints)
            {
                funcResults[point] = targetFunction.Func(point);
            }

            // 3) Выбор наилучшего и наихудшего (с точки зрения типа экстремума) значения

            while (true)
            {
                int G = targetFunction.BestValue(funcResults.Values);
                int D = targetFunction.WorstValue(funcResults.Values);

                double Fg = funcResults.Values.ElementAt(G);
                double Fd = funcResults.Values.ElementAt(D);

                // 4) Определение координат Сi центра Комплекса с отброшенной «наихудшей» вершиной
                var C = new Variable[n];

                for (int i = 0; i < n; i++)
                {
                    C[i] = (Variable)(1.0 / (N - 1) * (x[i].Sum(v => v) - x[i][D]));
                }

                // 5) Проверка условия окончания поиска.

                double B = 1.0 / (2 * n) * C
                    .Select((c, i) => new { c, i })
                    .Sum(ci => Math.Abs(ci.c - x[ci.i][D]) + Math.Abs(ci.c - x[ci.i][G]));

                if (B < accuracy)
                    return new Tuple<Variable[], double>(x.CoordinatesByPoints[G], Fg);

                var newPoint = new Variable[n][];
                // 6)  Вычисление координаты новой точки Комплекса взамен наихудшей
                for (int i = 0; i < n; i++)
                {
                    newPoint[i] = new Variable[1];
                    newPoint[i][0] = new Variable(x[i][0].VariableInfo,
                        2.3 * C[i] - 1.3 * x[i][D]);

                    if (newPoint[i][0] < newPoint[i][0].VariableInfo.ValueLowerBound)
                        newPoint[i][0].Value = newPoint[i][0].VariableInfo.ValueLowerBound + accuracy;
                    else if (newPoint[i][0] > newPoint[i][0].VariableInfo.ValueUpperBound)
                        newPoint[i][0].Value = newPoint[i][0].VariableInfo.ValueUpperBound - accuracy;
                }

                var xO = new Points(newPoint, Points.MatrixTranspose(newPoint));

                // 7) Проверка выполнения ограничений 2.го рода для новой точки.
                while (true)
                {
                    Points wrongZeroPoint = GetWrongPoints(xO.CoordinatesByPoints);

                    if (wrongZeroPoint is null)
                        break;

                    for (int i = 0; i < n; i++)
                        newPoint[i][0].Value = 1.0 / 2 * (newPoint[i][0] + C[i]);
                }

                // 8) Вычисление значения целевой функции F0 в новой точке
                double F0 = targetFunction.Func(xO.CoordinatesByPoints[0]);

                // 9) Нахождение новой вершины смещением xi0 на половину расстояния к лучшей из вершин комплекса с номером G

                while (targetFunction.BestValue(new List<double> { F0, Fd }) != 0)
                {
                    for (int i = 0; i < n; i++)
                        xO[i][0].Value = 1.0 / 2 * (xO[i][0] + x[i][G]);

                    F0 = targetFunction.Func(xO.CoordinatesByPoints[0]);
                }

                // 10) Фиксация точки Х0 и замена на F0 значения FD, если вычисленное в новой точке Х0 значение F0 лучше FD
                for (int i = 0; i < n; i++)
                {
                    x[i][D].Value = xO[i][0].Value;
                }

                funcResults[x.CoordinatesByPoints[D]] = F0;
            }
        }

        public static ISolutionMethod ConstructSolutionMethodOfBox()
        {
            ISolutionMethod methodOfBox = new SolutionMethod(MethodOfBox);
            return methodOfBox;
        }
    }
}