(function($) {
  'use strict';
  if ($.validator && $.validator.unobtrusive) {
    $.validator.unobtrusive.adapters.addSingleVal('maxwords', 'wordcount');
    $.validator.addMethod('maxwords', function(value, element, maxwords) {
      var wordArray = GOVUK.wordCount.createWordArray(value);
      return value
        ? this.optional(element) || wordArray.length <= maxwords
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
