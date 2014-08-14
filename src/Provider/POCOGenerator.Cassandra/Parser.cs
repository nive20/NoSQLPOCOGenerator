/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Uses Cassandrashrp library from https://www.nuget.org/packages/cassandra-sharp/
 * Can be used with Cassandra NOSQL Database

 * <Place holder> for other NOSQL database driver.
 * <Place holder> for other NOSQL database.

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using System;
using System.Collections.Generic;
using System.Linq;
using POCOGenerator.DatabaseObjects;


namespace POCOGenerator.Cassandra
{

    public class Parser
    {

        #region Global Variables

        static readonly string KeyspaceName = string.Empty;
        static Mapper _initializeMappings;

        #endregion

        #region Convert To Export Format List

        /// <summary>
        /// Convert Fetched Data to List format.
        /// </summary>
        /// <param name="nmspcenameName"></param>
        /// <param name="columnFamilyName"></param>
        /// <param name="keyDataType"></param>
        /// <param name="columnsAndDataType"></param>
        /// <param name="isCqlScript"></param>
        /// <returns>Returns List</returns>
        public static PocoObjectListForExport ConvertGeneratedMetaDataToListFormat(string nmspcenameName,string columnFamilyName,string keyDataType,
            Dictionary<string,string> columnsAndDataType, bool isCqlScript)
        {
            
            _initializeMappings = new Mapper();
            _initializeMappings.InitializeDataTypeMappings();

            var pocoChildList = new List<PocoListofObjects>();
            var exportPocoItem = new PocoObjectListForExport
            {
                ClassName = columnFamilyName,
                NamespaceName = String.IsNullOrEmpty(nmspcenameName) ? KeyspaceName : nmspcenameName
            };

           foreach (var columnName in columnsAndDataType.Keys)
            {
                var pocoChildItem = new PocoListofObjects
                {
                    ColumnFamilyName = columnName,
                    Type =
                        isCqlScript
                            ? _initializeMappings.DataTypes[String.Concat(columnsAndDataType[columnName], "type")]
                            : _initializeMappings.DataTypes[columnsAndDataType[columnName]]
                };
                pocoChildList.Add(pocoChildItem);
            }
            exportPocoItem.PocoListObjects = pocoChildList;
            return exportPocoItem;
        }

        /// <summary>
        /// This function converts Metadata to List.
        /// </summary>
        /// <param name="columnsAndDataType"></param>
        /// <returns>Returns Metadata List</returns>
        public static List<PocoListofObjects> PocoObjectListMetadata(Dictionary<string,string> columnsAndDataType)
        {
            if (columnsAndDataType == null) throw new ArgumentNullException("columnsAndDataType");

            _initializeMappings = new Mapper();
            _initializeMappings.InitializeDataTypeMappings();
            var pocoChildList = new List<PocoListofObjects>();
            foreach (var columnData in columnsAndDataType)
            {
                var pocoChildItem = new PocoListofObjects {ColumnFamilyName = columnData.Key};
                //string type = initializeMappings.DataTypes.Where(kvp => kvp.Value.ToLower() == columnData.Value.ToLower()).Select(kvp => kvp.Key).FirstOrDefault();
                var type = _initializeMappings.DataTypes.Where(kvp => String.Equals(kvp.Value, columnData.Value, StringComparison.CurrentCultureIgnoreCase))
                    .Select(kvp => kvp.Key).FirstOrDefault();
                if (type != null) pocoChildItem.Type = type.Remove(type.IndexOf("type", StringComparison.Ordinal));
                pocoChildList.Add(pocoChildItem);
            }
            return pocoChildList;
        }

        #endregion Convert To Export Format List

        #region Generate POCO Objects List

        /// <summary>
        /// This function generate PocoObjects List
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <param name="key"></param>
        /// <returns>Returns Final List</returns>
        internal static List<PocoObjectListForExport> GenratePocoObjects(string parent, ColumnFamilyName child, Dictionary<string, string> key)
        {
            var pocoList = new List<PocoObjectListForExport>();
            var pocoObjectsMetadataDetails = new PocoObjectsMetadata
            {
                NamespaceName = parent,
                ClassName = child.Columnfamilyname,
                Key = key
            };
            // Fetch Namespace name via keyspace name
            var isValidColumnFamily = AssignColumnFamilyDefinitionsMetadata(ref pocoObjectsMetadataDetails, child, key);
            if (!isValidColumnFamily)
            {
                return pocoList;
            }
            pocoList = AppendCSharpPocoClassMetadata(parent, pocoObjectsMetadataDetails);
            return pocoList;

        }

        /// <summary>
        /// This function creates List by Appending Metadata.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="pocoObjectsMetadataDetails"></param>
        /// <returns>Return List</returns>
        private static List<PocoObjectListForExport> AppendCSharpPocoClassMetadata(string parent,
                                PocoObjectsMetadata pocoObjectsMetadataDetails)
        {
            var exportPocoList = new List<PocoObjectListForExport>();
            _initializeMappings = new Mapper();
            _initializeMappings.InitializeDataTypeMappings();
            var listitem = new PocoObjectListForExport {ClassName = pocoObjectsMetadataDetails.ClassName};
            foreach (var detail in pocoObjectsMetadataDetails.PropertyDetail)
            {
                listitem.NamespaceName = parent;
                listitem.PocoListObjects.Add(new PocoListofObjects { ColumnFamilyName = detail.Key, Type = _initializeMappings.DataTypes[detail.Value] });

            }
            exportPocoList.Add(listitem);
            return exportPocoList;
        }

        /// <summary>
        /// This function assign values to Metadata .
        /// </summary>
        /// <param name="pocoObjectsMetadataDetails"></param>
        /// <param name="child"></param>
        /// <param name="key"></param>
        /// <returns>Returns bool</returns>
        private static bool AssignColumnFamilyDefinitionsMetadata(ref PocoObjectsMetadata pocoObjectsMetadataDetails, ColumnFamilyName child, Dictionary<string, string> key)
        {
            // Fetch class name via columnfamily name
            pocoObjectsMetadataDetails.ClassName = child.Columnfamilyname;
            // fetch the key data type
            pocoObjectsMetadataDetails.Key = key;

            foreach (ColumnFamilyMetadata data in child.ChildMetadata)
            {
                pocoObjectsMetadataDetails.PropertyDetail.Add(data.ColumnName, data.Validator);
            }
            const bool isValidColumnFamily = true;
            return isValidColumnFamily;
        }

        #endregion Generate POCO Objects List
    }
}
