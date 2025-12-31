const fs = require("fs");
const path = require("path");

const distDir = path.resolve(__dirname, "../dist/angular1/browser");
const outFile = path.join(distDir, "manifest.json");

console.log("📁 distDir =", distDir);

if (!fs.existsSync(distDir)) {
  console.error("❌ dist folder not found");
  process.exit(1);
}

const files = fs.readdirSync(distDir);
console.log("📦 files:", files);

const manifest = {};

for (const file of files) {
  if (/^main[-.].*\.js$/.test(file)) manifest.main = file;
  if (/^runtime[-.].*\.js$/.test(file)) manifest.runtime = file;
  if (/^polyfills[-.].*\.js$/.test(file)) manifest.polyfills = file;
  if (/^styles[-.].*\.css$/.test(file)) manifest.styles = file;
}

if (!manifest.main) {
  console.error("❌ main bundle not found");
  process.exit(1);
}

fs.writeFileSync(outFile, JSON.stringify(manifest, null, 2));
console.log("✅ manifest generated:", manifest);
