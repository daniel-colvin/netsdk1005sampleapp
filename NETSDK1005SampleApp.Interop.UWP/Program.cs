using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace NETSDK1005SampleApp.Interop.UWP
{
    class Program
    {
        static AppServiceConnection _connection = null;
        static AutoResetEvent _appServiceExit;

        static void Main(string[] args)
        {
            // connect to app service and wait until the connection gets closed
            _appServiceExit = new AutoResetEvent(false);
            InitializeAppServiceConnection();
            _appServiceExit.WaitOne();
        }

        static async void InitializeAppServiceConnection()
        {
            _connection = new AppServiceConnection();
            _connection.AppServiceName = "NETSDK1005SampleApp";
            _connection.PackageFamilyName = Windows.ApplicationModel.Package.Current.Id.FamilyName;
            _connection.ServiceClosed += Connection_ServiceClosed;

            AppServiceConnectionStatus status = await _connection.OpenAsync();
            if (status != AppServiceConnectionStatus.Success)
            {
                // TODO: error handling
            }
        }

        private static void Connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            // signal the event so the process can shut down
            _appServiceExit.Set();
        }
    }
}
