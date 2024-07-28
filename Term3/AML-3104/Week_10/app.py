import streamlit as st
import numpy     as np
import pandas    as pd
import pickle
import tensorflow as tf
from tensorflow.keras.models import load_model
from sklearn.preprocessing import StandardScaler, LabelEncoder, OneHotEncoder

import warnings
warnings.filterwarnings('ignore')

# load the trained model
model = load_model('model.h5')

# load all the pickle files
sc=pickle.load(open('scaled_data.pkl', 'rb'))
le=pickle.load(open('le_gender.pkl', 'rb'))
ohe=pickle.load(open('ohe_geog.pkl', 'rb'))


## Streamlit app
st.title("Customer Churn Prediction")
st.text("Enter customer details to get churn prediction") # option to enter customer details

# user input
# geography=st.selectbox('Geography', ohe.get_feature_names_out(['Geography']))
geography = st.selectbox('Geography', ohe.categories_[0])
gender=st.selectbox('Gender', le.classes_)
age=st.slider('Age', 18,99)
balance=st.number_input('Balance')
credit_score=st.number_input('Credit Score')
estimated_salary=st.number_input('Estimated Salary')
tenure=st.slider('Tenure', 0,10)
num_of_products=st.slider('Number of Products', 1, 5)
has_cr_card=st.selectbox('Has Credit Card', [0,1])
is_active_member=st.selectbox('Is Active Member', [0,1])

# Prepare the input data
input_data = pd.DataFrame({
                       'CreditScore': [credit_score],
                       'Gender': [le.transform([gender])[0]],
                       'Age': [age],
                       'Tenure': [tenure],
                       'Balance': [balance],
                       'NumOfProducts': [num_of_products],
                       'HasCrCard': [has_cr_card],
                       'IsActiveMember': [is_active_member],
                       'EstimatedSalary': [estimated_salary]
})

geo_encoded = ohe.transform([[geography]]).toarray()
geo_encoded_df = pd.DataFrame(geo_encoded, columns=ohe.get_feature_names_out(['Geography']))

input_data = pd.concat([input_data.reset_index(drop=True), geo_encoded_df], axis=1)

# Scale the input data
input_data_scaled = sc.transform(input_data)


# Predict churn
prediction = model.predict(input_data_scaled)
prediction_proba = prediction[0][0]

# display the results
# st.write("Churn probability: {}".format(predict_proba))
st.write(f'Churn Probability: {prediction_proba:.2f}')

if prediction_proba > 0.5:
    st.write('The customer is likely to churn.')
else:
    st.write('The customer is not likely to churn.')

