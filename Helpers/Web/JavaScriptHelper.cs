using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Helpers.Web
{
    public static class JavaScriptHelper
    {
        public static async Task<string> InvokeScriptAsync(WebView webView, string javascript)
        {
            return await webView.InvokeScriptAsync("eval", new string[] { javascript });
        }
    }
}
