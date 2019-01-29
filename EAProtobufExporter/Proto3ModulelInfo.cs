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

namespace EAProtobufExporter
{
    public class Proto3ModulelInfo
    {
        private String packageName = null;
        private String comment = null;
        private String stereotype = null;
        private Proto3MessageEnumeration proto3MessageEnumeration = null;
        private String selectedPath = null;
        private WriteProto3File writeProto3File = null;

        private const String IMPORT_PACKAGE_NAME = "descriptor.proto";

        public const String GO_PACKAGE_OPTION_NAME = "ProtobufTag_go_package";
        private String goPackageOptionValue = null;

        public const String JAVA_PACKAGE_OPTION_NAME = "ProtobufTag_java_package";
        private String javaPackageOptionValue = null;

        public const String JAVA_MULTIPLE_FILES_OPTION_NAME = "ProtobufTag_java_multiple_files";
        private String javaMultipleFilesOptionValue = null;

        public const String CSHARP_NAMESPACE_OPTION_NAME = "ProtobufTag_csharp_namespace";
        private String cSharpNamespaceOptionValue = null;

        public const String PACKAGE_NAME = "ProtobufTag_package";
        private String packageValue = null;

        public Proto3ModulelInfo(String packageName)
        {
            this.packageName = packageName;
            writeProto3File = new WriteProto3File();

        } // end of default constructor public Proto3ModuleInfo(String packageName)

        public Boolean isValid()
        {
            Boolean valid = true;

            if (getPackageName() == null)
            {
                Global.textBoxOutput.outputTextLine(0, "ERROR - Proto3ModuleInfo.IsValid - The packageName is null.");
                valid = false;
            } // end if

            if (getGoPackageOptionValue() == null)
            {
                Global.textBoxOutput.outputTextLine(0, "ERROR - Proto3ModuleInfo.IsValid - The ProtobufTag_go_package was not found.");
                valid = false;
            } // end if

            if (getJavaPackageOptionValue() == null)
            {
                Global.textBoxOutput.outputTextLine(0, "ERROR - Proto3ModuleInfo.IsValid - The ProtobufTag_java_package was not found.");
                valid = false;
            } // end if

            if (getJavaMultipleFilesOptionValue() == null)
            {
                Global.textBoxOutput.outputTextLine(0, "ERROR - Proto3ModuleInfo.IsValid - The ProtobufTag_java_multiple_files was not found.");
                valid = false;
            } // end if

            if (getCSharpNamespaceOptionValue() == null)
            {
                Global.textBoxOutput.outputTextLine(0, "ERROR - Proto3ModuleInfo.IsValid - The ProtobufTag_csharp_namespace was not found.");
                valid = false;
            } // end if

            return valid;

        } // end of public Boolean isValid()

        public void write()
        {
            writeProto3File.clear();
            writeProto3File.outputFileName = selectedPath + "\\" + packageName + "\\uml.proto";
            writeProto3File.append(0, getPackageSyntax());
            writeProto3File.append(0, "");
            writeProto3File.append(0, "// " + Global.umlFileName);
            writeProto3File.append(0, "// " + Global.protoFileGenerationDateTime);
            writeProto3File.append(0, "");

            if (comment != null && comment.Length > 0)
            {
                writeProto3File.writeComment(0, comment);
            } // end if

            writeProto3File.append(0, "package " + getPackageValue() + ";");
            writeProto3File.append(0, getGoPackageOptionValue() + "\";");
            writeProto3File.append(0, getJavaPackageOptionValue() + "\";");
            writeProto3File.append(0, getJavaMultipleFilesOptionValue());
            writeProto3File.append(0, getCSharpNamespaceOptionValue() + "\";");
            writeProto3File.append(0, "");

            if (stereotype != null)
            {
                String temp = stereotype.Replace(".", "/");
                writeProto3File.append(0, "import \"" + temp + "/" + IMPORT_PACKAGE_NAME + "\";");
            } // end if

            writeProto3File.append(0, "");

            if (proto3MessageEnumeration != null)
            {
                proto3MessageEnumeration.write(writeProto3File, false);
            } // end if

            writeProto3File.writeFile();
            Global.textBoxOutput.outputTextLine("Protobuf file '" + writeProto3File.outputFileName + "' saved.");

        } // end of public void write()

