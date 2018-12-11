/// <binding BeforeBuild='less, min' Clean='clean' />
"use strict";

var gulp = require("gulp"),
    fs = require("fs"),
    less = require("gulp-less"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    htmlmin = require("gulp-htmlmin"),
    uglify = require("gulp-uglify-es").default,
    merge = require("merge-stream"),
    del = require("del"),
    bundleconfig = require("./bundleconfig.json");

var gutil = require('gulp-util');

var regex = {
    css: /\.css$/,
    html: /\.(html|htm)$/,
    js: /\.js$/
};

gulp.task("less", function () {
    return gulp.src('Styles/main.less')
        .pipe(less())
        .pipe(gulp.dest('wwwroot/css'));
});

gulp.task("min", ["min:js", "min:css", "min:html"]);

gulp.task("min:js", function () {
    var tasks = getBundles(regex.js).map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: "." })
            .pipe(concat(bundle.outputFileName))
            .pipe(uglify())
            .on('error', function (err) { gutil.log(gutil.colors.red('[Error]'), err.toString()); })
            .pipe(gulp.dest("."));
    });
    return merge(tasks);
});

gulp.task("min:css", function () {
    var tasks = getBundles(regex.css).map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: "." })
            .pipe(concat(bundle.outputFileName))
            .pipe(cssmin())
            .pipe(gulp.dest("."));
    });
    return merge(tasks);
});

gulp.task("min:html", function () {
    var tasks = getBundles(regex.html).map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: "." })
            .pipe(concat(bundle.outputFileName))
            .pipe(htmlmin({ collapseWhitespace: true, minifyCSS: true, minifyJS: true }))
            .pipe(gulp.dest("."));
    });
    return merge(tasks);
});

gulp.task("clean", function () {
    var files = bundleconfig.map(function (bundle) {
        return bundle.outputFileName;
    });

    return del(files);
});

gulp.task("watch", function () {
    gulp.watch('Styles/**/*.less', ['less']);

    getBundles(regex.js).forEach(function (bundle) {
        gulp.watch(bundle.inputFiles, ["min:js"]);
    });

    getBundles(regex.css).forEach(function (bundle) {
        gulp.watch(bundle.inputFiles, ["min:css"]);
    });

    getBundles(regex.html).forEach(function (bundle) {
        gulp.watch(bundle.inputFiles, ["min:html"]);
    });
});

function getBundles(regexPattern) {
    return bundleconfig.filter(function (bundle) {
        return regexPattern.test(bundle.outputFileName);
    });
}