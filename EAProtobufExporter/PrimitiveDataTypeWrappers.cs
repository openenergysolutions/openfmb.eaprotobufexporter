// SPDX-FileCopyrightText: 2022 Open Energy Solutions Inc
//
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;

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
