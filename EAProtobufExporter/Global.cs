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
