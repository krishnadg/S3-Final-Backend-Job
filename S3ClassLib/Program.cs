using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
 namespace S3ClassLib {
     static class Program {
        
        //@Author Krishna Ganesan
        //Runs entire S3 backend job, ending with the output data structure
         static void Main() {

            //Initialze necessary object for injection to S3Job object
            AmazonS3Client client = new AmazonS3Client();
            string bucket = "S3TestBucket1"; //Bucket can be obtained from main arguments later..

            //Initialize S3Job object and assign it bucket
            S3Job s3JobDoer = new S3Job(client, bucket);

            //Return data structure (Dictionary<string, long>) key = Team Name, value = Team's Storage; also prints data in console
            Dictionary<string, long> dataStructure = s3JobDoer.DoS3Job();


         }
     }

 }