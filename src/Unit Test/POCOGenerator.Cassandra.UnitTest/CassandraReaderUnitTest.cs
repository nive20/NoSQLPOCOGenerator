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
    public class CassandraReaderUnitTest
    {
        [TestMethod]
        public void Generate_List_For_CreateColumnFamilyMetdata_UnitTest()
        {
            const string parents = "test";
            var child=new ColumnFamilyName {Columnfamilyname = "account", Ischecked = true};
            var metadata = new ColumnFamilyMetadata
            {
                ColumnName = "id",
                Columnfamilyname = "user",
                Type = null,
                Validator = "utf8type"
            };
            child.ChildMetadata.Add(metadata);

            var exportList = new List<PocoObjectListForExport>();
            var parent = new PocoObjectListForExport {NamespaceName = "test", ClassName = "account"};
            var firstChild = new PocoListofObjects {Type = "string", ColumnFamilyName = "id"};
            parent.PocoListObjects.Add(firstChild);
            exportList.Add(parent);
            var expected = exportList;

            var actual = Reader.GenerateListForColumnFamilyMetdata(parents, child);

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);
            Assert.AreEqual(expected[0].ClassName, actual[0].ClassName);
            Assert.AreEqual(expected[0].NamespaceName, actual[0].NamespaceName);
            Assert.IsTrue(actual[0].PocoListObjects.Count > 0);
            for (var i = 0; i < actual[0].PocoListObjects.Count; i++)
            {
                Assert.AreEqual(expected[0].PocoListObjects[i].Type, actual[0].PocoListObjects[i].Type);
                Assert.AreEqual(expected[0].PocoListObjects[i].ColumnFamilyName, actual[0].PocoListObjects[i].ColumnFamilyName);
            }
        }

        [TestMethod]
        public void Read_Per_Script_From_ExportList_UnitTest()
        {
            ScriptReader.ReinitializeExportList();
            var parent = new PocoObjectListForExport {NamespaceName = "test", ClassName = "account"};
            var child = new PocoListofObjects {Type = "string", ColumnFamilyName = "UserName"};
            parent.PocoListObjects.Add(child);
            const string expected = "create table test.account\r\n(\r\nUserName string,\r\nPRIMARY KEY(UserName) \n)\r\n";
            var actual = Reader.ReadPerScriptFromExportList(parent);
            Assert.AreEqual(expected, actual);
        }
    }
}
