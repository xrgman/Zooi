namespace WindowsFormsApplication1
{
    partial class OldTestData
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.label1 = new System.Windows.Forms.Label();
            this.oldTestsList = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.testDateLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.durationLabel = new System.Windows.Forms.Label();
            this.rpmChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.omslagTimeLabel = new System.Windows.Forms.Label();
            this.omslagPowerLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.rpmChart)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Old tests: ";
            // 
            // oldTestsList
            // 
            this.oldTestsList.FormattingEnabled = true;
            this.oldTestsList.Location = new System.Drawing.Point(17, 47);
            this.oldTestsList.Name = "oldTestsList";
            this.oldTestsList.Size = new System.Drawing.Size(191, 342);
            this.oldTestsList.TabIndex = 2;
            this.oldTestsList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.oldTestsList_MouseDoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(229, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Test: ";
            // 
            // testDateLabel
            // 
            this.testDateLabel.AutoSize = true;
            this.testDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testDateLabel.Location = new System.Drawing.Point(292, 9);
            this.testDateLabel.Name = "testDateLabel";
            this.testDateLabel.Size = new System.Drawing.Size(0, 25);
            this.testDateLabel.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(231, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "Duration:";
            // 
            // durationLabel
            // 
            this.durationLabel.AutoSize = true;
            this.durationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.durationLabel.Location = new System.Drawing.Point(314, 47);
            this.durationLabel.Name = "durationLabel";
            this.durationLabel.Size = new System.Drawing.Size(0, 20);
            this.durationLabel.TabIndex = 6;
            // 
            // rpmChart
            // 
            chartArea1.Name = "ChartArea1";
            this.rpmChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.rpmChart.Legends.Add(legend1);
            this.rpmChart.Location = new System.Drawing.Point(234, 84);
            this.rpmChart.Name = "rpmChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Legend = "Legend1";
            series1.Name = "rpm";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Legend = "Legend1";
            series2.Name = "Heart Rate (bpm)";
            this.rpmChart.Series.Add(series1);
            this.rpmChart.Series.Add(series2);
            this.rpmChart.Size = new System.Drawing.Size(467, 300);
            this.rpmChart.TabIndex = 7;
            this.rpmChart.Text = "chart1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(499, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 18);
            this.label4.TabIndex = 8;
            this.label4.Text = "Omslagpunt:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(510, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "Time:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(510, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "Power: ";
            // 
            // omslagTimeLabel
            // 
            this.omslagTimeLabel.AutoSize = true;
            this.omslagTimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.omslagTimeLabel.Location = new System.Drawing.Point(564, 36);
            this.omslagTimeLabel.Name = "omslagTimeLabel";
            this.omslagTimeLabel.Size = new System.Drawing.Size(0, 17);
            this.omslagTimeLabel.TabIndex = 11;
            // 
            // omslagPowerLabel
            // 
            this.omslagPowerLabel.AutoSize = true;
            this.omslagPowerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.omslagPowerLabel.Location = new System.Drawing.Point(564, 64);
            this.omslagPowerLabel.Name = "omslagPowerLabel";
            this.omslagPowerLabel.Size = new System.Drawing.Size(0, 17);
            this.omslagPowerLabel.TabIndex = 12;
            // 
            // OldTestData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 427);
            this.Controls.Add(this.omslagPowerLabel);
            this.Controls.Add(this.omslagTimeLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.rpmChart);
            this.Controls.Add(this.durationLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.testDateLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.oldTestsList);
            this.Controls.Add(this.label1);
            this.Name = "OldTestData";
            this.Text = "OldTestData";
            ((System.ComponentModel.ISupportInitialize)(this.rpmChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox oldTestsList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label testDateLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label durationLabel;
        private System.Windows.Forms.DataVisualization.Charting.Chart rpmChart;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label omslagTimeLabel;
        private System.Windows.Forms.Label omslagPowerLabel;
    }
}