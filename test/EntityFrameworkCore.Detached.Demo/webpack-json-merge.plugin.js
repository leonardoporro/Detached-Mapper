/* https://github.com/tettusud/merge-jsons-webpack-plugin
This is a modified version of merge-jsons-webpack-plugin from tettsud.
*/
"use strict";
var es6_promise_1 = require("es6-promise");
var path = require("path");
var Glob = require("glob");
var fs = require("fs");
var _root = path.resolve(__dirname, "./"); // project root folder
var MergeJsonWebpackPluginOptions = (function () {
    function MergeJsonWebpackPluginOptions() {
        this.bundles = [];
    }
    return MergeJsonWebpackPluginOptions;
}());
var MergeJsonWebpackPluginBundle = (function () {
    function MergeJsonWebpackPluginBundle() {
    }
    return MergeJsonWebpackPluginBundle;
}());
var MergeJsonWebpackPlugin = (function () {
    function MergeJsonWebpackPlugin(options) {
        this.options = options;
    }
    MergeJsonWebpackPlugin.prototype.apply = function (compiler) {
        var _this = this;
        compiler.plugin('emit', function (compilation, callback) {
            var _loop_1 = function(bundle) {
                _this.glob(bundle.pattern)
                    .then(function (files) {
                    return _this.loadJsonFilesAsync(files)
                        .then(function (json) {
                        _this.writeMergedJson(bundle.output, json);
                        for (var _i = 0, files_1 = files; _i < files_1.length; _i++) {
                            var file = files_1[_i];
                            console.log(path.join(compiler.context, file));
                            compilation.fileDependencies.push(path.join(compiler.context, file));
                        }
                        compilation.assets[bundle.output] = {
                            source: function () {
                                return json;
                            },
                            size: function () {
                                return json.length;
                            }
                        };
                        callback();
                    });
                });
            };
            for (var _i = 0, _a = _this.options.bundles; _i < _a.length; _i++) {
                var bundle = _a[_i];
                _loop_1(bundle);
            }
        });
    };
    MergeJsonWebpackPlugin.prototype.glob = function (pattern) {
        return new es6_promise_1.Promise(function (resolve, reject) {
            new Glob(pattern, { mark: true }, function (err, matches) {
                if (err) {
                    throw err;
                }
                resolve(matches);
            });
        });
    };
    MergeJsonWebpackPlugin.prototype.loadJsonFilesAsync = function (files) {
        var _this = this;
        return new es6_promise_1.Promise(function (resolve, reject) {
            var mergedJsons = {};
            for (var _i = 0, files_2 = files; _i < files_2.length; _i++) {
                var f = files_2[_i];
                f = f.trim();
                var entryData = undefined;
                try {
                    entryData = fs.readFileSync(f, "utf8");
                }
                catch (e) {
                    console.error("One of the entries in the files array given to the json-files-merge-plugin is not accessible (does not exist, unreadable, ...)", e);
                    throw e;
                }
                if (!entryData) {
                    throw new Error("One of the entries in the files array given to the json-files-merge-plugin could not be read: " + JSON.stringify(entryData));
                }
                // try to get a JSON object from the file data
                var entryDataAsJSON = {};
                try {
                    entryDataAsJSON = JSON.parse(entryData);
                }
                catch (e) {
                    console.error("Error parsing the json", e);
                    throw e;
                }
                if (typeof entryDataAsJSON !== "object") {
                    throw new Error("Not a valid object ");
                }
                _this.deepMerge(mergedJsons, entryDataAsJSON);
            }
            var retVal = JSON.stringify(mergedJsons);
            resolve(retVal);
        });
    };
    MergeJsonWebpackPlugin.prototype.deepMerge = function (target, source) {
        if (typeof target == "object" && typeof source == "object") {
            for (var key in source) {
                if (typeof source[key] == "object") {
                    if (!target[key])
                        Object.assign(target, (_a = {}, _a[key] = {}, _a));
                    this.deepMerge(target[key], source[key]);
                }
                else {
                    Object.assign(target, (_b = {}, _b[key] = source[key], _b));
                }
            }
        }
        return target;
        var _a, _b;
    };
    MergeJsonWebpackPlugin.prototype.writeMergedJson = function (_path, data) {
        try {
            this.ensureDirExistsAsync(_path)
                .then(function () {
                fs.writeFileSync(_path, data, "utf8");
            });
        }
        catch (e) {
            console.error("Unable to write output data to the file system ", e);
            throw e;
        }
    };
    MergeJsonWebpackPlugin.prototype.ensureDirExistsAsync = function (aPath) {
        var _this = this;
        return new es6_promise_1.Promise(function (resolve, reject) {
            _this.ensureDirExists(aPath);
            resolve();
        });
    };
    MergeJsonWebpackPlugin.prototype.ensureDirExists = function (aPath) {
        var dirname = path.dirname(aPath);
        if (fs.existsSync(dirname)) {
            return;
        }
        this.ensureDirExists(dirname);
        try {
            fs.mkdirSync(dirname);
        }
        catch (e) {
            console.error(" unable to create dir ", dirname, e);
        }
    };
    return MergeJsonWebpackPlugin;
}());
module.exports = MergeJsonWebpackPlugin;
//# sourceMappingURL=webpack-json-merge.plugin.js.map