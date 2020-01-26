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
        public int x = 1180;
        public int y = 500;
        public int size = 30;
        private int time_left;
        private Case going_to_case;

        public Client(int time_in_shop)
        {
            this.time_in_shop = time_in_shop;
            this.time_left = time_in_shop;
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
        
        public void Move(List<Case> shop_case)
        {
            int speed = (time_in_shop/1000)*-1;
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
                    var objective_position = Where_to_move(shop_case);
                }
            }
        }
        private (int x, int y) Where_to_move(List<Case> shop_case)
        {
            int x=0;
            int y=0;
            foreach (var item in shop_case)
            {
                var position = item.Is_available();
                if (position.x!=0)
                {
                    return (position.x, position.y);
                }

            }
            return (x,y);
        }
    }
}
