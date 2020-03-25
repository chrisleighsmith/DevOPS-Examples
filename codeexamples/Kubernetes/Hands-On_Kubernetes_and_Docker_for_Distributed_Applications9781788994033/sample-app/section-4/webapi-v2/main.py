from flask import (
    Flask,
    request,
    render_template,
    jsonify,
    make_response
)
import random
import socket

hostname = socket.gethostname()

app_version = "2.0"
app = Flask(__name__, template_folder="templates")

@app.route('/')
def home():

    return render_template('home.html', hostname=hostname, version=app_version)

@app.route('/data', methods=['GET'])
def getValue():
    version = request.headers.get('x-app-version')
    if version == "2.0":
        data = {
            'meta': {
                'version': version,
                'hostname': hostname
            },
            'data': {
                'value1': random.randint(0,10),
                'value2': random.randint(0,10),
                'value3': random.randint(0,10)
            }
        }
    else:
        data = {
            'value': random.randint(0,10),
            'version': app_version,
            'hostname': hostname
        }
    resp = make_response(jsonify(data))
    resp.headers['x-app-version'] = "2.0"
    return resp

if __name__ == "__main__":
    app.run(debug=False,host="0.0.0.0",port=5000)
