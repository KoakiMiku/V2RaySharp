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
        private static readonly string name = "V2Ray Sharp";

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                Icon = Resources.V2Ray;
                buttonSwitch.Text = I18N.GetString("Status");
                buttonRoute.Text = I18N.GetString("Status");
                buttonNode.Text = I18N.GetString("ChangeNode");
                buttonEdit.Text = I18N.GetString("EditConfig");
                Node.CompleteEvent += Complete;
                Configuration.Load();
                Node.Upgrade();
                UpgradeStatus(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name,
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
                MessageBox.Show(ex.Message, name,
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
                MessageBox.Show(ex.Message, name,
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
                MessageBox.Show(ex.Message, name,
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonNode_Click(object sender, EventArgs e)
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
                    throw new Exception(I18N.GetString("NodeNotSelect"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name,
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
                    throw new Exception(I18N.GetString("NodeNotSelect"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, name,
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
                MessageBox.Show(ex.Message, name,
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
                        labelUserInfo.Text = I18N.GetString("NoSubscription");
                    }
                    else if (tick == -1)
                    {
                        labelUserInfo.Text = I18N.GetString("UpgradeNodeError");
                    }
                    else
                    {
                        labelUserInfo.Text = Node.userInfo;
                    }
                    if (Configuration.Config.Upgrade != 0)
                    {
                        DateTime dateTime = new DateTime(Configuration.Config.Upgrade);
                        labelUpgrade.Text = $"{I18N.GetString("Upgrade")}:{dateTime.ToString("yyyy.MM.dd HH:mm:ss")}";
                    }
                    else
                    {
                        labelUpgrade.Text = $"{I18N.GetString("Upgrade")}:{I18N.GetString("None")}";
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
                    buttonNode.Enabled = false;
                    labelStatus.Text = $"{I18N.GetString("Waiting")}";
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
                        buttonSwitch.Text = I18N.GetString("Stop");
                        buttonSwitch.ForeColor = Color.Red;
                        buttonRoute.Text = I18N.GetString("Global");
                        buttonRoute.ForeColor = Color.Red;
                        labelStatus.Text = $"{I18N.GetString("RunningStatus")}:{I18N.GetString("Route")}";
                        labelStatus.ForeColor = Color.Green;
                    }
                    else if (isRunning && !isUsingRoute)
                    {
                        buttonSwitch.Text = I18N.GetString("Stop");
                        buttonSwitch.ForeColor = Color.Red;
                        buttonRoute.Text = I18N.GetString("Route");
                        buttonRoute.ForeColor = Color.Blue;
                        labelStatus.Text = $"{I18N.GetString("RunningStatus")}:{I18N.GetString("Global")}";
                        labelStatus.ForeColor = Color.Blue;
                    }
                    else if (!isRunning && isUsingRoute)
                    {
                        buttonSwitch.Text = I18N.GetString("Start");
                        buttonSwitch.ForeColor = Color.Green;
                        buttonRoute.Text = I18N.GetString("Global");
                        buttonRoute.ForeColor = Color.Red;
                        labelStatus.Text = $"{I18N.GetString("RunningStatus")}:{I18N.GetString("Stoped")}";
                        labelStatus.ForeColor = Color.Red;
                    }
                    else
                    {
                        buttonSwitch.Text = I18N.GetString("Start");
                        buttonSwitch.ForeColor = Color.Green;
                        buttonRoute.Text = I18N.GetString("Route");
                        buttonRoute.ForeColor = Color.Blue;
                        labelStatus.Text = $"{I18N.GetString("RunningStatus")}:{I18N.GetString("Stoped")}";
                        labelStatus.ForeColor = Color.Red;
                    }
                    buttonSwitch.Enabled = true;
                    buttonRoute.Enabled = true;
                    buttonNode.Enabled = true;
                }));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
