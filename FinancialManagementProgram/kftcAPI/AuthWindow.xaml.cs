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

        private readonly AuthCallback callback;

        public AuthWindow(string url, AuthCallback callback)
        {
            InitializeComponent();
            webBrowser.Source = new Uri(url);
            this.callback = callback;
        }

        private void webBrowser_NavigationStarting(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            Uri uri = new Uri(e.Uri);
            if (uri.Host == "localhost")
            {
                e.Cancel = true;
                Close();

                if (callback != null)
                    callback(uri.Query);
            }
        }
    }
}
