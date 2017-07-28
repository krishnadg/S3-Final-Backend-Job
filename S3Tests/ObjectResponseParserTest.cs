using System;
using System.Diagnostics;
using Xunit;
using Xunit.Sdk;
using S3ClassLib;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace S3Tests
{


    public abstract class ObjResponseTestsBase : IDisposable
    {
        protected AWSClientTest client;
        protected List<ListObjectsResponse> listResponsesArgs;
        protected Dictionary<string, long> expected;
        protected ObjectResponseParser sut;

        protected ObjResponseTestsBase()
        {
            // Do "global" initialization here; Called before every test method.

            client = new AWSClientTest();
            expected = new Dictionary<string, long>();
            listResponsesArgs = new List<ListObjectsResponse>();
        }

        public void Dispose()
        {
            // Do "global" teardown here; Called after every test method.

        }
    } 


    

    public class ObjectResponseParserTest : ObjResponseTestsBase
    {

        /* Test helper to more easily verify the Dictionary data structures are identical */
        public void DataStructuresAreSame(Dictionary<string, long> expected, Dictionary<string, long> result)
        {
            var countMatches = expected.Count == result.Count;
            Assert.True(countMatches, String.Format("expected Dict count {0}, got Dict count {1}", expected.Count, result.Count ));

           
            foreach (KeyValuePair<string, long> expectedTeamData in expected)
            {
                var teamNameMatches = result.ContainsKey(expectedTeamData.Key);
                Assert.True(teamNameMatches, String.Format("expected contains team name {0}, got contains {1}", expectedTeamData.Key, result.ContainsKey(expectedTeamData.Key) ));

                long resultTeamStorage;
                
                var teamDataFound = result.TryGetValue(expectedTeamData.Key, out resultTeamStorage);
                var teamDataMatches = expectedTeamData.Value == resultTeamStorage;
                Assert.True(teamDataMatches, String.Format("expected team storage size {0}, got size {1}", expectedTeamData.Value, resultTeamStorage ));

            }
        }

        // [Fact]
        public void ParseListObjectresponses_0ListResponses_Return0Entries()
        {
             //ARRANGE
            string prefix = "S3Bucket";
            sut = new ObjectResponseParser(prefix);
            
            expected.Clear();
            
            //ACT

            var result = sut.GetDataStructure(listResponsesArgs);

            //ASSERT
            DataStructuresAreSame(expected, result);
        }

        [Fact]
        public void ParseListObjectresponses_1ListResponses_Return1Entry()
        {
             //ARRANGE
            string prefix = "S3Bucket";
            sut = new ObjectResponseParser(prefix);

            listResponsesArgs.Add(client.GetFakeListResponseTeam1()); //Static List Response from AWSClient.cs
           
            var team1Data = 194;
            //var totalStorage = 194;
            expected.Add("Team1", team1Data);
            //expected.Add("Total-Storage", totalStorage);
            
            //ACT

            var result = sut.GetDataStructure(listResponsesArgs);

            //ASSERT
            DataStructuresAreSame(expected, result);
        }
    
        [Fact]
        public void ParseListObjectresponses_3ListResponses_Return3Entries()
        {
             //ARRANGE
            string prefix = "S3Bucket";
            sut = new ObjectResponseParser(prefix);

            listResponsesArgs.Add(client.GetFakeListResponseTeam1_3Files()); //Static Lists Response from AWSClientTest.cs
            listResponsesArgs.Add(client.GetFakeListResponseTeam2());
            listResponsesArgs.Add(client.GetFakeListResponseTeam3());

            var team1Size = 900;
            var team2Size = 1004;
            var team3Size = 100024;
            //var totalStorage = 101928;

            expected.Add("Team1", team1Size);
            expected.Add("Team2", team2Size);
            expected.Add("Team3", team3Size);
            //expected.Add("Total-Storage", totalStorage);



            
            //ACT

            var result = sut.GetDataStructure(listResponsesArgs);

            //ASSERT
            DataStructuresAreSame(expected, result);
        }
        
        
    }
}
