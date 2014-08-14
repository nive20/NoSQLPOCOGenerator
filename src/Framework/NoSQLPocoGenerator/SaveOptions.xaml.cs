/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using System.Collections.Generic;
using System.Windows;
using Microsoft.Win32;
using POCOGenerator.DatabaseObjects;
using POCOGenerator.Engine;
  
namespace POCOGenerator.UI
{
    /// <summary>
    /// Interaction logic for SaveOptions.xaml
    /// </summary>
    public partial class SaveOptions
    {

        #region Global Variables

        string _btnname = string.Empty;
        readonly List<PocoObjectListForExport> _exportPocoList = new List<PocoObjectListForExport>();
        readonly string _namespaceName = string.Empty;
        private bool _isJava;
        private bool _isVb;
        private bool _isCSharp;
        private bool _isRuby;
       readonly ScriptWriter _scriptWriter = new ScriptWriter(); 

        #endregion

        public SaveOptions(List<PocoObjectListForExport> pocoList, string nmespceName, bool isClipboard)
        {
            InitializeComponent();
            _exportPocoList = pocoList;
            _namespaceName = nmespceName;
            if (isClipboard)
                OneFileperObjectBtn.IsEnabled = false;
        }

        private void prevbtn_Click(object sender, RoutedEventArgs e)
        {

            Visibility = Visibility.Hidden;
            var reviewPane = new ReviewPaneCSharp();
            var win = Window.GetWindow(this);
            reviewPane.CreateReviewPane(_namespaceName);
            var homePage = (HomePage)(win);
            if (homePage != null) homePage.ContentArea.Content = reviewPane;
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            if (SingleFileBtn.IsChecked == true)
                _btnname = SingleFileBtn.Content.ToString();
            if (OneFileperObjectBtn.IsChecked == true)
                _btnname = OneFileperObjectBtn.Content.ToString();
            SaveFileForTabsWithOptions(_btnname);

        }

        /// <summary>
        /// This function provides option for save as single file and also as one file per object.
        /// </summary>
        /// <param name="selectedbtntype"></param>
        public void SaveFileForTabsWithOptions(string selectedbtntype)
        {
            var isSaved = false;
            _scriptWriter.GetLanguageType(out _isVb, out _isJava, out _isCSharp, out _isRuby);
            var saveFile = new SaveFileDialog();
            if (_isCSharp)
                saveFile.Filter = "csharp Files (*.cs)|*.cs|All Files (*.*)|*.*";
            else if (_isVb)
                saveFile.Filter = "vb Files (*.vb)|*.vb|All Files (*.*)|*.*";
            else if (_isJava)
                saveFile.Filter = "java Files (*.class)|*.class|All Files (*.*)|*.*";
            else if (_isRuby)
                saveFile.Filter = "Ruby Files (*.rb)|*.rb|All Files (*.*)|*.*";

            if (selectedbtntype == "Save As Single File")
            {
                if (saveFile.ShowDialog() == true)
                {
                    isSaved = _scriptWriter.WriteScriptAsPerSelectedLanguage(_exportPocoList, null, _namespaceName, saveFile.FileName);
                }
            }

            if (selectedbtntype == "One File Per Object")
            {
                foreach (var parentItem in _exportPocoList)
                {
                   if (saveFile.ShowDialog() == true)
                    {
                        isSaved = _scriptWriter.WriteScriptAsPerSelectedLanguage(null, parentItem, _namespaceName, saveFile.FileName);
                    } 
                }
            }
            if (!isSaved) return;
            Visibility = Visibility.Hidden;
            var saveSummary = new Summary();
            saveSummary.ShowSummary(_exportPocoList);
            var win = Window.GetWindow(this);
            var homePage = (HomePage)(win);
            if (homePage != null) homePage.Savelbl.FontWeight = FontWeights.Normal;
            var page = (HomePage)(win);
            if (page != null) page.Saveoptionlbl.FontWeight = FontWeights.Normal;
            var homePage1 = (HomePage)(win);
            if (homePage1 != null) homePage1.Summarylbl.FontWeight = FontWeights.Bold;
            var page1 = (HomePage)(win);
            if (page1 != null) page1.ContentArea.Content = saveSummary;
        }

    
    }
}
