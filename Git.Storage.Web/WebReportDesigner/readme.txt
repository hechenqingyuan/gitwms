FastReport.Net HTML5 on-line designer (beta)
----------------------------------------

How to use HTML5 on-line designer together a WebReport:
-------------------------------------------------------

- you need copy this folder in your web application
- load report in WebReport and call RegisterData in your code
- set the properties:
        // sets unique ID for UI control, need for future save of report
	webReport.ID = "DesignReport";
	// activate Designer in WebReport
        webReport.DesignReport = true;
	// you can restrict transfer in browser and edit of script code
        webReport.DesignScriptCode = false; 
	// path to the Designer index
        webReport.DesignerPath = "~/WebReportDesigner/index.html"; 
        // path with R/W permissions for saving of designed reports    
	webReport.DesignerSavePath = "~/App_Data/DesignedReports"; 
	// path to call-back page in your web application, we make a GET query with parameters reportID="here is webReport.ID"&reportUUID="here is saved report file name"
	webReport.DesignerSaveCallBack = "~/Home/SaveDesignedReport"; 
- open WebReportDesigner\scripts\config-data.js and set the paths 
	"getReportByUUIDFrom", 
	"saveReportByUUIDTo", 
	"makePreviewByUUID" 
	according to your web-application root.
- VERY IMPORTANT! clean a cache of your browser!


Deifferences between Demo and Full versions:
--------------------------------------------
- you can not save designed report in Demo version
- full version of Designer available in FastReport.Net Professional Edition (with full sources)


HTML5 Designer limitations:
----------------------------
- this is beta and some features still in development
- design of connection strings of data sources is prohibited and they does not send in browser for security reasons
- we recommend using of Designer on a separate page and below other objects

More details about using the On-line Report Designer you can read in our article http://www.fast-report.com/en/blog/56/show/

You can see an example of implementation HTML5 Designer in web-application in our demo \Demos\C#\MvcRazor.

If you have any questions please feel free to contact us support@fast-report.com!

Have a nice day!

-- 
FastReports Team
http://www.fast-report.com
