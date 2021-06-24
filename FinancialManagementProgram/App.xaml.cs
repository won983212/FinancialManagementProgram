using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace FinancialManagementProgram
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            base.OnStartup(e);

            Dispatcher.UnhandledException += (o, ev) =>
            {
                Logger.Error(ev.Exception);
                ev.Handled = true;
            };

            BinaryProperties.Load();
        }
    }
}
