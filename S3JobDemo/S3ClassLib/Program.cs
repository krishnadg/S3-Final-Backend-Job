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

            var creds = new BasicAWSCredentials("Foo", "Bar");
            var config = new AmazonS3Config();
            config.ServiceURL = "http://my_s3.isolated_nw:4569";


            var setup = new AWSSetup(creds, config);
            setup.PrintListBucketsAndObjectFiles();

        }
    }
}
