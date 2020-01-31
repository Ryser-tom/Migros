using System;
using System.Collections.Generic;
using System.Linq;

namespace Migros
{
    public class Checkout
    {
        public List<Client> Client_waiting = new List<Client>();
        public bool is_open;
        public bool status_changing;
        public string name;

        public int height;
        public int width;
        public int position_x;
        public int position_y;

        private int Time_to_open;
        private int max_client;
        private int left_to_change = 0;

        public Checkout(int max_client, string name, bool is_open, int x, int y, int time)
        {
            this.name = name;
            this.is_open = is_open;
            this.position_x = x;
            this.position_y = y;
            this.Time_to_open = time;
            this.max_client = max_client;
            this.status_changing = false;
        }
        /// <summary>
        /// To call when you want to change the status of a checkout.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool Status_change(string status)
        {
            if (!status_changing)
            {
                status_changing = true;
                left_to_change = Time_to_open;
            }
            return true;
        } 

        /// <summary>
        /// Function to know if the checkout is available.
        /// </summary>
        /// <returns></returns>
        public bool Is_available()
        {
            if (is_open)
            {
                if (max_client >= Client_waiting.Count())
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Function to add a client in the waiting list.
        /// </summary>
        /// <param name="client_comming">the object that contain the client that is comming.</param>
        public void Client_arrived_at_checkout(Client client_comming)
        {
            if (!Client_waiting.Any(c => c.name == client_comming.name))
            {
                Client_waiting.Add(client_comming);
            }
        }

        /// <summary>
        /// Function to know whether the waiting list is full.
        /// </summary>
        /// <returns></returns>
        public bool is_full()
        {
            if (max_client == Client_waiting.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        /// <summary>
        /// Function to remove a client of the "client_waiting" list.
        /// </summary>
        /// <returns></returns>
        public bool Checking_out()
        {
            if (Client_waiting.Count() > 0)
            {
                if (Client_waiting[0].Checkout(position_y))
                {
                    Client_waiting.RemoveAt(0);
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Function to check the time left before changing the status.
        /// </summary>
        public void time_check()
        {
            if (status_changing)
            {
                if (Client_waiting.Count <= 0)
                {
                    left_to_change--;
                    Console.WriteLine(name + " status will change in : " + left_to_change);
                    if (left_to_change <= 0 && status_changing)
                    {
                        is_open = !is_open;
                        status_changing = false;
                    }
                }
            }
        }
    }
}
