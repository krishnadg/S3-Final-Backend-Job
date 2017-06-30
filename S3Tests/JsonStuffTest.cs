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
using Newtonsoft.Json;


namespace S3Tests
{
    public class JsonStuffTest
    {
        
        partial class S3TeamStorageInfo
        {
            string teamName;
            long storageSize;

            DateTime lastDateModified;
            public S3TeamStorageInfo()
            {
                
            }
        }

        [Fact]
        public void TestJsonFast()
        {


            Dictionary<string, long> testDict = new Dictionary<string, long>
            {
                {"Team1", 1000},
                {"Team2", 2000},
                {"Team3", 3000},
                {"Team4", 4000},
                {"Team5", 5000},

            };

            var jsonString = JsonConvert.SerializeObject(testDict);


            Assert.True(true, jsonString);


        }


    }
}