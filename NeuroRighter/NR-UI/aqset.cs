﻿// Copyright (c) 2008-2012 Potter Lab
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using NationalInstruments;
using NationalInstruments.DAQmx;
using NationalInstruments.UI;
using NationalInstruments.UI.WindowsForms;
using NationalInstruments.Analysis;
using NationalInstruments.Analysis.Dsp;
using NationalInstruments.Analysis.Dsp.Filters;
using NationalInstruments.Analysis.Math;
using NationalInstruments.Analysis.SignalGeneration;
using csmatio.types;
using csmatio.io;
using rawType = System.Double;
using NeuroRighter.SpikeDetection;
using NeuroRighter.Filters;

namespace NeuroRighter
{
    ///<summary>Methods for setting up filtering, gain settings, digital referencing etc.</summary>
    ///<author>John Rolston</author>
    sealed internal partial class NeuroRighter
    {
        //Set gain for channels
        private void setGain(Task myTask, double cb)
        {
            for (int i = 0; i < myTask.AIChannels.Count; ++i)
            {
                myTask.AIChannels[i].RangeHigh = 10.0 / cb;
                myTask.AIChannels[i].RangeLow = -10.0 / cb;

                myTask.AIChannels[i].Maximum = 10.0 / cb;
                myTask.AIChannels[i].Minimum = -10.0 / cb;
            }
        }

        // Deal with change in gain setting
        private void comboBox_SpikeGain_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBox_SALPA.Checked = checkBox_SALPA.Enabled = false;
            label_noise.Text = "Noise levels not trained.";
            label_noise.ForeColor = Color.Red;
        }

        // Deal with changes to the number of selected input channels
        private void updateChannelCount()
        {
            bool countChanged = (numChannels != Properties.Settings.Default.NumChannels);
            if (countChanged)
            {
                numChannels = Properties.Settings.Default.NumChannels;
                numChannelsPerDev = (Properties.Settings.Default.NumChannels < 32 ? Properties.Settings.Default.NumChannels : 32);
                spikeFilter = null;
                lfpFilter = null;
                resetSpikeFilter();
                resetLFPFilter();
                thrSALPA = null;
                label_noise.Text = "Noise levels have not been trained.";
                label_noise.ForeColor = Color.Red;
                checkBox_SALPA.Enabled = false;
                checkBox_SALPA.Checked = false;

                //Add more available stim channels
                stimChannel.Maximum = Properties.Settings.Default.NumChannels;
                numericUpDown_impChannel.Maximum = Properties.Settings.Default.NumChannels;
                listBox_stimChannels.Items.Clear();
                for (int i = 0; i < Properties.Settings.Default.NumChannels; ++i)
                {
                    listBox_stimChannels.Items.Add(i + 1);
                    listBox_exptStimChannels.Items.Add(i + 1);
                }

           
                // Reset the spike detector if it exists
                if (spikeDet != null)
                {
                    spikeDet.Close();
                    setSpikeDetectorSettings();
                }
           

                resetReferencers();
            }
        }

       

     

