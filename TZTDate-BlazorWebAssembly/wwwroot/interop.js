"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendToUser").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    connection.invoke("GetConnectionId").then(function (id) {
        document.getElementById("connectionId").innerText = id;
    });
    var groupName = document.getElementById("groupName").value;
    connection.invoke("JoinGroup", groupName).catch(function (err) {
        return console.error(err.toString());
    });
    document.getElementById("sendToUser").disabled = false;
})
    .catch(function (err) {
        return console.error(err.toString());
    });

document.getElementById("sendToUser").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var groupName = document.getElementById("groupName").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessageToGroup", user, groupName, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});