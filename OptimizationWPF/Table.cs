using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OptimizationWPF
{
    public partial class Table : Form
    {
        public Table(Dictionary<List<Variable>, double> results)
        {
            InitializeComponent();

            int n = results.Keys.ElementAt(0).Count;

            dataGridView1.Columns.AddRange(results.Keys.ElementAt(0)
                .Select(v => new DataGridViewTextBoxColumn { HeaderText = v.VariableInfo.Name }).ToArray());

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "S" });

            foreach (var result in results)
            {
                int lastRowIndex = dataGridView1.Rows.Add();

                for (int i = 0; i < n; i++)
                    dataGridView1.Rows[lastRowIndex].Cells[i].Value = result.Key[i].Value;

                dataGridView1.Rows[lastRowIndex].Cells[n].Value = result.Value;
            }
        }
    }
}