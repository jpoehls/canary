/*jslint browser: true, indent: 2 */
(function (undefined) {
  "use strict";

  window.Canary = (function () {
    var reportError,
        loggerMethod = "POST";

    if (typeof (jQuery) !== 'undefined') {
      reportError = function (loggerUrl, params) {
        $.ajax(loggerUrl, {
          type: loggerMethod,
          data: params,
          cache: false,
          global: false // don't trigger global event handlers (like error handling)
        });
      };
    }
    else if (typeof (Ext) !== 'undefined') {
      reportError = function (loggerUrl, params) {
        Ext.Ajax.request({
          url: loggerUrl,
          method: loggerMethod,
          params: params,
          disableCaching: true
        });
      };
    }

    return {
      init: function (app, env, loggerUrl) {
        var originalErrorHandler = window.onerror;

        window.onerror = function (msg, url, line) {
          // report the error
          if (reportError) {
            try {
              reportError(loggerUrl, {
                app: app,
                env: env,
                level: 1,
                message: msg,
                source: url + ":" + line,
                type: "ClientScriptException",
                details: JSON.stringify(
                 { platform: navigator.platform,
                   userAgent: navigator.userAgent,
                   pageUrl: window.location.href
                 })
              });
            } catch (e) {
              // do nothing
            }
          }

          // bubble up the error to the original handler (probably the browser)
          if (originalErrorHandler) {
            originalErrorHandler(msg, url, line);
          }
        };
      }
    };

  } ());
} ());