using BrightIdeasSoftware;
using MetroFramework.Controls;

namespace QuickWin
{
    partial class WinForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinForm));
            this.lstView = new BrightIdeasSoftware.FastObjectListView();
            this.picBox = new System.Windows.Forms.PictureBox();
            this.lstLog = new BrightIdeasSoftware.FastObjectListView();
            this.chkAllProcs = new System.Windows.Forms.CheckBox();
            this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lblFilter = new System.Windows.Forms.ToolStripLabel();
            this.txtFilter = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.txtNum = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.splitLeft = new System.Windows.Forms.SplitContainer();
            this.splitRight = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.lstView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lstLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitLeft)).BeginInit();
            this.splitLeft.Panel1.SuspendLayout();
            this.splitLeft.Panel2.SuspendLayout();
            this.splitLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitRight)).BeginInit();
            this.splitRight.Panel1.SuspendLayout();
            this.splitRight.Panel2.SuspendLayout();
            this.splitRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstView
            // 
            this.lstView.BackColor = System.Drawing.SystemColors.Window;
            this.lstView.Cursor = System.Windows.Forms.Cursors.Default;
            this.lstView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstView.ForeColor = System.Drawing.Color.Black;
            this.lstView.FullRowSelect = true;
            this.lstView.HideSelection = false;
            this.lstView.Location = new System.Drawing.Point(0, 0);
            this.lstView.Name = "lstView";
            this.lstView.ShowGroups = false;
            this.lstView.ShowItemToolTips = true;
            this.lstView.Size = new System.Drawing.Size(491, 500);
            this.lstView.TabIndex = 0;
            this.lstView.UseCompatibleStateImageBehavior = false;
            this.lstView.UseExplorerTheme = true;
            this.lstView.UseFiltering = true;
            this.lstView.UseHotItem = true;
            this.lstView.View = System.Windows.Forms.View.Details;
            this.lstView.VirtualMode = true;
            this.lstView.SelectedIndexChanged += new System.EventHandler(this.lstView_SelectedIndexChanged);
            this.lstView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstView_KeyDown);
            this.lstView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstView_MouseDoubleClick);
            this.lstView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstView_MouseUp);
            // 
            // picBox
            // 
            this.picBox.BackColor = System.Drawing.SystemColors.Window;
            this.picBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBox.Location = new System.Drawing.Point(0, 0);
            this.picBox.Margin = new System.Windows.Forms.Padding(4);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(408, 321);
            this.picBox.TabIndex = 6;
            this.picBox.TabStop = false;
            this.picBox.SizeChanged += new System.EventHandler(this.picBox_SizeChanged);
            // 
            // lstLog
            // 
            this.lstLog.BackColor = System.Drawing.SystemColors.Window;
            this.lstLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstLog.Cursor = System.Windows.Forms.Cursors.Default;
            this.lstLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLog.ForeColor = System.Drawing.Color.Black;
            this.lstLog.FullRowSelect = true;
            this.lstLog.HideSelection = false;
            this.lstLog.Location = new System.Drawing.Point(0, 0);
            this.lstLog.Name = "lstLog";
            this.lstLog.ShowGroups = false;
            this.lstLog.ShowItemToolTips = true;
            this.lstLog.Size = new System.Drawing.Size(408, 175);
            this.lstLog.TabIndex = 11;
            this.lstLog.UseCompatibleStateImageBehavior = false;
            this.lstLog.UseExplorerTheme = true;
            this.lstLog.UseFiltering = true;
            this.lstLog.UseHotItem = true;
            this.lstLog.View = System.Windows.Forms.View.Details;
            this.lstLog.VirtualMode = true;
            // 
            // chkAllProcs
            // 
            this.chkAllProcs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAllProcs.AutoSize = true;
            this.chkAllProcs.Location = new System.Drawing.Point(688, 7);
            this.chkAllProcs.Name = "chkAllProcs";
            this.chkAllProcs.Size = new System.Drawing.Size(170, 21);
            this.chkAllProcs.TabIndex = 13;
            this.chkAllProcs.Text = "&All Processes (Alt + A)";
            this.chkAllProcs.CheckedChanged += new System.EventHandler(this.chkAllProcs_CheckedChanged);
            // 
            // metroStyleManager
            // 
            this.metroStyleManager.Owner = this;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblFilter,
            this.txtFilter,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(903, 31);
            this.toolStrip1.TabIndex = 15;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lblFilter
            // 
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(74, 28);
            this.lblFilter.Text = "Filter (F2):";
            // 
            // txtFilter
            // 
            this.txtFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(200, 31);
            this.txtFilter.TextChanged += new System.EventHandler(this.textBoxFilterSimple_TextChanged);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(111, 28);
            this.toolStripButton1.Text = "Refresh (F5)";
            this.toolStripButton1.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtNum,
            this.toolStripLabel1});
            this.toolStrip2.Location = new System.Drawing.Point(0, 531);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(903, 31);
            this.toolStrip2.TabIndex = 16;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // txtNum
            // 
            this.txtNum.Name = "txtNum";
            this.txtNum.Size = new System.Drawing.Size(60, 28);
            this.txtNum.Text = "0 Items.";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(179, 28);
            this.toolStripLabel1.Text = "To Show: Ctrl + Shift + W";
            // 
            // splitLeft
            // 
            this.splitLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitLeft.Location = new System.Drawing.Point(0, 31);
            this.splitLeft.Name = "splitLeft";
            // 
            // splitLeft.Panel1
            // 
            this.splitLeft.Panel1.Controls.Add(this.lstView);
            // 
            // splitLeft.Panel2
            // 
            this.splitLeft.Panel2.Controls.Add(this.splitRight);
            this.splitLeft.Size = new System.Drawing.Size(903, 500);
            this.splitLeft.SplitterDistance = 491;
            this.splitLeft.TabIndex = 17;
            // 
            // splitRight
            // 
            this.splitRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitRight.Location = new System.Drawing.Point(0, 0);
            this.splitRight.Name = "splitRight";
            this.splitRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitRight.Panel1
            // 
            this.splitRight.Panel1.Controls.Add(this.picBox);
            // 
            // splitRight.Panel2
            // 
            this.splitRight.Panel2.Controls.Add(this.lstLog);
            this.splitRight.Size = new System.Drawing.Size(408, 500);
            this.splitRight.SplitterDistance = 321;
            this.splitRight.TabIndex = 18;
            // 
            // WinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 562);
            this.Controls.Add(this.splitLeft);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.chkAllProcs);
            this.Controls.Add(this.toolStrip1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "WinForm";
            this.Text = "Quick Win";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WinForm_FormClosing);
            this.Load += new System.EventHandler(this.WinForm_Load);
            this.SizeChanged += new System.EventHandler(this.WinForm_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WinForm_KeyDown);
            this.Resize += new System.EventHandler(this.WinForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.lstView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lstLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.splitLeft.Panel1.ResumeLayout(false);
            this.splitLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitLeft)).EndInit();
            this.splitLeft.ResumeLayout(false);
            this.splitRight.Panel1.ResumeLayout(false);
            this.splitRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitRight)).EndInit();
            this.splitRight.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FastObjectListView lstView;
        private System.Windows.Forms.PictureBox picBox;
        private FastObjectListView lstLog;
        private System.Windows.Forms.CheckBox chkAllProcs;
        private MetroFramework.Components.MetroStyleManager metroStyleManager;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel lblFilter;
        private System.Windows.Forms.ToolStripTextBox txtFilter;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel txtNum;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.SplitContainer splitLeft;
        private System.Windows.Forms.SplitContainer splitRight;
    }
}

