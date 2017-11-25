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
    /// Interaction logic for addCounter.xaml
    /// </summary>
    public partial class addCounter : Window
    {
        private ILoadBalancerService proxy;
        public addCounter(ClientProxy Clientproxy)
        {
            proxy = Clientproxy;
            InitializeComponent();
        }

        private void AddEntity(object sender, RoutedEventArgs e)
        {
           if((tbID.Text=="" || tbIme.Text=="" || tbPrz.Text==""|| tbPot.Text==""))
            {
                MessageBox.Show("Molim vas popunite sva polja pre dodavanja entiteta!");
                return;
            }
            try
            {
                Brojilo br = new Brojilo(tbID.Text, tbIme.Text, tbPrz.Text, tbPot.Text);
                proxy.AddEntyty(br);
                MessageBox.Show("Dodavanje  entiteta uspesno zavrseno");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
