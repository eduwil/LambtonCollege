import numpy as np
from PIL import Image

# tensorflow
import tensorflow as tf
from tensorflow.keras.models        import load_model
from tensorflow.keras.preprocessing import image

# flask
from flask      import Flask, request, Response, render_template, jsonify
from flask_cors import CORS

# initializing Flask app
app = Flask(__name__)

CORS(app)

wsgi_app = app.wsgi_app

# loading h5 model
model = load_model('facetracker.h5')

# defining root endpoint
@app.route('/')
def root():
    return index()  # redirecting to index page

# defining index page endpoint
@app.route('/index.html')
def index():
    return render_template('index.html')    # rendering from template

# defining REST API endpoint for prediction
@app.route('/api/prediction', methods=['POST'])
def api_prediction_post():
    try:
       # data_json = request.json
       
       for file in request.files:
           print (file)
       
       if 'fileImg' in request.files:
           # retrieving sent image
           file = request.files['fileImg']
           
           # Reading the image via file.stream
           img = Image.open(file.stream)
              
           # Image preprocessing
           img = img.resize((120, 120)) 
           img_array = image.img_to_array(img)
           img_array = img_array / 255
           img_array = np.expand_dims(img_array, axis = 0)
           
           # predicting
           yhat = model.predict(img_array)
           
           # getting class (softmax) prediction
           classNumber = np.argmax(yhat[0][0])
           
           if yhat[0][0][classNumber] > 0.9:
              # getting regression prediction
              sample_coord = yhat[1][0]
              
              if classNumber == 0:
                 className = "Eduardo"
              elif classNumber == 1:
                   className = "Leo"
           else:
              sample_coord = [0, 0, 0, 0]
              className = ""

           print ('*** PREDICTION ***')
           print (className, sample_coord)
           
           response = {"result" : className,
                       "coord0" : (float(sample_coord[0]) * 480),
                       "coord1" : (float(sample_coord[1]) * 480),
                       "coord2" : (float(sample_coord[2]) * 480),
                       "coord3" : (float(sample_coord[3]) * 480)}
           status_code = 200
       else:
           error_msg = 'ERROR: fileImg has not been sent'
           response = {"result" : error_msg}
           status_code = 400
                     
    except Exception as ex:
          # returning the ERROR result as a JSON and 400 Bad Request
          error_msg = 'ERROR: ' + str(ex.args[0])
          response = {"result" : error_msg}
          status_code = 400
    finally:
          return jsonify(response), status_code
          
# defining the context for debugging
# if __name__ == '__main__':
#    app.run(debug=True)
