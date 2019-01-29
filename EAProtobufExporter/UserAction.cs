/*********************************************************************************************
   Copyright 2017 Duke Energy Corporation and Open Energy Solutions, Inc.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
**********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
