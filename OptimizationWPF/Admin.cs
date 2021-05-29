using System;
using System.Windows.Forms;

namespace OptimizationWPF
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
            dataGridView1.Rows.Add("Метод поочередного варьирования переменных");
            dataGridView1.Rows.Add("Метод наискорейшего спуска");
            dataGridView1.Rows.Add("Метод симплексный");
            dataGridView1.Rows.Add("Метод Бокса");
            dataGridView2.Rows.Add("Метод поочередного варьирования переменных");
            dataGridView2.Rows.Add("Метод наискорейшего спуска");
            dataGridView2.Rows.Add("Метод симплексный");
            dataGridView2.Rows.Add("Метод Бокса");
            dataGridView3.Rows.Add("C=a*(A1^2+b*A2-u*V1)^N+a1*(b1*A1+A2^2-u1*V2)^N");
            dataGridView3.Rows.Add("S = α * G*((T2- β*A) N + μ * exp(T1+T2) ^N + △ * (T2-T1))");
            dataGridView4.Rows.Add("C=a*(A1^2+b*A2-u*V1)^N+a1*(b1*A1+A2^2-u1*V2)^N");
            dataGridView4.Rows.Add("S = α * G*((T2- β*A) N + μ * exp(T1+T2) ^N + △ * (T2-T1))");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Приносим свои извинения, функция находиться в разработке.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Приносим свои извинения, функция находиться в разработке.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Приносим свои извинения, функция находиться в разработке.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Приносим свои извинения, функция находиться в разработке.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
