using System;
using Amazon.S3;
using Amazon.S3.Model;

namespace S3ClassLib
{
    public class Program
    {
        static void Main(string[] args)
        {
            var setup = new AWSSetup();
            setup.PrintListBucketsAndObjectFiles();

        }
    }
}
