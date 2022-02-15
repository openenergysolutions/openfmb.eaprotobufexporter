// SPDX-FileCopyrightText: 2022 Open Energy Solutions Inc
//
// SPDX-License-Identifier: Apache-2.0

using System;
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
