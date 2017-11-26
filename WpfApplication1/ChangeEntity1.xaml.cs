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
    /// Interaction logic for ChangeEntity1.xaml
    /// </summary>
    public partial class ChangeEntity1 : Window
    {
        private ILoadBalancerService proxy;
        private Brojilo br;
        private EnumType type;
        public ChangeEntity1(ClientProxy Clientproxy,EnumType _type)
        {
            type = _type;
            proxy = Clientproxy;
            InitializeComponent();
        }
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void button_Search(object sender, RoutedEventArgs e)
        {
            if(tb_id1.Text =="")
            {
                MessageBox.Show("Molim vas unesite ID brojila");
                return;
            }

            br = proxy.SearchId(Convert.ToInt32(tb_id1.Text));
            if (br!= null )
            {
                if(type == EnumType.Operator)
                {
                    tbID.Text = br.Id;
                    tb_potrsnja.Text = br.Potrosnja;
                    tb_ime.Text = br.Ime;
                    tb_prz.Text = br.Prezime;
                    tb_ime.IsReadOnly = true;
                    tb_prz.IsReadOnly=true;
                }
                else
                {
                    tbID.Text = br.Id;
                    tb_ime.Text = br.Ime;
                    tb_prz.Text = br.Prezime;
                    tb_potrsnja.Text = br.Potrosnja;
                }
            }
            else
            {
                MessageBox.Show("Zao nam je ovaj id ne postoji u bazi !!!");
                tb_id1.Text = "";
            }
        }

        private void ok_b_Click(object sender, RoutedEventArgs e)
        {
            if (tbID.Text == "" || tb_ime.Text=="" || tb_prz.Text=="" || tb_potrsnja.Text=="")
            {
                MessageBox.Show("Molim vas da unesete sve parametre za promenu brojila");
                return;
            }


            if ( (tbID.Text!="") &&  (tb_ime.Text!="") && (tb_prz.Text !="") && (tb_potrsnja.Text!="") )
            {
                Brojilo br2 = new Brojilo(tbID.Text, tb_ime.Text, tb_prz.Text, tb_potrsnja.Text);
               if( proxy.ChangeEntyty(br2, br))
                {
                    MessageBox.Show("Uspesno ste izmenili brojilo");
                }
               else
                {
                    MessageBox.Show("Nisam uspeo da izmeniti entitet");
                }
            }
        }
    }
}
