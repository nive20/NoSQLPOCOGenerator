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
    /// Interaction logic for PocoToDbConnection.xaml
    /// </summary>
    public partial class PocoToDbConnection
    {
        public PocoToDbConnection()
        {
            InitializeComponent();
        }

        private void cancelbtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void proceedbtn_Click(object sender, RoutedEventArgs e)
        {


            var win = Window.GetWindow(this);
            if (Clipboardbtn.IsChecked == true)
            {
                Visibility = Visibility.Hidden;
                var pocoToDbConnection = new PocoToDbClipboardScript();
                var homePage = (HomePage)(win);
                if (homePage != null) homePage.ContentArea.Content = pocoToDbConnection;
            }
            else if (Browsebtn.IsChecked == true)
            {

                var browseScripts = new BrowseInputScript();
               var filecount= browseScripts.BrowseFilesForConverting(true);
               if (filecount < 1)
               {
                   var homePage = (HomePage)(win);
                   if (homePage != null) homePage.ContentArea.Content = new PocoToDbConnection();
                   MessageBox.Show("No Files Selected");
               }
               else
               {
                   Visibility = Visibility.Hidden;
                   var homePage = (HomePage)(win);
                   if (homePage != null) homePage.ContentArea.Content = browseScripts;
                   browseScripts.NameSpaceNm.Visibility = Visibility.Hidden;
                   browseScripts.NmsLbl.Visibility = Visibility.Hidden;
               }
            }

            else
                MessageBox.Show("Enter the cluster Name to which you want to Connect.");
        }
    }
}
