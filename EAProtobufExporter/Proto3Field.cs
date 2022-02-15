// SPDX-FileCopyrightText: 2022 Open Energy Solutions Inc
//
// SPDX-License-Identifier: Apache-2.0

using System;

namespace EAProtobufExporter
{
    public class Proto3Field
    {
        private String variableType = null;
        private String variableName = null;
        private String comment = null;
        private String proto3FieldName = null;
        private String externalPackageName = null;
        private int defaultValue = -1;
        private Boolean parentMessage = false;
        private int minMultiplicity = -1;
        private int maxMultiplicity = -1;
        private Boolean infiniteMaxMultiplicity = false;
        private Boolean uuid = false;
        private Boolean key = false;
        private Boolean primitiveDataTypeWrapperSet = false;
        private Boolean optionalEnumerationWrapperSet = false;

        public Proto3Field(String variableType, String variableName)
        {
            this.variableType = variableType;
            this.variableName = variableName;

        } // end of default constructor public Proto3Field(String variableType, String variableName)

        public void write(WriteProto3File writeProto3File, Boolean writeComment, Boolean writeEnumeration, String fieldVariableNamePrefix)
        {
            String stringToWrite = null;

            proto3FieldName = DataTypeConversion.getProto3DataType(variableType);
            if (proto3FieldName != null && proto3FieldName.Length > 0)
            {
                externalPackageName = null;
                if (minMultiplicity == 0)
                {
                    String primitiveDataTypeWrapperName = PrimitiveDataTypeWrappers.getWrapperDataType(proto3FieldName);
                    if (primitiveDataTypeWrapperName != null)
                    {
                        proto3FieldName = primitiveDataTypeWrapperName;
                        primitiveDataTypeWrapperSet = true;
                    } // end if

                } // end if

            } // end if

            if (writeComment)
            {
                if (parentMessage && (comment == null || comment.Length == 0))
                {
                    comment = "UML inherited base object";
                } // end if

                writeProto3File.writeComment(1, comment);

            } // end if

            if (writeEnumeration)
            {
                variableName = fieldVariableNamePrefix + "_" + variableName;
            } // end if

            if (externalPackageName != null)
            {
                if (proto3FieldName == null)
                {
                    if (optionalEnumerationWrapperSet)
                    {
                        stringToWrite = externalPackageName + ".Optional_" + variableType + " " + variableName + " = " + defaultValue;
                    }
                    else
                    {
                        stringToWrite = externalPackageName + "." + variableType + " " + variableName + " = " + defaultValue;
                    } // end else

                }
                else
                {
                    if (optionalEnumerationWrapperSet)
                    {
                        stringToWrite = externalPackageName + ".Optional_" + proto3FieldName + " " + variableName + " = " + defaultValue;
                    }
                    else
                    {
                        stringToWrite = externalPackageName + "." + proto3FieldName + " " + variableName + " = " + defaultValue;
                    } // end else
                    
                } // end else

            }
            else
            {
                if (proto3FieldName == null)
                {
                    if (optionalEnumerationWrapperSet)
                    {
                        stringToWrite = "Optional_" + variableType + " " + variableName + " = " + defaultValue;
                    }
                    else
                    {
                        stringToWrite = variableType + " " + variableName + " = " + defaultValue;
                    } // end else

                }
                else
                {
                    if (optionalEnumerationWrapperSet)
                    {
                        stringToWrite = "Optional_" + proto3FieldName + " " + variableName + " = " + defaultValue;
                    }
                    else
                    {
                        stringToWrite = proto3FieldName + " " + variableName + " = " + defaultValue;
                    } // end else

                } // end else

            } // end else

            if (!writeEnumeration)
            {
                if (key)
                {
                    if (stringToWrite.Contains("[("))
                    {
                        stringToWrite += ", (uml.option_key) = true";
                    }
                    else
                    {
                        stringToWrite += " [(uml.option_key) = true";
                    } // end else

                } // end if

                if (uuid)
                {
                    if (stringToWrite.Contains("[("))
                    {
                        stringToWrite += ", (uml.option_uuid) = true";
                    }
                    else
                    {
                        stringToWrite += " [(uml.option_uuid) = true";
                    } // end else

                } // end if

                if (parentMessage)
                {
                    if (stringToWrite.Contains("[("))
                    {
                        stringToWrite += ", (uml.option_parent_message) = true";
                    }
                    else
                    {
                        stringToWrite += " [(uml.option_parent_message) = true";
                    } // end else

                } // end if

                if (minMultiplicity == 0 && infiniteMaxMultiplicity)
                {
                    stringToWrite = "repeated " + stringToWrite;
                    if (stringToWrite.Contains("[("))
                    {
                        stringToWrite += ", (uml.option_multiplicity_min) = " + minMultiplicity;
                    }
                    else
                    {
                        stringToWrite += " [(uml.option_multiplicity_min) = " + minMultiplicity;
                    } // end else

                }
                else if (minMultiplicity == 1)
                {
                    if (stringToWrite.Contains("[("))
                    {
                        stringToWrite += ", (uml.option_required_field) = true, (uml.option_multiplicity_min) = " + minMultiplicity;
                    }
                    else
                    {
                        stringToWrite += " [(uml.option_required_field) = true, (uml.option_multiplicity_min) = " + minMultiplicity; ;
                    } // end else

                    if (maxMultiplicity >= 2)
                    {
                        stringToWrite = "repeated " + stringToWrite + ", (uml.option_multiplicity_max) = " + maxMultiplicity;
                    }
                    else if (infiniteMaxMultiplicity)
                    {
                        stringToWrite = "repeated " + stringToWrite;
                    } // end else if

                } // end else if

            } // end if

            if (stringToWrite.Contains("[("))
            {
                stringToWrite += "];";
            }
            else
            {
                stringToWrite += ";";
            } // end else

            writeProto3File.append(1, stringToWrite);

        } // end of public void write(WriteProto3File writeProto3File, Boolean writeComment, Boolean writeEnumeration, String fieldVariableNamePrefix)

