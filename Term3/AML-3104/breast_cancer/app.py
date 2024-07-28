import joblib
import numpy      as np
import pandas     as pd
import streamlit  as st
import tensorflow as tf
from sklearn.preprocessing import StandardScaler
import warnings
warnings.filterwarnings('ignore')

# Load the trained model
model = tf.keras.models.load_model('breast_cancer.h5')

# Load the encoders and scaler
sc_pkl = joblib.load("scaler.pkl")

# streamlit app
st.title('Breast Cancer Data Analysis')

# User input                   FALSE              TRUE
# worst concave points	       0.2654             0.0000
# worst perimeter	         184.60              59.16    
# mean concave points          0.14710            0.00000
# worst radius                25.380              9.456
# mean perimeter             122.80              47.92
# worst area                2019.0              268.6
# mean radius	              17.99               7.76
# mean area                 1001.0              181.0
# mean concavity               0.30010            0.00000
# worst concavity              0.7119             0.0000

wcp  = st.number_input('Worst concave points')
wp   = st.number_input('Worst perimeter')
mcp  = st.number_input('Mean concave points')
wr   = st.number_input('Worst radius')
mp   = st.number_input('Mean perimeter')
wa   = st.number_input('Worst area')
mr   = st.number_input('Mean radius')
ma   = st.number_input('Mean area')
mc   = st.number_input('Mean concavity')
wc   = st.number_input('Worst concavity')

# Prepare the input data
input_data = pd.DataFrame({'worst concave points': [wcp],
                           'worst perimeter'     : [wp],
                           'mean concave points' : [mcp],
                           'worst radius'        : [wr],
                           'mean perimeter'      : [mp],
                           'worst area'          : [wa],
                           'mean radius'         : [mr],
                           'mean area'           : [ma],
                           'mean concavity'      : [mc],
                           'worst concavity'     : [wc]})

# Scale the input data
input_data_scaled = sc_pkl.transform(input_data)

prediction = model.predict(input_data_scaled)
prediction_proba = prediction[0][0]

st.write(f'Breast cancer probability: {prediction_proba:.2f}')

if prediction_proba > 0.5:
   st.write('It is likely to have breast cancer')
else:
   st.write('It is not likely to have breast cancer')
