using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Assignment
{
    class Train
    {
        public List<Color> colours;
        public Graph g;

        public Train(Color colour, Graph g)
        {
            this.colours = new List<Color> { colour };
            this.g = g;
        }

        public Train(Train t)
        {
            this.colours = new List<Color>(t.colours);
            this.g = new Graph(t.g);
        }

    }
}
