function getValue(){
    axios(
      {
        url: '/data',
        headers: {
          'x-app-version': '2.0'
        },
        data: {}
      })
      .then(function (response) {
        document.getElementById('error').style.visibility = "hidden";

        console.log(response);
        version = response.headers['x-app-version']
        console.log(version);
        if(version == '2.0'){
          resp = response.data
          document.getElementById('hostname').innerText = resp.meta.hostname;
          document.getElementById('version').innerText = resp.meta.version;
          document.getElementById('value1').innerText = resp.data.value1;
          document.getElementById('value2').innerText = resp.data.value2;
          document.getElementById('value3').innerText = resp.data.value3;
        } else {
          data = response.data
          document.getElementById('hostname').innerText = data.hostname;
          document.getElementById('version').innerText = data.version;
          document.getElementById('value1').innerText = data.value;
          document.getElementById('value2').innerText = 'N/A';
          document.getElementById('value3').innerText = 'N/A';
        }
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