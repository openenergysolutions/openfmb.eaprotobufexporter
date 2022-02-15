namespace EAProtobufExporter
{
    partial class OpenFMBMessageProfileSelector
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
            this.treeViewMessageProfile = new System.Windows.Forms.TreeView();
            this.textBoxProtobufOutput = new System.Windows.Forms.TextBox();
            this.buttonGenerateProtobuf = new System.Windows.Forms.Button();
            this.buttonSaveProtobuf = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewMessageProfile
            // 
            this.treeViewMessageProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewMessageProfile.CheckBoxes = true;
            this.treeViewMessageProfile.Location = new System.Drawing.Point(3, 3);
            this.treeViewMessageProfile.Name = "treeViewMessageProfile";
            this.treeViewMessageProfile.Size = new System.Drawing.Size(239, 380);
            this.treeViewMessageProfile.TabIndex = 0;
            this.treeViewMessageProfile.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewModule_AfterCheck);
            this.treeViewMessageProfile.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewModule_AfterSelect);
            // 
            // textBoxProtobufOutput
            // 
            this.textBoxProtobufOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxProtobufOutput.Location = new System.Drawing.Point(248, 3);
            this.textBoxProtobufOutput.Multiline = true;
            this.textBoxProtobufOutput.Name = "textBoxProtobufOutput";
            this.textBoxProtobufOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxProtobufOutput.Size = new System.Drawing.Size(244, 380);
            this.textBoxProtobufOutput.TabIndex = 0;
            this.textBoxProtobufOutput.TextChanged += new System.EventHandler(this.textBoxProtobufOutput_TextChanged);
            // 
            // buttonGenerateProtobuf
            // 
            this.buttonGenerateProtobuf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.buttonGenerateProtobuf.Location = new System.Drawing.Point(67, 403);
            this.buttonGenerateProtobuf.Name = "buttonGenerateProtobuf";
            this.buttonGenerateProtobuf.Size = new System.Drawing.Size(110, 26);
            this.buttonGenerateProtobuf.TabIndex = 1;
            this.buttonGenerateProtobuf.Text = "Export Protobuf";
            this.buttonGenerateProtobuf.UseVisualStyleBackColor = true;
            this.buttonGenerateProtobuf.Click += new System.EventHandler(this.buttonGenerateProtobuf_Click);
            // 
            // buttonSaveProtobuf
            // 
            this.buttonSaveProtobuf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.buttonSaveProtobuf.Enabled = false;
            this.buttonSaveProtobuf.Location = new System.Drawing.Point(315, 403);
            this.buttonSaveProtobuf.Name = "buttonSaveProtobuf";
            this.buttonSaveProtobuf.Size = new System.Drawing.Size(110, 26);
            this.buttonSaveProtobuf.TabIndex = 2;
            this.buttonSaveProtobuf.Text = "Save Protobuf";
            this.buttonSaveProtobuf.UseVisualStyleBackColor = true;
            this.buttonSaveProtobuf.Click += new System.EventHandler(this.buttonSaveProtobuf_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.501F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.499F));
            this.tableLayoutPanel1.Controls.Add(this.treeViewMessageProfile, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxProtobufOutput, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonSaveProtobuf, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonGenerateProtobuf, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 96.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(495, 473);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // OpenFMBMessageProfileSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 473);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "OpenFMBMessageProfileSelector";
            this.Text = "OpenFMBMessageProfileSelector";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewMessageProfile;
        private System.Windows.Forms.Button buttonGenerateProtobuf;
        private System.Windows.Forms.Button buttonSaveProtobuf;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.TextBox textBoxProtobufOutput;
    }
}