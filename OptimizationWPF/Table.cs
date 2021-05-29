using System.Windows.Forms;

namespace OptimizationWPF
{
    public partial class Table : Form
    {
        public Table(Vertex[] vx)
        {
            InitializeComponent();
            for (int i = 0; i < vx.Length; i++)
            {
                dataGridView1.Rows.Add(vx[i].x, vx[i].y, vx[i].z);
            }
        }
    }
}
