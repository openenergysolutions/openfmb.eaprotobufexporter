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
using System.Windows.Forms;

namespace EAProtobufExporter
{
    public class TextBoxOutput
    {
        private TextBox textBox = null;
        private WriteProto3File writeProto3LogFile = null;

        public TextBoxOutput(TextBox textBox)
        {
            this.textBox = textBox;
            writeProto3LogFile = new WriteProto3File();

        } // end of default constructor

        public void clear()
        {
            textBox.Clear();
            textBox.Refresh();

        } // end of public void clear()

        public String indent(int depth)
        {
            return "".PadLeft(4 * depth);

        } // end of public String indent(int depth)

        public void outputText(String text)
        {
            textBox.AppendText(text);
            writeProto3LogFile.append(0, text);

        } // end of public void outputText(String text)

        public void outputText(int depth, String text)
        {
            textBox.AppendText(indent(depth) + text);
            writeProto3LogFile.append(depth, text);

        } // end of public void outputText(int depth, String text)

        public void outputTextLine()
        {
            textBox.AppendText(Environment.NewLine);
            writeProto3LogFile.append(0, Environment.NewLine);

        } // end of public void outputTextLine()

        public void outputTextLine(String text)
        {
            textBox.AppendText(text + Environment.NewLine);
            writeProto3LogFile.append(0, text);

        } // end of public void outputTextLine(String text)

        public void outputTextLine(int depth, String text)
        {
            textBox.AppendText(indent(depth) + text + Environment.NewLine);
            writeProto3LogFile.append(depth, text);

        } // end of public void outputTextLine(int depth, String text)

        public void writeLogFile(String logFileName)
        {
            writeProto3LogFile.outputFileName = logFileName;
            writeProto3LogFile.writeFile();

        } // end of public void writeLogFile(String logFileName)

    } // end of public class TextBoxOutput

} // end of namespace EAProtobufExporter
