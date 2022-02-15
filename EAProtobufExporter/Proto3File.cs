// SPDX-FileCopyrightText: 2022 Open Energy Solutions Inc
//
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;

namespace EAProtobufExporter
{
    public class Proto3File
    {
        private Proto3ModulelInfo proto3ModuleInfo = null;
        private String parentPackageName = null;
        private String packageName = null;
        private String comment = null;
        private List<String> importPackageNames = null;
        private List<Proto3MessageEnumeration> proto3MessageEnumerations = null;
        private Boolean selectedPackage = false;
        private String selectedPath = null;
        private WriteProto3File writeProto3File = null;
        private Boolean primitiveDataTypeWrapperSet = false;

        public Proto3File(String parentPackageName, String packageName)
        {
            this.parentPackageName = parentPackageName;
            if (parentPackageName == null)
            {
                Global.errorMessages.Add("ERROR - Proto3File.Proto3File - The parentPackageName is null.");
                Global.errorGeneratingProtobuf = true;
            } // end if

            this.packageName = packageName;
            if (packageName == null)
            {
                Global.errorMessages.Add("ERROR - Proto3File.Proto3File - The packageName is null.");
                Global.errorGeneratingProtobuf = true;
            } // end if

            importPackageNames = new List<String>();
            writeProto3File = new WriteProto3File();
            proto3MessageEnumerations = new List<Proto3MessageEnumeration>();

        } // end of default constructor public Proto3File(String parentPackageName, String packageName)

        public Proto3MessageEnumeration findProto3MessageEnumeration(String elementName)
        {
            Proto3MessageEnumeration proto3MessageEnumeration = null;
            Boolean found = false;
            int i = 0;

            while (!found && i < proto3MessageEnumerations.Count)
            {
                proto3MessageEnumeration = proto3MessageEnumerations.ElementAt(i);
                if (proto3MessageEnumeration.getName().Equals(elementName))
                {
                    found = true;
                } // end if

                i++;

            } // end while

            if (!found)
            {
                proto3MessageEnumeration = null;
            } // end if

            return proto3MessageEnumeration;

        } // end of public Proto3MessageEnumeration findProto3MessageEnumeration(String elementName)

        public void write(Proto3ModulelInfo proto3ModuleInfo)
        {
            writeProto3File.clear();
            writeProto3File.outputFileName = selectedPath + "\\" + parentPackageName + "\\" + packageName + "\\" + packageName + ".proto";
            writeProto3File.append(0, proto3ModuleInfo.getPackageSyntax());
            writeProto3File.append(0, "");
            writeProto3File.append(0, "// " + Global.umlFileName);
            writeProto3File.append(0, "// " + Global.protoFileGenerationDateTime);
            writeProto3File.append(0, "");

            if (comment != null && comment.Length > 0)
            {
                writeProto3File.writeComment(0, comment);
            } // end if

            writeProto3File.append(0, "package " + packageName + ";");
            writeProto3File.append(0, proto3ModuleInfo.getGoPackageOptionValue() + "/" + packageName + "\";");
            writeProto3File.append(0, proto3ModuleInfo.getJavaPackageOptionValue() + "." + packageName + "\";");
            writeProto3File.append(0, proto3ModuleInfo.getJavaMultipleFilesOptionValue());
            writeProto3File.append(0, proto3ModuleInfo.getCSharpNamespaceOptionValue() + "." + packageName + "\";");
            writeProto3File.append(0, "");
            writeProto3File.append(0, proto3ModuleInfo.getImportPackageValue());

            foreach (String importPackageName in importPackageNames)
            {
                writeProto3File.append(0, "import \"" + importPackageName + "/" + importPackageName + ".proto\";");
            } // end foreach

            writeProto3File.append(0, "");

            foreach (Proto3MessageEnumeration proto3MessageEnumeration in proto3MessageEnumerations)
            {
                proto3MessageEnumeration.write(writeProto3File, true);
                if (!primitiveDataTypeWrapperSet && proto3MessageEnumeration.isPrimitiveDataTypeWrapperSet())
                {
                    primitiveDataTypeWrapperSet = true;
                } // end if

            } // end foreach

            if (primitiveDataTypeWrapperSet)
            {
                writeProto3File.insert("import \"uml.proto\";", "import \"google/protobuf/wrappers.proto\";");
            } // end if

            writeProto3File.writeFile();
            Global.textBoxOutput.outputTextLine("Protobuf file '" + writeProto3File.outputFileName + "' saved.");

        } // end of public void write(Proto3ModuleInfo proto3ModuleInfo)

