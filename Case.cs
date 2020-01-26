using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migros
{
    public class Case{
        public int time_to_wait;
        public bool is_open;
        public string name;

        public int height;
        public int width;
        public int position_x;
        public int position_y;
        private int max_client;
        private int count_client;

        public Case(int max_client, string name, bool is_open, int x, int y){
            this.name       = name;
            this.is_open    = is_open;
            this.position_x = x;
            this.position_y = y;
            this.max_client = max_client;
        }
        public bool Open(bool status)
        {
            if(status != is_open)
            {
                is_open = status;
                return true;
            }
            return false;
        }
        public (int x, int y) Is_available()
        {
            int x = 0;
            int y = 0;
            if (is_open)
            {
                if (max_client != count_client)
                {
                    x = position_x;
                    y = position_y;
                }
            }
            return (x, y);
        }
        public void Client_arrived_at_case()
        {
            count_client++;
        }
        public bool is_full()
        {
            if (max_client == count_client)
            {
                Console.WriteLine("the Case " + name + "is full !");
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Opening()
        {
            is_open = true;
        }
    }
}
