

function getUser(deviceId, targetTemp, realTemp, callback) {
    var devicesUrl = "http://localhost:8080/devices";
    var params = "?username=yurii&password=kot";

    var xhttp = new XMLHttpRequest();


    xhttp.addEventListener('load', callback);
    xhttp.addEventListener('error', () => console.log("Request to "+devicesUrl+params+" failed"));

    xhttp.open("GET", devicesUrl + params, true);
    xhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xhttp.onreadystatechange = function()
    {
        document.body.innerHTML += "\n" + xmlHttp.responseText;

    }
    xhttp.send();
}

let d = new Date();
document.body.innerHTML += "<p>Today's date is " + d + "</p>"

function UserAction() {
    document.body.innerHTML += "+1 call ";
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "http://samples.openweathermap.org/data/2.5/weather?q=London,uk&appid=b6907d289e10d714a6e88b30761fae22", false);
    xhttp.setRequestHeader("Content-type", "application/json");
    xhttp.send();
            document.body.innerHTML += "\n" + xhttp.responseText;

            var xhr = new XMLHttpRequest();
            xhr.open("GET", "https://www.codecademy.com/", false);
            xhr.send();

            console.log(xhr.status);
            console.log(xhr.statusText);
            console.log(xhr.responseText);
                        document.body.innerHTML += "\n" + xhr.responseText;

}

getUser(1, function() {
    console.log(JSON.parse(this.responseText));
});