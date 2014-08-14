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

using System.Collections.Generic;  
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOGenerator.DatabaseObjects;
using POCOGenerator.Engine;

namespace POCOGenerator.Cassandra.UnitTest
{
    [TestClass]
    public class SummaryUnitTest
    {
        [TestMethod]
        public void GenerateSummary_If_List_Provided()
        {
            const string expected = "Total Number of keyspaces : 1\r\n\r\nKeyspace Name  : test\r\nColumnFamily Count : 1\r\nColumnFamily Name : CreateTestClass\r\n\r\n";
            var exportList = new List<PocoObjectListForExport>();
            var exportItem = new PocoObjectListForExport {NamespaceName = "test", ClassName = "CreateTestClass"};
            var pocoObjItem = new PocoListofObjects {Type = "varchar", ColumnFamilyName = "idname"};
            exportItem.PocoListObjects.Add(pocoObjItem);
            exportList.Add(exportItem);
            ScriptReader.ReinitializeExportList();
            var actual = ListSummary.GenerateSummary(exportList);
            Assert.AreEqual(expected,actual);
        }

        [TestMethod]
        public void GenerateSummary_If_List_Not_Provided()
        {
            const string expected = "Unable To Generate Summary.";
            ScriptReader.ReinitializeExportList();
            var actual = ListSummary.GenerateSummary(null);
            Assert.AreEqual(expected, actual);
        }
    }
}
