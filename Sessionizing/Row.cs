using System;

namespace Sessionizing
{
    // Data model representing a single row in the CSV file, the fileds corresponds to the CSV files columns
    public class Row
    {
        public string VisitorId { get; set; }
        public string SiteUrl { get; set; }
        public string PageViewUrl { get; set; }
        public long TimeStamp { get; set; }

        public Row(string[] values)
        {
            validateCtorInput(values);

            VisitorId = values[0];
            SiteUrl = values[1];
            PageViewUrl = values[2];
            parseTimeStampToLong(values[3]);
        }

        // parse 'TimeStamp' field, throw exception if parsing fail
        private void parseTimeStampToLong(string value)
        {
            try
            {
                TimeStamp = long.Parse(value);
            }
            catch (Exception)
            {
                throw new Exception("Couldn't parse a value to a number in 'TimeStamp' columns");
            }
        }

        private void validateCtorInput(string[] values)
        {
            // validate that the input CSV file contains exactly 'NUM_OF_COLUMNS_IN_CSV_FILE' columns
            if (values.Length != CONSTS.NUM_OF_COLUMNS_IN_CSV_FILE)
            {
                throw new Exception("The code supports only CSVs with 4 columns");
            }

            // validate every one of the 'NUM_OF_COLUMNS_IN_CSV_FILE' columns contains a value
            for (int i = 0; i < CONSTS.NUM_OF_COLUMNS_IN_CSV_FILE; i++)
            {
                if (String.IsNullOrEmpty(values[i]))
                {
                    throw new Exception("One (or more) column has an empty value");
                }
            }
        }
    }
}
