(function (global) {

    function registerNamespace(namespace) {
        var variable = global,
            parts = namespace.split('.');
        parts.forEach(function (part) {
            if (typeof variable[part] === 'undefined') {
                variable[part] = {};
            }
            variable = variable[part];
        });
        return variable;
    }

    function onLoad(executionContext) {
        // Set form context
        var formContext = executionContext.getFormContext();

        // Add event handlers
        formContext.getAttribute("creditlimit").addOnChange(setCreditLimitNotification);
		formContext.getAttribute("industrycode").addOnChange(setIndustryCodeNotification);

        // OnLoad functionality
        setCreditLimitNotification(executionContext);
		setIndustryCodeNotification(executionContext);
    }

    function setCreditLimitNotification(executionContext) {
		var formContext = executionContext.getFormContext();
		var creditLimit = formContext.getAttribute("creditlimit").getValue();
		if(creditLimit !== null && creditLimit < 1000000)
			formContext.ui.setFormNotification("The credit limit of this account is under 1.000.000", "WARNING", "CreditLimit");
		else
			formContext.ui.clearFormNotification("CreditLimit");
    }
	
	function setIndustryCodeNotification(executionContext) {
	
		var formContext = executionContext.getFormContext();
		var industry = formContext.getAttribute("industrycode").getValue();
		
		if(industry === 4) {
			formContext.ui.setFormNotification("This account is from the broker industry. Additional rules applicable.", "WARNING", "BrokerIndustry");
		}
		else {
			formContext.ui.clearFormNotification("BrokerIndustry");
		}
	}


    var namespace = registerNamespace('vrm.Account');
    namespace.onLoad = onLoad;
})(window);