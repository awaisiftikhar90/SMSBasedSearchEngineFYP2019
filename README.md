# **SMSBasedSearchEngineFYP2019**

The project GSM based search engine is for low end GSM based mobile phones. It provides
facilities like search query from already searched data and Google based search returns in the
form of SMS. If query is sent to system and internet is not available or no answer is found from
both database and internet, then system diverts the user’s query to admin. A user can send SMS
to server with any query, for example “definition of artificial intelligence”.
The server has functionality to find answer of query in database. If there is no record found for
given query in database, then the server will search the query on Google and send back the
found content. The project consists of four main modules; GSM Module for sending/receiving
SMS, GSMCommServer API, SMS Server, Database/Human Expert. The application has been
developed using Visual Studio 2017. User will send request to GSM server which will use
GSMCommServer API and PDUConverter library to allow GSM based communication. Web
service is written in MVC framework to run in background, it uses GoogleSearchAPI which
addresses all the requests and navigate accordingly.
In test phase more than 50 people requested at the same time, system responded all these
requests in 10 seconds with good accuracy. In test phase the system achieved the accuracy of
70 percent. Rest of 30% is not accomplished due to user’s false requests or due to unpaid trail
version of web service.


**Tools**:
#C#
#.net
#asp_.net
#Mvc_frame_work
#ResfullAPI
#SQL
