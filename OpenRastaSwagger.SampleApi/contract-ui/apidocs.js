// Add new APIs to this array.
var api_names = ["basket"];

$(document).ready(function() {
	$(api_names).each(function(index, value) {
		 
		var api_contract_url = "../api-docs/contract";
		
		function getOperationStatusCssClass(operation) {
			var statusClass = "notStarted";
			if(operation.status && operation.status == "ProductionReady") {
				statusClass = "ready";
			}else if(operation.status) {
				statusClass = "inProgress";
			}
			return statusClass;
		};
		
		$.getJSON(api_contract_url, function(contract) {
		
			// Add API to the Table of Contents
			var toc_entry_id = "toc_api_" + value;
			var toc_entry_selector = "#" + toc_entry_id;
			$("#toc ul").append("<li><a href='#" + value + "' id='" + toc_entry_id + "'>" + contract.api + "</a></li>");
			
			var toc_entries = $("#toc ul").children("li").get();
			toc_entries.sort(function(a, b) {
				return $(a).text().localeCompare($(b).text());
			});
			$.each(toc_entries, function(index, item) {
				$("#toc ul").append(item);
			});
			
			// Create section for API
			var api_container_id = "api_" + value;
			var api_container_selector = "#" + api_container_id;
			
			// Wire up TOC entry
			$(toc_entry_selector).click(function() {
				$(".api").hide();
				$("#toc ul li").css("background-color", "");
				$("#toc ul li").css("border-left", "solid 5px #FFFFFF");
				$(this).parent().css("background-color", "#F2F3F5");
				$(this).parent().css("border-left", "solid 5px #F2F3F5");
				$(api_container_selector).show();
				return false;
			});
					
			$("#apis").append("<div id='" + api_container_id + "' class='api' style='display:none'></div>");
			$(api_container_selector).append("<a name='" + value + "'><h2>" + contract.api + "</h2></a>");
			$(api_container_selector).append("<p>Version: " + contract.version + "</p>");
			$(api_container_selector).append("<h3>Description</h3>");
			$(api_container_selector).append("<div class='description'>" + contract.description + "</div>");
			$("<p><a href='#' id='" + api_container_id + "_show_detailed_description'>Show detailed description</a></p>").insertBefore(api_container_selector + " .description .detail");
			$(api_container_selector + "_show_detailed_description").click(function() {
				if ($(api_container_selector + "_show_detailed_description").html() == "Show detailed description") {
					$(api_container_selector + "_show_detailed_description").html("Hide detailed description")
					$(api_container_selector + " .description .detail").show();
				}
				else {
					$(api_container_selector + "_show_detailed_description").html("Show detailed description")
					$(api_container_selector + " .description .detail").hide();
				}
				return false;
			});
			
			// API Operations
			$(api_container_selector).append("<h3>Operations</h3>");
			$(api_container_selector).append("Colour coding: <em class='ready'>Production Ready</em>,<em class='inProgress'>in progress</em>,<em class='notStarted'>not started</em>");
			var operations_toc_id = "operations-" + value;
			var operations_toc_selector = "#" + operations_toc_id;
			$(api_container_selector).append("<table id='" + operations_toc_id + "'><tr><th>Method</th><th>URL Stem</th><th>Name</th><th>Description</th></tr></table>");
			var sorted_operations = new Array();
			for (var operation in contract.operations) {
				sorted_operations.push(operation);
			}
			sorted_operations.sort(function(a, b) {
				return a.localeCompare(b);
			});
			$.each(sorted_operations, function(index, value) {
				var operation = contract.operations[value];
				var operation_id = operations_toc_id + "_" + value;
				var operation_name = operation.method + " /" + value;
				if (operation.urlFormat) {
					operation_name = operation.method + " " + operation.urlFormat + " (" + value + ")";
				}
				
				var url = value;
				if (operation.urlFormat) {
					url = operation.urlFormat;
				}

				$(operations_toc_selector).append("<tr class='" + getOperationStatusCssClass(operation) + "'><a href='#" + operation_id + "'><td>" + operation.method + "</td><td><a href='#" + operation_id + "'>" + url + "</a></td><td>" + value + "</td><td>" + operation.description + "</td></a></tr>");	
				$(api_container_selector).append("<a name='" + operation_id + "'><h4>" + operation_name + "</h4></a>");
				
				$(api_container_selector).append("<h5>Description</h5>");
				$(api_container_selector).append("<p>" + operation.description + "</p>");
				
				$(api_container_selector).append("<h5>Parameters</h5>");
				if (operation.parameters) {
					var parameters_id = operation_id + "_parameters";
					var parameters_selector = "#" + parameters_id;
					$(api_container_selector).append("<table id='" + parameters_id + "'></table>");
					$(parameters_selector).append("<tr><th>Name</th><th>Required</th><th>Type</th><th>Description</th></tr>");
					$.each(operation.parameters, function(index, value) {
						$(parameters_selector).append("<tr><td>" + index + "</td><td>" + value.required + "</td><td>" + value.type + "</td><td>" + value.description + "</td></tr>");
					});
				}
				
				$(api_container_selector).append("<h5>Returns</h5>");
				if (operation.returns == null) {
					$(api_container_selector).append("<p>Nothing</p>");
				}
				else {
					$(api_container_selector).append("<p>" + operation.returns.description + "</p>");
					var return_schema_link_id = operation_id + "return_schema_link";
					var return_schema_link_selector = "#" + return_schema_link_id;
					var return_schema_id = operation_id + "return_schema";
					var return_schema_selector = "#" + return_schema_id;
					$(api_container_selector).append("<p><a href='#' id='" + return_schema_link_id + "'>Show schema</a></p>");
					$(return_schema_link_selector).click(function() {
						if ($(return_schema_link_selector).html() == "Show schema") {
							$(return_schema_selector).show();
							$(return_schema_link_selector).html("Hide schema");
							return false;
						}
						else {
							$(return_schema_selector).hide();
							$(return_schema_link_selector).html("Show schema");
							return false;
						}
					});
					$(api_container_selector).append("<pre style='display:none' id='" + return_schema_id + "'>" + JSON.stringify(operation.returns.schema, null, 2) + "</pre>");
				}
				
				$(api_container_selector).append("<h5>Response Times</h5>");
				var responsetimes_id = operation_id + "_responsetimes";
				var responsetimes_selector = "#" + responsetimes_id;
				$(api_container_selector).append("<table id='" + responsetimes_id + "'><tr><th>Max Requests/Sec</th><th>90% within</th><th>99% within</th><th>99.9% within</th></tr></table>");
				$(responsetimes_selector).append("<tr><td>" + operation.maxResponseTime.limits.maxRequestsPerSecond + "</td><td>" + operation.maxResponseTime.percentiles["_90.0"].milliseconds + "ms" + "<td>" + operation.maxResponseTime.percentiles["_99.0"].milliseconds + "ms" + "</td><td>" + operation.maxResponseTime.percentiles["_99.9"].milliseconds + "ms" + "</td></tr>");
			});
			
			// API Common Request Headers
			$(api_container_selector).append("<h3>Common Request Headers</h3>");
			var common_request_headers_id = api_container_id + "_common_request_headers";
			var common_request_headers_selector = "#" + common_request_headers_id;
			$(api_container_selector).append("<table id='" + common_request_headers_id + "'><tr><th>Name</th><th>Required</th><th>Type</th><th>Description</th></tr></table>");
			$.each(contract.commonRequestHeaders, function(index, value) {
			    var fullTypeInfo = value.type;
			    if (value.pattern != null) {
			    	fullTypeInfo = fullTypeInfo + "<br />Pattern: " + value.pattern;
			    }
				$(common_request_headers_selector).append("<tr><td>" + index + "</td><td>" + value.required + "</td><td>" + fullTypeInfo + "</td><td>" + value.description + "</td></tr>");
			});
			
			// API Common Response Headers
			$(api_container_selector).append("<h3>Common Response Headers</h3>");
			var common_response_headers_id = api_container_id + "_common_response_headers";
			var common_request_headers_selector = "#" + common_response_headers_id;
			$(api_container_selector).append("<table id='" + common_response_headers_id + "'><tr><th>Name</th><th>Required</th><th>Type</th><th>Description</th></tr></table>");
			$.each(contract.commonResponseHeaders, function(index, value) {
			    var fullTypeInfo = value.type;
			    if (value.pattern != null) {
			    	fullTypeInfo = fullTypeInfo + "<br />Pattern: " + value.pattern;
			    }
				$(common_request_headers_selector).append("<tr><td>" + index + "</td><td>" + value.required + "</td><td>" + fullTypeInfo + "</td><td>" + value.description + "</td></tr>");
			});
			
			// API Dependencies
			$(api_container_selector).append("<h3>Dependencies</h3>");
			var dependencies_id = api_container_id + "_dependencies";
			var dependencies_selector = "#" + dependencies_id;
			$(api_container_selector).append("<table id='" + dependencies_id + "'><tr><th>Dependency</th><th>Details</th></table>");
			$.each(contract.dependencies, function(index, value) {
			    var dependency_details = "";
				$.each(value, function(index, value) {
					dependency_details += index + ": " + value + "<br />";
				});
				$(dependencies_selector).append("<tr><td>" + index + "</td><td>" + dependency_details + "</td></tr>");
			});
						
			// API Performance Monitoring
			$(api_container_selector).append("<h3>Performance Monitoring</h3>");
			var performance_monitoring_id = api_container_id + "_performance_monitoring";
			var performance_monitoring_selector = "#" + performance_monitoring_id;
			$(api_container_selector).append("<table id='" + performance_monitoring_id + "'><tr><th>Counter</th><th>90.0% within</th><th>99.0% within</th><th>Max value</th></tr></table>");
			$.each(contract.performanceMonitoring, function(index, value) {
				$(performance_monitoring_selector).append("<tr><td>" + index + "</td><td>" + value.maxPercentiles["90.0"] + "</td><td>" + value.maxPercentiles["99.0"] + "</td><td>" + value.maxValue + "</td></tr>");
			});
			
			// Display first API
			$("#toc ul li a:first").click();
		});
	});
});