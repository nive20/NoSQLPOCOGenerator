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
    /// Interaction logic for CassandraAuthenticationUI.xaml
    /// </summary>
    public partial class CassandraAuthenticationUi
    {

        public CassandraAuthenticationUi()
        {
            InitializeComponent();
            ErrorLog.Visibility = Visibility.Hidden;
        }

        private void proceedbtn_Click(object sender, RoutedEventArgs e)
        {
            var userName = Usernametxtbox.Text;
            var password = Passwordtxtbox.Text;
            var clusterName = Clustertxtbx.Text;
            var connectionObj = new DatabaseOperationsReader();
            connectionObj.AssignCredentialsToDatabase(userName, password, clusterName);
            var isConnected = connectionObj.ConnectToDatabase(DatabaseType.Cassandra);

            if (isConnected)
            {
                Visibility = Visibility.Hidden;
                var win = Window.GetWindow(this);
                var conversionTypeSelection = new ConversionTypeSelection();
                var homePage = (HomePage)(win);
                if (homePage != null) homePage.ContentArea.Content = conversionTypeSelection;
            }
            else
            {
                ErrorLog.Visibility = Visibility.Visible;
                ErrorLog.Content = "Enter correct credentials.";
            }
        }

        private void cancelbtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
