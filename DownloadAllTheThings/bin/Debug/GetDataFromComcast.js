var system = require('system');
var page = require('webpage').create();
var url = 'https://customer.comcast.com/Secure/UsageMeterDetail.aspx';


var username = system.args[1];
var password = system.args[2];

page.onConsoleMessage = function (msg) {
    console.log(msg);
};


page.open(url, function (status) {
	if (status !== 'success') {
		console.log('error loading page');
	} else {
		//page is open, now do a thing
		page.evaluate(function () {
			document.getElementById('user').value = window.username;
			document.getElementById('passwd').value = window.password;
			document.forms['signin'].submit();
			console.log('submitting form...');
			console.log(document.body.innerHTML);
		});
	}
	phantom.exit();
});

