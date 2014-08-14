/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POCOGenerator.DatabaseObjects;


namespace POCOGenerator.LanguageProvider.UnitTest
{
    [TestClass]
    public class LangParserUnitTest
    {  
        [TestMethod]
        public void Convert_Fetched_Data_To_Export_Format_In_PocoToDB()
        {
            const string keyspaceName = "test";
            const string columnFamilyName = "account";
            var pocoChildList = new List<PocoListofObjects>();
            var pocoChildItem = new PocoListofObjects {Type = "string"};
            pocoChildItem.Type = "name";
            pocoChildList.Add(pocoChildItem);

            var exportPocoItem=new PocoObjectListForExport
            {
                NamespaceName = "test",
                ClassName = "account",
                PocoListObjects = pocoChildList
            };
            var expected = exportPocoItem;
            var actual = LangParser.ConvertFetchedDataToExportFormatInPocoToDb(keyspaceName, columnFamilyName, pocoChildList);
            Assert.AreEqual(expected.NamespaceName, actual.NamespaceName);
            Assert.AreEqual(expected.ClassName, actual.ClassName);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.PocoListObjects.Count > 0);
            for (var i = 0; i < actual.PocoListObjects.Count; i++)
            {
                Assert.AreEqual(expected.PocoListObjects[i].Type, actual.PocoListObjects[i].Type);
                Assert.AreEqual(expected.PocoListObjects[i].ColumnFamilyName, actual.PocoListObjects[i].ColumnFamilyName);
            }

        }

        [TestMethod]
        public void CreateDbScriptFromList()
        {
            var pocoChildList = new List<PocoListofObjects>();
            var pocoChildItem = new PocoListofObjects {Type = "string"};
            pocoChildItem.Type = "name";
            pocoChildList.Add(pocoChildItem);
            var exportList = new List<PocoObjectListForExport>();
            var exportPocoItem = new PocoObjectListForExport
            {
                NamespaceName = "test",
                ClassName = "account",
                PocoListObjects = pocoChildList
            };
            exportList.Add(exportPocoItem);
            const string expected = "create table test.account\r\n(\r\n name,\r\nPRIMARY KEY() \n)\r\n\r\n";
            var actual = LangParser.CreateCqlScriptFromList(exportList);
            Assert.AreEqual(expected, actual);

        }


    }
}
