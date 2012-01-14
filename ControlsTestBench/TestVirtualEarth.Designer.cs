namespace AD.Windows.Forms.Controls
{
    partial class TestVirtualEarth
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
            this.button1 = new System.Windows.Forms.Button();
            this.virtualEarth1 = new AD.Windows.Forms.Controls.VirtualEarthMap();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 529);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Directions";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // virtualEarth1
            // 
            this.virtualEarth1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.virtualEarth1.CausesValidation = false;
            this.virtualEarth1.DashboardStyle = AD.Windows.Forms.Controls.VEDashboardStyle.Tiny;
            this.virtualEarth1.DisambiguationMode = AD.Windows.Forms.Controls.VEDisambiguation.Ignore;
            this.virtualEarth1.Location = new System.Drawing.Point(12, 12);
            this.virtualEarth1.Name = "virtualEarth1";
            this.virtualEarth1.Size = new System.Drawing.Size(760, 496);
            this.virtualEarth1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(93, 529);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Locations";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // TestVirtualEarth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 564);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.virtualEarth1);
            this.Name = "TestVirtualEarth";
            this.Text = "Virtual Earth Map Test";
            this.Load += new System.EventHandler(this.TestVirtualEarth_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private AD.Windows.Forms.Controls.VirtualEarthMap virtualEarth1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

