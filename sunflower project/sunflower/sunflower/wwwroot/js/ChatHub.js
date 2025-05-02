
//"use strict";

//var connection = new signalR.HubConnectionBuilder().withUrl("/NotiHubs").build();

////Disable the send button until connection is established.
//document.getElementById("sendButton").disabled = true;

//connection.on("ReceiveMessage", function (message) {
//    var li = document.createElement("li");
//    document.getElementById("messagesList").appendChild(li);
//    // We can assign user-supplied strings to an element's textContent because it
//    // is not interpreted as markup. If you're assigning in any other way, you 
//    // should be aware of possible script injection concerns.
//    li.textContent = message;
//});

//connection.start().then(function () {
//    document.getElementById("sendButton").disabled = false;
//}).catch(function (err) {
//    return console.error(err.toString());
//});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    // var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", "this is a test message").catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});
 

     const connection = new signalR.HubConnectionBuilder()
         .withUrl("/notificationHub")
         .configureLogging(signalR.LogLevel.Information) // Enable logging for debugging
         .build();

     connection.start()
         .then(function () {
             console.log("SignalR Connected");
         })
         .catch(function (err) {
             console.error("SignalR Connection Error: " + err.toString());
         });

     // Listen for messages from the hub
     connection.on("ReceiveMessage", function (message) {
         console.log("Received message: " + message);
         // Optionally display the message in the UI
         alert(message); // For example, show it as an alert
     });

     document.getElementById('confirm-link').addEventListener('click', function (event) {
         event.preventDefault(); // Prevent default behavior of the link

         console.log("Confirm link clicked!");

         // Call the SendMessage method in your NotificationHub on click
         connection.invoke("SendMessage")
             .then(function () {
                 console.log("Message sent successfully!");
             })
             .catch(function (err) {
                 console.error("SignalR Invoke Error: " + err.toString());
             });

         // Redirect to confirmation URL
         window.location.href = event.target.href;
     });
