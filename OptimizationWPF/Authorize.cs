using System;
using System.Windows.Forms;

namespace OptimizationWPF
{
    public partial class Authorize : Form
    {
        public Authorize()
        {
            InitializeComponent();
        }

        private void LogInBtn_Click(object sender, EventArgs e)
        {
            string lg = "admin"; string ps = "admin"; string login = ""; string password = "";
            try
            {
                login = textBox1.Text;
                password = textBox2.Text;
                if (lg == login && ps == password)
                {
                    Admin admin = new Admin();
                    admin.Show();
                    this.Close();
                }
                else
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception) { MessageBox.Show("Произшла ошибка, возможно вы ввели недопустимые символы", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
        }

        private void Authorize_Load(object sender, EventArgs e)
        {

        }

        private void Authorize_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
