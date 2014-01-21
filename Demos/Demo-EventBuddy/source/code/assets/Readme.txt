Please take into account that if you modify the server scripts that run on the Windows Azure Mobile Service, you'll have to update the snippets used in Internet Explorer's Favorites bar. 

In order to do so follow these steps:

1. Go to http://coderstoolbox.net/string/ and make sure the following options are selected: ECMAScript / Encode / US-ASCII. Paste the javascript code for the updated server script in the first text area. The code will be automatically converted to something like this:

Sample:
	function insert(item, user, request) {\n    item.userId = user.userId;\n    request.execute();\n}

Copy the converted snippet into the clipboard.

2. Replace the #snippet# placeholder with the javascript code you've obtained from the previous step:

	javascript:(function(){window.clipboardData.setData('text','#snippet#')})()

3. Replace the URL and ExtendedURL keys on the *.url corresponding file with the code obtained from the previous step 

NOTE: url files are located in the source\setup\snippets\IESnippets\ folder

