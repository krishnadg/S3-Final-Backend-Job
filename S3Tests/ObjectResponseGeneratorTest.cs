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
        protected AWSClientTest client;
        protected List<ListObjectsResponse> expectedObjectResponses;

        protected List<ListObjectsRequest> objRequestArgs;
        protected ObjectResponseGenerator sut;

        protected ObjResponseGenTestsBase()
        {
            // Do "global" initialization here; Called before every test method.
            client = new AWSClientTest();
            objRequestArgs = new List<ListObjectsRequest>();
            expectedObjectResponses = new List<ListObjectsResponse>();
        }

        public void Dispose()
        {
            // Do "global" teardown here; Called after every test method.
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
                
                //var httpStatusCodeMatches = expected[i].HttpStatusCode == result.ElementAt(i).HttpStatusCode;
                //Assert.True(httpStatusCodeMatches, String.Format("expected HTTP Status Code {0}, got code {1} in test {2}", expected[i].HttpStatusCode, result.ElementAt(i).HttpStatusCode, testCase));
                var isTruncated = expected[i].IsTruncated == result.ElementAt(i).IsTruncated;
                Assert.True(isTruncated, String.Format("expected is truncated {0}, got is truncated {1} in test {2}", expected[i].IsTruncated, result.ElementAt(i).IsTruncated, testCase));
                S3ObjectsAreSame(expected[i].S3Objects, result.ElementAt(i).S3Objects.OrderBy(x => x.Key), testCase);

                 
            }

        }

        /*Test Helper method comparing 2 Lists of S3Objects */
        public void S3ObjectsAreSame(List<S3Object> expected, IOrderedEnumerable<S3Object> result, string testCase)
        {
            var result2 = result.OrderBy(x => x.Key);
            var countMatches = expected.Count == result.Count();
            Assert.True(countMatches, String.Format("expected object count {0}, got object count {1} in test {2}", expected.Count, result.Count(), testCase));
            
            for (int i = 0; i < expected.Count; i++)
            {
                //Check Matching Keys
                var keyMatches = expected[i].Key == result.ElementAt(i).Key;
                Assert.True(keyMatches, String.Format("expected key name {0}, got name {1} in test {2} loop count {3}", expected[i].Key, result.ElementAt(i).Key, testCase,  i ));
                
                //If sizes were present, compare known file size vs actual (test cases would need to know these sizes)
                /*
                var sizeMatches = expected[i].Size == result.ElementAt(i).Size;
                Assert.True(keyMatches, String.Format("expected size {0}, got size {1} in test {2} loop count {3}", expected[i].Size, result.ElementAt(i).Size, testCase, i ));
                */

            }
        }


        [Fact]
        public void GetObjectResponseList_0ListObjectRequests_ReturnEmptyList()
        {
            //ARRANGE
            client.CreateEmptyBucket("S3ResponseGenTestBucket0RQ");
            sut = new ObjectResponseGenerator(client.GetClient());
            
            objRequestArgs = new List<ListObjectsRequest>();

            expectedObjectResponses = new List<ListObjectsResponse>();

            
            //ACT
            var result = sut.GetObjectResponseList(objRequestArgs).OrderBy(x => x.S3Objects[0].Key);
            
            //ASSERT
            ListObjResponsesAreSame(expectedObjectResponses, result, "1");
        }



        [Fact]
        public void GetObjectResponseList_1ListObjectRequests_ReturnList1Response()
        {
            //ARRANGE
            client.CreateBucket1Team1File("S3ResponseGenTestBucket1RQ");
            sut = new ObjectResponseGenerator(client.GetClient());
            
            objRequestArgs = new List<ListObjectsRequest>
            {
                client.GetFakeListRequestTeam1("S3ResponseGenTestBucket1RQ")
                              
            };

            expectedObjectResponses = new List<ListObjectsResponse>
            {
                client.GetFakeListResponseTeam1()
                   
            };

            
            //ACT
            var result = sut.GetObjectResponseList(objRequestArgs).OrderBy(x => x.S3Objects[0].Key);
            
            //ASSERT
            ListObjResponsesAreSame(expectedObjectResponses, result, "2");
        }




        [Fact]
        public void GetObjectResponseList_4ListObjectRequests_ReturnList4Responses()
        {
            //ARRANGE
            client.CreateBucket4TeamsMultipleFilesEach("S3ResponseGenTestBucket4RQ");
            sut = new ObjectResponseGenerator(client.GetClient());
            
            objRequestArgs = new List<ListObjectsRequest>
            {
                client.GetFakeListRequestTeam1("S3ResponseGenTestBucket4RQ"),
                client.GetFakeListRequestTeam2("S3ResponseGenTestBucket4RQ"),
                client.GetFakeListRequestTeam3("S3ResponseGenTestBucket4RQ"),
                client.GetFakeListRequestTeam4("S3ResponseGenTestBucket4RQ")
          
            };

            expectedObjectResponses = new List<ListObjectsResponse>
            {
                client.GetFakeListResponseTeam1_3Files(),
                client.GetFakeListResponseTeam2(),
                client.GetFakeListResponseTeam3(),
                client.GetFakeListResponseTeam4()
                
            };

            
            //ACT
            var result = sut.GetObjectResponseList(objRequestArgs).OrderBy(x => x.S3Objects[0].Key);
            
            //ASSERT
            ListObjResponsesAreSame(expectedObjectResponses, result, "3");
        }


    }
}
