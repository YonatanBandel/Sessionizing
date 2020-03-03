using System;
using System.Collections.Generic;

namespace Sessionizing.QueryExcecutors
{
    class NumOfUniqueVisitedSitesExcecutor : IQueryExcecutor
    {
        public string getQueryType() { return CONSTS.NUM_OF_UNIQUE_VISITED_SITES; }

        // returns the number of unique visited sites by the given visitor
        public double ExcecuteQuery(List<Row> data, string visitorId)
        {
            List<string> visitedSites = new List<string>();
            int uniqueVisitedSites = 0;

            foreach (Row row in data)
            {
                if (row.VisitorId.Equals(visitorId))
                {
                    // if this is the visitor's first visit to the web site, increment counter and add to List
                    if (!visitedSites.Contains(row.SiteUrl))
                    {
                        visitedSites.Add(row.SiteUrl);
                        uniqueVisitedSites++;
                    }
                }
            }

            return uniqueVisitedSites;
        }

        // currently, always returns 'true'. can easily be modified to validate input if needed
        public bool IsQueryInputLegal(List<Row> Data, string visitorId)
        {
            return true;
        }

        public void LogResult(double queryResult, string visitorId)
        {
            Console.WriteLine("Num of unique sites for '{0}' = {1}", visitorId, queryResult);
        }

        public void LogFailiure(string visitorId) { }
    }
}
