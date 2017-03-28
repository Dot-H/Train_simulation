using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing; 

namespace Assignment
{
    class Buffer
    {
        private List<Color> trainColor;
        private List<Tuple<Color, int>> loco;
        public bool[] empty;
        public Graph g;

        public Buffer(int len, Graph g)
        {
            
            empty = new bool[len];
            for (int i = 0; i < len; i++)
                empty[i] = true;
            this.g = g;
            loco = new List<Tuple<Color, int>>();
        }

        public void Read(ref List<Color> trainColor, int nb)
        {
            lock (this)
            {
                Tuple<Color, int> l = null;
                // Check whether the buffer is empty.
                while (empty[nb])
                    Monitor.Wait(this);

                trainColor = new List<Color>(this.trainColor); //Get the train color and its locomotives
                if ((l = get_loco(nb)) != null)// Check either there is a locomotive to pickup
                {
                    trainColor.Add(l.Item1);
                    if (trainColor.Count == 3)
                    {
                        if (trainColor[1].Equals(Color.Purple) || trainColor[1].Equals(Color.Brown))
                            black_to_blue();
                        else
                            blue_to_black();
                    }
                }
                Monitor.PulseAll(this);
            }
        }

        public void Write(List<Color> trainColor, int dst, int src)
        {
            lock (this)
            {
                empty[dst] = false;
                this.trainColor = new List<Color>(trainColor);
                if (src != -1)
                    empty[src] = true;
                Monitor.PulseAll(this);
            }
        }

        public void put_loco(Tuple<Color, int> l)
        {
            lock (this)
            {
                loco.Add(l);
                while (find(l))
                {
                    Monitor.Wait(this);
                }
            }
        }

        private Tuple<Color, int> get_loco(int nb)
        {
            lock (this)
            {
                foreach (Tuple<Color, int> l in loco)
                    if (nb == l.Item2 && !findColor(l.Item1))
                    {
                        Tuple<Color, int> res = l;
                        loco.Remove(l);
                        return res;
                    }
                return null;
            }
        }

        public int getNext(int nb)
        {
            lock (this)
            {
                int next;
                while ((next = g.getNext(nb, empty)) == -1)
                    Monitor.Wait(this);
                return next;
            }
        }

        private bool findColor(Color c)
        {
            lock (this)
            {
                foreach (var col in trainColor)
                    if (col == c)
                        return true;
                return false;
            }
        }

        private bool find(Tuple<Color, int> t)
        {
            lock (this)
            {
                foreach (var l in loco)
                    if (l.Item1 == t.Item1 && l.Item2 == t.Item2)
                        return true;
                return false;
            }
        }

        public void black_to_blue()
        {
            lock (this)
                g.swapAt(18, 19);
        }

        public void blue_to_black()
        {
            lock (this)
                g.swapAt(8, 9);
        }

        public void Start()
        {
        }


    }
}
