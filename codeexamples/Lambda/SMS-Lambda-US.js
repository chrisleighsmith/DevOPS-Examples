//AWS Lamda SNS Cloudwatch Message, 'Clean & Publish' 
//Modified CS at sosc.info 7/6/19

console.log("Loading function");
var AWS = require("aws-sdk");

exports.handler = function(event, context) {
     
     // convert all lines in SNS message ojvects to a string
     var eventText = JSON.stringify(event, null, 2);
     
     //create array elements on end of each message line
     var sniped = eventText.split(",\n");
     
     //Get the 'Body' of the Message from array element
     //var body = sniped.slice(2);
     var body = sniped.slice(2);
 
     
     console.log ("The body found:",body);
     
     var MessageText = JSON.stringify(body, null, 2);
     

     // Cleanup Message output removing backslaces , Speach markers curtly brackets
     var MessageText_declutter = MessageText.replace(/\\/g, '').replace(/{/g, '').replace(/"/g, '').replace(/,/g, ',\n').replace(/\[/g, '').replace(/}/g, '').replace(/]/g, '');

     console.log ("The final message is :",MessageText_declutter);
    
     var sns = new AWS.SNS();

     var params = {
         Message: MessageText_declutter,
         Subject: "Alert From Lamda US_sendSNS",
         
         // SNS Topic Name to Publish to 
         TopicArn: "arn:aws:sns:us-east-1:942075678723:SNS_SMS"
         };
  //Final Publish message from params above
  sns.publish(params, context.done);
};

//SQS Message format, Array variables from ',\n' slice
//0 MessagId
//1 ReceiptHandle
//2 Body (and all after) * <(2)
//3 attributes ( Aproximate RecieveCount)
//4 SentTimestamp:
//5 SenderId:
//6 ApproxFirstReceiveTimestamp
//7 Message attibutes:
//8 md5OfBody:
//9 eventSource:
//10 eventSourceARN
//11 awsRegion:
