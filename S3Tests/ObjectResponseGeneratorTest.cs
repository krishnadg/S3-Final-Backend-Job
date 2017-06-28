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
            Assert.True(countMatches, String.Format("expected list count {0}, got list count {1}", expected.Count, result.Count() ));

            for (int i = 0; i < expected.Count; i++)
            {
                var bucketNameMatches = expected[i].Name == result.ElementAt(i).Name;
                Assert.True(bucketNameMatches, String.Format("expected bucket name {0}, got name {1} in test {2}", expected[i].Name, result.ElementAt(i).Name, testCase ));
                //var httpStatusCodeMatches = expected[i].HttpStatusCode == result.ElementAt(i).HttpStatusCode;
                //Assert.True(httpStatusCodeMatches, String.Format("expected HTTP Status Code {0}, got code {1} in test {2}", expected[i].HttpStatusCode, result.ElementAt(i).HttpStatusCode, testCase));
                var isTruncated = expected[i].IsTruncated == result.ElementAt(i).IsTruncated;
                Assert.True(isTruncated, String.Format("expected is truncated {0}, got is truncated {1} in test {2}", expected[i].IsTruncated, result.ElementAt(i).IsTruncated, testCase));
                S3ObjectsAreSame(expected[i].S3Objects, result.ElementAt(i).S3Objects.OrderBy(x => x.Key), testCase);
                //Assert.Equal(expected[i].CommonPrefixes, result.ElementAt(i).CommonPrefixes);

                 
            }

        }

        /*Test Helper method comparing 2 Lists of S3Objects */
        public void S3ObjectsAreSame(List<S3Object> expected, IOrderedEnumerable<S3Object> result, string testCase)
        {
            var result2 = result.OrderBy(x => x.Key);
            var countMatches = expected.Count == result.Count();
            Assert.True(countMatches, String.Format("expected object count {0}, got object count {1}", expected.Count, result.Count()));
            
            for (int i = 0; i < expected.Count; i++)
            {
                //Check Matching Keys
                var keyMatches = expected[i].Key == result.ElementAt(i).Key;
                Assert.True(keyMatches, String.Format("expected key name {0}, got name {1} in test {2} loop count {3}", expected[i].Key, result.ElementAt(i).Key, testCase,  i ));
                //If sizes were present, compare known file size vs actual
                var sizeMatches = expected[i].Size == result.ElementAt(i).Size;
                Assert.True(keyMatches, String.Format("expected size {0}, got size {1} in test {2} loop count {3}", expected[i].Size, result.ElementAt(i).Size, testCase, i ));


            }
        }



        [Fact]
        public void GetObjectResponseList_1ListObjectRequests_ReturnList1Response()
        {
            //ARRANGE
            client.CreateBucket2("S3TestBucket2b");
            sut = new ObjectResponseGenerator(client.GetClient());
            
            objRequestArgs = new List<ListObjectsRequest>
            {
                new ListObjectsRequest 
                {
                BucketName = "S3TestBucket2b",
                Prefix = "S3Bucket/Team1",
                }
                              
            };

            expectedObjectResponses = new List<ListObjectsResponse>
            {
                new ListObjectsResponse
                {
                    Name = "S3TestBucket2b",
                    Prefix = "S3Bucket/Team1",
                    S3Objects = new List<S3Object>
                    {
                        new S3Object
                        {
                            BucketName = "S3TestBucket2b",
                            Key = "S3Bucket/Team1/smallsizefile",
                            
                        },
                        new S3Object
                        {
                            BucketName = "S3TestBucket2b",
                            Key = "S3Bucket/Team1/smallsizefile2"
                        },
                        new S3Object
                        {
                            BucketName = "S3TestBucket2b",
                            Key = "S3Bucket/Team1/smallsizefile3"
                        }


                    },
                   
                },
              

            };

            
            //ACT
            var result = sut.GetObjectResponseList(objRequestArgs).OrderBy(x => x.S3Objects[0].Key);
            
            //ASSERT
            ListObjResponsesAreSame(expectedObjectResponses, result, "1");
        }




        [Fact]
        public void GetObjectResponseList_4ListObjectRequests_ReturnList4Responses()
        {
            //ARRANGE
            client.CreateBucket3("S3TestBucket2c");
            sut = new ObjectResponseGenerator(client.GetClient());
            
            objRequestArgs = new List<ListObjectsRequest>
            {
                new ListObjectsRequest 
                {
                BucketName = "S3TestBucket2c",
                Prefix = "S3Bucket/Team1/",
                
                },
                new ListObjectsRequest{
                BucketName = "S3TestBucket2c",
                Prefix = "S3Bucket/Team2/",
                },
                new ListObjectsRequest
                {
                    BucketName = "S3TestBucket2c",
                    Prefix = "S3Bucket/Team3/"
                },
                new ListObjectsRequest
                {
                    BucketName = "S3TestBucket2c",
                    Prefix = "S3Bucket/Team4/"
                }
                
            };

            expectedObjectResponses = new List<ListObjectsResponse>
            {
                new ListObjectsResponse
                {
                    Name = "S3TestBucket2c",
                    
                    S3Objects = new List<S3Object>
                    {
                        new S3Object
                        {
                            BucketName = "S3TestBucket2c",
                            Key = "S3Bucket/Team1/smallsizefile",
                        },
                        new S3Object
                        {
                            BucketName = "S3TestBucket2c",
                            Key = "S3Bucket/Team1/smallsizefile2"
                        },
                        new S3Object
                        {
                            BucketName = "S3TestBucket2c",
                            Key = "S3Bucket/Team1/smallsizefile3"
                        }


                    },
                   
                },
                new ListObjectsResponse
                {
                    Name = "S3TestBucket2c",
                    S3Objects = new List<S3Object>
                    {
                         new S3Object
                        {
                            BucketName = "S3TestBucket2c",
                            Key = "S3Bucket/Team2/largesizefile",
                        },
                        new S3Object
                        {
                            BucketName = "S3TestBucket2c",
                            Key = "S3Bucket/Team2/largesizefile2"
                        },
                        new S3Object
                        {
                            BucketName = "S3TestBucket2c",
                            Key = "S3Bucket/Team2/largesizefile3"
                        },
                        new S3Object
                        {
                            BucketName = "S3TestBucket2c",
                            Key = "S3Bucket/Team2/mediumsizefile"
                        }

                    },
                    
                    
                },
                new ListObjectsResponse
                {
                    Name = "S3TestBucket2c",
                    S3Objects = new List<S3Object>
                    {
                         new S3Object
                        {
                            BucketName = "S3TestBucket2c",
                            Key = "S3Bucket/Team3/smallsizefile",
                        },
                    }
                },
                new ListObjectsResponse
                {
                    Name = "S3TestBucket2c",
                    S3Objects = new List<S3Object>
                    {
                         new S3Object
                        {
                            BucketName = "S3TestBucket2c",
                            Key = "S3Bucket/Team4/largesizefile2",
                        },
                         new S3Object
                        {
                            BucketName = "S3TestBucket2c",
                            Key = "S3Bucket/Team4/largesizefile3",
                        },
                    }
                }

            };

            
            //ACT
            var result = sut.GetObjectResponseList(objRequestArgs).OrderBy(x => x.S3Objects[0].Key);
            
            //ASSERT
            ListObjResponsesAreSame(expectedObjectResponses, result, "2");
        }


    }
}
