using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MayTinh {
    /// <summary>
    /// Interaction logic for WindowHistory.xaml
    /// </summary>
    public partial class WindowHistory : Window {
        DataBaseHistory his;
        public WindowHistory() {
            InitializeComponent();
            his = new DataBaseHistory();
            listView.ItemsSource = his.showHistory();
        }

        private void button_deleteAll_Click(object sender, RoutedEventArgs e) {
            Thread dele = new Thread(() => {
                Dispatcher.BeginInvoke(new Action(() => {
                    his.deleteAllHistory();
                    listView.ItemsSource = his.showHistory();
                }));
            });
            dele.Start();
        }
    }
}
