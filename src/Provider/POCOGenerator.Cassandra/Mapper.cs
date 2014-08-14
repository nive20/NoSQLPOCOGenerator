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
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using POCOGenerator.DatabaseObjects;  

namespace POCOGenerator.Cassandra
{

    public class Mapper
    {

        #region Global Variables

        public Dictionary<string, string> DataTypes = new Dictionary<string, string>();
        CassandraCSharpDataType _cassandraCSharpDataType; 

        #endregion

        #region Initailize Data Types From XML

        /// <summary>
        /// This function reads the DataType Mapping for Cassandra DataTypes and CSharp Datatypes.
        /// </summary>
        public void InitializeDataTypeMappings()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var textStreamReader = new StreamReader(assembly.GetManifestResourceStream("POCOGenerator.Cassandra.DataSource.DataTypeMapping.xml"));

            _cassandraCSharpDataType = new CassandraCSharpDataType();
            var settings = new XmlReaderSettings {ConformanceLevel = ConformanceLevel.Document};

            using (XmlReader reader = XmlReader.Create(textStreamReader, settings))
            {
                while (reader.Read())
                {
                    if (!(reader.IsStartElement() && reader.Name.Equals("CDataType")))
                        continue;
                    while (reader.MoveToNextAttribute())
                    {
                        switch (reader.Name)
                        {
                            case "key": _cassandraCSharpDataType.Key = reader.Value.ToString(CultureInfo.InvariantCulture).ToLower();

                                break;
                            case "value": _cassandraCSharpDataType.Value = reader.Value.ToString(CultureInfo.InvariantCulture);
                                break;
                        }
                    }
                    DataTypes.Add(_cassandraCSharpDataType.Key, _cassandraCSharpDataType.Value);
                }
            }
        } 

        #endregion


    }
}
