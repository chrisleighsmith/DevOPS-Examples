from flask import (
    Flask,
    render_template,
    jsonify
)
import random
import socket

hostname = socket.gethostname()

app_version = "1.0"
app = Flask(__name__, template_folder="templates")

@app.route('/')
def home():

    return render_template('home.html', hostname=hostname, version=app_version)

@app.route('/data', methods=['GET'])
def getValue():
    data = {
        'value': random.randint(0,10),
        'version': app_version,
        'hostname': hostname
    }
    return jsonify(data)

if __name__ == "__main__":
    app.run(debug=False,host="0.0.0.0",port=5000)
