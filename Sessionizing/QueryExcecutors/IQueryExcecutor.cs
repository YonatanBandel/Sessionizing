using System.Collections.Generic;

namespace Sessionizing.QueryExcecutors
{
    public interface IQueryExcecutor
    {
        string getQueryType();

        double ExcecuteQuery (List<Row> data, string queryInput);

        bool IsQueryInputLegal(List<Row> data, string queryInput);

        void LogResult(double queryResult, string queryInput);

        void LogFailiure(string queryInput);
    }
}
