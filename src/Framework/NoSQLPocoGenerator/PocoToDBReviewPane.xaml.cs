/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using System.Collections.Generic;
using System.Windows;  
using POCOGenerator.DatabaseObjects;
using POCOGenerator.Engine;

namespace POCOGenerator.UI
{
    /// <summary>
    /// Interaction logic for PocoToDBReviewPane.xaml
    /// </summary>
    public partial class PocoToDbReviewPane
    {

        #region Global Variables

        private readonly List<PocoObjectListForExport> _exportMetadataList;
        private static bool _isClipboard;
        private static bool _isBrowse;

        #endregion

        public PocoToDbReviewPane()
        {
            // TODO: Complete member initialization
            InitializeComponent();
            _exportMetadataList = ScriptReader.FetchExportList();
            Pocolistclass.ItemsSource = _exportMetadataList;
        }

        private void prevbtn_Click(object sender, RoutedEventArgs e)
        {
            if (_isClipboard)
            {
                var clipboardScript = new PocoToDbClipboardScript();
                Visibility = Visibility.Hidden;
                var win = Window.GetWindow(this);
                var homePage = (HomePage)(win);
                if (homePage != null) homePage.ContentArea.Content = clipboardScript;
            }
            else if(_isBrowse)
            {
                var outputScript = new OutputScriptForBrowse();
                outputScript.ReAssignPocoObjectList(_exportMetadataList);
                Visibility = Visibility.Hidden;
                var win = Window.GetWindow(this);
                var homePage = (HomePage)(win);
                if (homePage != null) homePage.ContentArea.Content = outputScript;
            }
            

        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            var isKeyspaceExists = false;
            var dbOperator = new DatabaseOperationsReader();
            var isAvailable = dbOperator.SaveDataToDb(_exportMetadataList, ref isKeyspaceExists);
            if (isAvailable)
            {
                Visibility = Visibility.Hidden;
                var saveSummary = new Summary();
                saveSummary.ShowSummary(_exportMetadataList);
                var win = Window.GetWindow(this);
                var homePage = (HomePage)(win);
                if (homePage != null) homePage.ContentArea.Content = saveSummary;
            }
            else
            {
                MessageBox.Show(!isKeyspaceExists
                    ? "This Keyspace doesnot Exists."
                    : "This ColumnFamily already exists.");
            }
        }

        /// <summary>
        /// This functions checks whether this class is called via Clipboard or Browse Functionality.
        /// </summary>
        /// <param name="clipboardOption"></param>
        /// <param name="browseOption"></param>
        internal void ClipboardOrBrowseIsChecked(bool clipboardOption, bool browseOption)
        {
            _isClipboard = clipboardOption;
            _isBrowse = browseOption;
        }
    }
}
