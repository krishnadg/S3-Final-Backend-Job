using System;
using System.Diagnostics;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;


namespace S3ClassLib
{

    public class FinalJsonRoot
    {

        public TeamData[] results{get;set;}

        public MetaData meta{get;set;}


    }

    public class TeamData
    {

        public string team{get;set;}

        public string sub_team{get;set;}

        public long value{get;set;}

    }

    public class MetaData
    {
        public string period{get;set;}

        public string unit{get;set;}

        public float costPerUnit{get;set;}

        public string name{get;set;}

    }


}