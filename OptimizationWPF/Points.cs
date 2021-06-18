using System;

namespace OptimizationWPF
{
    public class Points
    {
        public Points(Variable[][] coordinatesByVariables, Variable[][] coordinatesByPoints)
        {
            CoordinatesByVariables = coordinatesByVariables ?? throw new ArgumentNullException(nameof(coordinatesByVariables));
            CoordinatesByPoints = coordinatesByPoints ?? throw new ArgumentNullException(nameof(coordinatesByPoints));
        }

        public Variable[][] CoordinatesByVariables { get; set; }

        public Variable[][] CoordinatesByPoints { get; set; }

        public Variable[] this[int i]
        {
            get => CoordinatesByVariables[i];
            set
            {
                CoordinatesByVariables[i] = value;
                CoordinatesByPoints[i] = value;
            }
        }

        public static Variable[][] MatrixTranspose(Variable[][] matrix)
        {
            int rows = matrix.GetLength(0);
            int columns = matrix[0].Length;
            var transposedMatrix = new Variable[columns][];

            for (int j = 0; j < columns; j++)
            {
                transposedMatrix[j] = new Variable[rows];
                for (int i = 0; i < rows; i++)
                    transposedMatrix[j][i] = matrix[i][j];
            }

            return transposedMatrix;
        }
    }
}