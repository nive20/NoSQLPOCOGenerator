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
using System.Text;
using POCOGenerator.DatabaseObjects;
  
namespace POCOGenerator.Cassandra
{
    public class DbWriter
    {
        #region Execute Data To Cassandra DB

        /// <summary>
        /// This functions executes query to Cassandra DB TO Save Data .
        /// </summary>
        /// <param name="exportList"></param>
        /// <returns>Returns bool</returns>
        public bool SaveDataToDb(List<PocoObjectListForExport> exportList)
        {
            var conn = new ConnectionToCluster();
            var isAvailableKeyspaceNdColumnFamily = false;
            var keyspaceCollection = conn.FetchKeyspacesfromCluster();
            foreach (var parentItem in exportList)
            {
                var stringBuilder = new StringBuilder();
                isAvailableKeyspaceNdColumnFamily = conn.CheckAvailableKeyspaceName(keyspaceCollection, parentItem);
                if (!isAvailableKeyspaceNdColumnFamily) continue;

                isAvailableKeyspaceNdColumnFamily = conn.CheckAvailableColumnFamilyNames(parentItem);
                if (isAvailableKeyspaceNdColumnFamily)
                {
                    var tempScript = Reader.ReadPerScriptFromExportList(parentItem);
                    stringBuilder.AppendLine(tempScript);
                    string selectfromstorebyconsumer = stringBuilder.ToString().ToLower();
                    try
                    {
                        conn.ExceuteDyamicQueriesToCassandraDb(selectfromstorebyconsumer);
                    }
                    catch (Exception)
                    {
                        return isAvailableKeyspaceNdColumnFamily;
                    }
                }
                break;
            }
            return isAvailableKeyspaceNdColumnFamily;
        }
        
        #endregion
    }
}
