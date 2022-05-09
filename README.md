# Dynamics 365 CE Extension for SpecFlow
Do you want to test your Dynamics 365 CE implementation automatically, but do you think it is hard to do and very complex to implement? There are several exisiting technologies available to implement this like SpecFlow and EasyRepro. SpecFlow gives you the means to create test scenarios without writing any code. EasyRepro provides a set of functions to automatically work with Dynamics 365 CE. I combined both of these technologies into a solution so you can create test scenarios for Dynamics 365 CE without writing any code.

Detailed documentation can be found in the Wiki.

# Features
Below is an overview of all features. The framework is extensible, so you can easily ass your own functionality. I'm happy to include useful functionality in the framework. If you have any, feel free to create pull requests.

## One scenario, multiple targets
You only need to write your script once and run that same scripts on multiple browsers and/or the Dynamics CE API

## Use display names instead of schema names
You can use display names of attributes instead of schema names to make you scenarios easier to write and understand

## Multiple tab support
When entering data on a form, it will automatically determine the tab the field is on and switch tabs if needed

## Unified Data Type input
You specify your Date(time) format in your test project and the framework automatically figures out how this relates to the user's timezone and adapts the inputs and assertions. The same goes for decimal numbers. Invariant input is used for SpecFlow, but it will be entered in the form based on user settings.

## Creation of related records
Next to creating new standalone records, you can also created records that are related to another. Like pressing the 'Add New' on a subgrid or associated view.

## Wait for background processes
You can wait until background processes are finished. Very usefull when you want to test functionality that runs asynchonously

## Varied assertions
You can do all sorts of assertions like:
* Specific records has specific data
* A record exists with specified data
* Specific record has a set of related records
* Form state (required, read-only, visibility)
* Find specific error messages
* Find specific ribbon buttons
* Find specific form notifications

## Several entity specific actions
* Lead Qualification
* Closing Opportunities
* Conversion from quotes to orders

## API connection support
You use both Username/Password and ClientSecret authentication for API connections

## Multi user support
You can login with other users by writing a simple extension

## Multi language support
You can test using users with different languages using the same test scripts. See [here](https://github.com/DynamicHands/Crm.Specflow/wiki/Writing-Features#localization) for more info.

# NuGet packages

Two nuget packages are available.

Vermaat.Crm.Specflow (Branch: master) 
Vermaat.Crm.Specflow.Legacy (Branch: Legacy)

The legacy package is used for the old web interface and will only recieve minimal updates. It is recommended to use the unified interface with the regular client.

## Supported browsers

Currently supported browsers are: Chrome, Firefox, Internet Explorer and Edge. The support is solely based on EasyRepro (https://github.com/Microsoft/EasyRepro). Read more there for extra information
