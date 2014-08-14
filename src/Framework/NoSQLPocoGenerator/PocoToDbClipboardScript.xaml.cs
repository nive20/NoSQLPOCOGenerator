/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using System;
using System.Windows;
using System.Windows.Media;
using POCOGenerator.Engine;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace POCOGenerator.UI
{
    /// <summary>
    /// Interaction logic for PocoToDbClipboardScript.xaml
    /// </summary>
    public partial class PocoToDbClipboardScript
    {
        #region Global Variables

        public static string DbScript = string.Empty;
        public static string Script = string.Empty;
        string _errorMessage = string.Empty;
        bool _isValidScript = true;

        #endregion

        public PocoToDbClipboardScript()
        {
            InitializeComponent();
            Pocotxtbox.Text =
                                        "using System; \n" +
                                        "namespace  test { \n" +
                                       " public class testClass { \n" +
                                       " public string    username  { get;  set; } \n" +
                                        "public string    email  { get;  set; } \n" +
                                       " public int    phno  { get;  set; }  } } ";
            if (String.IsNullOrEmpty(Script) && String.IsNullOrEmpty(DbScript)) return;
            Pocotxtbox.Text = Script;
            GeneratedScripttxtbox.Text = DbScript;
            Reviewbtn.IsEnabled = true;
        }

        private void prevbtn_Click(object sender, RoutedEventArgs e)
        {
            Script = string.Empty;
            Visibility = Visibility.Hidden;
            var pocoToDbConnection = new PocoToDbConnection();
            var win = Window.GetWindow(this);
            var homePage = (HomePage)(win);
            if (homePage != null) homePage.ContentArea.Content = pocoToDbConnection;
        }

        private void Reviewbtn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(GeneratedScripttxtbox.Text)) return;
            Visibility = Visibility.Hidden;
            var pocoToDbReview = new PocoToDbReviewPane();
            pocoToDbReview.ClipboardOrBrowseIsChecked(true, false);
            var win = Window.GetWindow(this);
            var homePage = (HomePage)(win);
            if (homePage != null) homePage.ContentArea.Content = pocoToDbReview;
        }

        private void generatebtn_Click(object sender, RoutedEventArgs e)
        {
            bool isVb;
            bool isJava;
            bool isCSharp;
            bool isRuby;
            GeneratedScripttxtbox.Text = string.Empty;
            DbScript = string.Empty;
            ScriptReader.ReinitializeExportList();
            Script = Pocotxtbox.Text.ToLower();
            LanguageFactory.LoadLanguageReader(Script);
            ScriptReader.GetLanguageType(out isVb,out  isJava,out  isCSharp,out  isRuby);
            if (isCSharp)
                DbScript = ScriptReader.GenerateCqlScriptFromPocoCSharpScript(Script, ref _errorMessage, ref _isValidScript);
            else if (isVb)
                DbScript = ScriptReader.GenerateCqlScriptFromPocoVbScript(Script, ref _errorMessage, ref _isValidScript);

            else _errorMessage = "Wrong Script Provided.";
            if (!String.IsNullOrEmpty(_errorMessage))
                LogError(_errorMessage);
            else
            {
                GeneratedScripttxtbox.Text = DbScript;
                Reviewbtn.IsEnabled = true;
            }
         }

        /// <summary>
        /// This function displays errorMessage got while Validation.
        /// </summary>
        /// <param name="error"></param>
        private void LogError(string error)
        {
            if (String.IsNullOrEmpty(error))
            {
                var stringManager =
                 new ResourceManager("en-US", Assembly.GetExecutingAssembly());

                error = stringManager.GetString(
                    "VerifySyntax", CultureInfo.CurrentUICulture);
            }
            GeneratedScripttxtbox.Foreground = Brushes.Red;
            GeneratedScripttxtbox.Text = error;
        }

    }
}
