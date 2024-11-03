using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdig; 

namespace AirshowSchedules;


    public partial class ReadmeForm : Form
    {
        public ReadmeForm()
        {
            InitializeComponent();
            LoadReadme();
        }

        private void InitializeComponent()
        {
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(800, 450);
            this.webBrowser.TabIndex = 0;
            // 
            // ReadmeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.webBrowser);
            this.Name = "ReadmeForm";
            this.Text = "Readme";
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.WebBrowser webBrowser;

        private void LoadReadme()
        {
            string readmePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Readme.md");
            if (File.Exists(readmePath))
            {
                string markdown = File.ReadAllText(readmePath);
                string html = Markdown.ToHtml(markdown);
                webBrowser.DocumentText = html;
            }
            else
            {
                MessageBox.Show("Readme.md file not found.");
            }
        }
    }