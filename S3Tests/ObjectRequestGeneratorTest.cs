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
        protected AWSTestClient client;
        protected List<string> teamNamesArgs; 
        protected List<ListObjectsRequest> expectedObjectsRequests;

        protected ObjectRequestGenerator sut;

        protected ObjRequestTestsBase()
        {
            // Do "global" initialization here; Called before every test method.
            client = new AWSTestClient();
            teamNamesArgs = new List<string>();  
            sut = new ObjectRequestGenerator(client.GetClient(), "S3TestBucket");

        }

        public void Dispose()
        {
            // Do "global" teardown here; Called after every test method.
          //  client.RemoveBucketFromS3("S3TestBucket");
        }
    } 



    public class ObjectResponseRequestTest : ObjRequestTestsBase
    {
        /*Test Helper method */
        public void IsSame(List<ListObjectsRequest> expected, IOrderedEnumerable<ListObjectsRequest> result, String testCase)
        {  
            var countMatches = expected.Count == result.Count();
            Assert.True(countMatches, String.Format("got count {0}, expected count {1}", expected.Count, result.Count() ));

            for (int i = 0; i < expected.Count; i++)
            {
                var bucketNameMatches = expected[i].BucketName == result.ElementAt(i).BucketName;
                Assert.True(bucketNameMatches, String.Format("got bucket name {0}, expected name {1} in test {2}", expected[i].BucketName, result.ElementAt(i).BucketName, testCase ));
                var prefixMatches = expected[i].Prefix == result.ElementAt(i).Prefix;
                Assert.True(prefixMatches, String.Format("got prefix name {0}, expected name {1} in test {2}", expected[i].Prefix, result.ElementAt(i).Prefix, testCase));
                var delimeterMatches = expected[i].Delimiter == result.ElementAt(i).Delimiter;
                Assert.True(prefixMatches, String.Format("got delimiter name {0}, expected name {1} in test {2}", expected[i].Delimiter, result.ElementAt(i).Delimiter, testCase));

                
            }

        }


        [Fact]
        public void GetObjectRequestList_0TeamNames_ReturnEmptyList()
        {
            //ARRANGE
            client.CreateBucket0("S3TestBucket0");
            sut = new ObjectRequestGenerator(client.GetClient(), "S3TestBucket0");
            
            expectedObjectsRequests = new List<ListObjectsRequest>();
            
            //ACT
            var result = sut.GetObjectRequestList(teamNamesArgs).OrderBy(x => x.BucketName);

            //ASSERT
            IsSame(expectedObjectsRequests, result, "1");
            }


        [Fact]
        public void GetObjectRequestList_1TeamName_ReturnList1Request()
        {
            //ARRANGE
            client.CreateBucket0("S3TestBucket0");
            sut = new ObjectRequestGenerator(client.GetClient(), "S3TestBucket0");
            teamNamesArgs.Add("Team1");
            
            expectedObjectsRequests = new List<ListObjectsRequest> {
                new ListObjectsRequest{
                BucketName = "S3TestBucket0",
                Prefix = "S3Bucket/",
                Delimiter = "Team1/",
                }
            };
            
            //ACT
            var result = sut.GetObjectRequestList(teamNamesArgs).OrderBy(x => x.BucketName);

            //ASSERT
            IsSame(expectedObjectsRequests, result, "2");
        }


    
        [Fact]
        public void GetObjectRequestList_4TeamNames_ReturnList4Requests()
        {
            //ARRANGE
            client.CreateBucket0("S3TestBucket0");
            sut = new ObjectRequestGenerator(client.GetClient(), "S3TestBucket0");
            teamNamesArgs.Add("Team1");
            teamNamesArgs.Add("Team2");
            teamNamesArgs.Add("Team3");
            teamNamesArgs.Add("Team4");

            expectedObjectsRequests = new List<ListObjectsRequest> {
                new ListObjectsRequest{
                BucketName = "S3TestBucket0",
                Prefix = "S3Bucket/",
                Delimiter = "Team1/",
                
                },
                new ListObjectsRequest{
                BucketName = "S3TestBucket0",
                Prefix = "S3Bucket/",
                Delimiter = "Team2/"
                },
                new ListObjectsRequest{
                BucketName = "S3TestBucket0",
                Prefix = "S3Bucket/",
                Delimiter = "Team3/"
                },
                new ListObjectsRequest{
                BucketName = "S3TestBucket0",
                Prefix = "S3Bucket/",
                Delimiter = "Team4/"
                }   
            };
           
            
            //ACT
            var result = sut.GetObjectRequestList(teamNamesArgs).OrderBy(x => x.BucketName);

            //ASSERT
            IsSame(expectedObjectsRequests, result, "3");
            }

            
        

       
        
    }
}
