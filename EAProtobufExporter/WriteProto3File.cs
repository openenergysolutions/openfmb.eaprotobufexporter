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

using System.IO;
using System.Text.RegularExpressions;

namespace EAProtobufExporter
{
    public class WriteProto3File
    {
        public String outputFileName = null;
        public List<String> linesToWrite = new List<string>();

        public void clear()
        {
            linesToWrite.Clear();

        } // end of public void clear()

        public void append(int depth, String text)
        {
            linesToWrite.Add(indent(depth) + text);

        } // end of public void append(int depth, String text)

        public void insert(String findText, String insertText)
        {
            if (linesToWrite.Contains(findText))
            {
                int index = linesToWrite.IndexOf(findText);
                linesToWrite.Insert(linesToWrite.IndexOf(findText) + 1, insertText);
            } // end if

        } // end of public void insert(String findText, String insertText)

        public String indent(int depth)
        {
            return "".PadLeft(4 * depth);

        } // end of public String indent(int depth)

        public void writeComment(int depth, String comment)
        {
            int lineSplitIndex = 96;
            char[] charComment = null;
            Boolean done = false;

            if (comment == null || comment.Length == 0)
            {
                append(depth, "// MISSING DOCUMENTATION!!!");
            }
            else
            {
                comment = Regex.Replace(comment, @"\r\n?|\n|\t", " ");
                while (!done)
                {
                    if (comment.Length <= 100)
                    {
                        append(depth, "// " + comment);
                        done = true;
                    }
                    else
                    {
                        charComment = comment.ToCharArray(lineSplitIndex, 1);
                        if (charComment[0] == ' ' || charComment[0] == '.' ||
                        charComment[0] == '!' || charComment[0] == '?' ||
                        charComment[0] == '-' || charComment[0] == ')')
                        {
                            append(depth, "// " + comment.Substring(0, lineSplitIndex));
                            comment = comment.Substring(lineSplitIndex + 1);
                            lineSplitIndex = 100;
                        }
                        else
                        {
                            lineSplitIndex = lineSplitIndex - 1;
                        } // end else

                    } // end else

                } // end while

            } // end else

        } // end of public void writeComment(int depth, String comment)

        public void writeFile()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(outputFileName));
                using (StreamWriter file = new StreamWriter(outputFileName))
                {
                    foreach (String line in linesToWrite)
                    {
                        file.WriteLine(line);
                    } // end foreach

                } // end using
            }
            catch  (IOException e)
            {
                throw e;
            } // end catch

        } // end of public void writeFile()

    } // end of public class WriteProto3File

} // end of namespace EAProtobufExporter
