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
        private Panel panel;
        private Point origin;
        private Point train;
        private Button btn;
        private int delay;
        private bool westEast;
        private int xDelta;
        private int yDelta;
        private bool horizontal;
        private bool locked = true;
        private double lenght;
        private int nb;
        private List<Color> colour;
        private Buffer buffer;
        private Semaphore semaphore;
        private bool first = true;
        private bool is_loco;
        
        public PanelController(Panel panel, Button btn, int delay, int nb, Color colour, Buffer buffer, Semaphore semaphore, bool westEast = true, int xDelta = 10, bool horizontal = true, bool is_loco = false)
        {
            this.panel = panel;
            this.train = origin;
            this.horizontal = horizontal;
            this.btn = btn;
            this.btn.Click += new EventHandler(this.btnClick);
            this.delay = delay;
            this.nb = nb;
            this.westEast = westEast;
            xDelta = westEast ? xDelta : -xDelta;
            this.xDelta = horizontal ? xDelta : 0;
            yDelta = horizontal ? 0 : xDelta;
            get_origin();
            get_length();
            this.colour = new List<Color>() { colour };
            this.buffer = buffer;
            this.semaphore = semaphore;
            this.is_loco = is_loco;
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

        private void btnClick(object sender, EventArgs e)
        {
            this.locked = false;
            this.btn.Enabled = false;
            lock (this)
            {
                if (!locked)
                    buffer.Write(colour, nb, -1);
            }
        }

        private void panel_paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int i = 0;
            foreach (Color col in colour)
            {
                SolidBrush brush = new SolidBrush(col);
                g.FillRectangle(brush, train.X-(xDelta*i), train.Y-(yDelta*i), 10, 10);

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

        private void remove_colours()
        {
            for (int i = 0; i < colour.Count; i++)
                colour[i] = Color.White;
        }

        public void Start()
        {
            while (true)
            {
                
                this.zeroTrain();
                buffer.Read(ref colour, nb);
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
                first = false;

                if (is_loco)
                {
                    buffer.put_loco(new Tuple<Color, int>(colour[0], buffer.getNext(nb)));
                    remove_colours();
                    panel.Invalidate();
                    break;
                }

                buffer.Write(colour, buffer.getNext(nb), nb);
                remove_colours();
            }
        }

    }
}
