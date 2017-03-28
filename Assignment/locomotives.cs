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
    class Locomotives
    {
        private Color origin_colour;

        private Panel panel;
        private Point origin;
        private Point train;
        private Button btn;
        private int delay;
        private bool westEast;
        private int xDelta, yDelta;
        private bool horizontal;
        private bool locked = true;
        private double lenght;
        private int nb;
        private Color colour;
        private Buffer buffer;

        public Locomotives(Panel panel, Button btn, int delay, int nb, Color colour, Buffer buffer, bool westEast = true, int xDelta = 10, bool horizontal = true)
        {
            this.origin_colour = colour;

            this.panel = panel;
            this.train = origin;
            this.horizontal = horizontal;
            this.btn = btn;
            this.btn.Click += new EventHandler(this.btnClick2);
            this.delay = delay;
            this.nb = nb;
            this.westEast = westEast;
            xDelta = westEast ? xDelta : -xDelta;
            this.xDelta = horizontal ? xDelta : 0;
            yDelta = horizontal ? 0 : xDelta;
            get_origin();
            get_length();
            this.colour = Color.White;
            this.buffer = buffer;
            this.panel.Paint += new PaintEventHandler(panel_paint);

        }

        private void zeroTrain()
        {
            train.X = origin.X;
            train.Y = origin.Y;
            this.panel.Invalidate();
        }

        private void btnClick2(object sender, EventArgs e)
        {
            this.locked = false;
            lock (this)
            {
                while (locked) ;
                Thread p = new Thread(new ThreadStart(this.Start));
                p.Start();
            }
        }

        private void panel_paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            SolidBrush brush = new SolidBrush(colour);
            g.FillRectangle(brush, train.X, train.Y, 10, 10);

            brush.Dispose();

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

        private void remove_colours()
        {
            colour = Color.White;
        }

        public void Start()
        {
            this.zeroTrain();
            this.colour = origin_colour;

            this.panel.Invalidate();
            buffer.put_loco(new Tuple<Color, int>(origin_colour, buffer.getNext(nb)));

            remove_colours();
            this.panel.Invalidate();
        }
    }
}
