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
    /// Interaction logic for Scripts_POCO.xaml
    /// </summary>
    public partial class ScriptsPoco 
    {

        #region Global Variables

        ColumnFamilyList _columnfamilylist;
        static string _namespaceName = string.Empty;
        private bool _isJava;
        private bool _isVb;
        private bool _isCSharp;
        private bool _isRuby;
      
        #endregion

        public ScriptsPoco()
        {
            InitializeComponent();
        }

        private void prevbtn_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            _columnfamilylist = new ColumnFamilyList();
            var win = Window.GetWindow(this);
            var homePage = (HomePage)(win);
            if (homePage != null) homePage.ContentArea.Content = _columnfamilylist;
        }

        private void reviewbtn_Click(object sender, RoutedEventArgs e)
        {
            var scriptWriter = new ScriptWriter();
            scriptWriter.GetLanguageType(out _isVb, out _isJava, out _isCSharp, out _isRuby);
            Visibility = Visibility.Hidden;
            var win = Window.GetWindow(this);
            var page = (HomePage)(win);
            if (page != null) page.Generatelbl.FontWeight = FontWeights.Normal;
            var homePage1 = (HomePage)(win);
            if (homePage1 != null) homePage1.Reviewpanelbl.FontWeight = FontWeights.Bold;

            if (_isVb)
            {
                var  reviewPaneVb = new ReviewPaneVb();
                reviewPaneVb.ClipboardOrConnectionorBrowseIsChecked(false, false, true);
                reviewPaneVb.CreateReviewPane(_namespaceName);
                var homePage = (HomePage)(win);
                if (homePage != null) homePage.ContentArea.Content = reviewPaneVb;
            }
            else
            {
                var reviewPaneCsharp = new ReviewPaneCSharp();
                reviewPaneCsharp.ClipboardOrConnectionorBrowseIsChecked(false, true, false);
                reviewPaneCsharp.CreateReviewPane(_namespaceName);
                var homePage = (HomePage)(win);
                if (homePage != null) homePage.ContentArea.Content = reviewPaneCsharp;
            }

        }

        /// <summary>
        /// This function shows List in UI
        /// </summary>
        /// <param name="namespaceNm"></param>
        /// <param name="pocOobjectList"></param>
        internal void ShowGeneratedList(string namespaceNm, List<PocoObjectListForExport> pocOobjectList)
        {
            _namespaceName = namespaceNm;
            Pocolistclass.ItemsSource = pocOobjectList;
        }

        /// <summary>
        /// This function reassigns the List.
        /// </summary>
        /// <param name="exportPocoList"></param>
        internal void ReAssignPocoObjectList(List<PocoObjectListForExport> exportPocoList)
        {
            Pocolistclass.ItemsSource = exportPocoList;
        }
    }
}
