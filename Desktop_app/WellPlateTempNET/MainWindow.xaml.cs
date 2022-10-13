using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Windows.Markup;
using System.Reflection.Emit;
using System.Threading;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Windows.Threading;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;

namespace WellPlateTempNET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PlateData Plate;
        private serialReader sr;
        private bool Rec = false;
        private ColorHeatMap ColorHeatMap = new ColorHeatMap();

        public MainWindow()
        {
            InitializeComponent();
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                combo_COMports.Items.Add(port);
            }
            Plate = new PlateData();
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (combo_COMports.SelectedIndex != -1) {
                string line = sr.getCurrentLine();
                //Console.WriteLine(line);
                try
                {
                    dynamic json = JObject.Parse(line);
                    lbl_cur_a.Content = String.Format("{0:0.00}", json.A);
                    lbl_cur_b.Content = String.Format("{0:0.00}", json.B);
                    lbl_cur_c.Content = String.Format("{0:0.00}", json.C);
                    lbl_cur_d.Content = String.Format("{0:0.00}", json.D);
                    lbl_cur_e.Content = String.Format("{0:0.00}", json.E);
                    lbl_cur_f.Content = String.Format("{0:0.00}", json.F);
                    lbl_cur_g.Content = String.Format("{0:0.00}", json.G);
                    lbl_cur_h.Content = String.Format("{0:0.00}", json.H);
                    if (Rec)
                    {
                        RecColValues(getSelRbtn(), json);
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (combo_COMports.SelectedItem == null)
            {
                MessageBox.Show("Please select a COM Port", "Error");
            }
            else
            {
                int seconds = Convert.ToInt32(txt_seconds.Text);
                coverScreen(seconds);
                Plate.printAllData();
            }
        }

        private void SetAllRbtn(bool b)
        {
            rbtn_1.IsEnabled = b;
            rbtn_2.IsEnabled = b;
            rbtn_3.IsEnabled = b;
            rbtn_4.IsEnabled = b;
            rbtn_5.IsEnabled = b;
            rbtn_6.IsEnabled = b;
            rbtn_7.IsEnabled = b;
            rbtn_8.IsEnabled = b;
            rbtn_9.IsEnabled = b;
            rbtn_10.IsEnabled = b;
            rbtn_11.IsEnabled = b;
            rbtn_12.IsEnabled = b;
        }

        private int getSelRbtn()
        {
            int rbtn_selected = 0;
            if ((bool)rbtn_1.IsChecked) { rbtn_selected = 1; }
            if ((bool)rbtn_2.IsChecked) { rbtn_selected = 2; }
            if ((bool)rbtn_3.IsChecked) { rbtn_selected = 3; }
            if ((bool)rbtn_4.IsChecked) { rbtn_selected = 4; }
            if ((bool)rbtn_5.IsChecked) { rbtn_selected = 5; }
            if ((bool)rbtn_6.IsChecked) { rbtn_selected = 6; }
            if ((bool)rbtn_7.IsChecked) { rbtn_selected = 7; }
            if ((bool)rbtn_8.IsChecked) { rbtn_selected = 8; }
            if ((bool)rbtn_9.IsChecked) { rbtn_selected = 9; }
            if ((bool)rbtn_10.IsChecked) { rbtn_selected = 10; }
            if ((bool)rbtn_11.IsChecked) { rbtn_selected = 11; }
            if ((bool)rbtn_12.IsChecked) { rbtn_selected = 12; }
            return rbtn_selected;
        }

        private void coverScreen(int seconds)
        {
            Rec = true;
            SetAllRbtn(false);
            recBtn.IsEnabled = false;
            pauseBtn.IsEnabled = true;
            combo_COMports.IsEnabled = false;
            txt_seconds.IsEnabled = false;
            AutoClosingMessageBox.Show("Measuring Temperature. Do not move temperature probes", "Please wait", seconds*1000);
            Rec = false;
            SetAllRbtn(true);
            recBtn.IsEnabled = true;
            pauseBtn.IsEnabled = false;
            combo_COMports.IsEnabled = true;
            txt_seconds.IsEnabled = true;
            renderPlate();
        }

        private void RecColValues(int col, dynamic json)
        {
            double a_val = json.A;
            double b_val = json.B;
            double c_val = json.C;
            double d_val = json.D;
            double e_val = json.E;
            double f_val = json.F;
            double g_val = json.G;
            double h_val = json.H;

            switch (col)
            {
                case 1:
                    Plate.A1.TempValues.Add(a_val);
                    Plate.B1.TempValues.Add(b_val);
                    Plate.C1.TempValues.Add(c_val);
                    Plate.D1.TempValues.Add(d_val);
                    Plate.E1.TempValues.Add(e_val);
                    Plate.F1.TempValues.Add(f_val);
                    Plate.G1.TempValues.Add(g_val);
                    Plate.H1.TempValues.Add(h_val);
                    break;
                case 2:
                    Plate.A2.TempValues.Add(a_val);
                    Plate.B2.TempValues.Add(b_val);
                    Plate.C2.TempValues.Add(c_val);
                    Plate.D2.TempValues.Add(d_val);
                    Plate.E2.TempValues.Add(e_val);
                    Plate.F2.TempValues.Add(f_val);
                    Plate.G2.TempValues.Add(g_val);
                    Plate.H2.TempValues.Add(h_val);
                    break;
                case 3:
                    Plate.A3.TempValues.Add(a_val);
                    Plate.B3.TempValues.Add(b_val);
                    Plate.C3.TempValues.Add(c_val);
                    Plate.D3.TempValues.Add(d_val);
                    Plate.E3.TempValues.Add(e_val);
                    Plate.F3.TempValues.Add(f_val);
                    Plate.G3.TempValues.Add(g_val);
                    Plate.H3.TempValues.Add(h_val);
                    break;
                case 4:
                    Plate.A4.TempValues.Add(a_val);
                    Plate.B4.TempValues.Add(b_val);
                    Plate.C4.TempValues.Add(c_val);
                    Plate.D4.TempValues.Add(d_val);
                    Plate.E4.TempValues.Add(e_val);
                    Plate.F4.TempValues.Add(f_val);
                    Plate.G4.TempValues.Add(g_val);
                    Plate.H4.TempValues.Add(h_val);
                    break;
                case 5:
                    Plate.A5.TempValues.Add(a_val);
                    Plate.B5.TempValues.Add(b_val);
                    Plate.C5.TempValues.Add(c_val);
                    Plate.D5.TempValues.Add(d_val);
                    Plate.E5.TempValues.Add(e_val);
                    Plate.F5.TempValues.Add(f_val);
                    Plate.G5.TempValues.Add(g_val);
                    Plate.H5.TempValues.Add(h_val);
                    break;
                case 6:
                    Plate.A6.TempValues.Add(a_val);
                    Plate.B6.TempValues.Add(b_val);
                    Plate.C6.TempValues.Add(c_val);
                    Plate.D6.TempValues.Add(d_val);
                    Plate.E6.TempValues.Add(e_val);
                    Plate.F6.TempValues.Add(f_val);
                    Plate.G6.TempValues.Add(g_val);
                    Plate.H6.TempValues.Add(h_val);
                    break;
                case 7:
                    Plate.A7.TempValues.Add(a_val);
                    Plate.B7.TempValues.Add(b_val);
                    Plate.C7.TempValues.Add(c_val);
                    Plate.D7.TempValues.Add(d_val);
                    Plate.E7.TempValues.Add(e_val);
                    Plate.F7.TempValues.Add(f_val);
                    Plate.G7.TempValues.Add(g_val);
                    Plate.H7.TempValues.Add(h_val);
                    break;
                case 8:
                    Plate.A8.TempValues.Add(a_val);
                    Plate.B8.TempValues.Add(b_val);
                    Plate.C8.TempValues.Add(c_val);
                    Plate.D8.TempValues.Add(d_val);
                    Plate.E8.TempValues.Add(e_val);
                    Plate.F8.TempValues.Add(f_val);
                    Plate.G8.TempValues.Add(g_val);
                    Plate.H8.TempValues.Add(h_val);
                    break;
                case 9:
                    Plate.A9.TempValues.Add(a_val);
                    Plate.B9.TempValues.Add(b_val);
                    Plate.C9.TempValues.Add(c_val);
                    Plate.D9.TempValues.Add(d_val);
                    Plate.E9.TempValues.Add(e_val);
                    Plate.F9.TempValues.Add(f_val);
                    Plate.G9.TempValues.Add(g_val);
                    Plate.H9.TempValues.Add(h_val);
                    break;
                case 10:
                    Plate.A10.TempValues.Add(a_val);
                    Plate.B10.TempValues.Add(b_val);
                    Plate.C10.TempValues.Add(c_val);
                    Plate.D10.TempValues.Add(d_val);
                    Plate.E10.TempValues.Add(e_val);
                    Plate.F10.TempValues.Add(f_val);
                    Plate.G10.TempValues.Add(g_val);
                    Plate.H10.TempValues.Add(h_val);
                    break;
                case 11:
                    Plate.A11.TempValues.Add(a_val);
                    Plate.B11.TempValues.Add(b_val);
                    Plate.C11.TempValues.Add(c_val);
                    Plate.D11.TempValues.Add(d_val);
                    Plate.E11.TempValues.Add(e_val);
                    Plate.F11.TempValues.Add(f_val);
                    Plate.G11.TempValues.Add(g_val);
                    Plate.H11.TempValues.Add(h_val);
                    break;
                case 12:
                    Plate.A12.TempValues.Add(a_val);
                    Plate.B12.TempValues.Add(b_val);
                    Plate.C12.TempValues.Add(c_val);
                    Plate.D12.TempValues.Add(d_val);
                    Plate.E12.TempValues.Add(e_val);
                    Plate.F12.TempValues.Add(f_val);
                    Plate.G12.TempValues.Add(g_val);
                    Plate.H12.TempValues.Add(h_val);
                    break;
            }
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void pauseBtn_Click(object sender, RoutedEventArgs e)
        {
            recBtn.IsEnabled = true;
            pauseBtn.IsEnabled = false;
            combo_COMports.IsEnabled = true;
            txt_seconds.IsEnabled = true;
            SetAllRbtn(true);
        }

        private void combo_COMports_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string comPort = combo_COMports.SelectedItem as string;
            sr = new serialReader(comPort);
        }

        private void exportBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            string filter = "CSV file (*.csv)|*.csv| All Files (*.*)|*.*";
            saveFileDialog1.Filter = filter;
            const string header = "Well,Avg_Temp,Std_Dev";
            StreamWriter writer = null;

            //if (saveFileDialog1.ShowDialog() == DialogResult.GetValueOrDefault())
            if (saveFileDialog1.ShowDialog() == true)
            {
                filter = saveFileDialog1.FileName;
                writer = new StreamWriter(filter);

                writer.WriteLine(header);
                writer.WriteLine(Plate.ToString());

                writer.Close();
            }
        }

        private string setColor(double p)
        {
            if (p == 0) { return "#FFFFFF";}
            if (p == 50) { return "#FF00FF"; }
            int red = Convert.ToInt32(p < 50 ? 255 : Math.Round(256 - (p - 50) * 5.12));
            int blue = Convert.ToInt32(p > 50 ? 255 : Math.Round((p) * 5.12));
            string hex = string.Format("#{0:X2}{1:X2}{2:X2}", blue, 0, red);
            return hex;
        }

        private void renderPlate()
        {
            A1_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.A1.CalcTempAvg()));
            B1_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.B1.CalcTempAvg()));
            C1_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.C1.CalcTempAvg()));
            D1_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.D1.CalcTempAvg()));
            E1_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.E1.CalcTempAvg()));
            F1_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.F1.CalcTempAvg()));
            G1_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.G1.CalcTempAvg()));
            H1_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.H1.CalcTempAvg()));
            A2_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.A2.CalcTempAvg()));
            B2_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.B2.CalcTempAvg()));
            C2_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.C2.CalcTempAvg()));
            D2_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.D2.CalcTempAvg()));
            E2_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.E2.CalcTempAvg()));
            F2_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.F2.CalcTempAvg()));
            G2_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.G2.CalcTempAvg()));
            H2_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.H2.CalcTempAvg()));
            A3_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.A3.CalcTempAvg()));
            B3_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.B3.CalcTempAvg()));
            C3_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.C3.CalcTempAvg()));
            D3_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.D3.CalcTempAvg()));
            E3_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.E3.CalcTempAvg()));
            F3_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.F3.CalcTempAvg()));
            G3_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.G3.CalcTempAvg()));
            H3_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.H3.CalcTempAvg()));
            A4_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.A4.CalcTempAvg()));
            B4_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.B4.CalcTempAvg()));
            C4_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.C4.CalcTempAvg()));
            D4_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.D4.CalcTempAvg()));
            E4_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.E4.CalcTempAvg()));
            F4_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.F4.CalcTempAvg()));
            G4_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.G4.CalcTempAvg()));
            H4_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.H4.CalcTempAvg()));
            A5_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.A5.CalcTempAvg()));
            B5_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.B5.CalcTempAvg()));
            C5_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.C5.CalcTempAvg()));
            D5_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.D5.CalcTempAvg()));
            E5_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.E5.CalcTempAvg()));
            F5_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.F5.CalcTempAvg()));
            G5_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.G5.CalcTempAvg()));
            H5_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.H5.CalcTempAvg()));
            A6_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.A6.CalcTempAvg()));
            B6_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.B6.CalcTempAvg()));
            C6_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.C6.CalcTempAvg()));
            D6_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.D6.CalcTempAvg()));
            E6_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.E6.CalcTempAvg()));
            F6_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.F6.CalcTempAvg()));
            G6_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.G6.CalcTempAvg()));
            H6_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.H6.CalcTempAvg()));
            A7_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.A7.CalcTempAvg()));
            B7_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.B7.CalcTempAvg()));
            C7_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.C7.CalcTempAvg()));
            D7_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.D7.CalcTempAvg()));
            E7_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.E7.CalcTempAvg()));
            F7_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.F7.CalcTempAvg()));
            G7_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.G7.CalcTempAvg()));
            H7_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.H7.CalcTempAvg()));
            A8_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.A8.CalcTempAvg()));
            B8_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.B8.CalcTempAvg()));
            C8_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.C8.CalcTempAvg()));
            D8_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.D8.CalcTempAvg()));
            E8_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.E8.CalcTempAvg()));
            F8_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.F8.CalcTempAvg()));
            G8_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.G8.CalcTempAvg()));
            H8_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.H8.CalcTempAvg()));
            A9_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.A9.CalcTempAvg()));
            B9_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.B9.CalcTempAvg()));
            C9_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.C9.CalcTempAvg()));
            D9_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.D9.CalcTempAvg()));
            E9_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.E9.CalcTempAvg()));
            F9_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.F9.CalcTempAvg()));
            G9_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.G9.CalcTempAvg()));
            H9_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.H9.CalcTempAvg()));
            A10_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.A10.CalcTempAvg()));
            B10_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.B10.CalcTempAvg()));
            C10_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.C10.CalcTempAvg()));
            D10_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.D10.CalcTempAvg()));
            E10_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.E10.CalcTempAvg()));
            F10_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.F10.CalcTempAvg()));
            G10_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.G10.CalcTempAvg()));
            H10_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.H10.CalcTempAvg()));
            A11_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.A11.CalcTempAvg()));
            B11_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.B11.CalcTempAvg()));
            C11_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.C11.CalcTempAvg()));
            D11_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.D11.CalcTempAvg()));
            E11_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.E11.CalcTempAvg()));
            F11_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.F11.CalcTempAvg()));
            G11_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.G11.CalcTempAvg()));
            H11_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.H11.CalcTempAvg()));
            A12_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.A12.CalcTempAvg()));
            B12_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.B12.CalcTempAvg()));
            C12_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.C12.CalcTempAvg()));
            D12_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.D12.CalcTempAvg()));
            E12_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.E12.CalcTempAvg()));
            F12_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.F12.CalcTempAvg()));
            G12_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.G12.CalcTempAvg()));
            H12_circ.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(setColor(Plate.H12.CalcTempAvg()));
        }


    }
}