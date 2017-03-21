let webpack = require("webpack");
let ExtractTextPlugin = require("extract-text-webpack-plugin");
let ExtractTextPluginCss = new ExtractTextPlugin("../css/[name].css");
var MergeJsonWebpackPlugin = require("./webpack-json-merge.plugin");

module.exports = {
    entry: {
        app: ["./client/main"]
    },
    devtool: "source-map",
    output: {
        filename: "[name].js",
        path: __dirname + "/wwwroot/js",
        publicPath: "/"
    },
    module: {
        preLoaders: [
            { test: /\.js$/, loader: "source-map-loader" }
        ],
        loaders: [
            // all typescript.
            //{ test: /\.ts$/, loader: "ts" },
            // component embedded styles and templates.
            { test: /\.html$/, loader: "html" },
            // styles.
            { test: /\.css$/, loader: ExtractTextPluginCss.extract(["css"]) },
            // images and fonts.
            { test: /\.(png|jpg|jpeg|gif|svg)$/, loader: "url", query: { limit: 25000, name: "../images/[name].[ext]" } },
            { test: /\.(ttf|eot|svg|woff|woff2)$/, loader: "url", query: { limit: 25000, name: "../fonts/[name].[ext]" } }
        ]
    },
    plugins: [
       ExtractTextPluginCss,
       new webpack.DllReferencePlugin({
           context: __dirname,
           manifest: require("./wwwroot/js/vendor-manifest.json")
       }),
       //new MergeJsonWebpackPlugin({
       //    "bundles": [{
       //        "pattern": "client/**/*.i18n.json",
       //        "output": "./wwwroot/lang/res_en.json"
       //    }]
       //})
    ]
};
