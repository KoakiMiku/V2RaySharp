using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
                buttonSwitch.Text = Language.GetString("Switch");
                buttonChange.Text = Language.GetString("ChangeNode");
                buttonEdit.Text = Language.GetString("EditConfig");
                Node.CompleteEvent += Complete;
                Configuration.Load();
                Node.Upgrade();
                Task.Run(() => UpgradeButton());
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
                Task.Run(() => UpgradeButton());
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
                    if (listBoxNode.SelectedItem.ToString() != V2Ray.Select())
                    {
                        string name = listBoxNode.SelectedItem.ToString();
                        Task.Run(() => V2Ray.Change(name));
                        Task.Run(() => UpgradeButton());
                    }
                    else
                    {
                        throw new Exception(Language.GetString("NodeNotChange"));
                    }
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
                    labelUserInfo.Text = Node.userInfo;
                    listBoxNode.Items.Clear();
                    listBoxNode.Items.AddRange(Node.sses.Select(x => x.Name).ToArray());
                    listBoxNode.Items.AddRange(Node.vmesses.Select(x => x.Name).ToArray());
                    if (tick == 0 && Configuration.Config.Upgrade == 0)
                    {
                        Text = $"V2Ray Sharp";
                    }
                    else if (tick == 0 && Configuration.Config.Upgrade != 0)
                    {
                        DateTime dateTime = new DateTime(Configuration.Config.Upgrade);
                        Text = $"V2Ray Sharp - {dateTime.ToString("yyyy.MM.dd - HH:mm:ss")}";
                    }
                    else
                    {
                        DateTime dateTime = new DateTime(tick);
                        Text = $"V2Ray Sharp - {dateTime.ToString("yyyy.MM.dd - HH:mm:ss")}";
                    }
                    if (listBoxNode.Items.Count != 0)
                    {
                        listBoxNode.SelectedItem = V2Ray.Select();
                    }
                    else
                    {
                        buttonChange.Enabled = false;
                    }
                }));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UpgradeButton()
        {
            try
            {
                Invoke(new Action(() =>
                {
                    buttonSwitch.Enabled = false;
                    buttonChange.Enabled = false;
                }));
                Thread.Sleep(2000);
                Invoke(new Action(() =>
                {
                    if (V2Ray.IsRunning())
                    {
                        buttonSwitch.Text = Language.GetString("Stop");
                        buttonSwitch.ForeColor = Color.Red;
                    }
                    else
                    {
                        buttonSwitch.Text = Language.GetString("Start");
                        buttonSwitch.ForeColor = Color.Green;
                    }
                    buttonSwitch.Enabled = true;
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
