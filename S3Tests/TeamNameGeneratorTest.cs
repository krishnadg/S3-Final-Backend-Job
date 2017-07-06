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
using System.Linq;

namespace S3Tests
{


    public abstract class TeamNameGenTestsBase : IDisposable
    {   
        protected AWSClientTest client;
        protected Dictionary<string, List<string>> expectedTeamNamesAndPrefixes;

        protected List<string> prefixesArgs;
        protected TeamNameGenerator sut;
        protected TeamNameGenTestsBase()
        {
            // Do "global" initialization here; Called before every test method.
            client = new AWSClientTest();
            expectedTeamNamesAndPrefixes = new Dictionary<string, List<string>>();
            prefixesArgs = new List<string>();
        }

        public void Dispose()
        {
            // Do "global" teardown here; Called after every test method.

        }
    } 



    public class TeamNameGeneratorTest : TeamNameGenTestsBase
    {


         /*Test Helper method */
        public void AreSameDictionaries(Dictionary<string, List<string>> expected, Dictionary<string, List<string>> result)
        {  
            var countMatches = expected.Count == result.Count;
            Assert.True(countMatches, String.Format("expected count {0}, got count {1}", expected.Count, result.Count ));

            
            foreach (KeyValuePair<string, List<string>> pair in expected)
            {
                string teamName = pair.Key;
                bool hasTeamName = result.ContainsKey(teamName);
                Assert.True(hasTeamName, String.Format("expected team name {0}, got didn't have", teamName));

                List<string> expectedPrefixes = pair.Value;
                List<string> resultPrefixes = result[teamName];

                AreSameLists(expectedPrefixes, resultPrefixes);

            }

        }

        public void AreSameLists(List<string> expected, List<string> result)
        {
            Assert.Equal(expected, result);
        }

    
        [Fact]
        [Trait("Category", "Integration")]
        public void GetListOfTeamNames_0Teams0Files_ReturnEmpty()
        {
            //ARRANGE
            client.CreateEmptyBucket("S3TeamNameGenTestBucket0N");
            sut = new TeamNameGenerator(client.GetClient(), "S3TeamNameGenTestBucket0N");

            //ACT
            var result = sut.GetListOfTeamNames(prefixesArgs);

            //ASSERT
            AreSameDictionaries(expectedTeamNamesAndPrefixes, result);
        }

         
        [Fact]
        [Trait("Category", "Integration")]

        public void GetListOfTeamNames_1Team1File_Return1Teams()
        {
            //ARRANGE
            client.CreateBucket1Team1File("S3TeamNameGenTestBucket1T1F");
            sut = new TeamNameGenerator(client.GetClient(), "S3TeamNameGenTestBucket1T1F");

            prefixesArgs.Add("S3Bucket/12_04_13/"); 

            expectedTeamNamesAndPrefixes.Add("Team1", new List<string> {"S3Bucket/12_04_13/"});


            //ACT
            var result = sut.GetListOfTeamNames(prefixesArgs);
            

           //ASSERT
           AreSameDictionaries(expectedTeamNamesAndPrefixes, result);
        }
        

        [Fact]
        [Trait("Category", "Integration")]

        public void GetListOfTeamNames_3Teams1FileEach_Return3Teams()
        {
            //ARRANGE
            client.CreateBucket3Teams1FileEach("S3TeamNameGenTestBucket3T1F");
            sut = new TeamNameGenerator(client.GetClient(), "S3TeamNameGenTestBucket3T1F");

            prefixesArgs.Add("S3Bucket/12_04_13/"); 


            expectedTeamNamesAndPrefixes.Add("Team1", new List<string> {"S3Bucket/12_04_13/"});
            expectedTeamNamesAndPrefixes.Add("Team2", new List<string> {"S3Bucket/12_04_13/"});
            expectedTeamNamesAndPrefixes.Add("Team3", new List<string> {"S3Bucket/12_04_13/"});

            //ACT
            var result = sut.GetListOfTeamNames(prefixesArgs);
            

           //ASSERT
           AreSameDictionaries(expectedTeamNamesAndPrefixes, result);
         }

        [Fact]
        [Trait("Category", "Integration")]
        public void GetListOfTeamNames_2Teams3FilesEach_Return2Teams()
        {
            //ARRANGE
            client.CreateBucket2Teams3FilesEach("S3TeamNameGenTestBucket2T3F");
            sut = new TeamNameGenerator(client.GetClient(), "S3TeamNameGenTestBucket2T3F");

            prefixesArgs.Add("S3Bucket/12_04_13/");

            expectedTeamNamesAndPrefixes.Add("Team1", new List<string> {"S3Bucket/12_04_13/"});
            expectedTeamNamesAndPrefixes.Add("Team2", new List<string> {"S3Bucket/12_04_13/"});

            //ACT
            var result = sut.GetListOfTeamNames(prefixesArgs);
           

           //ASSERT
            AreSameDictionaries(expectedTeamNamesAndPrefixes, result);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void GetListOfTeamNames_4TeamsVariousFilesEach_Return4Teams()
        {
            //ARRANGE
            client.CreateBucket4TeamsMultipleFilesEach("S3TeamNameGenTestBucket4TVF");
            sut = new TeamNameGenerator(client.GetClient(), "S3TeamNameGenTestBucket4TVF");

            prefixesArgs.Add("S3Bucket/12_04_13/"); 

            expectedTeamNamesAndPrefixes.Add("Team1", new List<string> {"S3Bucket/12_04_13/"});
            expectedTeamNamesAndPrefixes.Add("Team2", new List<string> {"S3Bucket/12_04_13/"});
            expectedTeamNamesAndPrefixes.Add("Team3", new List<string> {"S3Bucket/12_04_13/"});
            expectedTeamNamesAndPrefixes.Add("Team4", new List<string> {"S3Bucket/12_04_13/"});


            //ACT
            var result = sut.GetListOfTeamNames(prefixesArgs);

            //ASSERT
            AreSameDictionaries(expectedTeamNamesAndPrefixes, result);
        }

        /*Tests against adding duplicate keys */
        [Fact]
        [Trait("Category", "Integration")]
        public void GetListOfTeamNames_1Team2Files_Return1Team2Prefixes() 
        {
            //ARRANGE
            client.CreateBucket1Team2FilesDifferentPrefixes("S3TeamNameGenTestBucket1T2F2P");
            sut = new TeamNameGenerator(client.GetClient(), "S3TeamNameGenTestBucket1T2F2P");

            prefixesArgs.Add("S3Bucket/12_04_13/"); 
            prefixesArgs.Add("S3Bucket/12_04_20/"); 


            expectedTeamNamesAndPrefixes.Add("Team1", new List<string> {"S3Bucket/12_04_13/", "S3Bucket/12_04_20/"});


            //ACT
            var result = sut.GetListOfTeamNames(prefixesArgs);

            //ASSERT
            AreSameDictionaries(expectedTeamNamesAndPrefixes, result);
        }
    }


}
