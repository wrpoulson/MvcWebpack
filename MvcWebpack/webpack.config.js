const path = require('path');
const ExtractTextPlugin = require("extract-text-webpack-plugin");

module.exports = {
    entry: {
       appOne: './Scripts/src/appOne/main.js'
    },
    output: {
        path: path.resolve(__dirname, './Scripts/build/js'),
        filename: '[name]-bundle.js'
    },
    module: {
        rules: [
            {
                loader: 'babel-loader',
                test: /\.js$/,
                exclude: /node_modules/
            },
            {
                test: /\.css$/,
                use: ExtractTextPlugin.extract({
                    use: "css-loader",
                }),
                exclude: /node_modules/
            },
        ]
    },
    plugins: [
        new ExtractTextPlugin("../css/[name]-bundle.css"),
    ]
}