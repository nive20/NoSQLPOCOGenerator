/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using System.Windows;
using POCOGenerator.Engine;

namespace POCOGenerator.UI
{
    /// <summary>
    /// Interaction logic for ConversionTypeSelection.xaml
    /// </summary>
    public partial class ConversionTypeSelection
    {
        public ConversionTypeSelection()
        {
            InitializeComponent();
        }

        private void proceedbtn_Click(object sender, RoutedEventArgs e)
        {
            ScriptReader.ReinitializeExportList();
            Window win;
            if (DbToPocoBtn.IsChecked == true)
            {
                Visibility = Visibility.Hidden;
                var welcomescreen = new Connection();
                win = Window.GetWindow(this);
                var homePage = (HomePage)(win);
                if (homePage != null) homePage.ContentArea.Content = welcomescreen;
                var page = (HomePage)(win);
                if (page != null) page.Pocogenlbl.Visibility = Visibility.Hidden;
                var homePage1 = (HomePage)(win);
                if (homePage1 != null) homePage1.Connectlbl.Visibility = Visibility.Visible;
                var page1 = (HomePage)(win);
                if (page1 != null) page1.Connectlbl.FontWeight = FontWeights.Bold;
                var homePage2 = (HomePage)(win);
                if (homePage2 != null) homePage2.Generatelbl.Visibility = Visibility.Visible;
                var page2 = (HomePage)(win);
                if (page2 != null) page2.Metadatalbl.Visibility = Visibility.Visible;
                var homePage3 = (HomePage)(win);
                if (homePage3 != null) homePage3.Savelbl.Visibility = Visibility.Visible;
                var page3 = (HomePage)(win);
                if (page3 != null) page3.Reviewpanelbl.Visibility = Visibility.Visible;
                var homePage4 = (HomePage)(win);
                if (homePage4 != null) homePage4.Summarylbl.Visibility = Visibility.Visible;
            }

            if (PocoToDbBtn.IsChecked != true) return;
            Visibility = Visibility.Hidden;
            var pocoToDbConnection = new PocoToDbConnection();
            win = Window.GetWindow(this);
            var page4 = (HomePage)(win);
            if (page4 != null) page4.ContentArea.Content = pocoToDbConnection;
        }

        private void cancelbtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
