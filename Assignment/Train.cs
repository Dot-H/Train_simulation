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
        private Stack<int> path;
        private int destination;
        private Queue<int> order;

        public Train(Color colour, Graph g, bool is_blue = true, int delay = 100)
        {
            this.Colours = new List<Color> { colour };
            this.G = g;
            this.Delay = delay;
            this.Is_blue = is_blue;
            this.Order = new Queue<int>();
        }

        public Train(Train t)
        {
            lock (this)
            {
                this.Colours = new List<Color>(t.Colours);
                this.G = new Graph(t.G);
                this.Delay = t.Delay;
                this.Is_blue = t.Is_blue;
                this.path = t.Path;
                this.Order = t.Order;
                this.Destination = t.Destination;
            }
        }

        public void set_acc(int value)
        {
            lock (this)
                Delay = value;
        }

        public string path_string()
        {
            string res = "";

            for (int i = 0; i < Path.Count - 1; i++)
                res += Path.ElementAt<int>(i) + " -> ";

            if (Path.Count > 0 && Path.ElementAt<int>(Path.Count - 1) != 24)
                res += Path.ElementAt<int>(Path.Count - 1);

            return res;

        }

    #region getters_setters

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

        public Stack<int> Path
        {
            get
            {
                lock (this)
                    return path;
            }

            set
            {
                lock (this)
                    path = value;
            }
        }

        public Queue<int> Order
        {
            get
            {
                lock (this)
                    return order;
                }

            set
            {
                lock (this)
                    order = value;
            }
        }

        public int Destination
        {
            get
            {
                lock (this)
                    return destination;
            }

            set
            {
                lock (this)
                    destination = value;
            }
        }
        #endregion

    }
}
