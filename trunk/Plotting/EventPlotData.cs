﻿// NeuroRighter v0.04
// Copyright (c) 2008 John Rolston
//
// This file is part of NeuroRighter v0.04.
//
// NeuroRighter v0.04 is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// NeuroRighter v0.04 is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with NeuroRighter v0.04.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;

namespace NeuroRighter
{
    using RawType = System.Double;

    internal class EventPlotData
    {
        private List<PlotSpikeWaveform> waveforms;
        private Int32 waveformLength; //Num samples
        private Double boxHeight;
        private RawType gain;
        private Int32 numRows;
        private Int32 numCols;
        private BackgroundWorker bgWorker;
        private Int32 maxWaveforms;
        private const Int32 REFRESH = 100; //Time between callbacks, in ms
        private Int32[] numWfmsStored;
        private String channelMapping;

        internal delegate void dataAcquiredHandler(object sender);
        internal event dataAcquiredHandler dataAcquired;

        internal EventPlotData(Int32 numChannels, Int32 waveformLength, Double boxHeight, Int32 numRows, 
            Int32 numCols, Int32 maxWaveforms, String channelMapping)
        {
            this.waveformLength = waveformLength;
            this.boxHeight = boxHeight;
            this.numRows = numRows;
            this.numCols = numCols;
            this.maxWaveforms = maxWaveforms;
            this.channelMapping = channelMapping;

            gain = 1.0;
            waveforms = new List<PlotSpikeWaveform>(10);
            numWfmsStored = new Int32[numChannels];

            bgWorker = new BackgroundWorker();
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
        }

        internal Int32 getMaxWaveforms() { return maxWaveforms; }

        void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool isRunning = true;
            while (isRunning)
            {
                if (bgWorker.CancellationPending)
                {
                    isRunning = false;
                    e.Cancel = true;
                    break;
                }
                else
                {
                    if (waveforms.Count > 0)
                    {
                        if (dataAcquired != null) dataAcquired(this);
                        else skipRead(); //No subscribers
                    }

                    Thread.Sleep(REFRESH);
                }
            }
        }

        void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Thread.Sleep(10);
            waveforms.Clear();
        }

        internal void start() { bgWorker.RunWorkerAsync(); }
        internal void stop() { bgWorker.CancelAsync(); }
        
//Would benefit from not doing graphic alignment in 'write', but rather 'read'
        internal void write(List<SpikeWaveform> newWaveforms)
        {
            //Only read first maxWaveforms for each channel
            lock (waveforms)
            {
                for (int i = 0; i < newWaveforms.Count; ++i)
                {
                    if (numWfmsStored[newWaveforms[i].channel] <= maxWaveforms)
                    {
                        RawType[] wfmDataOffset = new RawType[waveformLength];
                        double offset;
                        if (numRows == 8 && channelMapping == "invitro")
                            offset = -(MEAChannelMappings.ch2rc[newWaveforms[i].channel, 0] - 1) * boxHeight;
                        else
                            offset = -(newWaveforms[i].channel / numRows) * boxHeight;
                        for (int k = 0; k < waveformLength; ++k)
                        {
                            RawType temp = newWaveforms[i].waveform[k] * gain;
                            if (temp > boxHeight / 2) temp = boxHeight / 2;
                            else if (temp < -boxHeight / 2) temp = -boxHeight / 2;

                            wfmDataOffset[k] = temp + offset;
                        }
                        waveforms.Add(new PlotSpikeWaveform(newWaveforms[i].channel, wfmDataOffset));
                        ++numWfmsStored[newWaveforms[i].channel];
                    }
                }
            }
        }

        internal List<PlotSpikeWaveform> read()
        {
            lock (waveforms)
            {
                List<PlotSpikeWaveform> output = new List<PlotSpikeWaveform>(waveforms.Count);
                for (int i = 0; i < waveforms.Count; ++i) output.Add(waveforms[i]);

                waveforms.Clear();
                for (int i = 0; i < numWfmsStored.Length; ++i) numWfmsStored[i] = 0;
                return output;
            }
        }

        internal void skipRead()
        {
            lock (waveforms)
            {
                waveforms.Clear(); 
                for (int i = 0; i < numWfmsStored.Length; ++i) numWfmsStored[i] = 0;
            }
        }

        internal Double horizontalOffset(Int32 channel) //Displacement of channel's display in samples
        {
            return (channel % numCols) * waveformLength + 1;
        }

        internal Double getGain() { return gain; }
        internal void setGain(double gain) { this.gain = gain; }
    }
}