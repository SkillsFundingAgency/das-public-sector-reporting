(function($) {
    'use strict';
    if ($.validator && $.validator.unobtrusive) {
        $.validator.unobtrusive.adapters.addSingleVal('maxwords', 'wordcount');
        $.validator.addMethod('maxwords', function(value, element, maxwords) {
            console.log('Method added');
            if (value) {
                if (value.split(' ').length > maxwords) {
                    return false;
                }
            }
            return true;
        });
    }

    /* CLASS
     * =============================== */
    var STACKable = function(e, o) {
        this.stacker = {
            e: null,
            html: '<li class="menu-link"><a href="#">More</a><ul></ul></li>',
            width: 0
        };
        this.e = e;
        this.navItems = {
            items: {},
            length: 0
        };
        this.navWidth = this.containerWidth = 0;

        var t = 0,
            tw = 0,
            to = this.navItems.items;
        this.e.children('li').each(function() {
            to[t] = {
                html: $(this)[0].outerHTML,
                width: $(this).outerWidth(true),
                submenu: false,
                visible: true
            };
            if ($(this).hasClass('submenu')) to[t].submenu = true;
            tw += to[t].width;
            t++;
        });
        this.navItems.length = t;
        this.navWidth = tw;

        this.stacker.e = $(this.stacker.html).appendTo(this.e);
        this.stacker.width = this.stacker.e.outerWidth(true);
        this.stacker.e.remove();

        this.magic();
    };

    STACKable.prototype.resetStack = function() {
        for (var tc = 0; tc < this.navItems.length; tc++) {
            this.navItems.items[tc].visible = true;
        }
    };

    STACKable.prototype.magic = function() {
        this.resetStack();
        this.containerWidth = this.e.parent().width() - 15;

        var match = 0,
            tc = 0;

        if (this.containerWidth < this.navWidth) {
            for (tc = 0; tc < this.navItems.length; tc++) {
                match += this.navItems.items[tc].width;
                if (match + this.stacker.width > this.containerWidth) {
                    this.navItems.items[tc].visible = false;
                }
            }
        }
        this.stack();
    };

    STACKable.prototype.stack = function() {
        this.e.empty();
        var showStacker = false,
            tc = 0;
        for (tc = 0; tc < this.navItems.length; tc++) {
            if (this.navItems.items[tc].visible) {
                $(this.navItems.items[tc].html).appendTo(this.e);
            } else {
                showStacker = true;
                break;
            }
        }

        if (showStacker) {
            this.stacker.e = $(this.stacker.html).appendTo(this.e);
            for (tc = 0; tc < this.navItems.length; tc++) {
                if (!this.navItems.items[tc].visible) {
                    var th = this.navItems.items[tc].html;
                    if (th.indexOf('<ul>') !== -1) {
                        th = th
                            .replace(/<a[^>]*>/, '<span class="sa-anchor-replacement">')
                            .replace(/<\/a>/, '</span>');
                    }
                    $(th).appendTo(this.stacker.e.children('ul'));
                    this.navItems.items[tc].visible = false;
                }
            }
        }
    };

    STACKable.prototype.setActions = function() {
        var te = this.e,
            menuSelector = 'li.submenu, li.menu-link',
            menuAnchorSelector = 'li.submenu>a, li.menu-link>a';

        $(te).on('click.stackable.nav', menuAnchorSelector, function() {
            if (
                $(this)
                    .parent()
                    .hasClass('sub-menu-open')
            ) {
                $(te.selector)
                    .children(menuSelector)
                    .removeClass('sub-menu-open');
            } else {
                GOVUK.navigation.closeAllOpenMenus();
                $(te.selector)
                    .children(menuSelector)
                    .removeClass('sub-menu-open');
                $(this)
                    .parent()
                    .toggleClass('sub-menu-open');
            }
            return false;
        });
        $(document).on('click.stackable.closure', function() {
            $(te.selector)
                .children(menuSelector)
                .removeClass('sub-menu-open');
            GOVUK.navigation.closeAllOpenMenus();
        });
        $(document).on('keydown', this, function(e) {
            if (e.keyCode !== 9) {
                $(te.selector)
                    .children(menuSelector)
                    .removeClass('sub-menu-open');
                sfa.navigation.closeAllOpenMenus();
            }
        });
    };

    /* PLUGIN
   * =============================== */
    $.fn.collapsableNav = function(options) {
        var defaults = { stackerLabel: '+' },
            ko = false;
        this.each(function() {
            if (this.tagName != 'UL') ko = true;
        });

        if (!ko) {
            var d = new STACKable(this, $.extend(defaults, options));
            d.setActions();
            $(window).resize(function() {
                d.magic();
            });
        }
    };
})(jQuery);

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

    if (window.GOVUK && GOVUK.navigation) {
        GOVUK.navigation.init();
        $('ul#global-nav-links').collapsableNav();
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
