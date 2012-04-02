﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using simoc.UI;
using simoc.srv;
using simoc.persistantstate;
using NeuroRighter.StimSrv;
using NeuroRighter.DataTypes;


namespace simoc.filt2out
{
    class Filt2PIDPowerMultiFB : Filt2Out
    {
        ulong pulseWidthSamples;
        double offVoltage = -0.5;
        double K;
        double Ti;
        double Td;
        double currentFilteredValue;
        double maxStimPowerVolts;
        double stimPulseWidthMSec;
        double currentTargetIntenal;
        double lastErrorIntenal;
        double N = 10;
        double maxStimFreq;
        double maxStimPulseWidthMSec;

        public Filt2PIDPowerMultiFB(ref NRStimSrv stimSrv, ControlPanel cp)
            : base(ref stimSrv, cp)
        {
            numberOutStreams = 8; 
            K = c0;
            if (c1 != 0)
                Ti = 1 / c1;
            else
                Ti = 0;
            Td = c2;
            maxStimFreq = c3;
            maxStimPulseWidthMSec = c4;
            maxStimPowerVolts = c5;
        }

        internal override void CalculateError(ref double currentError, double currentTarget, double currentFilt)
        {
            currentFilteredValue = currentFilt;
            base.CalculateError(ref currentError, currentTarget, currentFilt);
            if (currentTarget >= 0)
            {
                lastErrorIntenal = currentError;
                currentError = (currentTarget - currentFilt);  // currentTarget;
            }
            else
            {
                lastErrorIntenal = currentError;
                currentError = 0;
            }
            currentErrorIntenal = currentError;
            currentTargetIntenal = currentTarget;
        }


