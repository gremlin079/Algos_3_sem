namespace WinFormsApp1
{
    partial class Form1
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
            listCities = new ListBox();
            btnGetWeather = new Button();
            lblWeather = new Label();
            SuspendLayout();
            // 
            // listCities
            // 
            listCities.FormattingEnabled = true;
            listCities.Location = new Point(0, 0);
            listCities.Name = "listCities";
            listCities.Size = new Size(449, 452);
            listCities.TabIndex = 0;
            // 
            // btnGetWeather
            // 
            btnGetWeather.Location = new Point(512, 27);
            btnGetWeather.Name = "btnGetWeather";
            btnGetWeather.Size = new Size(231, 80);
            btnGetWeather.TabIndex = 1;
            btnGetWeather.Text = "Показать погоду";
            btnGetWeather.UseVisualStyleBackColor = true;
            btnGetWeather.Click += btnGetWeather_Click;
            // 
            // lblWeather
            // 
            lblWeather.BorderStyle = BorderStyle.FixedSingle;
            lblWeather.Font = new Font("Segoe UI", 9F);
            lblWeather.Location = new Point(455, 143);
            lblWeather.Name = "lblWeather";
            lblWeather.Size = new Size(333, 298);
            lblWeather.TabIndex = 2;
            lblWeather.Text = "Выберите город для получения погоды";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblWeather);
            Controls.Add(btnGetWeather);
            Controls.Add(listCities);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private ListBox listCities;
        private Button btnGetWeather;
        private Label lblWeather;
    }
}
