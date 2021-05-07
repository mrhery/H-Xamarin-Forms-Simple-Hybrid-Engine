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

            var status = CheckAndRequestPermissionAsync(new Permissions.Camera());
            if (status.Result != PermissionStatus.Granted)
            {
                DisplayAlert("Permission Error", "Requested Camera not alloed.", "Ok");
            }

            var mstatus = CheckAndRequestPermissionAsync(new Permissions.Microphone());
            if (mstatus.Result != PermissionStatus.Granted)
            {
                DisplayAlert("Permission Error", "Requested Camera not alloed.", "Ok");
            }

            Content = wb;
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

        public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
            where T : Permissions.BasePermission
        {
            var status = await permission.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
            }

            return status;
        }
    }
}