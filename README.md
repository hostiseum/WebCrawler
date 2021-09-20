# WebCrawler using .Net 5.0
## How to build the application?
<p>Once the code is downloaded you can use visual studio to build the application.</p>

## Run the application
You can run the application by simply pressing F5 or directly running the EXE.

## Input
Upon running the application, It will prompts to enter the website URL. I have used http://wiprodigital.com for my testing, it takes about 5 minutes to finish crawling. 

## Output
Once the application finished crawling, It will create output.json in the same folder from where the application run.

![image](https://user-images.githubusercontent.com/17678287/134076517-7f2019d9-6a28-46c7-b1ab-729462bb5b45.png)


## Improvements
1. MemCache service should be replaced with more scalable service like Azure Redis
2. CrawledSite object does not populate names currently like wise any information that we are interested in can be captured
3. Better algorithm like Bloom filter should be used to check if the site is already crawled.
4. Result dump can be improved.
