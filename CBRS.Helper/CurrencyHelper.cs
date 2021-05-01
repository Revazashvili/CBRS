using System;

namespace CBRS.Helper
{
    public static class CurrencyHelper
    {
        // წამოღებულია ძველი სოფტიდან
        public static string get_cur5(string val, string scale, string number_len)
        {
            int number_length = 4;

            try
            {
                number_length = Convert.ToInt32(number_len);
            }
            catch
            {
                number_length = 4;
            }

            string s1 = (Convert.ToDouble(val) / Convert.ToDouble(scale)).ToString();

            string s = s1;
            string index = Math.PI.ToString().Substring(1, 1);
            int wertilis_num = s1.IndexOf(index[0]);
            if (wertilis_num == -1)
            {
                s = s1 = s1 + index[0] + "000000";
                wertilis_num = s1.IndexOf(index[0]);
            }
            else
            {
                s = s1 = s1 + "000000";
            }

            if (wertilis_num > (number_length - 1))
            {
                s = s.Substring(0, number_length);
                int k = number_length;
                if (s1[number_length] == index[0])
                    k = number_length + 1;
                s1 = s1 + "000000";
                if (s1[k] > '4')
                {
                    Int32 r = Convert.ToInt32(s);
                    r = r + 1;
                    s = r.ToString().Substring(0, number_length);
                }
            }
            else
            {
                s = s.Substring(0, number_length + 1);
                if (s1[number_length + 1] > '4')
                {
                    double r = Convert.ToDouble(s);
                    s = (r + Math.Pow(10.0, wertilis_num - number_length)).ToString();
                    wertilis_num = s.IndexOf(index[0]);
                    if (wertilis_num == -1)
                    {
                        s = s + index[0] + "000000";
                        wertilis_num = s.IndexOf(index[0]);
                    }
                    else
                    {
                        s = s + "000000";
                    }

                    if (wertilis_num == -1 || wertilis_num <= (number_length - 1))
                    {
                        s = s.Substring(0, number_length + 1);
                    }
                    else
                    {
                        s = s.Substring(0, number_length);
                    }
                }
            }

            return s;
        }
    }
}