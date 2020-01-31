using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Migros
{
    public class Client
    {
        public int time_in_shop;
        private int human_speed;
        private int speed_x = -4;
        private int speed_y = 4;
        public int x = 850;
        public int y = 500;
        public int size = 30;
        private int time_left;
        private Case going_to_case;
        public string name;
        public bool waiting = false;
        public bool exit_shop = false;
        public bool done = false;

        public Client(int time_in_shop, int human_speed, string name)
        {
            Random random = new Random();
            int rnd_x = random.Next(1100, 1300);
            int rnd_y = random.Next(400, 500);

            this.time_in_shop = time_in_shop;
            this.time_left = time_in_shop;
            this.human_speed = human_speed;
            this.name = name;
            this.x = rnd_x;
            this.y = rnd_y;
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

        public void Move(List<Case> shop_case)
        {
            //do i have everything ?
            if (time_left >= 0)
            {
                //No i don't
                Move_random();
            }
            else
            {
                try
                {
                    if (!waiting)
                    {
                        Where_to_move(shop_case);
                    }
                    if (going_to_case == null)
                    {
                        Move_random();
                        return;
                    }
                    //if i'm first  = *0

                    if (!going_to_case.is_full())
                    {
                        int obj_x = going_to_case.position_x + 120 + Position_in_case();
                        int obj_y = going_to_case.position_y;

                        if (Go(obj_x, obj_y))
                        {
                            going_to_case.Client_arrived_at_case(this);
                            waiting = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Move : error" + e);
                }
            }
        }
        
        private void Where_to_move(List<Case> shop_cases)
        {
            IEnumerable<Case> query = shop_cases.OrderBy(item => item.Client_waiting.Count);

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
                                Case to_add = shop_cases.Find(c => c.name.Contains(sequenceEnum.Current.name));
                                going_to_case = to_add;
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
        private int Position_in_case()
        {
            int count = going_to_case.Client_waiting.Count;
            for (int i = 0; i < count; i++)
            {
                if (going_to_case.Client_waiting[i].name == this.name)
                {
                    return (50 * i);
                }
            }
            return 50 * count;
        }
        public void Gathering()
        {
            if (time_left >= 0)
            {
                time_left--;
            }
        }
    }
}
