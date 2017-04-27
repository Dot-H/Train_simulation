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
        private Color origin_colour;
        private Panel panel;
        private Point origin;
        private Point trainPos;
        private Train train;
        private int delay;
        private bool westEast;
        private int xDelta;
        private int yDelta;
        private bool horizontal;
        private double lenght;
        private int nb;
        private Buffer buffer;

        public WaitPanel(Panel panel, Train train, int delay, int nb, Buffer buffer, bool westEast = true, int xDelta = 10, bool horizontal = true)
        {
            this.origin_colour = train.colours[0];

            this.panel = panel;
            this.trainPos = origin;
            this.train = train;
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

            this.panel.Paint += new PaintEventHandler(panel_paint);
        }

        private void zeroTrain()
        {
            trainPos.X = origin.X;
            trainPos.Y = origin.Y;
            this.panel.Invalidate();
        }

        private void moveTrain()
        {
            trainPos.X += xDelta;
            trainPos.Y += yDelta;
        }

        private void panel_paint(object sender, PaintEventArgs e)
        {
            lock (this)
            {
                Graphics g = e.Graphics;
                int i = 0;
                foreach (Color col in train.colours)
                {
                    SolidBrush brush = new SolidBrush(col);
                    g.FillRectangle(brush, trainPos.X - (i * xDelta), trainPos.Y - (i * yDelta), 10, 10);

                    brush.Dispose();
                    i++;
                }
                g.Dispose();
            }
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
                buffer.Read(ref train, this.nb);
                this.panel.Invalidate();

                for (int i = 1; i <= lenght; i++)
                {
                    this.moveTrain();
                    Thread.Sleep(delay);
                    panel.Invalidate();
                }

                buffer.Write(train, buffer.getNext(nb, train.g), nb);

                for (int i = 0; i < train.colours.Count; i++)
                    train.colours[i] = Color.White;
                
            }
        }
    }
}
