window.DASFrontend.initAll();

(function () {
  "use strict";
  var root = this;

  if (typeof root.GOVUK === "undefined") {
    root.GOVUK = {};
  }

  GOVUK.addCommas = {
    elems: $(".js-addcommas"),
    init: function () {
      if (!this.elems.length) return false;
      $(window).on("load", this.onLoaded(this));
      this.elems.on("input", this.onInput(this));
    },
    onInput: function (self) {
      return function (event) {
        event.target.value = self.convertString(event.target.value);
      };
    },
    onLoaded: function (self) {
      return function () {
        self.elems.each(function () {
          if ($(this).is("input")) {
            $(this).val(self.convertString($(this).val()));
          } else {
            $(this).text(self.convertString($(this).text()));
          }
        });
      };
    },
    convertString: function (string) {
      return string.match(/^([0])\1*$/g)
        ? "0"
        : string
            .replace(/\D/g, "") // removes all chars that are not digits
            .replace(/^0+/g, "") // remove leading zeroes
            .replace(/\B(?=(\d{3})+(?!\d))/g, ","); // add commas every 3 digits
    },
  };

  if (window.GOVUK && GOVUK.addCommas) {
    GOVUK.addCommas.init();
  }
}).call(this);
