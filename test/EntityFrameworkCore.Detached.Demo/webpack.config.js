let webpack = require("webpack");
var path = require("path");

var isDevBuild = process.argv.indexOf("--env.prod") < 0;
let ExtractTextPlugin = require("extract-text-webpack-plugin");
let extractCss = new ExtractTextPlugin("./css/[name].css");

module.exports = {
    cache: true,
    entry: {
        app: ["./client/index.ts"]
    },
    output: {
        filename: "./js/[name].js",
        path: __dirname + "/wwwroot",
        publicPath: "/"
    },
    plugins: [
        extractCss,
        new webpack.DllReferencePlugin({
            context: __dirname,
            manifest: require("./wwwroot/js/vendor-manifest.json")
        }),
    ],
    module: {
        loaders: [
            { test: /\.ts$/, loader: "ts" },
            { test: /\.html$/, loader: "raw" },
            { test: /\.scss$/, loader: extractCss.extract(["css", "sass"]) },
            { test: /\.css$/, loader: extractCss.extract(["css"]) },
            { test: /\.(png|jpg|jpeg|gif|svg)$/, loader: "url", query: { limit: 25000, name: "./images/[name].[ext]" } },
            { test: /\.(ttf|eot|svg|woff|woff2)$/, loader: "url", query: { limit: 25000, name: "./fonts/[name].[ext]" } }
        ]
    },
    resolve: {
        extensions: ["", ".ts", ".js", ".jsx"],
        root: path.resolve(__dirname, "client"),
        modulesDirectories: ["node_modules"]
    }
};
