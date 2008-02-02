var Story =
{
    _submitStoryUrl : '',
    _kiggStoryUrl : '',
    _commentStoryUrl : '',
    _centerMeTimer : null,

    set_submitStoryUrl : function(value)
    {
        if (Story._submitStoryUrl !== value)
        {
            Story._submitStoryUrl = value;
        }
    },

    set_kiggStoryUrl : function(value)
    {
        if (Story._kiggStoryUrl !== value)
        {
            Story._kiggStoryUrl = value;
        }
    },

    set_commentStoryUrl : function(value)
    {
        if (Story._commentStoryUrl !== value)
        {
            Story._commentStoryUrl = value;
        }
    },

    init : function()
    {
        $addHandler($get('divSubmitStoryClose'), "click", Story._closeButtonClick);

        $addHandler($get('txtStoryUrl'), "keydown", Story._keyDown);
        $addHandler($get('txtStoryTitle'), "keydown", Story._keyDown);
        $addHandler($get('txtStoryTags'), "keydown", Story._keyDown);
    },

    dispose : function()
    {
        Story._clearCenterMeTimer();

        $removeHandler($get('divSubmitStoryClose'), "click", Story._closeButtonClick);

        $removeHandler($get('txtStoryUrl'), "keydown", Story._keyDown);
        $removeHandler($get('txtStoryTitle'), "keydown", Story._keyDown);
        $removeHandler($get('txtStoryTags'), "keydown", Story._keyDown);
    },

    _clearCenterMeTimer : function()
    {
        if (Story._centerMeTimer != null)
        {
            clearInterval(Story._centerMeTimer);
            Story._centerMeTimer = null;
        }
    },

    _centerMe : function()
    {
        $U.centerIt('divSubmitStoryBox');
        Story._clearCenterMeTimer();
        Story._centerMeTimer = setInterval(
                                            function()
                                            {
                                                Story._centerMe();
                                            },
                                            500
                                          );
        
    },

    showSubmit : function()
    {
        if (!Membership.get_isLoggedIn())
        {
            Membership.showLogin();
            return;
        }

        $U.resetInputs('divSubmitStoryBox')
        $get('ddlStoryCategory').selectedIndex = 0;
        $get('txtStoryDescription').value = '';

        $U.noDisplayChidren('divSubmitStoryBox', 'span', 'validator');
        $U.noDisplayChidren('divSubmitStoryBox', 'span', 'message');

        $U.showModalDialog('divDimBackground', 'divSubmitStoryBox');
        Story._centerMe();

        $U.focus('txtStoryUrl');
    },

    _hide : function()
    {
        Story._clearCenterMeTimer();
        $U.noDisplay('divDimBackground');
        $U.noDisplay('divSubmitStoryBox');
    },

    submit : function()
    {
        $U.noDisplayChidren('divSubmitStoryBox', 'span', 'validator');
        $U.noDisplay('storyMessage');

        var txtUrl = $get('txtStoryUrl');
        var url = txtUrl.value.trim();

        if (url.length == 0)
        {
            Story._showMessage('valStoryUrl', 'Url cannot be blank.', true);
            $U.focus(txtUrl);
            return;
        }

        if (!url.toLowerCase().startsWith('http://'))
        {
            url = 'http://' + url;
        }

        if (!Story._isValidUrl(url))
        {
            Story._showMessage('valStoryUrl', 'Invalid url.', true);
            $U.focus(txtUrl);
            return;
        }

        var txtTitle = $get('txtStoryTitle');
        var title = txtStoryTitle.value.trim();

        if (title.length == 0)
        {
            Story._showMessage('valStoryTitle', 'Title cannot be blank.', true);
            $U.focus(txtTitle);
            return;
        }

        var ddlCategory = $get('ddlStoryCategory');
        var categoryId = parseInt(ddlCategory.options[ddlCategory.selectedIndex].value);

        if (categoryId < 1)
        {
            Story._showMessage('valStoryCategory', 'Select a category.', true);
            $U.focus(ddlCategory);
            return;
        }

        var tags = $get('txtStoryTags').value.trim();

        var txtDescription = $get('txtStoryDescription');
        var description = txtDescription.value.trim();

        if (description.length == 0)
        {
            Story._showMessage('valfStoryDescription', 'Description cannot be blank.', true);
            $U.focus(txtDescription);
            return;
        }

        var formFields = [['storyUrl', url],['storyTitle', title],['storyCategoryId', categoryId],['storyDescription', description],['storyTags', tags]];

        Story._showMessage('storyMessage', 'Submitting story...', false);
        $get('btnSubmitStory').disabled = true;

        $Ajax.post  (
                        Story._submitStoryUrl,
                        formFields,
                        function(result)
                        {
                            $get('btnSubmitStory').disabled = false;

                            if (result.isSuccessful == true)
                            {
                                Story._hide();
                            }
                            else
                            {
                                Story._showMessage('storyMessage', result.errorMessage, true);
                                $U.focus('txtStoryUrl');
                            }
                        },
                        function(error)
                        {
                            $get('btnSubmitStory').disabled = false;
                            Story._showMessage('storyMessage', 'An unexpected error has occurred while submitting story.', true);
                        }
                    );
    },

    kigg : function(storyId, currentKigg, count, it, ed, ing)
    {
        if (!Membership.get_isLoggedIn())
        {
            Membership.showLogin();
            return;
        }

        $U.noDisplay(it);
        $U.display(ing);

        $Ajax.post  (
                        Story._kiggStoryUrl,
                        [['storyId', storyId]],
                        function(result)
                        {
                            if (result.isSuccessful == true)
                            {
                                currentKigg += 1;
                                $U.setText(count, currentKigg.toString());
                                $U.noDisplay(ing);
                                $U.display(ed);
                            }
                            else
                            {
                                $U.noDisplay(ing);
                                $U.display(it);
                                alert(result.errorMessage);
                            }
                        }, 
                        function (error)
                        {
                            $U.noDisplay(ing);
                            $U.display(it);
                            alert('An unexpected error has occurred while Kigging the story.');
                        }
                    );
    },

    comment : function(storyId, commentBox, commentMessage)
    {
        if (!Membership.get_isLoggedIn())
        {
            Membership.showLogin();
            return;
        }

        $U.noDisplay('commentMessage');

        var txtComment = $get('txtComment');
        var comment = txtComment.value.trim();

        if (comment.length == 0)
        {
            Story._showMessage('commentMessage', 'Comment cannot be blank.', true);
            $U.focus(txtComment);
            return;
        }

        Story._showMessage('commentMessage', 'Submitting comment...', false);
        $get('btnSubmitComment').disabled = true;

        $Ajax.post  (
                        Story._commentStoryUrl,
                        [['storyId', storyId],['commentContent', comment]],
                        function(result)
                        {
                            if (result.isSuccessful == true)
                            {
                                txtComment.value = '';
                                Story._showMessage('commentMessage', 'Thanks for submitting comments. It will appear here shortly.', false);
                            }
                            else
                            {
                                Story._showMessage('commentMessage', result.errorMessage, true);
                                $U.focus(txtComment);
                            }

                            $get('btnSubmitComment').disabled = false;
                        }, 
                        function (error)
                        {
                            Story._showMessage('commentMessage', 'An unexpected error has occurred while posting comment.', true);
                            $get('btnSubmitComment').disabled = false;
                        }
                    );
    },

    _showMessage : function(e, msg, err)
    {
        var e = $U.fix(e);

        if (err)
        {
            e.style.color = '#ff0000';
        }
        else
        {
            e.style.color = '';
        }

        $U.setText(e, msg);
        $U.display(e);
    },

    _isValidUrl : function(url)
    {
        var regExp = /^(http|https)\:\/\/\w+([\.\-]\w+)*\.\w{2,4}(\:\d+)*([\/\.\-\?\&\%\#]\w+)*\/?$/i;
        return regExp.test(url);
    },

    _keyDown : function(e)
    {
        if (e.keyCode === Sys.UI.Key.enter)
        {
            Story.submit();
        }
        else if (e.keyCode === Sys.UI.Key.esc)
        {
            Story._hide();
        }
    },

    _closeButtonClick : function(e)
    {
        Story._hide();
    }
}