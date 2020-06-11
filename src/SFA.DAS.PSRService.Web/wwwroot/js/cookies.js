
(function (root) {
    'use strict'
    window.GOVUK = window.GOVUK || {}

    var DEFAULT_COOKIE_CONSENT = {
        'AnalyticsConsent': true,
        'MarketingConsent': false
    }

    window.GOVUK.cookie = function (name, value, options) {
        if (typeof value !== 'undefined') {
            if (value === false || value === null) {
                return window.GOVUK.setCookie(name, '', { days: -1 })
            } else {
                // Default expiry date of 30 days
                if (typeof options === 'undefined') {
                    options = { days: 30 }
                }
                return window.GOVUK.setCookie(name, value, options)
            }
        } else {
            return window.GOVUK.getCookie(name)
        }
    }

    window.GOVUK.approveAllCookieTypes = function () {
        Object.keys(DEFAULT_COOKIE_CONSENT).forEach(function (cookie) {
            window.GOVUK.setCookie(cookie, DEFAULT_COOKIE_CONSENT[cookie], { days: 365 })
        });
    }

    window.GOVUK.getConsentCookie = function () {
        var consentCookie = window.GOVUK.cookie('cookie_policy')
        var consentCookieObj

        if (consentCookie) {
            try {
                consentCookieObj = JSON.parse(consentCookie)
            } catch (err) {
                return null
            }

            if (typeof consentCookieObj !== 'object' && consentCookieObj !== null) {
                consentCookieObj = JSON.parse(consentCookieObj)
            }
        } else {
            return null
        }

        return consentCookieObj
    }

    window.GOVUK.setConsentCookie = function (options) {
        var cookieConsent = window.GOVUK.getConsentCookie()

        if (!cookieConsent) {
            cookieConsent = JSON.parse(JSON.stringify(DEFAULT_COOKIE_CONSENT))
        }

        for (var cookieType in options) {
            cookieConsent[cookieType] = options[cookieType]

            // Delete cookies of that type if consent being set to false
            if (!options[cookieType]) {
                for (var cookie in COOKIE_CATEGORIES) {
                    if (COOKIE_CATEGORIES[cookie] === cookieType) {
                        window.GOVUK.cookie(cookie, null)

                        if (window.GOVUK.cookie(cookie)) {
                            document.cookie = cookie + '=;expires=' + new Date() + ';domain=.' + window.location.hostname + ';path=/'
                        }
                    }
                }
            }
        }

        window.GOVUK.setCookie('cookie_policy', JSON.stringify(cookieConsent), { days: 365 })
    }

    window.GOVUK.setCookie = function (name, value, options) {

        if (typeof options === 'undefined') {
            options = {}
        }
        var cookieString = name + '=' + value + '; path=/'

        if (options.days) {
            var date = new Date()
            date.setTime(date.getTime() + (options.days * 24 * 60 * 60 * 1000))
            cookieString = cookieString + '; expires=' + date.toGMTString()
        }

        if (document.location.protocol === 'https:') {
            cookieString = cookieString + '; Secure'
        }

        document.cookie = cookieString
    }

    window.GOVUK.getCookie = function (name) {
        var nameEQ = name + '='
        var cookies = document.cookie.split(';')
        for (var i = 0, len = cookies.length; i < len; i++) {
            var cookie = cookies[i]
            while (cookie.charAt(0) === ' ') {
                cookie = cookie.substring(1, cookie.length)
            }
            if (cookie.indexOf(nameEQ) === 0) {
                return decodeURIComponent(cookie.substring(nameEQ.length))
            }
        }
        return null
    }
}(window))

function CookieSettings($module) {
    this.$module = $module;
    this.DEFAULT_COOKIE_CONSENT = [
        {
            name: 'AnalyticsConsent',
            value: true
        },
        {
            name: 'MarketingConsent',
            value: false
        }
    ]
    this.start()
}

CookieSettings.prototype.start = function () {

    this.$module.submitSettingsForm = this.submitSettingsForm.bind(this)

    document.querySelector('form[data-module=cookie-settings]').addEventListener('submit', this.$module.submitSettingsForm)

    this.setInitialFormValues()
}

CookieSettings.prototype.setInitialFormValues = function () {

    var cookieSettings = this.DEFAULT_COOKIE_CONSENT

    cookieSettings.forEach(function (cookieSetting) {

        var currentConsentCookie = window.GOVUK.cookie(cookieSetting.name)
        var radioButton

        if (currentConsentCookie === 'true' || cookieSetting.value === true) {
            radioButton = document.querySelector('input[name=cookies-' + cookieSetting.name + '][value=on]')
        } else {
            radioButton = document.querySelector('input[name=cookies-' + cookieSetting.name + '][value=off]')
        }

        radioButton.checked = true

    });

}

CookieSettings.prototype.submitSettingsForm = function (event) {

    event.preventDefault()

    var formInputs = event.target.getElementsByTagName("input")

    for (var i = 0; i < formInputs.length; i++) {
        var input = formInputs[i]
        if (input.checked) {
            var name = input.name.replace('cookies-', '')
            var value = input.value === "on" ? true : false
            window.GOVUK.setCookie(name, value, { days: 365 })
        }
    }

    if (!window.GOVUK.cookie("DASSeenCookieMessage")) {
        window.GOVUK.setCookie("DASSeenCookieMessage", true, { days: 365 })
    }

    this.showConfirmationMessage()

    return false
}

