using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Navigation;

namespace FinancialManagementProgram.kftcAPI
{
    public partial class AuthWindow : Window
    {
        public delegate void AuthCallback(string query);

        private AuthCallback callback;

        public AuthWindow(string url, AuthCallback callback)
        {
            InitializeComponent();
            webBrowser.Source = new Uri(url);
            Closed += AuthWindow_Closed;
            this.callback = callback;
        }

        private void AuthWindow_Closed(object sender, EventArgs e)
        {
            if (callback != null)
                callback(null);
        }

        private void webBrowser_NavigationStarting(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            Uri uri = new Uri(e.Uri);
            if (uri.Host == "localhost")
            {
                if (callback != null)
                {
                    callback(uri.Query);
                    callback = null;
                }

                e.Cancel = true;
                Close();
            }
        }
    }
}
