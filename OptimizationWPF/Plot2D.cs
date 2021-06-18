using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OptimizationWPF
{
    public partial class Plot2D : Form
    {
        List<Tuple<double, double, double>> points;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        List<double> sortedPoints;

        public Plot2D(List<Tuple<double, double, double>> points, List<double> sortedPoints)
        {
            this.points = points;
            this.sortedPoints = sortedPoints;
            InitializeComponent();
            DrawChart();
        }

        private void DrawChart()
        {
            double[] areaBorders = new double[16];
            for (int i = 0; i < 16; i++)
            {
                areaBorders[i] = sortedPoints[sortedPoints.Count / 16 * i];
            }

            foreach (Tuple<double, double, double> t in points)
            {
                chart1.Series["Series1"].Points.AddXY(t.Item1, t.Item2);

                if (t.Item3 < areaBorders[1])
                    chart1.Series["Series1"].Points.Last().Color = Color.FromArgb(10, 0, 50);
                else if (t.Item3 < areaBorders[2])
                    chart1.Series["Series1"].Points.Last().Color = Color.FromArgb(10, 0, 100);
                else if (t.Item3 < areaBorders[3])
                    chart1.Series["Series1"].Points.Last().Color = Color.FromArgb(10, 0, 150);
                else if (t.Item3 < areaBorders[4])
                    chart1.Series["Series1"].Points.Last().Color = Color.FromArgb(10, 0, 200);
                else if (t.Item3 < areaBorders[5])
                    chart1.Series["Series1"].Points.Last().Color = Color.FromArgb(10, 0, 255);
                else if (t.Item3 < areaBorders[6])
                    chart1.Series["Series1"].Points.Last().Color = Color.FromArgb(10, 50, 255);
                else if (t.Item3 < areaBorders[7])
                    chart1.Series["Series1"].Points.Last().Color = Color.FromArgb(10, 100, 255);
                else if (t.Item3 < areaBorders[8])
                    chart1.Series["Series1"].Points.Last().Color = Color.FromArgb(10, 150, 255);
                else if (t.Item3 < areaBorders[9])
                    chart1.Series["Series1"].Points.Last().Color = Color.FromArgb(10, 200, 255);
                else if (t.Item3 < areaBorders[10])
                    chart1.Series["Series1"].Points.Last().Color = Color.FromArgb(10, 255, 255);
                else if (t.Item3 < areaBorders[11])
                    chart1.Series["Series1"].Points.Last().Color = Color.FromArgb(60, 255, 200);
                else if (t.Item3 < areaBorders[12])
                    chart1.Series["Series1"].Points.Last().Color = Color.FromArgb(110, 200, 150);
                else if (t.Item3 < areaBorders[13])
                    chart1.Series["Series1"].Points.Last().Color = Color.FromArgb(160, 170, 100);
                else if (t.Item3 < areaBorders[14])
                    chart1.Series["Series1"].Points.Last().Color = Color.FromArgb(210, 140, 50);
                else if (t.Item3 < areaBorders[15])
                    chart1.Series["Series1"].Points.Last().Color = Color.FromArgb(255, 110, 0);
                else
                    chart1.Series["Series1"].Points.Last().Color = Color.FromArgb(255, 0, 0);
            }

            chart1.ChartAreas["ChartArea1"].AxisX.Title = "A1";
            chart1.ChartAreas["ChartArea1"].AxisY.Title = "A2";
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(778, 401);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // Plot2D
            // 
            this.ClientSize = new System.Drawing.Size(778, 401);
            this.Controls.Add(this.chart1);
            this.Name = "Plot2D";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
