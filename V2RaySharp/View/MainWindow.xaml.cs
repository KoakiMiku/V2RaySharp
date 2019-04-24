﻿using System;
using System.Linq;
using System.Threading;
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
                this.Focus();
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
                    string name = this.listBoxNode.SelectedItem.ToString();
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
                    string name = this.listBoxNode.SelectedItem.ToString();
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

        private void ButtonConfig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
                }
                else if (tick == -1)
                {
                    this.labelUpgrade.Content = I18N.GetString("UpgradeNodeError");
                }
                else
                {
                    var dateTime = new DateTime(Configuration.Config.Upgrade);
                    this.labelUpgrade.Content = $"{I18N.GetString("Upgrade")}: {dateTime.ToString("yyyy.MM.dd HH:mm:ss")}";
                }
                this.listBoxNode.Items.Clear();
                var sses = Node.sses.Select(x => x.Name).OrderBy(y => y).ToList();
                var vmesses = Node.vmesses.Select(x => x.Name).OrderBy(y => y).ToList();
                foreach (string item in sses)
                {
                    this.listBoxNode.Items.Add(item);
                }
                foreach (string item in vmesses)
                {
                    this.listBoxNode.Items.Add(item);
                }
                if (this.listBoxNode.Items.Count != 0)
                {
                    this.listBoxNode.SelectedItem = V2Ray.SelectNode();
                }
            }));
        }

        private void UpgradeStatus(bool isWait)
        {
            try
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.buttonSwitch.IsEnabled = false;
                    this.buttonRoute.IsEnabled = false;
                    this.buttonNode.IsEnabled = false;
                    this.labelStatus.Content = $"{I18N.GetString("Waiting")}";
                    this.labelStatus.Foreground = Brushes.Black;
                }));
                if (isWait)
                {
                    Thread.Sleep(2000);
                }
                this.Dispatcher.Invoke(new Action(() =>
                {
                    bool isRunning = V2Ray.IsRunning();
                    bool isUsingRoute = V2Ray.IsUsingRoute();
                    if (isRunning && isUsingRoute)
                    {
                        this.buttonSwitch.Content = I18N.GetString("Stop");
                        this.buttonSwitch.Foreground = Brushes.Red;
                        this.buttonRoute.Content = I18N.GetString("Global");
                        this.buttonRoute.Foreground = Brushes.Blue;
                        this.labelStatus.Content = $"{I18N.GetString("RunningStatus")}: {I18N.GetString("Route")}";
                        this.labelStatus.Foreground = Brushes.Green;
                    }
                    else if (isRunning && !isUsingRoute)
                    {
                        this.buttonSwitch.Content = I18N.GetString("Stop");
                        this.buttonSwitch.Foreground = Brushes.Red;
                        this.buttonRoute.Content = I18N.GetString("Route");
                        this.buttonRoute.Foreground = Brushes.Green;
                        this.labelStatus.Content = $"{I18N.GetString("RunningStatus")}: {I18N.GetString("Global")}";
                        this.labelStatus.Foreground = Brushes.Blue;
                    }
                    else if (!isRunning && isUsingRoute)
                    {
                        this.buttonSwitch.Content = I18N.GetString("Start");
                        this.buttonSwitch.Foreground = Brushes.Green;
                        this.buttonRoute.Content = I18N.GetString("Global");
                        this.buttonRoute.Foreground = Brushes.Blue;
                        this.labelStatus.Content = $"{I18N.GetString("RunningStatus")}: {I18N.GetString("Stoped")}";
                        this.labelStatus.Foreground = Brushes.Red;
                    }
                    else
                    {
                        this.buttonSwitch.Content = I18N.GetString("Start");
                        this.buttonSwitch.Foreground = Brushes.Green;
                        this.buttonRoute.Content = I18N.GetString("Route");
                        this.buttonRoute.Foreground = Brushes.Green;
                        this.labelStatus.Content = $"{I18N.GetString("RunningStatus")}: {I18N.GetString("Stoped")}";
                        this.labelStatus.Foreground = Brushes.Red;
                    }
                    this.buttonSwitch.IsEnabled = true;
                    this.buttonRoute.IsEnabled = true;
                    this.buttonNode.IsEnabled = true;
                }));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
