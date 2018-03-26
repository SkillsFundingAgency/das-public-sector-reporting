// Write your JavaScript code.

$(function() {
  var formValidator,
    $form = $('.questionForm');

  // add handler to the forms submit action
  $form.submit(function() {
    if (!formValidator) {
      formValidator = $form.validate({}); // Get existing jquery validate object
    }

    var errorList = [];

    // get existing summary errors from jQuery validate
    $.each(formValidator.errorList, function(index, errorListItem) {
      errorList.push(errorListItem.message);
    });

    // add our own errors
    if (testForErrorCondidtionA()) {
      errorList.push('Please fix error condition A!');
    }

    if (testForErrorCondidtionB()) {
      errorList.push('Please fix error condition B!');
    }

    // No errors, do nothing
    if (0 === errorList.length) {
      return true; // allow submit
    }

    // find summary div
    var $summary = $form.find('[data-valmsg-summary=true]');

    // find the unordered list
    var $ul = $summary.find('ul');

    // Clear existing errors from DOM by removing all element from the list
    $ul.empty();

    // Add all errors to the list
    $.each(errorList, function(index, message) {
      $('<li />')
        .html(message)
        .appendTo($ul);
    });

    // Add the appropriate class to the summary div
    $summary.removeClass('validation-summary-valid').addClass('validation-summary-errors');

    return false; // Block the submit
  });
});
