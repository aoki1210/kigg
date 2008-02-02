var $Ajax =
{
    post: function(url, formFields, onSuccess, onFailure)
    {
        var request = new Sys.Net.WebRequest();
        var requestBody = new Sys.StringBuilder();

        if (formFields)
        {
            if (formFields.length > 0)
            {
                for(var i = 0; i < formFields.length; i++)
                {
                    if (i > 0)
                    {
                        requestBody.append('&');
                    }

                    requestBody.append(String.format('{0}={1}', formFields[i][0], formFields[i][1]));
                }
            }
        }

        request.set_url(url);
        request.set_body(requestBody.toString());
        request.set_httpVerb('post');
        request.add_completed(onComplete);
        request.invoke();

        function onComplete(response, eventArgs)
        {
            if (response.get_responseAvailable())
            {
                var statusCode = response.get_statusCode();
                var result = null;
                
                try
                {
                    var contentType = response.getResponseHeader('Content-Type');

                    if (contentType.startsWith('application/json'))
                    {
                        result = response.get_object();
                    }
                    else if (contentType.startsWith('text/xml'))
                    {
                        result = response.get_xml();
                    }
                    else
                    {
                        result = response.get_responseData();
                    }
                }
                catch(e)
                {
                }

                if ((statusCode < 200) || (statusCode >= 300))
                {
                    if (onFailure)
                    {
                        if (!result)
                        {
                            result = new Sys.Net.WebServiceError(false , 'An unexpected error has occurred while processing your request.', '', '');
                        }

                        result._statusCode = statusCode;

                        onFailure(result);
                    }
                }
                else if (onSuccess)
                {
                    onSuccess(result, '', '');
                }
            }
            else
            {
                var msg;

                if (response.get_timedOut())
                {
                    msg = 'Request timed out';
                }
                else
                {
                    msg = 'An unexpected error has occurred while processing your request';
                }
            
                if (onFailure)
                {
                    onFailure(new Sys.Net.WebServiceError(response.get_timedOut(), msg, '', ''));
                }
            }
        }
    }
}