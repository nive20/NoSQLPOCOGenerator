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
using System.Windows;
using System.Windows.Controls;
using POCOGenerator.DatabaseObjects;
using POCOGenerator.Engine;

namespace POCOGenerator.UI
{
    /// <summary>
    /// Interaction logic for ColumnFamilyList.xaml
    /// </summary>
    public partial class ColumnFamilyList
    {

        #region Global Variables

        public static List<string> PocoObjectList = new List<string>();
        private static string _namespaceName = string.Empty;
        static List<string> _checkedParentNodes = new List<string>();
        static List<string> _checkedChildNodes = new List<string>();
        public static List<PocoObjectListForExport> PocoList = new List<PocoObjectListForExport>();
        static readonly DatabaseOperationsReader DBreader = new DatabaseOperationsReader(); 
        #endregion

        public ColumnFamilyList()
        {
            bool isRuby;
            bool isCSharp;
            bool isVb;
            bool isJava;
            InitializeComponent();
            PocoList = ScriptReader.FetchExportList();
            if (PocoList.Count > 0)
                PopulateKeyspacenames();
            var scriptWriter = new ScriptWriter();
            scriptWriter.GetLanguageType(out isVb, out isJava, out isCSharp, out isRuby);
        }

        void ColumnFamilyList_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void generatebtn_Click(object sender, RoutedEventArgs e)
        {
            _checkedParentNodes = new List<string>();
            _checkedChildNodes = new List<string>();
            var nodes = Columnfamilytree.Items;
            ScriptReader.ReinitializeExportList();
            GenerateScriptForCassandraDatabaseItems(nodes, ref _checkedParentNodes, ref _checkedChildNodes);
            Visibility = Visibility.Hidden;
            var scriptsPoco = new ScriptsPoco();
            scriptsPoco.ShowGeneratedList(_namespaceName, PocoList);
            var win = Window.GetWindow(this);
            var homePage = (HomePage)(win);
            if (homePage != null) homePage.Metadatalbl.FontWeight = FontWeights.Normal;
            var page = (HomePage)(win);
            if (page != null) page.Generatelbl.FontWeight = FontWeights.Bold;
            var homePage1 = (HomePage)(win);
            if (homePage1 != null) homePage1.ContentArea.Content = scriptsPoco;
        }

        /// <summary>
        /// This function populates the Keyspaces and its Metadata to Treeview.
        /// </summary>
        public void PopulateKeyspacenames()
        {
            if (Columnfamilytree == null)
            {
                Columnfamilytree = new TreeView {Visibility = Visibility.Visible};
            }
            PocoObjectList = new List<string>();
            try
            {

                var dataSource = DBreader.ReadKeyspaceNames();
                if (dataSource == null || dataSource.Count == 0)
                {
                    MessageBox.Show("No Keyspaces Are Present.");
                }
                else Columnfamilytree.ItemsSource = dataSource;
                if (_checkedParentNodes.Count > 0 || _checkedChildNodes.Count > 0)
                    RestoreTreeState();
            }
            catch (Exception)
            {
                MessageBox.Show("Some Problem Occured While Fetching Metadata");
            }
        }

        /// <summary>
        /// This function Restores the state of TreeView .
        /// </summary>
        private void RestoreTreeState()
        {
            var nodes = Columnfamilytree.Items;
            foreach (var checkedParent in _checkedParentNodes)
            {
                foreach (KeyspacesName parent in nodes)
                {
                    if (parent.Parent == checkedParent.ToString(CultureInfo.InvariantCulture))
                        parent.Ischecked = true;
                }
            }
            foreach (var checkedChild in _checkedChildNodes)
            {
                foreach (KeyspacesName parent in nodes)
                {
                    foreach (ColumnFamilyName child in parent.Children)
                    {
                        if (child.Columnfamilyname == checkedChild.ToString(CultureInfo.InvariantCulture))
                            child.Ischecked = true;
                    }
                }
            }
        }

        /// <summary>
        /// This function generates the detailed List for the Selection Provided.
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="checkedParentNodes"></param>
        /// <param name="checkedChildNodes"></param>
        private static void GenerateScriptForCassandraDatabaseItems(ItemCollection nodes, ref List<string> checkedParentNodes, ref List<string> checkedChildNodes)
        {
            foreach (KeyspacesName parent in nodes)
            {
                if (parent.Ischecked != true) continue;
                checkedParentNodes.Add(parent.Parent);
                foreach (var child in parent.Children)
                {
                    if (child.Ischecked != true) continue;
                    checkedChildNodes.Add(child.Columnfamilyname);
                    foreach (var metadataItem in child.ChildMetadata)
                    {
                        var cdatatype = metadataItem.Validator.ToString(CultureInfo.InvariantCulture);
                        var index = cdatatype.LastIndexOf(".", StringComparison.Ordinal);
                        cdatatype = cdatatype.Substring(index + 1);
                        metadataItem.Validator = cdatatype.ToLower();
                    }
                    PocoList = ScriptReader.GenerateListForCreateColumnFamilyMetdata(parent.Parent.ToString(CultureInfo.InvariantCulture), child);
                }
                if (String.IsNullOrEmpty(_namespaceName))
                    _namespaceName = _namespaceName + parent.Parent.ToString(CultureInfo.InvariantCulture);
                else if (String.Equals(_namespaceName, parent.Parent.ToString(CultureInfo.InvariantCulture)))
                    break;
                else
                    _namespaceName = _namespaceName + "_" + parent.Parent.ToString(CultureInfo.InvariantCulture);
            }
           
        }

    }
}