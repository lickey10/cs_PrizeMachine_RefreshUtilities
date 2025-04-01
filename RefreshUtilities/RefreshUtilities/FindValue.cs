using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RefreshUtilities
{
    public static class FindValue
    {
        public static string findValue(string stringToParse, string startPattern, string endPattern)
        {
            return findValue(stringToParse, startPattern, endPattern, false);
        }

        public static string findValue(string stringToParse, string startPattern, string endPattern, bool returnSearchPatterns)
        {
            int start = 0;
            int end = 0;
            string foundValue = "";

            try
            {
                start = stringToParse.IndexOf(startPattern);

                if (start > -1)
                {
                    if (!returnSearchPatterns)
                        stringToParse = stringToParse.Substring(start + startPattern.Length);
                    else
                        stringToParse = stringToParse.Substring(start);

                    if (endPattern == "")//go to the end of the string
                        foundValue = stringToParse;
                    else
                    {
                        end = stringToParse.IndexOf(endPattern);

                        if (end > 0)
                        {
                            if (returnSearchPatterns)
                                foundValue = stringToParse.Substring(0, end + endPattern.Length);
                            else
                                foundValue = stringToParse.Substring(0, end);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
                //Tools.WriteToFile(ex);
            }

            return foundValue;
        }
    }
}
