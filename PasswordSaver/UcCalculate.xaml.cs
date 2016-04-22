using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace PasswordSaver
{
    public sealed partial class UcCalculate : UserControl
    {
        public double A;
        public double B;
        public double C;
        public string Operator;
        public string lastButton;

        public UcCalculate()
        {
            this.InitializeComponent();
            Operator = "";
            lastButton = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string str = ((Button)sender).Content.ToString();
            switch (str)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    if (Operator == "=")
                    {
                        tbxShow.Text = str;
                        Operator = "";
                    }
                    else
                    {
                        tbxShow.Text += str;
                    }
                    lastButton = str;
                    break;
                case "+":
                case "-":
                case "*":
                case "/":
                    if (lastButton == "+" || lastButton == "-" || lastButton == "*" || lastButton == "/" || lastButton == "=" || lastButton == "CE")
                    {
                        if (lastButton != "=" && lastButton != "CE")
                        { Operator = str; }
                        lastButton = str; break;
                    }
                    else {
                        Operator = str;
                        double.TryParse(tbxShow.Text, out A);
                        tbxShow.Text = ""; lastButton = str;
                    }
                    break;
                case "=":
                    if (lastButton == "+" || lastButton == "-" || lastButton == "*" || lastButton == "/" || lastButton == "=" || lastButton == "CE")
                    {
                        tbxShow.Text = A.ToString();
                        lastButton = str; break;
                    }
                    if (Operator == "")
                    {
                        double.TryParse(tbxShow.Text, out A);
                        tbxShow.Text = A.ToString();
                        Operator ="="; lastButton = str; break;
                    }
                    double.TryParse(tbxShow.Text, out B);
                    C = Operate(A, B, Operator);
                    Operator = str;
                    tbxShow.Text = C.ToString();
                    lastButton = str;
                    break;
                case "CE":
                    Operator = "";
                    A = 0;
                    B = 0;
                    tbxShow.Text = "";
                    lastButton = str;
                    break;
                default:
                    Debug.WriteLine(str);
                    break;
            }

        }

        private double Operate(double A, double B, string operatorr)
        {
            switch (operatorr)
            {
                case "+":
                    return A + B;
                case "-":
                    return A - B;
                case "*":
                    return A * B;
                case "/":
                    return A / B;
                default:
                    return -1;
            }
        }

        private async void tbxShow_TextChanged(object sender, TextChangedEventArgs e)
        {
           await ((ViewModel)DataContext).CheckPasswordAsync(tbxShow.Text);
        }
    }
}
