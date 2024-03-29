﻿// SPDX-FileCopyrightText: 2022 Open Energy Solutions Inc
//
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;

namespace EAProtobufExporter
{
    class DataTypeConversion
    {
        static Dictionary<String, String> invalidDataTypeCheckDictionary = new Dictionary<String, String>()
        {
            {"bool", "bool - Invalid data type in Open FMB Model"},
            {"byte", "int32 - Invalid data type in Open FMB Model"},
            {"float", "float - Invalid data type in Open FMB Model"},
            {"int", "int32 - Invalid data type in Open FMB Model"},
            {"integer", "int32 - Invalid data type in Open FMB Model"},
            {"INT8", "int32 - Invalid data type in Open FMB Model"},
            {"INT16", "int32 - Invalid data type in Open FMB Model"},
            {"INT8U", "uint32 - Invalid data type in Open FMB Model"},
            {"INT16U", "uint32 - Invalid data type in Open FMB Model"},
            {"negativeInteger", "sint32 - Invalid data type in Open FMB Model"},
            {"nonPositveInteger", "sint64 - Invalid data type in Open FMB Model"},
            {"short", "int32 - Invalid data type in Open FMB Model"},
            {"long", "int64 - Invalid data type in Open FMB Model"},
            {"double", "double - Invalid data type in Open FMB Model"},
            {"decimal", "double - Invalid data type in Open FMB Model"},
            {"hexBinary", "bytes - Invalid data type in Open FMB Model"},
            {"nonNegativeInteger", "int32 - Invalid data type in Open FMB Model"},
            {"normalizedString", "string - Invalid data type in Open FMB Model"},
            {"positiveInteger", "int32 - Invalid data type in Open FMB Model"},
            {"unsignedByte", "uint32 - Invalid data type in Open FMB Model"},
            {"unsignedInt", "uint32 - Invalid data type in Open FMB Model"},
            {"unsignedShort", "uint32 - Invalid data type in Open FMB Model"},
            {"unsignedLong", "uint64 - Invalid data type in Open FMB Model"},
            {"ObjRef", "Invalid data type in Open FMB Model"},
        };

        static Dictionary<String, String> dataTypeDictionary = new Dictionary<String, String>()
        {
            {"boolean", "bool"},
            {"string", "string"},
            {"FLOAT32", "float"},
            {"FLOAT64", "double"},
            {"float", "float"},
            {"double", "double" },
            {"INT32", "int32"},
            {"int32", "int32"},
            {"INT64", "int64"},
            {"INT32U", "uint32"},
            {"INT64U", "uint64"},
            {"dateTime", "int64"},
            {"uuidType", "uuidType"},
        };

        static Dictionary<String, String> baseDataTypeDictionary = new Dictionary<String, String>()
        {
            {"boolean", "bool"},
            {"bool", "bool"},
            {"string", "string"},
            {"byte", "int32"},
            {"float", "float"},
            {"int", "int32"},
            {"integer", "int32"},
            {"negativeInteger", "sint32"},
            {"nonPositveInteger", "sint64"},
            {"short", "int32"},
            {"long", "int64"},
            {"double", "double"},
            {"decimal", "double"},
            {"hexBinary", "bytes"},
            {"nonNegativeInteger", "int32"},
            {"normalizedString", "string"},
            {"positiveInteger", "int32"},
            {"unsignedByte", "uint32"},
            {"unsignedInt", "uint32"},
            {"unsignedShort", "uint32"},
            {"unsignedLong", "uint64"},
            {"dateTime", "int64"},
        };

        public static Boolean checkInvalidDataType(String umlDataType)
        {
            Boolean returnValue = false;
            if (invalidDataTypeCheckDictionary.ContainsKey(umlDataType))
            {
                returnValue = (invalidDataTypeCheckDictionary[umlDataType]).Contains(Global.INVALID_PROTOBUF_DATATYPE);
            } // end if

            return returnValue;

        } // end of public static Boolean checkInvalidDataType(String umlDataType)

        public static String getProto3DataType(String umlDataType)
        {
            String returnValue = null;
            if (dataTypeDictionary.ContainsKey(umlDataType))
            {
                returnValue = dataTypeDictionary[umlDataType];
            } // end if

            return returnValue;

        } // end of public static String getProto3DataType(String umlDataType)

        public static String getBaseDataType(String umlDataType)
        {
            String returnValue = null;
            if (baseDataTypeDictionary.ContainsKey(umlDataType))
            {
                returnValue = baseDataTypeDictionary[umlDataType];
            } // end if

            return returnValue;

        } // end of public static String getBaseDataType(String umlDataType)

    } // end of class DataTypeConversion

} // end of namespace EAProtobufExporter
