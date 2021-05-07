using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace HXFSimpleHybrid
{
    public class MainPage : ContentPage
    {
        HWebView wb;
        ActivityIndicator ai;
        public MainPage()
        {
            wb = new HWebView
            {
                //Source = "http://127.0.0.1:"+ Config.port + @"/",
                IsVisible = true
            };

            wb.Invoking(data => {
                Invoke(data);
            });

            Content = wb;
        }

        void Invoke(string data)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var ds = data.Split(new char[] { ':' });

                if (ds.Length > 1)
                {
                    switch (ds[0])
                    {
                        case "permission":
                            switch (ds[1])
                            {
                                case "camera":
                                    var x = Permissions.CheckStatusAsync<Permissions.Camera>();

                                    if (x.Result != PermissionStatus.Granted)
                                    {
                                        x = Permissions.RequestAsync<Permissions.Camera>();
                                    }
                                    break;

                                case "microphone":
                                    var x1 = Permissions.CheckStatusAsync<Permissions.Microphone>();

                                    if (x1.Result != PermissionStatus.Granted)
                                    {
                                        x1 = Permissions.RequestAsync<Permissions.Microphone>();
                                    }
                                    break;
                            }
                            break;
                    }
                }
            });

            return;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Task.Run(() => Core.HHttpServer.Start());
            
            Task.Run(() =>
            {
                while (true)
                {
                    if (Core.HHttpServer.IsRunning)
                    {
                        Device.BeginInvokeOnMainThread(() => {
                            wb.Source = "http://127.0.0.1:" + Config.port + @"/";
                            wb.IsVisible = true;
                        });
                        
                        break;
                    }
                }
            });
        }
    }
}