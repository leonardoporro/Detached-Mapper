"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var ApiError = (function () {
    function ApiError() {
    }
    return ApiError;
}());
exports.ApiError = ApiError;
var ApiValidationError = (function (_super) {
    __extends(ApiValidationError, _super);
    function ApiValidationError() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return ApiValidationError;
}(ApiError));
exports.ApiValidationError = ApiValidationError;
var ApiErrorCodes = (function () {
    function ApiErrorCodes() {
    }
    return ApiErrorCodes;
}());
ApiErrorCodes.UnauthorizedAccess = "UnauthorizedAccess";
ApiErrorCodes.InternalError = "InternalError";
ApiErrorCodes.InvalidModel = "InvalidModel";
ApiErrorCodes.NotFound = "NotFound";
ApiErrorCodes.NoInternetConnection = "NoInternetConnection";
exports.ApiErrorCodes = ApiErrorCodes;
//# sourceMappingURL=error.js.map