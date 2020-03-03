using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sessionizing
{
    class DataManager
    {
        public List<Row> Data { get; set; }

        public DataManager(string csvsDirectoryPath)
        {
            Data = new List<Row>();
            populateData(csvsDirectoryPath);
        }

        // poplulate the 'Data' with the CSV files data from 'csvsDirectoryPath' folder
        // the CSV files in that folder can be dinamicly changed and the code will adapt automaticly
        private void populateData(string csvsDirectoryPath)
        {
            string[] csvFileNamePaths = getAllCsvFilePaths(csvsDirectoryPath);
            int numOfCsvFiles = csvFileNamePaths.Length;
            StreamReader[] streamReaders = new StreamReader [numOfCsvFiles];
            Row[] rowsToInsertToData = new Row [numOfCsvFiles];

            for (int i = 0; i < numOfCsvFiles; i++)
            {
                streamReaders[i] = new StreamReader(csvFileNamePaths[i]);
                readLineAndAddRowToArray(streamReaders, rowsToInsertToData, i);
            }
            int smallestRowIndex;
            while ((smallestRowIndex = findSmallestRowIndex(rowsToInsertToData)) != -1)
            {
                // add to 'Data' the Row with the smallest 'TimeStamp'
                Data.Add(rowsToInsertToData[smallestRowIndex]);
                // add to 'rowsToInsertToData' a new Row from the relevant stream reader
                readLineAndAddRowToArray(streamReaders, rowsToInsertToData, smallestRowIndex);
            }
        }

        internal bool IsDataEmpty()
        {
            return Data.Count == 0;
        }

        // return the index of the Row which has the smallest 'TimeStamp', or -1 if the array is empty
        private int findSmallestRowIndex(Row[] rowsArray)
        {
            int smallestRowIndex = -1;
            long smallestTimeStamp = long.MaxValue;
            for (int i = 0; i < rowsArray.Length; i++)
            {
                if (rowsArray[i] != null)
                {
                    if (rowsArray[i].TimeStamp < smallestTimeStamp)
                    {
                        smallestTimeStamp = rowsArray[i].TimeStamp;
                        smallestRowIndex = i;
                    }
                }
            }

            return smallestRowIndex;
        }

        // stream reader reads a single line and place a new Row in the 'rowsToInsertToData' array
        // if the stream reader reaced EOF it places a NULL in the 'rowsToInsertToData' array
        private void readLineAndAddRowToArray(StreamReader[] streamReaders, Row[] rowsToInsertToData, int index)
        {
            string line;

            if ((line = streamReaders[index].ReadLine()) != null)
            {                
                rowsToInsertToData[index] = createRowFromStringLine(line);
            }
            else
            {
                rowsToInsertToData[index] = null;
            }
        }

        // returns a new Row which is created from a string line in a CSV format
        // Row's c'tor works only with string array of length 4 
        private Row createRowFromStringLine(string lineInCsvFormat)
        {
            return new Row(lineInCsvFormat.Split(CONSTS.DELIMITER));
        }

        // returns CSV file paths in the given directory, if directory doesn't exist returns empty array
        private string[] getAllCsvFilePaths(string directory)
        {
            if (Directory.Exists(directory))
            {
                string[] filesInDir = Directory.GetFiles(directory);
                string[] csvFilesNames = filesInDir.Where(f => f.Contains(".csv")).ToArray();
                return csvFilesNames;
            }
            else
            {
                return new string [] { };
            }
        }
    }
}