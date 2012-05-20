var system = require('system'),
	page = require('webpage').create(),
	url = 'https://customer.comcast.com/Secure/UsageMeterDetail.aspx',
	stepIndex = 1,
	username = system.args[1],
	password = system.args[2];

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

		// save a screenshot - because otherwise I won't know wtf
		// TODO: remove
		page.render('step' + stepIndex++ + ".png");
	}
});

// Step 1
function initialize() {
	page.evaluate(function() {
		document.getElementById('user').value = 'lbieda5956';
		document.getElementById('passwd').value = 'incu13bus';
		document.forms['signin'].submit();
		console.log('submitting form...');
	});

	phantom.state = waitForLoad;
}

// Step 2
function waitForLoad() {
	page.evaluate(function() {
		// TODO: remove
		console.log(document.getElementById('loadingMessage').innerHTML);
	});

	phantom.state = waitForLoad2;
}

//step 3
function waitForLoad2() {
	page.evaluate(function() {
		// TODO: remove
		console.log(document.getElementById('loadingMessage').innerHTML);
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

