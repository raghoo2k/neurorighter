﻿// SETFILEPATH.CS
// Copyright (c) 2008-2011 John Rolston
//
// This file is part of NeuroRighter.
//
// NeuroRighter is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// NeuroRighter is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with NeuroRighter.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace NeuroRighter
{
    ///<summary>Set file path for NR recordings.</summary>
    ///<author>Jon Newman</author>
    sealed internal partial class NeuroRighter
    {
        // file name and directory for saving
        string filenameOutputs;
        string currentSaveDir;
        string currentSaveFile;

        private void button_BrowseOutputFile_Click(object sender, EventArgs e)
        {
            // Set dialog's default properties
            SaveFileDialog saveFileDialog_OutputFile = new SaveFileDialog();
            saveFileDialog_OutputFile.DefaultExt = "*.spk";         //default extension is for spike files
            saveFileDialog_OutputFile.FileName = filenameOutputs;    //default file name
            saveFileDialog_OutputFile.Filter = "NeuroRighter Files|*.spk|All Files|*.*";
            string tmp = Properties.Settings.Default.savedirectory;
            saveFileDialog_OutputFile.InitialDirectory = tmp;
            // Display Save File Dialog (Windows forms control)
            DialogResult result = saveFileDialog_OutputFile.ShowDialog();
            
            if (result == DialogResult.OK)
            {
                currentSaveDir = new FileInfo(saveFileDialog_OutputFile.FileName).DirectoryName;
                currentSaveFile = new FileInfo(saveFileDialog_OutputFile.FileName).Name;
                Properties.Settings.Default.savedirectory = currentSaveDir;
                Properties.Settings.Default.Save();
                filenameOutputs = saveFileDialog_OutputFile.FileName;
                textBox_OutputFile.Text = filenameOutputs;
                toolTip_outputFilename.SetToolTip(textBox_OutputFile, filenameOutputs);
                filenameBase = filenameOutputs.Substring(0, filenameOutputs.Length - 4);
                originalNameBase = filenameBase;// Save original namebase for repeated recording
            }
        }
    }
}