        // Train the SALPA filter
        private void button_Train_Click(object sender, EventArgs e)
        {
            thrSALPA = new rawType[Properties.Settings.Default.NumChannels];

            this.Cursor = Cursors.WaitCursor;

            label_noise.Text = "Noise levels not trained.";
            label_noise.ForeColor = Color.Red;
            label_noise.Update();
            buttonStart.Enabled = false;  //So users can't try to get data from the same card
            int numChannelsPerDevice = (numChannels > 32 ? 32 : numChannels);
            int numDevices = (numChannels > 32 ? Properties.Settings.Default.AnalogInDevice.Count : 1);
            spikeTask = new List<Task>(numDevices);
            for (int i = 0; i < numDevices; ++i)
            {
                spikeTask.Add(new Task("SALPATrainingTask_" + i));
                for (int j = 0; j < numChannelsPerDevice; ++j)
                {
                    spikeTask[i].AIChannels.CreateVoltageChannel(Properties.Settings.Default.AnalogInDevice[i] + "/ai" + j.ToString(), "",
                        AITerminalConfiguration.Nrse, -10.0, 10.0, AIVoltageUnits.Volts);
                }
            }

            //Change gain based on comboBox values (1-100)
            for (int i = 0; i < spikeTask.Count; ++i)
                setGain(spikeTask[i], Properties.Settings.Default.A2Dgain);

            for (int i = 0; i < spikeTask.Count; ++i)
                spikeTask[i].Timing.ReferenceClockSource = "OnboardClock";

            for (int i = 0; i < spikeTask.Count; ++i)
                spikeTask[i].Timing.ConfigureSampleClock("", spikeSamplingRate, SampleClockActiveEdge.Rising,
                    SampleQuantityMode.ContinuousSamples, Convert.ToInt32(spikeSamplingRate / 2));

            // Set reference clock source
            for (int i = 0; i < spikeTask.Count; ++i)
                spikeTask[i].Timing.ReferenceClockSource = "OnboardClock";

            //Verify the Task
            for (int i = 0; i < spikeTask.Count; ++i)
                spikeTask[i].Control(TaskAction.Verify);

            List<AnalogMultiChannelReader> readers = new List<AnalogMultiChannelReader>(spikeTask.Count);
            for (int i = 0; i < spikeTask.Count; ++i)
                readers.Add(new AnalogMultiChannelReader(spikeTask[i].Stream));
            double[][] data = new double[numChannels][];
            int c = 0; //Last channel of 'data' written to
            for (int i = 0; i < readers.Count; ++i)
            {
                double[,] tempData = readers[i].ReadMultiSample((int)(NUM_SECONDS_TRAINING * spikeSamplingRate)); //Get a few seconds of "noise"
                for (int j = 0; j < tempData.GetLength(0); ++j)
                    data[c++] = ArrayOperation.CopyRow(tempData, j);
            }
            for (int i = 0; i < numChannels; ++i)
            {
                thrSALPA[i] = 9 * (rawType)Statistics.Variance(data[i]) / Math.Pow(Properties.Settings.Default.PreAmpGain, 2);
                Console.Out.WriteLine("channel " + i + ": thr = " + thrSALPA[i]);
            }


            //Now, destroy the objects we made
            for (int i = 0; i < spikeTask.Count; ++i)
                spikeTask[i].Dispose();
            spikeTask.Clear();
            spikeTask = null;
            buttonStart.Enabled = true;
            label_noise.Text = "Noise levels trained.";
            label_noise.ForeColor = Color.Green;
            checkBox_SALPA.Enabled = true;

            this.Cursor = Cursors.Default;
        }
        // Set up Artifilt
        private void checkBox_artiFilt_CheckedChanged(object sender, EventArgs e)
        {
            //artiFilt = new Filters.ArtiFilt(0.001, 0.002, spikeSamplingRate, numChannels);
            artiFilt = new Filters.ArtiFilt_Interpolation(0.001, 0.002, spikeSamplingRate, numChannels);
        }

        //// Deal with changes to filter parameters
        //private void button_EnableFilterChanges_Click(object sender, EventArgs e)
        //{
        //    MessageBox.Show("The Potter Lab standard settings for spike detection are: \n" +
        //        "\tLow-Cut = 200 Hz\n" +
        //        "\tHigh-Cut = 5000 Hz\n" +
        //        "\tFilter = 2nd order Butterworth\n" +
        //        "Be careful with these settings because they will affect spike\n" +
        //        "detection and detected spike shape.");

        //    checkBox_spikesFilter.Enabled = true;
        //    SpikeLowCut.Enabled = true;
        //    SpikeHighCut.Enabled = true;
        //    SpikeFiltOrder.Enabled = true;
        //    checkBox_LFPsFilter.Enabled = true;
        //    LFPFiltOrder.Enabled = true;
        //    LFPHighCut.Enabled = true;
        //    LFPLowCut.Enabled = true;
        //    checkBox_artiFilt.Enabled = true;
        //    button_DisableFilterChanges.Enabled = true;
        //    button_EnableFilterChanges.Enabled = false;
        //}

        //private void button_DisableFilterChanges_Click(object sender, EventArgs e)
        //{
        //    checkBox_spikesFilter.Enabled = false;
        //    SpikeLowCut.Enabled = false;
        //    SpikeHighCut.Enabled = false;
        //    SpikeFiltOrder.Enabled = false;
        //    checkBox_LFPsFilter.Enabled = false;
        //    LFPFiltOrder.Enabled = false;
        //    LFPHighCut.Enabled = false;
        //    LFPLowCut.Enabled = false;
        //    checkBox_artiFilt.Enabled = false;
        //    button_DisableFilterChanges.Enabled = false;
        //    button_EnableFilterChanges.Enabled = true;
        //}

