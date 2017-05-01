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

        public delegate void setDestValueDelegate_blue(string txt);
        public delegate void setDestValueDelegate_black(string txt);

        public setDestValueDelegate_blue setDestValueCallback_blue;
        public setDestValueDelegate_black setDestValueCallback_black;

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

                train = new Train(trains[nb].Dequeue()); 
                if (train.Is_blue)
                    form1.setAccValueCallback_blue += train.set_acc;
                else
                    form1.setAccValueCallback_black += train.set_acc;

                if (train.Destination == nb && (l = get_loco(nb, train)) != null)// Check either there is a locomotive to pickup
                {
                    train.Colours.Add(l.Item1);
                    if (train.Order.Count > 0)
                    {
                        int end = train.Order.Dequeue();
                        train.Path = train.G.backtracking(nb, end);
                        train.Destination = train.Path.ElementAt<int>(train.Path.Count - 1);
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

        public int getNext(Train train)
        {
            lock (this)
            {
                int next;
                while ((next = train.G.getNext(train, empty)) == -1)
                    Monitor.Wait(this);

                write_path(train); 
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

        public void write_path(Train train)
        {
            if (train.Is_blue)
                write_path_blue(train);
            else
                write_path_black(train);
        }
        
        private void write_path_blue(Train train)
        {
            setDestValueCallback_blue(train.path_string());
        }

        private void write_path_black(Train train)
        {
            setDestValueCallback_black(train.path_string());
        }

        public void Start()
        {
        }


    }
}
