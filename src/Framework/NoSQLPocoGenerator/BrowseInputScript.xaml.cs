/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using POCOGenerator.Engine;

namespace POCOGenerator.UI
{
    /// <summary>
    /// Interaction logic for BrowseInputScript.xaml
    /// </summary>
    public partial class BrowseInputScript
    {
        #region Global Variables Region

        private static List<string> _filenames;
        private static string _namespaceName = string.Empty;
        private static bool _isPocoToDb;

        #endregion

        public BrowseInputScript()
        {
            bool isRuby;
            bool isCSharp;
            bool isVb;
            bool isJava;
            InitializeComponent();
            var scriptWriter = new ScriptWriter();
            scriptWriter.GetLanguageType(out isVb, out isJava, out isCSharp, out isRuby);
        }

        #region Implementation for browse functionality

        /// <summary>
        /// This function give browse for files functionality for POCO To DB or Vice-Versa
        /// </summary>
        /// <param name="pocoToDb"></param>
        /// <returns>Returns filecount as int</returns>
        internal int BrowseFilesForConverting(bool pocoToDb)
        {
            _filenames = new List<string>();
            _isPocoToDb = pocoToDb;
            var openfiledialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = !pocoToDb
                    ? "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
                    : "CSharp Files (*.cs)|*.cs|VB Files (*.vb)|*.vb|All Files (*.*)|*.*",
                FilterIndex = 1,
                Multiselect = true
            };

            if (openfiledialog.ShowDialog() == true)
            {
                _filenames.AddRange(openfiledialog.FileNames);
            }
            if (_filenames.Count > 0)
                InputScriptCollectionTabs();
            return _filenames.Count;

        }

        /// <summary>
        /// This funtions deisplays all the selected files content in tab control.
        /// </summary>
        internal void InputScriptCollectionTabs()
        {
            foreach (var file in _filenames)
            {
                var textscript = new TextBox
                {
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                    IsReadOnly = true,
                    Height = InputFilestab.Height,
                    Width = InputFilestab.Width
                };
                var tabpageItem = new TabItem();
                InputFilestab.Items.Add(tabpageItem);
                tabpageItem.Header = file.ToString(CultureInfo.InvariantCulture);
                textscript.Text = File.ReadAllText(file);
                tabpageItem.Content = textscript;
            }
            if (!_isPocoToDb)
                NameSpaceNm.Text = _namespaceName;
            else
            {
                NameSpaceNm.Visibility = Visibility.Hidden;
                NmsLbl.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// This functions Generates Script as Selected option i.e, current Selection or As Single File Option.
        /// </summary>
        /// <param name="btnname"></param>
        private void GenerateScriptForTabsWithOptions(string btnname)
        {
            bool isDbToPoco = false;
            _namespaceName = NameSpaceNm.Text;
            var script = ((TextBox)(InputFilestab.SelectedContent)).Text.ToString(CultureInfo.InvariantCulture).ToLower();
            ScriptReader.ReinitializeExportList();
            var outputScript = new OutputScriptForBrowse();
            if (btnname == "CurrentSelection")
            {
                if (_isPocoToDb)
                    outputScript.GenerateCqlScriptForCurrentPocoClass(script, _isPocoToDb, null);
                else
                    outputScript.GenerateScriptForCurrentFile(script, NameSpaceNm.Text);
            }
            if (btnname == "As Single File")
            {
                if (!_isPocoToDb)
                    isDbToPoco = true;
                outputScript.SaveAsSingleFileOption(NameSpaceNm.Text, _filenames, isDbToPoco);

            }

            Visibility = Visibility.Hidden;
            var win = Window.GetWindow(this);
            var homePage = (HomePage)(win);
            if (homePage != null) homePage.Genrtescriptlbl.FontWeight = FontWeights.Normal;
            var page = (HomePage)(win);
            if (page != null) page.Reviewlbl.FontWeight = FontWeights.Bold;
            var homePage1 = (HomePage)(win);
            if (homePage1 != null) homePage1.ContentArea.Content = outputScript;
        }

        #endregion

        private void generatebtn_Click(object sender, RoutedEventArgs e)
        {
            if (!_isPocoToDb)
            {
                if (!String.IsNullOrEmpty(NameSpaceNm.Text))
                {
                    Popup.IsOpen = true;
                }
                else
                    MessageBox.Show("Enter Keyspace Name"); 
            }
            else
            Popup.IsOpen = true;
        }

        private void generateAsBtn_Click(object sender, RoutedEventArgs e)
        {
            Popup.IsOpen = false;
            string btnname = string.Empty;
            if (CurrentSelectionBtn.IsChecked == true)
                btnname = CurrentSelectionBtn.Content.ToString();
            if (SingleFileBtn.IsChecked == true)
                btnname = SingleFileBtn.Content.ToString();
            GenerateScriptForTabsWithOptions(btnname);
        }

      
      

    }
}
