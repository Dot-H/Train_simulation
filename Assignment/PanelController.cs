using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace Assignment
{
    class PanelController
    {
        private Color origin_colour;

        private Panel panel;
        private Point origin;
        private Point train_pos;
        private Button btn;
        private bool westEast;
        private int xDelta, yDelta;
        private bool horizontal;
        private bool locked = true;
        private double lenght;
        private int nb;
        private Train train, train_origin;
        private Buffer buffer;
        
        public PanelController(Panel panel, Button btn, int nb, Train train, Buffer buffer, bool westEast = true, int xDelta = 10, bool horizontal = true)
        {
            this.origin_colour = train.Colours[0];

            this.panel = panel;
            this.train_pos = origin;
            this.horizontal = horizontal;
            this.btn = btn;
            this.btn.Click += new EventHandler(this.btnClick);
            this.nb = nb;
            this.westEast = westEast;
            xDelta = westEast ? xDelta : -xDelta;
            this.xDelta = horizontal ? xDelta : 0;
            yDelta = horizontal ? 0 : xDelta;
            get_origin();
            get_length();
            this.train = train;
            this.train_origin = train;
            this.buffer = buffer;

            train.Colours[0] = Color.White;
            this.panel.Paint += new PaintEventHandler(panel_paint);

        }

        private void zeroTrain()
        {
            train_pos.X = origin.X;
            train_pos.Y = origin.Y;
            this.panel.Invalidate();
        }

        private void moveTrain()
        {
            train_pos.X += xDelta;
            train_pos.Y += yDelta;
        }

        private void btnClick(object sender, EventArgs e)
        {
            this.locked = false;
            this.btn.Enabled = false;
            lock (this)
            {
                train = new Train(train_origin);
                int end = train.Order.Dequeue();

                train.Path = train.G.backtracking(nb, end);
                train.Destination = train.Path.ElementAt<int>(train.Path.Count - 1);
                buffer.write_path(train);

                while (locked || !buffer.empty[nb]);

                train.Colours[0] = origin_colour;
                buffer.Write(train, nb, -1);
            }
        }

        private void panel_paint(object sender, PaintEventArgs e)
        {
            try
            {
                lock (this)
                {
                    Graphics g = e.Graphics;
                    int i = 0;
                    foreach (Color col in train.Colours)
                    {
                        SolidBrush brush = new SolidBrush(col);
                        g.FillRectangle(brush, train_pos.X - (i * xDelta), train_pos.Y - (i * yDelta), 10, 10);

                        brush.Dispose();
                        i++;
                    }
                    g.Dispose();
                }
            }
            catch (System.InvalidOperationException)
            {
                Thread.Sleep(1);
                panel.Invalidate();
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

        private void remove_colours()
        {
            for (int i = 0; i < train.Colours.Count; i++)
                train.Colours[i] = Color.Transparent;
        }

        public void Start()
        {
            while (true)
            {
                
                this.zeroTrain();
                buffer.Read(ref train, nb);
                this.panel.Invalidate();

                for (int i = 1; i <= lenght; i++)
                {
                    this.moveTrain();
                    Thread.Sleep(train.Delay);
                    panel.Invalidate();
                }
                
                buffer.Write(train, buffer.getNext(train), nb);
                remove_colours();
            }
        }

    }
}