        public void print(int indent)
        {
            Global.textBoxOutput.outputTextLine();
            Global.textBoxOutput.outputTextLine(indent, "Proto3Field");
            indent += 1;
            Global.textBoxOutput.outputTextLine(indent, "variableType: " + variableType);
            Global.textBoxOutput.outputTextLine(indent, "proto3FieldName: " + proto3FieldName);
            Global.textBoxOutput.outputTextLine(indent, "variableName: " + variableName);
            Global.textBoxOutput.outputTextLine(indent, "comment: " + comment);
            Global.textBoxOutput.outputTextLine(indent, "externalPackageName: " + externalPackageName);
            Global.textBoxOutput.outputTextLine(indent, "defaultValue: " + defaultValue);
            Global.textBoxOutput.outputTextLine(indent, "parentMessage: " + parentMessage);
            Global.textBoxOutput.outputTextLine(indent, "minMultiplicity: " + minMultiplicity);
            Global.textBoxOutput.outputTextLine(indent, "maxMultiplicity: " + maxMultiplicity);
            Global.textBoxOutput.outputTextLine(indent, "infiniteMaxMultiplicity: " + infiniteMaxMultiplicity);
            Global.textBoxOutput.outputTextLine(indent, "uuid: " + uuid);

        } // end of public void print(int indent)

        public String getVariableType()
        {
            return variableType;

        } // end public String getVariableType()

        public void setVariableType(String variableType)
        {
            this.variableType = variableType;

        } // end public void setVariableType(String variableType)

        public String getVariableName()
        {
            return variableName;

        } // end public String getVariableName()

        public void setVariableName(String variableName)
        {
            this.variableName = variableName;

        } // end public void setVariableName(String variableName)

        public String getComment()
        {
            return comment;

        } // end public String getComment()

        public void setComment(String comment)
        {
            this.comment = comment;

        } // end public void setComment(String comment)

        public String getProto3FieldName()
        {
            return proto3FieldName;

        } // end public String getProto3FieldName()

        public void setProto3FieldName(String proto3FieldName)
        {
            this.proto3FieldName = proto3FieldName;

        } // end public void setProto3FieldName(String proto3FieldName)

        public String getExternalPackageName()
        {
            return externalPackageName;

        } // end public String getExternalPackageName()

        public void setExternalPackageName(String externalPackageName)
        {
            this.externalPackageName = externalPackageName;

        } // end public void setExternalPackageName(String externalPackageName)

        public int getDefaultValue()
        {
            return defaultValue;

        } // end public int getDefaultValue()

        public void setDefaultValue(int defaultValue)
        {
            this.defaultValue = defaultValue;

        } // end public void setDefaultValue(int defaultValue)

        public Boolean isParentMessage()
        {
            return parentMessage;

        } // end public Boolean isParentMessage()

        public void setParentMessage(Boolean parentMessage)
        {
            this.parentMessage = parentMessage;

        } // end public void setParentMessage(Boolean parentMessage)

        public int getMinMultiplicity()
        {
            return minMultiplicity;

        } // end public int getMinMultiplicity()

        public void setMinMultiplicity(int minMultiplicity)
        {
            this.minMultiplicity = minMultiplicity;

        } // end public void setMinMultiplicity(int minMultiplicity)

        public int getMaxMultiplicity()
        {
            return maxMultiplicity;

        } // end public int getMaxMultiplicity()

        public void setMaxMultiplicity(int maxMultiplicity)
        {
            this.maxMultiplicity = maxMultiplicity;

        } // end public void setMaxMultiplicity(int maxMultiplicity)

        public Boolean isInfiniteMaxMultiplicity()
        {
            return infiniteMaxMultiplicity;

        } // end public Boolean isInfiniteMaxMultiplicity()

        public void setInfiniteMaxMultiplicity(Boolean infiniteMaxMultiplicity)
        {
            this.infiniteMaxMultiplicity = infiniteMaxMultiplicity;

        } // end public void setInfiniteMaxMultiplicity(Boolean infiniteMaxMultiplicity)

        public Boolean isUUID()
        {
            return uuid;

        } // end public Boolean isUUID()

        public void setUUID(Boolean uuid)
        {
            this.uuid = uuid;

        } // end public void setUUID(Boolean uuid)

        public Boolean isKey()
        {
            return key;

        } // end public Boolean isKey()

        public void setKey(Boolean key)
        {
            this.key = key;

        } // end public void setKey(Boolean key)

        public Boolean isPrimitiveDataTypeWrapperSet()
        {
            return primitiveDataTypeWrapperSet;

        } // end public Boolean isPrimitiveDataTypeWrapperSet()

        public void setOptionalEnumerationWrapperSet(Boolean optionalEnumerationWrapperSet)
        {
            this.optionalEnumerationWrapperSet = optionalEnumerationWrapperSet;

        } // end public void setOptionalEnumerationWrapperSet(Boolean optionalEnumerationWrapperSet)

    } // end of public class Proto3Field

} // end of namespace EAProtobufExporter
