// SPDX-FileCopyrightText: 2022 Open Energy Solutions Inc
//
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Windows.Forms;

namespace EAProtobufExporter
{
    public class UserAction : Form
    {
        public void OnGenerateAction()
        {
            Main.generateProto3();

        } // end of public void OnGenerateAction()

        public void OnSaveAction(String selectedPath)
        {
            Main.saveProto3(selectedPath);

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // UserAction
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Name = "UserAction";
            this.Load += new System.EventHandler(this.UserAction_Load);
            this.ResumeLayout(false);

        }

        private void UserAction_Load(object sender, EventArgs e)
        {

        } // end of public void OnSaveAction(String selectedPath)

    } // end of public class UserAction

} // end of namespace EAProtobufExporter
