using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using System.Collections.Generic;
using Newtonsoft.Json;

//The class used to serialize the json of whatever s3 data desired
public class S3Leaderboard
{
    Dictionary<string, long> teamNamesAndStorage;
    long totalBucketStorage;

    DateTime leaderboardDate;
    
    public S3Leaderboard(Dictionary<string, long> _teamNamesAndStorage, long _totalBucketStorage, DateTime _leaderboardDate)
    {
        teamNamesAndStorage = _teamNamesAndStorage;
        totalBucketStorage = _totalBucketStorage;
        leaderboardDate = _leaderboardDate;
    }

    public string GetJson()
    {
        return JsonConvert.SerializeObject(this);
    }


}