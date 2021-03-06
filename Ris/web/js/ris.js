/*
	Ris object
	The Ris object is a singleton object representing the RIS client desktop application.
*/

if(window.external)
{
	var Ris = {
	
		// parse filter to customize some aspects of JSML parsing without modifying the JSML.js script
		_jsmlParserFilter: function(key, value)
		{
			// if it has the properties of a staff person, replace it with a Staff object
			if(value && value.hasOwnProperty('staffId') && value.hasOwnProperty('staffName'))
				return new Staff(value.staffId, value.staffName, value.staffType);
			return value;
		},
		
		// map used to store async callback functions
		_asyncCallbackMap: {},
		
		// callback from an async service operation (see GetServiceProxy)
		_asyncInvocationCompleted: function(invocationId, responseJsml)
		{
			// look up the callback function, by invocation ID
			var callbackFunc = this._asyncCallbackMap[invocationId];
			if(callbackFunc)
			{
				// convert response JSML to an object, and invoke the callback function
				callbackFunc(JSML.parse(responseJsml));
			}
		},
		
		// callback from an async service operation (see GetServiceProxy)
		_asyncInvocationError: function(invocationId, errorMessage)
		{
			//TODO what is the best way to handle async errors?
		},
		
		// equivalent of the window.confirm function, but routes the message through the RIS client desktop application
		// message - confirmation message to display
		// type - a string containing either "YesNo" or "OkCancel" (not case-sensitive)
		// returns true if the user pressed Yes or OK, false otherwise
		confirm: function(message, type)
		{
			return window.external.Confirm(message || "", type || "OkCancel");
		},
		
		// equivalent of the window.alert function, but routes the message through the RIS client desktop application
		alert: function(message)
		{
			window.external.Alert(message || "");
		},
		
		// informs the application that the user has modified data on the page
		// (sets the ApplicationComponent.Modified flag to the specified value)
		setModified: function(modified)
		{
			window.external.Modified = modified;
		},
		
		getActionHtml: function(labelSearch, actionLabel)
		{
			return window.external.GetActionHtml(labelSearch, actionLabel);
		},
		
		// attempts to resolve the specified query string to a staff person,
		// returning a Staff object if successful, otherwise null.
		// note: this method may interact with the user via a dialog box if the query is not specific enough
		resolveStaffName: function(query)
		{
			var staffSummary = JSML.parse(window.external.ResolveStaffName(query || ""));
			if(staffSummary == null)
				return null;
			
			return new Staff(
				staffSummary.StaffId,
				staffSummary.Name.FamilyName + ", " + staffSummary.Name.GivenName,
				staffSummary.StaffType.Value);
		},
		
		// attempts to resolve the specified query string to a staff person,
		// returning a Staff object if successful, otherwise null.
		// note: this method may interact with the user via a dialog box if the query is not specific enough
		resolveStaffNameWithFilter: function(query, filter)
		{
			filter = filter || [];
			var f = JSML.create(filter, "filter");
			var staffSummary = JSML.parse(window.external.ResolveFilteredStaffName(query, f));
			if(staffSummary == null)
				return null;
			
			return new Staff(
				staffSummary.StaffId,
				staffSummary.Name.FamilyName + ", " + staffSummary.Name.GivenName,
				staffSummary.StaffType.Value);
		},
		
		// gets the application format string for showing date only
		// this format string is based on the .NET DateTime class format specifiers
		getDateFormat: function()
		{
			return window.external.DateFormat;
		},
		
		// gets the application format string for showing time only
		// this format string is based on the .NET DateTime class format specifiers
		getTimeFormat: function()
		{
			return window.external.TimeFormat;
		},
		
		// gets the application format string for showing both date and time
		// this format string is based on the .NET DateTime class format specifiers
		getDateTimeFormat: function()
		{
			return window.external.DateTimeFormat;
		},
		
		// formats the specified date object to a string showing only the date
		formatDate: function(date)
		{
			return date ? window.external.FormatDate(date.toISOString()) : "";
		},

		// formats the specified date object to a string showing only the time
		formatTime: function(date)
		{
			return date ? window.external.FormatTime(date.toISOString()) : "";
		},

		// formats the specified date object to a string showing both date and time
		formatDateTime: function(date)
		{
			return date ? window.external.FormatDateTime(date.toISOString()) : "";
		},

		formatRelativeTime: function(relativeTimeSpan, resolution)
		{
			return relativeTimeSpan ? window.external.FormatRelativeTime(relativeTimeSpan, resolution) : "";
		},
		
		formatDescriptiveDateTime: function(dateTime)
		{
			return dateTime ? window.external.FormatDescriptiveTime(dateTime.toISOString()) : "";
		},

		// formats the specified Address object
		formatAddress: function(address)
		{
			return address ? window.external.FormatAddress(JSML.create(address, "Address")) : "";
		},

		// formats the specified HealthcardNumber object
		formatHealthcard: function(healthcard)
		{
			return healthcard ? window.external.FormatHealthcard(JSML.create(healthcard, "Healthcard")) : "";
		},

		// formats the specified MRN object
		formatMrn: function(mrn)
		{
			return mrn ? window.external.FormatMrn(JSML.create(mrn, "Mrn")) : "";
		},

		// formats the specified Visit number object
		formatVisitNumber: function(visitNumber)
		{
			return visitNumber ? window.external.FormatVisitNumber(JSML.create(visitNumber, "VisitNumber")) : "";
		},

		// formats the specified Visit number object
		formatAccessionNumber: function(accessionNumber)
		{
			return accessionNumber ? window.external.FormatAccessionNumber(accessionNumber) : "";
		},

		// formats the specified personName object
		formatPersonName: function(personName)
		{
			return personName ? window.external.FormatPersonName(JSML.create(personName, "PersonName")) : "";
		},
		
		formatStaffNameAndRole: function(staffSummaryOrDetail)
		{
			return staffSummaryOrDetail ? window.external.FormatStaffNameAndRole(JSML.create(staffSummaryOrDetail, "Staff")) :"";
		},
		
		formatProcedureName: function(procedureSummaryOrDetail)
		{
			return procedureSummaryOrDetail ? window.external.FormatProcedureName(JSML.create(procedureSummaryOrDetail, "Procedure")) :"";
		},

		formatOrderListItemProcedureName: function(orderListItem)
		{
			return orderListItem ? window.external.FormatOrderListItemProcedureName(JSML.create(orderListItem, "Procedure")) :"";
		},
		
		// formats the specified telephone object
		formatTelephone: function(telephone)
		{
			return telephone ? window.external.FormatTelephone(JSML.create(telephone, "Telephone")) : "";
		},
		
		formatStringTelephone: function(telephone)
		{
			return telephone ? window.external.FormatStringTelephone(telephone.toString()) : "";
		},
		
		// obtains base URL from RIS application settings
		getBaseUrl: function()
		{
			return BaseUrl ? window.external.WebResourcesSettings.BaseUrl : "";
		},
		
		// obtains a proxy to a RIS web-service
		getService: function(serviceContractName)
		{
			var innerProxy = window.external.GetServiceProxy(serviceContractName);
			var operations = JSML.parse(innerProxy.GetOperationNames());
			
			var proxy = {};
			var risObj = this;
			operations.each(
				function(operation)
				{
						// create camelCase (as opposed to PascalCase) version of the operation name
						var ccOperation = operation.slice(0,1).toLowerCase() + operation.slice(1);
						
						// allow the operation to be invoked via either camel or pascal casing
						proxy[operation] = proxy[ccOperation] = 
							function(request)
							{
								return JSML.parse( innerProxy.InvokeOperation(operation, JSML.create(request, "requestData")) );
							};
						proxy[operation + "Async"] = proxy[ccOperation + "Async"] = 
							function(request, callbackFunc)
							{
								// invoke the operation asynchronously
								var id = innerProxy.InvokeOperationAsync(operation, JSML.create(request, "requestData"));
								
								// store the callback function, associated with the invocation ID
								risObj._asyncCallbackMap[id] = callbackFunc;
							};
				});
			return proxy;
		},
		
		// convenience method to get the RIS BrowsePatientDataService, which provides access to a patient's data
		getPatientDataService: function()
		{
			return this.getService("ClearCanvas.Ris.Application.Common.BrowsePatientData.IBrowsePatientDataService, ClearCanvas.Ris.Application.Common");
		},
		
		// gets the healthcare context in which the page is running
		// the healthcare context is an object that contains all entity-refs, etc., that the page
		// can use as keys to load data.
		// the healthcare context object is defined by the component that is hosting the page
		// check that component's documentation
		getHealthcareContext: function()
		{
			return JSML.parse(window.external.GetHealthcareContext());
		},
		
		// returns the value associated with the specified tag
		// the hosting component may allow the page to store abritrary key-value pairs,
		// in which case this function retrieves the value associated with the tag (key)
		// the hosting component is responsible for determining the scope of the stored values -
		// typically they are relative to the HealthcareContext (see getHealthcareContext).
		getTag: function(tag)
		{
			return window.external.GetTag(tag);
		},
		
		// sets the value associated with the specified tag
		// the hosting component may allow the page to store abritrary key-value pairs,
		// in which case this function stores the data (value) associated with the tag (key)
		// the hosting component is responsible for determining the scope of the stored values -
		// typically they are relative to the HealthcareContext (see getHealthcareContext).
		setTag: function(tag, data)
		{
			window.external.SetTag(tag, data);
		},

		openPractitionerDetails: function(practitioner)
		{
			window.external.OpenPractitionerDetail(JSML.create(practitioner, "Practitioner"));
		},
		
		notifyScriptCompleted: function()
		{
			return window.external.OnScriptCompleted();
		},
		
		getAttachedDocumentUrl: function(attachedDocumentSummary)
		{
			return window.external.GetAttachedDocumentUrl(JSML.create(attachedDocumentSummary, "AttachedDocument"));
		}
	};
	
	// this function must be defined at global scope, as it is invoked programmatically from C# code
	function __asyncInvocationCompleted(invocationId, responseJsml)
	{
		// forward to the Ris object
		Ris._asyncInvocationCompleted(invocationId, responseJsml);
	}
	
	// this function must be defined at global scope, as it is invoked programmatically from C# code
	function __asyncInvocationError(invocationId, errorMessage)
	{
		// forward to the Ris object
		Ris._asyncInvocationError(invocationId, errorMessage);
	}
	
	// install global JSML parser filter
	JSML.setParseFilter(Ris._jsmlParserFilter);
	
	// redefine some browser functions to use Ris versions
	window.confirm = Ris.confirm;
	window.alert = Ris.alert;
	
	// attach an event to suppress the "backspace" key, which in some cases invokes the browser Back function, clearing the form
	document.attachEvent( "onkeydown", function () {
		if(event.keyCode == 8) { // backspace
			var el = document.activeElement;
			// only allow the backspace key on INPUT text fields and TEXTAREA fields
			if((el.tagName == "INPUT" && el.attributes["type"].value == "text") || el.tagName == "TEXTAREA")
				event.returnValue = true;	
			else
				event.returnValue = false;
		}
	});
}

/*
	Staff class
*/
function Staff(id, name, type)
{
	this.staffId = id;
	this.staffName = name;
	this.staffType = type;
}
// override the toString function - this just makes it work seamlessly with the Table view					
Staff.prototype.toString = function() { return this.staffName; }			
