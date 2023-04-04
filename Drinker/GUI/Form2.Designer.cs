namespace Drinker.GUI
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.button1 = new System.Windows.Forms.Button();
            this.UPS_statusText = new System.Windows.Forms.Label();
            this.charHP_statusText = new System.Windows.Forms.Label();
            this.charMP_statusText = new System.Windows.Forms.Label();
            this.StartStop_button = new System.Windows.Forms.Button();
            this.showOverlay_checkBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.button1.Location = new System.Drawing.Point(162, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 75);
            this.button1.TabIndex = 0;
            this.button1.Text = "Flask Setup";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // UPS_statusText
            // 
            this.UPS_statusText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.UPS_statusText.AutoSize = true;
            this.UPS_statusText.BackColor = System.Drawing.Color.Silver;
            this.UPS_statusText.Location = new System.Drawing.Point(12, 116);
            this.UPS_statusText.MinimumSize = new System.Drawing.Size(50, 0);
            this.UPS_statusText.Name = "UPS_statusText";
            this.UPS_statusText.Size = new System.Drawing.Size(53, 13);
            this.UPS_statusText.TabIndex = 1;
            this.UPS_statusText.Text = "UPS: 999";
            // 
            // charHP_statusText
            // 
            this.charHP_statusText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.charHP_statusText.AutoSize = true;
            this.charHP_statusText.BackColor = System.Drawing.Color.Silver;
            this.charHP_statusText.Location = new System.Drawing.Point(72, 116);
            this.charHP_statusText.MinimumSize = new System.Drawing.Size(80, 0);
            this.charHP_statusText.Name = "charHP_statusText";
            this.charHP_statusText.Size = new System.Drawing.Size(81, 13);
            this.charHP_statusText.TabIndex = 3;
            this.charHP_statusText.Text = "HP: 9999/9999";
            // 
            // charMP_statusText
            // 
            this.charMP_statusText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.charMP_statusText.AutoSize = true;
            this.charMP_statusText.BackColor = System.Drawing.Color.Silver;
            this.charMP_statusText.Location = new System.Drawing.Point(159, 116);
            this.charMP_statusText.MinimumSize = new System.Drawing.Size(80, 0);
            this.charMP_statusText.Name = "charMP_statusText";
            this.charMP_statusText.Size = new System.Drawing.Size(82, 13);
            this.charMP_statusText.TabIndex = 4;
            this.charMP_statusText.Text = "MP: 9999/9999";
            // 
            // StartStop_button
            // 
            this.StartStop_button.BackColor = System.Drawing.Color.IndianRed;
            this.StartStop_button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.StartStop_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.StartStop_button.Location = new System.Drawing.Point(15, 12);
            this.StartStop_button.Name = "StartStop_button";
            this.StartStop_button.Size = new System.Drawing.Size(138, 75);
            this.StartStop_button.TabIndex = 5;
            this.StartStop_button.Text = "Start (F4)";
            this.StartStop_button.UseVisualStyleBackColor = false;
            this.StartStop_button.Click += new System.EventHandler(this.StartStop_button_Click);
            // 
            // showOverlay_checkBox
            // 
            this.showOverlay_checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.showOverlay_checkBox.AutoSize = true;
            this.showOverlay_checkBox.Location = new System.Drawing.Point(132, 93);
            this.showOverlay_checkBox.Name = "showOverlay_checkBox";
            this.showOverlay_checkBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.showOverlay_checkBox.Size = new System.Drawing.Size(105, 17);
            this.showOverlay_checkBox.TabIndex = 6;
            this.showOverlay_checkBox.Text = "Оверлей в игре";
            this.showOverlay_checkBox.UseVisualStyleBackColor = true;
            this.showOverlay_checkBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 138);
            this.Controls.Add(this.showOverlay_checkBox);
            this.Controls.Add(this.StartStop_button);
            this.Controls.Add(this.charMP_statusText);
            this.Controls.Add(this.charHP_statusText);
            this.Controls.Add(this.UPS_statusText);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(265, 177);
            this.MinimumSize = new System.Drawing.Size(265, 177);
            this.Name = "Form2";
            this.Text = "Drinker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label UPS_statusText;
        private System.Windows.Forms.Label charHP_statusText;
        private System.Windows.Forms.Label charMP_statusText;
        private System.Windows.Forms.Button StartStop_button;
        private System.Windows.Forms.CheckBox showOverlay_checkBox;
    }
}