var $U =
{
    display : function(e)
    {
        $U.fix(e).style.display = '';
    },

    displayBlock : function(e)
    {
        $U.fix(e).style.display = 'block';
    },

    noDisplay : function(e)
    {
        $U.fix(e).style.display = 'none';
    },

    show : function(e)
    {
        $U.fix(e).style.visibility = 'visible';
    },

    hide : function(e)
    {
        $U.fix(e).style.visibility = 'hidden';
    },

    setText : function(e, t)
    {
        e = $U.fix(e);

        if (document.all)
        {
            e.innerText = t;
        }
        else
        {
            e.textContent = t;
        }
    },

    focus : function(e)
    {
        e = $U.fix(e);

        try
        {
            e.select();
        }
        catch(e)
        {
        }

        try
        {
            e.focus();
        }
        catch(e)
        {
        }
    },

    setLocation : function(e, x, y)
    {
        e = $U.fix(e);
        Sys.UI.DomElement.setLocation(e, x, y);
    },

    resetInputs : function()
    {
        if (arguments.length > 0)
        {
            var container;
            var controls;
            var control;

            for(var i = 0; i < arguments.length; i++)
            {
                container = $U.fix(arguments[i]);
                controls = container.getElementsByTagName('input');

                if (controls.length > 0)
                {
                    for(var j = 0; j < controls.length; j++)
                    {
                        control = controls[j];

                        if ((control.type == 'text') || (control.type == 'file') || (control.type == 'password'))
                        {
                            control.value = '';
                        }
                        else if ((control.type == 'checkbox') || (control.type == 'radio'))
                        {
                            control.checked = false;
                        }
                    }
                }
            }
        }
    },

    displayChidren : function(e, t, c)
    {
        $U.displayOrNoDisplayChidren(true, e, t, c);
    },

    noDisplayChidren : function(e, t, c)
    {
        $U.displayOrNoDisplayChidren(false, e, t, c);
    },

    displayOrNoDisplayChidren : function(display, e, t, c)
    {
        e = $U.fix(e);

        var elms = e.getElementsByTagName(t);

        if (elms.length > 0)
        {
            var e;

            for(var i = 0; i < elms.length; i++)
            {
                e = elms[i];

                if (c)
                {
                    if (Sys.UI.DomElement.containsCssClass(e, c))
                    {
                        if (display)
                        {
                            $U.display(e);
                        }
                        else
                        {
                            $U.noDisplay(e);
                        }
                    }
                }
                else
                {
                    if (display)
                    {
                        $U.display(e);
                    }
                    else
                    {
                        $U.noDisplay(e);
                    }
                }
            }
        }
    },

    fix : function(e)
    {
        if (typeof e != 'object')
        {
            e = $get(e);
        }

        return e;
    },

    showModalDialog : function (divBackground, divModal)
    {
        divModal = $U.fix(divModal);
        divBackground = $U.fix(divBackground);

        $U.display(divModal);
        $U.display(divBackground);

        var viewPortWidth = $U.getViewPortWidth();
        var viewPortHeight = $U.getViewPortHeight();
        var contentHeight = $U.getContentHeight();

        divBackground.style.width = viewPortWidth + 'px';
        divBackground.style.height = Math.max(viewPortHeight, contentHeight) + 'px';

        $U.setLocation(divBackground, 0, 0);
        $U.centerIt(divModal);
    },

    centerIt : function(e)
    {
        e = $U.fix(e);

        var x = (($U.getViewPortWidth() - e.offsetWidth) /2);
        var y = (($U.getViewPortHeight() - e.offsetHeight) /2) + $U.getViewPortScrollY();

        $U.setLocation(e, x, y);
    },

    getViewPortWidth : function()
    {
        var width = 0;

        if ((document.documentElement) && (document.documentElement.clientWidth))
        {
            width = document.documentElement.clientWidth;
        }
        else if ((document.body) && (document.body.clientWidth))
        {
            width = document.body.clientWidth;
        }
        else if (window.innerWidth)
        {
            width = window.innerWidth;
        }

        return width;
    },

    getViewPortHeight : function()
    {
        var height = 0;

        if (window.innerHeight)
        {
            height = window.innerHeight - 18;
        }
        else if ((document.documentElement) && (document.documentElement.clientHeight))
        {
            height = document.documentElement.clientHeight;
        }

        return height;
    },

    getContentHeight : function()
    {
        if ((document.body) && (document.body.offsetHeight))
        {
            return document.body.offsetHeight;
        }

        return 0;
    },

    getViewPortScrollX : function()
    {
        var scrollX = 0;

        if ((document.documentElement) && (document.documentElement.scrollLeft))
        {
            scrollX = document.documentElement.scrollLeft;
        }
        else if ((document.body) && (document.body.scrollLeft))
        {
            scrollX = document.body.scrollLeft;
        }
        else if (window.pageXOffset)
        {
            scrollX = window.pageXOffset;
        }
        else if (window.scrollX)
        {
            scrollX = window.scrollX;
        }

        return scrollX;
    },

    getViewPortScrollY : function()
    {
        var scrollY = 0;

        if ((document.documentElement) && (document.documentElement.scrollTop))
        {
            scrollY = document.documentElement.scrollTop;
        }
        else if ((document.body) && (document.body.scrollTop))
        {
            scrollY = document.body.scrollTop;
        }
        else if (window.pageYOffset)
        {
            scrollY = window.pageYOffset;
        }
        else if (window.scrollY)
        {
            scrollY = window.scrollY;
        }

        return scrollY;
    },

    parseQueryString : function(url)
    {
        url = new String(url);
        var queryStringValues = new Object();
        var querystring = url.substring((url.indexOf('?') + 1), url.length);
        var querystringSplit = querystring.split('&');

        for (var i = 0; i < querystringSplit.length; i++)
        {
            var pair = querystringSplit[i].split('=');
            var name = pair[0];
            var value = pair[1];

            queryStringValues[name]=value;
        }

        return queryStringValues;
    }
}