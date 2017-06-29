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


    public abstract class TeamNameGenTestsBase : IDisposable
    {   
        protected AWSClientTest client;
        protected List<string> expectedTeamNames;

        protected TeamNameGenerator sut;
        protected TeamNameGenTestsBase()
        {
            // Do "global" initialization here; Called before every test method.
            client = new AWSClientTest();
            expectedTeamNames = new List<string>();

        }

        public void Dispose()
        {
            // Do "global" teardown here; Called after every test method.

        }
    } 



    public class TeamNameGeneratorTest : TeamNameGenTestsBase
    {
    
        [Fact]
        public void GetListOfTeamNames_0Teams0Files_ReturnEmpty()
        {
            //ARRANGE
            client.CreateEmptyBucket("S3TeamNameGenTestBucket0N");
            sut = new TeamNameGenerator(client.GetClient(), "S3TeamNameGenTestBucket0N");

            //ACT
            var result = sut.GetListOfTeamNames();

            //ASSERT
            Assert.Equal(expectedTeamNames, result);
        }

         [Fact]
        public void GetListOfTeamNames_1Team1File_Return1Teams()
        {
            //ARRANGE
            client.CreateBucket1Team1File("S3TeamNameGenTestBucket1T1F");
            sut = new TeamNameGenerator(client.GetClient(), "S3TeamNameGenTestBucket1T1F");

            expectedTeamNames.Add("Team1");


            //ACT
            var result = sut.GetListOfTeamNames();
            

           //ASSERT
            Assert.Equal(expectedTeamNames, result);
        }
        

        [Fact]
        public void GetListOfTeamNames_3Teams1FileEach_Return3Teams()
        {
            //ARRANGE
            client.CreateBucket3Teams1FileEach("S3TeamNameGenTestBucket3T1F");
            sut = new TeamNameGenerator(client.GetClient(), "S3TeamNameGenTestBucket3T1F");

            expectedTeamNames.Add("Team1");
            expectedTeamNames.Add("Team2");
            expectedTeamNames.Add("Team3");

            //ACT
            var result = sut.GetListOfTeamNames();
            

           //ASSERT
            Assert.Equal(expectedTeamNames, result);
        }

        [Fact]
        public void GetListOfTeamNames_2Teams3FilesEach_Return2Teams()
        {
            //ARRANGE
            client.CreateBucket2Teams3FilesEach("S3TeamNameGenTestBucket2T3F");
            sut = new TeamNameGenerator(client.GetClient(), "S3TeamNameGenTestBucket2T3F");

            expectedTeamNames.Add("Team1");
            expectedTeamNames.Add("Team2");

            //ACT
            var result = base.sut.GetListOfTeamNames();
           

           //ASSERT
            Assert.Equal(expectedTeamNames, result);
        }

        [Fact]
        public void GetListOfTeamNames_4TeamsVariousFilesEach_Return4Teams()
        {
            //ARRANGE
            client.CreateBucket4TeamsMultipleFilesEach("S3TeamNameGenTestBucket4TVF");
            sut = new TeamNameGenerator(client.GetClient(), "S3TeamNameGenTestBucket4TVF");

            expectedTeamNames.Add("Team1");
            expectedTeamNames.Add("Team2");
            expectedTeamNames.Add("Team3");
            expectedTeamNames.Add("Team4");

            //ACT
            var result = base.sut.GetListOfTeamNames();

            //ASSERT
            Assert.Equal(expectedTeamNames, result);
        }
    }


}
