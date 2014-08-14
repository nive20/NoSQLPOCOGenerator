/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using System;
using System.Collections.Generic;

[assembly:CLSCompliant(true)]
namespace POCOGenerator.DatabaseObjects
{

    #region ExportAndMappingType


    public class ColumnFamilyMetadata
    {
        public string Columnfamilyname;
        public string ColumnName;
        public string Validator;
        public string Type;

    }

    
    public class PocoObjectListForExport
    {
        private List<PocoListofObjects> _pocoListObjects = new List<PocoListofObjects>();
        public string NamespaceName { get; set; }
        public string ClassName { get; set; }
        public List<PocoListofObjects> PocoListObjects
        {
            get { return _pocoListObjects; }
            set { _pocoListObjects = value; }
        }
    }

    public class PocoListofObjects
    {
        public string Type { get; set; }
        public string ColumnFamilyName { get; set; }
    }

    public class PocoObjectsMetadata
    {
        public string NamespaceName;
        public string ClassName;
        public Dictionary<string, string> PropertyDetail = new Dictionary<string, string>();
        public Dictionary<string, string> Key = new Dictionary<string, string>();

    }

    public class CassandraCSharpDataType
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class KeyspacesName
    {
        public string Parent
        {
            get;
            set;
        }

        public List<ColumnFamilyName> Children
        {
            get;
            set;
        }


        public bool? Ischecked
        {
            get;
            set;
        }

    }

    public class TableNames
    {
        public string Keyspacename;
    }

    public class ColumnFamilyName
    {
        private List<ColumnFamilyMetadata> _childMetadata = new List<ColumnFamilyMetadata>();
        public string Columnfamilyname { get; set; }

        public List<ColumnFamilyMetadata> ChildMetadata
        {
            get { return _childMetadata; }
            set { _childMetadata = value.Equals(null)? new List<ColumnFamilyMetadata>(): value; }
        }

        public bool? Ischecked
        {
            get;
            set;
        }
    }


    #endregion

    

}
