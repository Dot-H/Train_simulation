using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Drawing; 

namespace Assignment
{
    class Buffer
    {
        private Queue<Train>[] trains;
        private List<Tuple<Color, int>> loco;
        private Form1 form1;
        public bool[] empty;
        

        public Buffer(int len, Form1 form1)
        {
            this.form1 = form1;
            empty = new bool[len];
            trains = new Queue<Train>[len];

            for (int i = 0; i < len; i++)
            {
                empty[i] = true;
                trains[i] = new Queue<Train>();
            }

            loco = new List<Tuple<Color, int>>();
        }

        public void Read(ref Train train, int nb)
        {
            lock (this)
            {
                Tuple<Color, int> l = null;
                // Check whether the buffer is empty.
                while (empty[nb])
                    Monitor.Wait(this);

                train = new Train(trains[nb].Dequeue()); //Get the train color and its locomotives
                if (train.Is_blue)
                    form1.setAccValueCallback_blue += train.set_acc;
                else
                    form1.setAccValueCallback_black += train.set_acc;

                if ((l = get_loco(nb, train)) != null)// Check either there is a locomotive to pickup
                {
                    train.Colours.Add(l.Item1);
                    if (train.Colours.Count == 3)
                    {
                        if (train.Colours[1].Equals(Color.Purple) || train.Colours[1].Equals(Color.Brown))
                            black_to_blue(train.G);
                        else
                            blue_to_black(train.G);
                    }
                }

                Monitor.PulseAll(this);
            }
        }

        public void Write(Train train, int dst, int src)
        {
            lock (this)
            {
                empty[dst] = false;
                trains[dst].Enqueue(new Train(train));
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

        private Tuple<Color, int> get_loco(int nb, Train train)
        {
            lock (this)
            {
                foreach (Tuple<Color, int> l in loco)
                    if (nb == l.Item2 && !findColor(l.Item1, train))
                    {
                        Tuple<Color, int> res = l;
                        loco.Remove(l);
                        return res;
                    }
                return null;
            }
        }

        public int getNext(int nb, Graph g)
        {
            lock (this)
            {
                int next;
                while ((next = g.getNext(nb, empty)) == -1)
                    Monitor.Wait(this);
                return next;
            }
        }

        private bool findColor(Color c, Train train)
        {
            lock (this)
            {
                foreach (var col in train.Colours)
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

        public void black_to_blue(Graph g)
        {
            lock (this)
                g.swapAt(18, 19);
        }

        public void blue_to_black(Graph g)
        {
            lock (this)
                g.swapAt(8, 9);
        }

        public void Start()
        {
        }


    }
}
