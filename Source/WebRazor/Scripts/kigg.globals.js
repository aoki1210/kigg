/// <reference path="jquery-1.7-vsdoc.js" />
/// <reference path="kigg.namespace.js" />
/// <reference path="kigg.jquery.extensions.js" />
/// <reference path="kigg.ui.membership.js" />
/// <reference path="kigg.ui.smoothImage.js" />
kigg.dispose = (function () {
    return function () {
        for (var prop in kigg.ui) {
            for (var funcProp in kigg.ui[prop]) {
                var obj = kigg.ui[prop][funcProp];
                if (typeof obj !== 'undefined' && $.isFunction(obj) && funcProp === 'dispose') {
                    obj();
                }
            }
        }
    };
} ());
//Globals declaration
var membership = kigg.ui.membership;
var smoothImage = kigg.ui.membership;

