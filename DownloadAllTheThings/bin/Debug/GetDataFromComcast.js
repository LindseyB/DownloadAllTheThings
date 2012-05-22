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
	} else {
		// error: page load
		console.log('1');
		phantom.exit();
	}
});

// Step 1
function initialize() {
	var success = evaluate(page, function(username,password) {
		// enter username and password and submit the form
		if(document.getElementById('user')) {
			document.getElementById('user').value = username;
			document.getElementById('passwd').value = password;
			document.forms['signin'].submit();
			return true;
		} 
		
		return false;
	}, username, password);

	if(success) {
		phantom.state = waitForLoad;
	} else {
		// error: unable to enter username and password
		console.log('2')
		phantom.exit();
	}
}


// Step 2
function waitForLoad() {
	var success = page.evaluate(function() {
		// silently grab the loading text
		if(document.getElementById('loadingMessage')){
			document.getElementById('loadingMessage').innerHTML;
			return true;
		} 

		return false;
	});

	if(success) {
		phantom.state = waitForLoad2;
	} else {
		// error: the login credentials were incorrect
		console.log('3');
		phantom.exit();
	}
}

//step 3
function waitForLoad2() {
	var success = page.evaluate(function() {
		// silently grab the loading text
		if(document.getElementById('loadingMessage')){
			document.getElementById('loadingMessage').innerHTML;
			return true;
		} 

		return false;
	});

	if(success) {
		phantom.state = scrapeData;
	} else {
		// error: unknown
		console.log('4');
		phantom.exit();
	}
}

// Step 4
function scrapeData() {
	page.evaluate(function() {
		// grab the data from the website
		if(document.getElementById('ctl00_ctl00_ContentArea_PrimaryColumnContent_UsedWrapper')) {
			console.log(document.getElementById('ctl00_ctl00_ContentArea_PrimaryColumnContent_UsedWrapper').innerHTML);
		} else {
			// error: unable to retrieve value
			console.log('5');
		}
	});

	phantom.exit();
}

// A hack to pass arguments into evaluation
function evaluate(page, func) {
    var args = [].slice.call(arguments, 2);
    var fn = "function() { return (" + func.toString() + ").apply(this, " + JSON.stringify(args) + ");}";
    return page.evaluate(fn);
}
