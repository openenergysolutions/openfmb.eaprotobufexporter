# OpenFMB Protobuf Exporter

## Overview

The OpenFMB Protobuf Exporter is an [Enterprise Architect](https://sparxsystems.com/) Add-In to export OpenFMB UML modules or packages as Protocol Buffer definition (.proto) files.

[Enterprise Architect](https://sparxsystems.com/) is a commercial tool that needs appropriate licensing so that the OpenFMB Protobuf Exporter add-in can be installed.

## Installation

1. Download the installer from the release below:

    https://github.com

2. Double click on Setup.exe to start the installer

    ![image](https://user-images.githubusercontent.com/43071770/154144736-8b2e03f6-8cbc-4ed9-b898-eb089890cd31.png)

3. Press Next to continue

    ![image](https://user-images.githubusercontent.com/43071770/154144951-d8662d99-ac49-4fe4-9d76-f7d614693f0d.png)

4. Accept the default program installation directory or select another directory in which to install the DLL

    ![image](https://user-images.githubusercontent.com/43071770/154145129-1f4bc9ed-aa13-4f44-9bfd-1c059e407598.png)

5. Press Install to begin installation and press Finish when it is done

    ![image](https://user-images.githubusercontent.com/43071770/154145486-7b87137c-c4c8-4985-ba8f-de77067f8957.png)


6. If you accept the default directories during installation you will see the following directory structures:

    ![image](https://user-images.githubusercontent.com/43071770/154145959-ac782b09-3129-4113-a900-65e52ed98840.png)

## Running the OpenFMB Protobuf Exporter Add-In

1. Start [Enterprise Architech](https://sparxsystems.com/) and load [OpenFMB UML model](https://gitlab.com/openfmb/pim/ops/-/blob/master/OpenFMB%20Operational%20Model%20(61850-CIM)%20(v2.0.0).eap)

    ![image](https://user-images.githubusercontent.com/43071770/154147105-4cd939b7-c508-46b0-a5fd-085b506ebb58.png)

2. On "Project Browser" window, right click on "OpenFMB", select "Specialize", "OpenFMB Protobuf Exporter", then click on "Export to proto files..." to launch to add-in tool

    ![image](https://user-images.githubusercontent.com/43071770/154147389-1c02048f-5bba-4357-89fc-a05838870679.png)

3. The OpenFMB Protobuf Exporter will build a Tree View containing the model information with check boxes next to the Tree Nodes that are specific to the generation of the Protobuf files in the left pane and processing information in the right pane. Since it is used by the other modules, the Common is always checked.

    ![image](https://user-images.githubusercontent.com/43071770/154147925-f3482a91-5103-4d0c-8e34-46c4c981c7ad.png)

    ![image](https://user-images.githubusercontent.com/43071770/154148027-246620b8-41bf-4499-b391-5d57dfb972e1.png)

4. Click on Export Protobuf button to start exporting

5. When the generation process completes with no errors, the “Save Protobuf” button will be enabled. Clicking the “Save Protobuf” button will bring up a “Browse For Folder” window to allow the user to browse to the directory where the Protobuf files will be saved. When the save process completes, the right pane will display the Protobuf files saved with their absolute paths and the message “Save complete…”

    ![image](https://user-images.githubusercontent.com/43071770/154149301-3034a786-a12c-49a7-80db-f2ec29414444.png)

    The save process will create the Protobuf directory structure if it does not exist. If the directory structure does exist, then the save process will overwrite the existing files. The Protobuf directory structure under the user selected directory is openfmb/<Protobuf Module Name>.

6. Errors during exporting protobuf

    If an error is encountered during processing, the following will be displayed after all error messages:
    An error was encountered while generating the proto3 files.
    The saving of the proto3 files has not been disabled.
    Please correct the error(s) listed and regenerate the proto3 files.

    At this point the user has the following options:
    - Reselect the nodes that do not have errors and generate the Protobuf information. This option will not export a full set of Protobuf files.
    - Close the Tree View window, fix the errors, and re-export the Protobuf information

## Cloning

Clone the repository 

```bash
git clone https://github.com/openenergysolutions/openfmb.eaprotobufexporter.git
```

## Building

The OpenFMB Protobuf Export is written in C# Winform.  Microsoft Visual Studio is needed to build the tool.

## Manual Installation

Manually installing the OpenFMB Protobuf Exporter can be done by following these steps:
1.	Copy the EAProtobufExporter.dll to the desired directory (i.e. “C:\Program Files\OpenFMB\bin”)

2.	Register the DLL by running the Assembly Registration Tool
    ```bash
    C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe "C:\Program Files\OpenFMB\bin\EAProtobufExporter.dll" /codebase
    ```
3.	Create a new entry in the registry by running regedit. This will allow Enterprise Architect to recognize the presence of the OpenFMB Protobuf Exporter Add-In. Add the new key value “EAAddIns” under the appropriate location:
    - For single users: “HKEY_CURRENT_USER\Software\Sparx Systems”
    - For multiple users: “HKEY_LOCAL_MACHINE\Software\Sparx Systems”
    
4.	Under the “EAAddIns” key, add a new key value using the project name “EAProtobufExporter” of the OpenFMB Protobuf Exporter Add-In

5.	Under the “EAProtobufExporter” key, modify the default value by entering the "project-name.class-name" of the OpenFMB Protobuf Exporter Add-In “EAProtobufExporter.Main”

## Contributing

Contributing to the Adapter requires signing a CLA, please email cla@openenergysolutionsinc.com and
request it.

## Other OpenFMB Tools

For more information about OpenFMB and its toolset, visit [OpenFMB Adapter Toolset](https://openfmb.openenergysolutions.com/) document site.
