(function($) {
  'use strict';
  if ($.validator && $.validator.unobtrusive) {
    console.log($.validator.unobtrusive.adapters);
    $.validator.unobtrusive.adapters.addSingleVal('maxwords', 'wordcount');
    $.validator.addMethod('maxwords', function(value, element, maxwords) {
      return value
        ? this.optional(element) || value.split(' ').length < maxwords
        : true;
    });

    $.validator.addMethod('number', function(value, element) {
      var nocommas = parseInt(value.replace(/,/g, ''));
      return value
        ? this.optional(element) ||
            (Number.isInteger(nocommas) && nocommas >= 0)
        : true;
    });
  }
})(jQuery);
