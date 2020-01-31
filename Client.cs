using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Migros
{
    public class Client
    {
        public int time_in_shop;
        private int speed_x = -4;
        private int speed_y = 4;
        public int x = 850;
        public int y = 500;
        public int size = 30;
        private int time_left;
        private Checkout going_to_checkout;
        public string name;
        public bool waiting = false;
        public bool exit_shop = false;
        public bool done = false;

        public Client(int time_in_shop, string name)
        {
            Random random = new Random();
            int rnd_x = random.Next(1100, 1300);
            int rnd_y = random.Next(400, 500);

            this.time_in_shop = time_in_shop;
            this.time_left = time_in_shop;
            this.name = name;
            this.x = rnd_x;
            this.y = rnd_y;
        }

        /// <summary>
        /// Function that return the brush color for the client.
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
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
                    br = new SolidBrush(Color.Green);
                    break;
                case var expression when time_in_shop < slice * 3:
                    br = new SolidBrush(Color.Blue);
                    break;
                case var expression when time_in_shop < slice * 4:
                    br = new SolidBrush(Color.Purple);
                    break;
                default:
                    br = new SolidBrush(Color.Red);
                    break;
            }
            return br;
        }

        /// <summary>
        /// Function for the movement of the client.
        /// </summary>
        /// <param name="shop_checkout"></param>
        public void Move(List<Checkout> shop_checkout)
        {
            //do i have everything ?
            if (time_left >= 0)
            {
                //No i don't
                Move_random();
            }
            else
            {
                if (!waiting)
                {
                    Where_to_move(shop_checkout);
                }
                if (going_to_checkout == null)
                {
                    Move_random();
                    return;
                }
                //if i'm first  = *0

                if (!going_to_checkout.is_full())
                {
                    int obj_x = going_to_checkout.position_x + 120 + Position_in_checkout();
                    int obj_y = going_to_checkout.position_y;

                    if (Go(obj_x, obj_y))
                    {
                        going_to_checkout.Client_arrived_at_checkout(this);
                        waiting = true;
                    }
                }
            }
        }
        
        /// <summary>
        /// Function to choose which checkout the client is going.
        /// </summary>
        /// <param name="Shop_checkout"></param>
        private void Where_to_move(List<Checkout> Shop_checkout)
        {
            IEnumerable<Checkout> query = Shop_checkout.OrderBy(item => item.Client_waiting.Count);

            using (var sequenceEnum = query.GetEnumerator())
            {
                while (sequenceEnum.MoveNext())
                {
                    if (!sequenceEnum.Current.is_full())
                    {
                        try
                        {
                            var availability = sequenceEnum.Current.Is_available();
                            if (availability)
                            {
                                Checkout to_add = Shop_checkout.Find(c => c.name.Contains(sequenceEnum.Current.name));
                                going_to_checkout = to_add;
                                return;
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Where_to_move : error");
                        }
                    }

                }
                Move_random();
            }
        }
        
        /// <summary>
        /// Function to calculate the next coordonate for the client.
        /// </summary>
        /// <param name="obj_x"></param>
        /// <param name="obj_y"></param>
        /// <returns></returns>
        private bool Go(int obj_x, int obj_y)
        {
           if (speed_x < 0)
                speed_x = speed_x * -1;

            if (speed_y < 0)
                speed_y = speed_y * -1;

            if (exit_shop)
            {
                if(x <= -50)
                {
                    this.done = true;
                }
                x = x - 10;
                return false;
            }
            bool OK_x = false;
            bool OK_y = false;
            switch (x)
            {
                case var expression when x > obj_x - 10 && x < (obj_x + 10):
                    //ok do nothing
                    OK_x = true;
                    break;
                case var expression when x > obj_x:
                    x = x - speed_x;
                    break;
                case var expression when x < obj_x:
                    x = x + speed_x;
                    break;
                default:
                    //ok do nothinbg in x
                    break;
            }
            switch (y)
            {
                case var expression when y > obj_y - 10 && y < (obj_y + 10):
                    //ok do nothing
                    OK_y = true;
                    break;
                case var expression when y > obj_y:
                    y = y - speed_y;
                    break;
                case var expression when y < obj_y:
                    y = y + speed_y;
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

        /// <summary>
        /// Function that count the time left to end the checkout.
        /// </summary>
        /// <param name="pos_y"></param>
        /// <returns></returns>
        public bool Checkout(int pos_y)
        {
            if (time_in_shop / 10 <= 0)
            {
                exit_shop = true;
                return true;
            }
            time_in_shop--;

            return false;
        }

        /// <summary>
        /// Function for the client so he can move in the shop while waiting for checking. (Running his errands or waiting for a checkout to open up.)
        /// </summary>
        private void Move_random()
        {
            if (x < 500)
            {
                if (speed_x < 0)
                    speed_x = speed_x * -1;
            }
            else
            {
                //need correction
                if ((x + speed_x) >= 1350 || (x + speed_x) <= 500)
                {
                    speed_x = speed_x * -1;
                }
                if ((y + speed_y) > 800 || (y + speed_y) < 75)
                {
                    speed_y = speed_y * -1;
                }
            }
            x = x + speed_x;
            y = y + speed_y;
        }

        /// <summary>
        /// Function to get the coordinates for the client depending on the number of clients in the queue.
        /// </summary>
        /// <returns></returns>
        private int Position_in_checkout()
        {
            int count = going_to_checkout.Client_waiting.Count;
            for (int i = 0; i < count; i++)
            {
                if (going_to_checkout.Client_waiting[i].name == this.name)
                {
                    return (50 * i);
                }
            }
            return 50 * count;
        }
        
        /// <summary>
        /// Function to check the time left searching in the shop for the client.
        /// </summary>
        public void Gathering()
        {
            if (time_left >= 0)
            {
                time_left--;
            }
        }
    }
}
