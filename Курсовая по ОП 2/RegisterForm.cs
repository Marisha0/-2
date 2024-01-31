using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Курсовая_по_ОП_2
{
    public partial class RegisterForm : Form
    {
        Database DB = new Database("Data Source=dataBase.DB; Version = 3;");
        private Point lastPoint_2;

        public RegisterForm()
        {
            InitializeComponent();

            userNameField.Text = "Введите имя";
            userNameField.ForeColor = Color.Gray;

            textBoxPass2.Text = "Повторите пароль";
            textBoxPass2.ForeColor = Color.Gray;

            textBoxLogin1.Text = "Введите логин";
            textBoxLogin1.ForeColor = Color.Gray;

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void userNameField_Enter(object sender, EventArgs e) // текст внутрь прямоугольничка имени
        {
            if (userNameField.Text == "Введите имя")
            {
                userNameField.Text = "";
                userNameField.ForeColor = Color.Black;
            }
        }

        private void userNameField_Leave(object sender, EventArgs e) // текст внутрь имени
        {
            if (userNameField.Text == "")
            {
                userNameField.Text = "Введите имя";
                userNameField.ForeColor = Color.Gray;
            }
        }

        private void userSurnameField_Enter(object sender, EventArgs e) // текст внутрь прямоугольника фамилии
        {
            if (textBoxPass2.Text == "Повторите пароль")
            {
                textBoxPass2.Text = "";
                textBoxPass2.ForeColor = Color.Black;
            }
        }

        private void userSurnameField_Leave(object sender, EventArgs e) // текст внутрь фамилии 
        {
            if (textBoxPass2.Text == "")
            {
                textBoxPass2.Text = "Повторите пароль";
                textBoxPass2.ForeColor = Color.Gray;
            }
        }

        private void userLogin_Enter(object sender, EventArgs e) // Текст внутрь прямоугольника логина 
        {
            if (textBoxLogin1.Text == "Введите логин")
            {
                textBoxLogin1.Text = "";
                textBoxLogin1.ForeColor = Color.Black;
            }
        }

        private void userLogin_Leave(object sender, EventArgs e) // текст внутрь логина 
        {
            if (textBoxLogin1.Text == "")
            {
                textBoxLogin1.Text = "Введите логин";
                textBoxLogin1.ForeColor = Color.Gray;
            }
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxLogin1.Text) ||
                String.IsNullOrWhiteSpace(textBoxPass1.Text) ||
                String.IsNullOrWhiteSpace(textBoxPass2.Text))
            {
                MessageBox.Show("Поля заполнены некорректно.", "Ошибка регистрации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBoxPass1.Text.Length < 8)
            {
                MessageBox.Show("Пароль должен содержать миниммум 8 символов.", "Ошибка регистрации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else if (textBoxPass1.Text != textBoxPass2.Text)
            {
                MessageBox.Show("Пароли должны совпадать.", "Ошибка доступа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (DB.CheckUser(textBoxLogin1.Text))
                {
                    MessageBox.Show("Такой пользователь уже существует. Войдите в аккаунт или придумайте новый логин.", "Ошибка регистрации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DB.CreateUser(textBoxLogin1.Text, textBoxPass1.Text);
                this.DialogResult = DialogResult.OK;
                MessageBox.Show("Вы были успешно зарегестрированы!", "Регистрация пользователя", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                var formMain = new Form1();
                formMain.Closed += (s, args) => this.Close();
                formMain.Show();

            }

        }


        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint_2 = new Point(e.X, e.Y);
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint_2.X;
                this.Top += e.Y - lastPoint_2.Y;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label3_Click(object sender, EventArgs e)
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

    }
}
