namespace QRtoCSVSystem
{
    partial class FormQRtoCSV
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
            txtQRCode = new TextBox();
            lblExplan = new Label();
            lblResult = new Label();
            SuspendLayout();
            // 
            // txtQRCode
            // 
            txtQRCode.Location = new Point(17, 78);
            txtQRCode.Margin = new Padding(5);
            txtQRCode.Name = "txtQRCode";
            txtQRCode.Size = new Size(501, 32);
            txtQRCode.TabIndex = 0;
            txtQRCode.KeyDown += txtQRCode_KeyDown;
            // 
            // lblExplan
            // 
            lblExplan.AutoSize = true;
            lblExplan.Location = new Point(17, 49);
            lblExplan.Name = "lblExplan";
            lblExplan.Size = new Size(241, 24);
            lblExplan.TabIndex = 1;
            lblExplan.Text = "QRコードを読み込んで下さい。";
            // 
            // lblResult
            // 
            lblResult.AutoSize = true;
            lblResult.Location = new Point(17, 138);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(48, 24);
            lblResult.TabIndex = 2;
            lblResult.Text = "結果";
            // 
            // FormQRtoCSV
            // 
            AutoScaleDimensions = new SizeF(12F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(534, 211);
            Controls.Add(lblResult);
            Controls.Add(lblExplan);
            Controls.Add(txtQRCode);
            Font = new Font("Meiryo UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Margin = new Padding(5);
            MaximizeBox = false;
            MaximumSize = new Size(550, 250);
            MinimizeBox = false;
            MinimumSize = new Size(550, 250);
            Name = "FormQRtoCSV";
            ShowIcon = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "QR読込";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtQRCode;
        private Label lblExplan;
        private Label lblResult;
    }
}
