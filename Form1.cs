using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Migros
{
    public partial class Form1 : Form
    {
        private int MAX_CLIENT = 25;
        private int MAX_CLIENT_AT_CASE = 5;
        private int MIN_TIME_IN_SHOP = 5;
        private int MAX_TIME_IN_SHOP = 30;
        private int TIME_BEFOR_OPEN_NEW_CASE = 30;
        private int HUMAN_SPEED = 10;
        private int h = 66;
        private bool opening_case = false;

        private List<Case> Shop_cases = new List<Case>();
        private List<Client> Shop_clients = new List<Client>();

        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            build_shop();

        }

        private void build_shop()
        {
            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                {
                    Shop_cases.Add(new Case(MAX_CLIENT_AT_CASE, "case" + i, true, 20, (h + h / 2) * i + 200, TIME_BEFOR_OPEN_NEW_CASE));
                }
                else
                {
                    Shop_cases.Add(new Case(MAX_CLIENT_AT_CASE, "case" + i, false, 20, (h + h / 2) * i + 200, TIME_BEFOR_OPEN_NEW_CASE));
                }
            }
            Shop_cases[0].Status_change("open");

        }

        private void Form_Exit(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //draw shop
            SolidBrush br_entry = new SolidBrush(Color.Green);
            SolidBrush br_open = new SolidBrush(Color.Blue);
            SolidBrush br_close = new SolidBrush(Color.Red);

            //draw Entry
            e.Graphics.FillRectangle(br_entry, 1180, 250, 250, 500);

            //draw case
            for (int i = 0; i < Shop_cases.Count; i++)
            {
                SolidBrush color;
                if (Shop_cases[i].is_open)
                {
                    color = br_open;
                }
                else
                {
                    color = br_close;
                }
                e.Graphics.FillRectangle(color, 20, (h + h / 2) * i + 200, 100, h);
            }

            //draw circle
            foreach (var client in Shop_clients)
            {
                e.Graphics.FillEllipse(client.Get_color(MAX_TIME_IN_SHOP), client.x, client.y, client.size, client.size);
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //every Sec
            if (Shop_clients.Count < MAX_CLIENT)
            {
                Random random = new Random();
                int randomNumber = random.Next(MIN_TIME_IN_SHOP, MAX_TIME_IN_SHOP);
                Shop_clients.Add(new Client(randomNumber, HUMAN_SPEED, Shop_clients.Count.ToString()));
                //Console.WriteLine("New client (" + Shop_clients.Count +")");
            }
            foreach (var item in Shop_cases)
            {
                item.Checkout();
                if (item.is_open && item.Client_waiting.Count == 0)
                {
                    item.Status_change("close");
                }
            }


        }

        private void Move_tick(object sender, EventArgs e)
        {
            List<Client> Client_to_destroy = new List<Client>();
            foreach (var client in Shop_clients)
            {
                if (client.done)
                {
                    //TODO create list of client to destroy
                    var to_remove = Shop_clients.Find(item => item.done == true);
                    Client_to_destroy.Add(to_remove);
                }
                client.Move(Shop_cases);
            }
            //TODO destroy client
            foreach (var client in Client_to_destroy)
            {
                Shop_clients.Remove(client);
            }

            for (int i = 0; i < Shop_cases.Count; i++)
            {
                if (Shop_cases[i].is_full())
                {
                    try
                    {
                        if (!Shop_cases[i + 1].is_open)
                        {
                            if (!Shop_cases[i + 1].opening || opening_case)
                            {
                                Shop_cases[i + 1].Status_change("open");
                                opening_case = true;
                            }
                            Console.WriteLine("waiting for case to open");
                            return;
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Move_tick : error");
                    }
                }
            }
            Invalidate();
        }
    }
}
