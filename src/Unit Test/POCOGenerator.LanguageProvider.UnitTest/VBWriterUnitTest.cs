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
    public class VbWriterUnitTest
    {
        readonly VbLangWriter _vbWriter = new VbLangWriter();

        [TestMethod]  
        public void ConvertListToVbScriptUnitTestForSingleFile()
        {
            var exportPocoList = new List<PocoObjectListForExport>();
            var exportItem = new PocoObjectListForExport();
            const string namespaceName = "test";
            exportItem.NamespaceName = namespaceName;
            exportItem.ClassName = "CreateTestClass";
            var pocoObjItem = new PocoListofObjects {Type = "varchar", ColumnFamilyName = "idname"};
            exportItem.PocoListObjects.Add(pocoObjItem);
            exportPocoList.Add(exportItem);
            const string expected = "Imports System\r\nNamespace test\r\n\r\n\r\nPublic Class CreateTestClass\r\n\r\nPublic Property varchar  As  idname\r\n\r\nEnd Class\r\n\r\n\r\nEnd Namespace\r\n";
            var actual = _vbWriter.Writer(exportPocoList, null, namespaceName);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertListToVbScriptUnitTestForFilePerObject()
        {
            var exportPocoList = new List<PocoObjectListForExport>();
            var exportItem = new PocoObjectListForExport();
            const string namespaceName = "test";
            exportItem.NamespaceName = namespaceName;
            exportItem.ClassName = "CreateTestClass";
            var pocoObjItem = new PocoListofObjects {Type = "varchar", ColumnFamilyName = "idname"};
            exportItem.PocoListObjects.Add(pocoObjItem);
            exportPocoList.Add(exportItem);
            const string expected = "Imports System\r\nNamespace test\r\n\r\n\r\nPublic Class CreateTestClass\r\n\r\nPublic Property varchar  As  idname\r\n\r\nEnd Class\r\n\r\nEnd Namespace\r\n";
            var actual = _vbWriter.Writer(null, exportItem, namespaceName);
            Assert.AreEqual(expected, actual);
        }

    }
}
