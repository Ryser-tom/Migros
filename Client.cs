using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Migros
{
    class Client
    {
        private int time_in_shop;
        private int human_speed;
        public int x = 1180;
        public int y = 500;
        public int size = 30;
        private int time_left;
        private Case going_to_case;

        public Client(int time_in_shop, int human_speed)
        {
            this.time_in_shop = time_in_shop;
            this.time_left = time_in_shop;
            this.human_speed = human_speed;
        }

        public SolidBrush Get_color(int max)
        {
            int slice = max / 5;
            SolidBrush br;
            switch (time_in_shop)
            {
                case var expression when time_in_shop < slice:
                    br = new SolidBrush(Color.Gray);
                    break;
                case var expression when time_in_shop < slice * 2:
                    br = new SolidBrush(Color.Yellow);
                    break;
                case var expression when time_in_shop < slice * 3:
                    br = new SolidBrush(Color.Green);
                    break;
                case var expression when time_in_shop < slice * 4:
                    br = new SolidBrush(Color.Blue);
                    break;
                default:
                    br = new SolidBrush(Color.Red);
                    break;
            }
            return br;
        }
        
        public bool Move(List<Case> shop_case)
        {
            //do i have everything ?
            if (time_left != 0)
            {
                //No i don't
                time_left--;
            }
            else
            {
                //Yes i do
                Console.WriteLine("Time to pay");
                if (going_to_case == null)
                {
                    Where_to_move(shop_case);
                }
                else
                {
                    if (Go(going_to_case.position_x, going_to_case.position_y))
                    {
                        going_to_case.Client_arrived_at_case();
                        return true;
                    }
                }
            }
            return false;
        }
        private void Where_to_move(List<Case> shop_case)
        {
            foreach (var item in shop_case)
            {
                var position = item.Is_available();
                if (position.x!=0)
                {
                    going_to_case = item;
                }

            }
        }
        private bool Go(int obj_x, int obj_y)
        {
            int speed = human_speed - (time_in_shop/2);
            bool OK_x = false;
            bool OK_y = false;
            switch (x)
            {
                case var expression when x > obj_x && x < (obj_x + 120):
                    //ok do nothing
                    OK_x = true;
                    break;
                case var expression when x > obj_x:
                    x = x - speed;
                    break;
                case var expression when x < obj_x:
                    x = x + speed;
                    break;
                default:
                    //ok do nothinbg in x
                    break;
            }
            switch (y)
            {
                case var expression when y > obj_y && y < (obj_y + 25):
                    //ok do nothing
                    OK_y = true;
                    break;
                case var expression when y > obj_y:
                    y = y - speed;
                    break;
                case var expression when y < obj_y:
                    y = y + speed;
                    break;
                default:
                    //ok do nothinbg in y
                    break;
            }
            if (OK_x && OK_y)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
