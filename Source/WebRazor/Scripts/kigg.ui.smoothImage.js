/// <reference path="jquery-1.7-vsdoc.js" />
/// <reference path="kigg.jquery.extensions.js" />

kigg.namespace("ui.smoothImage");
kigg.ui.smoothImage = (function () {
    var self = {
        show: function (img) {
            if ($.isDefined(img)) {
                $(img).fadeIn('slow');
            }
            else {
                $('img.smooth-image').fadeIn('slow');
            }
        },
        dispose: function () {
            console.log('smoothImage dispose');
        }
    };
    return self;
} ());
