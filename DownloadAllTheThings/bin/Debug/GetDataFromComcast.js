var system = require('system'),
	page = require('webpage').create(),
	url = 'https://customer.comcast.com/Secure/UsageMeterDetail.aspx',
	stepIndex = 1,
	username = system.args[1],
	password = system.args[2];

page.onConsoleMessage = function (msg) {
	// for debugging, remove the console> later
    console.log('console>' + msg);
};

// for debugging, just in case
page.onAlert = function (msg) {
	console.log('alert>' + msg);
};


page.open(url, function (status) {
	if (status === 'success') {
		console.log('=======================================');
		console.log('Step ' + stepIndex);
		console.log('=======================================');

		// let's get some jquery up in this bitch
		page.injectJs('jquery-1.7.2.min.js');

		if(!phantom.state){
			initialize();
		} else {
			phantom.state();
		}

		// save a screenshot - because otherwise I won't know wtf
		page.render('step' + stepIndex++ + ".png");
	}
});

// Step 1
function initialize() {
	page.evaluate(function() {
		document.getElementById('user').value = 'hardcodedusername';
		document.getElementById('passwd').value = 'hardcodedpassword';
		document.forms['signin'].submit();
		console.log('submitting form...');
	});

	phantom.state = waitForLoad;
}

// Step 2
function waitForLoad() {
	page.evaluate(function() {
		console.log(document.getElementById('loadingMessage').innerHTML);
	});

	phantom.state = waitForLoad2;
}

//step 3
function waitForLoad2() {
	page.evaluate(function() {
		console.log(document.getElementById('loadingMessage').innerHTML);
	});

	phantom.state = scrapeData;	
}

// Step 4
function scrapeData() {
	page.evaluate(function() {
		// to do: grab the data
		//console.log(document.body.innerHTML);
		console.log(document.getElementById('ctl00_ctl00_ContentArea_PrimaryColumnContent_UsedWrapper').innerHTML);
	});

	phantom.exit();
}

