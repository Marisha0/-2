using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Курсовая_по_ОП_2
{
    public partial class GronForm : Form
    {
        public Database myDB;
        private bool Drag;
        private int MouseX;
        private int MouseY;

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        private bool m_aeroEnabled;

        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]

        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );

        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();
                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW; return cp;
            }
        }
        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0; DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 0
                        }; DwmExtendFrameIntoClientArea(this.Handle, ref margins);
                    }
                    break;
                default: break;
            }
            base.WndProc(ref m);

        }

        public GronForm()
        {
            InitializeComponent();
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

        private void GronForm_MouseDown(object sender, MouseEventArgs e)
        {
            Drag = true;
            MouseX = Cursor.Position.X - this.Left;
            MouseY = Cursor.Position.Y - this.Top;
        }

        private void GronForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (Drag)
            {
                this.Top = Cursor.Position.Y - MouseY;
                this.Left = Cursor.Position.X - MouseX;
            }
        }

        private void GronForm_MouseUp(object sender, MouseEventArgs e)
        {
            Drag = false;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm2 mainForm2 = new MainForm2();
            mainForm2.Show();
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

        static string rus = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        static string RUS = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        static string la = "abcdefghijklmnopqrstuvwxyz";
        static string LA = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox3.Text))
                {
                    MessageBox.Show("Установите шаг шифрования!");
                    return;
                }

                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("Введите текст для шифрования!");
                    return;
                }

                if (textBox1.Text.Any(char.IsDigit))
                {
                    MessageBox.Show("Текст для шифрования не должен содержать цифры!");
                    return;
                }

                string input = textBox1.Text;
                textBox2.Text = ori(input);

            }
            catch (FormatException)
            {
                MessageBox.Show("Шаг шифрования должен быть числом!");
            }
        }

        public string ori(string inp)
        {
            StringBuilder code = new StringBuilder();
            string s = textBox1.Text;
            string sd = textBox3.Text;
            int[] steps = new int[sd.Length];

            // Заполнение массива steps
            for (int i = 0; i < sd.Length; i++)
            {
                steps[i] = Convert.ToInt32(sd[i].ToString());
            }

            for (int i = 0; i < s.Length; i++)
            {
                int gronIndex = i % steps.Length; // получаем индекс из массива steps для текущего символа
                                                  // получаем сдвиг

                if (s[i] == ' ')
                {
                    code.Append(" ");
                }
                else if (la.Contains(s[i]))
                {
                    for (int j = 0; j < la.Length; j++)
                    {
                        if (s[i] == la[j])
                        {
                            code.Append(la[(j + steps[gronIndex]) % la.Length]);
                            break;
                        }
                    }
                }
                else if (LA.Contains(s[i]))
                {
                    for (int j = 0; j < LA.Length; j++)
                    {
                        if (s[i] == LA[j])
                        {
                            code.Append(LA[(j + steps[gronIndex]) % LA.Length]);
                            break;
                        }
                    }
                }
                else if (rus.Contains(s[i]))
                {
                    for (int j = 0; j < rus.Length; j++)
                    {
                        if (s[i] == rus[j])
                        {
                            code.Append(rus[(j + steps[gronIndex]) % rus.Length]);
                            break;
                        }
                    }
                }
                else if (RUS.Contains(s[i]))
                {
                    for (int j = 0; j < RUS.Length; j++)
                    {
                        if (s[i] == RUS[j])
                        {
                            code.Append(RUS[(j + steps[gronIndex]) % RUS.Length]);
                            break;
                        }
                    }
                }
            }

            return code.ToString();
        }


        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox3.Text))
                {
                    MessageBox.Show("Установите шаг шифрования!");
                    return;
                }

                if (string.IsNullOrEmpty(textBox2.Text))
                {
                    MessageBox.Show("Введите текст для шифрования!");
                    return;
                }

                if (textBox2.Text.Any(char.IsDigit))
                {
                    MessageBox.Show("Текст для шифрования не должен содержать цифры!");
                    return;
                }

                string input = textBox2.Text;
                textBox1.Text = org(input);

            }
            catch (FormatException)
            {
                MessageBox.Show("Шаг шифрования должен быть числом!");
            }

        }

        public string org(string inp)
        {
            StringBuilder code = new StringBuilder();
            string s = textBox2.Text;
            string sd = textBox3.Text;
            int[] steps = new int[sd.Length];

            // Заполнение массива steps
            for (int i = 0; i < sd.Length; i++)
            {
                steps[i] = Convert.ToInt32(sd[i].ToString());
            }

            for (int i = 0; i < s.Length; i++)
            {
                int gronIndex = i % steps.Length; // получаем индекс из массива steps для текущего символа
                                                  // получаем сдвиг

                if (s[i] == ' ')
                {
                    code.Append(" ");
                }
                else if (la.Contains(s[i]))
                {
                    for (int j = 0; j < la.Length; j++)
                    {
                        if (s[i] == la[j])
                        {
                            code.Append(la[(j - steps[gronIndex] + la.Length) % la.Length]);
                            break;
                        }
                    }
                }
                else if (LA.Contains(s[i]))
                {
                    for (int j = 0; j < LA.Length; j++)
                    {
                        if (s[i] == LA[j])
                        {
                            code.Append(LA[(j - steps[gronIndex] + LA.Length) % LA.Length]);
                            break;
                        }
                    }
                }
                else if (rus.Contains(s[i]))
                {
                    for (int j = 0; j < rus.Length; j++)
                    {
                        if (s[i] == rus[j])
                        {
                            code.Append(rus[(j - steps[gronIndex] + rus.Length) % rus.Length]);
                            break;
                        }
                    }
                }
                else if (RUS.Contains(s[i]))
                {
                    for (int j = 0; j < RUS.Length; j++)
                    {
                        if (s[i] == RUS[j])
                        {
                            code.Append(RUS[(j - steps[gronIndex] + RUS.Length) % RUS.Length]);
                            break;
                        }
                    }
                }
            }

            return code.ToString();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
        }

        private void save1_Click(object sender, EventArgs e)
        {
            string text1 = "Нешифрованное: " + textBox1.Text; // добавляем префикс "Нешифрованное: " к тексту textBox1
            string text2 = "Шифрованное: " + textBox2.Text; // добавляем префикс "Шифрованное: " к тексту textBox2
            MessageBox.Show(text1 + "\n" + text2); // выводим текст textBox1 и textBox2 в MessageBox
            SaveFileDialog open = new SaveFileDialog();

            // открываем окно сохранения
            open.ShowDialog();

            // присваиваем строке путь из открытого нами окна
            string path = open.FileName;

            try
            {
                // создаем файл используя конструкцию using
                using (FileStream fs = File.Create(path))
                {
                    // создаем переменную типа массива байтов
                    // и присваиваем ей метод перевода текста в байты
                    byte[] info = new UTF8Encoding(true).GetBytes(text1 + "\n" + text2); // объединяем текст textBox1 и textBox2
                                                                                         // производим запись байтов в файл
                    fs.Write(info, 0, info.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            string currentDirectoryPath = Path.GetDirectoryName(Application.ExecutablePath);
            if (!File.Exists($"{currentDirectoryPath}\\dataBase.DB"))
            {
                myDB = new Database("Data Source=dataBase.DB; Version = 3;");
                myDB.InitializeDatabase();
            }
            else
            {
                myDB = new Database("Data Source=dataBase.DB; Version = 3;");
            }

        }

    }
}
