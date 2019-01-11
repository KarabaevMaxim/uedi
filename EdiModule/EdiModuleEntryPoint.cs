namespace EdiModule
{
    using System.Runtime.InteropServices;
    using Windows;

    [Guid("4E38FF4C-391E-448F-92F8-241D1BE55408"), ClassInterface(ClassInterfaceType.None), ComSourceInterfaces(typeof(IEvents))]
    public class EdiModuleEntryPoint : IEntryPoint
    {
        public EdiModuleEntryPoint() { }

        public void MainWindowShow()
        {
            //EdiModuleCore.CoreInit.Init();
            //EdiModuleCore.SettingsManager.LoadSettings();
            //MainWindow window = new MainWindow();
            //window.ShowDialog();

            EdiModuleCore.SettingsManager.LoadSettings();
            LoginWindow window = new LoginWindow();
            window.ShowDialog();
        }
    }
}
