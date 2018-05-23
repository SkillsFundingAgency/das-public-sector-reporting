(function() {
    'use strict';
    var root = this;

    if (typeof root.GOVUK === 'undefined') {
        root.GOVUK = {};
    }

    GOVUK.navigation = {
        elems: {
            userNav: $('nav#user-nav > ul'),
            levyNav: $('ul#global-nav-links')
        },
        init: function() {
            this.setupMenus(this.elems.userNav);
            this.setupEvents(this.elems.userNav);
        },
        setupMenus: function(n) {
            n
                .find('ul')
                .addClass('js-hidden')
                .attr('aria-hidden', 'true');
        },
        setupEvents: function(n) {
            var t = this;
            n.find('li.has-sub-menu > a').on('click', function(n) {
                var i = $(this);
                t.toggleMenu(i, i.next('ul'));
                n.stopPropagation();
                n.preventDefault();
            });
            n.find('li.has-sub-menu > ul > li > a').on('focusout', function() {
                var n = $(this);
                $(this)
                    .parent()
                    .is(':last-child') && t.toggleMenu(n, n.next('ul'));
            });
        },
        toggleMenu: function(n, t) {
            var i = n.parent();
            i.hasClass('open')
                ? (i.removeClass('open'), t.addClass('js-hidden').attr('aria-hidden', 'true'))
                : (this.closeAllOpenMenus(),
                  i.addClass('open'),
                  t.removeClass('js-hidden').attr('aria-hidden', 'false'));
        },
        closeAllOpenMenus: function() {
            this.elems.userNav
                .find('li.has-sub-menu.open')
                .removeClass('open')
                .find('ul')
                .addClass('js-hidden')
                .attr('aria-hidden', 'true');
            this.elems.levyNav
                .find('li.open')
                .removeClass('open')
                .addClass('js-hidden')
                .attr('aria-hidden', 'true');
        },
        linkSettings: function() {
            var n = $('a#link-settings'),
                t = this;
            this.toggleUserMenu();
            n.attr('aria-hidden', 'false');
            n.on('click touchstart', function(n) {
                var i = $(this).attr('href');
                $(this).toggleClass('open');
                t.toggleUserMenu();
                n.preventDefault();
            });
        },
        toggleUserMenu: function() {
            var n = this.elems.userNav.parent();
            n.hasClass('close')
                ? n.removeClass('close').attr('aria-hidden', 'false')
                : n.addClass('close').attr('aria-hidden', 'true');
        }
    };

    if (window.GOVUK && GOVUK.navigation) {
        GOVUK.navigation.init();
        $('ul#global-nav-links').collapsableNav();
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
                if (currentWordCount[currentWordCount.length - 1] === '')
                    currentWordCount.splice(1, 1);
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

    GOVUK.addCommas = {
        elems: {
            $inputs: $('.js-addcommas')
        },
        init: function() {
            this.elems.$inputs.on('input', this.onInput);
        },
        onInput: function(event) {
            event.target.value = event.target.value
                .replace(/\D/g, '') // removes all chars that are not digits
                .replace(/\B(?=(\d{3})+(?!\d))/g, ','); // add commas every 3 digits
        }
    };

    if (window.GOVUK && GOVUK.addCommas) {
        GOVUK.addCommas.init();
    }
}.call(this));
