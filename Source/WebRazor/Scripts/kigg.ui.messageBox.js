/// <reference path="jquery-1.7-vsdoc.js" />
/// <reference path="kigg.jquery.extensions.js" />
/// <reference path="kigg.globals.js" />
/// <reference path="kigg.namespace.js" />

kigg.namespace("ui.messageBox");
kigg.ui.messageBox = (function () {
    var self = {
        show: function () {
            $('#notification-box').data('tWindow').center().open();
        },
        dispose: function () {
        }
    };
    return self;
})();

