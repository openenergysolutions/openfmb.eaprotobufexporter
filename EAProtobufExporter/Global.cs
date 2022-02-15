// SPDX-FileCopyrightText: 2022 Open Energy Solutions Inc
//
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;

namespace EAProtobufExporter
{
    class Global
    {
        public static EA.Repository repository = null;
        public static UserAction userAction = null;
        public static TextBoxOutput textBoxOutput = null;
        public static String umlFileName = null;
        public static String protoFileGenerationDateTime = null;
        public static HashSet<String> checkedElements = null;
        public static HashSet<String> hideCheckBoxList = null;
        public static List<String> informationalMessages = null;
        public static List<String> warningMessages = null;
        public static List<String> errorMessages = null;
        public static Boolean errorGeneratingProtobuf = false;

        public const String INVALID_PROTOBUF_DATATYPE = "Invalid data type in Open FMB Model";

    } // end of class Global

} // end of namespace EAProtobufExporter
