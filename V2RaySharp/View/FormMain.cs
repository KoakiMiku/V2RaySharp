using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using V2RaySharp.Controller;
using V2RaySharp.Properties;

namespace V2RaySharp.View
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                Icon = Resources.V2Ray;
                buttonSwitch.Text = Language.GetString("Status");
                buttonRoute.Text = Language.GetString("Status");
                buttonChange.Text = Language.GetString("ChangeNode");
                buttonEdit.Text = Language.GetString("EditConfig");
                Node.CompleteEvent += Complete;
                Configuration.Load();
                Node.Upgrade();
                UpgradeStatus(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName,
                  MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            try
            {
                Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName,
                  MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName,
                  MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonSwitch_Click(object sender, EventArgs e)
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
                MessageBox.Show(ex.Message, Application.ProductName,
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonChange_Click(object sender, EventArgs e)
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
                    throw new Exception(Language.GetString("NodeNotSelect"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName,
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonRoute_Click(object sender, EventArgs e)
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
                    throw new Exception(Language.GetString("NodeNotSelect"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName,
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                Task.Run(() => V2Ray.EditConfig());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName,
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Complete(long tick)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    if (tick == -2)
                    {
                        labelUserInfo.Text = Language.GetString("NoSubscription");
                    }
                    else if (tick == -1)
                    {
                        labelUserInfo.Text = Language.GetString("UpgradeNodeError");
                    }
                    else
                    {
                        labelUserInfo.Text = Node.userInfo;
                    }
                    if (Configuration.Config.Upgrade != 0)
                    {
                        DateTime dateTime = new DateTime(Configuration.Config.Upgrade);
                        labelUpgrade.Text = $"{Language.GetString("Upgrade")}:{dateTime.ToString("yyyy.MM.dd HH:mm:ss")}";
                    }
                    else
                    {
                        labelUpgrade.Text = $"{Language.GetString("Upgrade")}:{Language.GetString("None")}";
                    }
                    listBoxNode.Items.Clear();
                    listBoxNode.Items.AddRange(Node.sses.Select(x => x.Name).ToArray());
                    listBoxNode.Items.AddRange(Node.vmesses.Select(x => x.Name).ToArray());
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
                Invoke(new Action(() =>
                {
                    buttonSwitch.Enabled = false;
                    buttonRoute.Enabled = false;
                    buttonChange.Enabled = false;
                    labelStatus.Text = $"{Language.GetString("Waiting")}";
                    labelStatus.ForeColor = Color.Black;
                }));
                if (isWait)
                {
                    Thread.Sleep(2000);
                }
                Invoke(new Action(() =>
                {
                    bool isRunning = V2Ray.IsRunning();
                    bool isUsingRoute = V2Ray.IsUsingRoute();
                    if (isRunning && isUsingRoute)
                    {
                        buttonSwitch.Text = Language.GetString("Stop");
                        buttonSwitch.ForeColor = Color.Red;
                        buttonRoute.Text = Language.GetString("Global");
                        buttonRoute.ForeColor = Color.Red;
                        labelStatus.Text = $"{Language.GetString("RunningStatus")}:{Language.GetString("Route")}";
                        labelStatus.ForeColor = Color.Green;
                    }
                    else if (isRunning && !isUsingRoute)
                    {
                        buttonSwitch.Text = Language.GetString("Stop");
                        buttonSwitch.ForeColor = Color.Red;
                        buttonRoute.Text = Language.GetString("Route");
                        buttonRoute.ForeColor = Color.Blue;
                        labelStatus.Text = $"{Language.GetString("RunningStatus")}:{Language.GetString("Global")}";
                        labelStatus.ForeColor = Color.Blue;
                    }
                    else if (!isRunning && isUsingRoute)
                    {
                        buttonSwitch.Text = Language.GetString("Start");
                        buttonSwitch.ForeColor = Color.Green;
                        buttonRoute.Text = Language.GetString("Global");
                        buttonRoute.ForeColor = Color.Red;
                        labelStatus.Text = $"{Language.GetString("RunningStatus")}:{Language.GetString("Stoped")}";
                        labelStatus.ForeColor = Color.Red;
                    }
                    else
                    {
                        buttonSwitch.Text = Language.GetString("Start");
                        buttonSwitch.ForeColor = Color.Green;
                        buttonRoute.Text = Language.GetString("Route");
                        buttonRoute.ForeColor = Color.Blue;
                        labelStatus.Text = $"{Language.GetString("RunningStatus")}:{Language.GetString("Stoped")}";
                        labelStatus.ForeColor = Color.Red;
                    }
                    buttonSwitch.Enabled = true;
                    buttonRoute.Enabled = true;
                    buttonChange.Enabled = true;
                }));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
