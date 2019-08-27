using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RefreshUtilities
{
    public class RefreshUtilities
    {
        System.Windows.Forms.Timer goToURLTimer = new System.Windows.Forms.Timer();
        Random rnd = new Random();

        public event EventHandler ClickComplete;
        public event EventHandler CallMethodComplete;
        public event EventHandler GoToUrlComplete;
        public event EventHandler Error;

        public bool IsActive
        {
            get
            {
                return isActive;
            }
        }

        public bool IsDisabled
        {
            get
            {
                return isDisabled;
            }
            set
            {
                isDisabled = value;

                if (isDisabled)
                    Cancel();
            }
        }

        private bool isActive = false;
        private bool isDisabled = false;

        #region examples of usage
        //private void RefreshUtilities_GoToUrlComplete(object sender, EventArgs e)
        //{
        //    if (sender != null && sender is RefreshUtilities.TimerInfo && ((RefreshUtilities.TimerInfo)sender).Browser is ExtendedWebBrowser)
        //    {
        //        ExtendedWebBrowser tempBrowser = (ExtendedWebBrowser)((RefreshUtilities.TimerInfo)sender).Browser;

        //        if (tempBrowser.IsBusy)
        //            tempBrowser.Stop();

        //        tempBrowser.Url = new Uri(((RefreshUtilities.TimerInfo)sender).UrlToGoTo);
        //    }
        //}

        //private void RefreshUtilities_ClickComplete(object sender, EventArgs e)
        //{
        //    numberOfPrizesEntered++;
        //    txtPrizeCount.Text = numberOfPrizesEntered.ToString();

        //    foundPrize = false;
        //    enteredTheContest = false;

        //    refreshUtilities.GoToURL("https://www.prizecraze.com/", txtRefreshTimer, mainBrowser);
        //}
        #endregion

        public RefreshUtilities()
        {
            goToURLTimer.Enabled = true;
            goToURLTimer.Tick += Timer_Tick; ;
            goToURLTimer.Interval = 1000;//one second
            goToURLTimer.Stop();
        }
        
        private double randomSeconds(int seconds,int plusMinus)
        {
            int secondsLow = seconds - plusMinus;
            int secondsHigh = seconds + plusMinus;

            if (secondsLow < 0)
                secondsLow = 0;

            double rndSeconds = rnd.Next(secondsLow, secondsHigh);

            double percentage = rnd.Next(1, 100);

            if (percentage > 80)
                rndSeconds = rndSeconds + (((rnd.Next(1, 15))));
            else if (percentage > 60)
                rndSeconds = rndSeconds + ((rnd.Next(1, 10)));
            else if (percentage > 30)
                rndSeconds = rndSeconds + (((rnd.Next(1, 5))));

            return rndSeconds;
        }

        /// <summary>
        /// Stop all timers and null  the tags
        /// </summary>
        public void Cancel()
        {
            goToURLTimer.Stop();
            goToURLTimer.Tag = null;
        }

        public void GoToURL(string URL, System.Windows.Forms.Label lblDisplay, object browser)
        {
            GoToURL(URL, 12, lblDisplay, browser);
        }

        public void GoToURL(string URL, int refreshSeconds, System.Windows.Forms.Label lblDisplay, object browser)
        {
            GoToURL(URL, refreshSeconds, (int)(refreshSeconds/4), lblDisplay, browser);
        }

        public void GoToURL(string URL, int refreshSeconds, int plusMinusRefreshSeconds, System.Windows.Forms.Label lblDisplay, object browser)
        {
            GoToURL(URL, refreshSeconds, plusMinusRefreshSeconds, false, lblDisplay, browser);
        }

        public void GoToURL(string URL, bool OverrideCurrentRequests, System.Windows.Forms.Label lblDisplay, object browser)
        {
            GoToURL(URL, 12, OverrideCurrentRequests, lblDisplay, browser);
        }

        public void GoToURL(string URL, int refreshSeconds, bool OverrideCurrentRequests, System.Windows.Forms.Label lblDisplay, object browser)
        {
            GoToURL(URL, refreshSeconds, (int)(refreshSeconds / 4), OverrideCurrentRequests, lblDisplay, browser);
        }

        public void GoToURL(string URL, int refreshSeconds, int plusMinusRefreshSeconds, bool OverrideCurrentRequests, System.Windows.Forms.Label lblDisplay, object browser)
        {
            try
            {
                if (!isDisabled)
                {
                    if (OverrideCurrentRequests)
                        goToURLTimer.Tag = null;

                    if (goToURLTimer.Tag == null)
                    {
                        goToURLTimer.Stop();

                        //this is how long before the link is clicked
                        TimerInfo timerInfo = new TimerInfo();
                        timerInfo.StartTime = DateTime.Now;
                        timerInfo.UrlToGoTo = URL;
                        timerInfo.Duration = TimeSpan.FromSeconds(randomSeconds(refreshSeconds, plusMinusRefreshSeconds));
                        timerInfo.LblDisplay = lblDisplay;
                        timerInfo.Browser = browser;

                        goToURLTimer.Tag = timerInfo;
                        goToURLTimer.Tick += Timer_Tick;
                        goToURLTimer.Start();

                        isActive = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Tools.WriteToFile(ex);
                throw;
                //Application.Restart();
            }
        }
        
        public void ClickElement(System.Windows.Forms.HtmlElement ElementToClick, System.Windows.Forms.Label lblDisplay)
        {
            ClickElement(ElementToClick, 11, lblDisplay);
        }

        public void ClickElement(System.Windows.Forms.HtmlElement ElementToClick, bool OverrideCurrentRequests, System.Windows.Forms.Label lblDisplay)
        {
            ClickElement(ElementToClick, 11, OverrideCurrentRequests, lblDisplay);
        }

        public void ClickElement(System.Windows.Forms.HtmlElement ElementToClick, int refreshSeconds, System.Windows.Forms.Label lblDisplay)
        {
            ClickElement(ElementToClick, refreshSeconds, refreshSeconds / 4, lblDisplay);
        }

        public void ClickElement(System.Windows.Forms.HtmlElement ElementToClick, int refreshSeconds, bool OverrideCurrentRequests, System.Windows.Forms.Label lblDisplay)
        {
            ClickElement(ElementToClick, refreshSeconds, refreshSeconds / 4, OverrideCurrentRequests, lblDisplay);
        }

        public void ClickElement(System.Windows.Forms.HtmlElement ElementToClick, int refreshSeconds, int refreshSecondsPlusMinus, System.Windows.Forms.Label lblDisplay)
        {
            ClickElement(ElementToClick, refreshSeconds, refreshSecondsPlusMinus, false, lblDisplay);
        }
        
        public void ClickElement(System.Windows.Forms.HtmlElement ElementToClick, int refreshSeconds, int refreshSecondsPlusMinus, bool OverrideCurrentRequests, System.Windows.Forms.Label lblDisplay)
        {
            try
            {
                if (!isDisabled)
                {
                    if (OverrideCurrentRequests)
                        goToURLTimer.Tag = null;

                    if (goToURLTimer.Tag == null)
                    {
                        goToURLTimer.Stop();

                        //this is how long before the link is clicked

                        TimerInfo timerInfo = new TimerInfo();
                        timerInfo.StartTime = DateTime.Now;
                        timerInfo.ElementToClick = ElementToClick;
                        timerInfo.Duration = TimeSpan.FromSeconds(randomSeconds(refreshSeconds, refreshSecondsPlusMinus));
                        timerInfo.LblDisplay = lblDisplay;

                        goToURLTimer.Tag = timerInfo;
                        goToURLTimer.Tick += Timer_Tick;
                        goToURLTimer.Start();

                        isActive = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Tools.WriteToFile(ex);
                throw;
                //Application.Restart();
            }
        }

        public void SubmitForm(System.Windows.Forms.Form FormToSubmit, int refreshSeconds, int refreshSecondsPlusMinus, System.Windows.Forms.Label lblDisplay)
        {
            try
            {
                if (!isDisabled)
                {
                    if (goToURLTimer.Tag == null)
                    {
                        goToURLTimer.Stop();

                        //this is how long before the link is clicked

                        TimerInfo timerInfo = new TimerInfo();
                        timerInfo.StartTime = DateTime.Now;
                        timerInfo.FormToSubmit = FormToSubmit;
                        timerInfo.Duration = TimeSpan.FromSeconds(randomSeconds(refreshSeconds, refreshSecondsPlusMinus));
                        timerInfo.LblDisplay = lblDisplay;

                        goToURLTimer.Tag = timerInfo;
                        goToURLTimer.Tick += Timer_Tick;
                        goToURLTimer.Start();

                        isActive = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Tools.WriteToFile(ex);
                throw;
                //Application.Restart();
            }
        }

        /// <summary>
        /// triggers CallMethodComplete after default time sending back the MethodToCall
        /// </summary>
        /// <param name="MethodToCall">the method name sent to CallMethodComplete</param>
        /// <param name="lblDisplay"></param>
        public void CallMethod(string MethodToCall, System.Windows.Forms.Label lblDisplay)
        {
            CallMethod(MethodToCall, null, 3, false, lblDisplay);
        }

        public void CallMethod(string MethodToCall, bool OverrideCurrentRequests, System.Windows.Forms.Label lblDisplay)
        {
            CallMethod(MethodToCall, null, 3, OverrideCurrentRequests, lblDisplay);
        }

        public void CallMethod(string MethodToCall, int refreshSeconds, System.Windows.Forms.Label lblDisplay)
        {
            CallMethod(MethodToCall, null, refreshSeconds, false, lblDisplay);
        }

        public void CallMethod(string MethodToCall, int refreshSeconds, bool OverrideCurrentRequests, System.Windows.Forms.Label lblDisplay)
        {
            CallMethod(MethodToCall, null, refreshSeconds, OverrideCurrentRequests, lblDisplay);
        }

        public void CallMethod(string MethodToCall, ArrayList MethodArguments, System.Windows.Forms.Label lblDisplay)
        {
            CallMethod(MethodToCall, MethodArguments, 3, false, lblDisplay);
        }

        public void CallMethod(string MethodToCall, ArrayList MethodArguments, bool OverrideCurrentRequests, System.Windows.Forms.Label lblDisplay)
        {
            CallMethod(MethodToCall, MethodArguments, 3, OverrideCurrentRequests, lblDisplay);
        }

        public void CallMethod(string MethodToCall, ArrayList MethodArguments, int refreshSeconds, bool OverrideCurrentRequests, System.Windows.Forms.Label lblDisplay)
        {
            try
            {
                if (!isDisabled)
                {
                    if (OverrideCurrentRequests)
                        goToURLTimer.Tag = null;

                    if (goToURLTimer.Tag == null)
                    {
                        goToURLTimer.Stop();

                        //this is how long before the link is clicked
                        TimerInfo timerInfo = new TimerInfo();
                        timerInfo.StartTime = DateTime.Now;
                        timerInfo.MethodToCall = MethodToCall;
                        timerInfo.MethodArguments = MethodArguments;
                        timerInfo.Duration = TimeSpan.FromSeconds(refreshSeconds);
                        timerInfo.LblDisplay = lblDisplay;

                        goToURLTimer.Tag = timerInfo;
                        goToURLTimer.Tick += Timer_Tick;
                        goToURLTimer.Start();

                        isActive = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Tools.WriteToFile(ex);
                throw;
                //Application.Restart();
            }
        }

        public void ClickButton(System.Windows.Forms.Button ButtonToClick, int refreshSeconds, bool OverrideCurrentRequests, System.Windows.Forms.Label lblDisplay)
        {
            try
            {
                if (!isDisabled)
                {
                    if (OverrideCurrentRequests)
                        goToURLTimer.Tag = null;

                    if (goToURLTimer.Tag == null)
                    {
                        goToURLTimer.Stop();

                        //this is how long before the link is clicked
                        TimerInfo timerInfo = new TimerInfo();
                        timerInfo.StartTime = DateTime.Now;
                        timerInfo.ButtonToClick = ButtonToClick;
                        timerInfo.Duration = TimeSpan.FromSeconds(refreshSeconds);
                        timerInfo.LblDisplay = lblDisplay;

                        goToURLTimer.Tag = timerInfo;
                        goToURLTimer.Tick += Timer_Tick;
                        goToURLTimer.Start();

                        isActive = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Tools.WriteToFile(ex);
                throw;
                //Application.Restart();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (((System.Windows.Forms.Timer)sender).Tag is TimerInfo && !isDisabled)
                {
                    TimerInfo timerInfo = (TimerInfo)((System.Windows.Forms.Timer)sender).Tag;
                    TimeSpan elapsedTime = DateTime.Now - timerInfo.StartTime;
                    //int elapsedMilliseconds = ((int)((double)elapsedTime.Seconds) * 1000);

                    if (elapsedTime < timerInfo.Duration)
                    {
                        if (timerInfo.LblDisplay != null && timerInfo.LblDisplay is System.Windows.Forms.Label)
                            timerInfo.LblDisplay.Text = ((int)(timerInfo.Duration.TotalSeconds - elapsedTime.TotalSeconds)).ToString() + " seconds";
                    }
                    else //timer is expired
                    {
                        ((System.Windows.Forms.Timer)sender).Tag = null;
                        ((System.Windows.Forms.Timer)sender).Stop();

                        isActive = false;

                        if (timerInfo.LblDisplay != null && timerInfo.LblDisplay is System.Windows.Forms.Label)
                            timerInfo.LblDisplay.Text = "0 seconds";

                        if (timerInfo.UrlToGoTo.Trim().Length > 0 && timerInfo.Browser != null && timerInfo.Browser is object)
                        {
                            if(!timerInfo.UrlToGoTo.ToLower().StartsWith("http"))
                            {
                                timerInfo.UrlToGoTo = "http://" + timerInfo.UrlToGoTo;
                            }

                            EventHandler handler = GoToUrlComplete;
                            if (handler != null)
                            {
                                handler(timerInfo, e);
                            }
                        }
                        else if (timerInfo.ElementToClick != null)//click an html button or link
                        {
                            timerInfo.ElementToClick.InvokeMember("Click");

                            EventHandler handler = ClickComplete;
                            if (handler != null)
                            {
                                handler(timerInfo.ElementToClick, e);
                            }

                            timerInfo.ElementToClick = null;
                        }
                        else if (timerInfo.MethodToCall != null)//call a method
                        {
                            EventHandler handler = CallMethodComplete;
                            if (handler != null)
                            {
                                handler(timerInfo, e);
                            }
                        }
                        else if(timerInfo.FormToSubmit != null)
                        {
                            //timerInfo.FormToSubmit.InvokeMember("Submit");

                            //EventHandler handler = ClickComplete;
                            //if (handler != null)
                            //{
                            //    handler(timerInfo.ElementToClick, e);
                            //}

                            //timerInfo.ElementToClick = null;
                        }
                        else if(timerInfo.ButtonToClick != null)
                        {
                            timerInfo.ButtonToClick.PerformClick();

                            timerInfo.ButtonToClick = null;
                        }
                    }
                }
                else
                {
                    ((System.Windows.Forms.Timer)sender).Tag = null;
                    ((System.Windows.Forms.Timer)sender).Stop();

                    isActive = false;
                }
            }
            catch (Exception ex)
            {
                string found = ex.Message;
                //Tools.WriteToFile(ex);
                //throw;
                //Application.Restart();

                EventHandler handler = Error;
                if (handler != null)
                {
                    handler(ex, e);
                }
            }
        }
    }
}
