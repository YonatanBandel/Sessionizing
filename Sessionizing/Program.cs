using System;
using System.Collections.Generic;
using System.IO;
using Sessionizing.QueryExcecutors;

namespace Sessionizing
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, IQueryExcecutor> mapQueryTypeToExcecutor = new ExcecutorsUtil().GetExcecutorsMap();
            string csvsDirectoryPath = Directory.GetCurrentDirectory() + CONSTS.CSVS_FOLDER_SUFFIX;
            DataManager dataManager = new DataManager(csvsDirectoryPath);

            if (dataManager.IsDataEmpty())
            {
                PrintNoCsvFoundMessage(csvsDirectoryPath);
            }
            else
            {            
                if (ValidateInput(args, dataManager.Data, mapQueryTypeToExcecutor))
                {
                    IQueryExcecutor excecutor;
                    string queryType, queryInput;
                    double queryResult;

                    for (int i = 0; i < args.Length; i += 2)
                    {
                        queryType = args[i];
                        queryInput = args[i + 1];
                        excecutor = mapQueryTypeToExcecutor[queryType];

                        queryResult = excecutor.ExcecuteQuery(dataManager.Data, queryInput);
                        excecutor.LogResult(queryResult, queryInput);
                    }
                }
            }
        }

        private static bool ValidateInput(string[] args, List<Row> data, Dictionary<string, IQueryExcecutor> mapQueryTypeToExcecutor)
        {
            // validate arguments array isn't empty
            if (args.Length == 0)
            {
                Console.WriteLine("The program must recieve an input");
                return false;
            }

            // validate arguments array is not odd length
            if (IsArrayOddLength(args)) {
                Console.WriteLine("Illegal input: the program must recieve an even number of arguments");
                return false;
            }

            // validate query and query input are legal
            IQueryExcecutor excecutor;
            string queryType, queryInput;

            for (int i = 0; i < args.Length; i += 2)
            {
                // validate query type
                queryType = args[i];
                if (!mapQueryTypeToExcecutor.ContainsKey(queryType))
                {
                    PrintHowToUseTheProgramMessage(mapQueryTypeToExcecutor);
                    return false;
                }

                // validate query input
                queryInput = args[i + 1];
                excecutor = mapQueryTypeToExcecutor[queryType];
                if (!excecutor.IsQueryInputLegal(data, queryInput))
                {
                    excecutor.LogFailiure(queryInput);
                    return false;
                }
            }

            // input is valid
            return true;
        }

        private static bool IsArrayOddLength(string[] arr)
        {
            return arr.Length % 2 == 1;
        }

        private static void PrintHowToUseTheProgramMessage(Dictionary<string, IQueryExcecutor> mapQueryTypeToExcecutor)
        {
            Console.WriteLine("Illegal input, input must be in the form: Program (space) query (space) query input \n" +
                "where 'query' must be one of the following:");

            foreach(KeyValuePair<string,IQueryExcecutor> pair in mapQueryTypeToExcecutor)
            {
                Console.WriteLine("{0} ", pair.Key);
            }

            Console.WriteLine("\nLegal use example:\nProgram NumOfSessions www.s_1.com");
        }

        private static void PrintNoCsvFoundMessage(string csvsDirectoryPath)
        {
            Console.WriteLine("No CSV files were found in directory {0}", csvsDirectoryPath);
        }

    }
}
