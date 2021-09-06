## Steps to create and initialize the database.

1. CLI - Execute the following commands:

```
dotnet ef database update --project "src\Api\Api.csproj"
dotnet ef database update --project "tests\Application.IntegrationTests\Application.IntegrationTests.csproj"
```
			
P.S: You should have installed **dotnet-ef tool**.
	
Or

2. Visual Studio:
- Set Api project As Startup Project
- Go to Package Manager Console and execute: Update-Database
- Do the same steps for Application.IntegrationTests

## Steps to prepare the source code to build/run properly
  
1. CLI - Execute the following command:
```
dotnet run --configuration Release --project "src\Api\Api.csproj"
```
		
Or

2. Visual Studio: open project with Visual Studio and press F5.
   
P.S: If you have Postman installed you may use WebsiteManagementApi.postman_collection.json in Samples folder 
   
## Iteraction with Api
   
| HTTP Method		|		Request Payload		|	URI						        |    Response payload                   |
|-------------------|:-------------------------:|:---------------------------------:|:-------------------------------------:|
|   GET 			|    { "Username" : "admin", "Password" : "admin" }       |  api/login                        |	single user (contains bearer token) |
|	GET			    |            -              |  api/websites/{websiteid}         |	single website                      |
|	GET				|			 -			    |  api/websites                     |   website collection                  |
|	GET				|			 -			    |  api/websites/{websiteid}/image   |   website homepage snapshot           |
|    POST			|      single website       |  api/websites                     |	single website                      |
|   PUT			    |      single website       |  api/websites{websiteid}          |	       -                            |
|   DELETE			|           -               |  api/websites{websiteid}          |	       -                            |
|    OPTIONS		|           -               |  api/websites                     |	       -                            |
|    HEAD			|           -               |  api/websites                     |	       -                            |
