using System;
using System.Windows.Forms;
using CryptoCourse.Core.Algorithms.Modern.SecureWrappers;

namespace CryptoCourse.WinFormsUI.Controls
{
    public class AesPanel : Panel
    {
        public AesPanel()
        {
            this.Dock = DockStyle.Fill;
            // A simple layout for modern symmetric encryption
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 5, Padding = new Padding(15) };

            var plaintextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
            var passwordBox = new TextBox { Dock = DockStyle.Fill, UseSystemPasswordChar = true }; // Hide password
            var resultTextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, BackColor = System.Drawing.Color.White };
            var encryptButton = new Button { Text = "تشفير (AES)", Width = 120 };
            var decryptButton = new Button { Text = "فك التشفير (AES)", Width = 120 };
            var buttonPanel = new FlowLayoutPanel();
            buttonPanel.Controls.Add(encryptButton);
            buttonPanel.Controls.Add(decryptButton);

            layout.Controls.Add(new Label { Text = "النص:", AutoSize = true }, 0, 0);
            layout.Controls.Add(plaintextBox, 0, 1);
            layout.Controls.Add(new Label { Text = "كلمة المرور:", AutoSize = true }, 0, 2);
            layout.Controls.Add(passwordBox, 0, 3);
            layout.Controls.Add(buttonPanel, 0, 4);
            layout.Controls.Add(resultTextBox, 0, 5); // Add result box

            this.Controls.Add(layout);

            encryptButton.Click += (s, e) => {
                try { resultTextBox.Text = AesWrapper.Encrypt(plaintextBox.Text, passwordBox.Text); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Encryption Error"); }
            };
            decryptButton.Click += (s, e) => {
                try { resultTextBox.Text = AesWrapper.Decrypt(plaintextBox.Text, passwordBox.Text); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Decryption Error"); }
            };
        }
    }
}