        public void print(int indent)
        {
            Global.textBoxOutput.outputTextLine();
            Global.textBoxOutput.outputTextLine(indent, "Proto3File");
            indent += 1;
            Global.textBoxOutput.outputTextLine(indent, "packageName: " + packageName);
            Global.textBoxOutput.outputTextLine(indent, "parentPackageName: " + parentPackageName);
            Global.textBoxOutput.outputTextLine(indent, "comment: " + comment);
            Global.textBoxOutput.outputTextLine(indent, "selectedPackage: " + selectedPackage);
            Global.textBoxOutput.outputTextLine(indent, "selectedPath: " + selectedPath);
            foreach (String importPackageName in importPackageNames)
            {
                Global.textBoxOutput.outputTextLine(indent, "importPackageName: " + importPackageName);
            } // end foreach

            foreach (Proto3MessageEnumeration proto3MessageEnumeration in proto3MessageEnumerations)
            {
                proto3MessageEnumeration.print(indent);
            } // end foreach

        } // end of public void print(int indent)

        public Proto3ModulelInfo getProto3ModulelInfo()
        {
            return proto3ModuleInfo;

        } // end public Proto3ModulelInfo getProto3ModulelInfo()

        public void setProto3ModulelInfo(Proto3ModulelInfo proto3ModuleInfo)
        {
            this.proto3ModuleInfo = proto3ModuleInfo;

        } // end public void setProto3ModulelInfo(Proto3ModulelInfo proto3ModuleInfo)

        public String getPackageName()
        {
            return packageName;

        } // end public String getPackageName()

        public void setPackageName(String packageName)
        {
            this.packageName = packageName;

        } // end public void setPackageName(String packageName)

        public String getParentPackageName()
        {
            return parentPackageName;

        } // end public String getParentPackageName()

        public void setParentPackageName(String parentPackageName)
        {
            this.parentPackageName = parentPackageName;

        } // end public void setParentPackageName(String parentPackageName)

        public void AddImport(String packageName)
        {
            if (!importPackageNames.Contains(packageName))
            {
                importPackageNames.Add(packageName);
            } // end if

        } // end of public void AddImport(String packageName)

        public void AddProto3MessageEnumeration(Proto3MessageEnumeration proto3MessageEnumeration)
        {
            proto3MessageEnumerations.Add(proto3MessageEnumeration);

        } // end of public void AddProto3MessageEnumeration(Proto3MessageEnumeration proto3MessageEnumeration)

        public List<Proto3MessageEnumeration> getProto3MessageEnumerationsList()
        {
            return proto3MessageEnumerations;

        } // end public List<Proto3MessageEnumeration> getProto3MessageEnumerationsList()

        public String getComment()
        {
            return comment;

        } // end public String getComment()

        public void setComment(String comment)
        {
            this.comment = comment;

        } // end public void setComment(String comment)

        public String getSelectedPath()
        {
            return selectedPath;

        } // end public String getSelectedPath()

        public void setSelectedPath(String selectedPath)
        {
            this.selectedPath = selectedPath;

        } // end public void setSelectedPath(String selectedPath)

        public Boolean isSelectedPackage()
        {
            return selectedPackage;

        } // end public Boolean isSelectedPackage()

        public void setSelectedPackage(Boolean selectedPackage)
        {
            this.selectedPackage = selectedPackage;

        } // end public void setSelectedPackage(Boolean selectedPackage)

    } // end of public class Proto3File

} // end of namespace EAProtobufExporter
