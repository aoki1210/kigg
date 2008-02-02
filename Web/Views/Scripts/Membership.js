var Membership =
{
    _isLoggedIn : false,
    _loginUrl : '',
    _logoutUrl : '',
    _sendPasswordUrl : '',
    _signUpUrl : '',
    _centerMeTimer : null,

    get_isLoggedIn : function()
    {
        return Membership._isLoggedIn; 
    },

    set_isLoggedIn : function(value)
    {
        if (Membership._isLoggedIn !== value)
        {
            Membership._isLoggedIn = value;
        }
    },

    set_loginUrl : function(value)
    {
        if (Membership._loginUrl !== value)
        {
            Membership._loginUrl = value;
        }
    },

    set_logoutUrl : function(value)
    {
        if (Membership._logoutUrl !== value)
        {
            Membership._logoutUrl = value;
        }
    },

    set_signUpUrl : function(value)
    {
        if (Membership._signUpUrl !== value)
        {
            Membership._signUpUrl = value;
        }
    },

    set_sendPasswordUrl : function(value)
    {
        if (Membership._sendPasswordUrl !== value)
        {
            Membership._sendPasswordUrl = value;
        }
    },

    init : function()
    {
        $addHandler($get('divMembershipClose'), "click", Membership._closeButtonClick);

        $addHandler($get('txtLoginUserName'), "keydown", Membership._keyDownForLogin);
        $addHandler($get('txtLoginPassword'), "keydown", Membership._keyDownForLogin);

        $addHandler($get('txtForgotEmail'), "keydown", Membership._keyDownForForgotPassword);

        $addHandler($get('txtSignupUserName'), "keydown", Membership._keyDownForSignup);
        $addHandler($get('txtSignupPassword'), "keydown", Membership._keyDownForSignup);
        $addHandler($get('txtSignupConfirm'), "keydown", Membership._keyDownForSignup);
        $addHandler($get('txtSignupEmail'), "keydown", Membership._keyDownForSignup);
    },

    dispose : function()
    {
        Membership._clearCenterMeTimer();

        $removeHandler($get('divMembershipClose'), "click", Membership._closeButtonClick);

        $removeHandler($get('txtLoginUserName'), "keydown", Membership._keyDownForLogin);
        $removeHandler($get('txtLoginPassword'), "keydown", Membership._keyDownForLogin);

        $removeHandler($get('txtForgotEmail'), "keydown", Membership._keyDownForForgotPassword);

        $removeHandler($get('txtSignupUserName'), "keydown", Membership._keyDownForSignup);
        $removeHandler($get('txtSignupPassword'), "keydown", Membership._keyDownForSignup);
        $removeHandler($get('txtSignupConfirm'), "keydown", Membership._keyDownForSignup);
        $removeHandler($get('txtSignupEmail'), "keydown", Membership._keyDownForSignup);
    },

    _clearCenterMeTimer : function()
    {
        if (Membership._centerMeTimer != null)
        {
            clearInterval(Membership._centerMeTimer);
            Membership._centerMeTimer = null;
        }
    },

    _centerMe : function()
    {
        $U.centerIt('divMembershipBox');
        Membership._clearCenterMeTimer();
        Membership._centerMeTimer = setInterval(
                                                    function()
                                                    {
                                                        Membership._centerMe();
                                                    },
                                                    500
                                                );
        
    },

    _show : function()
    {
        $U.showModalDialog('divDimBackground', 'divMembershipBox');
        Membership._centerMe();
    },

    _hide : function()
    {
        Membership._clearCenterMeTimer();
        $U.noDisplay('divDimBackground');
        $U.noDisplay('divMembershipBox');
    },

    showLogin : function()
    {
        $U.noDisplay('divSignup');
        $U.display('divLogin');
        $U.resetInputs('divMembershipBox')
        $U.noDisplayChidren('divMembershipBox', 'span', 'validator');
        $U.noDisplayChidren('divMembershipBox', 'span', 'message');

        Membership._show();

        $U.focus('txtLoginUserName');
    },

    login : function()
    {
        $U.noDisplayChidren('divLogin', 'span', 'validator');
        $U.noDisplay('loginMessage');

        var txtUserName = $get('txtLoginUserName');
        var userName = txtUserName.value.trim();

        if (userName.length == 0)
        {
            Membership._showMessage('valLoginUserName', 'User name cannot be blank.', true);
            $U.focus(txtUserName);
            return;
        }

        var txtPassword = $get('txtLoginPassword');
        var password = txtPassword.value;

        if (password.length == 0)
        {
            Membership._showMessage('valLoginPassword', 'Password cannot be blank.', true);
            $U.focus(txtPassword);
            return;
        }

        var rememberMe = $get('chkLoginRememberMe').checked;

        Membership._showMessage('loginMessage', 'Authenticating...', false);
        $get('btnLogin').disabled = true;

        var formFields = [['userName', userName],['password', password],['rememberMe', rememberMe]];

        $Ajax.post( 
                        Membership._loginUrl,
                        formFields,
                        function(result)
                        {
                            $get('btnLogin').disabled = false;

                            if (result.isSuccessful == true)
                            {
                                window.location.reload();
                            }
                            else
                            {
                                Membership._showMessage('loginMessage', result.errorMessage, true);
                                $U.focus('txtLoginUserName');
                            }
                        },
                        function(error)
                        {
                            $get('btnLogin').disabled = false;
                            $U.focus('txtLoginUserName');
                            Membership._showMessage('loginMessage', 'An unexpected error has occurred while logging in.', true);
                        }
                    );
    },

    logout : function()
    {
        $Ajax.post(
                        Membership._logoutUrl,
                        null,
                        function(result)
                        {
                            if (result.isSuccessful == true)
                            {
                                window.location.reload();
                            }
                        }
                    );
    },

    sendPassword : function()
    {
        $U.noDisplayChidren('divLogin', 'span', 'validator');
        $U.noDisplay('passwordMessage');

        var txtEmail = $get('txtForgotEmail');
        var email = txtEmail.value.trim();

        if (email.length == 0)
        {
            Membership._showMessage('valForgotEmail', 'Email cannot be blank.', true);
            $U.focus(txtEmail);
            return;
        }

        if (!Membership._isValidEmail(email))
        {
            Membership._showMessage('valForgotEmail', 'Invalid email address.', true);
            $U.focus(txtEmail);
            return;
        }

        Membership._showMessage('passwordMessage', 'Sending password...', false);
        $get('btnPassword').disabled = true;

        $Ajax.post  (
                        Membership._sendPasswordUrl,
                        [['email', email]],
                        function(result)
                        {
                            $get('btnPassword').disabled = false;

                            if (result.isSuccessful == true)
                            {
                                Membership._hide();
                            }
                            else
                            {
                                $U.noDisplay('passwordMessage');
                                Membership._showMessage('valForgotEmail', result.errorMessage, true);
                                $U.focus('txtForgotEmail');
                            }
                        },
                        function(error)
                        {
                            $get('btnPassword').disabled = false;
                            Membership._showMessage('passwordMessage', 'An unexpected error has occurred while sending the password.', true);
                        }
                    );
    },

    showSignUp : function()
    {
        $U.noDisplay('divLogin');
        $U.display('divSignup');
        $U.resetInputs('divMembershipBox');
        $U.noDisplayChidren('divMembershipBox', 'span', 'validator');
        $U.noDisplayChidren('divMembershipBox', 'span', 'message');

        Membership._show();

        $U.focus('txtSignupUserName');
    },

    signUp : function()
    {
        $U.noDisplayChidren('divSignup', 'span', 'validator');
        $U.noDisplay('signupMessage');

        var txtUserName = $get('txtSignupUserName');
        var userName = txtUserName.value.trim();

        if (userName.length == 0)
        {
            Membership._showMessage('valSignupUserName', 'User name cannot be blank.', true);
            $U.focus(txtUserName);
            return;
        }

        var txtPassword = $get('txtSignupPassword');
        var password = txtPassword.value;

        if (password.length < 4)
        {
            Membership._showMessage('valSignupPassword', 'Password must be 4 character long.', true);
            $U.focus(txtPassword);
            return;
        }

        var txtConfirmPassword = $get('txtSignupConfirm');
        var confirmPassword = txtConfirmPassword.value;

        if (password.length == 0)
        {
            Membership._showMessage('valSignupConfirm', 'Confirm password cannot be blank.', true);
            $U.focus(txtConfirmPassword);
            return;
        }

        if (password != confirmPassword)
        {
            Membership._showMessage('valSignupConfirm', 'Confirm password does not match with password.', true);
            $U.focus(txtConfirmPassword);
            return;
        }

        var txtEmail = $get('txtSignupEmail');
        var email = txtEmail.value.trim();

        if (email.length == 0)
        {
            Membership._showMessage('valSignupEmail', 'Email cannot be blank.', true);
            $U.focus(txtEmail);
            return;
        }

        if (!Membership._isValidEmail(email))
        {
            Membership._showMessage('valSignupEmail', 'Invalid email address.', true);
            $U.focus(txtEmail);
            return;
        }

        var formFields = [['userName', userName],['password', password],['email', email]];

        Membership._showMessage('signupMessage', 'Signing up...', false);
        $get('btnSignup').disabled = true;

        $Ajax.post  (
                        Membership._signUpUrl,
                        formFields,
                        function(result)
                        {
                            $get('btnSignup').disabled = false;

                            if (result.isSuccessful == true)
                            {
                                $U.noDisplay('lnkLogin');
                                $U.noDisplay('lnkSignUp');

                                $U.display('lnkLogout');

                                Membership.set_isLoggedIn(true);
                                Membership._hide();
                                window.location.reload();
                            }
                            else
                            {
                                var msg = result.errorMessage;

                                $U.noDisplay('signupMessage');

                                if (msg.indexOf('username') > -1)
                                {
                                    Membership._showMessage('valSignupUserName', msg, true);
                                    $U.focus('txtSignupUserName');
                                }
                                else if (msg.indexOf('E-mail') > -1)
                                {
                                    Membership._showMessage('valSignupEmail', msg, true);
                                    $U.focus('txtSignupEmail');
                                }
                                else
                                {
                                    Membership._showMessage('signupMessage', msg, true);
                                }
                            }
                        },
                        function(error)
                        {
                            $get('btnSignup').disabled = false;
                            Membership._showMessage('signupMessage', 'An unexpected error has occurred while signning up.', true);
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

    _isValidEmail : function(email)
    {
        var regExp = /^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        return regExp.test(email);
    },

    _keyDownForLogin : function(e)
    {
        if (e.keyCode === Sys.UI.Key.enter)
        {
            Membership.login();
        }
        else if (e.keyCode === Sys.UI.Key.esc)
        {
            Membership._hide();
        }
    },

    _keyDownForForgotPassword : function(e)
    {
        if (e.keyCode === Sys.UI.Key.enter)
        {
            Membership.sendPassword();
        }
        else if (e.keyCode === Sys.UI.Key.esc)
        {
            Membership._hide();
        }
    },

    _keyDownForSignup : function(e)
    {
        if (e.keyCode === Sys.UI.Key.enter)
        {
            Membership.signUp();
        }
        else if (e.keyCode === Sys.UI.Key.esc)
        {
            Membership._hide();
        }
    },

    _closeButtonClick : function(e)
    {
        Membership._hide();
    }
}