# Sessionizing

1) Solution description:

	a. Program:
		Contains the 'Main' function, validates the input, transfers the query type and query input arguments to the relevant 			excecutors
		
	b. DataManager:
		Holds the data which is taken from the CSV files
		
	c. Row:
		Data model representing a single row in the CSV file, every field in it corresponds to the CSV files columns
		
	d. CONSTS:
		Constants
		
	e. IQueryExcecutor:
		An interface for QueryExcecutors objects
		
	f. NumOfSessionsExcecutor, MedianSessionLengthExcecutor, NumOfUniqueVisitedSitesExcecutor:
		Query excecutors, contain "ExcecuteQuery" function which holds the logic for running the query
		
	g. ExcecutorsUtil:
		Holds the field of QueryExcecutors array and contains a function which returns a mapping from excecutor query type to an 		excecutor object
	
	h. UnitTest:
		Unit tests class
		
		
2) Instructions - how to run the program:

	a. Download the "Program.exe" file and the "CSVs" folder.
	
	b. Open CMD and 'cd' to the "Program.exe" path.
	
	c. The program is ready to be called in the following form:
	
	The string 'Program', followed by an even number of arguments, where the first argument of every couple has to be one of the 		followings: NumOfSessions , MedianSessionLength or NumOfUniqueVisitedSites.
	
	Examples: 
	
		Program NumOfSessions www.s_4.com
		Program MedianSessionLength www.s_6.com
		Program NumOfUniqueVisitedSites visitor_8
		*The program can be called with multiple queries, in the following form:
		Program NumOfSessions www.s_4.com MedianSessionLength www.s_6.com NumOfUniqueVisitedSites visitor_8
		
	Note: the csv files have to be in the directory of the 'Program.exe' file inside a 'CSVs' folder

3) How would I support scale:

	a. I'd save the CSV files data in a database and access it using SQL queries.
	
	b. To handle many parallel requests, I'd use a Thread pool and use Async functions.

4) Space and time complexity:
	n - the length (number of rows) of all CSV files together:

	a. Loading the data to the program (DataManager's data field):
	space: 
	o(n): every row in the file is respresented with a 'Row' object which have a finite number of fields (4)
	time:
	o(n): every CSV file is sorted, so I implemented the code with k stream readers (one for every k possible CSV files) which make comparisons and insert roes by timestamp order

	b. "ExcecuteQuery" function in objects NumOfSessionsExcecutor , MedianSessionLengthExcecutor and NumOfUniqueVisitedSitesExcecutor:
	o(n) - going over the list and for every row operates a finite number of actions
	in MedianSessionLength: given k number of sessions (which might be at worst case o(n) itself),  finding the median takes o(k^2) for sorting the k sessions, therefore worst case is o(n ^ 2)
	finding the median can be done in o(k) that will eventually brings us to o(n)

5) How I tested my code:
	Unit tests, the relevant class is attached.

6) Additional issue:

	I chose to refer to 'timestamp' column as a number because an arithmethic operation is performed on it (subtract).
	I chose to use 'long' because if I chose 'int' then in about 32 years the code will break because the number representing that future time will be larger than 'int' max limit which is 2,147,483,647

7) Screenshots:

![alt text](https://github.com/YonatanBandel/Sessionizing/blob/master/Sessionizing_screenshot.png)
