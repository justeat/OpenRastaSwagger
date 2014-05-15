OpenRastaSwagger
================
_Swagger and Swagger ui for OpenRasta cooked up by JUST EAT_

---

* Configuration
  + OpenRasta Routing
  + ASP.NET
* Installing and configuring Swagger UI
* Conventions
* Extensibility
  + Required headers
  + Alternative root paths
  + Grouping
* Contributing
* Copyright

Swagger and the Swagger-UI are a nice, pretty and standardised way to express public meta-data over your RESTful API. 
Instead of hand-cranking documentation that rapidly gets out of date, let the code you execute document itself.

## Configuration

To enable OpenRastaSwagger you need to add the endpoints to your service that will provide the Swagger JSON - these can be either OpenRasta routes, or an .aspx file.

### Configuration - OpenRasta Routing 

To enable the OpenRasta routes, add a couple of lines to your IConfigurationSource.

Given a configuration source that looks like this:
```c#
public class Configuration : IConfigurationSource
{
	public void Configure()
	{
		using (OpenRastaConfiguration.Manual)
		{
			ResourceSpace.Has.ResourcesOfType<SimpleResource>()
				.AtUri("/simple/{message}")
				.And.AtUri("/simple/?message={message}&pageNumber={pageNumber}")
				.HandledBy<SimpleHandler>()
				.AsJsonDataContract();
		}
	}
}
```	
You need to use the static configuration methods provided in the package, modifying your configuration to look like this:

```c#
public class Configuration : IConfigurationSource
{
	public void Configure()
	{
		using (OpenRastaConfiguration.Manual)
		{
			SwaggerGenerator.Configuration.AddRequiredHeader("Some-Header", "Some-Value");
			SwaggerGenerator.Configuration.RegisterSwaggerHandler();

			ResourceSpace.Has.ResourcesOfType<SimpleResource>()
				.AtUri("/simple/{message}")
				.And.AtUri("/simple/?message={message}&pageNumber={pageNumber}")
				.HandledBy<SimpleHandler>()
				.AsJsonDataContract();
		}
	}
}
```	

Calls can also be chained:

```c#
SwaggerGenerator.Configuration
				.AddRequiredHeader("Some-Default-Header-For-Swagger-UI", "Some-Value")
				.RegisterSwaggerHandler();		
```		
### Configuration - ASP.NET 

Copy swagger.aspx from OpenRastaSwagger.SampleApi/swagger.aspx into your web application.

If you're using a custom IDependencyResolver, then set that to the SwaggerConfiguration.Resolver static property in your Application_Start.


## Installing and configuring Swagger UI

You then need to grab the latest version of Swagger-UI from https://github.com/wordnik/swagger-ui and extract the /dist folder into your OpenRasta application into a directory in the root called "swagger-ui". Open up the default index.html and make sure the Url set in the JavaScript bootstrapping code is pointing to "/api-docs/swagger" like so:

```javascript

	 $(function () {
		  window.swaggerUi = new SwaggerUi({
		  url: "/api-docs/swagger"  //or "swagger.aspx" for aspx routing,
		  dom_id: "swagger-ui-container",
		  supportedSubmitMethods: ['get', 'post', 'put', 'delete'],
		  onComplete: function(swaggerApi, swaggerUi){
			if(console) {
			  console.log("Loaded SwaggerUI")
			}
			$('pre code').each(function(i, e) {hljs.highlightBlock(e)});
		  },
		  onFailure: function(data) {
			if(console) {
			  console.log("Unable to Load SwaggerUI");
			  console.log(data);
			}
		  },
		  docExpansion: "none"
		});

```

			
For the aspx routing method, change the url to be "swagger.aspx".

At this point, visiting http://your-api.com/swagger-ui should load up an interactive set of documentation for your application.

## Conventions

We can only auto-discover so much from the reflected codebase, so in order to emit some swagger metadata, you need to supply hints as attributes on your OpenRasta handlers.

Usage:

* OpenRastaSwagger.DocumentationSupport.NotesAttribute - Add notes to your API docs
* OpenRastaSwagger.DocumentationSupport.PossibleResponseCodeAttribute - Add status code / descriptions to the docs
* OpenRastaSwagger.DocumentationSupport.ResponseTypeIsAttribute - To give the documentation a hint as to the actual DTO type returned by your handler, used when you're returning `OperationResult`s

## Extensibility

### Required headers

You can specify required headers in your `IConfigurationSource` by calling 

	SwaggerGenerator.Configuration.AddRequiredHeader("Some-Name", "Some-Value");
	
### Alternative root paths

You can change the uri that the swagger-ui metamodel is generated at by modifying the static property `SwaggerGenerator.Configuration.Root` before calling RegisterSwagger();

### Grouping

Two grouping methods for your resources are provided

	* GroupByUri - the default, configured by calling SwaggerGenerator.Configuration.GroupByUri();
	* GroupByResource - configured by calling SwaggerGenerator.Configuration.GroupByResource();
	
## Contributing

If you find a bug, have a feature request or even want to contribute an enhancement or fix, please follow the contributing guidelines included in the repository.

## Copyright

Copyright 2014 Just Eat, Inc. or its affiliates. All Rights Reserved.

Licensed under the Apache License, Version 2.0 (the "License"). You
may not use this file except in compliance with the License. A copy of
the License is located in the LICENSE file in this repository. 

This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
CONDITIONS OF ANY KIND, either express or implied. See the License 
for the specific language governing permissions and limitations under 
the License.

OpenRasta is licensed under the MIT license: http://opensource.org/licenses/MIT
and compiled binaries are included in this repository for build purposes.
OpenRasta is Copyright Sebastien Lambla 2007-2014.

The OpenRasta project wiki is available here: https://github.com/openrasta/openrasta/wiki