using System.Drawing;
using System.Windows.Forms;
using CryptoCourse.WinFormsUI.Controls;
using System;

namespace CryptoCourse.WinFormsUI
{
    /// <summary>
    /// The main window for the Crypto-Toolkit application.
    /// All UI elements within this form are generated programmatically.
    /// </summary>
    public partial class MainForm : Form
    {
        // This is the constructor. It runs once when the form is created.
        // We will define all our window's properties and controls here.
        // Define the main UI layout components as private fields
        private TableLayoutPanel _mainLayout;
        private ComboBox _algorithmSelector;
        private Panel _contentPanel; // This panel will host the specific cipher's UI

        public MainForm()
        {
            // 1. Set basic window properties
            this.Text = "مجموعة أدوات التشفير - Crypto-Toolkit"; // The title bar text
            this.Width = 800;                                 // Window width in pixels
            this.Height = 600;                                // Window height in pixels
            this.StartPosition = FormStartPosition.CenterScreen; // Start the window in the center of the screen
            this.BackColor = Color.WhiteSmoke;                // Set a light gray background color
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));


            _mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill, // Make the panel fill the entire window
                Padding = new Padding(10),
                ColumnCount = 1,
                RowCount = 2
            };
            var topPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight // Arrange controls from left to right
            };

            var selectorLabel = new Label
            {
                Text = "اختر الخوارزمية:",
                AutoSize = true,
                Padding = new Padding(0, 6, 0, 0) // Align text vertically
            };

            _algorithmSelector = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList, // Prevents user from typing custom text
                Width = 250
            };

            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle, // Add a border to make it visible
                BackColor = Color.White
            };

            // Populate the ComboBox with the ciphers we will implement
            _algorithmSelector.Items.AddRange(new string[] {
        "Caesar Cipher",
        "Playfair Cipher",
        "Affine Cipher",
         "Hill Cipher",
         "Rail Fence Cipher",
          "Reverse Text", // <-- أضف هذا
         "Columnar Transposition", // <-- وهذا
         "Reverse Blocks", // <-- أضف هذا
          "AES (Secure)",      // <-- أضف هذا
          "RSA (Secure)"       // <-- وهذا
    });

  _algorithmSelector.SelectedIndexChanged += AlgorithmSelector_SelectedIndexChanged;


            // Row 0: Fixed height for the algorithm selector
            _mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            // Row 1: Takes up the remaining 100% of the space
            _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // Define the width of the single column to take all space
            _mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _mainLayout.Controls.Add(_contentPanel, 0, 1);
            // Add the main layout panel to the form's controls collection
            this.Controls.Add(_mainLayout);
            topPanel.Controls.Add(selectorLabel);
            topPanel.Controls.Add(_algorithmSelector);
            _mainLayout.Controls.Add(topPanel, 0, 0);
        

    }

        private void AlgorithmSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            _contentPanel.Controls.Clear();
            if (_algorithmSelector.SelectedItem == null) return;
            string selectedCipher = _algorithmSelector.SelectedItem.ToString();

            switch (selectedCipher)
            {
                case "Caesar Cipher":
                    var caesarPanel = new CaesarPanel();
                    _contentPanel.Controls.Add(caesarPanel);
                    break;

                // --- ADD THIS NEW CASE ---
                case "Playfair Cipher":
                    var playfairPanel = new PlayfairPanel();
                    _contentPanel.Controls.Add(playfairPanel);
                    break;
                // -------------------------
                //  ---
                case "Affine Cipher":
                    var affinePanel = new AffinePanel();
                    _contentPanel.Controls.Add(affinePanel);
                    break;

                // --- ADD THIS NEW CASE ---
                case "Hill Cipher":
                    var hillPanel = new HillPanel();
                    _contentPanel.Controls.Add(hillPanel);
                    break;
                // -------------------------
                case "Rail Fence Cipher":
                    var railFencePanel = new RailFencePanel();
                    _contentPanel.Controls.Add(railFencePanel);
                    break;
                // -------------------------
                case "Reverse Text":
                    var reversePanel = new ReverseTextPanel();
                    _contentPanel.Controls.Add(reversePanel);
                    break;

                case "Columnar Transposition":
                    var columnarPanel = new ColumnarTranspositionPanel();
                    _contentPanel.Controls.Add(columnarPanel);
                    break;

                case "Reverse Blocks":
                    var reverseBlocksPanel = new ReverseBlocksPanel();
                    _contentPanel.Controls.Add(reverseBlocksPanel);
                    break;

                case "AES (Secure)":
                    _contentPanel.Controls.Add(new AesPanel());
                    break;

                case "RSA (Secure)":
                    _contentPanel.Controls.Add(new RsaPanel());
                    break;

                default:
                    _contentPanel.Controls.Add(new Label { Text = $"'{selectedCipher}' لم يتم تنفيذه بعد.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter });
                    break;
            }
        }
    }
}