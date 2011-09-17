﻿namespace NeuroRighter.FileWriting
{
    partial class RecordingSetup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.checkBox_RecordRaw = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBox_RecordSpikeFilt = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button_MakeRawSelections = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBox_RecordLFP = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.checkBox_RecordEEG = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.checkBox_RecordMUA = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox_RecordStim = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.checkBox_RecordAuxAnalog = new System.Windows.Forms.CheckBox();
            this.checkBox_RecordAuxDig = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.checkBox_RecordSpikes = new System.Windows.Forms.CheckBox();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.persistWindowComponent_ForRecSet = new Mowog.PersistWindowComponent(this.components);
            this.checkBox_RecordSALPA = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBox_RecordRaw
            // 
            this.checkBox_RecordRaw.AutoSize = true;
            this.checkBox_RecordRaw.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.checkBox_RecordRaw.Location = new System.Drawing.Point(175, 76);
            this.checkBox_RecordRaw.Name = "checkBox_RecordRaw";
            this.checkBox_RecordRaw.Size = new System.Drawing.Size(15, 14);
            this.checkBox_RecordRaw.TabIndex = 0;
            this.checkBox_RecordRaw.UseVisualStyleBackColor = true;
            this.checkBox_RecordRaw.CheckedChanged += new System.EventHandler(this.checkBox_RecordRaw_CheckedChanged);
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Location = new System.Drawing.Point(20, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(180, 2);
            this.label5.TabIndex = 76;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(145, 15);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(60, 16);
            this.label15.TabIndex = 74;
            this.label15.Text = "Record?";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(20, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 16);
            this.label8.TabIndex = 73;
            this.label8.Text = "Data Stream";
            // 
            // checkBox_RecordSpikeFilt
            // 
            this.checkBox_RecordSpikeFilt.AutoSize = true;
            this.checkBox_RecordSpikeFilt.Enabled = false;
            this.checkBox_RecordSpikeFilt.ForeColor = System.Drawing.Color.Yellow;
            this.checkBox_RecordSpikeFilt.Location = new System.Drawing.Point(175, 128);
            this.checkBox_RecordSpikeFilt.Name = "checkBox_RecordSpikeFilt";
            this.checkBox_RecordSpikeFilt.Size = new System.Drawing.Size(15, 14);
            this.checkBox_RecordSpikeFilt.TabIndex = 77;
            this.checkBox_RecordSpikeFilt.UseVisualStyleBackColor = true;
            this.checkBox_RecordSpikeFilt.CheckedChanged += new System.EventHandler(this.checkBox_RecordSpikeFilt_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 79;
            this.label1.Text = "Raw electrode data";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 80;
            this.label3.Text = "Spike filter";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(44, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 81;
            this.label4.Text = "SALPA";
            // 
            // button_MakeRawSelections
            // 
            this.button_MakeRawSelections.Location = new System.Drawing.Point(115, 345);
            this.button_MakeRawSelections.Name = "button_MakeRawSelections";
            this.button_MakeRawSelections.Size = new System.Drawing.Size(79, 23);
            this.button_MakeRawSelections.TabIndex = 83;
            this.button_MakeRawSelections.Text = "Enter";
            this.button_MakeRawSelections.UseVisualStyleBackColor = true;
            this.button_MakeRawSelections.Click += new System.EventHandler(this.button_MakeRawSelections_Click);
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(30, 97);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(2, 110);
            this.label6.TabIndex = 84;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Location = new System.Drawing.Point(30, 110);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(10, 2);
            this.label9.TabIndex = 86;
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(30, 134);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(10, 2);
            this.label10.TabIndex = 87;
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Location = new System.Drawing.Point(30, 158);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(10, 2);
            this.label11.TabIndex = 88;
            // 
            // label12
            // 
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Location = new System.Drawing.Point(30, 182);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(10, 2);
            this.label12.TabIndex = 89;
            // 
            // label13
            // 
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Location = new System.Drawing.Point(30, 206);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(10, 2);
            this.label13.TabIndex = 90;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(44, 153);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 92;
            this.label7.Text = "LFP";
            // 
            // checkBox_RecordLFP
            // 
            this.checkBox_RecordLFP.AutoSize = true;
            this.checkBox_RecordLFP.ForeColor = System.Drawing.Color.Yellow;
            this.checkBox_RecordLFP.Location = new System.Drawing.Point(175, 152);
            this.checkBox_RecordLFP.Name = "checkBox_RecordLFP";
            this.checkBox_RecordLFP.Size = new System.Drawing.Size(15, 14);
            this.checkBox_RecordLFP.TabIndex = 91;
            this.checkBox_RecordLFP.UseVisualStyleBackColor = true;
            this.checkBox_RecordLFP.CheckedChanged += new System.EventHandler(this.checkBox_RecordLFP_CheckedChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(44, 177);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(29, 13);
            this.label14.TabIndex = 94;
            this.label14.Text = "EEG";
            // 
            // checkBox_RecordEEG
            // 
            this.checkBox_RecordEEG.AutoSize = true;
            this.checkBox_RecordEEG.ForeColor = System.Drawing.Color.Yellow;
            this.checkBox_RecordEEG.Location = new System.Drawing.Point(175, 176);
            this.checkBox_RecordEEG.Name = "checkBox_RecordEEG";
            this.checkBox_RecordEEG.Size = new System.Drawing.Size(15, 14);
            this.checkBox_RecordEEG.TabIndex = 93;
            this.checkBox_RecordEEG.UseVisualStyleBackColor = true;
            this.checkBox_RecordEEG.CheckedChanged += new System.EventHandler(this.checkBox_RecordEEG_CheckedChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(44, 201);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(31, 13);
            this.label16.TabIndex = 96;
            this.label16.Text = "MUA";
            // 
            // checkBox_RecordMUA
            // 
            this.checkBox_RecordMUA.AutoSize = true;
            this.checkBox_RecordMUA.Enabled = false;
            this.checkBox_RecordMUA.ForeColor = System.Drawing.Color.Yellow;
            this.checkBox_RecordMUA.Location = new System.Drawing.Point(175, 200);
            this.checkBox_RecordMUA.Name = "checkBox_RecordMUA";
            this.checkBox_RecordMUA.Size = new System.Drawing.Size(15, 14);
            this.checkBox_RecordMUA.TabIndex = 95;
            this.checkBox_RecordMUA.UseVisualStyleBackColor = true;
            this.checkBox_RecordMUA.CheckedChanged += new System.EventHandler(this.checkBox_RecordMUA_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 228);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 98;
            this.label2.Text = "Stimulation data";
            // 
            // checkBox_RecordStim
            // 
            this.checkBox_RecordStim.AutoSize = true;
            this.checkBox_RecordStim.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.checkBox_RecordStim.Location = new System.Drawing.Point(175, 227);
            this.checkBox_RecordStim.Name = "checkBox_RecordStim";
            this.checkBox_RecordStim.Size = new System.Drawing.Size(15, 14);
            this.checkBox_RecordStim.TabIndex = 97;
            this.checkBox_RecordStim.UseVisualStyleBackColor = true;
            this.checkBox_RecordStim.CheckedChanged += new System.EventHandler(this.checkBox_RecordStim_CheckedChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(21, 255);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(69, 13);
            this.label17.TabIndex = 100;
            this.label17.Text = "Auxiliary data";
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Location = new System.Drawing.Point(31, 289);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(10, 2);
            this.label22.TabIndex = 102;
            // 
            // label23
            // 
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Location = new System.Drawing.Point(31, 276);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(2, 37);
            this.label23.TabIndex = 101;
            // 
            // label21
            // 
            this.label21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label21.Location = new System.Drawing.Point(31, 313);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(10, 2);
            this.label21.TabIndex = 103;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(45, 307);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(40, 13);
            this.label18.TabIndex = 105;
            this.label18.Text = "Analog";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(45, 283);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(39, 13);
            this.label19.TabIndex = 104;
            this.label19.Text = "Digital ";
            // 
            // checkBox_RecordAuxAnalog
            // 
            this.checkBox_RecordAuxAnalog.AutoSize = true;
            this.checkBox_RecordAuxAnalog.Enabled = false;
            this.checkBox_RecordAuxAnalog.ForeColor = System.Drawing.Color.Yellow;
            this.checkBox_RecordAuxAnalog.Location = new System.Drawing.Point(175, 306);
            this.checkBox_RecordAuxAnalog.Name = "checkBox_RecordAuxAnalog";
            this.checkBox_RecordAuxAnalog.Size = new System.Drawing.Size(15, 14);
            this.checkBox_RecordAuxAnalog.TabIndex = 107;
            this.checkBox_RecordAuxAnalog.UseVisualStyleBackColor = true;
            // 
            // checkBox_RecordAuxDig
            // 
            this.checkBox_RecordAuxDig.AutoSize = true;
            this.checkBox_RecordAuxDig.Enabled = false;
            this.checkBox_RecordAuxDig.ForeColor = System.Drawing.Color.Yellow;
            this.checkBox_RecordAuxDig.Location = new System.Drawing.Point(175, 282);
            this.checkBox_RecordAuxDig.Name = "checkBox_RecordAuxDig";
            this.checkBox_RecordAuxDig.Size = new System.Drawing.Size(15, 14);
            this.checkBox_RecordAuxDig.TabIndex = 106;
            this.checkBox_RecordAuxDig.UseVisualStyleBackColor = true;
            this.checkBox_RecordAuxDig.CheckedChanged += new System.EventHandler(this.checkBox_RecordAuxDig_CheckedChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(20, 51);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(58, 13);
            this.label20.TabIndex = 109;
            this.label20.Text = "Spike data";
            // 
            // checkBox_RecordSpikes
            // 
            this.checkBox_RecordSpikes.AutoSize = true;
            this.checkBox_RecordSpikes.Checked = true;
            this.checkBox_RecordSpikes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_RecordSpikes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.checkBox_RecordSpikes.Location = new System.Drawing.Point(175, 48);
            this.checkBox_RecordSpikes.Name = "checkBox_RecordSpikes";
            this.checkBox_RecordSpikes.Size = new System.Drawing.Size(15, 14);
            this.checkBox_RecordSpikes.TabIndex = 108;
            this.checkBox_RecordSpikes.UseVisualStyleBackColor = true;
            this.checkBox_RecordSpikes.CheckedChanged += new System.EventHandler(this.checkBox_RecordSpikes_CheckedChanged);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(26, 345);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(79, 23);
            this.button_Cancel.TabIndex = 110;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // persistWindowComponent_ForRecSet
            // 
            this.persistWindowComponent_ForRecSet.Form = this;
            this.persistWindowComponent_ForRecSet.XMLFilePath = global::NeuroRighter.Properties.Settings.Default.persistWindowPath;
            // 
            // checkBox_RecordSALPA
            // 
            this.checkBox_RecordSALPA.AutoSize = true;
            this.checkBox_RecordSALPA.Enabled = false;
            this.checkBox_RecordSALPA.ForeColor = System.Drawing.Color.Yellow;
            this.checkBox_RecordSALPA.Location = new System.Drawing.Point(175, 104);
            this.checkBox_RecordSALPA.Name = "checkBox_RecordSALPA";
            this.checkBox_RecordSALPA.Size = new System.Drawing.Size(15, 14);
            this.checkBox_RecordSALPA.TabIndex = 111;
            this.checkBox_RecordSALPA.UseVisualStyleBackColor = true;
            // 
            // RecordingSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(218, 380);
            this.ControlBox = false;
            this.Controls.Add(this.checkBox_RecordSALPA);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.checkBox_RecordSpikes);
            this.Controls.Add(this.checkBox_RecordAuxAnalog);
            this.Controls.Add(this.checkBox_RecordAuxDig);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBox_RecordStim);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.checkBox_RecordMUA);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.checkBox_RecordEEG);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.checkBox_RecordLFP);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button_MakeRawSelections);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox_RecordSpikeFilt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.checkBox_RecordRaw);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "RecordingSetup";
            this.Text = "Recording Stream Setup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_RecordRaw;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkBox_RecordSpikeFilt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_MakeRawSelections;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBox_RecordLFP;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox checkBox_RecordEEG;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.CheckBox checkBox_RecordMUA;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox_RecordStim;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.CheckBox checkBox_RecordAuxAnalog;
        private System.Windows.Forms.CheckBox checkBox_RecordAuxDig;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.CheckBox checkBox_RecordSpikes;
        private System.Windows.Forms.Button button_Cancel;
        private Mowog.PersistWindowComponent persistWindowComponent_ForRecSet;
        private System.Windows.Forms.CheckBox checkBox_RecordSALPA;
    }
}