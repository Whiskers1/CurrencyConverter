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
using HtmlAgilityPack;

namespace Valutaomregner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Valuta> valutas = new List<Valuta>();

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            valutas.Add(new Valuta("DKK", 100));

            var web = new HtmlWeb();
            var document = web.Load(@"http://www.nationalbanken.dk/valutakurser");

            var rows = document.DocumentNode.SelectNodes("//*[@id=\"currenciesTable\"]//tbody//tr");

            if (rows != null && rows.Count > 0)
            {
                foreach (var row in rows)
                {
                    var name = row.SelectNodes(".//td[2]");
                    var value = row.SelectNodes(".//td[3]");

                    valutas.Add(new Valuta(name[0].InnerText, Convert.ToDouble(value[0].InnerText.Replace(',', '.'))));
                }
            }

            foreach (var item in valutas)
            {
                combo1.Items.Add(item.Name);
                combo2.Items.Add(item.Name);
            }
            combo1.SelectedIndex = 0;
            combo2.SelectedIndex = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        public class Valuta
        {
            public Valuta(string name, double value)
            {
                this.Name = name;
                this.Value = value;
            }

            public string Name { get; set; }

            public double Value { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Converte_Click(object sender, RoutedEventArgs e)
        {

            if (!String.IsNullOrEmpty(TextBox_From.Text))
            {

                double from = Convert.ToDouble(TextBox_From.Text.Replace(',', '.'));

                double kurs1 = valutas[combo1.SelectedIndex].Value;

                double kurs2 = valutas[combo2.SelectedIndex].Value;

                double to = Math.Round(((from * kurs1) / kurs2), 2);

                TextBox_To.Text = Convert.ToString(to).Replace('.', ',');
            }
        }
    }
}
