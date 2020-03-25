var socket;
var uri = "ws://" + window.location.host + "/ws";
var output;
var numberOfFrauds = 0;

function write(s) {
    var p = document.createElement("div");
    p.innerHTML = s;
    output.appendChild(p);
}

function doConnect() {
    socket = new WebSocket(uri);
    socket.onopen = function (e) { 
        //write("opened " + uri); 
        document.getElementById("connect").hidden = true;
        document.getElementById("disconnect").hidden = false;
        document.getElementById("signal").className = "connected";
    };
    socket.onclose = function (e) { write("closed"); };
    socket.onmessage = function (e) { handleMessage(e); };
    socket.onerror = function (e) { write("Error: " + e.data); };
}

function handleMessage(e){
    var msg = JSON.parse(e.data);
    write(msg.creditCardNbr);
    
    numberOfFrauds++;
    document.getElementById("nbrOfFrauds").innerText = numberOfFrauds;
}

function doSend(text) {
    write("Sending: " + text);
    socket.send(text);
}

function onInit() {
    output = document.getElementById("output");
}

function onSend(){
    var text = document.getElementById("input").value;
    doSend(text);
}

function doClose(){
    document.getElementById("connect").hidden = false;
    document.getElementById("disconnect").hidden = true;
    document.getElementById("signal").className = "disconnected";
    socket.close();
}

window.onload = onInit;
