﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NationalInstruments.DAQmx;


namespace NeuroRighter.dbg
{
    /// <summary>
    /// Debugging tool usefule for fixing protocols created with NeuroRighter's API.
    /// </summary>
    public class RealTimeDebugger
    {

        FileStream debugOut;
        Task timekeeper;
        long reference;
        int index;
        object debuggerlock = new object();

        /// <summary>
        /// Real-time debugger for fixing protocols created with NeuroRighter's API.
        /// </summary>
        internal RealTimeDebugger() {}

        internal void SetPath(string path)
        {
            debugOut = new FileStream(path, FileMode.Create);
            reference = 0;
            index = 0;
            string header = "NeuroRighter real time debugger log\r\nexperiment starting at " + DateTime.Now.ToString() + "\r\nAll times are in ms\r\n\r\n";
            byte[] bytedata = Encoding.ASCII.GetBytes(header);
            debugOut.Write(bytedata, 0, bytedata.Length);
        }

        internal void GrabTimer(Task timekeeper)
        {
            this.timekeeper = timekeeper;
            timekeeper.Control(TaskAction.Commit);
        }

        /// <summary>
        /// Write to the debug log file.
        /// </summary>
        /// <param name="input">String to write to file.</param>
        public void Write(string input)
        {
//#ifdef debug
            if(false)
            lock (debuggerlock)
            {
                long time = 0;
                try
                {
                    time = timekeeper.Stream.TotalSamplesAcquiredPerChannel - reference;
                }
                catch (Exception me)
                { }
                byte[] bytedata = Encoding.ASCII.GetBytes(((double)(time) / 25).ToString() + " : " + input + "\r\n");
                try
                {
                    debugOut.Write(bytedata, 0, bytedata.Length);
                }
                catch (Exception me) { }
            }
        }

        internal double GetTimeMS()
        {
            return (timekeeper.Stream.TotalSamplesAcquiredPerChannel - reference) / 25;
        }

        internal void WriteReference(string input)
        {
            lock (debuggerlock)
            {
                reference = timekeeper.Stream.TotalSamplesAcquiredPerChannel;
                byte[] bytedata = Encoding.ASCII.GetBytes("\r\n"+((double)(reference) / 25).ToString() + " : " + input + "\r\n");
                debugOut.Write(bytedata, 0, bytedata.Length);
            }
        }

        internal void Close()
        {
            debugOut.Close();
        }
    }
}
