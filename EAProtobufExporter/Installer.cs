// SPDX-FileCopyrightText: 2022 Open Energy Solutions Inc
//
// SPDX-License-Identifier: Apache-2.0

using Microsoft.Build.Tasks;
using System.Collections;
using System.ComponentModel;

namespace EAProtobufExporter
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        public Installer()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            var regasmPath = System.IO.Path.Combine(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory(), @"regasm.exe");
            var file = System.IO.Path.Combine(Context.Parameters["targetdir"].TrimEnd('\\'), "EAProtobufExporter.dll");            
            System.Diagnostics.Process.Start(regasmPath, "/codebase \"" + file + "\"");
            base.Install(stateSaver);
        }

        public override void Commit(IDictionary savedState)
        {
            //var regasmPath = System.IO.Path.Combine(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory(), @"regasm.exe");
            //var file = System.IO.Path.Combine(Context.Parameters["targetdir"].TrimEnd('\\'), "EAProtobufExporter.dll");
            //System.Diagnostics.Process.Start(regasmPath, "/codebase \"" + file + "\"");
            base.Commit(savedState);
        }

        public override void Uninstall(IDictionary savedState)
        {
            var regasmPath = System.IO.Path.Combine(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory(), @"regasm.exe");
            var file = System.IO.Path.Combine(Context.Parameters["targetdir"].TrimEnd('\\'), "EAProtobufExporter.dll");
            System.Diagnostics.Process.Start(regasmPath, "/u \"" + file + "\"");
            base.Uninstall(savedState);
        }
    }
}
