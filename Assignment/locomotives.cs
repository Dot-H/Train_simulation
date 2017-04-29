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
        private bool locked = true;
        private int next;
        private Color colour;
        private Buffer buffer;

        public Locomotives(Panel panel, Button btn, int next, Color colour, Buffer buffer)
        {
            this.origin_colour = colour;

            this.panel = panel;
            this.train = origin;
            this.btn = btn;
            this.btn.Click += new EventHandler(this.btnClick2);
            this.next = next;
            get_origin();
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
            origin = new Point(10, panel.Size.Height - 20);
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
            buffer.put_loco(new Tuple<Color, int>(origin_colour, next));

            remove_colours();
            this.panel.Invalidate();
        }
    }
}
