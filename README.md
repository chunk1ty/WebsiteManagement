1. Steps to create and initialize the database.
	1.1. Execute the following commands:
			dotnet ef database update --project "src\Api\Api.csproj"
			dotnet ef database update --project "tests\Application.IntegrationTests\Application.IntegrationTests.csproj"
			
		P.S: You should have installed dotnet-ef tool.
	
	or
	1.2 Visual Studio
			Set Api project As Startup Project
			Go to Package Manager Console and execute: Update-Database
			Do the same steps for Application.IntegrationTests

2. Steps to prepare the source code to build/run properly
   2.1. Execute the following command:
			dotnet run --configuration Release --project "src\Api\Api.csproj"
			
   or
   2.2. Open project with Visual Studio.
   
   P.S: If you have Postman installed you may use WebsiteManagementApi.postman_collection.json in Samples folder 
   
   Iteraction with Api
   
   HTTP Method		|		Request Payload		|	URI						        |    Response payload
====================================================================================================================
    			    |    user (see below)       |  api/login                        |	single user (contains bearer token)
	GET			    |            -              |  api/websites/{websiteid}         |	single website
					|			 -			    |  api/websites                     |   website collection
					|			 -			    |  api/websites/{websiteid}/image   |   website homepage snapshot
====================================================================================================================
    POST			|      single website       |  api/websites                     |	single website
====================================================================================================================
    PUT			    |      single website       |  api/websites{websiteid}          |	       -
====================================================================================================================
    DELETE			|           -               |  api/websites{websiteid}          |	       -
====================================================================================================================
    OPTIONS			|           -               |  api/websites                     |	       -
====================================================================================================================
    HEAD			|           -               |  api/websites                     |	       -

  user payload:   
   {
		"Username" : "admin",
		"Password" : "admin"
   }
