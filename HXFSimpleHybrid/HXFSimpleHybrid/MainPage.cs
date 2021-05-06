using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HXFSimpleHybrid
{
    public class MainPage : ContentPage
    {
        WebView wb;
        ActivityIndicator ai;
        public MainPage()
        {
            wb = new WebView
            {
                //Source = "http://127.0.0.1:"+ Config.port + @"/",
                IsVisible = true
            };

            Content = wb;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Task.Run(() => Core.HHttpServer.Start());

            //Task.Delay(3000).ContinueWith(task => {
            //    Device.BeginInvokeOnMainThread(() => {
            //        ai.IsVisible = false;
            //        ai.IsRunning = false;

            //        wb.Source = "http://127.0.0.1:" + Config.port + @"/";
            //        wb.IsVisible = true;
            //    });
            //});

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