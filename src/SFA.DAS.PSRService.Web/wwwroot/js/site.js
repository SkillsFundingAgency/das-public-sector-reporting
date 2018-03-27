// Write your JavaScript code.

$(function() {
  var formValidator,
    $form = $('.questionForm');

  // add handler to the forms submit action
  $form.submit(function() {
    if (!formValidator) {
      formValidator = $form.validate({}); // Get existing jquery validate object
      // console.log(formValidator);
    }

    /* CODE BELOW TO ADD CUSTOM ERRORS 
    /*
    /* https://stackoverflow.com/questions/28840307/add-error-to-jquery-unobtrusive-validation-summary-without-a-key
    /*
    /* Modify to update current erros with corect oned for GDS
    */

    // var errorList = [];

    // // get existing summary errors from jQuery validate
    // $.each(formValidator.errorList, function(index, errorListItem) {
    //   errorList.push(errorListItem.message);
    // });

    // // add our own errors
    // if (testForErrorCondidtionA()) {
    //   errorList.push('Please fix error condition A!');
    // }

    // if (testForErrorCondidtionB()) {
    //   errorList.push('Please fix error condition B!');
    // }

    // // No errors, do nothing
    // if (0 === errorList.length) {
    //   return true; // allow submit
    // }

    // // find summary div
    // var $summary = $form.find('[data-valmsg-summary=true]');

    // // find the unordered list
    // var $ul = $summary.find('ul');

    // // Clear existing errors from DOM by removing all element from the list
    // $ul.empty();

    // // Add all errors to the list
    // $.each(errorList, function(index, message) {
    //   $('<li />')
    //     .html(message)
    //     .appendTo($ul);
    // });

    // // Add the appropriate class to the summary div
    // $summary.removeClass('validation-summary-valid').addClass('validation-summary-errors');

    // return false; // Block the submit
  });
});

/*
*
*
<textarea class="form-control js-word-limit" data-word-limit="250" rows="8"></textarea>
<span class="sfa-form-control-after">
    <span class="words-left">5</span>
</span>
*/

(function() {
  'use strict';
  var root = this;
  if (typeof root.GOVUK === 'undefined') {
    root.GOVUK = {};
  }

  GOVUK.wordCount = function() {
    var wordLengthTextarea = $('.js-word-limit');
    if (!wordLengthTextarea.length) return false;

    var wordLimit = wordLengthTextarea.data('word-limit');
    var currentWordCount = [];

    function countWords(event) {
      currentWordCount = wordLengthTextarea.val().split(/[\s]+/);
      if (currentWordCount[0] === '') currentWordCount.splice(0, 2);
      if (currentWordCount[currentWordCount.length - 1] === '') currentWordCount.splice(1, 1);
      // console.log(currentWordCount.length + ' words are typed out of an available ' + wordLimit);
      var wordsLeft = wordLimit - currentWordCount.length;
      if (currentWordCount === '') wordsLeft = wordLimit;
      var word_s = wordsLeft === 1 || wordsLeft === -1 ? 'word' : 'words';
      if (currentWordCount.length <= wordLimit) {
        $('.words-left').html(wordsLeft + ' ' + word_s + ' left');
      } else {
        $('.words-left').html((wordsLeft *= -1) + ' ' + word_s + ' too many');
      }
    }

    wordLengthTextarea.on('keyup input', countWords);
    countWords();
  };

  if (window.GOVUK && GOVUK.wordCount) {
    GOVUK.wordCount();
  }
}.call(this));

//   $(document)
//     .one('focus.js-auto-expand', 'textarea.js-auto-expand', function() {
//       var savedValue = this.value;
//       this.value = '';
//       this.baseScrollHeight = this.scrollHeight;
//       this.value = savedValue;
//       console.log(Math.ceil((this.scrollHeight - this.baseScrollHeight)))
//     })
//     .on('input.js-auto-expand', 'textarea.js-auto-expand', function() {
//       var minRows = this.getAttribute('data-min-rows') | 0,
//         rows;
//       this.rows = minRows;
//       rows = Math.ceil((this.scrollHeight - this.baseScrollHeight) / 16);
//       console.log(rows)
//       this.rows = minRows + rows;

//     });
