function getValue(){

    axios.get('/data')
      .then(function (response) {
        document.getElementById('error').style.visibility = "hidden";
        data = response.data
        document.getElementById('version').innerText = data.version;
        document.getElementById('value').innerText = data.value;
        document.getElementById('hostname').innerText = data.hostname;
        console.log(response);
      })
      .catch(function (error) {
        console.log(error);
        numErrors++;
        document.getElementById('error-number').innerText = numErrors;
        document.getElementById('error').style.visibility = "visible";
      })
}

var numErrors = 0;
setInterval(() => getValue(), 1000);