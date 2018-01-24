using System;
using Windows.ApplicationModel.Background;
using Windows.Devices.Power;
using Windows.System.Power;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace Full_Charge
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            GetBatteryReport();
            Battery.AggregateBattery.ReportUpdated += AggregateBattery_ReportUpdated;
            RegisterBakgroundTask();
        }

        private async void AggregateBattery_ReportUpdated(Battery sender, object args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {
                 GetBatteryReport();
             });
        }

        private void GetBatteryReport()
        {
            BatteryPercentage.Text = PowerManager.RemainingChargePercent.ToString() + "%";
            BatteryState.Text = PowerManager.BatteryStatus.ToString();
        }

        private const string taskName = "FullChargeDisconnect";
        private const string taskEntryPoint = "DisconnectTask.DIsconnectNotifier";

        private async void RegisterBakgroundTask()
        {
            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            if (backgroundAccessStatus == BackgroundAccessStatus.AllowedSubjectToSystemPolicy || backgroundAccessStatus == BackgroundAccessStatus.AlwaysAllowed)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == taskName)
                    {
                        task.Value.Unregister(true);
                    }
                }

                var taskBuilder = new BackgroundTaskBuilder
                {
                    Name = taskName,
                    TaskEntryPoint = taskEntryPoint
                };
                taskBuilder.SetTrigger(new TimeTrigger(15, false));
                BackgroundTaskRegistration regesteration = taskBuilder.Register();
            }
        }
    }
}
