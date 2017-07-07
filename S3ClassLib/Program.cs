using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.Json;
using Newtonsoft.Json;

 namespace S3ClassLib {
     static class Program {
        
        //@Author Krishna Ganesan
        //Runs entire S3 backend job, ending with the output data structure
        // Args in format []
         static void Main(string[] args) {

          


            
            //Initialze necessary object for injection to S3Job object
            AmazonS3Client client = new AmazonS3Client();
            
            Console.WriteLine("Received args " +args );
            string bucket = args[0]; //Bucket can be obtained from main arguments later..
            string bucketPrefix = args[1];
            //Initialize S3Job object and assign it bucket
            S3Job s3JobDoer = new S3Job(client, bucket, bucketPrefix);

            //Return data structure (Dictionary<string, long>) key = Team Name, value = Team's Storage; also prints data in console
            Dictionary<string, long> dataStructure = s3JobDoer.DoS3Job();
            
            
         }
     }

 }