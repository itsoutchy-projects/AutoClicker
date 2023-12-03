using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker
{
    public static class TimeEx
    {
        /// <summary>
        /// Converts a given time to milliseconds
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static long GetMilliseconds(int hours, int minutes, int seconds, int milliseconds)
        {
            return (long)((hours * 3.6e+6) + (minutes * 60000) + (seconds * 1000) + milliseconds);
        }
    }
}
