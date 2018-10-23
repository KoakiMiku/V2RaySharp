using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using V2RaySharpWPF.Controller;
using V2RaySharpWPF.Model;

namespace V2RaySharpWPF.View
{
    public partial class MainWindow : Window
    {
        private static readonly string name = "V2Ray Sharp";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                buttonSwitch.Content = Controller.Language.GetString("Status");
                buttonRoute.Content = Controller.Language.GetString("Status");
                buttonNode.Content = Controller.Language.GetString("ChangeNode");
                buttonEdit.Content = Controller.Language.GetString("EditConfig");
                Node.CompleteEvent += Complete;
                Configuration.Load();
                Node.Upgrade();
                UpgradeStatus(false);
                Focus();
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
                    string name = listBoxNode.SelectedItem.ToString();
                    Task.Run(() => V2Ray.ChangeNode(name));
                    Task.Run(() => UpgradeStatus(true));
                }
                else
                {
                    throw new Exception(Controller.Language.GetString("NodeNotSelect"));
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
                    string name = listBoxNode.SelectedItem.ToString();
                    Task.Run(() => V2Ray.ChangeRoute(name));
                    Task.Run(() => UpgradeStatus(true));
                }
                else
                {
                    throw new Exception(Controller.Language.GetString("NodeNotSelect"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name,
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Task.Run(() => V2Ray.EditConfig());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name,
                 MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Complete(long tick)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    if (tick == -2)
                    {
                        labelUserInfo.Content = Controller.Language.GetString("NoSubscription");
                    }
                    else if (tick == -1)
                    {
                        labelUserInfo.Content = Controller.Language.GetString("UpgradeNodeError");
                    }
                    else
                    {
                        labelUserInfo.Content = Node.userInfo;
                    }
                    if (Configuration.Config.Upgrade != 0)
                    {
                        DateTime dateTime = new DateTime(Configuration.Config.Upgrade);
                        labelUpgrade.Content = $"{Controller.Language.GetString("Upgrade")}:{dateTime.ToString("yyyy.MM.dd HH:mm:ss")}";
                    }
                    else
                    {
                        labelUpgrade.Content = $"{Controller.Language.GetString("Upgrade")}:{Controller.Language.GetString("None")}";
                    }
                    listBoxNode.Items.Clear();
                    List<string> sses = Node.sses.Select(x => x.Name).OrderBy(y => y).ToList();
                    List<string> vmesses = Node.vmesses.Select(x => x.Name).OrderBy(y => y).ToList();
                    foreach (var item in sses)
                    {
                        listBoxNode.Items.Add(item);
                    }
                    foreach (var item in vmesses)
                    {
                        listBoxNode.Items.Add(item);
                    }
                    if (listBoxNode.Items.Count != 0)
                    {
                        listBoxNode.SelectedItem = V2Ray.SelectNode();
                    }
                }));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UpgradeStatus(bool isWait)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    buttonSwitch.IsEnabled = false;
                    buttonRoute.IsEnabled = false;
                    buttonNode.IsEnabled = false;
                    labelStatus.Content = $"{Controller.Language.GetString("Waiting")}";
                    labelStatus.Foreground = Brushes.Black;
                }));
                if (isWait)
                {
                    Thread.Sleep(2000);
                }
                Dispatcher.Invoke(new Action(() =>
                {
                    bool isRunning = V2Ray.IsRunning();
                    bool isUsingRoute = V2Ray.IsUsingRoute();
                    if (isRunning && isUsingRoute)
                    {
                        buttonSwitch.Content = Controller.Language.GetString("Stop");
                        buttonSwitch.Foreground = Brushes.Red;
                        buttonRoute.Content = Controller.Language.GetString("Global");
                        buttonRoute.Foreground = Brushes.Red;
                        labelStatus.Content = $"{Controller.Language.GetString("RunningStatus")}:{Controller.Language.GetString("Route")}";
                        labelStatus.Foreground = Brushes.Green;
                    }
                    else if (isRunning && !isUsingRoute)
                    {
                        buttonSwitch.Content = Controller.Language.GetString("Stop");
                        buttonSwitch.Foreground = Brushes.Red;
                        buttonRoute.Content = Controller.Language.GetString("Route");
                        buttonRoute.Foreground = Brushes.Blue;
                        labelStatus.Content = $"{Controller.Language.GetString("RunningStatus")}:{Controller.Language.GetString("Global")}";
                        labelStatus.Foreground = Brushes.Blue;
                    }
                    else if (!isRunning && isUsingRoute)
                    {
                        buttonSwitch.Content = Controller.Language.GetString("Start");
                        buttonSwitch.Foreground = Brushes.Green;
                        buttonRoute.Content = Controller.Language.GetString("Global");
                        buttonRoute.Foreground = Brushes.Red;
                        labelStatus.Content = $"{Controller.Language.GetString("RunningStatus")}:{Controller.Language.GetString("Stoped")}";
                        labelStatus.Foreground = Brushes.Red;
                    }
                    else
                    {
                        buttonSwitch.Content = Controller.Language.GetString("Start");
                        buttonSwitch.Foreground = Brushes.Green;
                        buttonRoute.Content = Controller.Language.GetString("Route");
                        buttonRoute.Foreground = Brushes.Blue;
                        labelStatus.Content = $"{Controller.Language.GetString("RunningStatus")}:{Controller.Language.GetString("Stoped")}";
                        labelStatus.Foreground = Brushes.Red;
                    }
                    buttonSwitch.IsEnabled = true;
                    buttonRoute.IsEnabled = true;
                    buttonNode.IsEnabled = true;
                }));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
