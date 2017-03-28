using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Assignment
{
    class Graph
    {
        private int len;
        private int[,] graph;
        public Graph(int len)
        {
            this.len = len;
            graph = new int[len,len];
            init();
        }

        public int getNext(int nb, bool[] empty)
        {
            for (int i = 0; i < len; i++)
            {
                if (graph[nb, i] == 1 && empty[i])
                    return i;
            }
            return -1;        
        }

        private void init()
        {
            for (int i = 0; i < len; i++)
                for (int j = 0; j < len; j++)
                    graph[i, j] = -1;
        }

        public void setNext(int nb, int[] next)
        {
            foreach (int item in next)
                graph[nb, item] = 1;
        }

        public void swapAt(int nb, int b) // There is maximum two ways.
        {
            /*
            int a = -1;
            int b = -1;
            for (int i = 0; i < len && b == -1; i++)
            {
                if (graph[nb, i] != -1)
                {
                    if (a == -1)
                        a = i;
                    else
                        b = i;
                }
            }
            int tmp = graph[nb, a];
            graph[nb, a] = graph[nb, b];
            graph[nb, b] = tmp;
            */
            graph[nb, b] = 0;
        }
    }
}
