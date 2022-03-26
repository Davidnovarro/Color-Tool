
namespace ColorTool
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.CaptureButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.FilterComboBox_1 = new System.Windows.Forms.ComboBox();
            this.FilterComboBox_0 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // CaptureButton
            // 
            this.CaptureButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CaptureButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CaptureButton.Location = new System.Drawing.Point(670, 394);
            this.CaptureButton.Name = "CaptureButton";
            this.CaptureButton.Size = new System.Drawing.Size(118, 44);
            this.CaptureButton.TabIndex = 0;
            this.CaptureButton.Text = "Capture";
            this.CaptureButton.UseVisualStyleBackColor = true;
            this.CaptureButton.Click += new System.EventHandler(this.OnCaptureClick);
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SaveButton.Location = new System.Drawing.Point(546, 394);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(118, 44);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.OnSaveClick);
            // 
            // FilterComboBox_1
            // 
            this.FilterComboBox_1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FilterComboBox_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FilterComboBox_1.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FilterComboBox_1.FormattingEnabled = true;
            this.FilterComboBox_1.Location = new System.Drawing.Point(249, 407);
            this.FilterComboBox_1.Name = "FilterComboBox_1";
            this.FilterComboBox_1.Size = new System.Drawing.Size(231, 31);
            this.FilterComboBox_1.TabIndex = 4;
            this.FilterComboBox_1.SelectedIndexChanged += new System.EventHandler(this.OnFilter1_Changed);
            // 
            // FilterComboBox_0
            // 
            this.FilterComboBox_0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FilterComboBox_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FilterComboBox_0.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FilterComboBox_0.FormattingEnabled = true;
            this.FilterComboBox_0.Location = new System.Drawing.Point(12, 407);
            this.FilterComboBox_0.Name = "FilterComboBox_0";
            this.FilterComboBox_0.Size = new System.Drawing.Size(231, 31);
            this.FilterComboBox_0.TabIndex = 5;
            this.FilterComboBox_0.SelectedIndexChanged += new System.EventHandler(this.OnFilter0_Changed);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.FilterComboBox_0);
            this.Controls.Add(this.FilterComboBox_1);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.CaptureButton);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Color Tool";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CaptureButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.ComboBox FilterComboBox_0;
        private System.Windows.Forms.ComboBox FilterComboBox_1;
    }
}

