using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Курсовая_по_ОП_2
{
    public partial class InfForm : Form
    {
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
        private void PanelMove_MouseDown(object sender, MouseEventArgs e)
        {
            Drag = true;
            MouseX = Cursor.Position.X - this.Left;
            MouseY = Cursor.Position.Y - this.Top;
        }
        private void PanelMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (Drag)
            {
                this.Top = Cursor.Position.Y - MouseY;
                this.Left = Cursor.Position.X - MouseX;
            }
        }
        private void PanelMove_MouseUp(object sender, MouseEventArgs e) { Drag = false; }
        public InfForm()
        {
            InitializeComponent();
        }

        private void CloseButton1_MouseEnter_1(object sender, EventArgs e) // при наведении на крестик цвета 
        {
            CloseButton1.ForeColor = Color.FromArgb(64, 64, 64);
            CloseButton1.BackColor = Color.FromArgb(211, 211, 211);
        }

        private void CloseButton1_MouseLeave(object sender, EventArgs e) // при отведении от крестика цвета 
        {
            CloseButton1.ForeColor = Color.FromArgb(245, 245, 245);
            CloseButton1.BackColor = Color.FromArgb(64, 64, 64);
        }

        private void CloseButton1_Click(object sender, EventArgs e) // выключение формы кликая на крестик 
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e) // свертывание формы на тире 
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label2_MouseEnter(object sender, EventArgs e) // цвета при наведении на тире
        {
            label2.ForeColor = Color.FromArgb(64, 64, 64);
            label2.BackColor = Color.FromArgb(211, 211, 211);
        }

        private void label2_MouseLeave(object sender, EventArgs e) // цвета при отведении от тире
        {
            label2.ForeColor = Color.FromArgb(245, 245, 245);
            label2.BackColor = Color.FromArgb(64, 64, 64);
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

    }
}
