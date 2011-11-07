/// <reference path="jquery-1.7-vsdoc.js" />
/// <reference path="kigg.jquery.extensions.js" />

kigg.namespace("ui.membership");
kigg.ui.membership = (function () {
    //Privates
    var $loginLink = $('#login-link'),
        bindHandlers = function () {
            $loginLink.click(function (e) {
                e.preventDefault();
                self.showLogin();
            });
        };
    //module object return
    var self = {
        init: function () {
            bindHandlers();
            $('form.openid').openid();
        },
        showLogin: function () {
            $('#login-box').data('tWindow').center().open();
        },
        dispose: function () {
            $loginLink.unbind();
        }
    };
    return self;
} ());