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
            graph = new int[len, len];
            init();
        }

        public Graph(Graph g)
        {
            this.len = g.len;
            graph = new int[len, len];
            copy(g.graph);
        }

        public int getNext(Train train, bool[] empty)
        {
            if (train.Path.Count > 0 && empty[train.Path.Peek()])
            {
                return train.Path.Pop();
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

        public void swapAt(int nb, int b)
        {
            graph[nb, b] = 0;
        }

        private void copy(int[,] tab)
        {
            for (int i = 0; i < len; i++)
                for (int j = 0; j < len; j++)
                    graph[i, j] = tab[i, j];
        }

        public Stack<int> backtracking(int start, int end)
        {
            Stack<int> path = new Stack<int>();
            bool[] visited = new bool[len];
            backtracking_rec(start, end, visited, ref path);
            path.Pop();
            return path;
        }

        private void init_visited(ref bool[] visited)
        {
            for (int i = 0; i < len; i++)
                    visited[i] = false;
        }

        private bool find(Queue<int> list, int value)
        {
            foreach (int item in list)
                if (value == item)
                    return true;

            return false;
        }

        private bool backtracking_rec(int start, int end, bool[] visited, ref Stack<int> path)
        {
            visited[start] = true;
            if (start == end)
            {
                path.Push(start);
                return true;
            }
            for (int i = 0; i < len; i++)
            {
                if (graph[start, i] == 1 && !visited[i] && 
                    backtracking_rec(i, end, visited, ref path))
                {
                    path.Push(start);
                    return true;
                }
            }
            return false;
        }
    }
}