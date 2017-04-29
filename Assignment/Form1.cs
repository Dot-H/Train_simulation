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

        private Buffer buffer;
        private Train train1, train2, train_init;
        public delegate void setAccValueDelegate_blue(int value);
        public delegate void setAccValueDelegate_black(int value);
        public setAccValueDelegate_blue setAccValueCallback_blue;
        public setAccValueDelegate_black setAccValueCallback_black;

        public Form1()
        {
            InitializeComponent();

            #region GRAPH_SETUP
            int graph_len = 27;
            Graph graph = new Graph(graph_len);

            graph.setNext(0, new int[] { 1, 4, 21}); // 1 4
            graph.setNext(1, new int[] { 2 });
            graph.setNext(2, new int[] { 3 });
            graph.setNext(3, new int[] { 7 });
            graph.setNext(4, new int[] { 5 });
            graph.setNext(5, new int[] { 6 });
            graph.setNext(6, new int[] { 3 });
            graph.setNext(7, new int[] { 8 });
            graph.setNext(8, new int[] { 9, 22}); 
            graph.setNext(9, new int[] { 10 });
            graph.setNext(10, new int[] { 11 });
            graph.setNext(11, new int[] { 12 });
            graph.setNext(12, new int[] { 0 });
            graph.setNext(13, new int[] { 14, 22 }); 
            graph.setNext(14, new int[] { 15 });
            graph.setNext(15, new int[] { 16 });
            graph.setNext(16, new int[] { 17 });
            graph.setNext(17, new int[] { 18 });
            graph.setNext(18, new int[] { 19, 21});
            graph.setNext(19, new int[] { 20 });
            graph.setNext(20, new int[] { 13 });
            graph.setNext(21, new int[] { 1, 18});
            graph.setNext(22, new int[] { 14, 9});
            graph.setNext(23, new int[] { 16 });
            graph.setNext(24, new int[] { 15 });
            graph.setNext(25, new int[] { 10 });
            graph.setNext(26, new int[] { 11 });

            Graph graph2 = new Graph(graph);
            #endregion

            buffer = new Buffer(graph_len, this);

            Semaphore semaphore = new Semaphore();
            train1 = new Train(Color.Blue, graph);
            train2 = new Train(Color.Black, graph2, false);
            train_init = new Train(Color.White, graph);
            this.setAccValueCallback_blue += new setAccValueDelegate_blue(train1.set_acc);
            this.setAccValueCallback_black += new setAccValueDelegate_black(train2.set_acc);

            #region PANELS_SETUP

            PanelController p1 = new PanelController(blue1, button1, 0, train1, buffer);
            PanelController p2 = new PanelController(black1, button2, 13, train2, buffer, false);
            Locomotives p3 = new Locomotives(purple, button3, 5, Color.Purple, buffer);
            Locomotives p4 = new Locomotives(brown_l, button4, 15, Color.Brown, buffer);
            Locomotives p5 = new Locomotives(red, button5, 10, Color.Red, buffer);
            Locomotives p6 = new Locomotives(green, button6, 2, Color.Green, buffer);

            WaitPanel w1 = new WaitPanel(blue2, train_init, 1, buffer);
            WaitPanel w2 = new WaitPanel(blue3, train_init, 2, buffer);
            WaitPanel w3 = new WaitPanel(blue4, train_init, 3, buffer);
            WaitPanel w4 = new WaitPanel(blue5, train_init, 4, buffer, false, 10, false);
            WaitPanel w5 = new WaitPanel(blue6, train_init, 5, buffer);
            WaitPanel w6 = new WaitPanel(blue7, train_init, 6, buffer, true, 10, false);
            WaitPanel w7 = new WaitPanel(blue8, train_init, 7, buffer, true, 10, false);
            WaitPanel w8 = new WaitPanel(blue9, train_init, 8, buffer, false);
            WaitPanel w9 = new WaitPanel(blue10, train_init, 9, buffer, false);
            WaitPanel w10 = new WaitPanel(blue11, train_init, 10, buffer, false);
            WaitPanel w11 = new WaitPanel(blue12, train_init, 11, buffer, false);
            WaitPanel w12 = new WaitPanel(blue13, train_init, 12, buffer, false, 10, false);

            WaitPanel w13 = new WaitPanel(black2, train_init, 14, buffer, false);
            WaitPanel w14 = new WaitPanel(black3, train_init, 15, buffer, false);
            WaitPanel w15 = new WaitPanel(black4, train_init, 16, buffer, false);
            WaitPanel w16 = new WaitPanel(black5, train_init, 17, buffer, false, 10, false);
            WaitPanel w17 = new WaitPanel(black6, train_init, 18, buffer);
            WaitPanel w18 = new WaitPanel(black7, train_init, 19, buffer);
            WaitPanel w19 = new WaitPanel(black8, train_init, 20, buffer, true, 10, false);

            WaitPanel w20 = new WaitPanel(blackBlue, train_init, 21, buffer, true, 10, false);
            WaitPanel w21 = new WaitPanel(blueBlack, train_init, 22, buffer, false, 10, false);
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

        private void blackAcc_Scroll(object sender, EventArgs e)
        {
            setAccValueCallback_black(black_acc.Value);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void blue_acc_Scroll(object sender, EventArgs e)
        {
            setAccValueCallback_blue(blue_acc.Value);
        }
    }
}
