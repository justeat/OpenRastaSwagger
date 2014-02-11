OpenRastaSwagger
================

Implementation of swagger / swagger ui for OpenRasta

## Why do I need it?

Swagger and the Swagger-UI is a nice, pretty and standardised way to express public meta-data over your RESTful API. Instead of hand-cranking documentation that rapidly gets out of data, why not let the code you execute document itself.

## Configuration

To enable OpenRastaSwagger you need to add a couple of lines to your IConfigurationSource.

Given a configuration source that looks like this:

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
		
You need to use the static configuration methods provided in the package, modifying your configuration to look like this:


		public class Configuration : IConfigurationSource
		{
			public void Configure()
			{
				using (OpenRastaConfiguration.Manual)
				{
					SwaggerConfiguration.WithHeader("Some-Default-Header-For-Swagger-UI", "Some-Value");
					SwaggerConfiguration.RegisterSwagger();

					ResourceSpace.Has.ResourcesOfType<SimpleResource>()
						.AtUri("/simple/{message}")
						.And.AtUri("/simple/?message={message}&pageNumber={pageNumber}")
						.HandledBy<SimpleHandler>()
						.AsJsonDataContract();
				}
			}
		}
		
You then need to grab the latest version of Swagger-UI from https://github.com/wordnik/swagger-ui and extract it into your OpenRasta application into a directory in the root called "swagger-ui". Open up the default index.html and make sure the Url set in the JavaScript bootstrapping code is pointing to "/api-docs/swagger" like so:

		 $(function () {
			  window.swaggerUi = new SwaggerUi({
			  url: "/api-docs/swagger",
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

	SwaggerConfiguration.WithHeader("Some-Default-Header-For-Swagger-UI", "Some-Value");
	
## Alternative root paths

You can change the uri that the swagger-ui metamodel is generated at by modifying the static property `SwaggerConfiguration.Root` before calling RegisterSwagger();

## Grouping

Two grouping methods for your resources are provided

	* GroupByUri - the default, configured by calling SwaggerConfiguration.GroupByUri();
	* GroupByResource - configured by calling SwaggerConfiguration.GroupByResource();
