# rbc-demo
Improvements yet to be done
1. Add unit test case
2. Move the connection string, Queue Names,producerConfig and ConsumerConfig parameters to Secrets/Azure KeyVaults
3. Add Swagger
4. Add Versioning
5. Even the SignalURL has to be pushed to be part of secrets.

Note:- 1.Docker compose file was picked up from confluent web site for demo purpose.
       2. Uploaded the code from my laptop with my sons login unfortunately hence author appears as prahl(his name is prahlad)    


Things that were done
1. Common holds the entity(Employee), DBContext, Repository and Validator for the entity Employee
2. EmpdataProducer produces the employee data and this pumps in Employee Data randomly to Kafka
3. EmpdataConsumer consumes from Kafka and persists in DB also has the SignalRHub to publish the  employee data
4. SignalClient(mispelled missed an R) is a simple mvc application and has the java client file in \www\js\employeeSignalrClient.js file holding the client logic to 
      recieve the employee messages and the UI shows a Text Area and start/stop button to recieve the messages
5. EmpWebApi is a simple minimal API supporting CRUD actions against the Employee resource.(has that pagesize and page number based filtering logic)
