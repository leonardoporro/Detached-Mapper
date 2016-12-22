let webpack = require("webpack");
let ExtractTextPlugin = require("extract-text-webpack-plugin");
let extractCss = new ExtractTextPlugin("../css/[name].css");

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
            { test:   /\.js$/, loader: "source-map-loader" }
        ],
        loaders: [
            // all typescript.
            //{ test: /\.ts$/, loader: "ts" },
            // component embedded styles and templates.
            { test: /\.component\.html$/, loader: "raw" },
            { test: /\.component\.css$/, loaders: ["to-string", "css"] },
            { test: /\.component\.scss$/, loaders: ["to-string", "css", "sass"] },
            // other styles.
            { test: /^((?!component).)*\.scss$/, loader: extractCss.extract(["css", "sass"]) },
            { test: /^((?!component).)*\.css$/, loader: extractCss.extract(["css"]) },
            // images and fonts.
            { test: /\.(png|jpg|jpeg|gif|svg)$/, loader: "url", query: { limit: 25000, name: "../images/[name].[ext]" } },
            { test: /\.(ttf|eot|svg|woff|woff2)$/, loader: "url", query: { limit: 25000, name: "../fonts/[name].[ext]" } }
        ]
    },
    plugins: [
       extractCss,
       new webpack.DllReferencePlugin({
           context: __dirname,
           manifest: require("./wwwroot/js/dll-manifest.json")
       })
    ]
};
