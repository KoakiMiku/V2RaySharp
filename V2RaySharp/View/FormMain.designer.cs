namespace V2RaySharp.View
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonChange = new System.Windows.Forms.Button();
            this.listBoxNode = new System.Windows.Forms.ListBox();
            this.buttonSwitch = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.tableLayoutControl = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutButton = new System.Windows.Forms.TableLayoutPanel();
            this.labelUserInfo = new System.Windows.Forms.Label();
            this.tableLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutControl.SuspendLayout();
            this.tableLayoutButton.SuspendLayout();
            this.tableLayoutMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonChange
            // 
            this.buttonChange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonChange.Location = new System.Drawing.Point(3, 111);
            this.buttonChange.Name = "buttonChange";
            this.buttonChange.Size = new System.Drawing.Size(190, 102);
            this.buttonChange.TabIndex = 1;
            this.buttonChange.Text = "Change Node";
            this.buttonChange.UseVisualStyleBackColor = false;
            this.buttonChange.Click += new System.EventHandler(this.ButtonChange_Click);
            // 
            // listBoxNode
            // 
            this.listBoxNode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxNode.FormattingEnabled = true;
            this.listBoxNode.ItemHeight = 20;
            this.listBoxNode.Location = new System.Drawing.Point(3, 3);
            this.listBoxNode.Name = "listBoxNode";
            this.listBoxNode.Size = new System.Drawing.Size(368, 325);
            this.listBoxNode.Sorted = true;
            this.listBoxNode.TabIndex = 0;
            // 
            // buttonSwitch
            // 
            this.buttonSwitch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSwitch.Location = new System.Drawing.Point(3, 3);
            this.buttonSwitch.Name = "buttonSwitch";
            this.buttonSwitch.Size = new System.Drawing.Size(190, 102);
            this.buttonSwitch.TabIndex = 0;
            this.buttonSwitch.Text = "Switch";
            this.buttonSwitch.UseVisualStyleBackColor = false;
            this.buttonSwitch.Click += new System.EventHandler(this.ButtonSwitch_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEdit.Location = new System.Drawing.Point(3, 219);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(190, 103);
            this.buttonEdit.TabIndex = 2;
            this.buttonEdit.Text = "Edit Config";
            this.buttonEdit.UseVisualStyleBackColor = false;
            this.buttonEdit.Click += new System.EventHandler(this.ButtonEdit_Click);
            // 
            // tableLayoutControl
            // 
            this.tableLayoutControl.ColumnCount = 2;
            this.tableLayoutControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutControl.Controls.Add(this.tableLayoutButton, 1, 0);
            this.tableLayoutControl.Controls.Add(this.listBoxNode, 0, 0);
            this.tableLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutControl.Location = new System.Drawing.Point(3, 23);
            this.tableLayoutControl.Name = "tableLayoutControl";
            this.tableLayoutControl.RowCount = 1;
            this.tableLayoutControl.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutControl.Size = new System.Drawing.Size(576, 331);
            this.tableLayoutControl.TabIndex = 0;
            // 
            // tableLayoutButton
            // 
            this.tableLayoutButton.ColumnCount = 1;
            this.tableLayoutButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutButton.Controls.Add(this.buttonChange, 0, 1);
            this.tableLayoutButton.Controls.Add(this.buttonEdit, 0, 2);
            this.tableLayoutButton.Controls.Add(this.buttonSwitch, 0, 0);
            this.tableLayoutButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutButton.Location = new System.Drawing.Point(377, 3);
            this.tableLayoutButton.Name = "tableLayoutButton";
            this.tableLayoutButton.RowCount = 3;
            this.tableLayoutButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutButton.Size = new System.Drawing.Size(196, 325);
            this.tableLayoutButton.TabIndex = 0;
            // 
            // labelUserInfo
            // 
            this.labelUserInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUserInfo.Location = new System.Drawing.Point(3, 0);
            this.labelUserInfo.Name = "labelUserInfo";
            this.labelUserInfo.Size = new System.Drawing.Size(576, 20);
            this.labelUserInfo.TabIndex = 1;
            this.labelUserInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutMain
            // 
            this.tableLayoutMain.ColumnCount = 1;
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutMain.Controls.Add(this.labelUserInfo, 0, 0);
            this.tableLayoutMain.Controls.Add(this.tableLayoutControl, 0, 1);
            this.tableLayoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutMain.Name = "tableLayoutMain";
            this.tableLayoutMain.RowCount = 2;
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutMain.Size = new System.Drawing.Size(582, 357);
            this.tableLayoutMain.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(582, 357);
            this.Controls.Add(this.tableLayoutMain);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "V2Ray Sharp";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown);
            this.tableLayoutControl.ResumeLayout(false);
            this.tableLayoutButton.ResumeLayout(false);
            this.tableLayoutMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonChange;
        private System.Windows.Forms.ListBox listBoxNode;
        private System.Windows.Forms.Button buttonSwitch;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.TableLayoutPanel tableLayoutControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutButton;
        private System.Windows.Forms.Label labelUserInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutMain;
    }
}

