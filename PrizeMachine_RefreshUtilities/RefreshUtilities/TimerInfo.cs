using System;
using System.Collections;

namespace RefreshUtilities
{
    public class TimerInfo
    {
        public DateTime StartTime;
        public TimeSpan Duration;
        public string MethodToCall = "";
        public ArrayList MethodArguments;
        public string UrlToGoTo = "";
        public System.Windows.Forms.Label LblDisplay = null;
        public object Browser = null;
        public System.Windows.Forms.HtmlElement ElementToClick = null;
        public System.Windows.Forms.Form FormToSubmit;
    }
}
