using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Canary.Web.Models
{
    public class SquawkEventModel
    {
        private static readonly SHA1Managed Hasher = new SHA1Managed();

        public string App { get; set; }
        public string Env { get; set; }
        public int Level { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string User { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; }

        public string ComputeHash()
        {
            string value = Level + Type + Message + Source;

            byte[] data = System.Text.Encoding.ASCII.GetBytes(value);
            data = Hasher.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++)
                ret += data[i].ToString("x2").ToLower();
            return ret;
        }
    }

    public class AppDashboardModel
    {
        public string AppName { get; set; }
        public string EnvName { get; set; }
        public IEnumerable<EventSummaryModel> Events { get; set; }
    }

    public class EventSummaryModel
    {
        public string Hash { get; set; }
        public int Level { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public DateTime LastTimestamp { get; set; }
        public DateTime FirstTimestamp { get; set; }
        public int WeekTotal { get; set; }
        public int LifetimeTotal { get; set; }
    }

    public class FullEventModel
    {
        
    }
}