        private void SpikeLowCut_ValueChanged(object sender, EventArgs e)
        {
            resetSpikeFilter();
        }
        private void checkBox_spikesFilter_CheckedChanged(object sender, EventArgs e)
        {
            resetSpikeFilter();
            Properties.Settings.Default.UseSpikeBandFilter = checkBox_spikesFilter.Checked;
            recordingSettings.SetSpikeFiltAccess(checkBox_spikesFilter.Checked);
        }
        private void SpikeHighCut_ValueChanged(object sender, EventArgs e)
        {
            resetSpikeFilter();
        }
        private void checkBox_LFPsFilter_CheckedChanged(object sender, EventArgs e)
        {
            resetLFPFilter();
        }
        private void LFPLowCut_ValueChanged(object sender, EventArgs e)
        {
            resetLFPFilter();
        }
        private void LFPHighCut_ValueChanged(object sender, EventArgs e)
        {
            resetLFPFilter();
        }
        private void SpikeFiltOrder_ValueChanged(object sender, EventArgs e)
        {
            resetSpikeFilter();
        }
        private void LFPFiltOrder_ValueChanged(object sender, EventArgs e)
        {
            resetLFPFilter();
        }
        private void EEGLowCut_ValueChanged(object sender, EventArgs e)
        {
            resetEEGFilter();
        }
        private void EEGHighCut_ValueChanged(object sender, EventArgs e)
        {
            resetEEGFilter();
        }
        private void EEGFiltOrder_ValueChanged(object sender, EventArgs e)
        {
            resetEEGFilter();
        }
        private void checkBox_eegFilter_CheckedChanged(object sender, EventArgs e)
        {
            resetEEGFilter();
        }
        private void numericUpDown_salpa_halfwidth_ValueChanged(object sender, EventArgs e)
        {
            resetSALPA();
        }
        private void numericUpDown_salpa_postpeg_ValueChanged(object sender, EventArgs e)
        {
            resetSALPA();
        }
        private void numericUpDown_salpa_forcepeg_ValueChanged(object sender, EventArgs e)
        {
            resetSALPA();
        }
        private void numericUpDown_salpa_ahead_ValueChanged(object sender, EventArgs e)
        {
            resetSALPA();
        }
        private void numericUpDown_salpa_asym_ValueChanged(object sender, EventArgs e)
        {
            resetSALPA();
        }

        private void numericUpDown_MUAHighCut_ValueChanged(object sender, EventArgs e)
        {
            resetMUAFilter();
        }

        private void numericUpDown_MUAFilterOrder_ValueChanged(object sender, EventArgs e)
        {
            resetMUAFilter();
        }


