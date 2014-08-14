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
    public  class CSharpWriterUnitTest
    {
        readonly CSharpLangWriter _csharpLang = new CSharpLangWriter();

        [TestMethod]  
        public void ConvertListToCSharpScriptUnitTestForSingleFileOption()
        {
            var exportPocoList = new List<PocoObjectListForExport>() ;
            var exportItem=new PocoObjectListForExport();
            const string namespaceName = "test";
            exportItem.NamespaceName = namespaceName;
            exportItem.ClassName = "CreateTestClass";
            var pocoObjItem = new PocoListofObjects {Type = "varchar", ColumnFamilyName = "idname"};
            exportItem.PocoListObjects.Add(pocoObjItem);
            exportPocoList.Add(exportItem);
            const string expected = "using System\r\nnamespace test\r\n{\r\npublic class CreateTestClass\r\n{\r\npublic varchar    idname  { get;  set; } \n}\r\n\r\n\r\n}\r\n";
            var actual = _csharpLang.Writer(exportPocoList, null, namespaceName);
            Assert.AreEqual(expected,actual);
        }


        [TestMethod]
        public void ConvertListToCSharpScriptUnitTestForFilePerObject()
        {
            var exportPocoList = new List<PocoObjectListForExport>();
            var exportItem = new PocoObjectListForExport();
            const string namespaceName = "test";
            exportItem.NamespaceName = namespaceName;
            exportItem.ClassName = "CreateTestClass";
            var pocoObjItem = new PocoListofObjects {Type = "varchar", ColumnFamilyName = "idname"};
            exportItem.PocoListObjects.Add(pocoObjItem);
            exportPocoList.Add(exportItem);
            const string expected = "using System\r\nnamespace test\r\n{\r\npublic class CreateTestClass\r\n{\r\npublic varchar    idname  { get;  set; } \n}\r\n\r\n}\r\n";
            var actual = _csharpLang.Writer(null, exportItem, namespaceName);
            Assert.AreEqual(expected, actual);
        }


    }
}
