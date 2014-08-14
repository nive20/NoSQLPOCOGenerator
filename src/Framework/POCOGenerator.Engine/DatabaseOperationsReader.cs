/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using System.Collections.Generic;
using POCOGenerator.Cassandra;
using POCOGenerator.DatabaseObjects;

namespace POCOGenerator.Engine
{
    public class DatabaseOperationsReader:IDatabaseProvider
    {

        #region authenticationDetails

        static string _clusterNm = string.Empty;
        static string _userNm = string.Empty;
        static string _passwrd = string.Empty;
        readonly ConnectionToCluster _conn = new ConnectionToCluster();

        #endregion

        #region Assign Credentials Of DB

        public void AssignCredentialsToDatabase(string userName,string password,string clusterName)
        {
            _userNm = userName;
            _passwrd = password;
            _clusterNm = clusterName;
        } 

        #endregion

        #region Connection To Database
        /// <summary>
        /// This Function Connects to Database as DBType Provided.
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns>Returns isConnected</returns>
        public bool ConnectToDatabase(DatabaseType dbType)
        {
            var isConnected = false;

            switch (dbType)
            {
                case DatabaseType.Cassandra:
                    var clusterCon = new ConnectionToCluster();
                    isConnected = clusterCon.GetConnectionObject(_userNm, _passwrd, _clusterNm);
                    break;
                case DatabaseType.Mongo:
                    break;
                case DatabaseType.SimpleDb:
                    break;
            }

            return isConnected;
        }

        #endregion

        #region  Fetch Database Metadata
        /// <summary>
        /// This function fetches Database Metadata.
        /// </summary>
        /// <returns>Returns Metadata as List</returns>
        public List<KeyspacesName> ReadKeyspaceNames()
        {
            return _conn.FetchKeyspaceNames();
        }

        #endregion

        #region Save Data to Database
        /// <summary>
        /// This function saves DataList To DB
        /// </summary>
        /// <param name="exportMetadataList"></param>
        /// <param name="isKeyspaceExists"></param>
        /// <returns>Returns IsSaved</returns>
        public bool SaveDataToDb(List<PocoObjectListForExport> exportMetadataList, ref bool isKeyspaceExists)
        {
            var databaseWriter = new DbWriter();
           return databaseWriter.SaveDataToDb(exportMetadataList);
        }

        #endregion

    }
}
