// SPDX-FileCopyrightText: 2022 Open Energy Solutions Inc
//
// SPDX-License-Identifier: Apache-2.0

// To build a 32-bit setup, comment out this line
#define SETUP_64

using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;

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
            var file = System.IO.Path.Combine(Context.Parameters["targetdir"].TrimEnd('\\'), "EAProtobufExporter.dll");
#if SETUP_64
            RegistrationServices service = new RegistrationServices();
            var assembly = System.Reflection.Assembly.LoadFrom(file);
            service.RegisterAssembly(assembly, AssemblyRegistrationFlags.SetCodeBase);
#else
            var regasmPath = System.IO.Path.Combine(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory(), @"regasm.exe");                        
            System.Diagnostics.Process.Start(regasmPath, "/codebase \"" + file + "\"");
#endif
            base.Install(stateSaver);
        }

        public override void Uninstall(IDictionary savedState)
        {
            var file = System.IO.Path.Combine(Context.Parameters["targetdir"].TrimEnd('\\'), "EAProtobufExporter.dll");
#if SETUP_64
            RegistrationServices service = new RegistrationServices();
            var assembly = System.Reflection.Assembly.LoadFrom(file);
            service.UnregisterAssembly(assembly);
#else
            var regasmPath = System.IO.Path.Combine(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory(), @"regasm.exe");            
            System.Diagnostics.Process.Start(regasmPath, "/u \"" + file + "\"");
#endif
            base.Uninstall(savedState);
        }
    }
}
