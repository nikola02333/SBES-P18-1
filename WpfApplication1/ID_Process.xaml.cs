using Common;
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
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for ID_Process.xaml
    /// </summary>
    public partial class ID_Process : Window
    {
      private  ILoadBalancerService factory;
        public ID_Process(ClientProxy clientproxt)
        {
            factory = clientproxt;
            InitializeComponent();
        }

        private void ProcessID_b_Click(object sender, RoutedEventArgs e)
        {
            if(tbID.Text=="")
            {
                MessageBox.Show("Molim vas unesite ID brojila");
                return;
            }
           double x= factory.Process_Id(Convert.ToInt32(tbID.Text));
            tbID_Copy.Text = x.ToString();
                
        }
        private void Exit_b_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
