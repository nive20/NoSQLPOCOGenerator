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

using Microsoft.VisualStudio.TestTools.UnitTesting;  
using POCOGenerator.DatabaseObjects;

namespace POCOGenerator.Cassandra.UnitTest
{
    [TestClass]
    public class ConnectionToClusterTest
    {
        readonly ConnectionToCluster _conn = new ConnectionToCluster();

        [TestMethod]
        public void Check_Get_Connection_Object__When_Correct_Cluster_Name_Is_Provided()
        {

            //Checking connection With Wrong Cluster Name
            const bool expected = true;

            var actual = _conn.GetConnectionObject("cassandra", "cassandra", "Test Cluster");
            Assert.AreEqual(expected,actual);
        }

        [TestMethod]
        public void Check_Get_Connection_Object__When_InCorrect_Cluster_Name_Is_Provided()
        {
            //Checking connection With Correct Cluster Name
            const bool expected = false;
            
            var actual = _conn.GetConnectionObject("cassandra", "cassandra", "Test Cassandra");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Check_Available_KeyspaceName_If_Available_UnitTest()
        {
            //Checking if the specified keyspace exists in database keyspaces. (Available)
            const bool expected = true;
            var keyspaceCollection = _conn.FetchKeyspacesfromCluster();
            var parentItem = new PocoObjectListForExport {NamespaceName = "test"};
            var actual = _conn.CheckAvailableKeyspaceName(keyspaceCollection,parentItem);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Check_Available_KeyspaceName_If_Not_Available_UnitTest()
        {
            //Checking if the specified keyspace exists in database keyspaces. (Not Available)
            const bool expected = false;
            var keyspaceCollection = _conn.FetchKeyspacesfromCluster();
            var parentItem = new PocoObjectListForExport {NamespaceName = "ManageTestClass"};
            var actual = _conn.CheckAvailableKeyspaceName(keyspaceCollection, parentItem);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Check_Available_ColumnFamilyNames_If_Available_UnitTest()
        {
            //Checking if the specified columnFamilyName exists in database keyspaces. ( Available)
            const bool expected = false;
            var parentItem = new PocoObjectListForExport {NamespaceName = "test", ClassName = "blog"};
            var actual = _conn.CheckAvailableColumnFamilyNames(parentItem);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Check_Available_ColumnFamilyNames_If_Not_Available_UnitTest()
        {
            //Checking if the specified columnFamilyName exists in database keyspaces. (Not Available)
            const bool expected = true;
            var parentItem = new PocoObjectListForExport {NamespaceName = "test", ClassName = "TestNotAvailable"};
            var actual = _conn.CheckAvailableColumnFamilyNames(parentItem);
            Assert.AreEqual(expected, actual);
        }

    }
}
