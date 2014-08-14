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

using System;
using System.Collections.Generic;
using System.Linq;
using Cassandra;
using CassandraSharp;
using CassandraSharp.Config;
using CassandraSharp.CQLPoco;
using POCOGenerator.DatabaseObjects;

namespace POCOGenerator.Cassandra
{
    public class ConnectionToCluster
    {
         
        #region Connection Object

        private static ICluster _cluster; 

        #endregion

        #region Get Connection Status

        /// <summary>
        /// This functions creates the Connection With Cassandra DB.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="clusterName"></param>
        /// <returns>Returns isConnected</returns>
        public bool GetConnectionObject(string userName,string password, string clusterName)
        {
            bool isConnected;
            try
            {
                ClusterManager.Shutdown();
                XmlConfigurator.Configure();
                Cluster cluster = Cluster.Builder().AddContactPoints("localhost").WithCredentials(userName,
                                   password).Build();
                var session = cluster.Connect();
                if (session.Cluster.Metadata.ClusterName != null)
                {
                    _cluster = ClusterManager.GetCluster(clusterName);
                    isConnected = true;
                }
                else isConnected = false;
            }
            catch (Exception)
            {
                isConnected = false;
            }
            return isConnected;
        } 

        #endregion

        #region Fetch Data From DB

        /// <summary>
        /// This functions fetches the Metadata Of Cassandra DB
        /// </summary>
        /// <returns>Returns List of each Keyspace with Metadata</returns>
        public List<KeyspacesName> FetchKeyspaceNames()
        {
            var dataSource = new List<KeyspacesName>();
            var parentKeys = FetchKeyspacesfromCluster();
            foreach (TableNames tbName in parentKeys)
            {

                if (!tbName.Keyspacename.Contains("system"))
                {
                    if (!tbName.Keyspacename.Contains("OpsCenter"))
                    {
                        var keySpace = new KeyspacesName
                        {
                            Parent = tbName.Keyspacename,
                            Children = PopulateColumnfamilynames(tbName.Keyspacename)
                        };
                        foreach (var checkchild in keySpace.Children)
                        {
                            List<ColumnFamilyMetadata> childMetadata = FetchColumnFamilyMetadataDetails(keySpace.Parent, checkchild.Columnfamilyname);
                            checkchild.Ischecked = false;
                            checkchild.ChildMetadata = childMetadata;
                        }

                        keySpace.Ischecked = false;
                        dataSource.Add(keySpace); 
                    }

                }
            }
            return dataSource;
        }

        /// <summary>
        /// This function provide query for fetching the Keyspaces(TableNames)
        /// </summary>
        /// <returns>Returns List of Keyspaces</returns>
        public List<TableNames> FetchKeyspacesfromCluster()
        {
            try
            {

                var cmd = _cluster.CreatePocoCommand();
                const string selectfromstorebyconsumer = " SELECT * from system.schema_keyspaces;";
                var parentKeys = cmd.Execute<TableNames>(selectfromstorebyconsumer).AsFuture().Result.ToList();
                return parentKeys;
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// This function populates ColumnFamilyNames for each Keyspaces
        /// </summary>
        /// <param name="parent"></param>
        /// <returns>Returns List of ColumnFamilyNames</returns>
        public List<ColumnFamilyName> PopulateColumnfamilynames(string parent)
        {
            try
            {
                var cmd = _cluster.CreatePocoCommand();
                var columnfamilyfetchquery =
                        "SELECT columnfamily_name FROM System.schema_columnfamilies WHERE keyspace_name = '" +
                         parent + "'";
                return cmd.Execute<ColumnFamilyName>(columnfamilyfetchquery).AsFuture().Result.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// This function fetch ColumnFamily Metadata.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns>Returns ColumnFamily Metadata</returns>
        public List<ColumnFamilyMetadata> FetchColumnFamilyMetadataDetails(string parent, string child)
        {
            try
            {
                var cmd = _cluster.CreatePocoCommand();
                var columnfamilyfetchquery =
                       "SELECT  columnfamily_name, column_name,index_name, validator,index_type from system.schema_columns where keyspace_name='" + parent + "' and columnfamily_name='" + child + "' allow filtering";

                var columnfamilynamesresult = cmd.Execute<ColumnFamilyMetadata>(columnfamilyfetchquery).AsFuture().Result.ToList();
                return columnfamilynamesresult;
            }
            catch (Exception)
            {
                return null;
            }

        }

        #endregion

        #region Execute Queries To Database

        public void ExceuteDyamicQueriesToCassandraDb(string selectfromstorebyconsumer)
        {
            var cmd = _cluster.CreatePocoCommand();
            cmd.Execute(selectfromstorebyconsumer).AsFuture().Wait();
        } 

        #endregion

        #region Check Available KeyspaceName and ColumnFamilyNames

        /// <summary>
        /// This function checks the specified keyspace exists in Cassandra DB
        /// </summary>
        /// <param name="keyspaceCollection"></param>
        /// <param name="parentItem"></param>
        /// <returns>Returns bool</returns>
        public bool CheckAvailableKeyspaceName(List<TableNames> keyspaceCollection, PocoObjectListForExport parentItem)
        {
           return keyspaceCollection.Any(t => parentItem.NamespaceName == t.Keyspacename);
        }

        /// <summary>
        /// This function checks specified ColumnFamily Name is Available for specified keyspace in Cassandra DB
        /// </summary>
        /// <param name="parentItem"></param>
        /// <returns>Returns bool</returns>
        public bool CheckAvailableColumnFamilyNames(PocoObjectListForExport parentItem)
        {
            var columnFamilyNames = PopulateColumnfamilynames(parentItem.NamespaceName);
            return columnFamilyNames.All(childItem => childItem.Columnfamilyname != parentItem.ClassName);

            //foreach (var childItem in columnFamilyNames)
            //{
            //    if (childItem.Columnfamilyname == parentItem.ClassName)
            //    {
            //        flagchild = false; break;
            //    }
            //}
            //return flagchild;
        } 

        #endregion
    }
}
