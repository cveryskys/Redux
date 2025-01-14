using System.Windows;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;

namespace ReduxExecutorUI {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            // Enable dragging the window
            this.MouseDown += (sender, e) => {
                if (e.ChangedButton == System.Windows.Input.MouseButton.Left) {
                    this.DragMove();
                }
            };
        }

        // Minimize Button Click Event
        private void Minimize_Click(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Minimized;
        }

        // Close Button Click Event
        private void Close_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        // Inject Button Click Event
        private void Inject_Click(object sender, RoutedEventArgs e) {
            string dllPath = "sample.dll";

            if (!File.Exists(dllPath)) {
                MessageBox.Show("DLL not found. Please ensure sample.dll is in the application directory.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Reference the Injector class correctly
            bool result = Injector.Inject("RobloxPlayerBeta", dllPath);

            if (result) {
                MessageBox.Show("Injection successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            } else {
                MessageBox.Show("Injection failed. Make sure Roblox is running.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Add Script Button Click Event
        private void AddScript_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog {
                Filter = "Lua Scripts (*.lua)|*.lua"
            };

            if (openFileDialog.ShowDialog() == true) {
                string scriptName = Path.GetFileName(openFileDialog.FileName);
                ScriptList.Items.Add(scriptName);
                ScriptEditor.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        // Execute Button Click Event
        private void Execute_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrWhiteSpace(ScriptEditor.Text)) {
                MessageBox.Show("No script loaded. Please add a script first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show("Executing script: " + ScriptEditor.Text);
        }
    }
}
