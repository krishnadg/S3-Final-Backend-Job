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


    public abstract class ObjResponseGenTestsBase : IDisposable
    {   
        protected AWSTestClient client;
        protected List<ListObjectsResponse> expectedObjectResponses;

        protected List<ListObjectsRequest> objRequestArgs;
        protected ObjectResponseGenerator sut;

        protected ObjResponseGenTestsBase()
        {
            // Do "global" initialization here; Called before every test method.
            client = new AWSTestClient();
            objRequestArgs = new List<ListObjectsRequest>();
            expectedObjectResponses = new List<ListObjectsResponse>();
        }

        public void Dispose()
        {
            // Do "global" teardown here; Called after every test method.
          //  client.RemoveBucketFromS3("S3TestBucket");
        }
    } 



    public class ObjectResponseGeneratorTest : ObjResponseGenTestsBase
    {
        

        /*Test Helper method comparing 2 "Lists" of ListObjectResponse*/
        public void ListObjResponsesAreSame(List<ListObjectsResponse> expected, IOrderedEnumerable<ListObjectsResponse> result, String testCase)
        {  
            var countMatches = expected.Count == result.Count();
            Assert.True(countMatches, String.Format("got count {0}, expected count {1}", expected.Count, result.Count() ));

            for (int i = 0; i < expected.Count; i++)
            {
                var bucketNameMatches = expected[i].Name == result.ElementAt(i).Name;
                Assert.True(bucketNameMatches, String.Format("got bucket name {0}, expected name {1} in test {2}", expected[i].Name, result.ElementAt(i).Name, testCase ));
                var httpStatusCodeMatches = expected[i].HttpStatusCode == result.ElementAt(i).HttpStatusCode;
                Assert.True(httpStatusCodeMatches, String.Format("got HTTP Status Code {0}, expected code {1} in test {2}", expected[i].HttpStatusCode, result.ElementAt(i).HttpStatusCode, testCase));
                var isTruncated = expected[i].IsTruncated == result.ElementAt(i).IsTruncated;
                Assert.True(isTruncated, String.Format("got delimiter name {0}, expected name {1} in test {2}", expected[i].IsTruncated, result.ElementAt(i).IsTruncated, testCase));
                S3ObjectsAreSame(expected[i].S3Objects, result.ElementAt(i).S3Objects, testCase);
            }

        }

        /*Test Helper method comparing 2 Lists of S3Objects */
        public void S3ObjectsAreSame(List<S3Object> expected, List<S3Object> result, string testCase)
        {
            var countMatches = expected.Count == result.Count;
            Assert.True(countMatches, String.Format("got count {0}, expected count {1}", expected.Count, result.Count));

            for (int i = 0; i < expected.Count)
            {
                var keyMatches = expected[i].Key == result.ElementAt(i).Key;
                Assert.True(keyMatches, String.Format("got bucket name {0}, expected name {1} in test {2}", expected[i].Key, result[i].Key, testCase ));


            }
        }



        [Fact]
        public void GetObjectResponseList_2ObjectRequests_ReturnList2Responses()
        {
            //ARRANGE
            client.CreateBucket2("S3TestBucket2");
            sut = new ObjectResponseGenerator(client.GetClient());
            
            objRequestArgs = new List<ListObjectsRequest>
            {
                new ListObjectsRequest 
                {
                BucketName = "S3TestBucket2",
                Prefix = "S3Bucket/",
                Delimiter = "Team1/",
                
                },
                new ListObjectsRequest{
                BucketName = "S3TestBucket2",
                Prefix = "S3Bucket/",
                Delimiter = "Team2/"
                }
                
            };

            expectedObjectResponses = new List<ListObjectsResponse>
            {
                new ListObjectsResponse
                {
                    S3Objects = new List<S3Object>
                    {
                        new S3Object
                        {
                            BucketName = "S3TestBucket2",
                            Key = "S3Bucket/Team1/smallsizefile"
                        }

                    },
                    CommonPrefixes = new List<string>
                    {

                    }
                },
                new ListObjectsResponse
                {
                    
                    S3Objects = new List<S3Object>
                    {
                        new S3Object
                        {
                            BucketName = "S3TestBucket2",
                        }

                    },

                    CommonPrefixes = new List<string>
                    {

                    }
                },

            };

            
            //ACT
          //  var result = sut.GetObjectRequestList(teamNamesArgs).OrderBy(x => x.BucketName);

            var result = sut.GetObjectResponseList(objRequestArgs).OrderBy(x => x.Name);
            
            //ASSERT

            Console.WriteLine("S3Objects Count: " + result.ElementAt(0).S3Objects.Count);
            Console.WriteLine("Bucketname: " + result.ElementAt(0).Name);

          //  ListObjResponsesAreSame()

        }


        [Fact]
        public void GetObjectRequestList_1TeamName_ReturnList1Request()
        {
        }


    
        [Fact]
        public void GetObjectRequestList_4TeamNames_ReturnList4Requests()
        {
            
        }
             
        
    }
}
