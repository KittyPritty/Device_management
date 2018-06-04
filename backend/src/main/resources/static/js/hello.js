
var devices
var xhr = new XMLHttpRequest();
xhr.open('GET', "http://127.0.0.1:8080/devices?username=yurii&password=kot", true);
xhr.send();

xhr.onreadystatechange = processRequest;

String.prototype.format = function()
{
   var content = this;
   for (var i=0; i < arguments.length; i++)
   {
        var replacement = '{' + i + '}';
        content = content.replace(replacement, arguments[i]);
   }
   return content;
};

function processRequest(e) {
    if (xhr.readyState == 4 && xhr.status == 200){
        console.log("xhr.responseText-> " + xhr.responseText + " <-")
        window.devices = JSON.parse(xhr.responseText);

        var table = document.getElementById('table');
        table.innerHTML += "<tr><th>Id</th> <th>Name</th> <th>Target &deg;C</th><th>Real &deg;C</th></tr>";

        for(var i in window.devices) {
            var device = window.devices[i]
            table.innerHTML +=
            ('<tr> <td>{0}</td> <td>{1}</td>  <td>{2}'
            + '<div class="btn-group"> <button onClick="onMinusClick(\'' + i + '\')" class="button">-1</button> <button onClick="onPlusClick(\'' + i + '\')" class="button">+1</button> </div>'
            + '</td><td>{3} </td></tr>')
            .format(device.deviceId, device.name, device.targetTemperature, device.realTemperature);
        }
    }
}

function onMinusClick(i){
    device = window.devices[i]
    console.log("wtf")

    var devices
    var request = new XMLHttpRequest();
    request.open('GET', "http://127.0.0.1:8080/updateTargetTemperature?username=yurii&password=kot&deviceId={0}&temp={1}".format(device.deviceId, device.targetTemperature - 1), true);
    request.send();

    request.onreadystatechange = function(e){
        var table = document.getElementById('table');
        table.innerHTML = ""
//        xhr.send();
    };

}

function onPlusClick(i){
     device = window.devices[i]
    console.log("wtf")

        var devices
        var request = new XMLHttpRequest();
        request.open('GET', "http://127.0.0.1:8080/updateTargetTemperature?username=yurii&password=kot&deviceId={0}&temp={1}".format(device.deviceId, device.targetTemperature + 1), true);
        request.send();

        request.onreadystatechange = function(e){
            var table = document.getElementById('table');
            table.innerHTML = ""
//            xhr.send();
        };
}