let webpack = require("webpack");
let ExtractTextPlugin = require("extract-text-webpack-plugin");
let extractCss = new ExtractTextPlugin("../css/[name].css");

module.exports = {
    cache: true,
    entry: {
        dll: ["./client/dll/polyfills", "./client/dll/vendor"]
    },
    output: {
        filename: "[name].js",
        path: __dirname + "/wwwroot/js",
        library: "dll_[hash]"
    },
    plugins: [
       extractCss,
       new webpack.DllPlugin({
           path: "./wwwroot/js/dll-manifest.json",
           name: "dll_[hash]"
       })
    ],
    module: {
        loaders: [
            { test: /\.ts$/, loader: "ts" },
            { test: /\.html$/, loader: "raw" },
            { test: /\.scss$/, loader: extractCss.extract(["css", "sass"]) },
            { test: /\.css$/, loader: extractCss.extract(["css"]) },
            { test: /\.(png|jpg|jpeg|gif|svg)$/, loader: "url", query: { limit: 25000, name: "../images/[name].[ext]" } },
            { test: /\.(ttf|eot|svg|woff|woff2)$/, loader: "url", query: { limit: 25000, name: "../fonts/[name].[ext]" } }
        ]
    }
};