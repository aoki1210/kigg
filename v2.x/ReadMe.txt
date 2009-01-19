Installation Notes
==================

1. Create the Database from Database\Create.sql or Restore a blank from Web\App_Data or leave it if you are running sql express.
2. Run the Database\Data.sql to populate some category/tag.
3. Create a virtual directory named Kigg.
4. Create an account with the following external services:
	Full
		-http://www.pageglimpse.com/
		-http://www.websnapr.com/
		-http://antispam.typepad.com/
		-http://akismet.com/
		-http://defensio.com/
		-http://recaptcha.net/
	Minimum
		-http://www.websnapr.com/
		-http://recaptcha.net/

5. Open The web.full.config/web.minimum.config(depending upon your above) in your prefered text editor.
6. Rename web.full.config/web.minimum.config to web.config
7. Try find with "YOUR-" and replace with your value.
8. Open the Kigg.sln and run.