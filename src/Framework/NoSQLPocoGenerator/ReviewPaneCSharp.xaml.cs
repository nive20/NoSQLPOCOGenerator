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
    /// Interaction logic for ReviewPane.xaml
    /// </summary>
    public partial class ReviewPaneCSharp
    {

        #region Global Variabes

        List<PocoObjectListForExport> _exportPocoList = new List<PocoObjectListForExport>();
        static bool _isCheckedClipboard;
        static bool _isBrowseChecked;

        #endregion

        public ReviewPaneCSharp()
        {
            InitializeComponent();
        }

        private void prevbtn_Click(object sender, RoutedEventArgs e)
        {
            if (_isCheckedClipboard)
            {
                var clipScript = new ClipboardScript();
                clipScript.ReAssignValues();
                Visibility = Visibility.Hidden;
                var win = Window.GetWindow(this);
                var homePage = (HomePage)(win);
                if (homePage != null) homePage.Reviewpanelbl.FontWeight = FontWeights.Normal;
                var page = (HomePage)(win);
                if (page != null) page.Generatelbl.FontWeight = FontWeights.Bold;
                var homePage1 = (HomePage)(win);
                if (homePage1 != null) homePage1.ContentArea.Content = clipScript;
            }
            else if (_isBrowseChecked)
            {
                var outputScript = new OutputScriptForBrowse();
                outputScript.ReAssignPocoObjectList(_exportPocoList);
                Visibility = Visibility.Hidden;
                var win = Window.GetWindow(this);
                var homePage = (HomePage)(win);
                if (homePage != null) homePage.Genrtescriptlbl.FontWeight = FontWeights.Normal;
                var page = (HomePage)(win);
                if (page != null) page.Reviewlbl.FontWeight = FontWeights.Bold;
                var homePage1 = (HomePage)(win);
                if (homePage1 != null) homePage1.ContentArea.Content = outputScript;
            }
            else
            {
                var  generatedPoco = new ScriptsPoco();
                generatedPoco.ReAssignPocoObjectList(_exportPocoList);
                Visibility = Visibility.Hidden;
                var win = Window.GetWindow(this);
                var homePage = (HomePage)(win);
                if (homePage != null) homePage.Reviewpanelbl.FontWeight = FontWeights.Normal;
                var page = (HomePage)(win);
                if (page != null) page.Generatelbl.FontWeight = FontWeights.Bold;
                var homePage1 = (HomePage)(win);
                if (homePage1 != null) homePage1.ContentArea.Content = generatedPoco;
            }
            
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            var win = Window.GetWindow(this);
            var saveoptionsform = new SaveOptions(_exportPocoList, NamespaceName.Text,_isCheckedClipboard);
            var homePage = (HomePage)(win);
            if (homePage != null) homePage.Reviewpanelbl.FontWeight = FontWeights.Normal;
            var page = (HomePage)(win);
            if (page != null) page.Reviewlbl.FontWeight = FontWeights.Normal;
            var homePage1 = (HomePage)(win);
            if (homePage1 != null) homePage1.Saveoptionlbl.FontWeight = FontWeights.Bold;
            var page1 = (HomePage)(win);
            if (page1 != null) page1.ContentArea.Content = saveoptionsform;
        }

        /// <summary>
        /// This functions checks whether this class is called via Clipboard or Browse or Connection Metadata Functionality.
        /// </summary>
        /// <param name="clipboardChecked"></param>
        /// <param name="connectionChecked"></param>
        /// <param name="isBrowse"></param>
        internal void ClipboardOrConnectionorBrowseIsChecked(bool clipboardChecked, bool connectionChecked,bool isBrowse)
        {
            _isCheckedClipboard = clipboardChecked;
            _isBrowseChecked = isBrowse;
        }

        /// <summary>
        /// This functions Assigns Values to controls of UI.
        /// </summary>
        /// <param name="namespaceNm"></param>
        internal void CreateReviewPane(string namespaceNm)
        {
            NamespaceName.Text = namespaceNm;
            _exportPocoList = ScriptReader.FetchExportList();
            Pocolistclass.ItemsSource = _exportPocoList;
        }
    }
}
