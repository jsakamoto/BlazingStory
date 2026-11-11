//! Licensed to the .NET Foundation under one or more agreements.
//! The .NET Foundation licenses this file to you under the MIT license.

const e=()=>(async()=>{try{return new WebAssembly.Module(Uint8Array.from(atob("AGFzbQEAAAABBAFgAAADAgEAChABDgACaR9AAQMAAAsACxoL"),e=>e.codePointAt(0))),!0}catch(e){return!1}})(),o=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,5,1,96,0,1,123,3,2,1,0,10,15,1,13,0,65,1,253,15,65,2,253,15,253,128,2,11])),t=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,5,1,96,0,1,123,3,2,1,0,10,10,1,8,0,65,0,253,15,253,98,11])),n=Symbol.for("wasm promise_control");function r(e,o){let t=null;const r=new Promise(function(n,r){t={isDone:!1,promise:null,resolve:o=>{t.isDone||(t.isDone=!0,n(o),e&&e())},reject:e=>{t.isDone||(t.isDone=!0,r(e),o&&o())}}});t.promise=r;const s=r;return s[n]=t,{promise:s,promise_control:t}}function s(e){return e[n]}function i(e){e&&function(e){return void 0!==e[n]}(e)||We(!1,"Promise is not controllable")}const a="__mono_message__",l=["debug","log","trace","warn","info","error"],c="MONO_WASM: ";let d,u,f,m,g,p;function h(e){m=e}function b(e){if(ke.diagnosticTracing){const o="function"==typeof e?e():e;console.debug(c+o)}}function w(e,...o){console.info(c+e,...o)}function y(e,...o){console.info(e,...o)}function v(e,...o){console.warn(c+e,...o)}function _(e,...o){if(o&&o.length>0&&o[0]&&"object"==typeof o[0]){if(o[0].silent)return;if(o[0].toString)return void console.error(c+e,o[0].toString())}console.error(c+e,...o)}function A(e,o,t){return function(...n){try{let r=n[0];if(void 0===r)r="undefined";else if(null===r)r="null";else if("function"==typeof r)r=r.toString();else if("string"!=typeof r)try{r=JSON.stringify(r)}catch(e){r=r.toString()}o(t?JSON.stringify({method:e,payload:r,arguments:n.slice(1)}):[e+r,...n.slice(1)])}catch(e){f.error(`proxyConsole failed: ${e}`)}}}function x(e,o,t){u=o,m=e,f={...o};const n=`${t}/console`.replace("https://","wss://").replace("http://","ws://");d=new WebSocket(n),d.addEventListener("error",E),d.addEventListener("close",j),function(){for(const e of l)u[e]=A(`console.${e}`,R,!0)}()}function T(e){let o=30;const t=()=>{d?0==d.bufferedAmount||0==o?(e&&y(e),function(){for(const e of l)u[e]=A(`console.${e}`,f.log,!1)}(),d.removeEventListener("error",E),d.removeEventListener("close",j),d.close(1e3,e),d=void 0):(o--,globalThis.setTimeout(t,100)):e&&f&&f.log(e)};t()}function R(e){d&&d.readyState===WebSocket.OPEN?d.send(e):f.log(e)}function E(e){f.error(`[${m}] proxy console websocket error: ${e}`,e)}function j(e){f.debug(`[${m}] proxy console websocket closed: ${e}`,e)}function D(){ke.preferredIcuAsset=C(ke.config);let e="invariant"==ke.config.globalizationMode;if(!e)if(ke.preferredIcuAsset)ke.diagnosticTracing&&b("ICU data archive(s) available, disabling invariant mode");else{if("custom"===ke.config.globalizationMode||"all"===ke.config.globalizationMode||"sharded"===ke.config.globalizationMode){const e="invariant globalization mode is inactive and no ICU data archives are available";throw _(`ERROR: ${e}`),new Error(e)}ke.diagnosticTracing&&b("ICU data archive(s) not available, using invariant globalization mode"),e=!0,ke.preferredIcuAsset=null}const o="DOTNET_SYSTEM_GLOBALIZATION_INVARIANT",t=ke.config.environmentVariables;if(void 0===t[o]&&e&&(t[o]="1"),void 0===t.TZ)try{const e=Intl.DateTimeFormat().resolvedOptions().timeZone||null;e&&(t.TZ=e)}catch(e){w("failed to detect timezone, will fallback to UTC")}}function C(e){var o;if((null===(o=e.resources)||void 0===o?void 0:o.icu)&&"invariant"!=e.globalizationMode){const o=e.applicationCulture||(Me?globalThis.navigator&&globalThis.navigator.languages&&globalThis.navigator.languages[0]:Intl.DateTimeFormat().resolvedOptions().locale);e.applicationCulture||(e.applicationCulture=o);const t=e.resources.icu;let n=null;if("custom"===e.globalizationMode){if(t.length>=1)return t[0].name}else o&&"all"!==e.globalizationMode?"sharded"===e.globalizationMode&&(n=function(e){const o=e.split("-")[0];return"en"===o||["fr","fr-FR","it","it-IT","de","de-DE","es","es-ES"].includes(e)?"icudt_EFIGS.dat":["zh","ko","ja"].includes(o)?"icudt_CJK.dat":"icudt_no_CJK.dat"}(o)):n="icudt.dat";if(n)for(let e=0;e<t.length;e++){const o=t[e];if(o.virtualPath===n)return o.name}}return e.globalizationMode="invariant",null}(new Date).valueOf();const M=class{constructor(e){this.url=e}toString(){return this.url}};async function S(e){if(Se&&"function"!=typeof globalThis.atob){const e="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";globalThis.atob=o=>{const t=String(o).replace(/=+$/,"");let n="";for(let o=0,r=0,s=0;s<t.length;s++){const i=e.indexOf(t.charAt(s));-1!==i&&(r=o%4?64*r+i:i,o++%4&&(n+=String.fromCharCode(255&r>>(-2*o&6))))}return n}}if(Ee){const e=await import(/*! webpackIgnore: true */"process"),o=14;if(e.versions.node.split(".")[0]<o)throw new Error(`NodeJS at '${e.execPath}' has too low version '${e.versions.node}', please use at least ${o}.`)}const o=/*! webpackIgnore: true */import.meta.url,t=o.indexOf("?");var n;if(t>0&&(ke.modulesUniqueQuery=o.substring(t)),ke.scriptUrl=o.replace(/\\/g,"/").replace(/[?#].*/,""),ke.scriptDirectory=(n=ke.scriptUrl).slice(0,n.lastIndexOf("/"))+"/",ke.locateFile=e=>"URL"in globalThis&&globalThis.URL!==M?new URL(e,ke.scriptDirectory).toString():I(e)?e:ke.scriptDirectory+e,ke.fetch_like=U,ke.out=console.log,ke.err=console.error,ke.onDownloadResourceProgress=e.onDownloadResourceProgress,Me&&globalThis.navigator){const e=globalThis.navigator,o=e.userAgentData&&e.userAgentData.brands;o&&o.length>0?ke.isChromium=o.some(e=>"Google Chrome"===e.brand||"Microsoft Edge"===e.brand||"Chromium"===e.brand):e.userAgent&&(ke.isChromium=e.userAgent.includes("Chrome"),ke.isFirefox=e.userAgent.includes("Firefox"))}void 0===globalThis.URL&&(globalThis.URL=M)}async function U(e,o){try{const t="function"==typeof globalThis.fetch;if(Ee){const n=e.startsWith("file://");if(!n&&t)return globalThis.fetch(e,o||{credentials:"same-origin"});g||(p=await import(/*! webpackIgnore: true */"url"),g=await import(/*! webpackIgnore: true */"fs")),n&&(e=p.fileURLToPath(e));const r=await g.promises.readFile(e);return{ok:!0,headers:{length:0,get:()=>null},url:e,arrayBuffer:()=>r,json:()=>JSON.parse(r),text:()=>{throw new Error("NotImplementedException")}}}if(t)return globalThis.fetch(e,o||{credentials:"same-origin"});if("function"==typeof read)return{ok:!0,url:e,headers:{length:0,get:()=>null},arrayBuffer:()=>new Uint8Array(read(e,"binary")),json:()=>JSON.parse(read(e,"utf8")),text:()=>read(e,"utf8")}}catch(o){return{ok:!1,url:e,status:500,headers:{length:0,get:()=>null},statusText:"ERR28: "+o,arrayBuffer:()=>{throw o},json:()=>{throw o},text:()=>{throw o}}}throw new Error("No fetch implementation available")}const P=/^[a-zA-Z][a-zA-Z\d+\-.]*?:\/\//,k=/[a-zA-Z]:[\\/]/;function I(e){return Ee||Se?e.startsWith("/")||e.startsWith("\\")||-1!==e.indexOf("///")||k.test(e):P.test(e)}let O,$=0;const L=[],N=[],z=new Map,F={"js-module-runtime":!0,"js-module-dotnet":!0,"js-module-native":!0,"js-module-diagnostics":!0},W={...F,"js-module-library-initializer":!0},V={...F,dotnetwasm:!0,heap:!0,manifest:!0},B={...W,manifest:!0},J={...W,dotnetwasm:!0},H={dotnetwasm:!0,symbols:!0},Q={...W,dotnetwasm:!0,symbols:!0},q={symbols:!0};function G(e){return!("icu"==e.behavior&&e.name!=ke.preferredIcuAsset)}function Z(e,o,t){null!=o||(o=[]),We(1==o.length,`Expect to have one ${t} asset in resources`);const n=o[0];return n.behavior=t,K(n),e.push(n),n}function K(e){V[e.behavior]&&z.set(e.behavior,e)}function X(e){We(V[e],`Unknown single asset behavior ${e}`);const o=z.get(e);if(o&&!o.resolvedUrl)if(o.resolvedUrl=ke.locateFile(o.name),F[o.behavior]){const e=me(o);e?("string"!=typeof e&&We(!1,"loadBootResource response for 'dotnetjs' type should be a URL string"),o.resolvedUrl=e):o.resolvedUrl=le(o.resolvedUrl,o.behavior)}else if("dotnetwasm"!==o.behavior)throw new Error(`Unknown single asset behavior ${e}`);return o}function Y(e){const o=X(e);return We(o,`Single asset for ${e} not found`),o}let ee=!1;async function oe(){if(!ee){ee=!0,ke.diagnosticTracing&&b("mono_download_assets");try{const e=[],o=[],t=(e,o)=>{!Q[e.behavior]&&G(e)&&ke.expected_instantiated_assets_count++,!J[e.behavior]&&G(e)&&(ke.expected_downloaded_assets_count++,o.push(se(e)))};for(const o of L)t(o,e);for(const e of N)t(e,o);ke.allDownloadsQueued.promise_control.resolve(),Promise.all([...e,...o]).then(()=>{ke.allDownloadsFinished.promise_control.resolve()}).catch(e=>{throw ke.err("Error in mono_download_assets: "+e),Xe(1,e),e}),await ke.runtimeModuleLoaded.promise;const n=async e=>{const o=await e;if(H[o.behavior])return"symbols"===o.behavior&&(await Pe.instantiate_symbols_asset(o),ge(o)),void++ke.actual_downloaded_assets_count;if(o.buffer){if(!Q[o.behavior]){o.buffer&&"object"==typeof o.buffer||We(!1,"asset buffer must be array-like or buffer-like or promise of these"),"string"!=typeof o.resolvedUrl&&We(!1,"resolvedUrl must be string");const e=o.resolvedUrl,t=await o.buffer,n=new Uint8Array(t);ge(o),await Pe.beforeOnRuntimeInitialized.promise,await Pe.afterInstantiateWasm.promise,Pe.instantiate_asset(o,e,n)}}else o.isOptional||We(!1,"Expected asset to have the downloaded buffer"),!J[o.behavior]&&G(o)&&ke.expected_downloaded_assets_count--,!Q[o.behavior]&&G(o)&&ke.expected_instantiated_assets_count--},r=[],s=[];for(const o of e)r.push(n(o));for(const e of o)s.push(n(e));Promise.all(r).then(()=>{Ce||Pe.coreAssetsInMemory.promise_control.resolve()}).catch(e=>{throw ke.err("Error in mono_download_assets: "+e),Xe(1,e),e}),Promise.all(s).then(async()=>{Ce||(await Pe.coreAssetsInMemory.promise,Pe.allAssetsInMemory.promise_control.resolve())}).catch(e=>{throw ke.err("Error in mono_download_assets: "+e),Xe(1,e),e})}catch(e){throw ke.err("Error in mono_download_assets: "+e),e}}}let te=!1;function ne(){if(te)return;te=!0;const e=ke.config,o=[];if(e.assets)for(const o of e.assets)"object"!=typeof o&&We(!1,`asset must be object, it was ${typeof o} : ${o}`),"string"!=typeof o.behavior&&We(!1,"asset behavior must be known string"),"string"!=typeof o.name&&We(!1,"asset name must be string"),o.resolvedUrl&&"string"!=typeof o.resolvedUrl&&We(!1,"asset resolvedUrl could be string"),o.hash&&"string"!=typeof o.hash&&We(!1,"asset resolvedUrl could be string"),o.pendingDownload&&"object"!=typeof o.pendingDownload&&We(!1,"asset pendingDownload could be object"),o.isCore?L.push(o):N.push(o),K(o);else if(e.resources){const t=e.resources;t.wasmNative||We(!1,"resources.wasmNative must be defined"),t.jsModuleNative||We(!1,"resources.jsModuleNative must be defined"),t.jsModuleRuntime||We(!1,"resources.jsModuleRuntime must be defined"),Z(N,t.wasmNative,"dotnetwasm"),Z(o,t.jsModuleNative,"js-module-native"),Z(o,t.jsModuleRuntime,"js-module-runtime"),t.jsModuleDiagnostics&&Z(o,t.jsModuleDiagnostics,"js-module-diagnostics");const n=(e,o,t)=>{const n=e;n.behavior=o,t?(n.isCore=!0,L.push(n)):N.push(n)};if(t.coreAssembly)for(let e=0;e<t.coreAssembly.length;e++)n(t.coreAssembly[e],"assembly",!0);if(t.assembly)for(let e=0;e<t.assembly.length;e++)n(t.assembly[e],"assembly",!t.coreAssembly);if(0!=e.debugLevel&&ke.isDebuggingSupported()){if(t.corePdb)for(let e=0;e<t.corePdb.length;e++)n(t.corePdb[e],"pdb",!0);if(t.pdb)for(let e=0;e<t.pdb.length;e++)n(t.pdb[e],"pdb",!t.corePdb)}if(e.loadAllSatelliteResources&&t.satelliteResources)for(const e in t.satelliteResources)for(let o=0;o<t.satelliteResources[e].length;o++){const r=t.satelliteResources[e][o];r.culture=e,n(r,"resource",!t.coreAssembly)}if(t.coreVfs)for(let e=0;e<t.coreVfs.length;e++)n(t.coreVfs[e],"vfs",!0);if(t.vfs)for(let e=0;e<t.vfs.length;e++)n(t.vfs[e],"vfs",!t.coreVfs);const r=C(e);if(r&&t.icu)for(let e=0;e<t.icu.length;e++){const o=t.icu[e];o.name===r&&n(o,"icu",!1)}if(t.wasmSymbols)for(let e=0;e<t.wasmSymbols.length;e++)n(t.wasmSymbols[e],"symbols",!1)}if(e.appsettings)for(let o=0;o<e.appsettings.length;o++){const t=e.appsettings[o],n=pe(t);"appsettings.json"!==n&&n!==`appsettings.${e.applicationEnvironment}.json`||N.push({name:t,behavior:"vfs",cache:"no-cache",useCredentials:!0})}e.assets=[...L,...N,...o]}async function re(e){const o=await se(e);return await o.pendingDownloadInternal.response,o.buffer}async function se(e){try{return await ie(e)}catch(o){if(!ke.enableDownloadRetry)throw o;if(Se||Ee)throw o;if(e.pendingDownload&&e.pendingDownloadInternal==e.pendingDownload)throw o;if(e.resolvedUrl&&-1!=e.resolvedUrl.indexOf("file://"))throw o;if(o&&404==o.status)throw o;e.pendingDownloadInternal=void 0,await ke.allDownloadsQueued.promise;try{return ke.diagnosticTracing&&b(`Retrying download '${e.name}'`),await ie(e)}catch(o){return e.pendingDownloadInternal=void 0,await new Promise(e=>globalThis.setTimeout(e,100)),ke.diagnosticTracing&&b(`Retrying download (2) '${e.name}' after delay`),await ie(e)}}}async function ie(e){for(;O;)await O.promise;try{++$,$==ke.maxParallelDownloads&&(ke.diagnosticTracing&&b("Throttling further parallel downloads"),O=r());const o=await async function(e){if(e.pendingDownload&&(e.pendingDownloadInternal=e.pendingDownload),e.pendingDownloadInternal&&e.pendingDownloadInternal.response)return e.pendingDownloadInternal.response;if(e.buffer){const o=await e.buffer;return e.resolvedUrl||(e.resolvedUrl="undefined://"+e.name),e.pendingDownloadInternal={url:e.resolvedUrl,name:e.name,response:Promise.resolve({ok:!0,arrayBuffer:()=>o,json:()=>JSON.parse(new TextDecoder("utf-8").decode(o)),text:()=>new TextDecoder("utf-8").decode(o),headers:{get:()=>{}}})},e.pendingDownloadInternal.response}const o=e.loadRemote&&ke.config.remoteSources?ke.config.remoteSources:[""];let t;for(let n of o){n=n.trim(),"./"===n&&(n="");const o=ae(e,n);e.name===o?ke.diagnosticTracing&&b(`Attempting to download '${o}'`):ke.diagnosticTracing&&b(`Attempting to download '${o}' for ${e.name}`);try{e.resolvedUrl=o;const n=ue(e);if(e.pendingDownloadInternal=n,t=await n.response,!t||!t.ok)continue;return t}catch(e){t||(t={ok:!1,url:o,status:0,statusText:""+e});continue}}const n=e.isOptional||e.name.match(/\.pdb$/)&&ke.config.ignorePdbLoadErrors;if(t||We(!1,`Response undefined ${e.name}`),!n){const o=new Error(`download '${t.url}' for ${e.name} failed ${t.status} ${t.statusText}`);throw o.status=t.status,o}w(`optional download '${t.url}' for ${e.name} failed ${t.status} ${t.statusText}`)}(e);return o?(H[e.behavior]||(e.buffer=await o.arrayBuffer(),++ke.actual_downloaded_assets_count),e):e}finally{if(--$,O&&$==ke.maxParallelDownloads-1){ke.diagnosticTracing&&b("Resuming more parallel downloads");const e=O;O=void 0,e.promise_control.resolve()}}}function ae(e,o){let t;return null==o&&We(!1,`sourcePrefix must be provided for ${e.name}`),e.resolvedUrl?t=e.resolvedUrl:(t=""===o?"assembly"===e.behavior||"pdb"===e.behavior?e.name:"resource"===e.behavior&&e.culture&&""!==e.culture?`${e.culture}/${e.name}`:e.name:o+e.name,t=le(ke.locateFile(t),e.behavior)),t&&"string"==typeof t||We(!1,"attemptUrl need to be path or url string"),t}function le(e,o){return ke.modulesUniqueQuery&&B[o]&&(e+=ke.modulesUniqueQuery),e}let ce=0;const de=new Set;function ue(e){try{e.resolvedUrl||We(!1,"Request's resolvedUrl must be set");const o=function(e){let o=e.resolvedUrl;if(ke.loadBootResource){const t=me(e);if(t instanceof Promise)return t;"string"==typeof t&&(o=t)}const t={};return e.cache?t.cache=e.cache:ke.config.disableNoCacheFetch||(t.cache="no-cache"),e.useCredentials?t.credentials="include":!ke.config.disableIntegrityCheck&&e.hash&&(t.integrity=e.hash),ke.fetch_like(o,t)}(e),t={name:e.name,url:e.resolvedUrl,response:o};return de.add(e.name),t.response.then(()=>{"assembly"==e.behavior&&ke.loadedAssemblies.push(e.name),ce++,ke.onDownloadResourceProgress&&ke.onDownloadResourceProgress(ce,de.size)}),t}catch(o){const t={ok:!1,url:e.resolvedUrl,status:500,statusText:"ERR29: "+o,arrayBuffer:()=>{throw o},json:()=>{throw o}};return{name:e.name,url:e.resolvedUrl,response:Promise.resolve(t)}}}const fe={resource:"assembly",assembly:"assembly",pdb:"pdb",icu:"globalization",vfs:"configuration",manifest:"manifest",dotnetwasm:"dotnetwasm","js-module-dotnet":"dotnetjs","js-module-native":"dotnetjs","js-module-runtime":"dotnetjs"};function me(e){var o;if(ke.loadBootResource){const t=null!==(o=e.hash)&&void 0!==o?o:"",n=e.resolvedUrl,r=fe[e.behavior];if(r){const o=ke.loadBootResource(r,e.name,n,t,e.behavior);return"string"==typeof o?function(e){return"string"!=typeof e&&We(!1,"url must be a string"),!I(e)&&0!==e.indexOf("./")&&0!==e.indexOf("../")&&globalThis.URL&&globalThis.document&&globalThis.document.baseURI&&(e=new URL(e,globalThis.document.baseURI).toString()),e}(o):o}}}function ge(e){e.pendingDownloadInternal=null,e.pendingDownload=null,e.buffer=null,e.moduleExports=null}function pe(e){let o=e.lastIndexOf("/");return o>=0&&o++,e.substring(o)}async function he(e){e&&await Promise.all((null!=e?e:[]).map(e=>async function(e){try{const o=e.name;if(!e.moduleExports){const t=le(ke.locateFile(o),"js-module-library-initializer");ke.diagnosticTracing&&b(`Attempting to import '${t}' for ${e}`),e.moduleExports=await import(/*! webpackIgnore: true */t)}ke.libraryInitializers.push({scriptName:o,exports:e.moduleExports})}catch(o){v(`Failed to import library initializer '${e}': ${o}`)}}(e)))}async function be(e,o){if(!ke.libraryInitializers)return;const t=[];for(let n=0;n<ke.libraryInitializers.length;n++){const r=ke.libraryInitializers[n];r.exports[e]&&t.push(we(r.scriptName,e,()=>r.exports[e](...o)))}await Promise.all(t)}async function we(e,o,t){try{await t()}catch(t){throw v(`Failed to invoke '${o}' on library initializer '${e}': ${t}`),Xe(1,t),t}}function ye(e,o){if(e===o)return e;const t={...o};return void 0!==t.assets&&t.assets!==e.assets&&(t.assets=[...e.assets||[],...t.assets||[]]),void 0!==t.resources&&(t.resources=_e(e.resources||{assembly:[],jsModuleNative:[],jsModuleRuntime:[],wasmNative:[]},t.resources)),void 0!==t.environmentVariables&&(t.environmentVariables={...e.environmentVariables||{},...t.environmentVariables||{}}),void 0!==t.runtimeOptions&&t.runtimeOptions!==e.runtimeOptions&&(t.runtimeOptions=[...e.runtimeOptions||[],...t.runtimeOptions||[]]),Object.assign(e,t)}function ve(e,o){if(e===o)return e;const t={...o};return t.config&&(e.config||(e.config={}),t.config=ye(e.config,t.config)),Object.assign(e,t)}function _e(e,o){if(e===o)return e;const t={...o};return void 0!==t.coreAssembly&&(t.coreAssembly=[...e.coreAssembly||[],...t.coreAssembly||[]]),void 0!==t.assembly&&(t.assembly=[...e.assembly||[],...t.assembly||[]]),void 0!==t.lazyAssembly&&(t.lazyAssembly=[...e.lazyAssembly||[],...t.lazyAssembly||[]]),void 0!==t.corePdb&&(t.corePdb=[...e.corePdb||[],...t.corePdb||[]]),void 0!==t.pdb&&(t.pdb=[...e.pdb||[],...t.pdb||[]]),void 0!==t.jsModuleNative&&(t.jsModuleNative=[...e.jsModuleNative||[],...t.jsModuleNative||[]]),void 0!==t.jsModuleDiagnostics&&(t.jsModuleDiagnostics=[...e.jsModuleDiagnostics||[],...t.jsModuleDiagnostics||[]]),void 0!==t.jsModuleRuntime&&(t.jsModuleRuntime=[...e.jsModuleRuntime||[],...t.jsModuleRuntime||[]]),void 0!==t.wasmSymbols&&(t.wasmSymbols=[...e.wasmSymbols||[],...t.wasmSymbols||[]]),void 0!==t.wasmNative&&(t.wasmNative=[...e.wasmNative||[],...t.wasmNative||[]]),void 0!==t.icu&&(t.icu=[...e.icu||[],...t.icu||[]]),void 0!==t.satelliteResources&&(t.satelliteResources=function(e,o){if(e===o)return e;for(const t in o)e[t]=[...e[t]||[],...o[t]||[]];return e}(e.satelliteResources||{},t.satelliteResources||{})),void 0!==t.modulesAfterConfigLoaded&&(t.modulesAfterConfigLoaded=[...e.modulesAfterConfigLoaded||[],...t.modulesAfterConfigLoaded||[]]),void 0!==t.modulesAfterRuntimeReady&&(t.modulesAfterRuntimeReady=[...e.modulesAfterRuntimeReady||[],...t.modulesAfterRuntimeReady||[]]),void 0!==t.extensions&&(t.extensions={...e.extensions||{},...t.extensions||{}}),void 0!==t.vfs&&(t.vfs=[...e.vfs||[],...t.vfs||[]]),Object.assign(e,t)}function Ae(){const e=ke.config;if(e.environmentVariables=e.environmentVariables||{},e.runtimeOptions=e.runtimeOptions||[],e.resources=e.resources||{assembly:[],jsModuleNative:[],jsModuleRuntime:[],wasmNative:[],vfs:[],satelliteResources:{}},e.assets){ke.diagnosticTracing&&b("config.assets is deprecated, use config.resources instead");for(const o of e.assets){const t={};switch(o.behavior){case"assembly":t.assembly=[o];break;case"pdb":t.pdb=[o];break;case"resource":t.satelliteResources={},t.satelliteResources[o.culture]=[o];break;case"icu":t.icu=[o];break;case"symbols":t.wasmSymbols=[o];break;case"vfs":t.vfs=[o];break;case"dotnetwasm":t.wasmNative=[o];break;case"js-module-runtime":t.jsModuleRuntime=[o];break;case"js-module-native":t.jsModuleNative=[o];break;case"js-module-diagnostics":t.jsModuleDiagnostics=[o];break;case"js-module-dotnet":break;default:throw new Error(`Unexpected behavior ${o.behavior} of asset ${o.name}`)}_e(e.resources,t)}}e.debugLevel,void 0===e.virtualWorkingDirectory&&(e.virtualWorkingDirectory=Ue),e.applicationEnvironment||(e.applicationEnvironment="Production"),e.applicationCulture&&(e.environmentVariables.LANG=`${e.applicationCulture}.UTF-8`),Pe.diagnosticTracing=ke.diagnosticTracing=!!e.diagnosticTracing,Pe.waitForDebugger=e.waitForDebugger,ke.maxParallelDownloads=e.maxParallelDownloads||ke.maxParallelDownloads,ke.enableDownloadRetry=void 0!==e.enableDownloadRetry?e.enableDownloadRetry:ke.enableDownloadRetry}let xe=!1;async function Te(e){var o;if(xe)await ke.afterConfigLoaded.promise;else try{if(xe=!0,Ae(),await he(null===(o=ke.config.resources)||void 0===o?void 0:o.modulesAfterConfigLoaded),await be("onRuntimeConfigLoaded",[ke.config]),e.onConfigLoaded)try{await e.onConfigLoaded(ke.config,Oe),Ae()}catch(e){throw _("onConfigLoaded() failed",e),e}Ae(),ke.afterConfigLoaded.promise_control.resolve(ke.config)}catch(o){const t=`Failed to initialize config ${o} ${null==o?void 0:o.stack}`;throw ke.config=e.config=Object.assign(ke.config,{message:t,error:o,isError:!0}),Xe(1,new Error(t)),o}}function Re(){return!!globalThis.navigator&&(ke.isChromium||ke.isFirefox)}"function"==typeof importScripts&&(globalThis.dotnetSidecar=!0);const Ee="object"==typeof process&&"object"==typeof process.versions&&"string"==typeof process.versions.node,je="function"==typeof importScripts,De=je&&"undefined"!=typeof dotnetSidecar,Ce=je&&!De,Me="object"==typeof window||je&&!Ee,Se=!Me&&!Ee,Ue="/";let Pe={},ke={},Ie={},Oe={},$e={},Le=!1;const Ne={},ze={config:Ne},Fe={mono:{},binding:{},internal:$e,module:ze,loaderHelpers:ke,runtimeHelpers:Pe,diagnosticHelpers:Ie,api:Oe};function We(e,o){if(e)return;const t="Assert failed: "+("function"==typeof o?o():o),n=new Error(t);_(t,n),Pe.nativeAbort(n)}function Ve(){return void 0!==ke.exitCode}function Be(){return Pe.runtimeReady&&!Ve()}function Je(){Ve()&&We(!1,`.NET runtime already exited with ${ke.exitCode} ${ke.exitReason}. You can use dotnet.runMain() which doesn't exit the runtime.`),Pe.runtimeReady||We(!1,".NET runtime didn't start yet. Please call dotnet.create() first.")}function He(){Me&&(globalThis.addEventListener("unhandledrejection",eo),globalThis.addEventListener("error",oo))}let Qe,qe;function Ge(){Qe=ze.onAbort,qe=ze.onExit,ze.onAbort=Ke,ze.onExit=Ze}function Ze(e){qe&&qe(e),Xe(e,ke.exitReason)}function Ke(e){Qe&&Qe(e||ke.exitReason),Xe(1,e||ke.exitReason)}function Xe(e,o){var t;const n=o&&"object"==typeof o;e=n&&"number"==typeof o.status?o.status:void 0===e?-1:e;const r=n&&"string"==typeof o.message?o.message:""+o;(o=n?o:Pe.ExitStatus?function(e,o){const t=new Pe.ExitStatus(e);return t.message=o,t.toString=()=>o,t}(e,r):new Error("Exit with code "+e+" "+r)).status=e,o.message||(o.message=r);const s=""+(o.stack||(new Error).stack);try{Object.defineProperty(o,"stack",{get:()=>s})}catch(e){}const i=!!o.silent;if(o.silent=!0,Ve())ke.diagnosticTracing&&b("mono_exit called after exit");else{try{ze.onAbort==Ke&&(ze.onAbort=Qe),ze.onExit==Ze&&(ze.onExit=qe),Me&&(globalThis.removeEventListener("unhandledrejection",eo),globalThis.removeEventListener("error",oo)),Pe.runtimeReady?(Pe.jiterpreter_dump_stats&&Pe.jiterpreter_dump_stats(!1),0===e&&(null===(t=ke.config)||void 0===t?void 0:t.interopCleanupOnExit)&&Pe.forceDisposeProxies(!0,!0)):(ke.diagnosticTracing&&b(`abort_startup, reason: ${o}`),function(e){ke.allDownloadsQueued.promise_control.reject(e),ke.allDownloadsFinished.promise_control.reject(e),ke.afterConfigLoaded.promise_control.reject(e),ke.wasmCompilePromise.promise_control.reject(e),ke.runtimeModuleLoaded.promise_control.reject(e),Pe.dotnetReady&&(Pe.dotnetReady.promise_control.reject(e),Pe.afterInstantiateWasm.promise_control.reject(e),Pe.afterPreRun.promise_control.reject(e),Pe.beforeOnRuntimeInitialized.promise_control.reject(e),Pe.afterOnRuntimeInitialized.promise_control.reject(e),Pe.afterPostRun.promise_control.reject(e))}(o))}catch(e){v("mono_exit A failed",e)}try{i||(function(e,o){if(0!==e&&o){const e=Pe.ExitStatus&&o instanceof Pe.ExitStatus?b:_;"string"==typeof o?e(o):(void 0===o.stack&&(o.stack=(new Error).stack+""),o.message?e(Pe.stringify_as_error_with_stack?Pe.stringify_as_error_with_stack(o.message+"\n"+o.stack):o.message+"\n"+o.stack):e(JSON.stringify(o)))}!Ce&&ke.config&&(ke.config.logExitCode?ke.config.forwardConsole?T("WASM EXIT "+e):y("WASM EXIT "+e):ke.config.forwardConsole&&T())}(e,o),function(e){if(Me&&!Ce&&ke.config&&ke.config.appendElementOnExit&&document){const o=document.createElement("label");o.id="tests_done",0!==e&&(o.style.background="red"),o.innerHTML=""+e,document.body.appendChild(o)}}(e))}catch(e){v("mono_exit B failed",e)}ke.exitCode=e,ke.exitReason||(ke.exitReason=o),!Ce&&Pe.runtimeReady&&ze.runtimeKeepalivePop()}if(ke.config&&ke.config.asyncFlushOnExit&&0===e)throw(async()=>{try{await async function(){if(Ee)try{const e=await import(/*! webpackIgnore: true */"process"),o=e=>new Promise((o,t)=>{e.on("error",t),e.end("","utf8",o)}),t=o(e.stderr),n=o(e.stdout);let r;const s=new Promise(e=>{r=setTimeout(()=>e("timeout"),1e3)});await Promise.race([Promise.all([n,t]),s]),clearTimeout(r)}catch(e){_(`flushing std* streams failed: ${e}`)}}()}finally{Ye(e,o)}})(),o;Ye(e,o)}function Ye(e,o){if(Pe.runtimeReady&&Pe.nativeExit)try{Pe.nativeExit(e)}catch(e){!Pe.ExitStatus||e instanceof Pe.ExitStatus||v("set_exit_code_and_quit_now failed: "+e.toString())}if(0!==e||!Me)throw Ee?process.exit(e):Pe.quit&&Pe.quit(e,o),o}function eo(e){to(e,e.reason,"rejection")}function oo(e){to(e,e.error,"error")}function to(e,o,t){e.preventDefault();try{o||(o=new Error("Unhandled "+t)),void 0===o.stack&&(o.stack=(new Error).stack),o.stack=o.stack+"",o.silent||(_("Unhandled error:",o),Xe(1,o))}catch(e){}}!function(n){if(Le)throw new Error("Loader module already loaded");Le=!0,Pe=n.runtimeHelpers,ke=n.loaderHelpers,Ie=n.diagnosticHelpers,Oe=n.api,$e=n.internal,Object.assign(Oe,{INTERNAL:$e,invokeLibraryInitializers:be}),Object.assign(n.module,{config:ye(Ne,{environmentVariables:{}})});const a={mono_wasm_bindings_is_ready:!1,config:n.module.config,diagnosticTracing:!1,nativeAbort:e=>{throw e||new Error("abort")},nativeExit:e=>{throw new Error("exit:"+e)}},l={gitHash:"297acf930fb0f5c07fd4e51d78e521b4ad2e5d1f",config:n.module.config,diagnosticTracing:!1,maxParallelDownloads:16,enableDownloadRetry:!0,_loaded_files:[],loadedFiles:[],loadedAssemblies:[],libraryInitializers:[],workerNextNumber:1,actual_downloaded_assets_count:0,actual_instantiated_assets_count:0,expected_downloaded_assets_count:0,expected_instantiated_assets_count:0,afterConfigLoaded:r(),allDownloadsQueued:r(),allDownloadsFinished:r(),wasmCompilePromise:r(),runtimeModuleLoaded:r(),loadingWorkers:r(),is_exited:Ve,is_runtime_running:Be,assert_runtime_running:Je,mono_exit:Xe,createPromiseController:r,getPromiseController:s,assertIsControllablePromise:i,mono_download_assets:oe,resolve_single_asset_path:Y,setup_proxy_console:x,set_thread_prefix:h,installUnhandledErrorHandler:He,retrieve_asset_download:re,invokeLibraryInitializers:be,isDebuggingSupported:Re,exceptionsFinal:e,simd:t,relaxedSimd:o};Object.assign(Pe,a),Object.assign(ke,l)}(Fe);let no,ro,so,io=!1,ao=!1;async function lo(e){if(!ao){if(ao=!0,Me&&ke.config.forwardConsole&&void 0!==globalThis.WebSocket&&x("main",globalThis.console,globalThis.location.origin),ze||We(!1,"Null moduleConfig"),ke.config||We(!1,"Null moduleConfig.config"),"function"==typeof e){const o=e(Fe.api);if(o.ready)throw new Error("Module.ready couldn't be redefined.");Object.assign(ze,o),ve(ze,o)}else{if("object"!=typeof e)throw new Error("Can't use moduleFactory callback of createDotnetRuntime function.");ve(ze,e)}await S(ze)}}async function co(e){return await lo(e),ke.config.exitOnUnhandledError&&He(),Ge(),async function(){var e;await Te(ze),ne();const o=uo();(async function(){try{const e=Y("dotnetwasm");await se(e),e&&e.pendingDownloadInternal&&e.pendingDownloadInternal.response||We(!1,"Can't load dotnet.native.wasm");const o=await e.pendingDownloadInternal.response,t=o.headers&&o.headers.get?o.headers.get("Content-Type"):void 0;let n;if("function"==typeof WebAssembly.compileStreaming&&"application/wasm"===t)n=await WebAssembly.compileStreaming(o);else{Me&&"application/wasm"!==t&&v('WebAssembly resource does not have the expected content type "application/wasm", so falling back to slower ArrayBuffer instantiation.');const e=await o.arrayBuffer();ke.diagnosticTracing&&b("instantiate_wasm_module buffered"),n=Se?await Promise.resolve(new WebAssembly.Module(e)):await WebAssembly.compile(e)}e.pendingDownloadInternal=null,e.pendingDownload=null,e.buffer=null,e.moduleExports=null,ke.wasmCompilePromise.promise_control.resolve(n)}catch(e){ke.wasmCompilePromise.promise_control.reject(e)}})(),setTimeout(async()=>{try{D(),await oe()}catch(e){Xe(1,e)}},0);const t=await Promise.all(o);return await fo(t),await Pe.dotnetReady.promise,await he(null===(e=ke.config.resources)||void 0===e?void 0:e.modulesAfterRuntimeReady),await be("onRuntimeReady",[Fe.api]),Oe}()}function uo(){const e=Y("js-module-runtime"),o=Y("js-module-native");if(no&&ro)return[no,ro,so];"object"==typeof e.moduleExports?no=e.moduleExports:(ke.diagnosticTracing&&b(`Attempting to import '${e.resolvedUrl}' for ${e.name}`),no=import(/*! webpackIgnore: true */e.resolvedUrl)),"object"==typeof o.moduleExports?ro=o.moduleExports:(ke.diagnosticTracing&&b(`Attempting to import '${o.resolvedUrl}' for ${o.name}`),ro=import(/*! webpackIgnore: true */o.resolvedUrl));const t=X("js-module-diagnostics");return t&&("object"==typeof t.moduleExports?so=t.moduleExports:(ke.diagnosticTracing&&b(`Attempting to import '${t.resolvedUrl}' for ${t.name}`),so=import(/*! webpackIgnore: true */t.resolvedUrl))),[no,ro,so]}async function fo(e){const{initializeExports:o,initializeReplacements:t,configureRuntimeStartup:n,configureEmscriptenStartup:r,configureWorkerStartup:s,setRuntimeGlobals:i,passEmscriptenInternals:a}=e[0],{default:l}=e[1],c=e[2];i(Fe),o(Fe),c&&c.setRuntimeGlobals(Fe),await n(ze),ke.runtimeModuleLoaded.promise_control.resolve(),l(()=>(Object.assign(ze,{__dotnet_runtime:{initializeReplacements:t,configureEmscriptenStartup:r,configureWorkerStartup:s,passEmscriptenInternals:a}}),ze)).catch(e=>{if(e.message&&e.message.toLowerCase().includes("out of memory"))throw new Error(".NET runtime has failed to start, because too much memory was requested. Please decrease the memory by adjusting EmccMaximumHeapSize.");throw e})}Ce&&async function(){(function(){const e=new MessageChannel,o=e.port1,t=e.port2;o.addEventListener("message",e=>{!function(e){const o=JSON.parse(e.config),t=JSON.parse(e.monoThreadInfo);ze.config=o,ze.wasmModule=e.wasmModule,ze.wasmMemory=e.wasmMemory,ze.handlers=e.handlers,io?ke.diagnosticTracing&&b("mono config already received"):(ye(ke.config,o),Pe.monoThreadInfo=t,Ae(),ke.diagnosticTracing&&b("mono config received"),io=!0,ke.afterConfigLoaded.promise_control.resolve(ke.config),Me&&o.forwardConsole&&void 0!==globalThis.WebSocket&&ke.setup_proxy_console("worker-idle",console,globalThis.location.origin))}(e.data),o.close(),t.close()},{once:!0}),o.start(),self.postMessage({[a]:{monoCmd:"preload",port:t}},[t])})(),await ke.afterConfigLoaded.promise,function(){const e=ke.config;e.assets||We(!1,"config.assets must be defined");for(const o of e.assets)K(o),q[o.behavior]&&N.push(o)}();const e=uo(),o=await Promise.all(e);return globalThis.name="em-pthread",await fo(o),ke.config.exitOnUnhandledError&&He(),Ge(),Me&&ke.config.forwardConsole&&void 0!==globalThis.WebSocket&&x("main",globalThis.console,globalThis.location.origin),await S(ze),await oe(),self.dispatchEvent(new MessageEvent("message",{data:{cmd:1,handlers:ze.handlers,wasmMemory:ze.wasmMemory,wasmModule:ze.wasmModule}})),ze}().catch(e=>Xe(1,e));const mo=new class{withModuleConfig(e){try{return ve(ze,e),this}catch(e){throw Xe(1,e),e}}withInterpreterPgo(e,o){try{return ye(Ne,{interpreterPgo:e,interpreterPgoSaveDelay:o}),Ne.runtimeOptions?Ne.runtimeOptions.push("--interp-pgo-recording"):Ne.runtimeOptions=["--interp-pgo-recording"],this}catch(e){throw Xe(1,e),e}}withConfig(e){try{return ye(Ne,e),this}catch(e){throw Xe(1,e),e}}withConfigSrc(e){return this}withVirtualWorkingDirectory(e){try{return e&&"string"==typeof e||We(!1,"must be directory path"),ye(Ne,{virtualWorkingDirectory:e}),this}catch(e){throw Xe(1,e),e}}withEnvironmentVariable(e,o){try{const t={};return t[e]=o,ye(Ne,{environmentVariables:t}),this}catch(e){throw Xe(1,e),e}}withEnvironmentVariables(e){try{return e&&"object"==typeof e||We(!1,"must be dictionary object"),ye(Ne,{environmentVariables:e}),this}catch(e){throw Xe(1,e),e}}withDiagnosticTracing(e){try{return"boolean"!=typeof e&&We(!1,"must be boolean"),ye(Ne,{diagnosticTracing:e}),this}catch(e){throw Xe(1,e),e}}withDebugging(e){try{return null!=e&&"number"==typeof e||We(!1,"must be number"),ye(Ne,{debugLevel:e}),this}catch(e){throw Xe(1,e),e}}withApplicationArguments(...e){try{return e&&Array.isArray(e)||We(!1,"must be array of strings"),ye(Ne,{applicationArguments:e}),this}catch(e){throw Xe(1,e),e}}withRuntimeOptions(e){try{return e&&Array.isArray(e)||We(!1,"must be array of strings"),Ne.runtimeOptions?Ne.runtimeOptions.push(...e):Ne.runtimeOptions=e,this}catch(e){throw Xe(1,e),e}}withMainAssembly(e){try{return ye(Ne,{mainAssemblyName:e}),this}catch(e){throw Xe(1,e),e}}withApplicationArgumentsFromQuery(){try{if(!globalThis.window)throw new Error("Missing window to the query parameters from");if(void 0===globalThis.URLSearchParams)throw new Error("URLSearchParams is supported");const e=new URLSearchParams(globalThis.window.location.search).getAll("arg");return this.withApplicationArguments(...e)}catch(e){throw Xe(1,e),e}}withApplicationEnvironment(e){try{return ye(Ne,{applicationEnvironment:e}),this}catch(e){throw Xe(1,e),e}}withApplicationCulture(e){try{return ye(Ne,{applicationCulture:e}),this}catch(e){throw Xe(1,e),e}}withResourceLoader(e){try{return ke.loadBootResource=e,this}catch(e){throw Xe(1,e),e}}async download(){try{await async function(){lo(ze),await Te(ze),ne(),D(),oe(),await ke.allDownloadsFinished.promise}()}catch(e){throw Xe(1,e),e}}async create(){try{return this.instance||(this.instance=await async function(){return await co(ze),Fe.api}()),this.instance}catch(e){throw Xe(1,e),e}}run(){return this.runMainAndExit()}async runMainAndExit(){try{return ze.config||We(!1,"Null moduleConfig.config"),this.instance||await this.create(),this.instance.runMainAndExit()}catch(e){throw Xe(1,e),e}}async runMain(){try{return ze.config||We(!1,"Null moduleConfig.config"),this.instance||await this.create(),this.instance.runMain()}catch(e){throw Xe(1,e),e}}},go=Xe,po=co;Se||"function"==typeof globalThis.URL||We(!1,"This browser/engine doesn't support URL API. Please use a modern version."),"function"!=typeof globalThis.BigInt64Array&&We(!1,"This browser/engine doesn't support BigInt64Array API. Please use a modern version. See also https://learn.microsoft.com/aspnet/core/blazor/supported-platforms"),globalThis.performance&&"function"==typeof globalThis.performance.now||We(!1,"This browser/engine doesn't support performance.now. Please use a modern version."),Se||globalThis.crypto&&"object"==typeof globalThis.crypto.subtle||We(!1,"This engine doesn't support crypto.subtle. Please use a modern version."),Se||globalThis.crypto&&"function"==typeof globalThis.crypto.getRandomValues||We(!1,"This engine doesn't support crypto.getRandomValues. Please use a modern version."),Ee&&"function"!=typeof process.exit&&We(!1,"This engine doesn't support process.exit. Please use a modern version."),mo.withConfig(/*json-start*/{
  "mainAssemblyName": "MyBlazorWasmApp1.Stories",
  "resources": {
    "hash": "sha256-ZK9SbXXmzkLpxfSC/iFw28nFAEtOF5s2A50nuGIsyBo=",
    "jsModuleNative": [
      {
        "name": "dotnet.native.5se7c1ucb3.js"
      }
    ],
    "jsModuleRuntime": [
      {
        "name": "dotnet.runtime.uyfhwbfd13.js"
      }
    ],
    "wasmNative": [
      {
        "name": "dotnet.native.xqxtqcfth9.wasm",
        "hash": "sha256-53WsLqdPvxurVbIdHMUVxRDH5cIALI0PH/eEB69til0=",
        "cache": "force-cache"
      }
    ],
    "coreAssembly": [
      {
        "payloadSize": 80464,
        "virtualPath": "System.Runtime.InteropServices.JavaScript.wasm",
        "name": "System.Runtime.InteropServices.JavaScript.ppvmrbz86k.wasm",
        "hash": "sha256-837/pYCcCyG211ihuiLLu5r3188vAADAi0yd5DykD5E=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5412432,
        "virtualPath": "System.Private.CoreLib.wasm",
        "name": "System.Private.CoreLib.sz4ghvrgd1.wasm",
        "hash": "sha256-SLqkvMoC8pMigajMWVibrzztLIhs4JU+/Eq8eQ2bcNg=",
        "cache": "force-cache"
      }
    ],
    "assembly": [
      {
        "payloadSize": 47184,
        "virtualPath": "Microsoft.AspNetCore.Authorization.wasm",
        "name": "Microsoft.AspNetCore.Authorization.cib096z7rw.wasm",
        "hash": "sha256-vFTL6yQJ6iuydXJkdg8j6zSRroQ5QStlsEMvbACF924=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 428112,
        "virtualPath": "Microsoft.AspNetCore.Components.wasm",
        "name": "Microsoft.AspNetCore.Components.72mifddv5o.wasm",
        "hash": "sha256-0WwVpiweYrHdpPIrqy1k6yx2UtSNmawpDb9Nkv1XnYQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 51280,
        "virtualPath": "Microsoft.AspNetCore.Components.Forms.wasm",
        "name": "Microsoft.AspNetCore.Components.Forms.h06an5551r.wasm",
        "hash": "sha256-8Xe51dfXkmgreI/FWV+bndfV/OMgYbsXWnhUOgTnErk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 217680,
        "virtualPath": "Microsoft.AspNetCore.Components.Web.wasm",
        "name": "Microsoft.AspNetCore.Components.Web.3t3dmrnfil.wasm",
        "hash": "sha256-h6ZSSqEndfV2YXcja4iQEuZ2rLKD0ZAythkM8HIFdIQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 190032,
        "virtualPath": "Microsoft.AspNetCore.Components.WebAssembly.wasm",
        "name": "Microsoft.AspNetCore.Components.WebAssembly.wcqyr45rut.wasm",
        "hash": "sha256-EKsq+iS6YsSWpMgveJWFtxeVCnIl6aHnA6eR9afOC6E=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "Microsoft.AspNetCore.Metadata.wasm",
        "name": "Microsoft.AspNetCore.Metadata.w0ssh3ovoz.wasm",
        "hash": "sha256-IKYgK7uy/R7+wXLozqmbT/fff2Fuv8YujzQzytTBzfI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 34384,
        "virtualPath": "Microsoft.Extensions.Configuration.wasm",
        "name": "Microsoft.Extensions.Configuration.9rpw4ifbct.wasm",
        "hash": "sha256-ZQaMwz+B8lBbLnAzfJQ+wApCDgi0GENGxEinAbBPb9w=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 32848,
        "virtualPath": "Microsoft.Extensions.Configuration.Binder.wasm",
        "name": "Microsoft.Extensions.Configuration.Binder.56talnw6fw.wasm",
        "hash": "sha256-09zipNk6DJDNqGYn+FYMEQiyvNfRmU7bNAcFxOpcNCs=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 16976,
        "virtualPath": "Microsoft.Extensions.Configuration.EnvironmentVariables.wasm",
        "name": "Microsoft.Extensions.Configuration.EnvironmentVariables.z0vgmhrln1.wasm",
        "hash": "sha256-E6vhxF1xAbRO9cmPUYmrZuCayNbQwg1oq/c1KXi3C1o=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 19024,
        "virtualPath": "Microsoft.Extensions.Configuration.FileExtensions.wasm",
        "name": "Microsoft.Extensions.Configuration.FileExtensions.3v7h08qab2.wasm",
        "hash": "sha256-D4IKq4gTg30T2WLzZdTsGHeudWFg1oV2l0tVW1Fs3bk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 16976,
        "virtualPath": "Microsoft.Extensions.Configuration.Json.wasm",
        "name": "Microsoft.Extensions.Configuration.Json.hu75ojzlh3.wasm",
        "hash": "sha256-B5XjEypRdiCAdclTjArDbIbWY39cs4QSABlghv57WCo=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 86608,
        "virtualPath": "Microsoft.Extensions.DependencyInjection.wasm",
        "name": "Microsoft.Extensions.DependencyInjection.wpf2e66y5e.wasm",
        "hash": "sha256-FEJ0AI7EZyQKyR/RCmUGe/cX62fjrRMMNJ8U/e16aNM=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 37456,
        "virtualPath": "Microsoft.Extensions.Diagnostics.wasm",
        "name": "Microsoft.Extensions.Diagnostics.r4jikqj266.wasm",
        "hash": "sha256-peJU98fk9vTz0R/+O81n/4CZMwu95VfLhH9hRDgBgms=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 40528,
        "virtualPath": "Microsoft.Extensions.FileProviders.Physical.wasm",
        "name": "Microsoft.Extensions.FileProviders.Physical.8dnb0sugn7.wasm",
        "hash": "sha256-DBudzhvwBklS/rzTgbzvtXADWC7E7/A6TL6SQ/b3Q3U=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 36944,
        "virtualPath": "Microsoft.Extensions.FileSystemGlobbing.wasm",
        "name": "Microsoft.Extensions.FileSystemGlobbing.46os95nnpm.wasm",
        "hash": "sha256-XHaDq22dAzVhJoLzLafgEufO6sXNHHwC0nNM4usNbi8=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 8784,
        "virtualPath": "Microsoft.Extensions.Localization.Abstractions.wasm",
        "name": "Microsoft.Extensions.Localization.Abstractions.31oo3lfiw4.wasm",
        "hash": "sha256-Wakfv68W6c7KIL+LJnD4k8sLR2C5xy5lYWSXHoM/Y2M=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 41040,
        "virtualPath": "Microsoft.Extensions.Logging.wasm",
        "name": "Microsoft.Extensions.Logging.g6renuo6ud.wasm",
        "hash": "sha256-6M/X94so6mDJTi24LtTG+4NfmoTOBr9JRMGR+1PYIe8=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 11344,
        "virtualPath": "Microsoft.Extensions.Options.ConfigurationExtensions.wasm",
        "name": "Microsoft.Extensions.Options.ConfigurationExtensions.sy3zn47g40.wasm",
        "hash": "sha256-1HUSw4azH6pDocI50Db0aNhWaE5k3jpb3hN2vhCQ6w0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 11856,
        "virtualPath": "Microsoft.Extensions.Validation.wasm",
        "name": "Microsoft.Extensions.Validation.3y05m210dk.wasm",
        "hash": "sha256-9T3XO8THG/2cTkjjnHbxuW6w9dIeKuVriiIB4Jh4kBs=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 67664,
        "virtualPath": "Microsoft.JSInterop.wasm",
        "name": "Microsoft.JSInterop.44tczavxny.wasm",
        "hash": "sha256-ZLnM8EPLOmcOVulO7qjanoihqZacece0sRoSgjGKI5s=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 13904,
        "virtualPath": "Microsoft.JSInterop.WebAssembly.wasm",
        "name": "Microsoft.JSInterop.WebAssembly.dd1hyeex4f.wasm",
        "hash": "sha256-z2RuUgPel4iTe7O28sT0HMYBEhufKpHteeVIrrpNFM8=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 77904,
        "virtualPath": "Toolbelt.Blazor.HotKeys2.wasm",
        "name": "Toolbelt.Blazor.HotKeys2.t4i7rv6gv2.wasm",
        "hash": "sha256-c3HHrKEc+Hrv6tU5OB5On2NTEfN4tmRP8rHABU2KxI0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 24144,
        "virtualPath": "Toolbelt.Blazor.SplitContainer.wasm",
        "name": "Toolbelt.Blazor.SplitContainer.nfyxli1i9u.wasm",
        "hash": "sha256-bYb9oGDw7PfPpglDHELgF8cUIHYEArsZUE1Lf0Ps2dI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 9808,
        "virtualPath": "Toolbelt.Web.CssClassInlineBuilder.wasm",
        "name": "Toolbelt.Web.CssClassInlineBuilder.knaizh04mq.wasm",
        "hash": "sha256-cO35MRM760JuhhdK91FdVAdR3gEhOqE7XlTSMdas44E=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 300624,
        "virtualPath": "Microsoft.CSharp.wasm",
        "name": "Microsoft.CSharp.kfm9yuy100.wasm",
        "hash": "sha256-0NPLZ4kE9BQ2OaNu2gh6hZTMJwxZEwdhPL8b5wRoVds=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 32848,
        "virtualPath": "Microsoft.Extensions.Caching.Abstractions.wasm",
        "name": "Microsoft.Extensions.Caching.Abstractions.k41dg2oy4w.wasm",
        "hash": "sha256-9J7ax+L0dQ8VNbJ9wQa4/hWHo2Zp+zQNl7KxhGqKLhU=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 16464,
        "virtualPath": "Microsoft.Extensions.Configuration.Abstractions.wasm",
        "name": "Microsoft.Extensions.Configuration.Abstractions.tqz3qu0dvk.wasm",
        "hash": "sha256-S1bLHT5G1gydX/bZ+wL3Ec/MFla9hRE19jv5pRr+WsI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 57424,
        "virtualPath": "Microsoft.Extensions.DependencyInjection.Abstractions.wasm",
        "name": "Microsoft.Extensions.DependencyInjection.Abstractions.bqt2i35hya.wasm",
        "hash": "sha256-A9H2lNpT5LgYVksCwkcJLm4Xbh+BlbdUA1IFjb7Yeig=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 17488,
        "virtualPath": "Microsoft.Extensions.Diagnostics.Abstractions.wasm",
        "name": "Microsoft.Extensions.Diagnostics.Abstractions.6cr5dhj6ft.wasm",
        "hash": "sha256-ptpsxTSAZV8eeOuZ5JBZlFYef8rpz7L+1bb1rrqc8ME=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 11344,
        "virtualPath": "Microsoft.Extensions.FileProviders.Abstractions.wasm",
        "name": "Microsoft.Extensions.FileProviders.Abstractions.yxi3pter98.wasm",
        "hash": "sha256-pm/UlHKYTa0MR66RzFHOcDjunbZZD0SFqJfIiTrlafI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 21584,
        "virtualPath": "Microsoft.Extensions.Hosting.Abstractions.wasm",
        "name": "Microsoft.Extensions.Hosting.Abstractions.s3uaucuyu9.wasm",
        "hash": "sha256-6KTi7XSetncK3IWBIlIGHd1HRwfiDBkcEzCg1yG6174=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 56400,
        "virtualPath": "Microsoft.Extensions.Logging.Abstractions.wasm",
        "name": "Microsoft.Extensions.Logging.Abstractions.0golvgzk7y.wasm",
        "hash": "sha256-AKKgDitRWaOE/dfd4TGdVoj1UFWDQjE4n2oM9PYrams=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 97872,
        "virtualPath": "Microsoft.Extensions.Options.wasm",
        "name": "Microsoft.Extensions.Options.yub4h79uz0.wasm",
        "hash": "sha256-9q0IWDCY2KuSgvNlUJlul1gBB5j5ymJCdl5EJZmBtnU=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 35408,
        "virtualPath": "Microsoft.Extensions.Primitives.wasm",
        "name": "Microsoft.Extensions.Primitives.8nsenkuiw1.wasm",
        "hash": "sha256-hC/FEJ8CJ60guV8fqHSrBialdB+EeA4XykU41qTMn/w=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 418384,
        "virtualPath": "Microsoft.VisualBasic.Core.wasm",
        "name": "Microsoft.VisualBasic.Core.qyqh186u4r.wasm",
        "hash": "sha256-PrxO3N6JI8GjJnmc2msGgSqGa+qYiPZ8WqYupfV9/ss=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6736,
        "virtualPath": "Microsoft.VisualBasic.wasm",
        "name": "Microsoft.VisualBasic.utvux904of.wasm",
        "hash": "sha256-Xn5Puf6Al0ujUR+G3dY3hzDYS76EXv68xwIyI+KDNZM=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "Microsoft.Win32.Primitives.wasm",
        "name": "Microsoft.Win32.Primitives.s32a1m1goq.wasm",
        "hash": "sha256-KPKt4Xc2PPw/pJ8oxmM3qyYTLHy40rQuKE1nM6FxO1I=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 22608,
        "virtualPath": "Microsoft.Win32.Registry.wasm",
        "name": "Microsoft.Win32.Registry.6lka1uke1h.wasm",
        "hash": "sha256-RuC1O/ZTLqzkSzeuHwAneGkqFjgQ4Pe5UV4NFa9T51E=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.AppContext.wasm",
        "name": "System.AppContext.tj2sa9zr5x.wasm",
        "hash": "sha256-FwYzr/HE2EeXMo2jaeOgmqwn6I+tUZahrUJN0kTNXzs=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Buffers.wasm",
        "name": "System.Buffers.tqcydyowiz.wasm",
        "hash": "sha256-Bepey2yzvnZrZS2fhgdypPArD4WCL1eXTU1hH6+UC/k=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 53328,
        "virtualPath": "System.Collections.Concurrent.wasm",
        "name": "System.Collections.Concurrent.lthadgv4h5.wasm",
        "hash": "sha256-udWPFpbWlF88JKkrnMyutoCPeZtNnIkTx/hEQnbcZ8c=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 242256,
        "virtualPath": "System.Collections.Immutable.wasm",
        "name": "System.Collections.Immutable.l0he6zyypw.wasm",
        "hash": "sha256-pR+mZAvS4fxkHl2uNA9yZ2MYZSpgMm0XeZd9sQ2jM2M=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 37456,
        "virtualPath": "System.Collections.NonGeneric.wasm",
        "name": "System.Collections.NonGeneric.ad1pxu81mf.wasm",
        "hash": "sha256-0nNI661rsvGFohc6226vgIz4gp3SW0iIdBmS9PRKXUw=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 37456,
        "virtualPath": "System.Collections.Specialized.wasm",
        "name": "System.Collections.Specialized.gtk4obwfrf.wasm",
        "hash": "sha256-nNnRM4XSkLPbeB2FU+WkG5e3fMVGlCp/3A0+iVo/bXg=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 102480,
        "virtualPath": "System.Collections.wasm",
        "name": "System.Collections.zra07ht1fn.wasm",
        "hash": "sha256-j5FWPg78kdV/irMkN/a9Hul+XzxOiw0E0LqUsXtlBlI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 109136,
        "virtualPath": "System.ComponentModel.Annotations.wasm",
        "name": "System.ComponentModel.Annotations.nqt6nbrn32.wasm",
        "hash": "sha256-D8/wiGEr02FavcWX+/zIXepD7KcXuGeKh1qjL5llKtw=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6224,
        "virtualPath": "System.ComponentModel.DataAnnotations.wasm",
        "name": "System.ComponentModel.DataAnnotations.4e6cnvzxii.wasm",
        "hash": "sha256-XG6fnafukVMgSrczsbOioEt6nssqOUN+qC6g6e0VAOk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 15952,
        "virtualPath": "System.ComponentModel.EventBasedAsync.wasm",
        "name": "System.ComponentModel.EventBasedAsync.nbp7w77b68.wasm",
        "hash": "sha256-80dAxEoAcLpzzuDNEjyOKba2Mwtl7PAcXTeuYMNc+6k=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 31824,
        "virtualPath": "System.ComponentModel.Primitives.wasm",
        "name": "System.ComponentModel.Primitives.9j1l9lir7m.wasm",
        "hash": "sha256-Bfjz6nRgx113FuriRkaBykevaBfl0yr+29M3lXbWgWs=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 307280,
        "virtualPath": "System.ComponentModel.TypeConverter.wasm",
        "name": "System.ComponentModel.TypeConverter.punqig44zl.wasm",
        "hash": "sha256-vzI5K7pxJdneWftxJT0Ykx1RSK//5DENcBtTSKtKMRI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "System.ComponentModel.wasm",
        "name": "System.ComponentModel.0r7pscf5u4.wasm",
        "hash": "sha256-hJAp4YkPxz7/ka1jCQNiRN1m9Jn0WaPEMEzS8qlrQXQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 8784,
        "virtualPath": "System.Configuration.wasm",
        "name": "System.Configuration.fy2yd6l86x.wasm",
        "hash": "sha256-d5itzP7s6tFm83lV1OEIBoEzEJPfrhR8Ws78tB1dq8k=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 43088,
        "virtualPath": "System.Console.wasm",
        "name": "System.Console.5jthgzyw5m.wasm",
        "hash": "sha256-wUuQHqYEE70QCaL7xHMGtfuHU54LKZWGTEC0CJIA+JM=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 12880,
        "virtualPath": "System.Core.wasm",
        "name": "System.Core.wvv4updlri.wasm",
        "hash": "sha256-bGygeNKDZ+N9tqHVmirIuq01eGHkHDEniyT510iRLpE=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 1007184,
        "virtualPath": "System.Data.Common.wasm",
        "name": "System.Data.Common.285h6coz0g.wasm",
        "hash": "sha256-ThW+GEOBdAGr2jgj+UoWW4TWsq2J5as8lwnV7GMQ6go=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Data.DataSetExtensions.wasm",
        "name": "System.Data.DataSetExtensions.7lsumcpq6l.wasm",
        "hash": "sha256-O4Lu0yAtccpmVCTX10NMktWkyTVGSx5zVfl6sLL5bXA=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 14928,
        "virtualPath": "System.Data.wasm",
        "name": "System.Data.z70c9476z7.wasm",
        "hash": "sha256-H7XnrUjWPt0UF1jktJy0SJYOOkG9QCLWhsgxsND74Ck=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "System.Diagnostics.Contracts.wasm",
        "name": "System.Diagnostics.Contracts.anx05bvtue.wasm",
        "hash": "sha256-VguMGR/TC7QXX+3bHcecy8zkN3Ec1Ss158XbU+ZjgDk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Diagnostics.Debug.wasm",
        "name": "System.Diagnostics.Debug.p9qk64mjqw.wasm",
        "hash": "sha256-/TdXrWpQ7/A3K/k854gs07qtHTKZ5EtRO6A2Vw+rfBc=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 198224,
        "virtualPath": "System.Diagnostics.DiagnosticSource.wasm",
        "name": "System.Diagnostics.DiagnosticSource.xt514l75el.wasm",
        "hash": "sha256-O4RWX3nj9APqW2vYQ1GZ+KRWoWf85Sqyw9GQ2lTZQbs=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 11856,
        "virtualPath": "System.Diagnostics.FileVersionInfo.wasm",
        "name": "System.Diagnostics.FileVersionInfo.odr4hy4bza.wasm",
        "hash": "sha256-nNwkjpYcSCNSQ0FQxiZ6zaijh05sxrcMIYqE0rR/9dE=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 58448,
        "virtualPath": "System.Diagnostics.Process.wasm",
        "name": "System.Diagnostics.Process.lc2coeb9zo.wasm",
        "hash": "sha256-G2kCLMlmb5K+rhLNg3UhoPRNWRTinzPQrvqTwdiOLP0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 15440,
        "virtualPath": "System.Diagnostics.StackTrace.wasm",
        "name": "System.Diagnostics.StackTrace.citkvlllzc.wasm",
        "hash": "sha256-ZFWDbAJyxm1cgRbkoHSyhh3HWmCQ3vDQiiN9KG9pa40=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 21072,
        "virtualPath": "System.Diagnostics.TextWriterTraceListener.wasm",
        "name": "System.Diagnostics.TextWriterTraceListener.xstzkl8thi.wasm",
        "hash": "sha256-X7Z38o4SoFDleSl3/gGzFdHZEc8JG3AjvCu4WipF9Cc=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Diagnostics.Tools.wasm",
        "name": "System.Diagnostics.Tools.yqw8e0e66k.wasm",
        "hash": "sha256-NnrE4Jjz4vuApF5xSX8knVkGaMYyJs30FiQDM3x2P0k=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 48208,
        "virtualPath": "System.Diagnostics.TraceSource.wasm",
        "name": "System.Diagnostics.TraceSource.u6k98s0hvt.wasm",
        "hash": "sha256-dgazVBR8JtEC68MqzJnNQhkOhQ7V0GL4ntfCuCIbTWA=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "System.Diagnostics.Tracing.wasm",
        "name": "System.Diagnostics.Tracing.3qtycdw0wr.wasm",
        "hash": "sha256-BVULOztO/NH1rifbDKYjMoyCBHf9vzjSoNiE4D6K7bA=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 54352,
        "virtualPath": "System.Drawing.Primitives.wasm",
        "name": "System.Drawing.Primitives.20mnkxh81v.wasm",
        "hash": "sha256-IpcP9X3nY0x8nWAXHC8CI4YdEStp10631dbar15uwiU=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 9808,
        "virtualPath": "System.Drawing.wasm",
        "name": "System.Drawing.97w21dt5mi.wasm",
        "hash": "sha256-e+tPOJZQXlKA/uT1+w2jGyaKC6yR9jJJolatRON1/3c=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "System.Dynamic.Runtime.wasm",
        "name": "System.Dynamic.Runtime.mk5brvcn26.wasm",
        "hash": "sha256-CrhjXVT99prjJbHnuUEbf4DuDN1jP/W+w3jE2n2RN5U=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 96848,
        "virtualPath": "System.Formats.Asn1.wasm",
        "name": "System.Formats.Asn1.hg6ny6lbl4.wasm",
        "hash": "sha256-jzX6ZYhIixmvSb2ABh/UnILhSiqoKtsOS7BoL0RVvFo=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 30800,
        "virtualPath": "System.Formats.Tar.wasm",
        "name": "System.Formats.Tar.cden59mmah.wasm",
        "hash": "sha256-gHEzhd4Yo08Fk/B5s6YmnDk8rFJMfaKJ4Rh0W9wapz8=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Globalization.Calendars.wasm",
        "name": "System.Globalization.Calendars.ucibclsw86.wasm",
        "hash": "sha256-Ug73E8VxNw3LQh9Sx/Ef3/1olvR7YyGl6HlI2bPi7QU=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Globalization.Extensions.wasm",
        "name": "System.Globalization.Extensions.0gje76o1ka.wasm",
        "hash": "sha256-EKc+MZjCTrHDjO75131Z3bgwUqXXWrJDmVFklNhAvXo=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Globalization.wasm",
        "name": "System.Globalization.zf4sy4xuv3.wasm",
        "hash": "sha256-gXgi0WeWGj/s7ZI8YZt1TeJALBiLN6WlpB0oV8jnQc0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 17488,
        "virtualPath": "System.IO.Compression.Brotli.wasm",
        "name": "System.IO.Compression.Brotli.7vpa9c5ub5.wasm",
        "hash": "sha256-jTWcrqpwCnqyWvR4jd7Ls2VIVViO5Nv5sXXKvfFCHoE=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.IO.Compression.FileSystem.wasm",
        "name": "System.IO.Compression.FileSystem.0kg93tq43j.wasm",
        "hash": "sha256-8CxIkQqn1KblxSnt4mD7A+j4DdUH7Q9f9bqHPHB8PBU=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 56400,
        "virtualPath": "System.IO.Compression.ZipFile.wasm",
        "name": "System.IO.Compression.ZipFile.k995wshqjs.wasm",
        "hash": "sha256-KF4Xus2uWg8VrDHmafrCI8t0665cQ5RJ2gRth2n5vwI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 210512,
        "virtualPath": "System.IO.Compression.wasm",
        "name": "System.IO.Compression.kdu78a74gj.wasm",
        "hash": "sha256-eIpkVellhFiU9fbP2oX9oc4qUPNgs121+UJctjgBH7E=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 21584,
        "virtualPath": "System.IO.FileSystem.AccessControl.wasm",
        "name": "System.IO.FileSystem.AccessControl.qaz87sy4tt.wasm",
        "hash": "sha256-TiRSrMh32m2ATfVm5AayIDKaQuqS54OG7DzRErmWkgw=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 13392,
        "virtualPath": "System.IO.FileSystem.DriveInfo.wasm",
        "name": "System.IO.FileSystem.DriveInfo.h55bq0nk5s.wasm",
        "hash": "sha256-1bjef+ilq1FQ4iHRh8aoHl3+7NMEnMtefV65aD0Xvv8=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.IO.FileSystem.Primitives.wasm",
        "name": "System.IO.FileSystem.Primitives.iecsbmkurv.wasm",
        "hash": "sha256-Ng+4vQZwhPvSlHjRkDxX/DP/y/nn2f+7NXd7RPHo6HI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 22608,
        "virtualPath": "System.IO.FileSystem.Watcher.wasm",
        "name": "System.IO.FileSystem.Watcher.mte0a1pd5n.wasm",
        "hash": "sha256-9vaB+nKf5VufSmdp7I1Mx3Hzu5xvbaSHF2fIU/djelE=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.IO.FileSystem.wasm",
        "name": "System.IO.FileSystem.4ww3vzh0qu.wasm",
        "hash": "sha256-vDu8pX+mLya3N5D16gG55TaowaPgGNq+ylN7FNRrOJI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 24144,
        "virtualPath": "System.IO.IsolatedStorage.wasm",
        "name": "System.IO.IsolatedStorage.4opp3obsdc.wasm",
        "hash": "sha256-zL6f2y0Tk/Pxu5LeX7geW9dXm8LYx9+ED0v3jx6zYl4=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 38480,
        "virtualPath": "System.IO.MemoryMappedFiles.wasm",
        "name": "System.IO.MemoryMappedFiles.ba0qfp8hvu.wasm",
        "hash": "sha256-DUkREVrEAyiBA8grvjQmowobqIwj80AKR3sTwrKGpZ4=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 73296,
        "virtualPath": "System.IO.Pipelines.wasm",
        "name": "System.IO.Pipelines.c1mx5n5sjo.wasm",
        "hash": "sha256-0B7asSRhpik4vrRfFfWtV4+5Jx/4qe32CRQS4RTX12A=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 12880,
        "virtualPath": "System.IO.Pipes.AccessControl.wasm",
        "name": "System.IO.Pipes.AccessControl.cbm5cx4n9i.wasm",
        "hash": "sha256-OLCrhS/zxyZiIS41+WQvFhjeWPZVw9a+1QweE3VGuLs=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 32336,
        "virtualPath": "System.IO.Pipes.wasm",
        "name": "System.IO.Pipes.egon9m5hzh.wasm",
        "hash": "sha256-jiy2oFD1zWhSmSInr3Ni4vvlfOGX45VgeU+iiBLcSSc=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.IO.UnmanagedMemoryStream.wasm",
        "name": "System.IO.UnmanagedMemoryStream.6fwpq17y7d.wasm",
        "hash": "sha256-Yl7AgriGmHQ3EIcM84h/9rhQY9rbyauc/zCONX8vgI0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.IO.wasm",
        "name": "System.IO.x9soo596wt.wasm",
        "hash": "sha256-JQEvjPOUWXdMqZxQ/n/4GkOqIGgkAe9ABwl8m0VhJ8Y=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 503888,
        "virtualPath": "System.Linq.AsyncEnumerable.wasm",
        "name": "System.Linq.AsyncEnumerable.ivr0xvsc6z.wasm",
        "hash": "sha256-HuoKNTJJ+yEittYMxD5seG9xDOwuoiX9U4gXEUu4w6U=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 565328,
        "virtualPath": "System.Linq.Expressions.wasm",
        "name": "System.Linq.Expressions.iqneteaoxs.wasm",
        "hash": "sha256-VrubXsnHe2+Km4ekTTekTk40Xpc0fa+RKxLGNw6Roi4=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 214608,
        "virtualPath": "System.Linq.Parallel.wasm",
        "name": "System.Linq.Parallel.s4zwszxfmp.wasm",
        "hash": "sha256-JK51pB+YEjjIgDI1w8rRWWO3VDdYp081P312JoAFFRk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 71248,
        "virtualPath": "System.Linq.Queryable.wasm",
        "name": "System.Linq.Queryable.c2r4b1jfkl.wasm",
        "hash": "sha256-/B6sXvxYp0i8ypwIXw8ZCbgJSUUXOMcOBoA+EV/Hvbg=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 202832,
        "virtualPath": "System.Linq.wasm",
        "name": "System.Linq.dcl9hzdsug.wasm",
        "hash": "sha256-sz4wggUh1zaogavOi/0GVvLj80CA3c+8XT2v6Vqygak=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 49232,
        "virtualPath": "System.Memory.wasm",
        "name": "System.Memory.dgudlqqov0.wasm",
        "hash": "sha256-k6cauYdX66m7XIek/abItfd1B9d3Yy0O7hd88/E21i0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 45648,
        "virtualPath": "System.Net.Http.Json.wasm",
        "name": "System.Net.Http.Json.sluvyrihbu.wasm",
        "hash": "sha256-UEUeDwEbiXiOGHbS0ybe9/GrJF7kihRlll4LndrDLzg=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 293456,
        "virtualPath": "System.Net.Http.wasm",
        "name": "System.Net.Http.c7tmrl7o78.wasm",
        "hash": "sha256-WBzq8PgjOnIpe5rdvYNyo6iJDqIbSyvF35QhPGCD18E=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 45648,
        "virtualPath": "System.Net.HttpListener.wasm",
        "name": "System.Net.HttpListener.pu5tmfe9ry.wasm",
        "hash": "sha256-4VTuekktXsooPooCm0s93MTNqfVHVKoURzGNDMj17Os=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 126032,
        "virtualPath": "System.Net.Mail.wasm",
        "name": "System.Net.Mail.aj7pi0mjg2.wasm",
        "hash": "sha256-ZmQhJLXcwe9h4UGN46hRXaM/3V/M2vvye0rUFtEQUHQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 21072,
        "virtualPath": "System.Net.NameResolution.wasm",
        "name": "System.Net.NameResolution.62obb2odzi.wasm",
        "hash": "sha256-cr5qWsOv3RCFdKwUfnA/aG0PiylO1wsXE7i0lwO0C7s=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 31824,
        "virtualPath": "System.Net.NetworkInformation.wasm",
        "name": "System.Net.NetworkInformation.jn2lli2d0w.wasm",
        "hash": "sha256-rx3Qank9GaI+9LmiUIUMKDZfp7lufZyP3HXPfQSrIj8=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 17488,
        "virtualPath": "System.Net.Ping.wasm",
        "name": "System.Net.Ping.hanopjymx7.wasm",
        "hash": "sha256-F+9srYn1LpjTjgZbycSpO7T9v9NtnwqPDD/cKDUXsYI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 97872,
        "virtualPath": "System.Net.Primitives.wasm",
        "name": "System.Net.Primitives.wavmo5w73l.wasm",
        "hash": "sha256-CKx2T86jV5a0ruum8c9GGBDfIYiP1tKVh4KlQFs6bjc=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 28752,
        "virtualPath": "System.Net.Quic.wasm",
        "name": "System.Net.Quic.y9dv3vqaru.wasm",
        "hash": "sha256-9jOmC06PsGBdGICIN0JZJySqW197+Qy3+ZJZKtxu3Js=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 55376,
        "virtualPath": "System.Net.Requests.wasm",
        "name": "System.Net.Requests.jkr3410s9z.wasm",
        "hash": "sha256-U9ZXJ9Tp+VVWUiSNqwxqQoSqDmUYb/eXmesHiapp5Fo=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 113744,
        "virtualPath": "System.Net.Security.wasm",
        "name": "System.Net.Security.6mn4ataiep.wasm",
        "hash": "sha256-crH+2/wc9FDrYszRbo/bf8SU8M5xYOImg2H6t6XEIjw=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 31312,
        "virtualPath": "System.Net.ServerSentEvents.wasm",
        "name": "System.Net.ServerSentEvents.pr0qxfqtua.wasm",
        "hash": "sha256-ZcPKqsFSlVVjLrKuKHHZacSHWPJ5rmDSexNJJj5JNRE=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Net.ServicePoint.wasm",
        "name": "System.Net.ServicePoint.hrryvx0dt3.wasm",
        "hash": "sha256-81vrJqWQiDCi2wWEpf5BWm/2BoQsBEDDOQUdLfxDt+I=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 64080,
        "virtualPath": "System.Net.Sockets.wasm",
        "name": "System.Net.Sockets.eglsbm093j.wasm",
        "hash": "sha256-YmEYJs8/g5z9pom3lBxR5BKyINq61nE8Fxl7PPRju+0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 46672,
        "virtualPath": "System.Net.WebClient.wasm",
        "name": "System.Net.WebClient.v9l6hoop95.wasm",
        "hash": "sha256-zX2tKFWRjLOfBLkerLAau1m1xJ8iZcfnCIckK/4m9ag=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 22608,
        "virtualPath": "System.Net.WebHeaderCollection.wasm",
        "name": "System.Net.WebHeaderCollection.yx5lbk3szr.wasm",
        "hash": "sha256-VvyYHp8Q3ssPLm1bmrz+YVbd8/8kvIGQZ2Mg1TEiG+w=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 11856,
        "virtualPath": "System.Net.WebProxy.wasm",
        "name": "System.Net.WebProxy.34ebpqfxjg.wasm",
        "hash": "sha256-M63oBrvsCvli8ivOOT/IScK9krO19KgC9StBzOw6s2o=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 41552,
        "virtualPath": "System.Net.WebSockets.Client.wasm",
        "name": "System.Net.WebSockets.Client.kq11xbrjuo.wasm",
        "hash": "sha256-DPwGfheu7KtqrLkHC1ylDF2tMPeRKSmjHPAw4kFivzQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 100432,
        "virtualPath": "System.Net.WebSockets.wasm",
        "name": "System.Net.WebSockets.w9x69tmvfu.wasm",
        "hash": "sha256-DLXzFSPd/O773PN/iWurS/mK5naZOp/k9fginMzCt+s=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6736,
        "virtualPath": "System.Net.wasm",
        "name": "System.Net.2gnm43o499.wasm",
        "hash": "sha256-MhAWqdkkAiAlIHe4KwOtujaC1F3k7YT0jkYyQ6gU/BE=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "System.Numerics.Vectors.wasm",
        "name": "System.Numerics.Vectors.bw93k8orhp.wasm",
        "hash": "sha256-Jbc4GP+6QgqFhJuwV8uUZDr8fRplg6FnDQThDhirPQk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Numerics.wasm",
        "name": "System.Numerics.g5sh2mqcdn.wasm",
        "hash": "sha256-PF0ay2URV+SpCOv5heEZqGFpTIMC4oP+ZZ3gMDQaPqw=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 30800,
        "virtualPath": "System.ObjectModel.wasm",
        "name": "System.ObjectModel.vzy23y3uzd.wasm",
        "hash": "sha256-VYfXHurqhN3H6YBASulimU9FyHHi3oB8I81EdFIFF5w=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 848976,
        "virtualPath": "System.Private.DataContractSerialization.wasm",
        "name": "System.Private.DataContractSerialization.silvgrpf2n.wasm",
        "hash": "sha256-TNXDfN9xkGfnlWYorT4wvzwVFT45Fold9aTRK7Zvqkc=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 92240,
        "virtualPath": "System.Private.Uri.wasm",
        "name": "System.Private.Uri.tie9daveb3.wasm",
        "hash": "sha256-O0mg+ucaWvXvrba8P5De6vKriGxlRzlqIs7E6DsVKxY=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 144464,
        "virtualPath": "System.Private.Xml.Linq.wasm",
        "name": "System.Private.Xml.Linq.02fc3z4z55.wasm",
        "hash": "sha256-OCaihxbUNRFSWfv9Jy+LX6YQDKQZzpL+U04neJ6I/5w=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 3109968,
        "virtualPath": "System.Private.Xml.wasm",
        "name": "System.Private.Xml.opp3lj2zbn.wasm",
        "hash": "sha256-FBqxp5eZsSrh3BcZti6sUhjK4sNf8MFAnFV0jNPKpyA=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 27728,
        "virtualPath": "System.Reflection.DispatchProxy.wasm",
        "name": "System.Reflection.DispatchProxy.2y1slkljkm.wasm",
        "hash": "sha256-JvJFHDbmSfSM0BX12cau5HUnger1b7zGtJBSrTKhQGs=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Reflection.Emit.ILGeneration.wasm",
        "name": "System.Reflection.Emit.ILGeneration.59rukqv3sj.wasm",
        "hash": "sha256-edHA1Dz3yeUZ9QowabDkyorZy1IxOn6eje0Z9i2HJB4=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Reflection.Emit.Lightweight.wasm",
        "name": "System.Reflection.Emit.Lightweight.gqgu2rt6xn.wasm",
        "hash": "sha256-yHFwFtvcr6ijke9AEjdS253FpFGf2LthTQQwI4wG5PM=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 123984,
        "virtualPath": "System.Reflection.Emit.wasm",
        "name": "System.Reflection.Emit.fh0sc2wmvp.wasm",
        "hash": "sha256-c0WLh4BT29Z0krq9Xt4JBN1lx92dfc8s58aFdh1J98M=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Reflection.Extensions.wasm",
        "name": "System.Reflection.Extensions.vb7jl7kjxx.wasm",
        "hash": "sha256-JaO8UgBnVEea6Y0ZWFFY/dMUDCvrfBjEi59G60vXj+M=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 493648,
        "virtualPath": "System.Reflection.Metadata.wasm",
        "name": "System.Reflection.Metadata.63biykq5zy.wasm",
        "hash": "sha256-ohrjE2wNnS9juG+35mR2iUNVQqpsyqG6/FUOX+MAEU4=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Reflection.Primitives.wasm",
        "name": "System.Reflection.Primitives.x13qrcnytc.wasm",
        "hash": "sha256-9SzCQaIfydFPtuB3jI5o5W4YluZy9D6A69jJC0ICmB4=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 13904,
        "virtualPath": "System.Reflection.TypeExtensions.wasm",
        "name": "System.Reflection.TypeExtensions.re89t4b7m5.wasm",
        "hash": "sha256-089rgin5L0hJM4aXdpK7ECpPxr0ugJ0EDJeg4nhiWsg=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "System.Reflection.wasm",
        "name": "System.Reflection.uvcxdfqzxu.wasm",
        "hash": "sha256-hhdIgtdL9AulbvCUkwYtGpzc++ce+V/WWVRisQx14tc=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Resources.Reader.wasm",
        "name": "System.Resources.Reader.oir35l1zu5.wasm",
        "hash": "sha256-EBeG1/12c3JE1jS4cMCRsLLOX0E0lJrvgMIbb2eEfN0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Resources.ResourceManager.wasm",
        "name": "System.Resources.ResourceManager.4twwn3qfsb.wasm",
        "hash": "sha256-6M61/HW+4wK6BNLEonQau4j2e9JxPQKZOFzI944Eb/Y=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 16464,
        "virtualPath": "System.Resources.Writer.wasm",
        "name": "System.Resources.Writer.3k06dva2yo.wasm",
        "hash": "sha256-rY7P6DIVejjDGm3uNGkj74iz90HKhRmJrQlgEYmZrXo=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Runtime.CompilerServices.Unsafe.wasm",
        "name": "System.Runtime.CompilerServices.Unsafe.issto093zc.wasm",
        "hash": "sha256-Ap9BLeqYqbpW4Zyh0LUtIZrp/9NTSmXMqp68k1YEMe0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6736,
        "virtualPath": "System.Runtime.CompilerServices.VisualC.wasm",
        "name": "System.Runtime.CompilerServices.VisualC.9ocfasplqd.wasm",
        "hash": "sha256-6ffW77F2Gn9lK8O9x7Zh/JuA3wU3ImpaX4febZ6Sivk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 7248,
        "virtualPath": "System.Runtime.Extensions.wasm",
        "name": "System.Runtime.Extensions.bivwtb0v3y.wasm",
        "hash": "sha256-g3A4ELhyFel0Sj13w9xPExW9GTUnh8Xpf5TLSWFggGk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Runtime.Handles.wasm",
        "name": "System.Runtime.Handles.7u1q1wl1do.wasm",
        "hash": "sha256-McOX/WxvmG2QZrgxPetiMc+XRSPhZbxE+1Piof4h8zM=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Runtime.InteropServices.RuntimeInformation.wasm",
        "name": "System.Runtime.InteropServices.RuntimeInformation.ty0izun4ef.wasm",
        "hash": "sha256-ZYD6M/ETwPAoVY2X29dmfaj7jK+71cO6QM/NvgKedRY=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 53840,
        "virtualPath": "System.Runtime.InteropServices.wasm",
        "name": "System.Runtime.InteropServices.axpino88we.wasm",
        "hash": "sha256-vqrsJ9Mskk06KGdZQlhYrvHY/UEk0WrFOVwsNNid61I=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 8272,
        "virtualPath": "System.Runtime.Intrinsics.wasm",
        "name": "System.Runtime.Intrinsics.v8t1a4cu19.wasm",
        "hash": "sha256-T2RhZ+yL3qZEGsrTB2+O5Gt8LwpNiQ3CrRj1gbprdcs=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Runtime.Loader.wasm",
        "name": "System.Runtime.Loader.obpjv2xpwl.wasm",
        "hash": "sha256-9B3n2N2ukhYH3Hc7ZyGc0VITOqsD5cEBr7oD3GVU5iM=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 180304,
        "virtualPath": "System.Runtime.Numerics.wasm",
        "name": "System.Runtime.Numerics.lhwe0p8dkk.wasm",
        "hash": "sha256-+V+qaWAYeqEx34I3ToqZp3rLWlL4LQnAu7wKXj3LFww=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 55376,
        "virtualPath": "System.Runtime.Serialization.Formatters.wasm",
        "name": "System.Runtime.Serialization.Formatters.wh2npalni6.wasm",
        "hash": "sha256-nGI2krKyRpSBN6ETJpKI8baQpwN8U78GjDt8mgqjJjw=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Runtime.Serialization.Json.wasm",
        "name": "System.Runtime.Serialization.Json.rd8tbegpxn.wasm",
        "hash": "sha256-UQ364oHQfVb16yqBdWwdOPQpDwI6rIp3XSWX6hMhbz4=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 12880,
        "virtualPath": "System.Runtime.Serialization.Primitives.wasm",
        "name": "System.Runtime.Serialization.Primitives.b94e91wdpi.wasm",
        "hash": "sha256-D5rn66MdSREPgg3B28/d/gj7eR2JvgYHTow3mJOYQds=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6224,
        "virtualPath": "System.Runtime.Serialization.Xml.wasm",
        "name": "System.Runtime.Serialization.Xml.n3mw830utk.wasm",
        "hash": "sha256-46ucJPdA8RgAkFiwelu8crGMSlrrewfGsLuX6i58i1g=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6224,
        "virtualPath": "System.Runtime.Serialization.wasm",
        "name": "System.Runtime.Serialization.xtl39bnlne.wasm",
        "hash": "sha256-RfbiIh/uxHBFoUTQjTFCJdg9jBiVl6orpDCeJLcAPz8=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 35408,
        "virtualPath": "System.Runtime.wasm",
        "name": "System.Runtime.fz33mzlcvb.wasm",
        "hash": "sha256-FTeTJ6JpT3jgwwZugukQtlbiBKiQYnSGFecEVgUpJgY=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 47184,
        "virtualPath": "System.Security.AccessControl.wasm",
        "name": "System.Security.AccessControl.nyx7rr8fg8.wasm",
        "hash": "sha256-UdGIkAtRnwXKbCKrJ42WdQC48cMY+6c9/9AzftOEs4I=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 44112,
        "virtualPath": "System.Security.Claims.wasm",
        "name": "System.Security.Claims.klyjsqs37e.wasm",
        "hash": "sha256-GAbAVK/cjIv5EBnavCR4wV5MQKL0ZAtva8XcQEiZUI0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6736,
        "virtualPath": "System.Security.Cryptography.Algorithms.wasm",
        "name": "System.Security.Cryptography.Algorithms.zhm2cphi9n.wasm",
        "hash": "sha256-Z/6ke7eCglQfYDkeJEhRY+Mn3ZoTdW1DpQvk8TB2O84=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "System.Security.Cryptography.Cng.wasm",
        "name": "System.Security.Cryptography.Cng.lhk304sl0o.wasm",
        "hash": "sha256-eRq+X0/yCbU127ZktpKBIPg9jE5M5H6GxBKtdV9P8+U=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "System.Security.Cryptography.Csp.wasm",
        "name": "System.Security.Cryptography.Csp.2ortabmjec.wasm",
        "hash": "sha256-rliNr7O12WrHdeHbXDSwSZ43HbYHAopjlzfZOT1o29U=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Security.Cryptography.Encoding.wasm",
        "name": "System.Security.Cryptography.Encoding.20cmrdo2bo.wasm",
        "hash": "sha256-vsqgwLyD9qjibH7eyCgM7HX9wvQo//oYD79daXzCw0w=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Security.Cryptography.OpenSsl.wasm",
        "name": "System.Security.Cryptography.OpenSsl.85s81dt8ah.wasm",
        "hash": "sha256-fAXPZxIC2XcOYwhUNl1uDFHnz24LRgLnhxHyeqNy5ws=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Security.Cryptography.Primitives.wasm",
        "name": "System.Security.Cryptography.Primitives.pu7zcao7p4.wasm",
        "hash": "sha256-f7mysHpIy4CTYGbBlFaxpc+rPfgWV9r1SgrCXmpODp0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6224,
        "virtualPath": "System.Security.Cryptography.X509Certificates.wasm",
        "name": "System.Security.Cryptography.X509Certificates.ioxxbrvbrc.wasm",
        "hash": "sha256-wg9s2qI+eEZEwFhWpQcMnfELIRnrtTj0u6VCits977Y=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 714320,
        "virtualPath": "System.Security.Cryptography.wasm",
        "name": "System.Security.Cryptography.e0zfden0su.wasm",
        "hash": "sha256-AgxEf2wewzz/f/50Tefp2XzOnCdldYT/LKS+y5tnefc=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 27216,
        "virtualPath": "System.Security.Principal.Windows.wasm",
        "name": "System.Security.Principal.Windows.wwlbs0vwsq.wasm",
        "hash": "sha256-UDCLUE4f5/r0rJ7Dn0eDnGmtBWntpXcpcf4jLnQUHLA=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Security.Principal.wasm",
        "name": "System.Security.Principal.b4q1biewgn.wasm",
        "hash": "sha256-hXLe78DMmGmNqGYSPPIA6UTFG/wWxfKDrCBoPgwzOXw=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Security.SecureString.wasm",
        "name": "System.Security.SecureString.new9ysn0km.wasm",
        "hash": "sha256-sTGRAvJ2qVHzBI+3EGIGfdFf/YeBavoqlZEfh6bjOrQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 7760,
        "virtualPath": "System.Security.wasm",
        "name": "System.Security.wxc1i6x8wl.wasm",
        "hash": "sha256-xDFVtJbJu7IkaO/fTyNk9+fxDuiAU63JeBwPtAdG4LQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6224,
        "virtualPath": "System.ServiceModel.Web.wasm",
        "name": "System.ServiceModel.Web.3dl2nctyqn.wasm",
        "hash": "sha256-p09u3jjoeQpiVUSFLegnxXrljK0JgG+g7KfwjdMGwSY=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.ServiceProcess.wasm",
        "name": "System.ServiceProcess.fhfgntluxv.wasm",
        "hash": "sha256-Y42CHp5NLIprnTWdnKJlllY/JCDzLwDiJ5PyS0oDL1c=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 731728,
        "virtualPath": "System.Text.Encoding.CodePages.wasm",
        "name": "System.Text.Encoding.CodePages.9lyl53lq8c.wasm",
        "hash": "sha256-o3z5e4RqCMXqGpmETXGSj8xRJfuJJVEBdSoqh8aDmSs=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Text.Encoding.Extensions.wasm",
        "name": "System.Text.Encoding.Extensions.rycc9dq23j.wasm",
        "hash": "sha256-o+ncic9aFNpEPPsEBXz9O2i8/fomjK7CpQpYdP+Q/98=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Text.Encoding.wasm",
        "name": "System.Text.Encoding.iw4g18zh5w.wasm",
        "hash": "sha256-0q0g9jF4WfVgzeUHNQwOW/V2g8/3p3jDPuIbcLGJvRo=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 55888,
        "virtualPath": "System.Text.Encodings.Web.wasm",
        "name": "System.Text.Encodings.Web.ugxo1k3sqb.wasm",
        "hash": "sha256-kp9NIXd+D16rRRH61ztHbawVFRnkzp3URMYdfAMt0oI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 726096,
        "virtualPath": "System.Text.Json.wasm",
        "name": "System.Text.Json.dlm873fi9n.wasm",
        "hash": "sha256-YeeQMlg/dgUAQyNnsLoQ4vned9yIRXfCG9H92PndR00=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 383568,
        "virtualPath": "System.Text.RegularExpressions.wasm",
        "name": "System.Text.RegularExpressions.vb2y1bjgj2.wasm",
        "hash": "sha256-teJ7Y1SN1DaV4NHhYiA0fWe/kpexl8CEjancTbejrPw=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 22608,
        "virtualPath": "System.Threading.AccessControl.wasm",
        "name": "System.Threading.AccessControl.pn8lsdyqsi.wasm",
        "hash": "sha256-61CSEHptGC/QKQMRAFXzZg7dee41ZNlETHrz4vgQmas=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 55888,
        "virtualPath": "System.Threading.Channels.wasm",
        "name": "System.Threading.Channels.psyi3umkjj.wasm",
        "hash": "sha256-VwSPojIABvPr2/UCmCF/hW6sxUj6nXAbngg6MLBsWs4=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Threading.Overlapped.wasm",
        "name": "System.Threading.Overlapped.c1vzk9j1mj.wasm",
        "hash": "sha256-4E/oiYmuFxEMVhGnjs+smjZGeCzAbVvl6fNnmdQDms0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 175696,
        "virtualPath": "System.Threading.Tasks.Dataflow.wasm",
        "name": "System.Threading.Tasks.Dataflow.varkygmqpc.wasm",
        "hash": "sha256-ybZo6W0zGuPHm+fxUpxVMdoxgax/hdFotncYWTTmC70=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Threading.Tasks.Extensions.wasm",
        "name": "System.Threading.Tasks.Extensions.vmfz9vnzaj.wasm",
        "hash": "sha256-GRGTmMUl60Lt6JlTqpIlX1TSV7bmSR3DAXaN2bj4Ou8=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 51280,
        "virtualPath": "System.Threading.Tasks.Parallel.wasm",
        "name": "System.Threading.Tasks.Parallel.4naydz1lda.wasm",
        "hash": "sha256-YFOJhd87Q7dJcOUWFzY8yHyiBH2eq2LJJE8IwsBdZV0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6224,
        "virtualPath": "System.Threading.Tasks.wasm",
        "name": "System.Threading.Tasks.b3m2dkom2n.wasm",
        "hash": "sha256-qSbbDTfEY7ntu6+sJQEWZSbbiqPI3sLq2dks2ar34pk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Threading.Thread.wasm",
        "name": "System.Threading.Thread.qy1njmr2fl.wasm",
        "hash": "sha256-Tlu4iGukx+zrVF0PXPYpvK2ORq9zSVRV37eqanZ3jjE=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Threading.ThreadPool.wasm",
        "name": "System.Threading.ThreadPool.verm800txi.wasm",
        "hash": "sha256-R3zD+q2ND51siSNfqRr0IjEAov//ZebgECfn3Y/ZsC4=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Threading.Timer.wasm",
        "name": "System.Threading.Timer.gapebxrttj.wasm",
        "hash": "sha256-iHYAbimfVbJKJywQDW9YbcXbKYICZkdjAmQ6u+gtr5g=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 34384,
        "virtualPath": "System.Threading.wasm",
        "name": "System.Threading.e01ehmqnbq.wasm",
        "hash": "sha256-YtrF7VnJUyybpUmDt4JSdkQ4LpGStYtg/kpbGt/3sbw=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 164432,
        "virtualPath": "System.Transactions.Local.wasm",
        "name": "System.Transactions.Local.wn9dhhrvrn.wasm",
        "hash": "sha256-LHcZ52tSm/pROQ985QQNx6vmcO0hDltLOippj0rjmwA=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6224,
        "virtualPath": "System.Transactions.wasm",
        "name": "System.Transactions.jp1diug9uq.wasm",
        "hash": "sha256-5xPNfCnW1SZl9xXjFXmrrNdwBqE92wzSmbhojZ7FJVA=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.ValueTuple.wasm",
        "name": "System.ValueTuple.en1u953gmw.wasm",
        "hash": "sha256-Jo+n6/SjUbD2/rQvDA1nsb1wxJtE5slHtxYncSqQL1A=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 19536,
        "virtualPath": "System.Web.HttpUtility.wasm",
        "name": "System.Web.HttpUtility.scmk0sllom.wasm",
        "hash": "sha256-LC7QOyT4WcMt+0125I/aK5k7Wa/rKVqdaEEQhMVnKjc=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Web.wasm",
        "name": "System.Web.4i33lta79t.wasm",
        "hash": "sha256-fF1oy0GHf7dB3yYWSN7cpZOV5bQDkm6AOAP88s45Jbw=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Windows.wasm",
        "name": "System.Windows.lnjussi7qj.wasm",
        "hash": "sha256-/9QJcNO5W3H0GlfiRMPEeP/kuCQD6APrX58v9Xqe7TQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Xml.Linq.wasm",
        "name": "System.Xml.Linq.maodw0zvaq.wasm",
        "hash": "sha256-wjO3yIAgB6qszUl3dCHO1uLsH7CpJjqLvrXayAH+ArM=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 11344,
        "virtualPath": "System.Xml.ReaderWriter.wasm",
        "name": "System.Xml.ReaderWriter.tlye01cwdk.wasm",
        "hash": "sha256-sQrf4S+dN+J5VrvZ+zY8dzWYNB/skPSJjiLEyFjxAwI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "System.Xml.Serialization.wasm",
        "name": "System.Xml.Serialization.rflrbq684v.wasm",
        "hash": "sha256-uWchppQkxf7ufJwS7anM5jupv7kzOLM/IKPZ1VIYnmU=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Xml.XDocument.wasm",
        "name": "System.Xml.XDocument.3hku9dt0vu.wasm",
        "hash": "sha256-J5AiH5QGxsoy6VWbFjnpQLzIipl83ZgwBnWKdkR4/BY=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Xml.XPath.XDocument.wasm",
        "name": "System.Xml.XPath.XDocument.5mztpustxn.wasm",
        "hash": "sha256-c7UazCAffmPlW9CNcGrVG+nHGnyKIEWK+fdo/KE5ESk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Xml.XPath.wasm",
        "name": "System.Xml.XPath.z9o1qrzv6q.wasm",
        "hash": "sha256-2Tie3WDSfMnVs6Gae7p8bjW4sz1CXIR3r/IxY2xU+bQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Xml.XmlDocument.wasm",
        "name": "System.Xml.XmlDocument.4nyqwmf7vi.wasm",
        "hash": "sha256-IOYhl5DxuGnCh8PbZWM5G4cAt+9O9EuYTYCUC3dSfZQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 7248,
        "virtualPath": "System.Xml.XmlSerializer.wasm",
        "name": "System.Xml.XmlSerializer.5okepy1b7g.wasm",
        "hash": "sha256-JbHXU4oICzdeLaeHGz/7miSsNuuMcdbasBe/PhVylco=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 12880,
        "virtualPath": "System.Xml.wasm",
        "name": "System.Xml.vt96jj889x.wasm",
        "hash": "sha256-Qxbt3zVJFkxl6np9g4fSQL8ySMF2r11rG3EW/OPEAyw=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 39504,
        "virtualPath": "System.wasm",
        "name": "System.noiytmvos5.wasm",
        "hash": "sha256-6Sf9JikQX7X5sJ9LDidOgnjFOnP5ZTeH6WvZZomJpYg=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "WindowsBase.wasm",
        "name": "WindowsBase.fz90iktqd6.wasm",
        "hash": "sha256-i6Seushw58YNvHcbvnnxZbrwiSo/z6LkQod95uIJSaI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 49232,
        "virtualPath": "mscorlib.wasm",
        "name": "mscorlib.x4ni8ean3v.wasm",
        "hash": "sha256-oaWsn83kD2RPiJ2Gyax4j80eDMS1rB9TO5mP9Ngxu8E=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 90704,
        "virtualPath": "netstandard.wasm",
        "name": "netstandard.co8az3jtr9.wasm",
        "hash": "sha256-lFYdE0Hvy9n5U9lLXt3r8Bh/9qGltfekIvYwSFowHgg=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 10320,
        "virtualPath": "BlazingStory.Abstractions.wasm",
        "name": "BlazingStory.Abstractions.c5l78qm30g.wasm",
        "hash": "sha256-0Db9scFQgl4msLCopWPxW2AoMPeOZfedmNBh1216cmo=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 143952,
        "virtualPath": "BlazingStory.Addons.BuiltIns.wasm",
        "name": "BlazingStory.Addons.BuiltIns.tihqdbzlca.wasm",
        "hash": "sha256-y3v8QncDhuCEL1bNzx+ZsGTs8SI2wrPlzxUbZGGhw8o=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 12368,
        "virtualPath": "BlazingStory.Addons.wasm",
        "name": "BlazingStory.Addons.nezy4r61f3.wasm",
        "hash": "sha256-v1dSHC8yYW8Xr0o22910q+O8vhNC6TpAsK7O3Tp0FrQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 288336,
        "virtualPath": "BlazingStory.wasm",
        "name": "BlazingStory.udnqt4vpdq.wasm",
        "hash": "sha256-4vVQdbuqosURIS1unUpRRPIpL8e+Y29rx2qgrMRdKzU=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 140880,
        "virtualPath": "BlazingStory.ToolKit.wasm",
        "name": "BlazingStory.ToolKit.y1yhqzrdo6.wasm",
        "hash": "sha256-WoN0z/s1uVTpgMQ3qzT2w7TNijmW0qHt0YVDSf69Fe8=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 24144,
        "virtualPath": "MyBlazorWasmApp1.wasm",
        "name": "MyBlazorWasmApp1.ryd2tkjrmb.wasm",
        "hash": "sha256-M7Aueh6TMD417LFX9RkURKzeU3UgcGZdQ6VAIQzi568=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 32336,
        "virtualPath": "MyBlazorWasmApp1.Stories.wasm",
        "name": "MyBlazorWasmApp1.Stories.9sd3vygfe7.wasm",
        "hash": "sha256-iW+nkYyv3MIrsf3KEddqY+qp4BzKb0yR4UgtnWJjcUM=",
        "cache": "force-cache"
      }
    ],
    "libraryInitializers": [
      {
        "name": "_content/BlazingStory/BlazingStory.ej9xdi73o4.lib.module.js"
      }
    ],
    "modulesAfterConfigLoaded": [
      {
        "name": "../_content/BlazingStory/BlazingStory.ej9xdi73o4.lib.module.js"
      }
    ]
  },
  "debugLevel": 0,
  "globalizationMode": "invariant",
  "extensions": {
    "blazor": {}
  },
  "runtimeConfig": {
    "runtimeOptions": {
      "configProperties": {
        "Microsoft.AspNetCore.Components.Routing.RegexConstraintSupport": false,
        "Toolbelt.Blazor.SplitContainer.JavaScriptCacheBusting": true,
        "Toolbelt.Blazor.HotKeys2.OptimizeForWasm": true,
        "Toolbelt.Blazor.HotKeys2.JavaScriptCacheBusting": true,
        "System.Diagnostics.Debugger.IsSupported": false,
        "System.Diagnostics.Metrics.Meter.IsSupported": true,
        "System.Diagnostics.Tracing.EventSource.IsSupported": true,
        "System.GC.Server": true,
        "System.Globalization.Invariant": true,
        "System.TimeZoneInfo.Invariant": true,
        "System.Globalization.PredefinedCulturesOnly": true,
        "System.Linq.Enumerable.IsSizeOptimized": true,
        "System.Net.Http.EnableActivityPropagation": true,
        "System.Net.Http.WasmEnableStreamingResponse": true,
        "System.Net.SocketsHttpHandler.Http3Support": false,
        "System.Reflection.Metadata.MetadataUpdater.IsSupported": false,
        "System.Resources.UseSystemResourceKeys": true,
        "System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization": false,
        "System.Text.Encoding.EnableUnsafeUTF7Encoding": false,
        "System.Text.Json.JsonSerializer.IsReflectionEnabledByDefault": true,
        "System.Diagnostics.StackTrace.IsLineNumberSupported": false,
        "System.Runtime.CompilerServices.RuntimeFeature.IsMultithreadingSupported": false
      }
    }
  }
}/*json-end*/);export{po as default,mo as dotnet,go as exit};
