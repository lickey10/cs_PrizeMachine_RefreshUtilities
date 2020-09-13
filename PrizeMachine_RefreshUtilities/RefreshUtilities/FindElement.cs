namespace RefreshUtilities
{
    class FindElement
    {
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
    }
}
