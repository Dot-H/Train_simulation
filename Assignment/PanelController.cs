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
        private Point train;
        private Button btn;
        private int delay;
        private bool westEast;
        private int xDelta, yDelta;
        private bool horizontal;
        private bool locked = true;
        private double lenght;
        private int nb;
        private List<Color> colour;
        private Buffer buffer;
        
        public PanelController(Panel panel, Button btn, int delay, int nb, Color colour, Buffer buffer, bool westEast = true, int xDelta = 10, bool horizontal = true)
        {
            this.origin_colour = colour;

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
            this.colour = new List<Color>() { Color.White };
            this.buffer = buffer;

            this.panel.Paint += new PaintEventHandler(panel_paint);

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
            //this.btn.Enabled = false;
            lock (this)
            {
                while (locked || !buffer.empty[nb]) ;
                this.colour = new List<Color> { origin_colour };
                Thread p = new Thread(new ThreadStart(this.Start));
                p.Start();
                buffer.Write(new List<Color>() { origin_colour }, nb, -1);
            }
        }

        private void panel_paint(object sender, PaintEventArgs e)
        {
            
            Graphics g = e.Graphics;
            int i = 0;
            foreach (Color col in colour)
            {
                SolidBrush brush = new SolidBrush(col);
                g.FillRectangle(brush, train.X - (xDelta * i), train.Y - (yDelta * i), 10, 10);

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

                for (int i = 1; i <= lenght; i++)
                {
                    this.moveTrain();
                    Thread.Sleep(delay);
                    panel.Invalidate();
                }
                
                buffer.Write(colour, buffer.getNext(nb), nb);
                remove_colours();
            }
        }

    }
}
