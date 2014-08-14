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

namespace POCOGenerator.Cassandra
{
    public static  class Validator
    {
        static  bool _isValidColumnFamily = true;

        #region DB To Poco Script Validation

        /// <summary>
        /// This function Validate the Column and its DataType
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="columnName"></param>
        /// <param name="dataType"></param>
        /// <returns>Returns bool</returns>
        public static bool ValidateColumnsAndDataType(ref string errorMessage,string columnName, string dataType)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(dataType))
            {
                errorMessage = "Column names / DataType is not defined properly. Please verify the script.";
                _isValidColumnFamily = false;
            }

            return _isValidColumnFamily;
        }

        /// <summary>
        /// This function Validates CLI Script
        /// </summary>
        /// <param name="columnFamilyDefinitions"></param>
        /// <param name="errorMessage"></param>
        /// <param name="createColumnFamily"></param>
        /// <returns>Returns bool</returns>
        public static bool ValidateCliScript(string columnFamilyDefinitions, ref string errorMessage, string createColumnFamily)
        {
            if (string.IsNullOrEmpty(columnFamilyDefinitions))
            {
                errorMessage = "Please provide the Create Column Family definition on the left panel window";
                _isValidColumnFamily = false;
            }

            if (columnFamilyDefinitions != null && !columnFamilyDefinitions.StartsWith(createColumnFamily))
            {
                errorMessage = "CLI Definition doesn't start with create column family keyword";
                _isValidColumnFamily = false;
            }
            return _isValidColumnFamily;
        }

        /// <summary>
        /// This function validates CQL Script
        /// </summary>
        /// <param name="columnFamilyDefinitions"></param>
        /// <param name="errorMessage"></param>
        /// <param name="createColumnFamily"></param>
        /// <param name="createColumnFamilyTable"></param>
        /// <returns>Returns bool</returns>
        public static bool ValidateCqlScript(string columnFamilyDefinitions, ref string errorMessage, ref string createColumnFamily, string createColumnFamilyTable)
        {
            if (string.IsNullOrEmpty(columnFamilyDefinitions))
            {
                errorMessage = "Please provide the Create Column Family definition on the left panel window";
                _isValidColumnFamily = false;
            }

            if (columnFamilyDefinitions != null && !columnFamilyDefinitions.StartsWith(createColumnFamily))
            {
                if (columnFamilyDefinitions.StartsWith(createColumnFamilyTable))
                    createColumnFamily = createColumnFamilyTable;
                else
                {
                    errorMessage = "CQL Definition doesn't start with CREATE TABLE keyword";
                    _isValidColumnFamily = false;
                }
            }
            return _isValidColumnFamily;
        }

        /// <summary>
        /// This function validates column details of CLI Script
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="columnFamilyName"></param>
        /// <param name="keyDataType"></param>
        /// <returns>Returns bool</returns>
        public static bool ValidateCliScriptsColumnDetails( ref string errorMessage, string columnFamilyName, string keyDataType)
        {
            if (string.IsNullOrEmpty(columnFamilyName))
            {
                errorMessage = "Not able to identify the column family name.  Please verify the script.";
                _isValidColumnFamily = false;
            }


            if (string.IsNullOrEmpty(keyDataType))
            {
                errorMessage = "Key validation class is missing.  Please verify the script.";
                _isValidColumnFamily = false;
            }


            if (string.IsNullOrEmpty(keyDataType))
            {
                errorMessage = "Column details are missing.  Please verify the script.";
                _isValidColumnFamily = false;
            }
            return _isValidColumnFamily;
        }

        /// <summary>
        /// This function validates columnDetails for CQL Script
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="columnFamilyName"></param>
        /// <param name="columnDetails"></param>
        /// <returns>Returns bool</returns>
        public static bool ValidateCqlScriptsColumnDetails(ref string errorMessage, string columnFamilyName, string columnDetails)
        {
            if (string.IsNullOrEmpty(columnFamilyName))
            {
                errorMessage = "Not able to identify the column family name.  Please verify the script.";
                _isValidColumnFamily = false;
            }
            if (string.IsNullOrEmpty(columnDetails))
            {
                errorMessage = "Column details are missing.  Please verify the script.";
                _isValidColumnFamily = false;
            }
            return _isValidColumnFamily;
        }

        #endregion DB To Poco Script Validation

    }

}
