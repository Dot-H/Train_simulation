using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;

namespace Assignment
{
    class WaitPanel
    {
        private Panel panel;
        private Point origin;
        private Point train;
        private new List<Color> colour;
        private int delay;
        private bool westEast;
        private int xDelta;
        private int yDelta;
        private bool horizontal;
        private double lenght;
        private int nb;
        private Buffer buffer;
        private Semaphore semaphore;
        private bool first = true;

        public WaitPanel(Panel panel, Color colour, int delay, int nb, Buffer buffer, Semaphore semaphore, bool westEast = true, int xDelta = 10, bool horizontal = true)
        {
            this.panel = panel;
            this.train = origin;
            this.colour = new List<Color>() { colour };
            this.horizontal = horizontal;
            this.delay = delay;
            this.westEast = westEast;
            xDelta = westEast ? xDelta : -xDelta;
            this.xDelta = horizontal ? xDelta : 0;
            yDelta = horizontal ? 0 : xDelta;
            get_origin();
            get_length();
            this.nb = nb;
            this.buffer = buffer;
            this.semaphore = semaphore;
        }

        private void zeroTrain()
        {
            train.X = origin.X;
            train.Y = origin.Y;
            this.panel.Invalidate();
        }

        private void moveTrain()
        {
            train.X += xDelta;
            train.Y += yDelta;
        }

        private void panel_paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int i = 0;
            foreach (Color col in colour)
            {
                SolidBrush brush = new SolidBrush(col);
                g.FillRectangle(brush, train.X - (i * xDelta), train.Y - (i * yDelta), 10, 10);

                brush.Dispose();
                i++;
            }
            g.Dispose();
        }

        private void get_origin()
        {
            if (westEast)
                origin = new Point(10, 10);
            else if (horizontal)
                origin = new Point(panel.Size.Width - 20, 10);
            else
                origin = new Point(10, panel.Size.Height - 20);
        }

        private void get_length()
        {
            if (horizontal)
                lenght = (panel.Size.Width - 20 - 10) / 10;
            else
                lenght = (panel.Size.Height - 20 - 10) / 10;
        }

        public void Start()
        {
            while(true)
            {

                this.zeroTrain();
                buffer.Read(ref colour, this.nb);
                this.panel.Invalidate();

                if (first)
                    this.panel.Paint += new PaintEventHandler(panel_paint);
                this.zeroTrain();
                this.panel.Invalidate();

                for (int i = 1; i <= lenght; i++)
                {
                    this.moveTrain();
                    Thread.Sleep(delay);
                    panel.Invalidate();
                }

                buffer.Write(colour, buffer.getNext(nb), nb);

                for (int i = 0; i < colour.Count; i++)
                    colour[i] = Color.White;
                
                first = false;
            }
        }
    }
}
