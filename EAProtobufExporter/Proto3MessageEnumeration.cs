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
    public class Proto3MessageEnumeration
    {
        private String type = null;
        private String name = null;
        private String comment = null;
        private Boolean writeEnumeration = false;
        private String fieldVariableNamePrefix = null;
        private List<Proto3Field> proto3Fields = null;
        private Boolean primitiveDataTypeWrapperSet = false;
        private Boolean openFmbProfile = false;

        public Proto3MessageEnumeration(String type, String name)
        {
            this.type = type;
            this.name = name;
            proto3Fields = new List<Proto3Field>();
            if (type.Equals("enum"))
            {
                writeEnumeration = true;
                fieldVariableNamePrefix = this.name;
            } // end if

        } // end of default constructor public Proto3MessageEnumeration(String type, String name)

        public Boolean sortProto3Fields()
        {
            Boolean sorted = true;

            if (type.Equals("message"))
            {
                sorted = sortProtobufTags();
            }
            else if (type.Equals("enum"))
            {
                System.Collections.SortedList sortedList = new System.Collections.SortedList();
                foreach (Proto3Field proto3Field in proto3Fields)
                {
                    sortedList.Add(proto3Field.getDefaultValue(), proto3Field);
                } // end foreach

                proto3Fields.Clear();

                for (int i = 0; i < sortedList.Count; i++)
                {
                    proto3Fields.Add((Proto3Field)sortedList.GetByIndex(i));
                } // end for

            } // end else

            return sorted;

        } // end of public Boolean sortProto3Fields()

        private Boolean sortProtobufTags()
        {
            var missingTag = proto3Fields.FirstOrDefault(x => x.getDefaultValue() == -1);
            if (missingTag != null)
            {
                Global.errorGeneratingProtobuf = true;
                Global.textBoxOutput.outputTextLine(1, "Variable Type: " + missingTag.getVariableType() + ", " + "Variable Name: " + missingTag.getVariableName() + ", ProtobufTag not defined.");
                return false;
            }

            proto3Fields = proto3Fields.OrderBy(x => x.getDefaultValue()).ToList();
            return true;
        } // end of private Boolean sortProto3Fields()

        public void write(WriteProto3File writeProto3File, Boolean writeComment)
        {
            Boolean error = false;

            if (proto3Fields != null && proto3Fields.Count > 0)
            {
                if (writeComment)
                {
                    writeProto3File.writeComment(0, comment);
                } // end if

                writeProto3File.append(0, type + " " + name);
                writeProto3File.append(0, "{");

                if (isOpenFmbProfile())
                {
                    writeProto3File.append(1, "option (uml.option_openfmb_profile) = true;");
                }

                if (!error)
                {
                    foreach (Proto3Field proto3Field in proto3Fields)
                    {
                        proto3Field.write(writeProto3File, writeComment, writeEnumeration, fieldVariableNamePrefix);
                        if (proto3Field.isPrimitiveDataTypeWrapperSet())
                        {
                            primitiveDataTypeWrapperSet = true;
                        } // end if

                    } // end foreach

                } // end if

                writeProto3File.append(0, "}");
                writeProto3File.append(0, "");

                if (type.Equals("enum"))
                {
                    writeOptionalEnumerationWrapper(writeProto3File);
                } // end if

            } // end if

        } // end of public void write(WriteProto3File writeProto3File, Boolean writeComment)

        private void writeOptionalEnumerationWrapper(WriteProto3File writeProto3File)
        {
            writeProto3File.append(0, "message Optional_" + name);
            writeProto3File.append(0, "{");
            writeProto3File.append(1, name + " value = 1;");
            writeProto3File.append(0, "}");
            writeProto3File.append(0, "");

        } // end of private void writeOptionalEnumerationWrapper(WriteProto3File writeProto3File)

        public void print(int indent)
        {
            Global.textBoxOutput.outputTextLine();
            Global.textBoxOutput.outputTextLine(indent, "Proto3MessageEnumeration");
            indent += 1;
            Global.textBoxOutput.outputTextLine(indent, "type: " + type);
            Global.textBoxOutput.outputTextLine(indent, "name: " + name);
            Global.textBoxOutput.outputTextLine(indent, "comment: " + comment);
            Global.textBoxOutput.outputTextLine(indent, "writeEnumeration: " + writeEnumeration);
            Global.textBoxOutput.outputTextLine(indent, "fieldVariableNamePrefix: " + fieldVariableNamePrefix);
            foreach (Proto3Field proto3Field in proto3Fields)
            {
                proto3Field.print(indent);
            } // foreach

        } // end of public void print(int indent)

        public String getType()
        {
            return type;

        } // end of public String getType()

        public void setType(String type)
        {
            this.type = type;

        } // end public void setType(String type)

        public String getName()
        {
            return name;

        } // end of public String getName()

        public void setName(String name)
        {
            this.name = name;

        } // end public void setName(String name)

        public String getComment()
        {
            return comment;

        } // end public String getComment()

        public void setComment(String comment)
        {
            this.comment = comment;

        } // end public void setComment(String comment)

        public void setProto3FieldsList(List<Proto3Field> proto3Fields)
        {
            this.proto3Fields = proto3Fields;

        } // end of public void setProto3FieldsList(List<Proto3Field> proto3Fields)

        public void AddProto3Field(Proto3Field proto3Field)
        {
            proto3Fields.Add(proto3Field);

        } // end of public void AddProto3Field(Proto3Field proto3Field)

        public Boolean isPrimitiveDataTypeWrapperSet()
        {
            return primitiveDataTypeWrapperSet;

        } // end public Boolean isPrimitiveDataTypeWrapperSet()

        public Boolean isOpenFmbProfile()
        {
            return openFmbProfile;
        }

        public void setOpenFmbProfile(Boolean flag)
        {
            openFmbProfile = flag;
        }

    } // end of public class Proto3MessageEnumeration

} // end of namespace EAProtobufExporter
