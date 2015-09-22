/// <binding BeforeBuild='sass' Clean='clean' />

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    $ = require("gulp-load-plugins")(),
    project = require("./project.json");

var paths = {
    webroot: "./" + project.webroot + "/"
};

var AUTOPREFIXER_BROWSERS = [
    "ie >= 10",
    "ie_mob >= 10",
    "ff >= 30",
    "chrome >= 34",
    "safari >= 7",
    "opera >= 23",
    "ios >= 7",
    "android >= 4.4",
    "bb >= 10"
];

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";
paths.sass = paths.webroot + "sass/**/*.scss";

gulp.task("clean:js", function(cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function(cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:js", function() {
    gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe($.concat(paths.concatJsDest))
        .pipe($.uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function() {
    gulp.src([paths.css, "!" + paths.minCss])
        .pipe($.concat(paths.concatCssDest))
        .pipe($.cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min", ["min:js", "min:css"]);

gulp.task("sass", function() {
    return gulp.src(paths.sass)
        .pipe($.sass().on("error", $.sass.logError))
        .pipe($.autoprefixer({ browsers: AUTOPREFIXER_BROWSERS }))
        .pipe(gulp.dest(paths.webroot + "css"));
});

gulp.task("watch:sass", function() {
    return gulp.watch(paths.sass, ["sass"]);
});

gulp.task("watch", ["watch:sass"]);
