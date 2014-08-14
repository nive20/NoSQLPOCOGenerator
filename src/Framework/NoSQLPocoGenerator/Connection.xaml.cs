/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/  

using System.Windows;
using System.Windows.Controls;
using POCOGenerator.Engine;

namespace POCOGenerator.UI
{
    /// <summary>
    /// Interaction logic for Connection.xaml
    /// </summary>
    public partial class Connection
    {

        #region Global Variables
        private string _selectedLanguage = string.Empty; 
        #endregion

        public Connection()
        {
            InitializeComponent();

        }

        private void proceedbtn_Click(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            AssignLanguageType();
            if (Clipboardbtn.IsChecked == true)
            {
                Visibility = Visibility.Hidden;
                var clipboardscript = new ClipboardScript();
                var homePage = (HomePage)(win);
                if (homePage != null) homePage.Connectlbl.Visibility = Visibility.Hidden;
                var page = (HomePage)(win);
                if (page != null) page.Metadatalbl.Visibility = Visibility.Hidden;
                var homePage1 = (HomePage)(win);
                if (homePage1 != null) homePage1.Generatelbl.Visibility = Visibility.Hidden;
                var page1 = (HomePage)(win);
                if (page1 != null) page1.Reviewpanelbl.Visibility = Visibility.Hidden;
                var homePage2 = (HomePage)(win);
                if (homePage2 != null) homePage2.Savelbl.Visibility = Visibility.Hidden;
                var page2 = (HomePage)(win);
                if (page2 != null) page2.Genrtescriptlbl.Visibility = Visibility.Visible;
                var homePage3 = (HomePage)(win);
                if (homePage3 != null) homePage3.Genrtescriptlbl.FontWeight = FontWeights.Bold;
                var page3 = (HomePage)(win);
                if (page3 != null) page3.Reviewlbl.Visibility = Visibility.Visible;
                var homePage4 = (HomePage)(win);
                if (homePage4 != null) homePage4.Saveoptionlbl.Visibility = Visibility.Visible;
                var page4 = (HomePage)(win);
                if (page4 != null) page4.Summarylbl.Visibility = Visibility.Visible;
                var homePage5 = (HomePage)(win);
                if (homePage5 != null) homePage5.ContentArea.Content = clipboardscript;
            }
            else if(Browsebtn.IsChecked==true)
            {

                var inputScript = new BrowseInputScript();
                var filecount= inputScript.BrowseFilesForConverting(false);  //Argument indicates IsDbToPoco as false
                if (filecount < 1)
                {
                    MessageBox.Show("No Files Selected");
                }
                else
                {
                    Visibility = Visibility.Hidden;
                    var homePage = (HomePage)(win);
                    if (homePage != null) homePage.ContentArea.Content = inputScript;
                    var page = (HomePage)(win);
                    if (page != null) page.Connectlbl.Visibility = Visibility.Hidden;
                    var homePage1 = (HomePage)(win);
                    if (homePage1 != null) homePage1.Metadatalbl.Visibility = Visibility.Hidden;
                    var page1 = (HomePage)(win);
                    if (page1 != null) page1.Generatelbl.Visibility = Visibility.Hidden;
                    var homePage2 = (HomePage)(win);
                    if (homePage2 != null) homePage2.Reviewpanelbl.Visibility = Visibility.Hidden;
                    var page2 = (HomePage)(win);
                    if (page2 != null) page2.Savelbl.Visibility = Visibility.Hidden;
                    var homePage3 = (HomePage)(win);
                    if (homePage3 != null) homePage3.Genrtescriptlbl.Visibility = Visibility.Visible;
                    var page3 = (HomePage)(win);
                    if (page3 != null) page3.Genrtescriptlbl.FontWeight = FontWeights.Bold;
                    var homePage4 = (HomePage)(win);
                    if (homePage4 != null) homePage4.Reviewlbl.Visibility = Visibility.Visible;
                    var page4 = (HomePage)(win);
                    if (page4 != null) page4.Saveoptionlbl.Visibility = Visibility.Visible;
                    var homePage5 = (HomePage)(win);
                    if (homePage5 != null) homePage5.Summarylbl.Visibility = Visibility.Visible;
                }
            }
            else 
            {
                Visibility = Visibility.Hidden;
                var columnfamilyList = new ColumnFamilyList();
                columnfamilyList.PopulateKeyspacenames();
                var homePage = (HomePage)(win);
                if (homePage != null) homePage.Connectlbl.FontWeight = FontWeights.Normal;
                var page = (HomePage)(win);
                if (page != null) page.Metadatalbl.FontWeight = FontWeights.Bold;
                var homePage1 = (HomePage)(win);
                if (homePage1 != null) homePage1.ContentArea.Content = columnfamilyList;
            }
        }

        private void prevbtn_Click(object sender, RoutedEventArgs e)
        {
            var conversionTypeSelection = new ConversionTypeSelection();
            var win = Window.GetWindow(this);
            var homePage = (HomePage)(win);
            if (homePage != null) homePage.ContentArea.Content = conversionTypeSelection;
            var page = (HomePage)(win);
            if (page != null) page.Summarylbl.FontWeight = FontWeights.Normal;
            var homePage1 = (HomePage)(win);
            if (homePage1 != null) homePage1.Pocogenlbl.Visibility = Visibility.Visible;
            var page1 = (HomePage)(win);
            if (page1 != null) page1.Connectlbl.Visibility = Visibility.Hidden;
            var homePage2 = (HomePage)(win);
            if (homePage2 != null) homePage2.Generatelbl.Visibility = Visibility.Hidden;
            var page2 = (HomePage)(win);
            if (page2 != null) page2.Metadatalbl.Visibility = Visibility.Hidden;
            var homePage3 = (HomePage)(win);
            if (homePage3 != null) homePage3.Savelbl.Visibility = Visibility.Hidden;
            var page3 = (HomePage)(win);
            if (page3 != null) page3.Reviewpanelbl.Visibility = Visibility.Hidden;
            var homePage4 = (HomePage)(win);
            if (homePage4 != null) homePage4.Summarylbl.Visibility = Visibility.Hidden;
        }

        private void ScriptTypesCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedLanguage = ((ComboBoxItem)ScriptTypesCombo.SelectedItem).Content.ToString();
        }

        /// <summary>
        /// This function assigns the language type.
        /// </summary>
        private void AssignLanguageType()
        {
            if (_selectedLanguage == "VB")
                LanguageFactory.LoadLanguageWriter(ScriptType.Vb);
            if (_selectedLanguage == "C#")
                LanguageFactory.LoadLanguageWriter(ScriptType.CSharp);
            if (_selectedLanguage == "Java")
                LanguageFactory.LoadLanguageWriter(ScriptType.Java);
            if (_selectedLanguage == "Ruby")
                LanguageFactory.LoadLanguageWriter(ScriptType.Ruby);
        }

    }
}
