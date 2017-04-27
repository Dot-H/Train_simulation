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
        private int delay;
        private bool westEast;
        private int xDelta, yDelta;
        private bool horizontal;
        private bool locked = true;
        private double lenght;
        private int nb;
        private Train train;
        private Buffer buffer;
        
        public PanelController(Panel panel, Button btn, int delay, int nb, Train train, Buffer buffer, bool westEast = true, int xDelta = 10, bool horizontal = true)
        {
            this.origin_colour = train.colours[0];

            this.panel = panel;
            this.train_pos = origin;
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
            this.train = train;
            this.buffer = buffer;

            train.colours[0] = Color.White;
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
                while (locked || !buffer.empty[nb]) ;
                train.colours[0] = origin_colour;
                Thread p = new Thread(new ThreadStart(this.Start));
                p.Start();
                buffer.Write(train, nb, -1);
            }
        }

        private void panel_paint(object sender, PaintEventArgs e)
        {
            
            Graphics g = e.Graphics;
            int i = 0;
            foreach (Color col in train.colours)
            {
                SolidBrush brush = new SolidBrush(col);
                g.FillRectangle(brush, train_pos.X - (xDelta * i), train_pos.Y - (yDelta * i), 10, 10);

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
            for (int i = 0; i < train.colours.Count; i++)
                train.colours[i] = Color.White;
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
                    Thread.Sleep(delay);
                    panel.Invalidate();
                }
                
                buffer.Write(train, buffer.getNext(nb, train.g), nb);
                remove_colours();
            }
        }

    }
}
