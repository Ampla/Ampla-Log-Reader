Ampla-Log-Reader
================

The Ampla software logs information in a number of places.  These include the WCF Logs, the Production Analyst Logs, and the Windows Event Log.

It can be difficult to combine this information and get a high level summary of any issues that are occuring. The project looks to provide a summary of some of the log files.

In the future, common issue can be addressed to provide indication about the health of the Ampla service.

WCF Logs
===
Provides information about the number of calls, errors and types of messages from the Wcf Logs.

Production Analyst Logs (Remoting Logs)
===
Provides summary information about the remoting calls between Ampla Server and Production Analyst clients

Event Logs (Windows Event Logs)
===
Provides summary information from machine event logs. 

To Run 
===

Output all Wcf and Remoting logs from all the project logs in %ProgramData%\Citect\Ampla\Projects
```
Ampla.LogReader.Console.exe
```

Each Project will output a {ProjectName}.Wcf.xlsx and {ProjectName}.Remoting.xlsx.



Reports
===
The Ampla Log Reader outputs its findings in Reports.  Reports provide high level statistics about the entries in each of the logs.

A list of current reports include:
  - WCF Reports
	- Wcf Summary Report
	- Wcf Action Report 
	- Wcf Fault Report
	- Wcf Url Report
	- Wcf Hourly Summary Report
	- Wcf Details Report

  - Event Log Reports
    - Event Log Summary Report
	- Event Log Hourly Summary Report
	- Event Log Details Report
	
  - Remoting Reports
    - Remoting Summary Report
    - Remoting Identity Report
	
Most reports provide group the events using different dimension and provide the following statistics
 - Count per category
 - Number of errors per category
 - Percentage errors
 - First record
 - Last record
 - Total Duration 
 - Average duration
 
[![Bitdeli Badge](https://d2weczhvl823v0.cloudfront.net/Ampla/ampla-wcf-reader/trend.png)](https://bitdeli.com/free "Bitdeli Badge")

