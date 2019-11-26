using MaterialDesignThemes.Wpf;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using V2RaySharp.Controller;
using V2RaySharp.Utiliy;

namespace V2RaySharp.View
{
    public partial class MainWindow : Window
    {
        private static readonly string name = "V2RaySharp";

        internal MainWindow() => this.InitializeComponent();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.buttonSwitch.Content = I18N.GetString("Status");
                this.buttonRoute.Content = I18N.GetString("Status");
                this.buttonNode.Content = I18N.GetString("ChangeNode");
                this.buttonConfig.Content = I18N.GetString("EditConfig");
                this.buttonLoopback.Content = I18N.GetString("EditLoopback");
                Node.CompleteEvent += this.Complete;
                Configuration.Load();
                Node.Upgrade();
                this.UpgradeStatus(false);
                this.listBoxNode.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Escape)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name, MessageBoxButton.OK, MessageBoxImage.Error);
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
                Task.Run(() => this.UpgradeStatus(true));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonNode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.listBoxNode.SelectedItem != null)
                {
                    Task.Run(() => SnakeBarMessage(I18N.GetString("PleaseWait")));
                    var name = this.listBoxNode.SelectedItem.ToString();
                    Task.Run(() => V2Ray.ChangeNode(name));
                    Task.Run(() => this.UpgradeStatus(true));
                }
                else
                {
                    throw new Exception(I18N.GetString("NodeNotSelect"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonRoute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.listBoxNode.SelectedItem != null)
                {
                    Task.Run(() => SnakeBarMessage(I18N.GetString("PleaseWait")));
                    var name = this.listBoxNode.SelectedItem.ToString();
                    Task.Run(() => V2Ray.ChangeRoute(name));
                    Task.Run(() => this.UpgradeStatus(true));
                }
                else
                {
                    throw new Exception(I18N.GetString("NodeNotSelect"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonListen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.listBoxNode.SelectedItem != null)
                {
                    Task.Run(() => SnakeBarMessage(I18N.GetString("PleaseWait")));
                    var name = this.listBoxNode.SelectedItem.ToString();
                    Task.Run(() => V2Ray.ChangeListen(name));
                    Task.Run(() => this.UpgradeStatus(true));
                }
                else
                {
                    throw new Exception(I18N.GetString("NodeNotSelect"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonConfig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Task.Run(() => SnakeBarMessage(I18N.GetString("PleaseWait")));
                Task.Run(() => V2Ray.EditConfig());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonLoopback_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Task.Run(() => SnakeBarMessage(I18N.GetString("PleaseWait")));
                Task.Run(() => Loopback.Start());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Complete(long tick)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                if (tick == -2)
                {
                    this.labelUpgrade.Content = I18N.GetString("NoSubscription");
                    Task.Run(() => SnakeBarMessage(I18N.GetString("NoSubscription")));
                }
                else if (tick == -1)
                {
                    this.labelUpgrade.Content = I18N.GetString("UpgradeNodeError");
                    Task.Run(() => SnakeBarMessage(I18N.GetString("UpgradeNodeError")));
                }
                else
                {
                    var dateTime = new DateTime(Configuration.Config.Upgrade);
                    this.labelUpgrade.Content = $"{I18N.GetString("Upgrade")}: {dateTime.ToString("yyyy.MM.dd HH:mm:ss")}";

                    if ((DateTime.Now - dateTime).TotalSeconds < 2)
                    {
                        Task.Run(() => SnakeBarMessage(I18N.GetString("UpgradeCompleted")));
                    }
                }
                this.listBoxNode.Items.Clear();
                var sses = Node.sses.Select(x => x.Name).OrderBy(y => y).ToList();
                var vmesses = Node.vmesses.Select(x => x.Name).OrderBy(y => y).ToList();
                foreach (var item in sses)
                {
                    this.listBoxNode.Items.Add(item);
                }
                foreach (var item in vmesses)
                {
                    this.listBoxNode.Items.Add(item);
                }
                if (this.listBoxNode.Items.Count != 0)
                {
                    this.listBoxNode.SelectedItem = V2Ray.SelectNode();
                    this.listBoxNode.ScrollIntoView(this.listBoxNode.SelectedItem);
                    this.listBoxNode.Focus();
                }
            }));
        }

        private void UpgradeStatus(bool isWait)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.buttonSwitch.IsEnabled = false;
                this.buttonRoute.IsEnabled = false;
                this.buttonNode.IsEnabled = false;
                this.buttonListen.IsEnabled = false;
                this.buttonConfig.IsEnabled = false;
                this.buttonLoopback.IsEnabled = false;
                this.labelStatus.Content = $"{I18N.GetString("Waiting")}";
                this.labelStatus.Foreground = this.labelUpgrade.Foreground;
            }));
            if (isWait)
            {
                Task.Delay(2000).Wait();
            }
            this.Dispatcher.Invoke(new Action(() =>
            {
                var isRunning = V2Ray.IsRunning();
                var isUsingRoute = V2Ray.IsUsingRoute();
                var isListenHostOnly = V2Ray.IsListenHostOnly();
                if (isRunning)
                {
                    this.buttonSwitch.Content = I18N.GetString("Stop");
                }
                else
                {
                    this.buttonSwitch.Content = I18N.GetString("Start");
                }
                if (isUsingRoute)
                {
                    this.buttonRoute.Content = I18N.GetString("Global");
                }
                else
                {
                    this.buttonRoute.Content = I18N.GetString("Route");
                }
                if (isListenHostOnly)
                {
                    this.buttonListen.Content = I18N.GetString("AllowAny");
                }
                else
                {
                    this.buttonListen.Content = I18N.GetString("HostOnly");
                }
                if (isRunning)
                {
                    if (isUsingRoute && isListenHostOnly)
                    {
                        this.labelStatus.Content = $"{I18N.GetString("RunningStatus")}: {I18N.GetString("Route")}, {I18N.GetString("HostOnly")}";
                        this.labelStatus.Foreground = Brushes.DarkCyan;
                    }
                    else if (isUsingRoute && !isListenHostOnly)
                    {
                        this.labelStatus.Content = $"{I18N.GetString("RunningStatus")}: {I18N.GetString("Route")}, {I18N.GetString("AllowAny")}";
                        this.labelStatus.Foreground = Brushes.DarkCyan;
                    }
                    else if (!isUsingRoute && isListenHostOnly)
                    {
                        this.labelStatus.Content = $"{I18N.GetString("RunningStatus")}: {I18N.GetString("Global")}, {I18N.GetString("HostOnly")}";
                        this.labelStatus.Foreground = Brushes.DeepPink;
                    }
                    else
                    {
                        this.labelStatus.Content = $"{I18N.GetString("RunningStatus")}: {I18N.GetString("Global")}, {I18N.GetString("AllowAny")}";
                        this.labelStatus.Foreground = Brushes.DeepPink;
                    }
                }
                else
                {
                    this.labelStatus.Content = $"{I18N.GetString("RunningStatus")}: {I18N.GetString("Stoped")}";
                    this.labelStatus.Foreground = this.labelUpgrade.Foreground;
                }
                this.buttonSwitch.IsEnabled = true;
                this.buttonRoute.IsEnabled = true;
                this.buttonNode.IsEnabled = true;
                this.buttonListen.IsEnabled = true;
                this.buttonConfig.IsEnabled = true;
                this.buttonLoopback.IsEnabled = true;
            }));
        }

        private void SnakeBarMessage(string message)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                var snackbarMessage = new SnackbarMessage();
                snackbarMessage.Content = message;
                snackbar.Message = snackbarMessage;
                snackbar.IsActive = true;
            }));
            Task.Delay(2000).Wait();
            this.Dispatcher.Invoke(new Action(() =>
            {
                var snackbarMessage = new SnackbarMessage();
                snackbarMessage.Content = string.Empty;
                snackbar.Message = snackbarMessage;
                snackbar.IsActive = false;
            }));
        }
    }
}
