using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.System.Power;
using Windows.UI.Notifications;

namespace DisconnectTask
{
    public sealed class DIsconnectNotifier: IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral backgroundTaskDeferral = taskInstance.GetDeferral();
            SendDisconnectNotification();
            backgroundTaskDeferral.Complete();
        }

        private void SendDisconnectNotification()
        {
            if (PowerManager.RemainingChargePercent == 100)
            {
                string toastContent = "Battery 100% charged disconnect charger now.";
                string toastPayload = $@"
                                        <toast>
                                            <visual>
                                                <binding template='ToastGeneric'>
                                                    <text>Disconnect Charger</text>
                                                    <text>{toastContent}</text>
                                                </binding>
                                            </visual>
                                        </toast>";
                XmlDocument xmlPayload = new XmlDocument();
                xmlPayload.LoadXml(toastPayload);

                ToastNotification toastNotification = new ToastNotification(xmlPayload);
                ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
            }
        }
    }
}
