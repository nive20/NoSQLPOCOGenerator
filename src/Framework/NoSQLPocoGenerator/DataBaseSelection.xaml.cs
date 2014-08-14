/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/
  
using System.Windows.Controls;

namespace POCOGenerator.UI
{
    /// <summary>
    /// Interaction logic for DataBaseSelection.xaml
    /// </summary>
    public partial class DataBaseSelection
    {
        public DataBaseSelection()
        {
            InitializeComponent();
        }

        private void DBTypesCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DbTypesCombo.SelectedItem.ToString() == DbTypesCombo.Items[0].ToString())
            {
                var cassandraUi = new CassandraAuthenticationUi();
                AuthenticatiContentArea.Content = cassandraUi;
            }

            else if (DbTypesCombo.SelectedItem.ToString() == DbTypesCombo.Items[1].ToString())
            {
                var mongoDbui = new MongoDbAuthenticationUi();
                AuthenticatiContentArea.Content = mongoDbui;
            }

        }
    }
}
