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
            this.autoPayse_chekbox = new System.Windows.Forms.CheckBox();
            this.POEPath_lable = new System.Windows.Forms.Label();
            this.charES_statusText = new System.Windows.Forms.Label();
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
            this.UPS_statusText.Location = new System.Drawing.Point(12, 141);
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
            this.charHP_statusText.Location = new System.Drawing.Point(72, 141);
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
            this.charMP_statusText.Location = new System.Drawing.Point(159, 141);
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
            this.showOverlay_checkBox.Location = new System.Drawing.Point(132, 118);
            this.showOverlay_checkBox.Name = "showOverlay_checkBox";
            this.showOverlay_checkBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.showOverlay_checkBox.Size = new System.Drawing.Size(105, 17);
            this.showOverlay_checkBox.TabIndex = 6;
            this.showOverlay_checkBox.Text = "Оверлей в игре";
            this.showOverlay_checkBox.UseVisualStyleBackColor = true;
            this.showOverlay_checkBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // autoPayse_chekbox
            // 
            this.autoPayse_chekbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.autoPayse_chekbox.AutoSize = true;
            this.autoPayse_chekbox.Location = new System.Drawing.Point(128, 94);
            this.autoPayse_chekbox.Name = "autoPayse_chekbox";
            this.autoPayse_chekbox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.autoPayse_chekbox.Size = new System.Drawing.Size(109, 17);
            this.autoPayse_chekbox.TabIndex = 7;
            this.autoPayse_chekbox.Text = "Авто пауза в ХО";
            this.autoPayse_chekbox.UseVisualStyleBackColor = true;
            this.autoPayse_chekbox.CheckedChanged += new System.EventHandler(this.autoPayse_chekbox_CheckedChanged);
            // 
            // POEPath_lable
            // 
            this.POEPath_lable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.POEPath_lable.AutoSize = true;
            this.POEPath_lable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.POEPath_lable.ForeColor = System.Drawing.Color.RoyalBlue;
            this.POEPath_lable.Location = new System.Drawing.Point(15, 95);
            this.POEPath_lable.MaximumSize = new System.Drawing.Size(117, 13);
            this.POEPath_lable.MinimumSize = new System.Drawing.Size(117, 13);
            this.POEPath_lable.Name = "POEPath_lable";
            this.POEPath_lable.Size = new System.Drawing.Size(117, 13);
            this.POEPath_lable.TabIndex = 8;
            this.POEPath_lable.Text = "Указать путь к игре..";
            this.POEPath_lable.Click += new System.EventHandler(this.POEPath_lable_Click);
            // 
            // charES_statusText
            // 
            this.charES_statusText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.charES_statusText.AutoSize = true;
            this.charES_statusText.BackColor = System.Drawing.Color.Silver;
            this.charES_statusText.Location = new System.Drawing.Point(12, 122);
            this.charES_statusText.MinimumSize = new System.Drawing.Size(80, 0);
            this.charES_statusText.Name = "charES_statusText";
            this.charES_statusText.Size = new System.Drawing.Size(80, 13);
            this.charES_statusText.TabIndex = 9;
            this.charES_statusText.Text = "ES: 9999/9999";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 160);
            this.Controls.Add(this.charES_statusText);
            this.Controls.Add(this.POEPath_lable);
            this.Controls.Add(this.autoPayse_chekbox);
            this.Controls.Add(this.showOverlay_checkBox);
            this.Controls.Add(this.StartStop_button);
            this.Controls.Add(this.charMP_statusText);
            this.Controls.Add(this.charHP_statusText);
            this.Controls.Add(this.UPS_statusText);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(265, 199);
            this.MinimumSize = new System.Drawing.Size(265, 199);
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
        private System.Windows.Forms.CheckBox autoPayse_chekbox;
        private System.Windows.Forms.Label POEPath_lable;
        private System.Windows.Forms.Label charES_statusText;
    }
}