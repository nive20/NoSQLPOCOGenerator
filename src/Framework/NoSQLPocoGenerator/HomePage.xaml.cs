/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using System.Windows;

namespace POCOGenerator.UI
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage
    {
       
        public HomePage()
        {
            InitializeComponent();
            ContentArea.Content = new DataBaseSelection();
           
        }

        private void dscnntbtn_Click(object sender, RoutedEventArgs e)
        {
            var conversionTypeSelection = new ConversionTypeSelection();
            ContentArea.Content = conversionTypeSelection;
            var win = GetWindow(this);
            var homePage = (HomePage)(win);
            if (homePage != null) homePage.Pocogenlbl.Visibility = Visibility.Visible;
            var page = (HomePage)(win);
            if (page != null) page.Connectlbl.Visibility = Visibility.Hidden;
            var homePage1 = (HomePage)(win);
            if (homePage1 != null) homePage1.Generatelbl.Visibility = Visibility.Hidden;
            var page1 = (HomePage)(win);
            if (page1 != null) page1.Metadatalbl.Visibility = Visibility.Hidden;
            var homePage2 = (HomePage)(win);
            if (homePage2 != null) homePage2.Savelbl.Visibility = Visibility.Hidden;
            var page2 = (HomePage)(win);
            if (page2 != null) page2.Reviewpanelbl.Visibility = Visibility.Hidden;
            var homePage3 = (HomePage)(win);
            if (homePage3 != null) homePage3.Summarylbl.Visibility = Visibility.Hidden;
            var page3 = (HomePage)(win);
            if (page3 != null) page3.Genrtescriptlbl.Visibility = Visibility.Hidden;
            var homePage4 = (HomePage)(win);
            if (homePage4 != null) homePage4.Reviewlbl.Visibility = Visibility.Hidden;
            var page4 = (HomePage)(win);
            if (page4 != null) page4.Saveoptionlbl.Visibility = Visibility.Hidden;
        }

      
  
    }
}
