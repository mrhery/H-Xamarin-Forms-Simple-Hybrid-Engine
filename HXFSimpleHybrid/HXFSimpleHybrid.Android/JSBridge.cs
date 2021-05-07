

using Android.Webkit;
using Java.Interop;
using System;

namespace HXFSimpleHybrid.Droid
{
    public class JSBridge : Java.Lang.Object
    {
        readonly WeakReference<HAWebView> hybridWebViewRenderer;

        public JSBridge(HAWebView hybridRenderer)
        {
            hybridWebViewRenderer = new WeakReference<HAWebView>(hybridRenderer);
        }

        [JavascriptInterface]
        [Export("invokeAction")]
        public void InvokeAction(string data)
        {
            HAWebView hybridRenderer;

            if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
            {
                ((HWebView)hybridRenderer.Element).InvokeAction(data);
            }
        }
    }
} 