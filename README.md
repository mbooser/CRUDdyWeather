Simple C# + ASP.NET MVC Program for querying the OpenMeteo Weather API

Form Takes a Search Name, Lat/Lng, and Search Type as variables
Displays information about the current weather at the location on the table on the homepage

Name is generic description for the search

Lat is a number between +/- 80
Lng is a number between +/- 180
	Both Numbers are set to accept up to 6 decimal places of accuracy
	Numbers out of bounds or containing too many decimals will be asked to round

Search Type is an Enum used by the UrlCaller Service for determining which forcast to grab with the API

________________________________________________________________________________________________________

Known Issues:

Form incorrectly Parses Enum from form >> Disabled Values besides current;
Data layer incomplete >> All data currently stored in memory;
Detail View not called correctly >> Removed from cleaned copy of the codebase/

________________________________________________________________________________________________________

Full version of the codebase used in the Friday Demo available with the following hash:
commit: 496e8665769f4a3ae01e6b74af07ff0d639e0b01
