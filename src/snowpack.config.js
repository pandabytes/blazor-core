module.exports = {
  plugins: [
    ['@snowpack/plugin-optimize']
  ],

  buildOptions: {
    out: './wwwroot/js/',
    clean: true
  },
  mount: {
    '.': '/'
  },
  include: [
    '**/*.ts'
  ],
  exclude: [
    '**/node_modules/**/*',
    '**/bin/**/*',
    '**/obj/**/*',
    '**/*.razor',
    '**/*.cs',
    '**/*.csproj',
    '**/package.json',
    '**/package-lock.json',
    '**/snowpack.config.js',
    '**/tsconfig.json',
  ],
};
