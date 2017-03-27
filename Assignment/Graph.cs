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
                    graph[i, j] = 0;
        }

        public void setNext(int nb, int[] next)
        {
            foreach (int item in next)
                graph[nb, item] = 1;
        }
    }
}
