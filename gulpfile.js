/// <binding ProjectOpened='sass:watch' />
var gulp = require('gulp');
var gulpSass = require('gulp-sass');

gulp.task('sass', function() {
    return gulp.src('./MovieTime/scss/**/*.scss')
        .pipe(gulpSass({outputStyle: 'compressed'}).on('error', gulpSass.logError))
        .pipe(gulp.dest('./MovieTime/wwwroot/css'));
});

gulp.task('sass:watch', function() {
    gulp.watch('./MovieTime/scss/**/*.scss', gulp.series('sass'));
});
