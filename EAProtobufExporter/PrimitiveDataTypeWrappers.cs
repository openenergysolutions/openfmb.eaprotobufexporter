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
    class PrimitiveDataTypeWrappers
    {
        static Dictionary<String, String> primitiveDataTypeWrappersDictionary = new Dictionary<String, String>()
        {
            {"double", "DoubleValue"},
            {"float", "FloatValue"},
            {"int64", "Int64Value"},
            {"uint64", "UInt64Value"},
            {"int32", "Int32Value"},
            {"uint32", "UInt32Value"},
            {"bool", "BoolValue"},
            {"string", "StringValue"},
            {"bytes", "BytesValue"},
        };

        public static String getWrapperDataType(String primitiveDataType)
        {
            String returnValue = null;
            if (primitiveDataTypeWrappersDictionary.ContainsKey(primitiveDataType))
            {
                returnValue = "google.protobuf." + primitiveDataTypeWrappersDictionary[primitiveDataType];
            } // end if

            return returnValue;

        } // end of public static String getProto3DataType(String umlDataType)

    } // end of class PrimitiveDataTypeWrappers

} // end of namespace EAProtobufExporter
