/* https://github.com/tettusud/merge-jsons-webpack-plugin 
This is a modified version of merge-jsons-webpack-plugin from tettsud.
*/

import { Promise } from "es6-promise";
let path = require("path");
let Glob = require("glob");
let fs = require("fs");
const _root = path.resolve(__dirname, "./"); // project root folder

class MergeJsonWebpackPluginOptions {
    public bundles: MergeJsonWebpackPluginBundle[] = [];
}

class MergeJsonWebpackPluginBundle {
    public pattern: string;
    public output: string;
}

class MergeJsonWebpackPlugin {
    private options: MergeJsonWebpackPluginOptions;

    constructor(options: MergeJsonWebpackPluginOptions) {
        this.options = options;
    }

    public apply(compiler: any) {
        compiler.plugin('emit', (compilation, callback) => {
            for (let bundle of this.options.bundles) {
                this.glob(bundle.pattern)
                    .then(files => {
                        return this.loadJsonFilesAsync(files)
                            .then(json => {
                                this.writeMergedJson(bundle.output, json);

                                for (let file of files) {
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
            }
        });
    }

    private glob(pattern: string): Promise<Array<string>> {
        return new Promise((resolve, reject) => {
            new Glob(pattern, { mark: true }, function (err, matches) {
                if (err) {
                    throw err;
                }
                resolve(matches);
            })
        });
    }

    private loadJsonFilesAsync(files: Array<any>): Promise<Array<Object>> {
        return new Promise((resolve, reject) => {
            let mergedJsons: any = {};
            for (let f of files) {

                f = f.trim();
                let entryData = undefined;

                try {
                    entryData = fs.readFileSync(f, "utf8");

                } catch (e) {
                    console.error("One of the entries in the files array given to the json-files-merge-plugin is not accessible (does not exist, unreadable, ...)", e);
                    throw e;
                }

                if (!entryData) {
                    throw new Error("One of the entries in the files array given to the json-files-merge-plugin could not be read: " + JSON.stringify(entryData));
                }

                // try to get a JSON object from the file data
                let entryDataAsJSON = {};

                try {
                    entryDataAsJSON = JSON.parse(entryData);
                } catch (e) {
                    console.error("Error parsing the json", e);
                    throw e;
                }

                if (typeof entryDataAsJSON !== "object") {
                    throw new Error("Not a valid object ");
                }
                this.deepMerge(mergedJsons, entryDataAsJSON);
            }
            let retVal = JSON.stringify(mergedJsons);
            resolve(retVal);
        });
    }

    private deepMerge(target, source) {
        if (typeof target == "object" && typeof source == "object") {
            for (const key in source) {
                if (typeof source[key] == "object") {
                    if (!target[key]) Object.assign(target, { [key]: {} });
                    this.deepMerge(target[key], source[key]);
                } else {
                    Object.assign(target, { [key]: source[key] });
                }
            }
        }
        return target;
    }

    private writeMergedJson(_path: string, data: any) {
        try {
            this.ensureDirExistsAsync(_path)
                .then(() => {
                    fs.writeFileSync(_path, data, "utf8");
                })
        } catch (e) {
            console.error("Unable to write output data to the file system ", e);
            throw e;
        }
    }

    private ensureDirExistsAsync(aPath: string) {
        return new Promise((resolve, reject) => {
            this.ensureDirExists(aPath);
            resolve();
        });
    }

    private ensureDirExists(aPath: string) {
        let dirname = path.dirname(aPath);
        if (fs.existsSync(dirname)) {
            return;
        }
        this.ensureDirExists(dirname);
        try {
            fs.mkdirSync(dirname);
        } catch (e) {
            console.error(" unable to create dir ", dirname, e);
        }

    }
}

module.exports = MergeJsonWebpackPlugin; 