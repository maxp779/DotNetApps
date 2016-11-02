var path = require('path');
var webpack = require('webpack');

module.exports = {
    entry: {
        main: './Scripts/main',
    },
    debug:false,
    //this allows for stepping through code in the chrome debugger
    //debug: true,
    //devtool: "#eval-source-map",
    output: {
        publicPath: "/js/",
        path: path.join(__dirname, '/wwwroot/js/'),
        filename: 'countdownsolver.min.js'
    },


    plugins: [
      new webpack.DefinePlugin({
          'process.env': {
              'NODE_ENV': JSON.stringify('production')
          }
      }),
        new webpack.optimize.UglifyJsPlugin({
            include: /\.min\.js$/,
            minimize: true
        })
    ],

    module: {
        loaders: [
          {
              test: /.jsx?$/,
              loader: 'babel-loader',
              //exclude: /node_modules/,
              include: [
                path.resolve(__dirname, "Scripts"),
              ],
              query: {
                  presets: ['es2015', 'react']
              }
          }
        ],
    },
};