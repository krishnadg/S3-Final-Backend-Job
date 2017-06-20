using System;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
namespace S3ClassLib
{
    public class Program
    {
        static void Main(string[] args)
        {

            var creds = new BasicAWSCredentials();
            var config = new AmazonS3Config();
            var setup = new AWSSetup(creds, config);
            setup.PrintListBucketsAndObjectFiles();

        }
    }
}
