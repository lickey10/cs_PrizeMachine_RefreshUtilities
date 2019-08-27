using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public System.Windows.Forms.Button ButtonToClick;
    }
}
