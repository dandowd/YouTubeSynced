/// <binding AfterBuild='test' />
var gulp = require('gulp');
var Server = require('karma').Server;

gulp.task('default', ['test']);

gulp.task('test', function (done) {
  new Server({
      configFile: __dirname + '\\ClientApp\\test\\karma.conf.js',
    singleRun: true
  }, done).start();
});