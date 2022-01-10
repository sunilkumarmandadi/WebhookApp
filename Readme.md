# Airline price alert publishing app #

### Whenever there is a airline price update, it will push the alerts to Airline agent app. ###

# Airline Web #
### The Airline agents can subscribe/register to the Airline price alerts by providing an Call back API. ###
### FlightController --> When this end point is triggered, it publishes the message in RabbitMQ ###

# AirlineSendAgent #
### This app listens the events that are published in RabbitMQ. ###
### After the event received, it will fetch all the subscribed API's from DB and do a callback with the event details ###

##App Setup guide

 1. Set all three applications as startup projects
 2. Run https://localhost:5001/index.html 
 3. Subscribe Callback URL (https://localhost:6003/api/Notifications)
 4. Do a Price update from https://localhost:5001/api//api/Flights/{id}
 5.It will publishes the message in RabbitMQ 
 6. AirlineSendAgent --> This app listens the events that are published in RabbitMQ.
 7. After the event received, it will fetch all the subscribed API's from DB and do a callback with the event details (https://localhost:6003/api/Notifications)
