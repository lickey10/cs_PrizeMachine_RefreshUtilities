using mshtml;
using SHDocVw;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RefreshUtilities
{
    public static class CrossFrameIE
    {
        private static FieldInfo ShimManager = typeof(HtmlWindow).GetField("shimManager", BindingFlags.NonPublic | BindingFlags.Instance);
        private static ConstructorInfo HtmlDocumentCtor = typeof(HtmlDocument).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[0];

        #region examples
        //private bool findNextPage(HtmlDocument pageDocument)
        //{
        //    foundPrize = findWatchWindowLinkElement(pageDocument.GetElementsByTagName("div"));

        //    if (!foundPrize)//didn't find the link in the page now check the frames
        //    {
        //        //iterate the frames and look for our elements
        //        foundPrize = findElementInFrames(pageDocument.Window.Frames);
        //    }

        //    return foundPrize;
        //}

        //private bool findElementInFrames(HtmlWindowCollection FramesCollection)
        //{
        //    bool foundElement = false;

        //    foreach (HtmlWindow frame in FramesCollection)
        //    {
        //        if (!foundElement)
        //            foundElement = findWatchWindowLinkElement(frame.GetDocument().GetElementsByTagName("div"));

        //        if (foundElement)
        //            break;

        //        if (frame.Frames.Count > 0)
        //        {
        //            foundElement = findElementInFrames(frame.Frames);

        //            if (foundElement)
        //                break;
        //        }
        //    }

        //    return foundElement;
        //}

        //private bool findWatchWindowLinkElement(HtmlElementCollection elc)
        //{
        //    foreach (HtmlElement el in elc)
        //    {
        //        if (el.GetAttribute("onclick").ToLower().Contains(".submit();") || (el.InnerHtml != null && el.InnerHtml.ToLower().Contains("<div class=\"price\">") && !el.InnerHtml.ToLower().Contains("<div class=\"duration\"")))
        //        {
        //            refreshUtilities.ClickElement(el, txtRefreshTimer);

        //            return true;
        //        }
        //    }

        //    return false;
        //}
        #endregion

        public static HtmlDocument GetDocument(this HtmlWindow window)
        {
            var rawDocument = (window.DomWindow as IHTMLWindow2).GetDocumentFromWindow();

            var shimManager = ShimManager.GetValue(window);

            var htmlDocument = HtmlDocumentCtor
                .Invoke(new[] { shimManager, rawDocument }) as HtmlDocument;

            return htmlDocument;
        }


        // Returns null in case of failure.
        public static IHTMLDocument2 GetDocumentFromWindow(this IHTMLWindow2 htmlWindow)
        {
            if (htmlWindow == null)
            {
                return null;
            }

            // First try the usual way to get the document.
            try
            {
                IHTMLDocument2 doc = htmlWindow.document;

                return doc;
            }
            catch (COMException comEx)
            {
                // I think COMException won't be ever fired but just to be sure ...
                if (comEx.ErrorCode != E_ACCESSDENIED)
                {
                    return null;
                }
            }
            catch (System.UnauthorizedAccessException)
            {
            }
            catch
            {
                // Any other error.
                return null;
            }

            // At this point the error was E_ACCESSDENIED because the frame contains a document from another domain.
            // IE tries to prevent a cross frame scripting security issue.
            try
            {
                // Convert IHTMLWindow2 to IWebBrowser2 using IServiceProvider.
                IServiceProvider sp = (IServiceProvider)htmlWindow;

                // Use IServiceProvider.QueryService to get IWebBrowser2 object.
                Object brws = null;
                sp.QueryService(ref IID_IWebBrowserApp, ref IID_IWebBrowser2, out brws);

                // Get the document from IWebBrowser2.
                IWebBrowser2 browser = (IWebBrowser2)(brws);

                return (IHTMLDocument2)browser.Document;
            }
            catch
            {
            }

            return null;
        }

        private const int E_ACCESSDENIED = unchecked((int)0x80070005L);
        private static Guid IID_IWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
        private static Guid IID_IWebBrowser2 = new Guid("D30C1661-CDAF-11D0-8A3E-00C04FC9E26E");
    }

    // This is the COM IServiceProvider interface, not System.IServiceProvider .Net interface!
    [ComImport(), ComVisible(true), Guid("6D5140C1-7436-11CE-8034-00AA006009FA"),
    InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IServiceProvider
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int QueryService(ref Guid guidService, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);
    }
}
