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


    public abstract class ObjRequestTestsBase : IDisposable
    {   
        protected AWSClientTest client;
        protected List<string> teamNamesArgs; //To be used for the arguments of sut's "do its job" method
        protected List<ListObjectsRequest> expectedObjectsRequests;

        protected ObjectRequestGenerator sut;

        protected ObjRequestTestsBase()
        {
            // Do "global" initialization here; Called before every test method.
            client = new AWSClientTest();
            teamNamesArgs = new List<string>();  

        }

        public void Dispose()
        {
            // Do "global" teardown here; Called after every test method.
        }
    } 



    public class ObjectResponseRequestTest : ObjRequestTestsBase
    {
        /*Test Helper method */
        public void IsSameObjectRequests(List<ListObjectsRequest> expected, IOrderedEnumerable<ListObjectsRequest> result, String testCase)
        {  
            var countMatches = expected.Count == result.Count();
            Assert.True(countMatches, String.Format("expected count {0}, got count {1}", expected.Count, result.Count() ));

            for (int i = 0; i < expected.Count; i++)
            {
                var bucketNameMatches = expected[i].BucketName == result.ElementAt(i).BucketName;
                Assert.True(bucketNameMatches, String.Format("expected bucket name {0}, got name {1} in test {2}", expected[i].BucketName, result.ElementAt(i).BucketName, testCase ));
                var prefixMatches = expected[i].Prefix == result.ElementAt(i).Prefix;
                Assert.True(prefixMatches, String.Format("expected prefix name {0}, got name {1} in test {2}", expected[i].Prefix, result.ElementAt(i).Prefix, testCase));
                var delimeterMatches = expected[i].Delimiter == result.ElementAt(i).Delimiter;
                Assert.True(prefixMatches, String.Format("expected delimiter name {0}, got name {1} in test {2}", expected[i].Delimiter, result.ElementAt(i).Delimiter, testCase));

                
            }

        }


        [Fact]
        public void GetObjectRequestList_0TeamNames_ReturnEmptyList()
        {
            //ARRANGE
            client.CreateEmptyBucket("S3RequestGenTestBucket0TN");
            sut = new ObjectRequestGenerator( "S3RequestGenTestBucket0TN");
            
            expectedObjectsRequests = new List<ListObjectsRequest>(); //Empty
            
            //ACT
            var result = sut.GetObjectRequestList(teamNamesArgs).OrderBy(x => x.BucketName);

            //ASSERT
            IsSameObjectRequests(expectedObjectsRequests, result, "1");
            }


        [Fact]
        public void GetObjectRequestList_1TeamName_ReturnList1Request()
        {
            //ARRANGE
            client.CreateBucket1Team1File("S3RequestGenTestBucket1TN");
            sut = new ObjectRequestGenerator( "S3RequestGenTestBucket1TN");
            teamNamesArgs.Add("Team1");
            
            expectedObjectsRequests = new List<ListObjectsRequest> {
                client.GetFakeListRequestTeam1("S3RequestGenTestBucket1TN")
            };
            
            //ACT
            var result = sut.GetObjectRequestList(teamNamesArgs).OrderBy(x => x.BucketName);

            //ASSERT
            IsSameObjectRequests(expectedObjectsRequests, result, "2");
        }


    
        [Fact]
        public void GetObjectRequestList_4TeamNames_ReturnList4Requests()
        {
            //ARRANGE
            client.CreateBucket4TeamsMultipleFilesEach("S3RequestGenTestBucket4TN");
            sut = new ObjectRequestGenerator("S3RequestGenTestBucket4TN");
            teamNamesArgs.Add("Team1");
            teamNamesArgs.Add("Team2");
            teamNamesArgs.Add("Team3");
            teamNamesArgs.Add("Team4");


            expectedObjectsRequests = new List<ListObjectsRequest> {
                client.GetFakeListRequestTeam1("S3RequestGenTestBucket4TN"),
                client.GetFakeListRequestTeam2("S3RequestGenTestBucket4TN"),
                client.GetFakeListRequestTeam3("S3RequestGenTestBucket4TN"),
                client.GetFakeListRequestTeam4("S3RequestGenTestBucket4TN")
            };  
           
            
            //ACT
            var result = sut.GetObjectRequestList(teamNamesArgs).OrderBy(x => x.BucketName);

            //ASSERT
            IsSameObjectRequests(expectedObjectsRequests, result, "3");
            }
        
    }
}
