// SPDX-FileCopyrightText: 2022 Open Energy Solutions Inc
//
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EAProtobufExporter
{
    public partial class OpenFMBMessageProfileSelector : Form
    {
        private const int TVIF_STATE = 0x8;
        private const int TVIS_STATEIMAGEMASK = 0xF000;
        private const int TV_FIRST = 0x1100;
        private const int TVM_SETITEM = TV_FIRST + 63;

        public OpenFMBMessageProfileSelector()
        {
            InitializeComponent();

        } // end of public OpenFMBMessageProfileSelector()

        [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
        private struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;

        } // end of private struct TVITEM

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref TVITEM lParam);

        /// <summary>
        /// Hides the checkbox for the specified node on a TreeView control.
        /// </summary>
        public void hideCheckBox(TreeNode treeNoode)
        {
            TVITEM tvi = new TVITEM();
            tvi.hItem = treeNoode.Handle;
            tvi.mask = TVIF_STATE;
            tvi.stateMask = TVIS_STATEIMAGEMASK;
            tvi.state = 0;
            SendMessage(treeNoode.TreeView.Handle, TVM_SETITEM, IntPtr.Zero, ref tvi);

        } // end of public void hideCheckBox(TreeNode treeNoode)

        // Updates all child tree nodes recursively.
        public void setCheckBoxOfAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                if (!node.ForeColor.Equals(Color.Gray))
                {
                    node.Checked = nodeChecked;
                } // end if

                if (Global.hideCheckBoxList.Contains(node.Text))
                {
                    hideCheckBox(node);
                }
                else if (nodeChecked)
                {
                    Global.checkedElements.Add(node.Text);
                } // end else

                if (node.Nodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                    this.setCheckBoxOfAllChildNodes(node, nodeChecked);
                } // end if

            } // end foreach

        } // end of private void setCheckBoxOfAllChildNodes(TreeNode treeNode, bool nodeChecked)

        private void setCheckBoxOfAllParentNodes(TreeNode treeNode, bool nodeChecked)
        {
            if (nodeChecked)
            {
                TreeNode parentNode = treeNode.Parent;
                if (parentNode != null)
                {
                    parentNode.Checked = nodeChecked;
                    setCheckBoxOfAllParentNodes(parentNode, nodeChecked);
                } // end if

            } // end if

        } // end of private void setCheckBoxOfAllParentNodes(TreeNode treeNode, bool nodeChecked)

        // NOTE   This code can be added to the BeforeCheck event handler instead of the AfterCheck event.
        // After a tree node's Checked property is changed, all its child nodes are updated to the same value.
        private void node_AfterCheck(object sender, TreeViewEventArgs eventArgs)
        {
            this.buttonGenerateProtobuf.Enabled = true;
            // The code only executes if the user caused the checked state to change.
            if (eventArgs.Action != TreeViewAction.Unknown)
            {
                if (eventArgs.Node.Nodes.Count > 0)
                {
                    /* Calls the CheckAllChildNodes method, passing in the current 
                    Checked value of the TreeNode whose checked state changed. */
                    Global.checkedElements.Clear();
                    this.setCheckBoxOfAllChildNodes(eventArgs.Node, eventArgs.Node.Checked);
                    this.setCheckBoxOfAllParentNodes(eventArgs.Node, eventArgs.Node.Checked);
                } // end if

                whatNodesAreChecked();

                if (Global.checkedElements.Count == 0)
                {
                    this.buttonGenerateProtobuf.Enabled = false;
                } // end if

            } // end if

        } // end of private void node_AfterCheck(object sender, TreeViewEventArgs eventArgs)

        public void whatNodesAreChecked()
        {
            this.textBoxProtobufOutput.Clear();
            this.textBoxProtobufOutput.Refresh();
            TreeNodeCollection treeNodes = getTreeView().Nodes;
            if (treeNodes != null & treeNodes.Count > 0)
            {
                getTextBox().AppendText("The following nodes have beeen selected for generation:" + Environment.NewLine);
                processTreeNodes(treeNodes, 1);
            } // end if

        } // end of public void whatNodesAreChecked()

        private void processTreeNodes(TreeNodeCollection treeNodes, int depth)
        {
            foreach (TreeNode treeNode in treeNodes)
            {
                if (treeNode.Checked)
                {
                    Global.checkedElements.Add(treeNode.Text);
                    getTextBox().AppendText("".PadLeft(4 * depth) + treeNode.Text + Environment.NewLine);
                } // end if

                if (treeNode.Nodes != null & treeNode.Nodes.Count > 0)
                {
                    processTreeNodes(treeNode.Nodes, depth + 1);
                } // end if

            } // end foreach

        } //end of private void processTreeNodes(TreeNode treeNode)

        public void disableGenerateButton()
        {
            this.buttonGenerateProtobuf.Enabled = false;

        } // end of public void DisableGenerateButton()

        public TreeView getTreeView()
        {
            return treeViewMessageProfile;

        } // end of public TreeView getTreeView()

        public TextBox getTextBox()
        {
            return textBoxProtobufOutput;

        } // end of public TextBox getTextBox()

        private void buttonSaveProtobuf_Click(object sender, EventArgs eeventArgs)
        {
            this.buttonSaveProtobuf.Enabled = false;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Global.userAction.OnSaveAction(folderBrowserDialog.SelectedPath);
            } // end if

        } // end of private void buttonSaveProtobuf_Click(object sender, EventArgs eventArgs)

        private void buttonGenerateProtobuf_Click(object sender, EventArgs eventArgs)
        {
            this.buttonGenerateProtobuf.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            Global.userAction.OnGenerateAction();
            Cursor.Current = Cursors.Default;
            if (!Global.errorGeneratingProtobuf)
            {
                this.buttonSaveProtobuf.Enabled = true;
            } // end if

        } // end of private void buttonGenerateProtobuf_Click(object sender, EventArgs eventArgs)

        private void treeViewModule_AfterCheck(object sender, TreeViewEventArgs eventArgs)
        {
            node_AfterCheck(sender, eventArgs);

        } // end of private void treeViewModule_AfterCheck(object sender, TreeViewEventArgs eventArgs)

        private void treeViewModule_AfterSelect(object sender, TreeViewEventArgs eventArgs)
        {

        } // end of private void treeViewModule_AfterSelect(object sender, TreeViewEventArgs eventArgs)

        private void textBoxProtobufOutput_TextChanged(object sender, EventArgs eventArgs)
        {
            return;

        } // end of private void textBoxProtobufOutput_TextChanged(object sender, EventArgs eventArgs)

    } // end of public partial class OpenFMBMessageProfileSelector : Form

} // end of namespace EAProtobufExporter
