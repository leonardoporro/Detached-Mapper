"use strict";
var HttpRestBaseDataSource = (function () {
    function HttpRestBaseDataSource() {
    }
    /**@desc replaces url params with query values, e.g.: /api/users/:id -> /api/users/1
      *@param baseUrl - the template url.
      *@param params - the replacement values.
      */
    HttpRestBaseDataSource.prototype.resolveUrl = function (baseUrl, params) {
        return baseUrl.replace(/:[a-zA-z0-9]+/, function (match, args) {
            var paramName = match.substr(1);
            var value = params.get(paramName);
            if (!value)
                throw new URIError("parameter " + match + " in url '" + this.url + "' is not defined.");
            params.delete(paramName); //don't send duplicated parameters.
            return value;
        }.bind(this));
    };
    return HttpRestBaseDataSource;
}());
exports.HttpRestBaseDataSource = HttpRestBaseDataSource;
//# sourceMappingURL=base.datasource.js.map