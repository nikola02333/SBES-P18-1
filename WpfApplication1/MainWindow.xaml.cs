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
using Common;
using System.ServiceModel;
using System.Security.Principal;
using SecurityManager;
using System.Threading;

namespace WpfApplication1
{
   
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static ClientProxy proxy;
        public static EnumType _type;
        public void Start()
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:65000/ILoadBalancerService";
          
            proxy = new ClientProxy(binding, new EndpointAddress(new Uri(address)));
              _type = proxy.Detekcija();
			if(_type == EnumType.Customer)
			{
				this.buttonProcessIdB.IsEnabled = true;
			}
			if(_type == EnumType.Administrator)
			{
				this.buttonProcessIdB.IsEnabled = true;
				this.ChangeCounterB.IsEnabled = true;
				this.AddEntityB.IsEnabled = true;
				this.DeleteEntityB.IsEnabled = true;
			}
            if(_type== EnumType.Operator)
            {
                this.ChangeCounterB.IsEnabled = true;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            Start();
        }
        private void B1_Click(object sender, RoutedEventArgs e)
        {
            addCounter ad = new addCounter(proxy);
            ad.Show();
        }

		private void ProcesId_function(object sender, RoutedEventArgs e)
		{
            ID_Process ip = new ID_Process(proxy);
            ip.Show();
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

        private void ChangeCounterB_Click(object sender, RoutedEventArgs e)
        {
            ChangeEntity1 c = new ChangeEntity1(proxy,_type);
            c.Show();

        }

        private void AddEntityB_Click(object sender, RoutedEventArgs e)
        {
            addCounter ad = new addCounter(proxy);
            ad.Show();

        }

        private void DeleteEntityB_Click(object sender, RoutedEventArgs e)
        {
            DeleteEntity dE = new DeleteEntity(proxy);
            dE.Show();

        }
    }
}