        internal override void SendFeedBack(PersistentSimocVar simocVariableStorage)
        {
            base.SendFeedBack(simocVariableStorage);

            simocVariableStorage.LastErrorValue = lastErrorIntenal;


            // Proportional Term
            simocVariableStorage.GenericDouble2 = 
                K * currentErrorIntenal;

            // Derivative Approx
            simocVariableStorage.GenericDouble4 =
                (Td / (Td + N * stimSrv.DACPollingPeriodSec)) * (simocVariableStorage.GenericDouble4 - K * N * (currentErrorIntenal - simocVariableStorage.LastErrorValue));

            // PI feedback signal
            simocVariableStorage.GenericDouble1 = simocVariableStorage.GenericDouble2 + simocVariableStorage.GenericDouble3 + simocVariableStorage.GenericDouble4;

            // Tustin's Integral approximation w/ anti-windup
            if (simocVariableStorage.GenericDouble1 != 1 && simocVariableStorage.GenericDouble1 != -1)
                simocVariableStorage.GenericDouble3 += (K * stimSrv.DACPollingPeriodSec)/Ti * currentErrorIntenal;
                
            // Set upper and lower bounds
            if (simocVariableStorage.GenericDouble1 < -1)
                simocVariableStorage.GenericDouble1 = -1;
            if (simocVariableStorage.GenericDouble1 > 1)
                simocVariableStorage.GenericDouble1 = 1;


            double stimPowerVolts;
            double stimFreqHz;
            ushort channel;
            if (simocVariableStorage.GenericDouble1 > 0)
            {
                // Get the pulse width (msec)
                stimPulseWidthMSec = maxStimPulseWidthMSec * simocVariableStorage.GenericDouble1;
                pulseWidthSamples = (ulong)(stimSrv.sampleFrequencyHz * stimPulseWidthMSec / 1000);

                // Stim current (volts ~ amps)
                stimPowerVolts = maxStimPowerVolts * simocVariableStorage.GenericDouble1;

                // Get stim frequency
                stimFreqHz = maxStimFreq * simocVariableStorage.GenericDouble1 - 10 * (simocVariableStorage.GenericDouble1 - 1);

                // Blue channel
                channel = 0;
            }
            else
            {
                // Get the pulse width (msec)
                stimPulseWidthMSec = maxStimPulseWidthMSec * (0-simocVariableStorage.GenericDouble1);
                pulseWidthSamples = (ulong)(stimSrv.sampleFrequencyHz * stimPulseWidthMSec / 1000);

                // Stim current (volts ~ amps)
                stimPowerVolts = maxStimPowerVolts * (0 - simocVariableStorage.GenericDouble1);

                // Get stim frequency
                stimFreqHz = 0; // maxStimFreq * (0 - simocVariableStorage.GenericDouble1) - 10 * (-1 - simocVariableStorage.GenericDouble1);

                // yellow channel
                channel = 1;
            }

            // set the currentFeedback array
            currentFeedbackSignals = new double[numberOutStreams];
            currentFeedbackSignals[0] = simocVariableStorage.GenericDouble1;
            currentFeedbackSignals[1] = simocVariableStorage.GenericDouble2;
            currentFeedbackSignals[2] = simocVariableStorage.GenericDouble3;
            currentFeedbackSignals[3] = simocVariableStorage.GenericDouble4;
            currentFeedbackSignals[4] = stimFreqHz;
            currentFeedbackSignals[5] = stimPulseWidthMSec;
            currentFeedbackSignals[6] = stimPowerVolts;
            currentFeedbackSignals[7] = channel;

            // Create the output buffer
            List<AuxOutEvent> toAppendAux = new List<AuxOutEvent>();
            List<DigitalOutEvent> toAppendDig = new List<DigitalOutEvent>();
            ulong isi = 0;
            if (channel == 0)
                isi = (ulong)(hardwareSampFreqHz / stimFreqHz);

            // Get the current buffer sample and make sure that we are going
            // to produce stimuli that are in the future
            if (simocVariableStorage.NextAuxEventSample < nextAvailableSample)
            {
                simocVariableStorage.NextAuxEventSample = nextAvailableSample;
            }

            // Make periodic stimulation
            while (simocVariableStorage.NextAuxEventSample <= (nextAvailableSample + (ulong)stimSrv.GetBuffSize()))
            {
                // Send a V_ctl = simocVariableStorage.GenericDouble1 volt pulse to channel 0 for c2 milliseconds.
                if (simocVariableStorage.GenericDouble1 != 0)
                {
                    // Raise LED control voltage
                    toAppendAux.Add(new AuxOutEvent((ulong)(simocVariableStorage.NextAuxEventSample + loadOffset), channel, stimPowerVolts));
                    // Encode stimulus
                    toAppendDig.Add(new DigitalOutEvent((ulong)(simocVariableStorage.NextAuxEventSample + loadOffset), (uint)(10000.0 * simocVariableStorage.GenericDouble1)));

                    // Drop control voltages
                    if (channel == 0)
                    {
                        toAppendAux.Add(new AuxOutEvent((ulong)(simocVariableStorage.NextAuxEventSample + loadOffset) + pulseWidthSamples, 1, offVoltage));
                        toAppendAux.Add(new AuxOutEvent((ulong)(simocVariableStorage.NextAuxEventSample + loadOffset) + pulseWidthSamples, 0, offVoltage));
                        toAppendDig.Add(new DigitalOutEvent((ulong)(simocVariableStorage.NextAuxEventSample + loadOffset) + pulseWidthSamples, 0));
                   
                        
                        simocVariableStorage.LastAuxEventSample = simocVariableStorage.NextAuxEventSample;
                        simocVariableStorage.NextAuxEventSample += isi;
                    }
                    else
                    {
                        simocVariableStorage.LastAuxEventSample = simocVariableStorage.NextAuxEventSample;
                        simocVariableStorage.NextAuxEventSample += (ulong)stimSrv.GetBuffSize();
                    }


                }
                else
                {
                    // Make sure LEDs are off
                    toAppendAux.Add(new AuxOutEvent((ulong)(simocVariableStorage.NextAuxEventSample), 0, offVoltage));
                    toAppendAux.Add(new AuxOutEvent((ulong)(simocVariableStorage.NextAuxEventSample), 1, offVoltage));
                    break;
                }
            }

            // Send to bit 0 of the digital output port
            SendAuxAnalogOutput(toAppendAux);
            SendAuxDigitalOutput(toAppendDig);

        }
    }
}
