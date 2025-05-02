
    // Establish the connection to the Hub
    const connection = new signalR.HubConnectionBuilder()
    .withUrl("/NotifyHubs")
    .build();

    // Start the connection
    connection.start().then(function () {
        console.log("SignalR connected");
    }).catch(function (err) {
        console.error("SignalR connection error:", err.toString());
    });

    // Functionality when button is pressed
    document.getElementById("sendButton").addEventListener("click", function () {
        console.log("Button pressed, sending message...");
    connection.invoke("SendMessage", "home Page!").catch(function (err) {
        console.error("Error sending message:", err.toString());
        });
    });

    // Receive message from the server
    connection.on("ReceiveMessage", function (message) {
        console.log("Message received:", message);
    document.getElementById("messageDisplay").innerText = message;
    });
