using System;
using System.Collections.Generic;

namespace Sessionizing.QueryExcecutors
{
    class MedianSessionLengthExcecutor : IQueryExcecutor
    {
        public string getQueryType() { return CONSTS.MEDIAN_SESSION_LENGTH; }

        // returns the median of sessions length (in seconds) for the given site
        public double ExcecuteQuery(List<Row> data, string siteUrl)
        {
            // this could have been done with a single mapping from string 'VisitorId' to two 'long's timestamps
            Dictionary<string, long> mapVisitorToLastTimeVisit = new Dictionary<string, long>();
            Dictionary<string, long> mapVisitorToFirstVisitInLastSession = new Dictionary<string, long>();
            List<long> sessions = new List<long>();

            foreach (Row row in data)
            {
                if (row.SiteUrl.Equals(siteUrl))
                {
                    // if this is the visitor's first visit to the web site, add to both maps
                    if (!mapVisitorToLastTimeVisit.ContainsKey(row.VisitorId))
                    {
                        mapVisitorToLastTimeVisit.Add(row.VisitorId, row.TimeStamp);
                        mapVisitorToFirstVisitInLastSession.Add(row.VisitorId, row.TimeStamp);
                    }
                    else
                    {
                        // if the time that passed since the last visit to the current one is bigger than half an hour
                        if (row.TimeStamp - mapVisitorToLastTimeVisit[row.VisitorId] > CONSTS.HALF_AN_HOUR_IN_SECONDS)
                        {
                            // add the last session length and update map
                            sessions.Add(mapVisitorToLastTimeVisit[row.VisitorId] - mapVisitorToFirstVisitInLastSession[row.VisitorId]);
                            mapVisitorToFirstVisitInLastSession[row.VisitorId] = row.TimeStamp;
                        }
                        mapVisitorToLastTimeVisit[row.VisitorId] = row.TimeStamp;
                    }
                }
            }

            // adding all the sessions which weren't added yet and still exist in the map
            foreach (KeyValuePair<string, long> pair in mapVisitorToLastTimeVisit)
            {
                sessions.Add(pair.Value - mapVisitorToFirstVisitInLastSession[pair.Key]);
            }

            // special case when exactly one session was found (prevents an index exception)
            if (sessions.Count == 1)
            {
                return sessions[0];
            }

            // calculating the sessions median
            return calcMedianSession(sessions);
        }

        private double calcMedianSession(List<long> sessions)
        {
            sessions.Sort((x, y) => x.CompareTo(y));
            int mid = sessions.Count / 2;
            double median = (sessions.Count % 2 == 1) ? sessions[mid] : (double)(sessions[mid - 1] + sessions[mid]) / 2;
            return median;
        }

        public bool IsQueryInputLegal(List<Row> data, string siteUrl)
        {
            return doesDataContainSiteUrl(data, siteUrl);
        }

        private bool doesDataContainSiteUrl(List<Row> data, string siteUrl)
        {
            foreach (Row row in data)
            {
                if (row.SiteUrl.Equals(siteUrl))
                {
                    return true;
                }
            }
            return false;
        }

        public void LogResult(double queryResult, string siteUrl)
        {
            Console.WriteLine("Median session length for site '{0}' = {1}", siteUrl, queryResult);
        }

        public void LogFailiure(string siteUrl)
        {
            Console.WriteLine("Data doesn't contain site url '{0}', therefore a median can't be computed", siteUrl);
        }
    }
}
