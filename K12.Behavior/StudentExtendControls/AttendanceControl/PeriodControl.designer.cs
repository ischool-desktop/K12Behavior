﻿namespace K12.Behavior.StudentExtendControls
{
    partial class PeriodControl
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該公開 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.label = new DevComponents.DotNetBar.LabelX();
            this.textBox = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.Dock = System.Windows.Forms.DockStyle.Top;
            this.label.Location = new System.Drawing.Point(0, 0);
            this.label.Margin = new System.Windows.Forms.Padding(0);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(60, 33);
            this.label.TabIndex = 0;
            this.label.Text = "labelX1";
            this.label.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // textBox
            // 
            // 
            // 
            // 
            this.textBox.Border.Class = "TextBoxBorder";
            this.textBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox.Location = new System.Drawing.Point(0, 32);
            this.textBox.Margin = new System.Windows.Forms.Padding(0);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(60, 25);
            this.textBox.TabIndex = 1;
            // 
            // PeriodControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.label);
            this.Font = new System.Drawing.Font("微軟正黑體", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PeriodControl";
            this.Size = new System.Drawing.Size(60, 57);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX label;
        private DevComponents.DotNetBar.Controls.TextBoxX textBox;
    }
}