        public void print(int indent)
        {
            Global.textBoxOutput.outputTextLine();
            Global.textBoxOutput.outputTextLine(indent, "Proto3ModuleInfo");
            indent += 1;
            Global.textBoxOutput.outputTextLine(indent, "packageName: " + packageName);
            Global.textBoxOutput.outputTextLine(indent, "comment: " + comment);
            Global.textBoxOutput.outputTextLine(indent, "stereotype: " + stereotype);
            Global.textBoxOutput.outputTextLine(indent, "ProtobufTag_go_package: " + goPackageOptionValue);
            Global.textBoxOutput.outputTextLine(indent, "ProtobufTag_java_package: " + javaPackageOptionValue);
            Global.textBoxOutput.outputTextLine(indent, "ProtobufTag_java_multiple_files: " + javaMultipleFilesOptionValue);
            Global.textBoxOutput.outputTextLine(indent, "ProtobufTag_csharp_namespace: " + cSharpNamespaceOptionValue);
            Global.textBoxOutput.outputTextLine(indent, "ProtobufTag_package: " + packageValue);
            if (proto3MessageEnumeration != null)
            {
                proto3MessageEnumeration.print(indent);
            } // end if

        } // end of public void print(int indent)

        public String getPackageName()
        {
            return packageName;

        } // end of public String getPackageName()

        public String getComment()
        {
            return comment;

        } // end of public String getComment()

        public void setComment(String comment)
        {
            this.comment = comment;

        } // end of public void setComment(String comment)

        public void setStereotype(String stereotype)
        {
            this.stereotype = stereotype;

        } // end of public void setSterotype(String sterotype)

        public Proto3MessageEnumeration getProto3MessageEnumeration()
        {
            return proto3MessageEnumeration;

        } // end of public Proto3MessageEnumeration getProto3MessageEnumeration()

        public void setProto3MessageEnumeration(Proto3MessageEnumeration proto3MessageEnumeration)
        {
            this.proto3MessageEnumeration = proto3MessageEnumeration;

        } // end of public void setProto3MessageEnumeration(Proto3MessageEnumeration proto3MessageEnumeration)

        public void setSelectedPath(String selectedPath)
        {
            this.selectedPath = selectedPath;

        } // end of public void setSelectedPath(String selectedPath)

        public String getPackageSyntax()
        {
            return "syntax = \"proto3\";";

        } // end of public String getPackageSyntax()

        public String getPackage()
        {
            return "package ";

        } // end of public String getPackage()

        public String getGoPackageOptionValue()
        {
            return goPackageOptionValue;

        } // end of public String getGoPackageOptionValue()

        public void setGoPackageOptionValue(String value)
        {
            goPackageOptionValue = "option go_package = \"" + value;

        } // end of public void setGoPackageOptionValue(String value)

        public String getJavaPackageOptionValue()
        {
            return javaPackageOptionValue;

        } // end of public String getJavaPackageOptionValue()

        public void setJavaPackageOptionValue(String value)
        {
            javaPackageOptionValue = "option java_package = \"" + value;

        } // end of public void setJavaPackageOptionValue(String value)

        public String getJavaMultipleFilesOptionValue()
        {
            return javaMultipleFilesOptionValue;

        } // end of public String getJavaMultipleFilesOptionValue()

        public void setJavaMultipleFilesOptionValue(String value)
        {
            javaMultipleFilesOptionValue = "option java_multiple_files = " + value + ";";

        } // end of public void setJavaMultipleFilesOptionValue(String value)

        public String getCSharpNamespaceOptionValue()
        {
            return cSharpNamespaceOptionValue;

        } // end of public String getCSharpNamespaceOptionValue()

        public void setCSharpNamespaceOptionValue(String value)
        {
            cSharpNamespaceOptionValue = "option csharp_namespace = \"" + value;

        } // end of public void setCSharpNamespaceOptionValue(String value)

        public String getPackageValue()
        {
            return packageValue;

        } // end of public String getPackageValue()

        public void setPackageValue(String value)
        {
            packageValue = value;

        } // end of public void setPackageValue(String value)

        public String getImportPackageValue()
        {
            return "import \"" + getPackageValue() + ".proto\";";

        } // end of public String getImportPackageValue()

    } // end of public class Proto3ModuleInfo

} // end of namespace EAProtobufExporter
