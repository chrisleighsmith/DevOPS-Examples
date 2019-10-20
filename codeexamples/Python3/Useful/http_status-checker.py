#!/usr/bin/env python3
#Name: http_status_checker.py 
#Description: To test external services http status code changes (using a proxy)
# and log to Slack Channel if status changes from '200' or Timeout limit exceeded (defined by webhooks setup in Slack app)

#Versions: 
# 1.2 @chrisleighsmith Loops through list or URL'S
# 1.3 @chrisleighsmith Reduced Prints only on Errors
# 1.4 @chrisleighsmith use pycurl instead of urllib for accurate http status codes and http response time checks
# 1.4a @chrisleighsmith Bug Fix's: Create Slack Message Function to prevent messages not being sent & Report double faults
# 1.5 @chrisleighsmith Proxy feature via public 

#Refs: https://api.slack.com/apps/AGD1BJ75H/incoming-webhooks
#      https://api.slack.com/docs/message-attachments
#      http://pycurl.io/docs/latest/quickstart.html
#      https://stackoverflow.com/questions/9445489/performing-http-requests-with-curl-using-proxy

#test curl -s -o /dev/null -w "%{http_code}" https://auth.uber.com/login/ #check this gives 200 Status

import pycurl
import requests 
import json

#Test Curl slack message: 'curl -X POST -H 'Content-type: application/json' --data '{"text":"Http response error. Uber or USGov issues"}' https://hooks.slack.com/services/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 
def Post_Slack_Alert (slack_message,slack_text):
  webhook_url = 'https://hooks.slack.com/services/T0ZxxxxxxGG/BGCxxxx/oRWth3i5lCtxxxxxxxxxxxxiVg6VO2w3N'
  slack_data = {'attachments': [
       {  
        "pretext": "http_status-checker.py:",
        #Shorten url for display reasons
        "author_name": (url_req[0:30]),
        "title": (slack_message),
        "title_link": url_req,
        "text" : (slack_text),    
        }   
                              ]    
            }
  response = requests.post(
  webhook_url, data=json.dumps(slack_data),
  headers={'Content-Type': 'application/json'}
  ) 
  ##Check Response back from Slack
  if response.status_code != 200:
    raise ValueError(
       'Request to slack returned an error %s, the response is:\n%s'
       % (response.status_code, response.text)
   )
   
# print (slack_message)

# Dictionary of hosts to be tested: With Expected http responses
urls = {
"https://auth.uber.com/login/": "200",
# "https://login.uber.com/oauth/v2/authorized": "302",  # Redirected to auth.uber.com see Above
"https://api.trade.gov/consolidated_screening_list/search?api_key=5ka2llAxxxxxxbgDnzmH&name=Ralph%20SCOTT&fuzzy_name=true&countries=GB&size=100&offset=0&type=Individual&address=SEAFORD": "200"
}


# Loop through urls Dictionary, assigning key to url_req and value to url_value_expected 
for key in urls:
  url_req = (key)
  url_value_expected = (urls[key])
  expected_response = int(url_value_expected)
  
  # Request URLS
  try:
    from io import BytesIO
  except ImportError:
    from StringIO import StringIO as BytesIO
  buffer = BytesIO()
  c = pycurl.Curl()
  c.setopt(c.URL, url_req ) # Use Dictionary List of URLS
  c.setopt(c.WRITEDATA, buffer)
  c.perform()

  # Get HTTP response code, e.g. 200.
  http_response_code = ('%d' % c.getinfo(c.RESPONSE_CODE))
  #print (http_response_code)

  # Get HTTP response time
  http_response_time = ('%f' % c.getinfo(c.TOTAL_TIME))
  print ("URL tested:")
  print (url_req, http_response_time)
  print ("------------------------------")
  # getinfo must be called before close.
  c.close()

  returned_http_code = int(http_response_code)
  returned_http_time = float(http_response_time)

  # Send Alert to Slack if http response codes dont match 'normal' expected values  
  if (expected_response != returned_http_code) :
    slack_message = "Bad Status"
    slack_text = (returned_http_code) 
  # Post_Slack_Message now
    Post_Slack_Alert (slack_message,slack_text)
     
  if returned_http_time >= 1.2:
    slack_message = "Response is slow"
    slack_text = (returned_http_time)
  # Post_Slack_Message now
    Post_Slack_Alert (slack_message,slack_text)
  
    ##for element in slack_message:
    ##  print(element)
#else print (url_req,"OK")
