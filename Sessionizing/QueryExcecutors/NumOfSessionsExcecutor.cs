using System;
using System.Collections.Generic;

namespace Sessionizing.QueryExcecutors
{
    class NumOfSessionsExcecutor : IQueryExcecutor
    {
        public string getQueryType() { return CONSTS.NUM_OF_SESSIONS; }

        // returns the number of sessions for the given site
        public double ExcecuteQuery(List<Row> data, string siteUrl)
        {
            Dictionary<string, long> mapVisitorToLastTimeVisit = new Dictionary<string, long>();
            int numOfSessions = 0;

            foreach (Row row in data)
            {
                if (row.SiteUrl.Equals(siteUrl))
                {
                    // if this is the visitor's first visit to the web site, increment counter and add to map
                    if (!mapVisitorToLastTimeVisit.ContainsKey(row.VisitorId))
                    {
                        mapVisitorToLastTimeVisit.Add(row.VisitorId, row.TimeStamp);
                        numOfSessions++;
                    }
                    else
                    {
                        // if the time that passed since the last visit to the current one is bigger than half an hour
                        if (row.TimeStamp - mapVisitorToLastTimeVisit[row.VisitorId] > CONSTS.HALF_AN_HOUR_IN_SECONDS)
                        {
                            numOfSessions++;
                        }
                        mapVisitorToLastTimeVisit[row.VisitorId] = row.TimeStamp;
                    }
                }
            }

            return numOfSessions;
        }

        // currently, always returns 'true'. can easily be modified to validate input if needed
        public bool IsQueryInputLegal(List<Row> Data, string siteUrl)
        {
            return true;
        }

        public void LogResult(double queryResult, string siteUrl)
        {
            Console.WriteLine("Num of sessions for site '{0}' = {1}", siteUrl, queryResult);
        }

        public void LogFailiure(string siteUrl) { }
    }
}
