using Android.Annotation;
using Android.App;
using Android.Content;
using Android.Webkit;
using HXFSimpleHybrid;
using HXFSimpleHybrid.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HWebView), typeof(HAWebView))]
namespace HXFSimpleHybrid.Droid
{
    public class HAWebView : WebViewRenderer
    {
        const string JavascriptFunction = @"function HXFInvoke(data){jsBridge.invokeAction(data);}";
        private WebNavigationEvent _lastNavigationEvent;
        private WebViewSource _lastSource;
        private string _lastUrl;
        private new HWebView Element { get { return (HWebView)base.Element; } }
        public static Activity mContext;
        public HAWebView(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if(e.OldElement != null)
            {
                Control.RemoveJavascriptInterface("jsBridge");
                ((HWebView)Element).Cleanup();
            }

            if(e.NewElement != null)
            {
                Control.Settings.JavaScriptEnabled = true;
                Control.ClearCache(true);
                Control.Settings.SetAppCacheEnabled(true);

                var x = new MC(mContext, JavascriptFunction);
                
                Control.SetWebChromeClient(x);
                Control.Settings.MediaPlaybackRequiresUserGesture = false;
                Control.Settings.UserAgentString = "Android";
                Control.AddJavascriptInterface(new JSBridge(this), "jsBridge");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ((HWebView)Element).Cleanup();
            }

            base.Dispose(disposing);
        }

        private void HandleElementNavigating(object sender, WebNavigatingEventArgs e)
        {
            _lastNavigationEvent = e.NavigationEvent;
            _lastSource = e.Source;
            _lastUrl = e.Url;
        }

        public class MyWebClient : FormsWebViewClient
        {
            Activity mContext;
            string _javascript;
            public MyWebClient(HAWebView wb, string js) : base(wb)
            {
                //this.mContext = context;
                _javascript = js;

            }

            public override void OnPageFinished(Android.Webkit.WebView view, string url)
            {
                base.OnPageFinished(view, url);
                view.EvaluateJavascript(_javascript, null);
            }

            
        }

        public class MC : WebChromeClient
        {
            Activity m;
            string js;

            public MC(Activity _m, string _js)
            {
                m = _m;
                js = _js;
            }

            public override void OnProgressChanged(Android.Webkit.WebView view, int newProgress)
            {
                base.OnProgressChanged(view, newProgress);

                if(newProgress == 100)
                {
                    view.EvaluateJavascript(js, null);
                }
            }

            [TargetApi(Value = 21)]
            public override void OnPermissionRequest(PermissionRequest request)
            {
                mContext.RunOnUiThread(() =>
                {
                    request.Grant(request.GetResources());
                });

            }
        }
    }
}