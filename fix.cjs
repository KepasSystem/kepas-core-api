const fs = require('fs');
let svc = fs.readFileSync('Infrastructure/Services/ServiceAccountService.cs', 'utf8');
let idx = svc.lastIndexOf('public async Task<bool> UpdatePipelineAsync');
let before = svc.substring(0, idx);
let after = svc.substring(idx);
if (!before.trim().endsWith('}')) {
    before = before.trimEnd() + '\n        }\n\n        ';
}
svc = before + after;
// remove duplicate braces at end
svc = svc.replace(/\}\s*\}\s*\}\s*$/, '    }\n}\n');
fs.writeFileSync('Infrastructure/Services/ServiceAccountService.cs', svc, 'utf8');

let ctrl = fs.readFileSync('Controllers/ServiceAccountsController.cs', 'utf8');
let cidx = ctrl.lastIndexOf('[HttpPatch("{id}/pipeline")]');
let cbefore = ctrl.substring(0, cidx);
let cafter = ctrl.substring(cidx);
if (cbefore.trim().endsWith('}')) {
    // remove the last } so it's inside the class
    cbefore = cbefore.replace(/\}\s*$/, '        ');
}
ctrl = cbefore + cafter;
fs.writeFileSync('Controllers/ServiceAccountsController.cs', ctrl, 'utf8');
