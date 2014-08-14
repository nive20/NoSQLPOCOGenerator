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
using System.Globalization;
using System.Text;
using POCOGenerator.DatabaseObjects;

namespace POCOGenerator.Cassandra
{
    public static class Reader
    {

        #region Global Varibales

        static public Dictionary<string, string> Key = new Dictionary<string, string>();

        #endregion

        #region Generate List for ColumnFamily Metadata.
        /// <summary>
        /// This functions creates List for ColumnFamily Metadata
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns>Returns List</returns>
        public static List<PocoObjectListForExport> GenerateListForColumnFamilyMetdata(string parent, ColumnFamilyName child)
        {
            var exportPocoList = new List<PocoObjectListForExport>();
            var primarykeylist = string.Empty;
            Key = new Dictionary<string, string>();

            foreach (var data in child.ChildMetadata)
            {
                if (String.IsNullOrEmpty(data.Type)) continue;

                if (!data.Type.Equals("partition_key") && !data.Type.Equals("clustering_key")) continue;

                Key.Add(data.ColumnName, data.Validator);
                primarykeylist = String.IsNullOrEmpty(primarykeylist) ? data.ColumnName.ToString(CultureInfo.InvariantCulture) : 
                    String.Concat(primarykeylist, ", ", data.ColumnName.ToString(CultureInfo.InvariantCulture));
            }
            exportPocoList.AddRange(Parser.GenratePocoObjects(parent, child, Key));
            return exportPocoList;
        }

        #endregion

        #region Cassandra Script generated from POCO

        /// <summary>
        /// This function read data per keyspace.
        /// </summary>
        /// <param name="parentItem"></param>
        /// <returns>Returns script created per keyspace</returns>
        public static string ReadPerScriptFromExportList(PocoObjectListForExport parentItem)
        {
            var stringBuilder = new StringBuilder();
            var columnFamilyName = parentItem.PocoListObjects[0].ColumnFamilyName;
            stringBuilder.AppendLine("create table " + parentItem.NamespaceName + "." + parentItem.ClassName);
            stringBuilder.AppendLine("(");
            foreach (var childItem in parentItem.PocoListObjects)
                stringBuilder.AppendLine(childItem.ColumnFamilyName + " " + childItem.Type + ",");
            stringBuilder.Append("PRIMARY KEY(" + columnFamilyName + ") \n");
            stringBuilder.AppendLine(")");
            return stringBuilder.ToString();
        }

        #endregion

    }
}
