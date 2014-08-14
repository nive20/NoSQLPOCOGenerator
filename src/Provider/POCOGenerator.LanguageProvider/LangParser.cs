/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  


 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using System;
using System.Collections.Generic;
using System.Text;
using POCOGenerator.DatabaseObjects;


namespace POCOGenerator.LanguageProvider
{
    public static class LangParser
    {

        /// <summary>
        /// This function Converts fetched Data as List Item
        /// </summary>
        /// <param name="keyspaceName"></param>
        /// <param name="columnFamilyName"></param>
        /// <param name="pocoChildList"></param>
        /// <returns>Returns List Item</returns>
        public static PocoObjectListForExport ConvertFetchedDataToExportFormatInPocoToDb(string keyspaceName, string columnFamilyName, List<PocoListofObjects> pocoChildList)
        {

            var exportPocoItem = new PocoObjectListForExport
            {
                ClassName = columnFamilyName,
                NamespaceName = keyspaceName,
                PocoListObjects = pocoChildList
            };
            return exportPocoItem;
        }

        /// <summary>
        /// This function generates CQL Script from List 
        /// </summary>
        /// <param name="exportPocoList"></param>
        /// <returns>Returns CQL Script</returns>
        public static string CreateCqlScriptFromList(List<PocoObjectListForExport> exportPocoList)
        {
            var key = string.Empty;
            var stringBuilder = new StringBuilder();
            foreach (var parentItem in exportPocoList)
            {
                var tempScript = ExtractPerScriptFromExportList(parentItem, key);
                stringBuilder.AppendLine(tempScript);
            }
            return stringBuilder.ToString().TrimEnd(',');
        }

        /// <summary>
        /// This function generates Per Object CQL Script
        /// </summary>
        /// <param name="parentItem"></param>
        /// <param name="key"></param>
        /// <returns>Returns CQL Script</returns>
        private static string ExtractPerScriptFromExportList(PocoObjectListForExport parentItem, string key)
        {
            if (key == null) throw new ArgumentNullException("key");
            var stringBuilder = new StringBuilder();
            key = parentItem.PocoListObjects[0].ColumnFamilyName;
            stringBuilder.AppendLine("create table " + parentItem.NamespaceName + "." + parentItem.ClassName);
            stringBuilder.AppendLine("(");
            foreach (var childItem in parentItem.PocoListObjects)
                stringBuilder.AppendLine(childItem.ColumnFamilyName + " " + childItem.Type + ",");
            stringBuilder.Append("PRIMARY KEY(" + key + ") \n");
            stringBuilder.AppendLine(")");
            return stringBuilder.ToString();
        }
    }
}