        private void checkBox_SALPA_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_SALPA.Checked)
            {
                numericUpDown_salpa_halfwidth.Enabled = false;
                numericUpDown_salpa_postpeg.Enabled = false;
                numericUpDown_salpa_forcepeg.Enabled = false;
                numericUpDown_salpa_ahead.Enabled = false;
                numericUpDown_salpa_asym.Enabled = false;
                resetSALPA();
            }
            else
            {
                numericUpDown_salpa_halfwidth.Enabled = true;
                numericUpDown_salpa_postpeg.Enabled = true;
                numericUpDown_salpa_forcepeg.Enabled = true;
                numericUpDown_salpa_ahead.Enabled = true;
                numericUpDown_salpa_asym.Enabled = true;
            }

            recordingSettings.SetSalpaAccess(checkBox_SALPA.Checked);
        }

        //Reset spike filter
        private void resetSpikeFilter()
        {
            if (spikeFilter != null)
            {
                lock (spikeFilter)
                {
                    for (int i = 0; i < spikeFilter.Length; ++i)
                        spikeFilter[i].Reset((int)SpikeFiltOrder.Value, Properties.Settings.Default.RawSampleFrequency,
                            Convert.ToDouble(SpikeLowCut.Value), Convert.ToDouble(SpikeHighCut.Value), spikeBufferLength);
                }
            }
            else //spikeFilter is uninitialized
            {
                spikeFilter = new ButterworthFilter[numChannels];
                for (int i = 0; i < numChannels; ++i)
                    spikeFilter[i] = new ButterworthFilter((int)SpikeFiltOrder.Value, Properties.Settings.Default.RawSampleFrequency,
                        Convert.ToDouble(SpikeLowCut.Value), Convert.ToDouble(SpikeHighCut.Value), spikeBufferLength);
            }
        }

        private void setSpikeDetectorSettings() //resets both the spike detector settings gui (which is also used for the spike sorter), as well as the spike detector itself
        {
            spikeDet = new SpikeDetSettings(spikeBufferLength, Properties.Settings.Default.NumChannels);
            spikeDet.SettingsHaveChanged += new SpikeDetSettings.resetSpkDetSettingsHandler(spikeDet_SettingsHaveChanged);
            setSpikeDetector();
        }

        private void setSpikeDetector() //just reset the spike detector- use if need to retrain
        {
            spikeDet.SetSpikeDetector(spikeBufferLength);
        }

        //Reset SALPA filter
        private void resetSALPA()
        {
            // Set SALPA parameters

            int asym_sams = Convert.ToInt32(numericUpDown_salpa_asym.Value);
            int blank_sams = Convert.ToInt32(numericUpDown_salpa_postpeg.Value);
            int ahead_sams = Convert.ToInt32(numericUpDown_salpa_ahead.Value); //0.0002 = 5 samples @ 25 kHz
            int forcepeg_sams = Convert.ToInt32(numericUpDown_salpa_forcepeg.Value);
            SALPA_WIDTH = Convert.ToInt32(numericUpDown_salpa_halfwidth.Value);

            //if (4 * SALPA_WIDTH + 1 > spikeBufferLength) // Make sure that the number of samples needed for polynomial fit is not more than the current buffersize
            //{
            //    double max_halfwidth = Convert.ToDouble((spikeBufferLength - 1) / 4);
            //    SALPA_WIDTH = (int)Math.Floor(max_halfwidth);
            //}


            //public SALPA3(int length_sams,int asym_sams,int blank_sams,int ahead_sams, int forcepeg_sams, rawType railLow, rawType railHigh, int numElectrodes, int bufferLength, rawType[] thresh)
            if (thrSALPA == null)
                MessageBox.Show("train salpa before editing parameters");
            else
                SALPAFilter = new global::NeuroRighter.Filters.SALPA3(SALPA_WIDTH, asym_sams, blank_sams, ahead_sams, forcepeg_sams, (rawType)(-4 * Math.Pow(10, -3)),
                    (rawType)(4 * Math.Pow(10, -3)), numChannels, spikeBufferLength, thrSALPA);
            //SALPAFilter = new SALPA3(SALPA_WIDTH, prepeg, postpeg, postpegzero, (rawType)(-10 / Convert.ToDouble(comboBox_SpikeGain.SelectedItem) + 0.01),
            //    (rawType)(10 / Convert.ToDouble(comboBox_SpikeGain.SelectedItem) - 0.01), numChannels, delta, spikeBufferLength);
        }

        //Reset LFP filter
        private void resetLFPFilter()
        {
            rawType Fs;
            if (Properties.Settings.Default.SeparateLFPBoard)
                Fs = Properties.Settings.Default.LFPSampleFrequency;
            else
                Fs = Properties.Settings.Default.RawSampleFrequency;

            if (lfpFilter != null)
            {
                lock (lfpFilter)
                {
                    for (int i = 0; i < lfpFilter.Length; ++i)
                        //lfpFilter[i].Reset((int)LFPFiltOrder.Value, Fs,
                        //    Convert.ToDouble(LFPLowCut.Value), Convert.ToDouble(LFPHighCut.Value));
                        if (Properties.Settings.Default.SeparateLFPBoard)
                            lfpFilter[i].Reset((int)LFPFiltOrder.Value, Fs,
                                Convert.ToDouble(LFPLowCut.Value), Convert.ToDouble(LFPHighCut.Value), lfpBufferLength);
                        else
                            lfpFilter[i].Reset((int)LFPFiltOrder.Value, Fs,
                                Convert.ToDouble(LFPLowCut.Value), Convert.ToDouble(LFPHighCut.Value), spikeBufferLength);
                }
            }
            else //lfpFilter is uninitialized
            {
                //lfpFilter = new BesselBandpassFilter[numChannels];
                lfpFilter = new ButterworthFilter[numChannels];
                for (int i = 0; i < numChannels; ++i)
                    //lfpFilter[i] = new BesselBandpassFilter((int)LFPFiltOrder.Value, Convert.ToDouble(textBox_lfpSamplingRate.Text),
                    //    Convert.ToDouble(LFPLowCut.Value), Convert.ToDouble(LFPHighCut.Value));
                    if (Properties.Settings.Default.SeparateLFPBoard)
                        lfpFilter[i] = new ButterworthFilter((int)LFPFiltOrder.Value, Properties.Settings.Default.LFPSampleFrequency,
                            Convert.ToDouble(LFPLowCut.Value), Convert.ToDouble(LFPHighCut.Value), lfpBufferLength);
                    else
                        lfpFilter[i] = new ButterworthFilter((int)LFPFiltOrder.Value, Properties.Settings.Default.LFPSampleFrequency,
                            Convert.ToDouble(LFPLowCut.Value), Convert.ToDouble(LFPHighCut.Value), spikeBufferLength);
            }
        }

        //Reset EEG filter
        private void resetEEGFilter()
        {
            if (eegFilter != null)
            {
                lock (eegFilter)
                {
                    for (int i = 0; i < eegFilter.Length; ++i)
                        eegFilter[i].Reset((int)EEGFiltOrder.Value, Convert.ToDouble(Properties.Settings.Default.EEGSamplingRate),
                            (double)EEGLowCut.Value, (double)EEGHighCut.Value);
                }
            }
            else //eegFilter is uninitialized
            {
                //eegFilter = new ButterworthBandpassFilter[16];
                eegFilter = new BesselBandpassFilter[Properties.Settings.Default.EEGNumChannels];
                for (int i = 0; i < Properties.Settings.Default.EEGNumChannels; ++i)
                    //eegFilter[i] = new ButterworthBandpassFilter((int)EEGFiltOrder.Value, Convert.ToDouble(textBox_eegSamplingRate.Text),
                    //    Convert.ToDouble(EEGLowCut.Value), Convert.ToDouble(EEGHighCut.Value));
                    eegFilter[i] = new BesselBandpassFilter((int)EEGFiltOrder.Value, Convert.ToDouble(Properties.Settings.Default.EEGSamplingRate),
                        (double)EEGLowCut.Value, (double)EEGHighCut.Value);
            }
        }

        //Reset MUA filter
        private void resetMUAFilter()
        {
            Properties.Settings.Default.MUAHighCutHz = Convert.ToDouble(numericUpDown_MUAHighCut.Value);
            Properties.Settings.Default.MUAFilterOrder = (int)(numericUpDown_MUAFilterOrder.Value);

            if (muaFilter != null)
            {
                lock (muaFilter)
                {
                    muaFilter.Reset(Properties.Settings.Default.MUAHighCutHz, Properties.Settings.Default.MUAFilterOrder);
                }
            }
            else //lfpFilter is uninitialized
            {
                //lfpFilter = new BesselBandpassFilter[numChannels];
                muaFilter = new Filters.MUAFilter(
                            numChannels, (double)spikeSamplingRate, spikeBufferLength, 
                            Properties.Settings.Default.MUAHighCutHz, 
                            Properties.Settings.Default.MUAFilterOrder, 
                            MUA_DOWNSAMPLE_FACTOR, 
                            Properties.Settings.Default.ADCPollingPeriodSec);
            }
        }

        //Reset dig referencer
        private void resetReferencers()
        {
            if (radioButton_spikeReferencingNone.Checked)
                referncer = null;
            else if (radioButton_spikesReferencingCommonAverage.Checked)
            {
                referncer = new
                Filters.CommonAverageReferencer(spikeBufferLength);
            }
            else if (radioButton_spikesReferencingCommonMedian.Checked)
            {
                referncer = new
                Filters.CommonMedianReferencer(spikeBufferLength, numChannels);
            }
            else if (radioButton_spikesReferencingCommonMedianLocal.Checked)
            {
                int channelsPerGroup =
                Convert.ToInt32(numericUpDown_CommonMedianLocalReferencingChannelsPerGroup.Value);
                referncer = new
                Filters.CommonMedianLocalReferencer(spikeBufferLength, channelsPerGroup, numChannels / channelsPerGroup);
            }
        }
    }
}
