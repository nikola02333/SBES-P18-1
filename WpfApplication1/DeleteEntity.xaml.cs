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
    /// Interaction logic for DeleteEntity.xaml
    /// </summary>
    public partial class DeleteEntity : Window
    {
        private ILoadBalancerService factory;
        public DeleteEntity(ClientProxy clientproxy)
        {
            factory = clientproxy;
            InitializeComponent();
        }

      
        private void DeleteEntityB(object sender, RoutedEventArgs e)
        {
            if(tbID.Text=="")
            {
                MessageBox.Show("Molim vas unesti prvo ID brojila koji zelite da obrisete");
                return;
            }
            if (factory.RemoveEntyty(Convert.ToInt32(tbID.Text)) )
            {
                MessageBox.Show("Brisanje Klijenta uspesno obavljeno");
            }
            else
            {
                MessageBox.Show("Brisanje Klijenta neuspesno obavljeno");
            }
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
