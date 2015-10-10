var webdriver = require('selenium-webdriver'),
	By = require('selenium-webdriver').By,
	until = require('selenium-webdriver').until;
	
var driver = new webdriver.Builder()
	.forBrowser('firefox')
	.build();

driver.get('http://localhost:49433/');
process.stdout.write(driver.getTitle());

driver.findElement(By.name('provider')).click();
driver.wait(until.titleIs('Log into Facebook | Facebook'),1000);
driver.quit();