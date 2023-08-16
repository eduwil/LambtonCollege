# import libraries
import sys
import pyspark as ps
from pyspark.sql           import SparkSession
from pyspark.sql.functions import *

print ("Importing pyspark libraries...OK")
print ()

# retrieve command line arguments and store them as variables
datadir    = sys.argv[1] # gs://dataproc-staging-us-central1-321442252608-e66zqwhf/yelp/data/
outputfile = sys.argv[2] # gs://dataproc-staging-us-central1-321442252608-e66zqwhf/yelp/results_mapr
print ("Retrieving command line arguments and store them as variables...OK")
print ()

# Defining spark/sql context
sqlContext = SparkSession.builder.getOrCreate()
print ("Defining spark/sql context...OK")
print ()

# loading csv files
df_reviews    = sqlContext.read.format('com.databricks.spark.csv').options(header = 'true', inferschema = 'true').load(datadir + 'reviews.csv')
df_categ_rest = sqlContext.read.format('com.databricks.spark.csv').options(header = 'true', inferschema = 'true').load(datadir + 'rest_categories.csv')
df_categ      = sqlContext.read.format('com.databricks.spark.csv').options(header = 'true', inferschema = 'true').load(datadir + 'categories.csv')
print ("Loading csv files...OK")
print ()

# Getting restaurant categories
lst_rest_cat = df_categ_rest.select("category").rdd.flatMap(lambda x: x).collect()
print ("Getting restaurant categories...OK")
print ()

# Filtering by restaurant categories
df_categ = df_categ.where(df_categ.cat_category.isin(lst_rest_cat))
print ("Filtering by restaurant categories...OK")
print ()

# Joining necessary tables to get the required information
df_main = df_reviews.join(df_categ, df_reviews.business_id == df_categ.cat_business_id, "inner")
print ("Joining business reviews and categories...OK")
print ()

# Getting general avg of ratings
df_avg = df_main.agg(avg(df_main.stars).alias("general_avg"))
df_avg.show()
print ("Getting general avg of ratings...OK")
print ()

# Getting avg rate by category
df_cat_avg = df_main.groupBy(df_main.cat_category).agg(avg("stars").alias("category_avg"))
df_cat_avg = df_cat_avg.join(df_avg)
print ("Getting avg rates by category...OK")
print ()

# Getting categories over and under the general avg
df_cat_avg_over  = df_cat_avg.where(df_cat_avg.category_avg >= df_cat_avg.general_avg)
df_cat_avg_under = df_cat_avg.where(df_cat_avg.category_avg <  df_cat_avg.general_avg)
print ("Getting categories over and under the general avg...OK")
print ()

# Getting reviews from categories over the general avg
df_main_over = df_main.join(df_cat_avg_over, on = "cat_category")
df_main_over = df_main_over.select(df_main_over.review_text)
print ("Getting reviews from categories over the general avg...OK")
print ()

# Getting reviews from categories under the general avg
df_main_under = df_main.join(df_cat_avg_under, on = "cat_category")
df_main_under = df_main_under.select(df_main_under.review_text)
print ("Getting reviews from categories under the general avg...OK")
print ()

# Saving results
df_main_over.write.mode("overwrite").format("com.databricks.spark.csv").option("header", "false").csv(outputfile)
df_main_under.write.mode("append").format("com.databricks.spark.csv").option("header", "false").csv(outputfile)
print ("Saving results...OK")
print ()
