using System.Reflection;
using System.Windows;

namespace ProductRationing
{
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(
$@"Ой, что-то пошло не так.. Покажите текст ошибки разработчикам:
{e.Exception.Message}",
"Необработанное исключение");
        }
    }
}
