(function($) {
    'use strict';
    if ($.validator && $.validator.unobtrusive) {
        $.validator.unobtrusive.adapters.addSingleVal('maxwords', 'wordcount');
        $.validator.addMethod('maxwords', function(value, element, maxwords) {
            if (value) {
                if (value.split(' ').length > maxwords) {
                    return false;
                }
            }
            return true;
        });
    }
})(jQuery);
