/* eslint-disable */
const fs = require('fs');
const gulp = require('gulp');
const pkg = require('./package.json');

/**
 * Requires async for gulp function
 */
gulp.task('create-version', async () => {
    const data = {
        version: pkg.version,
        timestamp: Date.now(),
    };
    fs.writeFileSync('src/assets/version.json', JSON.stringify(data));
});
