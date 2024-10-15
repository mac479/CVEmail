Small library to send an email given message, subject, recipient, and smtp credentials. Can be called from a console app (CVEmailApp) or from a WPF app using an API (CVFrontEnd & CVEmailApi). API only features one post function to send the email.


To build project
  1. Open solution in visual studio.
  2. Configure App.config files in CVFrontEnd and CVEmailApp to include SMTP information.
  3. (Optional) Configure App.config in CVEmailApp and CVEmailAPI to set location where log file will be stored. 
    *Note: Default is same folder. Api config is solely for where it stores the log files.
  4. Make sure the start-up project is either just CVEmailApp or both CVEmailAPI and CVFrontEnd.
    *Note: CVFrontEnd won't work correctly unless both it and the api are running.

Typical post request body for the api should be formatted like so:
```
{"smtpServer": "",
  "smtpPort": ,
  "smtpUsername": "",
  "smtpPassword": "",
  "to": "",
  "subject": "",
  "message": ""
}
```
