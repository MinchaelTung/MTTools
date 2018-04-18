using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MTFramework.CreaditTerm.Properties;

namespace MTFramework.CreaditTerm
{
    public class CreditTermTools
    {
        public static string Interpretation(string code)//<IN30@15,0/W>
        {
            string pattern = Resources.RegexPattern;//@"^<?[a-zA-Z]+\d+(@\d+(,\d+)*)?(/[a-zA-Z]+)?>?$";
            if (!Regex.IsMatch(code, pattern))
            {
                return Resources.CodeError;
            }
            string str2 = code.Trim(new char[] { '<', '>' });
            string baseDay = string.Empty;
            string cutOffDay = string.Empty;
            string delay = string.Empty;
            string[] strArray = str2.Split(new char[] { '/' });
            if (strArray.Length > 1)
            {
                delay = strArray[1];
            }
            strArray = strArray[0].Split(new char[] { '@' });
            if (strArray.Length > 1)
            {
                cutOffDay = strArray[1];
            }
            baseDay = strArray[0];
            baseDay = interpretationBaseDay(baseDay);
            cutOffDay = interpretationCutOffDay(cutOffDay);
            delay = interpretationDelay(delay);
            return string.Format("{0}{1}{2}{3}{4}", new object[] { baseDay, (string.IsNullOrEmpty(baseDay) || string.IsNullOrEmpty(cutOffDay)) ? string.Empty : Environment.NewLine, cutOffDay, ((string.IsNullOrEmpty(baseDay) && string.IsNullOrEmpty(cutOffDay)) || string.IsNullOrEmpty(delay)) ? string.Empty : Environment.NewLine, delay });
        }

        private static string interpretationBaseDay(string baseDay)
        {
            if (string.IsNullOrEmpty(baseDay))
            {
                return string.Empty;
            }
            string str = string.Empty;
            string str2 = Regex.Match(baseDay, "[a-zA-Z]+").Value;
            string s = Regex.Match(baseDay, @"\d+").Value;
            try
            {
                int num = int.Parse(s);
                string str5 = str2.ToUpper();
                if (str5 != null)
                {
                    if (!(str5 == "IN"))
                    {
                        if (str5 == "RP")
                        {
                            goto Label_00CD;
                        }
                        if (str5 == "LC")
                        {
                            goto Label_00F9;
                        }
                        if (str5 != "SH")
                        {
                            return str;
                        }
                        goto Label_0125;
                    }
                    if (num == 0)
                    {
                        str = Resources.BaseDay_IN_Zero;
                    }
                    else
                    {
                        str = string.Format(Resources.BaseDay_IN_Title, num);
                    }
                }
                return str;
            Label_00CD:
                if (num == 0)
                {
                    str = Resources.BaseDay_RP_Zero;
                }
                else
                {
                    str = string.Format(Resources.BaseDay_RP_Title, num);
                }
                return str;
            Label_00F9:
                if (num == 0)
                {
                    str = Resources.BaseDay_LC_Zero;
                }
                else
                {
                    str = string.Format(Resources.BaseDay_LC_Title, num);
                }
                return str;
            Label_0125:
                if (num == 0)
                {
                    str = Resources.BaseDay_SH_Zero;
                }
                else
                {
                    str = string.Format(Resources.BaseDay_SH_Title, num);
                }
            }
            catch
            {
                return string.Empty;
            }
            return str;
        }

        private static string interpretationCutOffDay(string cutOffDay)
        {
            if (string.IsNullOrEmpty(cutOffDay))
            {
                return string.Empty;
            }
            string str = string.Empty;
            string[] strArray = cutOffDay.Split(new char[] { ',' });
            List<string> list = new List<string>();
            foreach (string str2 in strArray)
            {
                try
                {
                    int num = int.Parse(str2);
                    if (num == 0)
                    {
                        list.Add(Resources.CutOffDay_Last);
                    }
                    else
                    {
                        list.Add(string.Format(Resources.CutOffDay_Day, num));
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }
            strArray = list.ToArray();
            if (strArray.Length > 2)
            {
                str = string.Join(Resources.CutOffDay_Sign, strArray, 0, strArray.Length - 1);
                str = string.Format(Resources.XOR, str, strArray[strArray.Length - 1]);
            }
            else
            {
                str = string.Join(Resources.OR, strArray, 0, strArray.Length);
            }
            return string.Format(Resources.CutOffDay_Title, str);
        }

        private static string interpretationDelay(string delay)
        {
            if (string.IsNullOrEmpty(delay))
            {
                return string.Empty;
            }
            string str = string.Empty;
            switch (delay.ToUpper())
            {
                case "W":
                    return string.Format(Resources.Delay_Title, Resources.Delay_W);

                case "SUN":
                    return string.Format(Resources.Delay_Title, Resources.Delay_Sun);

                case "MON":
                    return string.Format(Resources.Delay_Title, Resources.Delay_Mon);

                case "TUE":
                    return string.Format(Resources.Delay_Title, Resources.Delay_Tue);

                case "WED":
                    return string.Format(Resources.Delay_Title, Resources.Delay_Wed);

                case "THUR":
                    return string.Format(Resources.Delay_Title, Resources.Delay_Thur);

                case "FRI":
                    return string.Format(Resources.Delay_Title, Resources.Delay_Fri);

                case "SAT":
                    return string.Format(Resources.Delay_Title, Resources.Delay_Sat);
            }
            return str;
        }


    }
}
