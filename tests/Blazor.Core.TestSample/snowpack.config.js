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
    '**/Properties/**/*',
    '**/wwwroot/**/*',
    '**/bin/**/*',
    '**/obj/**/*',
    '**/*.cs',
    '**/*.csproj',
    '**/*.razor',
    '**/package.json',
    '**/package-lock.json',
    '**/snowpack.config.js',
    '**/tsconfig.json',
  ],
};
