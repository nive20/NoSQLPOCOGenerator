/* NoSQLPOCOGenerator - A database to object mapper for NOSQL databases.
 * Developed by Happiest Minds Private Limited http://www.happiestminds.com  

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.*/

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace POCOGenerator.LanguageProvider.UnitTest
{
    [TestClass]
    public class LanguageSelectedUnitTest
    {

        [TestMethod]
        public void AssignSelectedLanguageTypeUnitTestIfPresentDB_TO_POCO()
        {  
            const bool expected = true;
            var actual= LanguageSelector.AssignSelectedLanguageTypeDbToPoco("VB");
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void AssignSelectedLanguageTypeUnitTestIfNotPresentDB_TO_POCO()
        {
            const bool expected = false;
            var actual = LanguageSelector.AssignSelectedLanguageTypeDbToPoco("Test");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AssignSelectedLanguageType_POCO_TO_DB_UnitTestIfPresent()
        {
            const bool expected = true;
            var actual = LanguageSelector.AssignSelectedLanguageTypePocoToDb("using");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AssignSelectedLanguageType_POCO_TO_DB_UnitTestIfNotPresent()
        {
            const bool expected = false;
            var actual = LanguageSelector.AssignSelectedLanguageTypePocoToDb("abc");
            Assert.AreEqual(expected, actual);
        }
    }
}
