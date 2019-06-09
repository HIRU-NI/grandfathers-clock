using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clock_new
{
    public partial class Form1 : Form
    {
        double[,] x = new double[3, 2];
        double[,] y = new double[3, 2];
        int hour, minute, second;

        Graphics g;

        public Form1()
        {
            InitializeComponent();
        }

        private int Dpx(double x)
        {
            return (int)(x + 0.5);
        }

        private int Dpy(double y)
        {
            return panel.Height - ((int)(y + 0.5));
        }

        private void DisplayHand(int o)
        {
            try
            {
                g = panel.CreateGraphics();
            }
            catch (ObjectDisposedException e)
            { }
            g.DrawLine(Pens.Black, Dpx(x[o, 0]),Dpy(y[o, 0]),Dpx(x[o, 1]),Dpy(y[o, 1]));
        }

        private void HideHand(int o)
        {
            try
            {
                g = panel.CreateGraphics();
            }
            catch (ObjectDisposedException e)
            { }
            g.DrawLine(Pens.White, Dpx(x[o, 0]), Dpy(y[o, 0]), Dpx(x[o, 1]), Dpy(y[o, 1]));
        }

        public void DrawCircle(float centerX, float centerY, float radius)
        {
            try
            {
                g = panel.CreateGraphics();
            }
            catch (ObjectDisposedException e)
            { }
            g.DrawEllipse(Pens.Black, centerX - radius, panel.Height -(centerY + radius), radius + radius, radius + radius);
        }

        private void Translate(int o, double tx, double ty)
        {
            for (int i = 0; i < 2; i++)
            {
                x[o, i] += tx;
                y[o, i] += ty;
            }
        }

        private void Rotate(int o, double t)
        {
            double x1, y1;
            for (int i = 0; i < 2; i++)
            {
                x1 = x[o, i];
                y1 = y[o, i];
                x[o, i] = x1 * Math.Cos(t) - y1 * Math.Sin(t);
                y[o, i] = x1 * Math.Sin(t) + y1 * Math.Cos(t);
            }
        }

        private void FixedRotate(int o, double t, double x, double y)
        {
            Translate(o, -x, -y);
            Rotate(o,t);
            Translate(o, x, y);
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            DrawCircle(150, 250, 100);
            g = panel.CreateGraphics();
            Rectangle rect = new Rectangle(90, panel.Height - 150, 120, 180);
            g.DrawRectangle(Pens.Black, rect);

        }

        private async void buttonStart_Click(object sender, EventArgs e)
        {
            DateTime nowTime = DateTime.Now;

            hour = Convert.ToInt32(DateTime.Now.Hour);
            if (hour > 12)
                hour -= 12;
            minute = Convert.ToInt32(DateTime.Now.Minute);
            second = Convert.ToInt32(DateTime.Now.Second);



            //hour hand
            x[0, 0] = 0; y[0, 0] = 0;
            x[0, 1] = 0; y[0, 1] = 50;

            //minute hand
            x[1, 0] = 0; y[1, 0] = 0;
            x[1, 1] = 0; y[1, 1] = 70;

            //second hand
            x[2, 0] = 0; y[2, 0] = 0;
            x[2, 1] = 0; y[2, 1] = 90;

            Translate(0, 150, 250);
            Translate(1, 150, 250);
            Translate(2, 150, 250);

            FixedRotate(0, -((Math.PI / 180) * 6*second), 150, 250);
            FixedRotate(1, -((Math.PI / 180) * 6*minute), 150, 250);
            FixedRotate(2, -((Math.PI / 180) * 30*hour), 150, 250);

            

            while(true)
            {
                for(int j=minute;j<60;j++)
                {
                    for (int i = second; i < 60; i++)
                    {
                        DisplayHand(0);
                        DisplayHand(1);
                        DisplayHand(2);
                        await Task.Delay(1000);
                        HideHand(2);
                        FixedRotate(2, -((Math.PI / 180) * 6), 150, 250);
                        DisplayHand(0);
                        DisplayHand(1);
                        DisplayHand(2);

                    }
                    second = 0;
                    HideHand(1);
                    FixedRotate(1, -((Math.PI / 180) * 6), 150, 250);
                    DisplayHand(0);
                    DisplayHand(1);
                    DisplayHand(2);
                    

                }
                minute = 0;
                FixedRotate(0, -((Math.PI / 180) * 30), 150, 250);
                HideHand(0);
                DisplayHand(0);
                DisplayHand(1);
                DisplayHand(2);


            }

        }

        
    }
}
