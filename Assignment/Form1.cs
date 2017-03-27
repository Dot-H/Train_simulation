using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Assignment
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            InitializeComponent();

            #region GRAPH_SETUP
            int graph_len = 27;
            Graph graph = new Graph(graph_len);

            graph.setNext(0, new int[] { 1, 4}); // 1 4
            graph.setNext(1, new int[] { 2 });
            graph.setNext(2, new int[] { 3 });
            graph.setNext(3, new int[] { 7 });
            graph.setNext(4, new int[] { 5 });
            graph.setNext(5, new int[] { 6 });
            graph.setNext(6, new int[] { 3 });
            graph.setNext(7, new int[] { 8 });
            graph.setNext(8, new int[] { 9, 22 }); // 9 22
            graph.setNext(9, new int[] { 10 });
            graph.setNext(10, new int[] { 11 });
            graph.setNext(11, new int[] { 12 });
            graph.setNext(12, new int[] { 0 });
            graph.setNext(13, new int[] { 14 }); 
            graph.setNext(14, new int[] { 15 });
            graph.setNext(15, new int[] { 16 });
            graph.setNext(16, new int[] { 17 });
            graph.setNext(17, new int[] { 18 });
            graph.setNext(18, new int[] { 19, 21 }); // 19 21
            graph.setNext(19, new int[] { 20 });
            graph.setNext(20, new int[] { 13 });
            graph.setNext(21, new int[] { 1 });
            graph.setNext(22, new int[] { 14 });
            graph.setNext(23, new int[] { 16 });
            graph.setNext(24, new int[] { 15 });
            graph.setNext(25, new int[] { 10 });
            graph.setNext(26, new int[] { 11 });
            #endregion

            Buffer buffer = new Buffer(graph_len, graph);
            Semaphore semaphore = new Semaphore();

            #region PANELS_SETUP
            PanelController p1 = new PanelController(blue1, button1, 100, 0, Color.Blue, buffer, semaphore);
            PanelController p2 = new PanelController(black1, button2, 100, 13, Color.Black, buffer, semaphore, false);
            PanelController p3 = new PanelController(purple, button3, 100, 23, Color.Purple, buffer, semaphore, false, 10, false, true);
            PanelController p4 = new PanelController(brown_l, button4, 100, 24, Color.Brown, buffer, semaphore, false, 10, false, true);
            PanelController p5 = new PanelController(red, button5, 100, 25, Color.Red, buffer, semaphore, false, 10, false, true);
            PanelController p6 = new PanelController(green, button6, 100, 26, Color.Green, buffer, semaphore, false, 10, false, true);

            WaitPanel w1 = new WaitPanel(blue2, Color.Purple, 100, 1, buffer, semaphore);
            WaitPanel w2 = new WaitPanel(blue3, Color.White, 100, 2, buffer, semaphore);
            WaitPanel w3 = new WaitPanel(blue4, Color.White, 100, 3, buffer, semaphore);
            WaitPanel w4 = new WaitPanel(blue5, Color.White, 40, 4, buffer, semaphore, false, 10, false);
            WaitPanel w5 = new WaitPanel(blue6, Color.Purple, 100, 5, buffer, semaphore);
            WaitPanel w6 = new WaitPanel(blue7, Color.White, 100, 6, buffer, semaphore, true, 10, false);
            WaitPanel w7 = new WaitPanel(blue8, Color.White, 40, 7, buffer, semaphore, true, 10, false);
            WaitPanel w8 = new WaitPanel(blue9, Color.White, 100, 8, buffer, semaphore, false);
            WaitPanel w9 = new WaitPanel(blue10, Color.White, 100, 9, buffer, semaphore, false);
            WaitPanel w10 = new WaitPanel(blue11, Color.Purple, 100, 10, buffer, semaphore, false);
            WaitPanel w11 = new WaitPanel(blue12, Color.White, 100, 11, buffer, semaphore, false);
            WaitPanel w12 = new WaitPanel(blue13, Color.White, 100, 12, buffer, semaphore, false, 10, false);

            WaitPanel w13 = new WaitPanel(black2, Color.White, 100, 14, buffer, semaphore, false);
            WaitPanel w14 = new WaitPanel(black3, Color.White, 100, 15, buffer, semaphore, false);
            WaitPanel w15 = new WaitPanel(black4, Color.White, 40, 16, buffer, semaphore, false);
            WaitPanel w16 = new WaitPanel(black5, Color.White, 100, 17, buffer, semaphore, false, 10, false);
            WaitPanel w17 = new WaitPanel(black6, Color.White, 100, 18, buffer, semaphore);
            WaitPanel w18 = new WaitPanel(black7, Color.White, 100, 19, buffer, semaphore);
            WaitPanel w19 = new WaitPanel(black8, Color.White, 100, 20, buffer, semaphore, true, 10, false);

            WaitPanel w20 = new WaitPanel(blackBlue, Color.White, 100, 21, buffer, semaphore, true, 10, false);
            WaitPanel w21 = new WaitPanel(blueBlack, Color.White, 100, 22, buffer, semaphore, false, 10, false);
            #endregion

            #region THREAD_SETUP
            Thread thread1 = new Thread(new ThreadStart(p1.Start));
            Thread thread2 = new Thread(new ThreadStart(p2.Start));
            Thread thread3 = new Thread(new ThreadStart(p3.Start));
            Thread thread4 = new Thread(new ThreadStart(p4.Start));
            Thread thread5 = new Thread(new ThreadStart(p5.Start));
            Thread thread6 = new Thread(new ThreadStart(p6.Start));

            Thread wait1 = new Thread(new ThreadStart(w1.Start));
            Thread wait2 = new Thread(new ThreadStart(w2.Start));
            Thread wait3 = new Thread(new ThreadStart(w3.Start));
            Thread wait4 = new Thread(new ThreadStart(w4.Start));
            Thread wait5 = new Thread(new ThreadStart(w5.Start));
            Thread wait6 = new Thread(new ThreadStart(w6.Start));
            Thread wait7 = new Thread(new ThreadStart(w7.Start));
            Thread wait8 = new Thread(new ThreadStart(w8.Start));
            Thread wait9 = new Thread(new ThreadStart(w9.Start));
            Thread wait10 = new Thread(new ThreadStart(w10.Start));
            Thread wait11 = new Thread(new ThreadStart(w11.Start));
            Thread wait12 = new Thread(new ThreadStart(w12.Start));
            Thread wait13 = new Thread(new ThreadStart(w13.Start));
            Thread wait14 = new Thread(new ThreadStart(w14.Start));
            Thread wait15 = new Thread(new ThreadStart(w15.Start));
            Thread wait16 = new Thread(new ThreadStart(w16.Start));
            Thread wait17 = new Thread(new ThreadStart(w17.Start));
            Thread wait18 = new Thread(new ThreadStart(w18.Start));
            Thread wait19 = new Thread(new ThreadStart(w19.Start));
            Thread wait20 = new Thread(new ThreadStart(w20.Start));
            Thread wait21 = new Thread(new ThreadStart(w21.Start));

            Thread bufThread = new Thread(new ThreadStart(buffer.Start));

            Thread sem = new Thread(new ThreadStart(semaphore.Start));
            #endregion

            #region THREAD_START
            bufThread.Start();
            sem.Start();

            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();
            thread5.Start();
            thread6.Start();

            wait1.Start();
            wait2.Start();
            wait3.Start();
            wait4.Start();
            wait5.Start();
            wait6.Start();
            wait7.Start();
            wait8.Start();
            wait9.Start();
            wait10.Start();
            wait11.Start();
            wait12.Start();
            wait13.Start();
            wait14.Start();
            wait15.Start();
            wait16.Start();
            wait17.Start();
            wait18.Start();
            wait19.Start();
            wait20.Start();
            wait21.Start();
            #endregion
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
