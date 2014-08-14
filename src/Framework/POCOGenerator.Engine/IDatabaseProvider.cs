/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using System.Collections.Generic;
using POCOGenerator.DatabaseObjects;

namespace POCOGenerator.Engine
{
    interface IDatabaseProvider
    {

        #region Connection To Database

        /// <summary>
        /// This Function Connects to Database as DBType Provided.
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns>Returns isConnected</returns>
        bool ConnectToDatabase(DatabaseType dbType);

        #endregion

        #region  Fetch Database Metadata
        /// <summary>
        /// This function fetches Database Metadata.
        /// </summary>
        /// <returns>Returns Metadata as List</returns>
        List<KeyspacesName> ReadKeyspaceNames();

        #endregion

        #region Save Data to Database
        /// <summary>
        /// This function saves DataList To DB
        /// </summary>
        /// <param name="exportMetadataList"></param>
        /// <param name="isKeyspaceExists"></param>
        /// <returns>Returns IsSaved</returns>
        bool SaveDataToDb(List<PocoObjectListForExport> exportMetadataList, ref bool isKeyspaceExists); 

        #endregion

      }
}
