## cliWeather

### Commands

For now there are four commands implemented:

- exit &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Exit the program
- now [city] &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Show the current temperature
- weather [city] &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Show the lowest and highest temperatures for the current day
- hourly [city] &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Show the weather for the next 5 hours

### Description

I made this command line interface weather app using C# and the .NET framework. The used location API is: https://geocoding-api.open-meteo.com/v1 and the used weather API is: https://api.open-meteo.com/v1.

Any command regarding the weather will at first execute the function responsible for converting the given city into the latitude and longitude this is done by the help of the first named API. These coordinates are then used to call the weather API. The weather API gets called with different parameters, depending on the executed command.

![project picture](/assets/image.png)


### Usage

The binary files can be found under releases.  
The project does not need anything aditional to run.
