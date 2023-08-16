# import libraries
import sys
import pyspark as ps
from pyspark.sql           import SparkSession
from pyspark.sql.functions import *

print ("Importing pyspark libraries...OK")
print ()

# retrieve command line arguments and store them as variables
datadir    = sys.argv[1] # gs://dataproc-staging-us-central1-321442252608-e66zqwhf/yelp/data/
outputfile = sys.argv[2] # gs://dataproc-staging-us-central1-321442252608-e66zqwhf/yelp/results
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

# Getting avg results
print ("Number of categories with rates avg greater and smaller than the general avg")
df_cat_avg.createOrReplaceTempView("df_cat_avg")
df_count = sqlContext.sql('''select (select count(1) from df_cat_avg where category_avg >= general_avg) as categ_greater_avg,
                                    (select count(1) from df_cat_avg where category_avg <  general_avg) as categ_smaller_avg''')
df_count.show()

df_cat_avg = df_cat_avg.sort(df_cat_avg.category_avg.desc(), df_cat_avg.cat_category.asc())
df_cat_avg.show()

print ("Getting avg results...OK")
print ()

# Getting standard deviation by category
df_cat_std = df_main.groupBy(df_main.cat_category).agg(stddev("stars").alias("category_std"))
df_cat_std = df_cat_std.sort(df_cat_std.category_std.desc(), df_cat_std.cat_category.asc())
df_cat_std.show()
print ("Getting standard deviation by category...OK")
print ()

# Getting top 5 categories by rating
df_tmp_avg = df_cat_avg.drop("general_avg")
df_tmp_avg = df_tmp_avg.sort(df_tmp_avg.category_avg.desc(), df_tmp_avg.cat_category.asc())
df_cat_top = df_tmp_avg.limit(5)
df_cat_top.show(truncate = False)
print ("Getting top 5 categories by rating...OK")
print ()

# Getting bottom 5 categories by rating
df_tmp_avg = df_tmp_avg.sort(df_tmp_avg.category_avg.asc(), df_tmp_avg.cat_category.asc())
df_cat_bot = df_tmp_avg.limit(5)
df_cat_bot.show(truncate = False)
print ("Getting bottom 5 categories by rating...OK")
print ()

# Joining results
df_cat_rate = df_cat_top.union(df_cat_bot)
df_cat_rate = df_cat_rate.sort(df_cat_rate.category_avg.desc(), df_cat_rate.cat_category.asc())
df_cat_rate.show()
print ("Joining results...OK")
print ()

# Getting avg rates by city and categories
df_city_avg = df_main.groupBy(df_main.city, df_main.cat_category).agg(avg("stars").alias("city_category_avg"))
df_city_avg = df_city_avg.join(df_cat_rate, on = "cat_category")
print ("Getting avg rates by city and categories...OK")
print ()

# Getting correlation between city/categories avg
stat_corr = df_city_avg.stat.corr("city_category_avg", "category_avg")
print (stat_corr)
print ("Getting correlation between city/categories avg...OK")
print ()

# Getting final results
df_city_avg = df_city_avg.withColumn("correlation", lit(stat_corr))
df_city_avg = df_city_avg.sort(df_city_avg.city.asc(), df_city_avg.cat_category.asc())
df_city_avg = df_city_avg.select(df_city_avg.city, df_city_avg.cat_category, df_city_avg.city_category_avg, df_city_avg.category_avg, df_city_avg.correlation)
df_city_avg.show()
print ("Getting final results...OK")
print ()

# Saving results
df_count.write.mode("overwrite").format("com.databricks.spark.csv").option("header", "true").csv(outputfile)
df_cat_avg.write.mode("append").format("com.databricks.spark.csv").option("header", "true").csv(outputfile)
df_cat_std.write.mode("append").format("com.databricks.spark.csv").option("header", "true").csv(outputfile)
df_cat_rate.write.mode("append").format("com.databricks.spark.csv").option("header", "true").csv(outputfile)
df_city_avg.write.mode("append").format("com.databricks.spark.csv").option("header", "true").csv(outputfile)
print ("Saving results...OK")
print ()
