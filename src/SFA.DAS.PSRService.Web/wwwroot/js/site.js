﻿$(function() {
  if ($.validator && $.validator.unobtrusive) {
    $.validator.unobtrusive.adapters.addSingleVal('maxwords', 'wordcount');
    $.validator.addMethod('maxwords', function(value, element, maxwords) {
      console.log(value, element, maxwords);
      if (value) {
        if (value.split(' ').length > maxwords) {
          return false;
        }
      }
      return true;
    });
  }

  // var formValidator,
  //   $form = $('.questionForm');

  // // add handler to the forms submit action
  // $form.submit(function() {
  //   if (!formValidator) {
  //     formValidator = $form.validate({}); // Get existing jquery validate object
  //     // console.log(formValidator);
  //   }

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
  // });
});

(function() {
  'use strict';
  var root = this;
  if (typeof root.GOVUK === 'undefined') {
    root.GOVUK = {};
  }

  GOVUK.wordCount = {
    elems: {
      $wordLengthTextarea: $('.js-word-limit')
    },
    init: function() {
      if (!this.elems.$wordLengthTextarea.length) return false;
      this.elems.$wordLengthTextarea.on('keyup input', this.countWords(this));
      this.countWords(this).call();
    },
    countWords: function(self) {
      return function() {
        var wordLimit = self.elems.$wordLengthTextarea.data('word-limit');
        var currentWordCount = self.elems.$wordLengthTextarea.val().split(/[\s]+/);
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
      };
    }
  };

  if (window.GOVUK && GOVUK.wordCount) {
    GOVUK.wordCount.init();
  }

  GOVUK.stickyNav = {
    elems: {
      $nav: $('.floating-menu'),
      $navHolder: $('#floating-menu-holder'),
      $window: $(window),
      $body: $(document.body)
    },
    init: function() {
      if (!this.elems.$nav.length) return false;
      this.elems.topOfNav = this.elems.$navHolder.offset().top;
      this.elems.$window.on('scroll', this.fixedNav(this));
      this.elems.$window.on('resize', this.pageResized(this));
    },
    pageResized: function(self) {
      return function() {
        self.elems.topOfNav = self.elems.$navHolder.offset().top;
      };
    },
    fixedNav: function(self) {
      return function() {
        var isSticky = self.elems.$body.hasClass('sticky-nav');
        if (self.elems.$window.scrollTop() >= self.elems.topOfNav) {
          if (!isSticky) {
            self.elems.$body
              .addClass('sticky-nav')
              .css('padding-top', self.elems.$nav.height() + 'px');
          }
        } else if (isSticky) {
          self.elems.$body.removeClass('sticky-nav').css('padding-top', 0);
        }
      };
    }
  };

  if (window.GOVUK && GOVUK.stickyNav) {
    GOVUK.stickyNav.init();
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
