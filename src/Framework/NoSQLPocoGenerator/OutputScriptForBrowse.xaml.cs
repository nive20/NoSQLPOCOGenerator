/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/  

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using POCOGenerator.DatabaseObjects;
using POCOGenerator.Engine;

namespace POCOGenerator.UI
{
    /// <summary>
    /// Interaction logic for OutputScriptForBrowse.xaml
    /// </summary>
    public partial class OutputScriptForBrowse
    {

        #region Global Variables

        public static string ColumnFamilyDefinitions = string.Empty;
        public static string NamespaceName = string.Empty;
        public bool IsValidColumnFamily = true;
        static string _pocoScript = string.Empty;

        string _errorMessage = string.Empty;
        public static bool IsPocoToDb = false;
        bool _isValidScript = true;

        public static bool IsJava;
        public static bool IsVb;
        public static bool IsCSharp;
        public static bool IsRuby;
        private List<PocoObjectListForExport> _pocOobjectList;

        #endregion

        public OutputScriptForBrowse()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(_pocoScript))
            {
                OutputScript.Text = _pocoScript;
                Reviewbtn.IsEnabled = true;
            }
        }

        private void reviewbtn_Click(object sender, RoutedEventArgs e)
        {
            var scriptWriter = new ScriptWriter();
            scriptWriter.GetLanguageType(out IsVb, out IsJava, out IsCSharp, out IsRuby);
            if (IsPocoToDb)
            {
                Visibility = Visibility.Hidden;
                var pocoToDbReview = new PocoToDbReviewPane();
                pocoToDbReview.ClipboardOrBrowseIsChecked(false, true);
                var win = Window.GetWindow(this);
                var homePage = (HomePage)(win);
                if (homePage != null) homePage.ContentArea.Content = pocoToDbReview;
            }
            else
            {
                Visibility = Visibility.Hidden;
                var win = Window.GetWindow(this);
                if (IsVb)
                {
                    var reviewPaneVb = new ReviewPaneVb();
                    reviewPaneVb.ClipboardOrConnectionorBrowseIsChecked(false, false, true);
                    reviewPaneVb.CreateReviewPane(NamespaceName);
                    var homePage = (HomePage)(win);
                    if (homePage != null) homePage.ContentArea.Content = reviewPaneVb;
                }
                else
                {
                    var reviewPaneCsharp = new ReviewPaneCSharp();
                    reviewPaneCsharp.ClipboardOrConnectionorBrowseIsChecked(false, false, true);
                    reviewPaneCsharp.CreateReviewPane(NamespaceName);
                    var homePage = (HomePage)(win);
                    if (homePage != null) homePage.ContentArea.Content = reviewPaneCsharp;
                }
                var page = (HomePage)(win);
                if (page != null) page.Genrtescriptlbl.FontWeight = FontWeights.Normal;
                var homePage1 = (HomePage)(win);
                if (homePage1 != null) homePage1.Reviewlbl.FontWeight = FontWeights.Bold;
            }
        }

        private void prevbtn_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            var win = Window.GetWindow(this);
            var inputScript = new BrowseInputScript();
            inputScript.InputScriptCollectionTabs();
            var homePage = (HomePage)(win);
            if (homePage != null) homePage.Genrtescriptlbl.FontWeight = FontWeights.Bold;
            var page = (HomePage)(win);
            if (page != null) page.Reviewlbl.FontWeight = FontWeights.Bold;
            var homePage1 = (HomePage)(win);
            if (homePage1 != null) homePage1.ContentArea.Content = inputScript;
        }

        internal void GenerateScriptForCurrentFile(string script,string namespaceNm)
        {
            OutputScript.Visibility = Visibility.Hidden;
            HeirarchyId.Visibility = Visibility.Visible;
            _pocOobjectList = new List<PocoObjectListForExport>();
            var isCqlScript=false;

            ColumnFamilyDefinitions = script;
            NamespaceName = namespaceNm;
            _pocOobjectList = ScriptReader.GenerateListFromScriptDbToPoco(NamespaceName, ColumnFamilyDefinitions, ref _errorMessage, ref  isCqlScript);
            if (!String.IsNullOrEmpty(_errorMessage))
            {
                LogError(_errorMessage);
                return;
            }
            if (IsPocoToDb)
                OutputScript.Text = _pocoScript;
            else
                Pocolistclass.ItemsSource = _pocOobjectList;
            Reviewbtn.IsEnabled = true;
            
        }

        internal void SaveAsSingleFileOption(string namespaceNm, List<string> filenames, bool isDbToPoco)
        {
          
            _pocOobjectList = new List<PocoObjectListForExport>();
            //bool IsPocoToDb = false;
            var isCqlScript = false;
            var message = string.Empty;
            if (!isDbToPoco)
            {
                IsPocoToDb = true;
                OutputScript.Visibility = Visibility.Visible;
                HeirarchyId.Visibility = Visibility.Hidden;
            }
            else
            {
                OutputScript.Visibility = Visibility.Hidden;
                HeirarchyId.Visibility = Visibility.Visible;
            }
            NamespaceName = namespaceNm;
            foreach (var file in filenames)
            {
                ColumnFamilyDefinitions = File.ReadAllText(file).ToLower();
                if (file.EndsWith(".txt"))
                {
                    _pocOobjectList = ScriptReader.GenerateListFromScriptDbToPoco(NamespaceName, ColumnFamilyDefinitions, ref message, ref isCqlScript);
                }
                else if (LanguageFactory.LoadLanguageReader(ColumnFamilyDefinitions))
                {
                    IsPocoToDb = true;
                    ScriptReader.GetLanguageType(out IsVb, out  IsJava, out  IsCSharp, out  IsRuby);
                    if (IsCSharp)
                        _pocOobjectList = ScriptReader.GenerateListByExtractingMultipleClassesCSharp(ColumnFamilyDefinitions, ref  message, ref  _isValidScript);
                    if (IsVb)
                        _pocOobjectList = ScriptReader.GenerateListByExtractingMultipleClassesVb(ColumnFamilyDefinitions, ref  message, ref  _isValidScript);

                }
                else message = "This Language is not supported.";
            }
            if (!_isValidScript)
            {
                LogError(message);
                return;
            }
            if (IsPocoToDb)
            {
                _pocoScript = ScriptReader.CreateCqlScriptFromList(_pocOobjectList);
                OutputScript.Text = _pocoScript;
            }
            else
                Pocolistclass.ItemsSource = _pocOobjectList;
            Reviewbtn.IsEnabled = true;
           
        }

        /// <summary>
        /// This functions displays the errorMessage got while Script Validation.
        /// </summary>
        /// <param name="error"></param>
        private void LogError(string error)
        {
            OutputScript.Visibility = Visibility.Visible;
            OutputScript.Foreground = Brushes.Red;
            OutputScript.Text = error;
        }

        /// <summary>
        /// This function generates Script for provided POCO.
        /// </summary>
        /// <param name="pocoScript"></param>
        /// <param name="dbToPoco"></param>
        /// <param name="nsName"></param>
        internal void GenerateCqlScriptForCurrentPocoClass(string pocoScript, bool dbToPoco,string nsName)
        {
            OutputScript.Visibility = Visibility.Visible;
            HeirarchyId.Visibility = Visibility.Hidden;
            IsPocoToDb = dbToPoco;
            if (LanguageFactory.LoadLanguageReader(pocoScript))
            {
                ScriptReader.GetLanguageType(out IsVb, out  IsJava, out  IsCSharp, out  IsRuby);
                if (IsCSharp)
                    _pocoScript = ScriptReader.GenerateCqlScriptFromPocoCSharpScript(pocoScript, ref _errorMessage, ref _isValidScript);
                if (IsVb)
                    _pocoScript = ScriptReader.GenerateCqlScriptFromPocoVbScript(pocoScript, ref _errorMessage, ref _isValidScript);
                if (!_isValidScript)
                {
                    LogError(_errorMessage);
                    return;
                }
            }
            else
            {
                _errorMessage = "This language is not supported.";
                LogError(_errorMessage);
            }
            OutputScript.Text = _pocoScript;
            Reviewbtn.IsEnabled = true;
        }

        /// <summary>
        /// This functions Reassign Values i.e. List or Script
        /// </summary>
        /// <param name="exportPocoList"></param>
        internal void ReAssignPocoObjectList(List<PocoObjectListForExport> exportPocoList)
        {
            if (IsPocoToDb)
            {
                OutputScript.Visibility = Visibility.Visible;
                HeirarchyId.Visibility = Visibility.Hidden;
                _pocoScript = ScriptReader.CreateCqlScriptFromList(exportPocoList);
                OutputScript.Text = _pocoScript;
            }
            else
            {
                OutputScript.Visibility = Visibility.Hidden;
                HeirarchyId.Visibility = Visibility.Visible;
                Pocolistclass.ItemsSource = exportPocoList;
            }
            Reviewbtn.IsEnabled = true;
        }
    }
}
