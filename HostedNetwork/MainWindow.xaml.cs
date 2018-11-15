using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace HostedNetwork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
        }

        

        Process setterProcess = new Process();
        Process luncherProcess = new Process();
        

        ProcessStartInfo setterInfo = new ProcessStartInfo();
        ProcessStartInfo luncherInfo = new ProcessStartInfo();
 


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            setterInfo.FileName = "netsh.exe";
            setterInfo.UseShellExecute = false;
            setterInfo.RedirectStandardOutput = true;
            setterInfo.WindowStyle = ProcessWindowStyle.Hidden;
            setterInfo.CreateNoWindow = true;

            setterProcess.StartInfo = setterInfo;

            luncherInfo.FileName = "netsh.exe";
            luncherInfo.Arguments = "wlan start hostednetwork";
            luncherInfo.UseShellExecute = false;
            luncherInfo.RedirectStandardOutput = true;
            luncherInfo.WindowStyle = ProcessWindowStyle.Hidden;
            luncherInfo.CreateNoWindow = true;

            luncherProcess.StartInfo = luncherInfo;

            ssidBox.Text = Environment.UserName;
        }

        private void Ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
            if (radarEllipse.Tag.ToString() == "OFF")
            {
                if (passwordBox.Password.Length > 7 && ssidBox.Text.Length > 2)
                {
                    setterInfo.Arguments = "wlan set hostednetwork mode=allow ssid=" + ssidBox.Text.Trim() + " key=" + passwordBox.Password.Trim();
                    setterProcess.Start();
                    string setterLog = setterProcess.StandardOutput.ReadLine().ToString();
                    if (setterLog == "The hosted network mode has been set to allow. ")
                    {
                        setterProcess.WaitForExit();
                        setterProcess.Close();
                        luncherProcess.Start();
                        string message = luncherProcess.StandardOutput.ReadLine().ToString();
                        //MessageBox.Show(luncherProcess.StandardOutput.ReadLine().ToString());
                        if (message.Equals("The hosted network started. "))
                        {
                            luncherInfo.Arguments = "wlan stop hostednetwork";
                            radarEllipse.Tag = (radarEllipse.Tag.ToString() == "ON") ? "OFF" : "ON";
                            AlertBorder.Tag = "Success";
                            AlertBorderText.Text = "Hosted network created.";
                        }
                        else
                        {

                            AlertBorder.Tag = "Alert";
                            AlertBorderText.Text = message;

                        }
                        luncherProcess.WaitForExit();
                        luncherProcess.Close();
                    }
                    else
                    {
                        AlertBorder.Tag = "Alert";
                        AlertBorderText.Text = setterLog;
                        setterProcess.WaitForExit();
                        setterProcess.Close();
                    }

                }
                else
                {
                    AlertBorder.Tag = "Error";
                    AlertBorderText.Text = "The value for option Password or SSID is invalid. It should be 8 to 63 characters.";
                }
            }
            else
            {
                luncherProcess.Start();
                luncherProcess.WaitForExit();
                luncherProcess.Close();
                luncherInfo.Arguments = "wlan start hostednetwork";
                AlertBorder.Tag = "Normal";
                AlertBorderText.Text = "Set a valid password and SSID and then click on the wireless button";
                radarEllipse.Tag = "OFF";
            }

            MainWin.Cursor = Cursors.Arrow;
        }

        private void MainWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            setterProcess.Close();
            luncherProcess.Close();
            luncherInfo.Arguments = "wlan stop hostednetwork";
            luncherProcess.Start();
            luncherProcess.WaitForExit();
            luncherProcess.Close();
            
        }



        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://aryan-pc.blog.ir");
        }
    }
}
