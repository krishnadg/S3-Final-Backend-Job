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
        protected List<string> teamNamesArgs; 
        protected List<ListObjectsRequest> expectedObjectsRequests;

        protected ObjectRequestGenerator sut;

        protected ObjResponseGenTestsBase()
        {
            // Do "global" initialization here; Called before every test method.
            client = new AWSTestClient();
            

        }

        public void Dispose()
        {
            // Do "global" teardown here; Called after every test method.
          //  client.RemoveBucketFromS3("S3TestBucket");
        }
    } 



    public class ObjectResponseGeneratorTest : ObjResponseGenTestsBase
    {
        


        [Fact]
        public void GetObjectRequestList_0TeamNames_ReturnEmptyList()
        {
            //ARRANGE
            
            
            //ACT

            //ASSERT
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
