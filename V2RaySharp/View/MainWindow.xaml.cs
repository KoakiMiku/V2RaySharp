using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using V2RaySharp.Controller;
using V2RaySharp.Utiliy;

namespace V2RaySharp.View
{
    public partial class MainWindow : Window
    {
        private static readonly string name = "V2RaySharp";

        public MainWindow() => InitializeComponent();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                buttonSwitch.Content = I18N.GetString("Status");
                buttonRoute.Content = I18N.GetString("Status");
                buttonNode.Content = I18N.GetString("ChangeNode");
                Node.CompleteEvent += Complete;
                Configuration.Load();
                Node.Upgrade();
                UpgradeStatus(false);
                listBoxNode.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Escape)
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonSwitch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Task.Run(() => SnakeBarMessage(I18N.GetString("PleaseWait")));
                if (V2Ray.IsRunning())
                {
                    Task.Run(() => V2Ray.Stop());
                }
                else
                {
                    Task.Run(() => V2Ray.Start());
                }
                Task.Run(() => UpgradeStatus(true));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonNode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listBoxNode.SelectedItem != null)
                {
                    Task.Run(() => SnakeBarMessage(I18N.GetString("PleaseWait")));
                    var name = listBoxNode.SelectedItem.ToString();
                    Task.Run(() => V2Ray.ChangeNode(name));
                    Task.Run(() => UpgradeStatus(true));
                }
                else
                {
                    throw new Exception(I18N.GetString("NodeNotSelect"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonRoute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listBoxNode.SelectedItem != null)
                {
                    Task.Run(() => SnakeBarMessage(I18N.GetString("PleaseWait")));
                    var name = listBoxNode.SelectedItem.ToString();
                    Task.Run(() => V2Ray.ChangeRoute(name));
                    Task.Run(() => UpgradeStatus(true));
                }
                else
                {
                    throw new Exception(I18N.GetString("NodeNotSelect"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonListen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listBoxNode.SelectedItem != null)
                {
                    Task.Run(() => SnakeBarMessage(I18N.GetString("PleaseWait")));
                    var name = listBoxNode.SelectedItem.ToString();
                    Task.Run(() => V2Ray.ChangeListen(name));
                    Task.Run(() => UpgradeStatus(true));
                }
                else
                {
                    throw new Exception(I18N.GetString("NodeNotSelect"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Complete(long tick)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (tick == -2)
                {
                    labelUpgrade.Content = I18N.GetString("NoSubscription");
                    Task.Run(() => SnakeBarMessage(I18N.GetString("NoSubscription")));
                }
                else if (tick == -1)
                {
                    labelUpgrade.Content = I18N.GetString("UpgradeNodeError");
                    Task.Run(() => SnakeBarMessage(I18N.GetString("UpgradeNodeError")));
                }
                else
                {
                    var dateTime = new DateTime(Configuration.Config.UpgradeTime);
                    labelUpgrade.Content = $"{I18N.GetString("Upgrade")}: " +
                        $"{dateTime:yyyy.MM.dd HH:mm:ss}";

                    if ((DateTime.Now - dateTime).TotalSeconds < 2)
                    {
                        Task.Run(() => SnakeBarMessage(I18N.GetString("UpgradeCompleted")));
                    }
                }
                listBoxNode.Items.Clear();
                var vmesses = Node.vmesses.Select(x => x.Name).OrderBy(y => y).ToList();
                foreach (var item in vmesses)
                {
                    listBoxNode.Items.Add(item);
                }
                if (listBoxNode.Items.Count != 0)
                {
                    listBoxNode.SelectedItem = V2Ray.SelectNode();
                    listBoxNode.ScrollIntoView(listBoxNode.SelectedItem);
                    listBoxNode.Focus();
                }
            }));
        }

        private void UpgradeStatus(bool isWait)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                buttonSwitch.IsEnabled = false;
                buttonRoute.IsEnabled = false;
                buttonNode.IsEnabled = false;
                buttonListen.IsEnabled = false;
                labelStatus.Content = $"{I18N.GetString("Waiting")}";
                labelStatus.Foreground = labelUpgrade.Foreground;
            }));
            if (isWait)
            {
                Task.Delay(2000).Wait();
            }
            Dispatcher.Invoke(new Action(() =>
            {
                var isRunning = V2Ray.IsRunning();
                var isUsingRoute = V2Ray.IsUsingRoute();
                var isListenHostOnly = V2Ray.IsListenHostOnly();
                if (isRunning)
                {
                    buttonSwitch.Content = I18N.GetString("Stop");
                }
                else
                {
                    buttonSwitch.Content = I18N.GetString("Start");
                }
                if (isUsingRoute)
                {
                    buttonRoute.Content = I18N.GetString("Global");
                }
                else
                {
                    buttonRoute.Content = I18N.GetString("Route");
                }
                if (isListenHostOnly)
                {
                    buttonListen.Content = I18N.GetString("AllowAny");
                }
                else
                {
                    buttonListen.Content = I18N.GetString("HostOnly");
                }
                if (isRunning)
                {
                    if (isUsingRoute && isListenHostOnly)
                    {
                        labelStatus.Content = $"{I18N.GetString("RunningStatus")}: " +
                            $"{I18N.GetString("Route")}, {I18N.GetString("HostOnly")}";
                        labelStatus.Foreground = Brushes.DarkCyan;
                    }
                    else if (isUsingRoute && !isListenHostOnly)
                    {
                        labelStatus.Content = $"{I18N.GetString("RunningStatus")}: " +
                            $"{I18N.GetString("Route")}, {I18N.GetString("AllowAny")}";
                        labelStatus.Foreground = Brushes.DarkCyan;
                    }
                    else if (!isUsingRoute && isListenHostOnly)
                    {
                        labelStatus.Content = $"{I18N.GetString("RunningStatus")}: " +
                            $"{I18N.GetString("Global")}, {I18N.GetString("HostOnly")}";
                        labelStatus.Foreground = Brushes.DeepPink;
                    }
                    else
                    {
                        labelStatus.Content = $"{I18N.GetString("RunningStatus")}: " +
                            $"{I18N.GetString("Global")}, {I18N.GetString("AllowAny")}";
                        labelStatus.Foreground = Brushes.DeepPink;
                    }
                }
                else
                {
                    labelStatus.Content = $"{I18N.GetString("RunningStatus")}: " +
                        $"{I18N.GetString("Stoped")}";
                    labelStatus.Foreground = labelUpgrade.Foreground;
                }
                buttonSwitch.IsEnabled = true;
                buttonRoute.IsEnabled = true;
                buttonNode.IsEnabled = true;
                buttonListen.IsEnabled = true;
            }));
        }

        private void SnakeBarMessage(string message)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var snackbarMessage = new SnackbarMessage();
                snackbarMessage.Content = message;
                snackbar.Message = snackbarMessage;
                snackbar.IsActive = true;
            }));
            Task.Delay(2000).Wait();
            Dispatcher.Invoke(new Action(() =>
            {
                var snackbarMessage = new SnackbarMessage();
                snackbarMessage.Content = string.Empty;
                snackbar.Message = snackbarMessage;
                snackbar.IsActive = false;
            }));
        }
    }
}
