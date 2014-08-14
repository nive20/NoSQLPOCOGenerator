/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and  
 * limitations under the License.*/

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using POCOGenerator.DatabaseObjects;
using POCOGenerator.Engine;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace POCOGenerator.UI
{
    /// <summary>
    /// Interaction logic for ClipboardScript.xaml
    /// </summary>
    public partial class ClipboardScript
    {
        #region Global Variables 

        public bool IsValidColumnFamily = true;
        public string ColumnFamilyName = string.Empty;
        public static string ColumnFamilyDefinitions = string.Empty;
        static List<PocoObjectListForExport> _pocOobjectList = new List<PocoObjectListForExport>();
        public static string NamespaceName = string.Empty;
        private readonly bool _isVb;

        #endregion

        public ClipboardScript()
        {
            bool isJava;
            bool isCSharp;
            bool isRuby;
            InitializeComponent();
            TxtCliDefinition.Text =
                
            "CREATE COLUMN FAMILY account \n "+
           " WITH comparator = UTF8Type \n" +
                "AND key_validation_class=UTF8Type \n" +
                " AND column_metadata = [ \n " +
               " {column_name: title, validation_class: UTF8Type} \n "+
                "{column_name: created, validation_class: DateType} \n "+
                "{column_name: use_namespace, validation_class: BooleanType} \n "+
                "]; ";
            var scriptWriter = new ScriptWriter();
            scriptWriter.GetLanguageType(out _isVb, out isJava, out isCSharp, out isRuby);
        }

        private void prevbtn_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            var welcomescreen = new Connection();
            var win = Window.GetWindow(this);
            var homePage = (HomePage)(win);
            if (homePage != null) homePage.ContentArea.Content = welcomescreen;
        }

        private void generatebtn_Click(object sender, RoutedEventArgs e)
        {
            Pocolistclass.ItemsSource = null;
            _pocOobjectList = new List<PocoObjectListForExport>();
                var errorMessage = string.Empty;
                var isCqlScript = false;
                ColumnFamilyDefinitions = TxtCliDefinition.Text.ToLower();
                ColumnFamilyDefinitions = ColumnFamilyDefinitions.Trim('\n', '\r');
                if (String.IsNullOrEmpty(Nmspcenametxtbox.Text))
                    MessageBox.Show("Enter NAMESPACE name !!!");
                else
                { try
                    {
                    ScriptReader.ReinitializeExportList();
                    NamespaceName=Nmspcenametxtbox.Text;
                    _pocOobjectList = ScriptReader.GenerateListFromScriptDbToPoco(NamespaceName, ColumnFamilyDefinitions, ref errorMessage, ref isCqlScript);
                  
                    if (!String.IsNullOrEmpty(errorMessage))
                       LogError(errorMessage);
                   else
                   { 
                       Pocolistclass.ItemsSource = _pocOobjectList;
                       Reviewbtn.IsEnabled = true;
                   }
                    }
                    catch (Exception)
                    {
                        LogError(errorMessage);
                    }
            }
               
        }

        private void Reviewbtn_Click(object sender, RoutedEventArgs e)
        {
            if (_pocOobjectList.Count>0)
            {

                Visibility = Visibility.Hidden;
                var win = Window.GetWindow(this);
                if (_isVb)
                {
                    var reviewPaneVb = new ReviewPaneVb();
                    reviewPaneVb.ClipboardOrConnectionorBrowseIsChecked(true, false, false);
                    reviewPaneVb.CreateReviewPane(Nmspcenametxtbox.Text);
                    var homePage = (HomePage)(win);
                    if (homePage != null) homePage.ContentArea.Content = reviewPaneVb;
                }
                else
                {
                    var reviewPaneCsharp = new ReviewPaneCSharp();
                    reviewPaneCsharp.ClipboardOrConnectionorBrowseIsChecked(true, false, false);
                    reviewPaneCsharp.CreateReviewPane(Nmspcenametxtbox.Text);
                    var homePage = (HomePage)(win);
                    if (homePage != null) homePage.ContentArea.Content = reviewPaneCsharp;
                }
                var page = (HomePage)(win);
                if (page != null) page.Genrtescriptlbl.FontWeight = FontWeights.Normal;
                var homePage1 = (HomePage)(win);
                if (homePage1 != null) homePage1.Reviewlbl.FontWeight = FontWeights.Bold;
            }
        }


        /// <summary>
        /// This function ReAssigns the value of UI Controls.
        /// </summary>
        internal void ReAssignValues()
        {
            TxtCliDefinition.Text = ColumnFamilyDefinitions;
            Pocolistclass.ItemsSource = _pocOobjectList;
            Nmspcenametxtbox.Text = NamespaceName;
            Reviewbtn.IsEnabled = true;
            
        }

        /// <summary>
        /// This function displays the errorMessage returned while Script Validation.
        /// </summary>
        /// <param name="errorMessage"></param>
        private void LogError(string errorMessage)
        {
            if (String.IsNullOrEmpty(errorMessage))
            {
                var stringManager =
                  new ResourceManager("en-US", Assembly.GetExecutingAssembly());

                errorMessage = stringManager.GetString(
                    "VerifySyntax", CultureInfo.CurrentUICulture);
            }
            Namespacelbl.Foreground = Brushes.Red;
            Namespacelbl.Text = errorMessage;
        }

    }
}
