using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Курсовая_по_ОП_2
{
    public partial class MainForm2 : Form
    {
        private Point lastPoint_2;

        public MainForm2()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, EventArgs e) // закрываем окно 
        {
            Application.Exit();
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint_2 = new Point(e.X, e.Y);
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint_2.X;
                this.Top += e.Y - lastPoint_2.Y;
            }
        }

        private void button1_Click(object sender, EventArgs e) // переводим на информационное окно 
        {
            this.Hide();
            InfForm infForm = new InfForm();
            infForm.ShowDialog();
        }

        private void label2_Click(object sender, EventArgs e) // закрываем окно 
        {
            Application.Exit();
        }

        private void label3_Click(object sender, EventArgs e) // сворачиваем окно 
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.ForeColor = Color.FromArgb(64, 64, 64);
            label2.BackColor = Color.FromArgb(211, 211, 211);
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.FromArgb(245, 245, 245);
            label2.BackColor = Color.FromArgb(64, 64, 64);
        }

        private void label3_MouseEnter(object sender, EventArgs e)
        {
            label3.ForeColor = Color.FromArgb(64, 64, 64);
            label3.BackColor = Color.FromArgb(211, 211, 211);
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.ForeColor = Color.FromArgb(245, 245, 245);
            label3.BackColor = Color.FromArgb(64, 64, 64);
        }

        private void button2_Click(object sender, EventArgs e) // перевести на окно шифрования
        {
            this.Hide();
            GronForm gronForm = new GronForm();
            gronForm.ShowDialog();
        }

        private void label6_Click(object sender, EventArgs e) // венуть на авторизацию
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.ShowDialog();
        }

        private void label6_MouseEnter(object sender, EventArgs e)
        {
            label6.ForeColor = Color.FromArgb(64, 64, 64);
            label6.BackColor = Color.FromArgb(211, 211, 211);
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            label6.ForeColor = Color.FromArgb(245, 245, 245);
            label6.BackColor = Color.FromArgb(64, 64, 64);
        }
    }
}
