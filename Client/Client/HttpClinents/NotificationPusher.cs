using Client.DataProviders;
using Client.Exeptions;
using Client.Models.ReturnedDTOs;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.HttpClinents
{
    public class NotificationPusher : HttpHelper, INotificationsPusher
    {
        public event Action<Notification> Pushed;
        HubConnection hubConnection;
        IHubProxy chatHubProxy;


        public NotificationPusher() : base("http://localhost:63377/api/Social/")
        {
            InitHub();
        }



        public async Task RemoveNotification(Notification notification)
        {
            var response = await httpClient.PostAsJsonAsync("RemoveNotification", notification);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    SetCurrentToken(response.Headers.GetValues("Token").FirstOrDefault());
                    break;
                case HttpStatusCode.Unauthorized:
                    throw new TokenExpiredExeption();
            }
        }

        private void InitHub()
        {
            //hubConnection = new HubConnection("http://localhost:52527/");
            //chatHubProxy = hubConnection.CreateHubProxy("ChatHub");
            //hubConnection.Start().Wait();

            //chatHubProxy.On<Notification>("Notification", OnPushed);
            //    (Notification newNotification) =>
            //{
            //    Pushed(newNotification);

            //    //await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            //    //{
            //    //    await new MessageDialog(message).ShowAsync();

            //    //});
            //});
        }

        private void OnPushed(Notification notification)
        {
            Pushed?.Invoke(notification);
        }
    }
}
