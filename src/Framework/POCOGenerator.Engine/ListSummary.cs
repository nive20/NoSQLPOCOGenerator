/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

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

namespace POCOGenerator.Engine
{
    public static class ListSummary  
    {
        #region Generate Summary Script

        /// <summary>
        /// This function generates Summary of the Task Performed.
        /// </summary>
        /// <param name="pocoObjectList"></param>
        /// <returns>Returns Generated Summary as String</returns>
        public static string GenerateSummary(List<PocoObjectListForExport> pocoObjectList)
        {
            var namespaceName = string.Empty;
            var stringBuilder = new StringBuilder();

            try
            {
                var initializeSummaryDetails = AssignValuesToSummary(pocoObjectList);
                stringBuilder.AppendLine("Total Number of keyspaces : " + initializeSummaryDetails.KeyspaceCount);
                stringBuilder.AppendLine();
                foreach (var item in initializeSummaryDetails.KeyspaceDetails)
                {
                    if (item.KeyspaceName.Equals(namespaceName)) continue;
                    stringBuilder.AppendLine("Keyspace Name  : " + item.KeyspaceName);
                    stringBuilder.AppendLine("ColumnFamily Count : " + item.ColumnfamilyCount.ToString(CultureInfo.InvariantCulture));
                    namespaceName = item.KeyspaceName;
                    foreach (var childItem in item.ColumnFamilyDetails)
                    {
                        stringBuilder.AppendLine("ColumnFamily Name : " + childItem.ClassName);
                    }
                    stringBuilder.AppendLine();
                }
            }
            catch (OutOfMemoryException)
            {

                stringBuilder.Append("Unable To Generate Summary.");
            }
            catch (Exception)
            {

                stringBuilder.Append("Unable To Generate Summary.");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// This function Assign the Values to the Summary Object.
        /// </summary>
        /// <param name="pocoObjectList"></param>
        /// <returns>Returns Object Of Summary Details</returns>
        private static SummaryDetails AssignValuesToSummary(IEnumerable<PocoObjectListForExport> pocoObjectList)
        {
            var initializeSummaryDetails = new SummaryDetails();
            var keyspacesInfo = new List<KeySpaceDetails>();
            KeySpaceDetails keyspaceObject = null;
            List<PocoObjectListForExport> columnfamilyDetails = null;

            var namespce = string.Empty;
            var keyspaceCount = 0;

            foreach (var parentItem in pocoObjectList)
            {
                if (!parentItem.NamespaceName.Equals(namespce))
                {
                    keyspaceObject = new KeySpaceDetails();
                    columnfamilyDetails = new List<PocoObjectListForExport>();
                    namespce = parentItem.NamespaceName;
                    keyspaceObject.KeyspaceName = parentItem.NamespaceName;
                    keyspaceCount = keyspaceCount + 1;
                }
                var columnfamilyObject = new PocoObjectListForExport {ClassName = parentItem.ClassName};
                if (columnfamilyDetails != null)
                {
                    columnfamilyDetails.Add(columnfamilyObject);
                    keyspaceObject.ColumnFamilyDetails = columnfamilyDetails;
                }
                if (keyspaceObject != null)
                {
                    keyspaceObject.ColumnfamilyCount = keyspaceObject.ColumnFamilyDetails.Count;
                    keyspacesInfo.Add(keyspaceObject);
                }
            }


            initializeSummaryDetails.KeyspaceCount = keyspaceCount;
            initializeSummaryDetails.KeyspaceDetails = keyspacesInfo;
            return initializeSummaryDetails;
        } 

        #endregion

    }
}
