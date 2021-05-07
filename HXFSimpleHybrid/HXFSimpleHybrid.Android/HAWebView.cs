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

            Control.Settings.JavaScriptEnabled = true;
            Control.ClearCache(true);
            Control.Settings.SetAppCacheEnabled(true);
            var x = new MyWebClient(mContext);

            Control.Settings.MediaPlaybackRequiresUserGesture = false;
            Control.Settings.UserAgentString = "Android";
            Control.SetWebChromeClient(x);

            //if(Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            //{
            //    Control.EvaluateJavascript("setTimeout(() => { fetch_lang('en') }, 3000)", null);
            //}
            //else
            //{
            //    Control.LoadUrl("javascript:setTimeout(() => { fetch_lang('en') }, 3000)");
            //}

            //Control.Settings.UserAgentString = "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.0.4) Gecko/20100101 Firefox/4.0";
        }

        private void HandleElementNavigating(object sender, WebNavigatingEventArgs e)
        {
            _lastNavigationEvent = e.NavigationEvent;
            _lastSource = e.Source;
            _lastUrl = e.Url;
        }

        public class MyWebClient : WebChromeClient
        {
            Activity mContext;
            public MyWebClient(Activity context)
            {
                this.mContext = context;

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