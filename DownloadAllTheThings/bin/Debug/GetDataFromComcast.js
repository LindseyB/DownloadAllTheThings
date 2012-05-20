var system = require('system'),
	page = require('webpage').create(),
	url = 'https://customer.comcast.com/Secure/UsageMeterDetail.aspx',
	stepIndex = 1,
	username = system.args[1],
	password = system.args[2];

// require to log information from the evalute functions to the console
page.onConsoleMessage = function (msg) {
    console.log(msg);
};

page.open(url, function (status) {
	if (status === 'success') {
		if(!phantom.state){
			initialize();
		} else {
			phantom.state();
		}
	}
});

// Step 1
function initialize() {
	evaluate(page, function(username,password) {
		// enter username and password and submit the form
		document.getElementById('user').value = username;
		document.getElementById('passwd').value = password;
		document.forms['signin'].submit();
	}, username, password);

	phantom.state = waitForLoad;
}


// Step 2
function waitForLoad() {
	page.evaluate(function() {
		// silently grab the loading text
		document.getElementById('loadingMessage').innerHTML;
	});

	phantom.state = waitForLoad2;
}

//step 3
function waitForLoad2() {
	page.evaluate(function() {
		// silently grab the loading text
		document.getElementById('loadingMessage').innerHTML;
	});

	phantom.state = scrapeData;	
}

// Step 4
function scrapeData() {
	page.evaluate(function() {
		// grab the data from the website
		console.log(document.getElementById('ctl00_ctl00_ContentArea_PrimaryColumnContent_UsedWrapper').innerHTML);
	});

	phantom.exit();
}

// A hack to pass arguments into evaluation
function evaluate(page, func) {
    var args = [].slice.call(arguments, 2);
    var fn = "function() { return (" + func.toString() + ").apply(this, " + JSON.stringify(args) + ");}";
    return page.evaluate(fn);
}
