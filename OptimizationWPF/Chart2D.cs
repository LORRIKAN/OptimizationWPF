using System;
using System.Windows.Forms;

namespace OptimizationWPF
{
    public partial class Chart2D : Form
    {
        Vertex[] vx;
        public Chart2D(Vertex[] vertexes)
        {
            InitializeComponent();
            vx = vertexes;
            DrawChart();
            chart1.ChartAreas["ChartArea1"].AxisX.Title = "A1";
            chart1.ChartAreas["ChartArea1"].AxisY.Title = "A2";
        }

        void DrawChart()
        {
            foreach (Vertex item in vx)
                chart1.Series["Series1"].Points.AddXY(item.x, item.y);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Printing.PrintPreview();
        }
    }
}
