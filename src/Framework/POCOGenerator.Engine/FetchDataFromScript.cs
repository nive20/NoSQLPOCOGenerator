/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using System.Collections.Generic;  
using System.Globalization;
using System.Linq;
using POCOGenerator.Cassandra;
using POCOGenerator.LanguageProvider;

namespace POCOGenerator.Engine
{
   public class FetchDataFromScript
    {

       #region global Variables  

       //Assigned for keeping the StartIndex After Previous Data Fetch

        int _startindex; 

        #endregion


       #region Fetch Data As Keyspace Name, Column Family Name , Column Details

       /// <summary>
       /// This function Fetches Data From All Kind Of Scripts
       /// </summary>
       /// <param name="mainString"></param>
       /// <param name="firstDelimiter"></param>
       /// <param name="secondDelimiter"></param>
       /// <returns>Returns Fetched Data as string</returns>
        public string FetchData(string mainString, string firstDelimiter, string secondDelimiter)
        {
            int startPos; int length;
            if (!mainString.Contains(firstDelimiter))
                return string.Empty;
            if (firstDelimiter.Equals("{"))
            { //poco columndetails
                startPos = mainString.IndexOf(firstDelimiter, _startindex, System.StringComparison.Ordinal) + firstDelimiter.Length;
                length = mainString.LastIndexOf(secondDelimiter, System.StringComparison.Ordinal) - startPos;
            }
            else if (secondDelimiter.Equals("(") || firstDelimiter.Equals("("))
            {//CQL
                startPos = mainString.IndexOf(firstDelimiter, System.StringComparison.Ordinal) + firstDelimiter.Length;
                length = mainString.IndexOf(secondDelimiter, startPos, System.StringComparison.Ordinal) - startPos;
            }
            else
            {//CLI  //Any POCO Script
                startPos = mainString.IndexOf(firstDelimiter, System.StringComparison.Ordinal) + firstDelimiter.Length + 1;
                length = mainString.IndexOf(secondDelimiter, startPos, System.StringComparison.Ordinal) - startPos;
            }
            mainString = mainString.Substring(startPos, length);
            mainString = mainString.TrimEnd('\r', '\n', '\'', ' ', '{', '}');
            mainString = mainString.TrimStart('\r', '\n', '\'', ' ', '}', '{');
            _startindex = startPos;
            return mainString;

        } 

        #endregion


       #region Parse ColumnName and DataType for Both POCO TO DB and Vice-Versa

       /// <summary>
       /// This function Parse ColumnNames and DataTypes From CQL/CLI Script.
       /// </summary>
       /// <param name="columnDetails"></param>
       /// <param name="isCli"></param>
       /// <param name="errorMessage"></param>
       /// <param name="isValidColumnFamily"></param>
       /// <returns>Returns Dictionary of Column Name and Type</returns>
        internal Dictionary<string, string> ParseColumnNameAndDataType(string columnDetails, bool isCli, ref string errorMessage, ref bool isValidColumnFamily)
        {
            var columnsAndDataType = new Dictionary<string, string>();
           columnDetails = columnDetails.TrimEnd(',', ' ');
           var columns = columnDetails.Split(isCli ? new[] { '}' } : new[] { ',' });
           foreach (var column in columns)
           {
               var columnData = column;
               columnData = columnData.TrimEnd('\r', '\n', ' ', ',', '}');
               columnData = columnData.TrimStart('\r', '\n', ' ', ',', '{');
               columnData = columnData.Trim('\r', '\n', '[', ']');
               if (string.IsNullOrEmpty(columnData))
                   break;
               string columnName;
               string dataType;
               string[] columnValues;
               if (isCli)
               {
                   columnValues = columnData.Split(new[] {','});
                   columnName =
                       columnValues[0].Split(new[] {':'})[1].TrimEnd('\r', '\n', ',', ' ', '\'')
                           .TrimStart(' ', '\'');
                   dataType =
                       columnValues[1].Split(new[] {':'})[1].TrimEnd('\r', '\n', ',', ' ', '\'')
                           .TrimStart(' ', '\'');
               }
               else
               {
                   columnValues = columnData.Split(new[] {' '});
                   columnName = columnValues[0].ToString(CultureInfo.InvariantCulture);
                   dataType = columnValues[1].ToString(CultureInfo.InvariantCulture);
               }
               isValidColumnFamily = Validator.ValidateColumnsAndDataType(ref errorMessage, columnName, dataType);
               if (!isValidColumnFamily)
                   break;
               columnsAndDataType.Add(columnName, dataType);
           }
           return columnsAndDataType;

        }

       /// <summary>
       /// This function parse column Name and Type for CSharp Script.
       /// </summary>
       /// <param name="columnDetails"></param>
       /// <param name="errorMessage"></param>
       /// <param name="isValidScript"></param>
       /// <returns>Returns Dictionary of ColumnName and Type</returns>
       internal Dictionary<string, string> ParseColumnNameAndDataTypeForCSharpScript(string columnDetails, ref string errorMessage, ref bool isValidScript)
        {
            //POCO TO DB  CSharp Script
            var columnsAndDataType = new Dictionary<string, string>();
           var columns = columnDetails.Split(new[] { '}' });
            foreach (var column in columns)
            {
                var columnData = column;

                columnData = columnData.Replace("public", "");
                columnData = columnData.Replace("get", "");
                columnData = columnData.Replace("set", "");
                columnData = columnData.Trim('\r', '\n', ' ', ',', '{', '}', ';');
                if (string.IsNullOrEmpty(columnData))
                    break;
                var columnValues = columnData.Split(' ');
                columnValues = columnValues.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                var dataType = columnValues[0].ToString(CultureInfo.InvariantCulture);
                var columnName = columnValues[1].ToString(CultureInfo.InvariantCulture);

                isValidScript = LangValidator.ValidateColumnsAndDataType(ref errorMessage, columnName, dataType);
                columnsAndDataType.Add(columnName, dataType);
            }
            return columnsAndDataType;
        }

       /// <summary>
       /// This function parse column Name and Type for VB Script.
       /// </summary>
       /// <param name="columnDetails"></param>
       /// <param name="errorMessage"></param>
       /// <param name="isValidScript"></param>
       /// <returns>Returns Dictionary of ColumnName and Type</returns>
       internal Dictionary<string, string> ParseColumnNameAndDataTypeForVbScript(string columnDetails, ref string errorMessage, ref bool isValidScript)
        {
            var columnsAndDataType = new Dictionary<string, string>();
            var columns = columnDetails.Split(new[] { '\r' }).Where(x => x.Contains("property")).ToArray().Where(y => !y.Contains("end")).ToArray();
            foreach (var column in columns)
            {
                var columnData = column;
                columnData = columnData.Replace("public", "");
                columnData = columnData.Replace("property", "");
                columnData = columnData.Replace("as", "");
                columnData = columnData.Replace("()", "");

                columnData = columnData.Trim('\r', '\n', ' ', ',', '{', '}', ';');
                if (string.IsNullOrEmpty(columnData))
                    continue;
                var columnValues = columnData.Split(' ');
                columnValues = columnValues.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                var columnName = columnValues[0].ToString(CultureInfo.InvariantCulture);
                var dataType = columnValues[1].ToString(CultureInfo.InvariantCulture);

                isValidScript = LangValidator.ValidateColumnsAndDataType(ref errorMessage, columnName, dataType);
                columnsAndDataType.Add(columnName, dataType);
            }
            return columnsAndDataType;
        }

       #endregion

    }
}
