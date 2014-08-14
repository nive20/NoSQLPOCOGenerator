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
using POCOGenerator.Engine;

namespace POCOGenerator.Cassandra.UnitTest
{
    [TestClass]
    public class FetchDataFromScriptUnitTest
    {

        static readonly FetchDataFromScript Fetch = new FetchDataFromScript();

        private const string DbToPocoScriptCli = "CREATE COLUMN FAMILY account \n " + " WITH comparator = UTF8Type \n" + "AND key_validation_class=UTF8Type \n" + 
            " AND column_metadata = [ \n " + " {column_name: title, validation_class: UTF8Type} \n " + "{column_name: created, validation_class: DateType} \n " +
            "{column_name: use_namespace, validation_class: BooleanType} \n " + "]; ";

        private const string DbToPocoScriptCql = "CREATE TABLE timeline (" + "userid uuid," + "posted_month int," + "posted_time uuid," + "body text," + "posted_by text,"
            + "PRIMARY KEY (userid) ) ";


        private const string PocoToDbScript = "using System; \n" + "namespace  test { \n" +
            " public class testClass { \n" + " public string    username  { get;  set; } \n"
            + "public string    email  { get;  set; } \n" + " public int    phno  { get;  set; }  } } ";

        [TestMethod]
        public void Fetch_Data_ColumnFamilyName_DBToPOCO_CLI_UnitTest()
        {

            //Fetch_Data_ColumnFamilyName_DBToPOCO_CLI_UnitTest
            const string createColumnFamily = "create column family";
            const string withCompartor = "with comparator";
            const string expected = "account";
            var actual = Fetch.FetchData(DbToPocoScriptCli.ToLower(), createColumnFamily, withCompartor);
            Assert.AreEqual(expected, actual);
        }

        
        [TestMethod]
        public void Fetch_Data_key_DataType_DBToPOCO_CLI_UnitTest()
        {
            //Fetch_Data_key_DataType_DBToPOCO_CLI_UnitTest
            const string keyValidationClass = "key_validation_class";
            const string columnMetaData = "and column_metadata";
            const string expected = "utf8type";
            var actual = Fetch.FetchData(DbToPocoScriptCli.ToLower(), keyValidationClass, columnMetaData);
            Assert.AreEqual(expected, actual);
        }

        
        [TestMethod]
        public void Fetch_Data_columnDetails_DBToPOCO_CLI_UnitTest()
        {
            //Fetch_Data_columnDetails_DBToPOCO_CLI_UnitTest
            const string expected = "column_name: title, validation_class: utf8type} \n"+
                                    " {column_name: created, validation_class: datetype} \n"+
                                    " {column_name: use_namespace, validation_class: booleantype";

            var actual = Fetch.FetchData(DbToPocoScriptCli.ToLower(), "[", "]");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Fetch_Data_ColumnFamilyName_DBToPOCO_CQL_UnitTest()
        {

            //Fetch_Data_ColumnFamilyName_DBToPOCO_CQL_UnitTest
            string createColumnFamily = "create table if not exists";
            const string createColumnFamilyTable = "create table";
            const string expected = "timeline";
            if (!DbToPocoScriptCql.Contains(createColumnFamily))
                createColumnFamily = createColumnFamilyTable;
            var actual = Fetch.FetchData(DbToPocoScriptCql.ToLower(), createColumnFamily, "(");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Fetch_Data_columnDetails_DBToPOCO_CQL_UnitTest()
        {
            //Fetch_Data_columnDetails_DBToPOCO_CQL_UnitTest
            const string expected = "userid uuid,posted_month int,posted_time uuid,body text,posted_by text,primary key (userid";

            var actual = Fetch.FetchData(DbToPocoScriptCql.ToLower(), "(", ")");
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Fetch_Data_KeyspaceName_POCOToDB_UnitTest()
        {
            //Fetch_Data_KeyspaceName_POCOToDB_UnitTest
            const string expected = "test";
            var actual = Fetch.FetchData(PocoToDbScript.ToLower(), "namespace", "{");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Fetch_Data_ColumnFamily_Name_POCOToDB_UnitTest()
        {

            //Fetch_Data_ColumnFamily_Name_POCOToDB_UnitTest
            const string expected = "testclass";
            var actual = Fetch.FetchData(PocoToDbScript.ToLower(), "public class", "{");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Fetch_Data_ColumnDetails_POCOToDB_UnitTest()
        {
            //Fetch_Data_ColumnDetails_POCOToDB_UnitTest
            const string expected = "public string    username  { get;  set; } \n" +
                                    "public string    email  { get;  set; } \n" +
                                    " public int    phno  { get;  set;";
            var actual = Fetch.FetchData(PocoToDbScript.ToLower(), "{", "}");
            Assert.AreEqual(expected, actual);
        }

    }
}