CookieSettings.prototype.showConfirmationMessage = function () {
    var confirmationMessage = document.querySelector('div[data-cookie-confirmation]')
    var previousPageLink = document.querySelector('.cookie-settings__prev-page')

    document.body.scrollTop = document.documentElement.scrollTop = 0

    confirmationMessage.style.display = "block"
}

CookieSettings.prototype.getReferrerLink = function () {
    return document.referrer ? new URL(document.referrer).pathname : false
}


function CookieBanner(module) {
    this.module = module;
    this.settings = {
        seenCookieName: 'DASSeenCookieMessage',
        cookiePolicy: {
            AnalyticsConsent: false
        }
    }
    if (!window.GOVUK.cookie(this.settings.seenCookieName)) {
        this.start()
    }
}

CookieBanner.prototype.start = function () {
    this.module.cookieBanner = this.module.querySelector('.das-cookie-banner')
    this.module.cookieBannerInnerWrap = this.module.querySelector('.das-cookie-banner__wrapper')
    this.module.cookieBannerConfirmationMessage = this.module.querySelector('.das-cookie-banner__confirmation')
    this.setupCookieMessage()
}


CookieBanner.prototype.setupCookieMessage = function () {
    this.module.hideLink = this.module.querySelector('button[data-hide-cookie-banner]')
    this.module.acceptCookiesButton = this.module.querySelector('button[data-accept-cookies]')

    if (this.module.hideLink) {
        this.module.hideLink.addEventListener('click', this.hideCookieBanner.bind(this))
    }

    if (this.module.acceptCookiesButton) {
        this.module.acceptCookiesButton.addEventListener('click', this.acceptAllCookies.bind(this))
    }
    this.showCookieBanner()
}

CookieBanner.prototype.showCookieBanner = function () {
    var cookiePolicy = this.settings.cookiePolicy;
    this.module.cookieBanner.style.display = 'block';
    Object.keys(cookiePolicy).forEach(function (cookieName) {
        window.GOVUK.cookie(cookieName, cookiePolicy[cookieName].toString(), { days: 365 })
    });
}


CookieBanner.prototype.hideCookieBanner = function () {
    this.module.cookieBanner.style.display = 'none';
    window.GOVUK.cookie(this.settings.seenCookieName, true, { days: 365 })
}


window.GOVUK = window.GOVUK || {}

window.GOVUK.cookie = function (name, value, options) {
    if (typeof value !== 'undefined') {
        if (value === false || value === null) {
            return window.GOVUK.setCookie(name, '', { days: -1 })
        } else {
            // Default expiry date of 30 days
            if (typeof options === 'undefined') {
                options = { days: 30 }
            }
            return window.GOVUK.setCookie(name, value, options)
        }
    } else {
        return window.GOVUK.getCookie(name)
    }
}

window.GOVUK.setCookie = function (name, value, options) {
    if (typeof options === 'undefined') {
        options = {}
    }
    var cookieString = name + '=' + value + '; path=/'

    if (options.days) {
        var date = new Date()
        date.setTime(date.getTime() + (options.days * 24 * 60 * 60 * 1000))
        cookieString = cookieString + '; expires=' + date.toGMTString()
    }
    if (!options.domain) {
        options.domain = window.GOVUK.getDomain()
    }

    if (document.location.protocol === 'https:') {
        cookieString = cookieString + '; Secure'
    }
    document.cookie = cookieString + ';domain=' + options.domain
}

window.GOVUK.getCookie = function (name) {
    var nameEQ = name + '='
    var cookies = document.cookie.split(';')
    for (var i = 0, len = cookies.length; i < len; i++) {
        var cookie = cookies[i]
        while (cookie.charAt(0) === ' ') {
            cookie = cookie.substring(1, cookie.length)
        }
        if (cookie.indexOf(nameEQ) === 0) {
            return decodeURIComponent(cookie.substring(nameEQ.length))
        }
    }
    return null
}

window.GOVUK.getDomain = function () {
    return window.location.hostname !== 'localhost'
        ? '.' + window.location.hostname.slice(window.location.hostname.indexOf('.') + 1)
        : window.location.hostname;
}

// Legacy cookie clean up
var currentDomain = window.location.hostname;
var cookieDomain = window.GOVUK.getDomain();

if (currentDomain !== cookieDomain) {
    // Delete the 3 legacy cookies without the domain attribute defined
    document.cookie = "DASSeenCookieMessage=false; path=/;SameSite=None; expires=Thu, 01 Jan 1970 00:00:01 GMT";
    document.cookie = "AnalyticsConsent=false; path=/;SameSite=None; expires=Thu, 01 Jan 1970 00:00:01 GMT";
    document.cookie = "MarketingConsent=false; path=/;SameSite=None; expires=Thu, 01 Jan 1970 00:00:01 GMT";
}

var $cookieBanner = document.querySelector('[data-module="cookie-banner"]');
if ($cookieBanner != null) {
    new CookieBanner($cookieBanner);
}
