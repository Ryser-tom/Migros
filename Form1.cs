using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Migros
{
    public partial class Form1 : Form
    {
        private int MAX_CLIENT = 10;
        private int MAX_CLIENT_AT_CASE = 3;
        private int MIN_TIME_IN_SHOP = 5;
        private int MAX_TIME_IN_SHOP = 15;
        private int TIME_BEFOR_OPEN_NEW_CASE = 30;
        private int HUMAN_SPEED = 10;
        private int h = 66;

        private List<Case> Shop_cases = new List<Case>();
        private List<Client> Shop_clients = new List<Client>();

        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            build_shop();

        }

        private void build_shop(){
            for (int i = 0; i < 5; i++)
            {
                //TODO: create case with position
                Shop_cases.Add(new Case(MAX_CLIENT_AT_CASE,"case"+i, false, 20, (h + h / 2) * i + 200));
            }
            Shop_cases[0].Open(true);

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
            for (int i = 0; i < Shop_cases.Count; i++) {
                SolidBrush color;
                if (Shop_cases[i].is_open)
                {
                    color = br_open;
                }else{
                    color = br_close;
                }
                e.Graphics.FillRectangle(color, 20, (h+h/2)*i+200, 100, h);
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
                Shop_clients.Add(new Client(randomNumber, HUMAN_SPEED));
                Console.WriteLine("New client (" + Shop_clients.Count +")");
            }
            
        }

        private void Move_tick(object sender, EventArgs e)
        {
            //TODO: call move
            for (int i = 0; i < Shop_cases.Count; i++)
            {
                if (Shop_cases[i].is_full())
                {
                    for (int j = 0; j < Shop_cases.Count; j++)
                    {
                        if (!Shop_cases[i].is_open)
                        {
                            Shop_cases[i + 1].Opening();
                            return;
                        }
                        else
                        {
                            Console.WriteLine("all shop cases are open");
                        }
                    }
                }
            }
            foreach (var client in Shop_clients)
            {

                client.Move(Shop_cases);
            }
            Invalidate();
        }
    }
}
