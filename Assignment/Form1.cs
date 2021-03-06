﻿using System;
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

        private int[] train_order_blue, train_order_black;
        private Button[] button_order_blue, button_order_black;
        private int order_blue, order_black;

        private Panel[] panel_array;
        private List<int> old_panels_blue;
        private List<int> old_panels_black;

        public Form1()
        {

            InitializeComponent();

            #region GRAPH_SETUP
            int graph_len = 27;
            Graph graph = new Graph(graph_len);

            graph.setNext(0, new int[] { 1, 4 }); // 1 4
            graph.setNext(1, new int[] { 2 });
            graph.setNext(2, new int[] { 3 });
            graph.setNext(3, new int[] { 7 });
            graph.setNext(4, new int[] { 5 });
            graph.setNext(5, new int[] { 6 });
            graph.setNext(6, new int[] { 3 });
            graph.setNext(7, new int[] { 8, 23 });
            graph.setNext(8, new int[] { 9, 22}); 
            graph.setNext(9, new int[] { 10 });
            graph.setNext(10, new int[] { 11 });
            graph.setNext(11, new int[] { 12 });
            graph.setNext(12, new int[] { 0 });
            graph.setNext(13, new int[] { 14 }); 
            graph.setNext(14, new int[] { 15 });
            graph.setNext(15, new int[] { 16 });
            graph.setNext(16, new int[] { 17 });
            graph.setNext(17, new int[] { 18 });
            graph.setNext(18, new int[] { 19, 21 });
            graph.setNext(19, new int[] { 20 });
            graph.setNext(20, new int[] { 13 });
            graph.setNext(21, new int[] { 1});
            graph.setNext(22, new int[] { 14});
            graph.setNext(23, new int[] { 16, 24 });
            graph.setNext(24, new int[] { 24 });
            graph.setNext(25, new int[] { 10 });
            graph.setNext(26, new int[] { 11 });

            Graph graph2 = new Graph(graph);
            #endregion

            buffer = new Buffer(graph_len, this);

            #region TRAIN_SETUP

            train1 = new Train(Color.Blue, graph);
            train2 = new Train(Color.Black, graph2, false);
            train_init = new Train(Color.Transparent, graph);

            this.setAccValueCallback_blue += new setAccValueDelegate_blue(train1.set_acc);
            this.setAccValueCallback_black += new setAccValueDelegate_black(train2.set_acc);

            buffer.setDestValueCallback_blue = new Buffer.setDestValueDelegate_blue(set_panels_blue);
            buffer.setDestValueCallback_black = new Buffer.setDestValueDelegate_black(set_panels_black);

            buffer.rmvDestColorCallback_blue = new Buffer.rmvDestColorDelegate_blue(rmv_panels_blue);
            buffer.rmvDestColorCallback_black = new Buffer.rmvDestColorDelegate_black(rmv_panels_black);

            train_order_blue = new int[] { 2, 20, 5, 10 };
            train_order_black = new int[] { 2, 20, 5, 10 };

            button_order_blue = new Button[] { blueOrdrBtn1, blueOrdrBtn2, blueOrdrBtn3, blueOrdrBtn4 };
            button_order_black = new Button[] { blackOrdrBtn1, blackOrdrBtn2, blackOrdrBtn3, blackOrdrBtn4 };

            order_blue = 0;
            order_black = 0;

            #endregion

            #region PANELS_SETUP
            panel_array = new Panel[] { blue1, blue2, blue3, blue4, blue5, blue6, blue7, blue8, blue9, blue10, blue11, blue12, blue13,
                                        black1, black2, black3, black4, black5, black6, black7, black8, blackBlue, blueBlack, end1, end2 };
            old_panels_blue = new List<int>();
            old_panels_black = new List<int>();
            

            PanelController p1 = new PanelController(blue1, button1, 0, train1, buffer);
            PanelController p2 = new PanelController(black1, button2, 13, train2, buffer, false);
            Locomotives p3 = new Locomotives(purple, button3, 2, Color.Purple, buffer);
            Locomotives p4 = new Locomotives(brown_l, button4, 5, Color.Brown, buffer);
            Locomotives p5 = new Locomotives(red, button5, 20, Color.Red, buffer);
            Locomotives p6 = new Locomotives(green, button6, 10, Color.Green, buffer);

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

            WaitPanel w20 = new WaitPanel(blackBlue, train_init, 21, buffer, false, 10, false);
            WaitPanel w21 = new WaitPanel(blueBlack, train_init, 22, buffer, false, 10, false);

            WaitPanel w22 = new WaitPanel(end1, train_init, 23, buffer);
            WaitPanel w23 = new WaitPanel(end2, train_init, 24, buffer);
            #endregion

            #region THREAD_SETUP
            Thread thread1 = new Thread(new ThreadStart(p1.Start));
            Thread thread2 = new Thread(new ThreadStart(p2.Start));

            Thread loco1 = new Thread(new ThreadStart(p3.Start));
            Thread loco2 = new Thread(new ThreadStart(p4.Start));
            Thread loco3 = new Thread(new ThreadStart(p5.Start));
            Thread loco4  = new Thread(new ThreadStart(p6.Start));
 
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
            Thread wait22 = new Thread(new ThreadStart(w22.Start));
            Thread wait23 = new Thread(new ThreadStart(w23.Start));

            Thread bufThread = new Thread(new ThreadStart(buffer.Start));

            #endregion

            #region THREAD_START
            bufThread.Start();
            
            thread1.Start();
            thread2.Start();

            loco1.Start();
            loco2.Start();
            loco3.Start();
            loco4.Start();
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
            wait22.Start();
            wait23.Start();
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

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        #region ORDER_BTN

        private void blueBtnPrple_Click(object sender, EventArgs e)
        {
            train1.Order.Enqueue(train_order_blue[0]);
            blueBtnPrple.Hide();
            blueBtnPrple.Enabled = false;
            button_order_blue[order_blue].BackColor = Color.Purple;

            if (train1.Order.Count == 4)
                blue_order_chosen();

            order_blue++;
        }

        private void blueBtnRed_Click(object sender, EventArgs e)
        {
            train1.Order.Enqueue(train_order_blue[1]);
            blueBtnRed.Hide();
            blueBtnRed.Enabled = false;
            button_order_blue[order_blue].BackColor = Color.Red;

            if (train1.Order.Count == 4)
                blue_order_chosen();

            order_blue++;
        }

        private void blueBtnBrwn_Click(object sender, EventArgs e)
        {
            train1.Order.Enqueue(train_order_blue[2]);
            blueBtnBrwn.Hide();
            blueBtnBrwn.Enabled = false;
            button_order_blue[order_blue].BackColor = Color.Brown;

            if (train1.Order.Count == 4)
                blue_order_chosen();

            order_blue++;
        }

        private void blueBtnGrn_Click(object sender, EventArgs e)
        {
            train1.Order.Enqueue(train_order_blue[3]);
            blueBtnGrn.Hide();
            blueBtnGrn.Enabled = false;
            button_order_blue[order_blue].BackColor = Color.Green;

            if (train1.Order.Count == 4)
                blue_order_chosen();

            order_blue++;
        }

        private void blueOrdrBtn3_Click(object sender, EventArgs e)
        {

        }

        private void blackOrdrBtn4_Click(object sender, EventArgs e)
        {

        }

        private void blackBtnPrple_Click(object sender, EventArgs e)
        {
            train2.Order.Enqueue(train_order_black[0]);
            blackBtnPrple.Hide();
            blackBtnPrple.Enabled = false;
            button_order_black[order_black].BackColor = Color.Purple;

            if (train2.Order.Count == 4)
                black_order_chosen();

            order_black++;
        }

        private void blackBtnRed_Click(object sender, EventArgs e)
        {
            train2.Order.Enqueue(train_order_black[1]);
            blackBtnRed.Hide();
            blackBtnRed.Enabled = false;
            button_order_black[order_black].BackColor = Color.Red;

            if (train2.Order.Count == 4)
                black_order_chosen();

            order_black++;
        }

        private void blackBtnGrn_Click(object sender, EventArgs e)
        {
            train2.Order.Enqueue(train_order_black[3]);
            blackBtnGrn.Hide();
            blackBtnGrn.Enabled = false;
            button_order_black[order_black].BackColor = Color.Green;

            if (train2.Order.Count == 4)
                black_order_chosen();

            order_black++;
        }

        private void blackBtnBrwn_Click(object sender, EventArgs e)
        {
            train2.Order.Enqueue(train_order_black[2]);
            blackBtnBrwn.Hide();
            blackBtnBrwn.Enabled = false;
            button_order_black[order_black].BackColor = Color.Brown;

            if (train2.Order.Count == 4)
                black_order_chosen();

            order_black++;
        }

        private void blue_order_chosen()
        {
            train1.Order.Enqueue(24);

            blueOrdrBtn1.Show();
            blueOrdrBtn2.Show();
            blueOrdrBtn3.Show();
            blueOrdrBtn4.Show();

            blueOrdrTxt.Text = "Order of the blue train:";

            button1.Enabled = true;
        }

        private void black_order_chosen()
        {
            train2.Order.Enqueue(24);

            blackOrdrBtn1.Show();
            blackOrdrBtn2.Show();
            blackOrdrBtn3.Show();
            blackOrdrBtn4.Show();

            blackOrdrTxt.Text = "Order of the black train:";

            button2.Enabled = true;
        }

        #endregion

        #region path_drawing

        private void set_panels_blue(int nb)
        {
           if (panel_array[nb].BackColor == Color.LightGray)
               panel_array[nb].Invoke(new Action(() => panel_array[nb].BackColor = Color.Thistle));
           else
               panel_array[nb].Invoke(new Action(() => panel_array[nb].BackColor = Color.AliceBlue));
           old_panels_blue.Add(nb);
        }

        private void set_panels_black(int nb)
        {
            if (panel_array[nb].BackColor == Color.AliceBlue)
                panel_array[nb].Invoke(new Action(() => panel_array[nb].BackColor = Color.Thistle));
            else
                panel_array[nb].Invoke(new Action(() => panel_array[nb].BackColor = Color.LightGray));
            old_panels_black.Add(nb);
        }

        private void rmv_panels_blue()
        {
            foreach (int nb in old_panels_blue)
            {
                if (panel_array[nb].BackColor == Color.Thistle)
                    panel_array[nb].Invoke(new Action(() => panel_array[nb].BackColor = Color.LightGray));
                else
                    panel_array[nb].Invoke(new Action(() => panel_array[nb].BackColor = Color.White));
            }
            old_panels_blue = new List<int>();
        }

        private void rmv_panels_black()
        {
            foreach (int nb in old_panels_black)
            {
                if (panel_array[nb].BackColor == Color.Thistle)
                    panel_array[nb].Invoke(new Action(() => panel_array[nb].BackColor = Color.AliceBlue));
                else
                    panel_array[nb].Invoke(new Action(() => panel_array[nb].BackColor = Color.White));
            }
            old_panels_black = new List<int>();
        }

        #endregion

        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
        private void blue_acc_Scroll(object sender, EventArgs e)
        {
            setAccValueCallback_blue(blue_acc.Value);
        }

    }
}
