//! Licensed to the .NET Foundation under one or more agreements.
//! The .NET Foundation licenses this file to you under the MIT license.

var e=!1;const t=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,4,1,96,0,0,3,2,1,0,10,8,1,6,0,6,64,25,11,11])),o=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,5,1,96,0,1,123,3,2,1,0,10,15,1,13,0,65,1,253,15,65,2,253,15,253,128,2,11])),n=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,5,1,96,0,1,123,3,2,1,0,10,10,1,8,0,65,0,253,15,253,98,11])),r=Symbol.for("wasm promise_control");function i(e,t){let o=null;const n=new Promise((function(n,r){o={isDone:!1,promise:null,resolve:t=>{o.isDone||(o.isDone=!0,n(t),e&&e())},reject:e=>{o.isDone||(o.isDone=!0,r(e),t&&t())}}}));o.promise=n;const i=n;return i[r]=o,{promise:i,promise_control:o}}function s(e){return e[r]}function a(e){e&&function(e){return void 0!==e[r]}(e)||Be(!1,"Promise is not controllable")}const l="__mono_message__",c=["debug","log","trace","warn","info","error"],d="MONO_WASM: ";let u,f,m,g,p,h;function w(e){g=e}function b(e){if(Pe.diagnosticTracing){const t="function"==typeof e?e():e;console.debug(d+t)}}function y(e,...t){console.info(d+e,...t)}function v(e,...t){console.info(e,...t)}function E(e,...t){console.warn(d+e,...t)}function _(e,...t){if(t&&t.length>0&&t[0]&&"object"==typeof t[0]){if(t[0].silent)return;if(t[0].toString)return void console.error(d+e,t[0].toString())}console.error(d+e,...t)}function x(e,t,o){return function(...n){try{let r=n[0];if(void 0===r)r="undefined";else if(null===r)r="null";else if("function"==typeof r)r=r.toString();else if("string"!=typeof r)try{r=JSON.stringify(r)}catch(e){r=r.toString()}t(o?JSON.stringify({method:e,payload:r,arguments:n.slice(1)}):[e+r,...n.slice(1)])}catch(e){m.error(`proxyConsole failed: ${e}`)}}}function j(e,t,o){f=t,g=e,m={...t};const n=`${o}/console`.replace("https://","wss://").replace("http://","ws://");u=new WebSocket(n),u.addEventListener("error",A),u.addEventListener("close",S),function(){for(const e of c)f[e]=x(`console.${e}`,T,!0)}()}function R(e){let t=30;const o=()=>{u?0==u.bufferedAmount||0==t?(e&&v(e),function(){for(const e of c)f[e]=x(`console.${e}`,m.log,!1)}(),u.removeEventListener("error",A),u.removeEventListener("close",S),u.close(1e3,e),u=void 0):(t--,globalThis.setTimeout(o,100)):e&&m&&m.log(e)};o()}function T(e){u&&u.readyState===WebSocket.OPEN?u.send(e):m.log(e)}function A(e){m.error(`[${g}] proxy console websocket error: ${e}`,e)}function S(e){m.debug(`[${g}] proxy console websocket closed: ${e}`,e)}function D(){Pe.preferredIcuAsset=O(Pe.config);let e="invariant"==Pe.config.globalizationMode;if(!e)if(Pe.preferredIcuAsset)Pe.diagnosticTracing&&b("ICU data archive(s) available, disabling invariant mode");else{if("custom"===Pe.config.globalizationMode||"all"===Pe.config.globalizationMode||"sharded"===Pe.config.globalizationMode){const e="invariant globalization mode is inactive and no ICU data archives are available";throw _(`ERROR: ${e}`),new Error(e)}Pe.diagnosticTracing&&b("ICU data archive(s) not available, using invariant globalization mode"),e=!0,Pe.preferredIcuAsset=null}const t="DOTNET_SYSTEM_GLOBALIZATION_INVARIANT",o=Pe.config.environmentVariables;if(void 0===o[t]&&e&&(o[t]="1"),void 0===o.TZ)try{const e=Intl.DateTimeFormat().resolvedOptions().timeZone||null;e&&(o.TZ=e)}catch(e){y("failed to detect timezone, will fallback to UTC")}}function O(e){var t;if((null===(t=e.resources)||void 0===t?void 0:t.icu)&&"invariant"!=e.globalizationMode){const t=e.applicationCulture||(ke?globalThis.navigator&&globalThis.navigator.languages&&globalThis.navigator.languages[0]:Intl.DateTimeFormat().resolvedOptions().locale),o=e.resources.icu;let n=null;if("custom"===e.globalizationMode){if(o.length>=1)return o[0].name}else t&&"all"!==e.globalizationMode?"sharded"===e.globalizationMode&&(n=function(e){const t=e.split("-")[0];return"en"===t||["fr","fr-FR","it","it-IT","de","de-DE","es","es-ES"].includes(e)?"icudt_EFIGS.dat":["zh","ko","ja"].includes(t)?"icudt_CJK.dat":"icudt_no_CJK.dat"}(t)):n="icudt.dat";if(n)for(let e=0;e<o.length;e++){const t=o[e];if(t.virtualPath===n)return t.name}}return e.globalizationMode="invariant",null}(new Date).valueOf();const C=class{constructor(e){this.url=e}toString(){return this.url}};async function k(e,t){try{const o="function"==typeof globalThis.fetch;if(Se){const n=e.startsWith("file://");if(!n&&o)return globalThis.fetch(e,t||{credentials:"same-origin"});p||(h=Ne.require("url"),p=Ne.require("fs")),n&&(e=h.fileURLToPath(e));const r=await p.promises.readFile(e);return{ok:!0,headers:{length:0,get:()=>null},url:e,arrayBuffer:()=>r,json:()=>JSON.parse(r),text:()=>{throw new Error("NotImplementedException")}}}if(o)return globalThis.fetch(e,t||{credentials:"same-origin"});if("function"==typeof read)return{ok:!0,url:e,headers:{length:0,get:()=>null},arrayBuffer:()=>new Uint8Array(read(e,"binary")),json:()=>JSON.parse(read(e,"utf8")),text:()=>read(e,"utf8")}}catch(t){return{ok:!1,url:e,status:500,headers:{length:0,get:()=>null},statusText:"ERR28: "+t,arrayBuffer:()=>{throw t},json:()=>{throw t},text:()=>{throw t}}}throw new Error("No fetch implementation available")}function I(e){return"string"!=typeof e&&Be(!1,"url must be a string"),!M(e)&&0!==e.indexOf("./")&&0!==e.indexOf("../")&&globalThis.URL&&globalThis.document&&globalThis.document.baseURI&&(e=new URL(e,globalThis.document.baseURI).toString()),e}const U=/^[a-zA-Z][a-zA-Z\d+\-.]*?:\/\//,P=/[a-zA-Z]:[\\/]/;function M(e){return Se||Ie?e.startsWith("/")||e.startsWith("\\")||-1!==e.indexOf("///")||P.test(e):U.test(e)}let L,N=0;const $=[],z=[],W=new Map,F={"js-module-threads":!0,"js-module-runtime":!0,"js-module-dotnet":!0,"js-module-native":!0,"js-module-diagnostics":!0},B={...F,"js-module-library-initializer":!0},V={...F,dotnetwasm:!0,heap:!0,manifest:!0},q={...B,manifest:!0},H={...B,dotnetwasm:!0},J={dotnetwasm:!0,symbols:!0},Z={...B,dotnetwasm:!0,symbols:!0},Q={symbols:!0};function G(e){return!("icu"==e.behavior&&e.name!=Pe.preferredIcuAsset)}function K(e,t,o){null!=t||(t=[]),Be(1==t.length,`Expect to have one ${o} asset in resources`);const n=t[0];return n.behavior=o,X(n),e.push(n),n}function X(e){V[e.behavior]&&W.set(e.behavior,e)}function Y(e){Be(V[e],`Unknown single asset behavior ${e}`);const t=W.get(e);if(t&&!t.resolvedUrl)if(t.resolvedUrl=Pe.locateFile(t.name),F[t.behavior]){const e=ge(t);e?("string"!=typeof e&&Be(!1,"loadBootResource response for 'dotnetjs' type should be a URL string"),t.resolvedUrl=e):t.resolvedUrl=ce(t.resolvedUrl,t.behavior)}else if("dotnetwasm"!==t.behavior)throw new Error(`Unknown single asset behavior ${e}`);return t}function ee(e){const t=Y(e);return Be(t,`Single asset for ${e} not found`),t}let te=!1;async function oe(){if(!te){te=!0,Pe.diagnosticTracing&&b("mono_download_assets");try{const e=[],t=[],o=(e,t)=>{!Z[e.behavior]&&G(e)&&Pe.expected_instantiated_assets_count++,!H[e.behavior]&&G(e)&&(Pe.expected_downloaded_assets_count++,t.push(se(e)))};for(const t of $)o(t,e);for(const e of z)o(e,t);Pe.allDownloadsQueued.promise_control.resolve(),Promise.all([...e,...t]).then((()=>{Pe.allDownloadsFinished.promise_control.resolve()})).catch((e=>{throw Pe.err("Error in mono_download_assets: "+e),Xe(1,e),e})),await Pe.runtimeModuleLoaded.promise;const n=async e=>{const t=await e;if(t.buffer){if(!Z[t.behavior]){t.buffer&&"object"==typeof t.buffer||Be(!1,"asset buffer must be array-like or buffer-like or promise of these"),"string"!=typeof t.resolvedUrl&&Be(!1,"resolvedUrl must be string");const e=t.resolvedUrl,o=await t.buffer,n=new Uint8Array(o);pe(t),await Ue.beforeOnRuntimeInitialized.promise,Ue.instantiate_asset(t,e,n)}}else J[t.behavior]?("symbols"===t.behavior&&(await Ue.instantiate_symbols_asset(t),pe(t)),J[t.behavior]&&++Pe.actual_downloaded_assets_count):(t.isOptional||Be(!1,"Expected asset to have the downloaded buffer"),!H[t.behavior]&&G(t)&&Pe.expected_downloaded_assets_count--,!Z[t.behavior]&&G(t)&&Pe.expected_instantiated_assets_count--)},r=[],i=[];for(const t of e)r.push(n(t));for(const e of t)i.push(n(e));Promise.all(r).then((()=>{Ce||Ue.coreAssetsInMemory.promise_control.resolve()})).catch((e=>{throw Pe.err("Error in mono_download_assets: "+e),Xe(1,e),e})),Promise.all(i).then((async()=>{Ce||(await Ue.coreAssetsInMemory.promise,Ue.allAssetsInMemory.promise_control.resolve())})).catch((e=>{throw Pe.err("Error in mono_download_assets: "+e),Xe(1,e),e}))}catch(e){throw Pe.err("Error in mono_download_assets: "+e),e}}}let ne=!1;function re(){if(ne)return;ne=!0;const e=Pe.config,t=[];if(e.assets)for(const t of e.assets)"object"!=typeof t&&Be(!1,`asset must be object, it was ${typeof t} : ${t}`),"string"!=typeof t.behavior&&Be(!1,"asset behavior must be known string"),"string"!=typeof t.name&&Be(!1,"asset name must be string"),t.resolvedUrl&&"string"!=typeof t.resolvedUrl&&Be(!1,"asset resolvedUrl could be string"),t.hash&&"string"!=typeof t.hash&&Be(!1,"asset resolvedUrl could be string"),t.pendingDownload&&"object"!=typeof t.pendingDownload&&Be(!1,"asset pendingDownload could be object"),t.isCore?$.push(t):z.push(t),X(t);else if(e.resources){const o=e.resources;o.wasmNative||Be(!1,"resources.wasmNative must be defined"),o.jsModuleNative||Be(!1,"resources.jsModuleNative must be defined"),o.jsModuleRuntime||Be(!1,"resources.jsModuleRuntime must be defined"),K(z,o.wasmNative,"dotnetwasm"),K(t,o.jsModuleNative,"js-module-native"),K(t,o.jsModuleRuntime,"js-module-runtime"),o.jsModuleDiagnostics&&K(t,o.jsModuleDiagnostics,"js-module-diagnostics");const n=(e,t,o)=>{const n=e;n.behavior=t,o?(n.isCore=!0,$.push(n)):z.push(n)};if(o.coreAssembly)for(let e=0;e<o.coreAssembly.length;e++)n(o.coreAssembly[e],"assembly",!0);if(o.assembly)for(let e=0;e<o.assembly.length;e++)n(o.assembly[e],"assembly",!o.coreAssembly);if(0!=e.debugLevel&&Pe.isDebuggingSupported()){if(o.corePdb)for(let e=0;e<o.corePdb.length;e++)n(o.corePdb[e],"pdb",!0);if(o.pdb)for(let e=0;e<o.pdb.length;e++)n(o.pdb[e],"pdb",!o.corePdb)}if(e.loadAllSatelliteResources&&o.satelliteResources)for(const e in o.satelliteResources)for(let t=0;t<o.satelliteResources[e].length;t++){const r=o.satelliteResources[e][t];r.culture=e,n(r,"resource",!o.coreAssembly)}if(o.coreVfs)for(let e=0;e<o.coreVfs.length;e++)n(o.coreVfs[e],"vfs",!0);if(o.vfs)for(let e=0;e<o.vfs.length;e++)n(o.vfs[e],"vfs",!o.coreVfs);const r=O(e);if(r&&o.icu)for(let e=0;e<o.icu.length;e++){const t=o.icu[e];t.name===r&&n(t,"icu",!1)}if(o.wasmSymbols)for(let e=0;e<o.wasmSymbols.length;e++)n(o.wasmSymbols[e],"symbols",!1)}if(e.appsettings)for(let t=0;t<e.appsettings.length;t++){const o=e.appsettings[t],n=he(o);"appsettings.json"!==n&&n!==`appsettings.${e.applicationEnvironment}.json`||z.push({name:o,behavior:"vfs",cache:"no-cache",useCredentials:!0})}e.assets=[...$,...z,...t]}async function ie(e){const t=await se(e);return await t.pendingDownloadInternal.response,t.buffer}async function se(e){try{return await ae(e)}catch(t){if(!Pe.enableDownloadRetry)throw t;if(Ie||Se)throw t;if(e.pendingDownload&&e.pendingDownloadInternal==e.pendingDownload)throw t;if(e.resolvedUrl&&-1!=e.resolvedUrl.indexOf("file://"))throw t;if(t&&404==t.status)throw t;e.pendingDownloadInternal=void 0,await Pe.allDownloadsQueued.promise;try{return Pe.diagnosticTracing&&b(`Retrying download '${e.name}'`),await ae(e)}catch(t){return e.pendingDownloadInternal=void 0,await new Promise((e=>globalThis.setTimeout(e,100))),Pe.diagnosticTracing&&b(`Retrying download (2) '${e.name}' after delay`),await ae(e)}}}async function ae(e){for(;L;)await L.promise;try{++N,N==Pe.maxParallelDownloads&&(Pe.diagnosticTracing&&b("Throttling further parallel downloads"),L=i());const t=await async function(e){if(e.pendingDownload&&(e.pendingDownloadInternal=e.pendingDownload),e.pendingDownloadInternal&&e.pendingDownloadInternal.response)return e.pendingDownloadInternal.response;if(e.buffer){const t=await e.buffer;return e.resolvedUrl||(e.resolvedUrl="undefined://"+e.name),e.pendingDownloadInternal={url:e.resolvedUrl,name:e.name,response:Promise.resolve({ok:!0,arrayBuffer:()=>t,json:()=>JSON.parse(new TextDecoder("utf-8").decode(t)),text:()=>{throw new Error("NotImplementedException")},headers:{get:()=>{}}})},e.pendingDownloadInternal.response}const t=e.loadRemote&&Pe.config.remoteSources?Pe.config.remoteSources:[""];let o;for(let n of t){n=n.trim(),"./"===n&&(n="");const t=le(e,n);e.name===t?Pe.diagnosticTracing&&b(`Attempting to download '${t}'`):Pe.diagnosticTracing&&b(`Attempting to download '${t}' for ${e.name}`);try{e.resolvedUrl=t;const n=fe(e);if(e.pendingDownloadInternal=n,o=await n.response,!o||!o.ok)continue;return o}catch(e){o||(o={ok:!1,url:t,status:0,statusText:""+e});continue}}const n=e.isOptional||e.name.match(/\.pdb$/)&&Pe.config.ignorePdbLoadErrors;if(o||Be(!1,`Response undefined ${e.name}`),!n){const t=new Error(`download '${o.url}' for ${e.name} failed ${o.status} ${o.statusText}`);throw t.status=o.status,t}y(`optional download '${o.url}' for ${e.name} failed ${o.status} ${o.statusText}`)}(e);return t?(J[e.behavior]||(e.buffer=await t.arrayBuffer(),++Pe.actual_downloaded_assets_count),e):e}finally{if(--N,L&&N==Pe.maxParallelDownloads-1){Pe.diagnosticTracing&&b("Resuming more parallel downloads");const e=L;L=void 0,e.promise_control.resolve()}}}function le(e,t){let o;return null==t&&Be(!1,`sourcePrefix must be provided for ${e.name}`),e.resolvedUrl?o=e.resolvedUrl:(o=""===t?"assembly"===e.behavior||"pdb"===e.behavior?e.name:"resource"===e.behavior&&e.culture&&""!==e.culture?`${e.culture}/${e.name}`:e.name:t+e.name,o=ce(Pe.locateFile(o),e.behavior)),o&&"string"==typeof o||Be(!1,"attemptUrl need to be path or url string"),o}function ce(e,t){return Pe.modulesUniqueQuery&&q[t]&&(e+=Pe.modulesUniqueQuery),e}let de=0;const ue=new Set;function fe(e){try{e.resolvedUrl||Be(!1,"Request's resolvedUrl must be set");const t=function(e){let t=e.resolvedUrl;if(Pe.loadBootResource){const o=ge(e);if(o instanceof Promise)return o;"string"==typeof o&&(t=o)}const o={};return e.cache?o.cache=e.cache:Pe.config.disableNoCacheFetch||(o.cache="no-cache"),e.useCredentials?o.credentials="include":!Pe.config.disableIntegrityCheck&&e.hash&&(o.integrity=e.hash),Pe.fetch_like(t,o)}(e),o={name:e.name,url:e.resolvedUrl,response:t};return ue.add(e.name),o.response.then((()=>{"assembly"==e.behavior&&Pe.loadedAssemblies.push(e.name),de++,Pe.onDownloadResourceProgress&&Pe.onDownloadResourceProgress(de,ue.size)})),o}catch(t){const o={ok:!1,url:e.resolvedUrl,status:500,statusText:"ERR29: "+t,arrayBuffer:()=>{throw t},json:()=>{throw t}};return{name:e.name,url:e.resolvedUrl,response:Promise.resolve(o)}}}const me={resource:"assembly",assembly:"assembly",pdb:"pdb",icu:"globalization",vfs:"configuration",manifest:"manifest",dotnetwasm:"dotnetwasm","js-module-dotnet":"dotnetjs","js-module-native":"dotnetjs","js-module-runtime":"dotnetjs","js-module-threads":"dotnetjs"};function ge(e){var t;if(Pe.loadBootResource){const o=null!==(t=e.hash)&&void 0!==t?t:"",n=e.resolvedUrl,r=me[e.behavior];if(r){const t=Pe.loadBootResource(r,e.name,n,o,e.behavior);return"string"==typeof t?I(t):t}}}function pe(e){e.pendingDownloadInternal=null,e.pendingDownload=null,e.buffer=null,e.moduleExports=null}function he(e){let t=e.lastIndexOf("/");return t>=0&&t++,e.substring(t)}async function we(e){e&&await Promise.all((null!=e?e:[]).map((e=>async function(e){try{const t=e.name;if(!e.moduleExports){const o=ce(Pe.locateFile(t),"js-module-library-initializer");Pe.diagnosticTracing&&b(`Attempting to import '${o}' for ${e}`),e.moduleExports=await import(/*! webpackIgnore: true */o)}Pe.libraryInitializers.push({scriptName:t,exports:e.moduleExports})}catch(t){E(`Failed to import library initializer '${e}': ${t}`)}}(e))))}async function be(e,t){if(!Pe.libraryInitializers)return;const o=[];for(let n=0;n<Pe.libraryInitializers.length;n++){const r=Pe.libraryInitializers[n];r.exports[e]&&o.push(ye(r.scriptName,e,(()=>r.exports[e](...t))))}await Promise.all(o)}async function ye(e,t,o){try{await o()}catch(o){throw E(`Failed to invoke '${t}' on library initializer '${e}': ${o}`),Xe(1,o),o}}function ve(e,t){if(e===t)return e;const o={...t};return void 0!==o.assets&&o.assets!==e.assets&&(o.assets=[...e.assets||[],...o.assets||[]]),void 0!==o.resources&&(o.resources=_e(e.resources||{assembly:[],jsModuleNative:[],jsModuleRuntime:[],wasmNative:[]},o.resources)),void 0!==o.environmentVariables&&(o.environmentVariables={...e.environmentVariables||{},...o.environmentVariables||{}}),void 0!==o.runtimeOptions&&o.runtimeOptions!==e.runtimeOptions&&(o.runtimeOptions=[...e.runtimeOptions||[],...o.runtimeOptions||[]]),Object.assign(e,o)}function Ee(e,t){if(e===t)return e;const o={...t};return o.config&&(e.config||(e.config={}),o.config=ve(e.config,o.config)),Object.assign(e,o)}function _e(e,t){if(e===t)return e;const o={...t};return void 0!==o.coreAssembly&&(o.coreAssembly=[...e.coreAssembly||[],...o.coreAssembly||[]]),void 0!==o.assembly&&(o.assembly=[...e.assembly||[],...o.assembly||[]]),void 0!==o.lazyAssembly&&(o.lazyAssembly=[...e.lazyAssembly||[],...o.lazyAssembly||[]]),void 0!==o.corePdb&&(o.corePdb=[...e.corePdb||[],...o.corePdb||[]]),void 0!==o.pdb&&(o.pdb=[...e.pdb||[],...o.pdb||[]]),void 0!==o.jsModuleWorker&&(o.jsModuleWorker=[...e.jsModuleWorker||[],...o.jsModuleWorker||[]]),void 0!==o.jsModuleNative&&(o.jsModuleNative=[...e.jsModuleNative||[],...o.jsModuleNative||[]]),void 0!==o.jsModuleDiagnostics&&(o.jsModuleDiagnostics=[...e.jsModuleDiagnostics||[],...o.jsModuleDiagnostics||[]]),void 0!==o.jsModuleRuntime&&(o.jsModuleRuntime=[...e.jsModuleRuntime||[],...o.jsModuleRuntime||[]]),void 0!==o.wasmSymbols&&(o.wasmSymbols=[...e.wasmSymbols||[],...o.wasmSymbols||[]]),void 0!==o.wasmNative&&(o.wasmNative=[...e.wasmNative||[],...o.wasmNative||[]]),void 0!==o.icu&&(o.icu=[...e.icu||[],...o.icu||[]]),void 0!==o.satelliteResources&&(o.satelliteResources=function(e,t){if(e===t)return e;for(const o in t)e[o]=[...e[o]||[],...t[o]||[]];return e}(e.satelliteResources||{},o.satelliteResources||{})),void 0!==o.modulesAfterConfigLoaded&&(o.modulesAfterConfigLoaded=[...e.modulesAfterConfigLoaded||[],...o.modulesAfterConfigLoaded||[]]),void 0!==o.modulesAfterRuntimeReady&&(o.modulesAfterRuntimeReady=[...e.modulesAfterRuntimeReady||[],...o.modulesAfterRuntimeReady||[]]),void 0!==o.extensions&&(o.extensions={...e.extensions||{},...o.extensions||{}}),void 0!==o.vfs&&(o.vfs=[...e.vfs||[],...o.vfs||[]]),Object.assign(e,o)}function xe(){const e=Pe.config;if(e.environmentVariables=e.environmentVariables||{},e.runtimeOptions=e.runtimeOptions||[],e.resources=e.resources||{assembly:[],jsModuleNative:[],jsModuleWorker:[],jsModuleRuntime:[],wasmNative:[],vfs:[],satelliteResources:{}},e.assets){Pe.diagnosticTracing&&b("config.assets is deprecated, use config.resources instead");for(const t of e.assets){const o={};switch(t.behavior){case"assembly":o.assembly=[t];break;case"pdb":o.pdb=[t];break;case"resource":o.satelliteResources={},o.satelliteResources[t.culture]=[t];break;case"icu":o.icu=[t];break;case"symbols":o.wasmSymbols=[t];break;case"vfs":o.vfs=[t];break;case"dotnetwasm":o.wasmNative=[t];break;case"js-module-threads":o.jsModuleWorker=[t];break;case"js-module-runtime":o.jsModuleRuntime=[t];break;case"js-module-native":o.jsModuleNative=[t];break;case"js-module-diagnostics":o.jsModuleDiagnostics=[t];break;case"js-module-dotnet":break;default:throw new Error(`Unexpected behavior ${t.behavior} of asset ${t.name}`)}_e(e.resources,o)}}e.debugLevel,e.applicationEnvironment||(e.applicationEnvironment="Production"),e.applicationCulture&&(e.environmentVariables.LANG=`${e.applicationCulture}.UTF-8`),Ue.diagnosticTracing=Pe.diagnosticTracing=!!e.diagnosticTracing,Ue.waitForDebugger=e.waitForDebugger,Pe.maxParallelDownloads=e.maxParallelDownloads||Pe.maxParallelDownloads,Pe.enableDownloadRetry=void 0!==e.enableDownloadRetry?e.enableDownloadRetry:Pe.enableDownloadRetry}let je=!1;async function Re(e){var t;if(je)return void await Pe.afterConfigLoaded.promise;let o;try{if(e.configSrc||Pe.config&&0!==Object.keys(Pe.config).length&&(Pe.config.assets||Pe.config.resources)||(e.configSrc="dotnet.boot.js"),o=e.configSrc,je=!0,o&&(Pe.diagnosticTracing&&b("mono_wasm_load_config"),await async function(e){const t=e.configSrc,o=Pe.locateFile(t);let n=null;void 0!==Pe.loadBootResource&&(n=Pe.loadBootResource("manifest",t,o,"","manifest"));let r,i=null;if(n)if("string"==typeof n)n.includes(".json")?(i=await s(I(n)),r=await Ae(i)):r=(await import(I(n))).config;else{const e=await n;"function"==typeof e.json?(i=e,r=await Ae(i)):r=e.config}else o.includes(".json")?(i=await s(ce(o,"manifest")),r=await Ae(i)):r=(await import(ce(o,"manifest"))).config;function s(e){return Pe.fetch_like(e,{method:"GET",credentials:"include",cache:"no-cache"})}Pe.config.applicationEnvironment&&(r.applicationEnvironment=Pe.config.applicationEnvironment),ve(Pe.config,r)}(e)),xe(),await we(null===(t=Pe.config.resources)||void 0===t?void 0:t.modulesAfterConfigLoaded),await be("onRuntimeConfigLoaded",[Pe.config]),e.onConfigLoaded)try{await e.onConfigLoaded(Pe.config,Le),xe()}catch(e){throw _("onConfigLoaded() failed",e),e}xe(),Pe.afterConfigLoaded.promise_control.resolve(Pe.config)}catch(t){const n=`Failed to load config file ${o} ${t} ${null==t?void 0:t.stack}`;throw Pe.config=e.config=Object.assign(Pe.config,{message:n,error:t,isError:!0}),Xe(1,new Error(n)),t}}function Te(){return!!globalThis.navigator&&(Pe.isChromium||Pe.isFirefox)}async function Ae(e){const t=Pe.config,o=await e.json();t.applicationEnvironment||o.applicationEnvironment||(o.applicationEnvironment=e.headers.get("Blazor-Environment")||e.headers.get("DotNet-Environment")||void 0),o.environmentVariables||(o.environmentVariables={});const n=e.headers.get("DOTNET-MODIFIABLE-ASSEMBLIES");n&&(o.environmentVariables.DOTNET_MODIFIABLE_ASSEMBLIES=n);const r=e.headers.get("ASPNETCORE-BROWSER-TOOLS");return r&&(o.environmentVariables.__ASPNETCORE_BROWSER_TOOLS=r),o}"function"!=typeof importScripts||globalThis.onmessage||(globalThis.dotnetSidecar=!0);const Se="object"==typeof process&&"object"==typeof process.versions&&"string"==typeof process.versions.node,De="function"==typeof importScripts,Oe=De&&"undefined"!=typeof dotnetSidecar,Ce=De&&!Oe,ke="object"==typeof window||De&&!Se,Ie=!ke&&!Se;let Ue={},Pe={},Me={},Le={},Ne={},$e=!1;const ze={},We={config:ze},Fe={mono:{},binding:{},internal:Ne,module:We,loaderHelpers:Pe,runtimeHelpers:Ue,diagnosticHelpers:Me,api:Le};function Be(e,t){if(e)return;const o="Assert failed: "+("function"==typeof t?t():t),n=new Error(o);_(o,n),Ue.nativeAbort(n)}function Ve(){return void 0!==Pe.exitCode}function qe(){return Ue.runtimeReady&&!Ve()}function He(){Ve()&&Be(!1,`.NET runtime already exited with ${Pe.exitCode} ${Pe.exitReason}. You can use runtime.runMain() which doesn't exit the runtime.`),Ue.runtimeReady||Be(!1,".NET runtime didn't start yet. Please call dotnet.create() first.")}function Je(){ke&&(globalThis.addEventListener("unhandledrejection",et),globalThis.addEventListener("error",tt))}let Ze,Qe;function Ge(e){Qe&&Qe(e),Xe(e,Pe.exitReason)}function Ke(e){Ze&&Ze(e||Pe.exitReason),Xe(1,e||Pe.exitReason)}function Xe(t,o){var n,r;const i=o&&"object"==typeof o;t=i&&"number"==typeof o.status?o.status:void 0===t?-1:t;const s=i&&"string"==typeof o.message?o.message:""+o;(o=i?o:Ue.ExitStatus?function(e,t){const o=new Ue.ExitStatus(e);return o.message=t,o.toString=()=>t,o}(t,s):new Error("Exit with code "+t+" "+s)).status=t,o.message||(o.message=s);const a=""+(o.stack||(new Error).stack);try{Object.defineProperty(o,"stack",{get:()=>a})}catch(e){}const l=!!o.silent;if(o.silent=!0,Ve())Pe.diagnosticTracing&&b("mono_exit called after exit");else{try{We.onAbort==Ke&&(We.onAbort=Ze),We.onExit==Ge&&(We.onExit=Qe),ke&&(globalThis.removeEventListener("unhandledrejection",et),globalThis.removeEventListener("error",tt)),Ue.runtimeReady?(Ue.jiterpreter_dump_stats&&Ue.jiterpreter_dump_stats(!1),0===t&&(null===(n=Pe.config)||void 0===n?void 0:n.interopCleanupOnExit)&&Ue.forceDisposeProxies(!0,!0),e&&0!==t&&(null===(r=Pe.config)||void 0===r||r.dumpThreadsOnNonZeroExit)):(Pe.diagnosticTracing&&b(`abort_startup, reason: ${o}`),function(e){Pe.allDownloadsQueued.promise_control.reject(e),Pe.allDownloadsFinished.promise_control.reject(e),Pe.afterConfigLoaded.promise_control.reject(e),Pe.wasmCompilePromise.promise_control.reject(e),Pe.runtimeModuleLoaded.promise_control.reject(e),Ue.dotnetReady&&(Ue.dotnetReady.promise_control.reject(e),Ue.afterInstantiateWasm.promise_control.reject(e),Ue.beforePreInit.promise_control.reject(e),Ue.afterPreInit.promise_control.reject(e),Ue.afterPreRun.promise_control.reject(e),Ue.beforeOnRuntimeInitialized.promise_control.reject(e),Ue.afterOnRuntimeInitialized.promise_control.reject(e),Ue.afterPostRun.promise_control.reject(e))}(o))}catch(e){E("mono_exit A failed",e)}try{l||(function(e,t){if(0!==e&&t){const e=Ue.ExitStatus&&t instanceof Ue.ExitStatus?b:_;"string"==typeof t?e(t):(void 0===t.stack&&(t.stack=(new Error).stack+""),t.message?e(Ue.stringify_as_error_with_stack?Ue.stringify_as_error_with_stack(t.message+"\n"+t.stack):t.message+"\n"+t.stack):e(JSON.stringify(t)))}!Ce&&Pe.config&&(Pe.config.logExitCode?Pe.config.forwardConsoleLogsToWS?R("WASM EXIT "+e):v("WASM EXIT "+e):Pe.config.forwardConsoleLogsToWS&&R())}(t,o),function(e){if(ke&&!Ce&&Pe.config&&Pe.config.appendElementOnExit&&document){const t=document.createElement("label");t.id="tests_done",0!==e&&(t.style.background="red"),t.innerHTML=""+e,document.body.appendChild(t)}}(t))}catch(e){E("mono_exit B failed",e)}Pe.exitCode=t,Pe.exitReason||(Pe.exitReason=o),!Ce&&Ue.runtimeReady&&We.runtimeKeepalivePop()}if(Pe.config&&Pe.config.asyncFlushOnExit&&0===t)throw(async()=>{try{await async function(){try{const e=await import(/*! webpackIgnore: true */"process"),t=e=>new Promise(((t,o)=>{e.on("error",o),e.end("","utf8",t)})),o=t(e.stderr),n=t(e.stdout);let r;const i=new Promise((e=>{r=setTimeout((()=>e("timeout")),1e3)}));await Promise.race([Promise.all([n,o]),i]),clearTimeout(r)}catch(e){_(`flushing std* streams failed: ${e}`)}}()}finally{Ye(t,o)}})(),o;Ye(t,o)}function Ye(e,t){if(Ue.runtimeReady&&Ue.nativeExit)try{Ue.nativeExit(e)}catch(e){!Ue.ExitStatus||e instanceof Ue.ExitStatus||E("set_exit_code_and_quit_now failed: "+e.toString())}if(0!==e||!ke)throw Se&&Ne.process?Ne.process.exit(e):Ue.quit&&Ue.quit(e,t),t}function et(e){ot(e,e.reason,"rejection")}function tt(e){ot(e,e.error,"error")}function ot(e,t,o){e.preventDefault();try{t||(t=new Error("Unhandled "+o)),void 0===t.stack&&(t.stack=(new Error).stack),t.stack=t.stack+"",t.silent||(_("Unhandled error:",t),Xe(1,t))}catch(e){}}!function(e){if($e)throw new Error("Loader module already loaded");$e=!0,Ue=e.runtimeHelpers,Pe=e.loaderHelpers,Me=e.diagnosticHelpers,Le=e.api,Ne=e.internal,Object.assign(Le,{INTERNAL:Ne,invokeLibraryInitializers:be}),Object.assign(e.module,{config:ve(ze,{environmentVariables:{}})});const r={mono_wasm_bindings_is_ready:!1,config:e.module.config,diagnosticTracing:!1,nativeAbort:e=>{throw e||new Error("abort")},nativeExit:e=>{throw new Error("exit:"+e)}},l={gitHash:"95017c711e6afc1085133d440e42b4bd78155701",config:e.module.config,diagnosticTracing:!1,maxParallelDownloads:16,enableDownloadRetry:!0,_loaded_files:[],loadedFiles:[],loadedAssemblies:[],libraryInitializers:[],workerNextNumber:1,actual_downloaded_assets_count:0,actual_instantiated_assets_count:0,expected_downloaded_assets_count:0,expected_instantiated_assets_count:0,afterConfigLoaded:i(),allDownloadsQueued:i(),allDownloadsFinished:i(),wasmCompilePromise:i(),runtimeModuleLoaded:i(),loadingWorkers:i(),is_exited:Ve,is_runtime_running:qe,assert_runtime_running:He,mono_exit:Xe,createPromiseController:i,getPromiseController:s,assertIsControllablePromise:a,mono_download_assets:oe,resolve_single_asset_path:ee,setup_proxy_console:j,set_thread_prefix:w,installUnhandledErrorHandler:Je,retrieve_asset_download:ie,invokeLibraryInitializers:be,isDebuggingSupported:Te,exceptions:t,simd:n,relaxedSimd:o};Object.assign(Ue,r),Object.assign(Pe,l)}(Fe);let nt,rt,it,st=!1,at=!1;async function lt(e){if(!at){if(at=!0,ke&&Pe.config.forwardConsoleLogsToWS&&void 0!==globalThis.WebSocket&&j("main",globalThis.console,globalThis.location.origin),We||Be(!1,"Null moduleConfig"),Pe.config||Be(!1,"Null moduleConfig.config"),"function"==typeof e){const t=e(Fe.api);if(t.ready)throw new Error("Module.ready couldn't be redefined.");Object.assign(We,t),Ee(We,t)}else{if("object"!=typeof e)throw new Error("Can't use moduleFactory callback of createDotnetRuntime function.");Ee(We,e)}await async function(e){if(Se){const e=await import(/*! webpackIgnore: true */"process"),t=14;if(e.versions.node.split(".")[0]<t)throw new Error(`NodeJS at '${e.execPath}' has too low version '${e.versions.node}', please use at least ${t}. See also https://aka.ms/dotnet-wasm-features`)}const t=/*! webpackIgnore: true */import.meta.url,o=t.indexOf("?");var n;if(o>0&&(Pe.modulesUniqueQuery=t.substring(o)),Pe.scriptUrl=t.replace(/\\/g,"/").replace(/[?#].*/,""),Pe.scriptDirectory=(n=Pe.scriptUrl).slice(0,n.lastIndexOf("/"))+"/",Pe.locateFile=e=>"URL"in globalThis&&globalThis.URL!==C?new URL(e,Pe.scriptDirectory).toString():M(e)?e:Pe.scriptDirectory+e,Pe.fetch_like=k,Pe.out=console.log,Pe.err=console.error,Pe.onDownloadResourceProgress=e.onDownloadResourceProgress,ke&&globalThis.navigator){const e=globalThis.navigator,t=e.userAgentData&&e.userAgentData.brands;t&&t.length>0?Pe.isChromium=t.some((e=>"Google Chrome"===e.brand||"Microsoft Edge"===e.brand||"Chromium"===e.brand)):e.userAgent&&(Pe.isChromium=e.userAgent.includes("Chrome"),Pe.isFirefox=e.userAgent.includes("Firefox"))}Ne.require=Se?await import(/*! webpackIgnore: true */"module").then((e=>e.createRequire(/*! webpackIgnore: true */import.meta.url))):Promise.resolve((()=>{throw new Error("require not supported")})),void 0===globalThis.URL&&(globalThis.URL=C)}(We)}}async function ct(e){return await lt(e),Ze=We.onAbort,Qe=We.onExit,We.onAbort=Ke,We.onExit=Ge,We.ENVIRONMENT_IS_PTHREAD?async function(){(function(){const e=new MessageChannel,t=e.port1,o=e.port2;t.addEventListener("message",(e=>{var n,r;n=JSON.parse(e.data.config),r=JSON.parse(e.data.monoThreadInfo),st?Pe.diagnosticTracing&&b("mono config already received"):(ve(Pe.config,n),Ue.monoThreadInfo=r,xe(),Pe.diagnosticTracing&&b("mono config received"),st=!0,Pe.afterConfigLoaded.promise_control.resolve(Pe.config),ke&&n.forwardConsoleLogsToWS&&void 0!==globalThis.WebSocket&&Pe.setup_proxy_console("worker-idle",console,globalThis.location.origin)),t.close(),o.close()}),{once:!0}),t.start(),self.postMessage({[l]:{monoCmd:"preload",port:o}},[o])})(),await Pe.afterConfigLoaded.promise,function(){const e=Pe.config;e.assets||Be(!1,"config.assets must be defined");for(const t of e.assets)X(t),Q[t.behavior]&&z.push(t)}(),setTimeout((async()=>{try{await oe()}catch(e){Xe(1,e)}}),0);const e=dt(),t=await Promise.all(e);return await ut(t),We}():async function(){var e;await Re(We),re();const t=dt();(async function(){try{const e=ee("dotnetwasm");await se(e),e&&e.pendingDownloadInternal&&e.pendingDownloadInternal.response||Be(!1,"Can't load dotnet.native.wasm");const t=await e.pendingDownloadInternal.response,o=t.headers&&t.headers.get?t.headers.get("Content-Type"):void 0;let n;if("function"==typeof WebAssembly.compileStreaming&&"application/wasm"===o)n=await WebAssembly.compileStreaming(t);else{ke&&"application/wasm"!==o&&E('WebAssembly resource does not have the expected content type "application/wasm", so falling back to slower ArrayBuffer instantiation.');const e=await t.arrayBuffer();Pe.diagnosticTracing&&b("instantiate_wasm_module buffered"),n=Ie?await Promise.resolve(new WebAssembly.Module(e)):await WebAssembly.compile(e)}e.pendingDownloadInternal=null,e.pendingDownload=null,e.buffer=null,e.moduleExports=null,Pe.wasmCompilePromise.promise_control.resolve(n)}catch(e){Pe.wasmCompilePromise.promise_control.reject(e)}})(),setTimeout((async()=>{try{D(),await oe()}catch(e){Xe(1,e)}}),0);const o=await Promise.all(t);return await ut(o),await Ue.dotnetReady.promise,await we(null===(e=Pe.config.resources)||void 0===e?void 0:e.modulesAfterRuntimeReady),await be("onRuntimeReady",[Fe.api]),Le}()}function dt(){const e=ee("js-module-runtime"),t=ee("js-module-native");if(nt&&rt)return[nt,rt,it];"object"==typeof e.moduleExports?nt=e.moduleExports:(Pe.diagnosticTracing&&b(`Attempting to import '${e.resolvedUrl}' for ${e.name}`),nt=import(/*! webpackIgnore: true */e.resolvedUrl)),"object"==typeof t.moduleExports?rt=t.moduleExports:(Pe.diagnosticTracing&&b(`Attempting to import '${t.resolvedUrl}' for ${t.name}`),rt=import(/*! webpackIgnore: true */t.resolvedUrl));const o=Y("js-module-diagnostics");return o&&("object"==typeof o.moduleExports?it=o.moduleExports:(Pe.diagnosticTracing&&b(`Attempting to import '${o.resolvedUrl}' for ${o.name}`),it=import(/*! webpackIgnore: true */o.resolvedUrl))),[nt,rt,it]}async function ut(e){const{initializeExports:t,initializeReplacements:o,configureRuntimeStartup:n,configureEmscriptenStartup:r,configureWorkerStartup:i,setRuntimeGlobals:s,passEmscriptenInternals:a}=e[0],{default:l}=e[1],c=e[2];s(Fe),t(Fe),c&&c.setRuntimeGlobals(Fe),await n(We),Pe.runtimeModuleLoaded.promise_control.resolve(),l((e=>(Object.assign(We,{ready:e.ready,__dotnet_runtime:{initializeReplacements:o,configureEmscriptenStartup:r,configureWorkerStartup:i,passEmscriptenInternals:a}}),We))).catch((e=>{if(e.message&&e.message.toLowerCase().includes("out of memory"))throw new Error(".NET runtime has failed to start, because too much memory was requested. Please decrease the memory by adjusting EmccMaximumHeapSize. See also https://aka.ms/dotnet-wasm-features");throw e}))}const ft=new class{withModuleConfig(e){try{return Ee(We,e),this}catch(e){throw Xe(1,e),e}}withOnConfigLoaded(e){try{return Ee(We,{onConfigLoaded:e}),this}catch(e){throw Xe(1,e),e}}withConsoleForwarding(){try{return ve(ze,{forwardConsoleLogsToWS:!0}),this}catch(e){throw Xe(1,e),e}}withExitOnUnhandledError(){try{return ve(ze,{exitOnUnhandledError:!0}),Je(),this}catch(e){throw Xe(1,e),e}}withAsyncFlushOnExit(){try{return ve(ze,{asyncFlushOnExit:!0}),this}catch(e){throw Xe(1,e),e}}withExitCodeLogging(){try{return ve(ze,{logExitCode:!0}),this}catch(e){throw Xe(1,e),e}}withElementOnExit(){try{return ve(ze,{appendElementOnExit:!0}),this}catch(e){throw Xe(1,e),e}}withInteropCleanupOnExit(){try{return ve(ze,{interopCleanupOnExit:!0}),this}catch(e){throw Xe(1,e),e}}withDumpThreadsOnNonZeroExit(){try{return ve(ze,{dumpThreadsOnNonZeroExit:!0}),this}catch(e){throw Xe(1,e),e}}withWaitingForDebugger(e){try{return ve(ze,{waitForDebugger:e}),this}catch(e){throw Xe(1,e),e}}withInterpreterPgo(e,t){try{return ve(ze,{interpreterPgo:e,interpreterPgoSaveDelay:t}),ze.runtimeOptions?ze.runtimeOptions.push("--interp-pgo-recording"):ze.runtimeOptions=["--interp-pgo-recording"],this}catch(e){throw Xe(1,e),e}}withConfig(e){try{return ve(ze,e),this}catch(e){throw Xe(1,e),e}}withConfigSrc(e){try{return e&&"string"==typeof e||Be(!1,"must be file path or URL"),Ee(We,{configSrc:e}),this}catch(e){throw Xe(1,e),e}}withVirtualWorkingDirectory(e){try{return e&&"string"==typeof e||Be(!1,"must be directory path"),ve(ze,{virtualWorkingDirectory:e}),this}catch(e){throw Xe(1,e),e}}withEnvironmentVariable(e,t){try{const o={};return o[e]=t,ve(ze,{environmentVariables:o}),this}catch(e){throw Xe(1,e),e}}withEnvironmentVariables(e){try{return e&&"object"==typeof e||Be(!1,"must be dictionary object"),ve(ze,{environmentVariables:e}),this}catch(e){throw Xe(1,e),e}}withDiagnosticTracing(e){try{return"boolean"!=typeof e&&Be(!1,"must be boolean"),ve(ze,{diagnosticTracing:e}),this}catch(e){throw Xe(1,e),e}}withDebugging(e){try{return null!=e&&"number"==typeof e||Be(!1,"must be number"),ve(ze,{debugLevel:e}),this}catch(e){throw Xe(1,e),e}}withApplicationArguments(...e){try{return e&&Array.isArray(e)||Be(!1,"must be array of strings"),ve(ze,{applicationArguments:e}),this}catch(e){throw Xe(1,e),e}}withRuntimeOptions(e){try{return e&&Array.isArray(e)||Be(!1,"must be array of strings"),ze.runtimeOptions?ze.runtimeOptions.push(...e):ze.runtimeOptions=e,this}catch(e){throw Xe(1,e),e}}withMainAssembly(e){try{return ve(ze,{mainAssemblyName:e}),this}catch(e){throw Xe(1,e),e}}withApplicationArgumentsFromQuery(){try{if(!globalThis.window)throw new Error("Missing window to the query parameters from");if(void 0===globalThis.URLSearchParams)throw new Error("URLSearchParams is supported");const e=new URLSearchParams(globalThis.window.location.search).getAll("arg");return this.withApplicationArguments(...e)}catch(e){throw Xe(1,e),e}}withApplicationEnvironment(e){try{return ve(ze,{applicationEnvironment:e}),this}catch(e){throw Xe(1,e),e}}withApplicationCulture(e){try{return ve(ze,{applicationCulture:e}),this}catch(e){throw Xe(1,e),e}}withResourceLoader(e){try{return Pe.loadBootResource=e,this}catch(e){throw Xe(1,e),e}}async download(){try{await async function(){lt(We),await Re(We),re(),D(),oe(),await Pe.allDownloadsFinished.promise}()}catch(e){throw Xe(1,e),e}}async create(){try{return this.instance||(this.instance=await async function(){return await ct(We),Fe.api}()),this.instance}catch(e){throw Xe(1,e),e}}async run(){try{return We.config||Be(!1,"Null moduleConfig.config"),this.instance||await this.create(),this.instance.runMainAndExit()}catch(e){throw Xe(1,e),e}}},mt=Xe,gt=ct;Ie||"function"==typeof globalThis.URL||Be(!1,"This browser/engine doesn't support URL API. Please use a modern version. See also https://aka.ms/dotnet-wasm-features"),"function"!=typeof globalThis.BigInt64Array&&Be(!1,"This browser/engine doesn't support BigInt64Array API. Please use a modern version. See also https://aka.ms/dotnet-wasm-features"),ft.withConfig(/*json-start*/{
  "mainAssemblyName": "MyBlazorWasmApp1.Stories",
  "resources": {
    "hash": "sha256-KYO0he5hilj9Swifz1+8xlFoT4AeTyayYtM972LeNs0=",
    "jsModuleNative": [
      {
        "name": "dotnet.native.87xg72lh3g.js"
      }
    ],
    "jsModuleRuntime": [
      {
        "name": "dotnet.runtime.v06hirbjsv.js"
      }
    ],
    "wasmNative": [
      {
        "name": "dotnet.native.9wnhopqwq9.wasm",
        "hash": "sha256-x7AU25bhzDfuQXc2CIymy5ceaoFcZ8qj5ksloDeMfKU=",
        "cache": "force-cache"
      }
    ],
    "coreAssembly": [
      {
        "virtualPath": "System.Runtime.InteropServices.JavaScript.wasm",
        "name": "System.Runtime.InteropServices.JavaScript.9kq1vofond.wasm",
        "hash": "sha256-uQsOh1GEq2ZX0sXBxXnyoYzNMf4yeXUeLKUgb1q0fKo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Private.CoreLib.wasm",
        "name": "System.Private.CoreLib.1vy46s12u6.wasm",
        "hash": "sha256-gzDngaKE1fzrenrnFvi+7MHiHJEUQn0brQD1g3Q+hFI=",
        "cache": "force-cache"
      }
    ],
    "assembly": [
      {
        "virtualPath": "BlazingStory.wasm",
        "name": "BlazingStory.uv379txpe5.wasm",
        "hash": "sha256-lmO4h7IIJ4H8hR5l4iw5K9W0O1tT5y2tFfq7HQXiQ8k=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "BlazingStory.Abstractions.wasm",
        "name": "BlazingStory.Abstractions.7vninfrg26.wasm",
        "hash": "sha256-Uwz/DuCpTxMWVBfKmCuJwuo+Eo0JT7Bm8h3wUR8dy+c=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "BlazingStory.Addons.wasm",
        "name": "BlazingStory.Addons.delzudo7sh.wasm",
        "hash": "sha256-FcmBrn5KYUQ4pJwkN4NZzD+0gCinToIUbvZoHzuQP4s=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "BlazingStory.Addons.BuiltIns.wasm",
        "name": "BlazingStory.Addons.BuiltIns.zd70btp705.wasm",
        "hash": "sha256-mQsB+i5UGp74MEg6wTFe41oi5UYWLQ2t1P3/UCwgHB4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "BlazingStory.ToolKit.wasm",
        "name": "BlazingStory.ToolKit.4py4k8wf8c.wasm",
        "hash": "sha256-WAMH8xFMS3GMpAl576SxFBDAMq+U/eeCYbE6twZ7TSU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Authorization.wasm",
        "name": "Microsoft.AspNetCore.Authorization.k17bz9ghvf.wasm",
        "hash": "sha256-aP8WKS4T25ZYmwBxuOhGRK65QE/HGKSWYxMtXDBIYjg=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Components.wasm",
        "name": "Microsoft.AspNetCore.Components.aimal0hppb.wasm",
        "hash": "sha256-juoEIPp8RoC3PtrYH2MiQ/vyn/IGIPt+jyvP9wnRflg=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Components.Forms.wasm",
        "name": "Microsoft.AspNetCore.Components.Forms.81edhczag4.wasm",
        "hash": "sha256-TJnaKnkQYyx86CTPJc3Q5OoMgnSI6pfZGyfvAyHACsk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Components.Web.wasm",
        "name": "Microsoft.AspNetCore.Components.Web.uj2fgye3s8.wasm",
        "hash": "sha256-qjKYeTa4yLBsuVeaeQUJz0MW7ZcFapshmVcKGRNKR3k=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Components.WebAssembly.wasm",
        "name": "Microsoft.AspNetCore.Components.WebAssembly.s7qzmsk17i.wasm",
        "hash": "sha256-iEpObDNVvHySdLptzNYlU/j/g/vcHXMS3sYQ/Mj2wHM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Metadata.wasm",
        "name": "Microsoft.AspNetCore.Metadata.q2trwnoavt.wasm",
        "hash": "sha256-DljoTp3NkZPHFzzL2slIirD4RBBgZS7RGV0fQexZbNU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.wasm",
        "name": "Microsoft.Extensions.Configuration.f1h675694q.wasm",
        "hash": "sha256-PSnciWnNXMK//gr+udb0elW/q8iLCYYXWR/eTBtp2Qk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.Abstractions.wasm",
        "name": "Microsoft.Extensions.Configuration.Abstractions.0hgjfwn1mu.wasm",
        "hash": "sha256-JHlpaYwiFqUQsagYTswarGEumsuA2+B1FVlqNZlkfzo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.Binder.wasm",
        "name": "Microsoft.Extensions.Configuration.Binder.3jotnx07h9.wasm",
        "hash": "sha256-kYMVPmvUG8KS2TX3Mg+rKrWefG5VEnAfiPzaK7KlQgA=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.FileExtensions.wasm",
        "name": "Microsoft.Extensions.Configuration.FileExtensions.y1cmvestdm.wasm",
        "hash": "sha256-eqKI9GaF5EaNNBO7Jr95dtOMx+89xnNn+qOkOHlEdZ0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.Json.wasm",
        "name": "Microsoft.Extensions.Configuration.Json.yt3noczii3.wasm",
        "hash": "sha256-Juv0aOwRMRtiLu8PrW9cEuACGqPkxiPXw4iGEXb5BB4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.DependencyInjection.wasm",
        "name": "Microsoft.Extensions.DependencyInjection.6lng263cnf.wasm",
        "hash": "sha256-ltrV7AuG5saT+kD92VT0248FL6wL5t4JuEI87HjoAS0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.DependencyInjection.Abstractions.wasm",
        "name": "Microsoft.Extensions.DependencyInjection.Abstractions.0774chwaiw.wasm",
        "hash": "sha256-lBKCOBVvAMXlOis/l1+M33Cn6tKVgMg3bdHHrXj/a7Q=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Diagnostics.wasm",
        "name": "Microsoft.Extensions.Diagnostics.i4mi4tc4vw.wasm",
        "hash": "sha256-fucavrV2xP++Dn/l5sNeH6M5xt1BBtbTDZQyLuSiikg=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Diagnostics.Abstractions.wasm",
        "name": "Microsoft.Extensions.Diagnostics.Abstractions.ne0bxy880x.wasm",
        "hash": "sha256-MSDiTWDEv1a00OkP+a6/SwW6ekKoVPA746sMdaFKA8E=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.FileProviders.Abstractions.wasm",
        "name": "Microsoft.Extensions.FileProviders.Abstractions.bk6u2jnsv8.wasm",
        "hash": "sha256-u1pbzcqB3EhvmIqm2anbV87z0dkRS+nVqejWPRnvp00=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.FileProviders.Physical.wasm",
        "name": "Microsoft.Extensions.FileProviders.Physical.zosiazxghi.wasm",
        "hash": "sha256-PdgfipOod7naMzOVrKtgwvuJxcb/NNSvvEiRuTI88RM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.FileSystemGlobbing.wasm",
        "name": "Microsoft.Extensions.FileSystemGlobbing.1rsw5d4pjy.wasm",
        "hash": "sha256-jVIdwrH+D8wVEIFI2aKoHyZTPBdIK5trQpCCbYnCu8M=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Logging.wasm",
        "name": "Microsoft.Extensions.Logging.ptjwzqfpka.wasm",
        "hash": "sha256-o/gQAuPMwH+rbawuWz6xWOq1Ci5tN52xGlVFOOwBtl4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Logging.Abstractions.wasm",
        "name": "Microsoft.Extensions.Logging.Abstractions.9iwncqwny2.wasm",
        "hash": "sha256-Ufg8lS+p6OZ00WWMeQqTGKA/IS8O9sECUPEuGvM5WA4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Options.wasm",
        "name": "Microsoft.Extensions.Options.i52tw7ob6b.wasm",
        "hash": "sha256-JoxbwT+ZaOtmIRkTmwFOip/K7uoJJpFRzW56h6XkRpQ=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Options.ConfigurationExtensions.wasm",
        "name": "Microsoft.Extensions.Options.ConfigurationExtensions.3i6cpeqz4b.wasm",
        "hash": "sha256-67yI4vesQjof/F/jpJtEmH6kmQ5HDkvaYPoH2YpZiPk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Primitives.wasm",
        "name": "Microsoft.Extensions.Primitives.iq6oe4oszz.wasm",
        "hash": "sha256-5uk1If5BOfuRl+IDKfk3cqyIIvd49ThDPGjllxw4z10=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Validation.wasm",
        "name": "Microsoft.Extensions.Validation.jj5ns90kh6.wasm",
        "hash": "sha256-z9tzmlV7XqYLp+qslYfz0xzmIu+2MMvtZlq8avKoBnE=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.JSInterop.wasm",
        "name": "Microsoft.JSInterop.570ul0nip6.wasm",
        "hash": "sha256-gYELgmTOyKtRl6GD0I+cUt4ftDSyY6Ygsy5P30n3EcY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.JSInterop.WebAssembly.wasm",
        "name": "Microsoft.JSInterop.WebAssembly.wrc8eavzcp.wasm",
        "hash": "sha256-VCeDTHijW2LZG2I156kMtdK6hB6/aOgO0CadhOOUpaA=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Toolbelt.Blazor.HotKeys2.wasm",
        "name": "Toolbelt.Blazor.HotKeys2.ydv9gxwp79.wasm",
        "hash": "sha256-JnvhTx5LUtB6mfnbc8IIygQ4pApCYfFOMLpRtd9b9aQ=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Toolbelt.Blazor.SplitContainer.wasm",
        "name": "Toolbelt.Blazor.SplitContainer.i8tkqljl5s.wasm",
        "hash": "sha256-Ag+HPmIwT8JLy0oOvGaSIfQ4jBkZ8hyEG6GtXD7uXto=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Toolbelt.Web.CssClassInlineBuilder.wasm",
        "name": "Toolbelt.Web.CssClassInlineBuilder.r0k6jvyfqx.wasm",
        "hash": "sha256-21l5poJYk/1BPyr2mAfqNWLAFwQlyOuKBSrVo7sp+40=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.CSharp.wasm",
        "name": "Microsoft.CSharp.mp8lhr6rez.wasm",
        "hash": "sha256-psMoxmxraX+UqMuR6yuCJ2c/xLT3w20OO1JzVzNJ8qQ=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.VisualBasic.Core.wasm",
        "name": "Microsoft.VisualBasic.Core.17wn27qal2.wasm",
        "hash": "sha256-veBhGyorrzdMHfiRDt/VovNG125rLLrL7enX5fvvfnE=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.VisualBasic.wasm",
        "name": "Microsoft.VisualBasic.0e5fchkecn.wasm",
        "hash": "sha256-+AZA2Jpi7YedyPg/Z7OAvV4jcPpueAU4ylSeRvJDt80=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Win32.Primitives.wasm",
        "name": "Microsoft.Win32.Primitives.9d566sle9e.wasm",
        "hash": "sha256-UxlTjVzD23T3TLHwBAoQkPNd2pcDDBI/aV05iU6Dxwo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Win32.Registry.wasm",
        "name": "Microsoft.Win32.Registry.52dy96yk8i.wasm",
        "hash": "sha256-Y+jj+a+2gTHHiYlyKc94wEH41wg3Vjrdd9KSP0crV6g=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.AppContext.wasm",
        "name": "System.AppContext.jwujeh069z.wasm",
        "hash": "sha256-M1WENJWA+3wr2NvHhLXh44JWWE4VVZag928GiVWbXG0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Buffers.wasm",
        "name": "System.Buffers.y0isk1x97h.wasm",
        "hash": "sha256-QtI7D2XiD7NyCRDNW7Q5jPY5RrljqkJWOofJWLBpLOo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Collections.Concurrent.wasm",
        "name": "System.Collections.Concurrent.x5koi7bzem.wasm",
        "hash": "sha256-60VYOK8iVcWICnieMcztr96fSxmXaDwHYglhHVkfcWI=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Collections.Immutable.wasm",
        "name": "System.Collections.Immutable.diwg7advch.wasm",
        "hash": "sha256-lWe3YUN/Fc5uruvGWP/m8qaunD50OIQZlIigCznydZc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Collections.NonGeneric.wasm",
        "name": "System.Collections.NonGeneric.4089xs440e.wasm",
        "hash": "sha256-PH4kaa45WLnMxiRfMTzKCQ9u42dAwwdy99kptJLonnA=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Collections.Specialized.wasm",
        "name": "System.Collections.Specialized.hj81u7zblb.wasm",
        "hash": "sha256-g9RG+K+iaKmrK6vAd+/MRgz68IbHHpSlRyOFIY27sUI=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Collections.wasm",
        "name": "System.Collections.nvrdeoknr1.wasm",
        "hash": "sha256-nQNshHg3oKyRQXw5zLw1fEm4B4SQKVyeH2F66wBEe3o=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ComponentModel.Annotations.wasm",
        "name": "System.ComponentModel.Annotations.yatcnfjeuk.wasm",
        "hash": "sha256-WuI5nXRLzegBvcDXd12p9roPYpi8jofCeEJ6v8s63+E=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ComponentModel.DataAnnotations.wasm",
        "name": "System.ComponentModel.DataAnnotations.led9l5awu3.wasm",
        "hash": "sha256-40rhAIB8j+yJ1YvnDmQ2rPIVWKNJO6UHLv/U4oSxlBI=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ComponentModel.EventBasedAsync.wasm",
        "name": "System.ComponentModel.EventBasedAsync.hpvp6cf4de.wasm",
        "hash": "sha256-Oy0URE8zV5ayhb45ITWZbEnl6sGM9LatkYgbeAURRKM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ComponentModel.Primitives.wasm",
        "name": "System.ComponentModel.Primitives.v7cajwpaqo.wasm",
        "hash": "sha256-W9/zGt2iJxheWHJRApI7G7Z0ckE5RfuWUz4Nnwxgbq4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ComponentModel.TypeConverter.wasm",
        "name": "System.ComponentModel.TypeConverter.xk1hnyt762.wasm",
        "hash": "sha256-QfHpIsM6pT5gYELaVBtPojDrl0GNwck46q4Mu933WCo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ComponentModel.wasm",
        "name": "System.ComponentModel.rrnda1lucx.wasm",
        "hash": "sha256-aYhTsxu5l5DdQMIprD4ShbtwjTiJBH78I6wMQzBRnys=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Configuration.wasm",
        "name": "System.Configuration.bn9vghjcq8.wasm",
        "hash": "sha256-l2Yo9KFaXkgXpXguTZdnuTCytOYuPLjF8cZT0HHWwlg=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Console.wasm",
        "name": "System.Console.09ajpiafhz.wasm",
        "hash": "sha256-XKTYAvVqGguN3HteeYxjDwZqlFcFmIfpLOjNVjwXSNE=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Core.wasm",
        "name": "System.Core.wc649o5mw9.wasm",
        "hash": "sha256-0C1e8tZ9+pmcU1MPfcf8D7NNcmR2pH8LdxsDMYrVHG0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Data.Common.wasm",
        "name": "System.Data.Common.s4cowzd9a9.wasm",
        "hash": "sha256-bO/LPpxha6hSooSI6P+lazCSxTKXav6d+SSGI5xKoF8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Data.DataSetExtensions.wasm",
        "name": "System.Data.DataSetExtensions.5fm360ikxl.wasm",
        "hash": "sha256-QXrN9ZMMpWZeFTBu6yZlaw5lYx1+tUxEjhfaNEdHG4o=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Data.wasm",
        "name": "System.Data.fsu4j4t4jw.wasm",
        "hash": "sha256-ITg6VN5fCPuyFKC9ug/0vqXRoagnjGYhWj10y5heiTU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.Contracts.wasm",
        "name": "System.Diagnostics.Contracts.p74re5oxzn.wasm",
        "hash": "sha256-6+8hQYc91I+/Kqc5U6zCuNw2ConQevD1fmEvhSZFfoc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.Debug.wasm",
        "name": "System.Diagnostics.Debug.622sixg49n.wasm",
        "hash": "sha256-kpDQTiMcfbO9nP5Mja6gzL4XTOp/9ixE1CD739WQlMo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.DiagnosticSource.wasm",
        "name": "System.Diagnostics.DiagnosticSource.2j35bc2g2b.wasm",
        "hash": "sha256-HlxtCU+2k1YAqiabFqu5jOoUbMyV2ZEjNNlUEs/57NM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.FileVersionInfo.wasm",
        "name": "System.Diagnostics.FileVersionInfo.7n0wdvltaa.wasm",
        "hash": "sha256-vZsGhIqd6ATctrY/LP4iZy0cDa+gN+Vzk8mA9ecl4as=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.Process.wasm",
        "name": "System.Diagnostics.Process.8iffh49mew.wasm",
        "hash": "sha256-QE2oJ8DjLG05OdC+fLmt+YhFiUEOSwyowwQ25oZCvTw=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.StackTrace.wasm",
        "name": "System.Diagnostics.StackTrace.1yz88vico8.wasm",
        "hash": "sha256-+Wsf/lB7CDI+7OUXetsTyE+lo4TYc/SAS1aMv1yniNM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.TextWriterTraceListener.wasm",
        "name": "System.Diagnostics.TextWriterTraceListener.ua8iuh2vhj.wasm",
        "hash": "sha256-emPIv5g8xFmDr7oIZhZM/DTWGrb2Lggg7c+fjA99i5c=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.Tools.wasm",
        "name": "System.Diagnostics.Tools.roy5445ugj.wasm",
        "hash": "sha256-2yLIVwYrPkQWk0Vxh+qS+KxNElAj+Z4UTmjJNv8uYNk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.TraceSource.wasm",
        "name": "System.Diagnostics.TraceSource.6qx1ldzz7g.wasm",
        "hash": "sha256-AiYhBtmkwpOLWooLU6+KmJ0ySg6RY/ihjlCDgwQAT8E=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.Tracing.wasm",
        "name": "System.Diagnostics.Tracing.b4799zpaas.wasm",
        "hash": "sha256-S7GiSac0VcsB7GLrukNOyOx3UkyChvLMWC5lQW1nj0Q=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Drawing.Primitives.wasm",
        "name": "System.Drawing.Primitives.y2uveru2yk.wasm",
        "hash": "sha256-CiSvrtqNxb5mKdwQ7obhnTRpcvSS1e31SMHq2s6nrUc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Drawing.wasm",
        "name": "System.Drawing.8gcjjqj0pu.wasm",
        "hash": "sha256-OG93onlStdjfWzcLq3GSd7p2posHrpoBAptujpzu/ks=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Dynamic.Runtime.wasm",
        "name": "System.Dynamic.Runtime.023zrvzh9x.wasm",
        "hash": "sha256-yL5qlZRpb8jHMNsapIzA7qNxs9DZvvAJx3/hCobHnpM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Formats.Asn1.wasm",
        "name": "System.Formats.Asn1.znpklkbgqv.wasm",
        "hash": "sha256-EYyyGznhWECmsc5FFdH6k26PlZIvsOFYMbRyQHlFCas=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Formats.Tar.wasm",
        "name": "System.Formats.Tar.5yybjepfi2.wasm",
        "hash": "sha256-rcCcYOlxhVRG07eBm/7U2xhPK/cimbMV9phmtLI+B3o=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Globalization.Calendars.wasm",
        "name": "System.Globalization.Calendars.10g6hyjy3k.wasm",
        "hash": "sha256-gb8ndHQ5edkgeIuuwJAZmpSvDSqmmSPYJbBgTzel/ps=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Globalization.Extensions.wasm",
        "name": "System.Globalization.Extensions.hrinfrismg.wasm",
        "hash": "sha256-vZkkaKr0ImZEgmnHPzo/msMMeEJQCqxSo2mTbfOzRzw=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Globalization.wasm",
        "name": "System.Globalization.uvs4rah8fk.wasm",
        "hash": "sha256-xiEtuDrnECbsdmpAPAp2VzmtL1v4IfFyExR96Fud89Y=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.Compression.Brotli.wasm",
        "name": "System.IO.Compression.Brotli.fv80xs79j3.wasm",
        "hash": "sha256-643QZL7ILdpDBnNsv23Zi0Cyzz5SoTjI9wGguHqusmM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.Compression.FileSystem.wasm",
        "name": "System.IO.Compression.FileSystem.88jdddz3sw.wasm",
        "hash": "sha256-aOwoN3Ibisa2gVhxzBpktC2L4SEDgvRphJChLSaTA+Q=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.Compression.ZipFile.wasm",
        "name": "System.IO.Compression.ZipFile.jkn9avr4vo.wasm",
        "hash": "sha256-k7XPfNshrfMVIgaiuvKd377GtI7UYYnjDqJZyUkj+jc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.Compression.wasm",
        "name": "System.IO.Compression.2z7vdt6tfb.wasm",
        "hash": "sha256-Hpsq0zZpxCksmebhGlgCLpBfZlw6fRJn/pZ/QIvF8YQ=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.FileSystem.AccessControl.wasm",
        "name": "System.IO.FileSystem.AccessControl.yme0uhbu37.wasm",
        "hash": "sha256-GjwWqllU9odjGWVpAjmupGx8/yiy5lsN2Not1a1soGg=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.FileSystem.DriveInfo.wasm",
        "name": "System.IO.FileSystem.DriveInfo.qas7wf73rs.wasm",
        "hash": "sha256-/ojn2l2CNWusI2atVrRd8dbi7k4BARzLTJjRH6YgotM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.FileSystem.Primitives.wasm",
        "name": "System.IO.FileSystem.Primitives.d1jmhkzu0e.wasm",
        "hash": "sha256-H8W0uho5WxWoH8hgJZEmNbdPpPYUgD8xkWHRSKx7MMc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.FileSystem.Watcher.wasm",
        "name": "System.IO.FileSystem.Watcher.z570derp0d.wasm",
        "hash": "sha256-ucYunghHF7vLfVmQGmCY6aJsB2Kx9YKcaLYiWtXTNlY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.FileSystem.wasm",
        "name": "System.IO.FileSystem.o6jtz51fup.wasm",
        "hash": "sha256-YA3yQ7lL5H0WBYDoElU5ZeVKxPgfltC+tweV2Ybbn/A=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.IsolatedStorage.wasm",
        "name": "System.IO.IsolatedStorage.hqyeyb5ipe.wasm",
        "hash": "sha256-pxoMRl5qWZqgJSQnJ8mOHEcxOyZYWmM2Mr5oYUy+Glo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.MemoryMappedFiles.wasm",
        "name": "System.IO.MemoryMappedFiles.zhamjnusxd.wasm",
        "hash": "sha256-WZLsRujfD/qa85HQq+jH/mNvqaSC8EfcorhN7V+E+I8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.Pipelines.wasm",
        "name": "System.IO.Pipelines.wrmw5vws31.wasm",
        "hash": "sha256-TDz5qGlXb4ZW8yA27qUZMYc8j1MMcr9och2a7PScXg4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.Pipes.AccessControl.wasm",
        "name": "System.IO.Pipes.AccessControl.qcancj8gj2.wasm",
        "hash": "sha256-KsyFANWIAfdArrjBnJnX3LWpJAMzXAJi29tm3Kb5mgw=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.Pipes.wasm",
        "name": "System.IO.Pipes.zhcyguvwkr.wasm",
        "hash": "sha256-OVQT1MG8JbyAXuZTrGXlrR+u0SL2S/0PT6accLkQu54=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.UnmanagedMemoryStream.wasm",
        "name": "System.IO.UnmanagedMemoryStream.o8cpjnlard.wasm",
        "hash": "sha256-yHbYTenL8GXOuqqmxkdwREWeOUezLPi2eCSzn979C+k=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.wasm",
        "name": "System.IO.4j645gw40d.wasm",
        "hash": "sha256-8KAD71Rcb1edG/x+Cx4wUA8/0cmGUGSciPma6jeXLXo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Linq.AsyncEnumerable.wasm",
        "name": "System.Linq.AsyncEnumerable.hyc0b7okjw.wasm",
        "hash": "sha256-mSgezbBarV5SBEY+t7LObP5nQ3XgEVF6R5qAhQY90so=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Linq.Expressions.wasm",
        "name": "System.Linq.Expressions.ldibvm0efj.wasm",
        "hash": "sha256-yUg/PQ8henAChE7w0Fuf2GVEpweY2df9f20ChCg2GME=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Linq.Parallel.wasm",
        "name": "System.Linq.Parallel.2buevezvch.wasm",
        "hash": "sha256-Eggt4fURp1Dc3UevhKbp4Lj9fjdjlc8/jGqM2ifdzIs=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Linq.Queryable.wasm",
        "name": "System.Linq.Queryable.cvq43g1awj.wasm",
        "hash": "sha256-+OubGWG+/zfH+Fs0lXunuGpq8wzuc4YXIfJU2fHdzDA=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Linq.wasm",
        "name": "System.Linq.lz6mvi0hm2.wasm",
        "hash": "sha256-4ROqweguD0hFn3XZ2XC+JKzncsxZNBiX2yeKPuQs9bM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Memory.wasm",
        "name": "System.Memory.3jekerw0ga.wasm",
        "hash": "sha256-cV8BYmP+pMPC5iDRi3x21fsgc2dUObz+vFs7usZY2js=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.Http.Json.wasm",
        "name": "System.Net.Http.Json.8k237kkwec.wasm",
        "hash": "sha256-SFmib3glSdfRTQZAtBg0AFgrNS9J3dUgpsDmt9X9vJE=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.Http.wasm",
        "name": "System.Net.Http.cityx2bvcl.wasm",
        "hash": "sha256-5EKrlM9SabAClzp0n57qTFE2eaLB+jP0/z1nRz/iwQk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.HttpListener.wasm",
        "name": "System.Net.HttpListener.s1jreclykx.wasm",
        "hash": "sha256-MOeibcHl0sx5SEqNQHQhRZOR598ACQUcNnX/FVecteo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.Mail.wasm",
        "name": "System.Net.Mail.cmlj4c0gto.wasm",
        "hash": "sha256-zNeqttiCsELKrGrpy/T5NQKxD09jocn8hXJv3BcfKo4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.NameResolution.wasm",
        "name": "System.Net.NameResolution.q3gf18q13l.wasm",
        "hash": "sha256-ul/5prZbC0vGrIt6UESE+4lpJo/wXYgWcu+1+gAgc5k=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.NetworkInformation.wasm",
        "name": "System.Net.NetworkInformation.xaz5tjw7d0.wasm",
        "hash": "sha256-+WqoHrz7stgVcQGbtpi7jQqel4rJ2hx2SF0HJokwHa8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.Ping.wasm",
        "name": "System.Net.Ping.a40atim1h0.wasm",
        "hash": "sha256-5nvH8j0VIm/1nMN1+lVTI54Ri8D5ntm30A2fhB0hc/Q=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.Primitives.wasm",
        "name": "System.Net.Primitives.i88i20r8sr.wasm",
        "hash": "sha256-zhN/oiyikQbwjunDNbYt4XbUn3w/DgR7jOA8zD1phRA=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.Quic.wasm",
        "name": "System.Net.Quic.hx06eo5xkj.wasm",
        "hash": "sha256-NQhcYyf0Hr4tfihcptLj6kjDKjbD3nIU6f7VUfP+jXk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.Requests.wasm",
        "name": "System.Net.Requests.hkjxlfe5f5.wasm",
        "hash": "sha256-UdJOWmu1xm90dNiv+WiZLNoDnNaUDC8SByJPZAY/fSM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.Security.wasm",
        "name": "System.Net.Security.ch34yya69o.wasm",
        "hash": "sha256-YMvWX7YyMp6BsYKuNwDVWeeq0qr6S82HoFuZJ08E3DY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.ServerSentEvents.wasm",
        "name": "System.Net.ServerSentEvents.0u7o0jk3p1.wasm",
        "hash": "sha256-qGmAP22IUTQuqxOQHHoMv2/ULrRmNDq/8/wmKKhaui0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.ServicePoint.wasm",
        "name": "System.Net.ServicePoint.xbhxnytx4l.wasm",
        "hash": "sha256-/STSB/pmoEd7eBP1ewThAYMeJGveTDOrKftoV06TyiI=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.Sockets.wasm",
        "name": "System.Net.Sockets.hzgzlvkc9l.wasm",
        "hash": "sha256-vZEy2LgIvPJwM0p6XiZJMoT5fvBlV6yhrIf0ZiUU2Ok=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.WebClient.wasm",
        "name": "System.Net.WebClient.84i8nwzbgu.wasm",
        "hash": "sha256-SAuUMWKRzK2wStVZ46JGxoZNUEPdH9QiUK2Psd4LYcc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.WebHeaderCollection.wasm",
        "name": "System.Net.WebHeaderCollection.326nl7s9g3.wasm",
        "hash": "sha256-ay8HWX3V52s3fo/zr8I3p76ZOdAuY0ecre1U2TCwOVI=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.WebProxy.wasm",
        "name": "System.Net.WebProxy.oq3mgkkqef.wasm",
        "hash": "sha256-kKO3TH8HsBXfC92Sy938oIV1HdKMS0uVpztIaUDDeU4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.WebSockets.Client.wasm",
        "name": "System.Net.WebSockets.Client.rafzkktw8j.wasm",
        "hash": "sha256-M2xvJviBA3wTLi4RJU+payoXVPiEryPgwyydiLJ04WA=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.WebSockets.wasm",
        "name": "System.Net.WebSockets.2cx1v74vg8.wasm",
        "hash": "sha256-/qUjzqNF74n5FiEQpOww2wGmAOT57HohJWFc6N/CtJc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.wasm",
        "name": "System.Net.w1hpj3bo6r.wasm",
        "hash": "sha256-bE5JhulvXs+VFtgIW1IMKhELHc5gUDe0EPzNiDtoZpE=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Numerics.Vectors.wasm",
        "name": "System.Numerics.Vectors.oyv7biw9ux.wasm",
        "hash": "sha256-cK8cVJbRJLyKnySzrxM3O/YJXg8JlRarWMJAYxG2QcU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Numerics.wasm",
        "name": "System.Numerics.lyu36wqn19.wasm",
        "hash": "sha256-g5p6oqfQl2OUj/Or3JJiTCnkMEiv5Yl6r3Aki08Xdvc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ObjectModel.wasm",
        "name": "System.ObjectModel.kxu80demuu.wasm",
        "hash": "sha256-aP1cHdnCvYP1drrICHt/oMYUqSOQVVVlW5fxAzhNYuE=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Private.DataContractSerialization.wasm",
        "name": "System.Private.DataContractSerialization.ao8k9iu77a.wasm",
        "hash": "sha256-6jmrkfhbtGkAs8F0pAMzP0ymubYGNI7n7a1J2xY4y98=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Private.Uri.wasm",
        "name": "System.Private.Uri.itn0n2zjd0.wasm",
        "hash": "sha256-atgB1gY60K6/ntpQXNN8jsGpnw185Y8zBq6WNNt5LKg=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Private.Xml.Linq.wasm",
        "name": "System.Private.Xml.Linq.tw5eq1hjvo.wasm",
        "hash": "sha256-beNcumb5VoNQYWiR9bxGCuUPTD15NSD6EqARdh9HU/Y=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Private.Xml.wasm",
        "name": "System.Private.Xml.19nzpxrx8w.wasm",
        "hash": "sha256-i9KhUrIpGVyhMpgL4GV/JczYp4NdDf+o18PilPlm48E=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Reflection.DispatchProxy.wasm",
        "name": "System.Reflection.DispatchProxy.7lajnj8s8c.wasm",
        "hash": "sha256-W9+0UBxtz0EvZ9xgkmng+tHOhXWW2GBKdjcZAVbc6YE=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Reflection.Emit.ILGeneration.wasm",
        "name": "System.Reflection.Emit.ILGeneration.1ut3qgacw8.wasm",
        "hash": "sha256-yfOooDyPto0Lng17CwgJ55NXqhWz+t9SfhiGJeT79HY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Reflection.Emit.Lightweight.wasm",
        "name": "System.Reflection.Emit.Lightweight.d0gvllb1jj.wasm",
        "hash": "sha256-MwR9j/suPuezXSGdbNPU23VHe5GIBf6R9Bs1p9BtP3k=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Reflection.Emit.wasm",
        "name": "System.Reflection.Emit.7liz9zln06.wasm",
        "hash": "sha256-JZK/huZ9doWYIPv0yzev1wYuhUXAIQrxo2NCyu7A1OA=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Reflection.Extensions.wasm",
        "name": "System.Reflection.Extensions.zr5bhwfczd.wasm",
        "hash": "sha256-/xKkpAsqwNQ8soTda97aUed6al1D9dGTdTA2lRvrLxM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Reflection.Metadata.wasm",
        "name": "System.Reflection.Metadata.ff6np6mz2j.wasm",
        "hash": "sha256-tc7STzUxi0O+S8ivukRXeP7SVKVDwsqSYPtr8b0nhqQ=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Reflection.Primitives.wasm",
        "name": "System.Reflection.Primitives.igpmvwv78c.wasm",
        "hash": "sha256-niYygMlK1HnbEugbNiTeBc7EkQIXXPnnZkgZUqq3qLs=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Reflection.TypeExtensions.wasm",
        "name": "System.Reflection.TypeExtensions.qouojzlnp9.wasm",
        "hash": "sha256-pt8RKbO93gvHhs2DfKf4ctYNpnH6Ec2j6l+M0SgdAS4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Reflection.wasm",
        "name": "System.Reflection.mk3jyxvh13.wasm",
        "hash": "sha256-1k6WUVW1mJwUKSKAwvKj5wtG/WTYf3scCmFyc4uRtWs=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Resources.Reader.wasm",
        "name": "System.Resources.Reader.41ppfyz2vc.wasm",
        "hash": "sha256-+DE3yDPtRF5JM8T52ycQt1qSAJ0vbjX5nu4Zoj+SuZ4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Resources.ResourceManager.wasm",
        "name": "System.Resources.ResourceManager.sfwvfmw1ea.wasm",
        "hash": "sha256-CIM4gUMmot/+RliOJhpf94ZlIsAr4YiEfWvOAXbovMU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Resources.Writer.wasm",
        "name": "System.Resources.Writer.3uvs9tp38z.wasm",
        "hash": "sha256-Kx23sKGyz/kEgJVd1ZRRDUI93A95CFpkYIzCejkCxtQ=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.CompilerServices.Unsafe.wasm",
        "name": "System.Runtime.CompilerServices.Unsafe.puz1xm53oc.wasm",
        "hash": "sha256-P6tIzozdCJqHiy0zGjuPtLVfOYb5yb0QPwFMwMUn4AY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.CompilerServices.VisualC.wasm",
        "name": "System.Runtime.CompilerServices.VisualC.9qdau315py.wasm",
        "hash": "sha256-/4HwDzaQ5a7S2V+Qzr94Nz3zi91/aZ30tRhta0XKqYE=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.Extensions.wasm",
        "name": "System.Runtime.Extensions.i7gntui96b.wasm",
        "hash": "sha256-Mnk3n1eK4eqTEZkm/ZfT7LQmGdwGAoF2anLqrZbDPbM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.Handles.wasm",
        "name": "System.Runtime.Handles.00oxj4o6ov.wasm",
        "hash": "sha256-QIVT4ckpz17JUAEsvaaKYYH35Ou8V9ErIKuwNI05CZU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.InteropServices.RuntimeInformation.wasm",
        "name": "System.Runtime.InteropServices.RuntimeInformation.my7qeqbhfh.wasm",
        "hash": "sha256-zhC5CCW9nCkqGXdOfIfwawjFHi5JjlQNnD8pGkdfWoE=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.InteropServices.wasm",
        "name": "System.Runtime.InteropServices.vqp80tnwc7.wasm",
        "hash": "sha256-Vzg1KlSYjHMKB8KW97OKaZsRei8Q593KoPgnQe1n5Uo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.Intrinsics.wasm",
        "name": "System.Runtime.Intrinsics.u0samjybfo.wasm",
        "hash": "sha256-Xo6xM56FQTZ30/6Vggp3fYR+4FxV/KmMooNBNpt+w6I=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.Loader.wasm",
        "name": "System.Runtime.Loader.n6zxw1b98a.wasm",
        "hash": "sha256-X+RMnAfTVC4+TDBQoS/h7LhyPBwJjorpoELFBfV7248=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.Numerics.wasm",
        "name": "System.Runtime.Numerics.o40sjhz59x.wasm",
        "hash": "sha256-qDYDPlthKrlj2dnaBsNoMK5X/nxKhTIdUiQ9ugVTuW0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.Serialization.Formatters.wasm",
        "name": "System.Runtime.Serialization.Formatters.1esvva4te3.wasm",
        "hash": "sha256-h/dAFonk9eigobeeUZLSOdbPf0Gz2On4fYCbW4hGwmk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.Serialization.Json.wasm",
        "name": "System.Runtime.Serialization.Json.4n7u09advh.wasm",
        "hash": "sha256-MCbIAsHQBugo8qpJp51Z0UHPviJI013X8cqoUXXfa7M=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.Serialization.Primitives.wasm",
        "name": "System.Runtime.Serialization.Primitives.xs2bo91wvk.wasm",
        "hash": "sha256-DzErfw7r3LKDKFr89PBxgF+SxxMEJcb6lGnpmdvn3A8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.Serialization.Xml.wasm",
        "name": "System.Runtime.Serialization.Xml.ny21jmziti.wasm",
        "hash": "sha256-P4CWph0hEmcsfiwKYkRp4me/5o8puJqWxBfj0AcLfWk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.Serialization.wasm",
        "name": "System.Runtime.Serialization.qcxaur5wqs.wasm",
        "hash": "sha256-pr4aZh6q8+LiDRjtVtJkkLeCZuVgRF9iOhru3cXTfUY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.wasm",
        "name": "System.Runtime.odq454d6j9.wasm",
        "hash": "sha256-dJxxG1+xZ1brKjnFZcEsO69maKgVzhnw94U5gUor6Gs=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.AccessControl.wasm",
        "name": "System.Security.AccessControl.8yfnzmqzaq.wasm",
        "hash": "sha256-gCsoRrVikUlzNxsrNbZ+6QVGT2+XUPKJWx95hBKEl/w=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Claims.wasm",
        "name": "System.Security.Claims.8ogy0860de.wasm",
        "hash": "sha256-GPcSbHwWVLH1fca0stI2MqZE+xsVLrA/kskuiQycJ4Q=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Cryptography.Algorithms.wasm",
        "name": "System.Security.Cryptography.Algorithms.8zijp1svfw.wasm",
        "hash": "sha256-LDzYiGnR0brxYcFNo0ocBo59dYZZb9MBe3ild2znJSc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Cryptography.Cng.wasm",
        "name": "System.Security.Cryptography.Cng.yxagtt77no.wasm",
        "hash": "sha256-ZpyrNDtC+3ti6m3d6mhH1as/m+ZFqLtc2bZBFNtuVH8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Cryptography.Csp.wasm",
        "name": "System.Security.Cryptography.Csp.uhfbvvau84.wasm",
        "hash": "sha256-Mp7WemXwikMFbyEZOblQA8l8wqAZ5jhmh6QboTii32o=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Cryptography.Encoding.wasm",
        "name": "System.Security.Cryptography.Encoding.2ywrusvy8q.wasm",
        "hash": "sha256-duIpVcBN7g31lDD77yYuAG3xM3U5GZtePHznUiVz0m4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Cryptography.OpenSsl.wasm",
        "name": "System.Security.Cryptography.OpenSsl.oa268u7i92.wasm",
        "hash": "sha256-4H6EYIOcQKX7spTR+G74jmTVJg6+MgAtY9g8b22KcFg=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Cryptography.Primitives.wasm",
        "name": "System.Security.Cryptography.Primitives.5uhvqe8puk.wasm",
        "hash": "sha256-DX5w2C46UrZY0ubTaEn3lxmlfYnJFHE1TN8wI4qsMgc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Cryptography.X509Certificates.wasm",
        "name": "System.Security.Cryptography.X509Certificates.6i7pxilopt.wasm",
        "hash": "sha256-wnRBilXju2GhWZsScBp6pBZ8dNSD81FuW+DXXZxUu1I=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Cryptography.wasm",
        "name": "System.Security.Cryptography.d0quqk1iyr.wasm",
        "hash": "sha256-LZkStq41MX59Tptph5/JGQRSG8B5+a1ldrb0QpNVB6I=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Principal.Windows.wasm",
        "name": "System.Security.Principal.Windows.gpqua4rz29.wasm",
        "hash": "sha256-TNOfYwVGiv+d/Xk6LXgrOyiDkxGzv9NyH2IGyuVIMR4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Principal.wasm",
        "name": "System.Security.Principal.i20kfklf3g.wasm",
        "hash": "sha256-Wow4n7EbYXc1HfQqHQu1Jhl7f8vCrcVNkFAXLZvyvTg=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.SecureString.wasm",
        "name": "System.Security.SecureString.x2y8tzci3c.wasm",
        "hash": "sha256-d7gVbpst3NnK88ZEcpaJoH5w3zfmsRXDJQvvqbL+RVY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.wasm",
        "name": "System.Security.e83vvh6sl2.wasm",
        "hash": "sha256-HuX7LBO46UViQb2u/0vvs+XLxgVQE+KVlOs6UH+Gw0g=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ServiceModel.Web.wasm",
        "name": "System.ServiceModel.Web.poglliv2no.wasm",
        "hash": "sha256-R4tw5kXVUZ+Qsq6/4Xkq7Qedv5pzouTdXVacoeG0V/0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ServiceProcess.wasm",
        "name": "System.ServiceProcess.yro5uj2opo.wasm",
        "hash": "sha256-Usmz3M5o5WSM1Sc2dU3njqXU1MeSMvgc8oaxTK8gBGo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Text.Encoding.CodePages.wasm",
        "name": "System.Text.Encoding.CodePages.2wchx40i7z.wasm",
        "hash": "sha256-giy48Parl41Ox46pfccHLTXlypNv3dXKu+yRKiWcJC0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Text.Encoding.Extensions.wasm",
        "name": "System.Text.Encoding.Extensions.8utaw61kn5.wasm",
        "hash": "sha256-kL2CE+V74498vfb1kuYNsti2ylJDViK5eEnTmKJv+54=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Text.Encoding.wasm",
        "name": "System.Text.Encoding.cxdyfepnpj.wasm",
        "hash": "sha256-AM6c0tx+lLrDZ6OZN3oLhe6nC21LZ6j1sEKHYC0gX9Q=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Text.Encodings.Web.wasm",
        "name": "System.Text.Encodings.Web.37ehbi7cye.wasm",
        "hash": "sha256-H0VkIeM0rlJX/Veq/zPhEyH7piskV3JOKngzP/djJE8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Text.Json.wasm",
        "name": "System.Text.Json.ixiaa8hd3h.wasm",
        "hash": "sha256-qsdCabiszf3qHMsvYiRuxa71WNCjNekpqBRHs8469vQ=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Text.RegularExpressions.wasm",
        "name": "System.Text.RegularExpressions.2fhfmkk6vf.wasm",
        "hash": "sha256-7msxYtUuVfZjmYO75wrZGO6rapEVdjT7RCgt6Y9EyGc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.AccessControl.wasm",
        "name": "System.Threading.AccessControl.76zfdfy8go.wasm",
        "hash": "sha256-z0hatN5fwMJgHdz4QHMG8+e9nDMcL+jWvoEY/4+50K8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.Channels.wasm",
        "name": "System.Threading.Channels.gpxnr01lmp.wasm",
        "hash": "sha256-ZFVZEgZJ4fAvrAa84Zoj5j1OLX+7VhF5jq0XJoGdPX0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.Overlapped.wasm",
        "name": "System.Threading.Overlapped.aqpl8tzdyc.wasm",
        "hash": "sha256-goELF2wzUusbMI1lATDF4myHde5GT8E3X4qmtHFNOeg=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.Tasks.Dataflow.wasm",
        "name": "System.Threading.Tasks.Dataflow.ytwttt7fzh.wasm",
        "hash": "sha256-dkzneK8QtxQQ6flGCimi/ofDvV+VowH1LcxXReD935E=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.Tasks.Extensions.wasm",
        "name": "System.Threading.Tasks.Extensions.0deavcb00v.wasm",
        "hash": "sha256-NEYDYaYBpgF3So553VgamGq3KZUL32UDvps2Y/dLdis=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.Tasks.Parallel.wasm",
        "name": "System.Threading.Tasks.Parallel.tjwtxugso0.wasm",
        "hash": "sha256-97xFkzLi4/C0otWqBmpjcxGrMAFBRhu4b29HpMXaTZw=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.Tasks.wasm",
        "name": "System.Threading.Tasks.fu4qd5h3qy.wasm",
        "hash": "sha256-+TOeSAsKIGT1UJBmjHRX61gRkGIQ+0vEtBQr8k+7HlI=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.Thread.wasm",
        "name": "System.Threading.Thread.klaua2by9m.wasm",
        "hash": "sha256-KFOGstVXEAMQHwhfWTIZj3A5HwiPl+KlgZetTOfe0cY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.ThreadPool.wasm",
        "name": "System.Threading.ThreadPool.mkzm212njz.wasm",
        "hash": "sha256-atxs3p14Jj6AnnJM+AEdbVR3Le/oST9CtB8bz0mcM5U=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.Timer.wasm",
        "name": "System.Threading.Timer.yp49llu5h2.wasm",
        "hash": "sha256-2rbLSkDUh/CdKMUXN12xK590P5PdtQ6+D/HUF1Y2zNY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.wasm",
        "name": "System.Threading.6pry74a050.wasm",
        "hash": "sha256-ulcimTPEQYo+vzvzjcj97+VdF4TYl1acGxv2F4064sc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Transactions.Local.wasm",
        "name": "System.Transactions.Local.vz6cgeh4w7.wasm",
        "hash": "sha256-lTkTSlfTqj7jJYcWurf1fHboUKCwCLKHNyJpgQFQy1E=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Transactions.wasm",
        "name": "System.Transactions.ipwm3rsy66.wasm",
        "hash": "sha256-6kE7mivQoxfhMTVAkmYi5VlsQhqXLYcImAuc9D7Skjo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ValueTuple.wasm",
        "name": "System.ValueTuple.7ol6ljuhx8.wasm",
        "hash": "sha256-yctvPmRbtoHiPpM5pKnajABrjRQR9a3b+HPWHyga2kY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Web.HttpUtility.wasm",
        "name": "System.Web.HttpUtility.itlvncjd96.wasm",
        "hash": "sha256-Nhc8L23J1+APB7ju97KZlDcnoEa6XvC6p4LluoAjVkY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Web.wasm",
        "name": "System.Web.np8uzzakvl.wasm",
        "hash": "sha256-m5aBz++zzF8tNzlN4+R5pPqGhS882Pxw4KwoakadVrI=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Windows.wasm",
        "name": "System.Windows.18nzio44t3.wasm",
        "hash": "sha256-r62brvRKMGvdcIg3SIbWWi7k8/NqCfpn+LPqnwitvKE=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Xml.Linq.wasm",
        "name": "System.Xml.Linq.v9v8udncm1.wasm",
        "hash": "sha256-rQ0vSy0Z16GnVFmS/wCs02+prtLem1HSrbHdfWIzNK4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Xml.ReaderWriter.wasm",
        "name": "System.Xml.ReaderWriter.a9mr08uzci.wasm",
        "hash": "sha256-bmkzRCDnwMgFnyZvQKnjJh5rdfZJtZ4nirjaJufIrqk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Xml.Serialization.wasm",
        "name": "System.Xml.Serialization.03juoguwgc.wasm",
        "hash": "sha256-HPQyo7cnsrAul9FfHP0JLVYXonc/DYwM87t6IXA4jPc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Xml.XDocument.wasm",
        "name": "System.Xml.XDocument.c9dhjnzqjb.wasm",
        "hash": "sha256-YLwIjMK8F1xq/FIgzG6+jiJStkbL+5sLQLdzD2sgpa8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Xml.XPath.XDocument.wasm",
        "name": "System.Xml.XPath.XDocument.138b12i1pm.wasm",
        "hash": "sha256-rUb1sC2EPqYnLys9V/C0H/313aLByFrqm0qT6zJpUcE=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Xml.XPath.wasm",
        "name": "System.Xml.XPath.pvxm9341ce.wasm",
        "hash": "sha256-+5VqXw8HmAjDvyi2+y7K95eMarW7XY4wPW0bh7x8L/I=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Xml.XmlDocument.wasm",
        "name": "System.Xml.XmlDocument.450wsyxvl0.wasm",
        "hash": "sha256-uPwda+H2hEsj0Jbt2bXGnSVbjkdbRsFU3TjifwMLQi4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Xml.XmlSerializer.wasm",
        "name": "System.Xml.XmlSerializer.y3t2n0tce7.wasm",
        "hash": "sha256-IrnG91lN1tfKsabToUYpusckhRr9wXb3Jxm6CqsOTJE=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Xml.wasm",
        "name": "System.Xml.45jprtg1pa.wasm",
        "hash": "sha256-KPyqHJl+ZMo3c6PCl2vPaQGBdD8P4KUBxsUkvR90/hk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.wasm",
        "name": "System.gjriafnqmz.wasm",
        "hash": "sha256-7DGslki9AfExPpvTTP5Lou+UeVMPtxC7TMoTwvo5sKY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "WindowsBase.wasm",
        "name": "WindowsBase.8zsce9qxfb.wasm",
        "hash": "sha256-tI94+TOjFqR3Z880lf+2Ye9R7uo6OxX3t73SZC3Wodc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "mscorlib.wasm",
        "name": "mscorlib.8ptuc08eli.wasm",
        "hash": "sha256-3H2G5ngT7eIaYsTDs+CoGgdvfTchxty175b/4H9+qxI=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "netstandard.wasm",
        "name": "netstandard.eymp85mooh.wasm",
        "hash": "sha256-dtyh2m2c4Ak4ASCotVspn+EmOiuXkI5zqS2q90qHb+s=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "MyBlazorWasmApp1.wasm",
        "name": "MyBlazorWasmApp1.czjxypapzu.wasm",
        "hash": "sha256-mNagsGGEFjricvVrujArNgicC3Rc3rnmv8uABXNoNfU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "MyBlazorWasmApp1.Stories.wasm",
        "name": "MyBlazorWasmApp1.Stories.creju17uw8.wasm",
        "hash": "sha256-eEPuBkjKSS1Zpr6ZljWlGURUaoNpVUooGLZNDO3Q8s4=",
        "cache": "force-cache"
      }
    ],
    "libraryInitializers": [
      {
        "name": "_content/BlazingStory/BlazingStory.isoonq25b3.lib.module.js"
      }
    ],
    "modulesAfterConfigLoaded": [
      {
        "name": "../_content/BlazingStory/BlazingStory.isoonq25b3.lib.module.js"
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
        "System.Diagnostics.Metrics.Meter.IsSupported": false,
        "System.Diagnostics.Tracing.EventSource.IsSupported": false,
        "System.GC.Server": true,
        "System.Globalization.Invariant": true,
        "System.TimeZoneInfo.Invariant": true,
        "System.Globalization.PredefinedCulturesOnly": true,
        "System.Linq.Enumerable.IsSizeOptimized": true,
        "System.Net.Http.EnableActivityPropagation": false,
        "System.Net.Http.WasmEnableStreamingResponse": true,
        "System.Net.SocketsHttpHandler.Http3Support": false,
        "System.Reflection.Metadata.MetadataUpdater.IsSupported": false,
        "System.Resources.UseSystemResourceKeys": true,
        "System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization": false,
        "System.Text.Encoding.EnableUnsafeUTF7Encoding": false,
        "System.Text.Json.JsonSerializer.IsReflectionEnabledByDefault": true
      }
    }
  }
}/*json-end*/);export{gt as default,ft as dotnet,mt as exit};
