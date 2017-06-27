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
        protected AWSTestClient client;
        protected List<string> expectedTeamNames;

        protected TeamNameGenerator sut;
        protected TeamNameGenTestsBase()
        {
            // Do "global" initialization here; Called before every test method.
            client = new AWSTestClient();
            expectedTeamNames = new List<string>();

        }

        public void Dispose()
        {
            // Do "global" teardown here; Called after every test method.
        }
    } 



    public class TestTeamNameGenerator : TeamNameGenTestsBase
    {
    
        [Fact]
        public void GetListOfTeamNames_0Teams0Files_ReturnEmpty()
        {
            //ARRANGE
            client.CreateBucket0("S3TestBucket0");
            sut = new TeamNameGenerator(client.GetClient(), "S3TestBucket0");

            //ACT
            var result = sut.GetListOfTeamNames();

            //ASSERT
            Assert.True(true);
        }
        

        [Fact]
        public void GetListOfTeamNames_3Teams1FileEach_Return3Teams()
        {
            //ARRANGE
            client.CreateBucket1("S3TestBucket1");
            sut = new TeamNameGenerator(client.GetClient(), "S3TestBucket1");

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
            client.CreateBucket2("S3TestBucket2a");
            sut = new TeamNameGenerator(client.GetClient(), "S3TestBucket2a");

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
            client.CreateBucket3("S3TestBucket3");
            sut = new TeamNameGenerator(client.GetClient(), "S3TestBucket3");

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
