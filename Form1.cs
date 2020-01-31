using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Migros
{
    public partial class Form1 : Form
    {
        //Change the next values in function of what you want.
        private int MAX_CLIENT = 30;
        private int MAX_CLIENT_AT_CHECKOUT = 5;
        private int MIN_TIME_IN_SHOP = 5;
        private int MAX_TIME_IN_SHOP = 27;
        private int TIME_BEFOR_OPEN_NEW_CHECKOUT = 5;
        private int CHECKOUT_HEIGHT = 66;
        private bool TEST_CHECKOUTS_CLOSING = false;

        private List<Checkout> Shop_checkout = new List<Checkout>();
        private List<Client> Shop_clients = new List<Client>();

        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            build_shop();

        }
        /// <summary>
        /// Function to construct the shop, with all the checkout.
        /// </summary>
        private void build_shop()
        {
            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                {
                    Shop_checkout.Add(new Checkout(MAX_CLIENT_AT_CHECKOUT, "checkout_" + i, true, 20, (CHECKOUT_HEIGHT + CHECKOUT_HEIGHT / 2) * i + 200, TIME_BEFOR_OPEN_NEW_CHECKOUT));
                }
                else
                {
                    Shop_checkout.Add(new Checkout(MAX_CLIENT_AT_CHECKOUT, "checkout_" + i, false, 20, (CHECKOUT_HEIGHT + CHECKOUT_HEIGHT / 2) * i + 200, TIME_BEFOR_OPEN_NEW_CHECKOUT));
                }
            }
            //Shop_checkout[0].Status_change("open");

        }

        /// <summary>
        /// Function to exit the app when Esc is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Exit(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Function to paint the shop every frame.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //draw shop
            SolidBrush br_entry = new SolidBrush(Color.Green);
            SolidBrush br_open = new SolidBrush(Color.Blue);
            SolidBrush br_close = new SolidBrush(Color.Red);

            //draw Entry
            e.Graphics.FillRectangle(br_entry, 1180, 250, 250, 500);

            //draw checkout
            for (int i = 0; i < Shop_checkout.Count; i++)
            {
                SolidBrush color;
                if (Shop_checkout[i].is_open)
                {
                    color = br_open;
                }
                else
                {
                    color = br_close;
                }
                e.Graphics.FillRectangle(color, Shop_checkout[i].position_x, Shop_checkout[i].position_y, 100, CHECKOUT_HEIGHT);
            }

            //draw circle
            foreach (var client in Shop_clients)
            {
                e.Graphics.FillEllipse(client.Get_color(MAX_TIME_IN_SHOP), client.x, client.y, client.size, client.size);
            }

        }

        /// <summary>
        /// Function which is called every second for the diffrents tests.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Shop_clients.Count < MAX_CLIENT)
            {
                Random random = new Random();
                int randomNumber = random.Next(MIN_TIME_IN_SHOP, MAX_TIME_IN_SHOP);
                Shop_clients.Add(new Client(randomNumber, Shop_clients.Count.ToString()));
                //Console.WriteLine("New client (" + Shop_clients.Count +")");
            }

            foreach (var client in Shop_clients)
            {
                client.Gathering();
            }

            foreach (var item in Shop_checkout)
            {
                item.Checking_out();
                item.time_check();
                if (item.is_open && item.Client_waiting.Count == 0 && item.name != "checkout_0")
                {
                    item.Status_change("close");
                }
            }


        }

        /// <summary>
        /// Function called every 10 milli-seconds for the calcul of movement.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Move_tick(object sender, EventArgs e)
        {
            List<Client> Client_to_destroy = new List<Client>();
            foreach (var client in Shop_clients)
            {
                if (client.done)
                {
                    var to_remove = Shop_clients.Find(item => item.done == true);
                    Client_to_destroy.Add(to_remove);
                }
                client.Move(Shop_checkout);
            }
            foreach (var client in Client_to_destroy)
            {
                if (!TEST_CHECKOUTS_CLOSING)
                {
                    Shop_clients.Remove(client);
                }
            }

            for (int i = 0; i < Shop_checkout.Count; i++)
            {
                if (Shop_checkout[i].is_full())
                {
                    if (Shop_checkout.Count-1 != i && !Shop_checkout[i + 1].is_open && !Shop_checkout[i + 1].status_changing)
                    {
                        Shop_checkout[i + 1].Status_change("open");
                    }
                }
            }
            Invalidate();
        }
    }
}
