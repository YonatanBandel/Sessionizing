using System.Collections.Generic;

namespace Sessionizing.QueryExcecutors
{
    public class ExcecutorsUtil
    {
        // an array of all excecutors objects
        private IQueryExcecutor[] excecutors = new IQueryExcecutor [] 
        {
            new NumOfSessionsExcecutor(),
            new MedianSessionLengthExcecutor(),
            new NumOfUniqueVisitedSitesExcecutor(),
        };

        // returns a mapping from excecutor query type to excecutor object
        public Dictionary<string, IQueryExcecutor> GetExcecutorsMap()
        {
            Dictionary<string, IQueryExcecutor> mapQueryTypeToExcecutor = new Dictionary<string, IQueryExcecutor>();
            foreach (IQueryExcecutor excecutor in excecutors)
            {
                mapQueryTypeToExcecutor.Add(excecutor.getQueryType(), excecutor);
            }
            return mapQueryTypeToExcecutor;
        }
    }
}
