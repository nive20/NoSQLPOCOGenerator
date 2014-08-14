/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  


 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using System.Collections.Generic;  

namespace POCOGenerator.DatabaseObjects
{
    #region ConverterSummary

    public class SummaryDetails
    {
        public int KeyspaceCount { get; set; }
        public List<KeySpaceDetails> KeyspaceDetails { get; set; }
    }
    public class KeySpaceDetails
    {
        private List<PocoObjectListForExport> _columnFamilyObjects = new List<PocoObjectListForExport>();
        public string KeyspaceName { get; set; }

        public List<PocoObjectListForExport> ColumnFamilyDetails
        {
            get { return _columnFamilyObjects; }
           set { _columnFamilyObjects = value; }
        }

        public int ColumnfamilyCount { get; set; }
    }
    public class PreviousKeySpaceDetails
    {
        private List<PocoObjectListForExport> _columnFamilyObjects = new List<PocoObjectListForExport>();
        public string KeyspaceName { get; set; }

        public List<PocoObjectListForExport> ColumnFamilyDetails
        {
            get { return _columnFamilyObjects; }
            set { _columnFamilyObjects = value; }
        }

        public int ColumnfamilyCount { get; set; }
    }

    #endregion
}
