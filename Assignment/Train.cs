using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Assignment
{
    class Train
    {
        private List<Color> colours;
        private Graph g;
        private int delay;
        private bool is_blue;

        public Train(Color colour, Graph g, bool is_blue = true, int delay = 100)
        {
            this.Colours = new List<Color> { colour };
            this.G = g;
            this.Delay = delay;
            this.Is_blue = is_blue;
        }

        public Train(Train t)
        {
            lock (this)
            {
                this.Colours = new List<Color>(t.Colours);
                this.G = new Graph(t.G);
                this.Delay = t.Delay;
                this.Is_blue = t.is_blue;
            }
        }

        public void set_acc(int value)
        {
            lock (this)
                Delay = value;
        }

        public List<Color> Colours
        {
            get
            {
                lock (this)
                    return colours;
            }

            set
            {
                lock (this)
                    colours = value;
            }
        }

        internal Graph G
        {
            get
            {
                lock (this)
                    return g;
            }

            set
            {
                lock (this)
                    g = value;
            }
        }

        public int Delay
        {
            get
            {
                lock (this)
                    return delay;
            }

            set
            {
                lock (this)
                    delay = value;
            }
        }

        public bool Is_blue
        {
            get
            {
                lock (this)
                    return is_blue;
            }

            set
            {
                lock (this)
                    is_blue = value;
            }
        }
    }
}
