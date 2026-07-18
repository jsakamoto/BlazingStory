//! Licensed to the .NET Foundation under one or more agreements.
//! The .NET Foundation licenses this file to you under the MIT license.

var e=!1;const t=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,4,1,96,0,0,3,2,1,0,10,8,1,6,0,6,64,25,11,11])),o=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,5,1,96,0,1,123,3,2,1,0,10,15,1,13,0,65,1,253,15,65,2,253,15,253,128,2,11])),n=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,5,1,96,0,1,123,3,2,1,0,10,10,1,8,0,65,0,253,15,253,98,11])),r=Symbol.for("wasm promise_control");function i(e,t){let o=null;const n=new Promise((function(n,r){o={isDone:!1,promise:null,resolve:t=>{o.isDone||(o.isDone=!0,n(t),e&&e())},reject:e=>{o.isDone||(o.isDone=!0,r(e),t&&t())}}}));o.promise=n;const i=n;return i[r]=o,{promise:i,promise_control:o}}function s(e){return e[r]}function a(e){e&&function(e){return void 0!==e[r]}(e)||Be(!1,"Promise is not controllable")}const l="__mono_message__",c=["debug","log","trace","warn","info","error"],d="MONO_WASM: ";let u,f,m,g,p,h;function w(e){g=e}function b(e){if(Pe.diagnosticTracing){const t="function"==typeof e?e():e;console.debug(d+t)}}function y(e,...t){console.info(d+e,...t)}function v(e,...t){console.info(e,...t)}function E(e,...t){console.warn(d+e,...t)}function _(e,...t){if(t&&t.length>0&&t[0]&&"object"==typeof t[0]){if(t[0].silent)return;if(t[0].toString)return void console.error(d+e,t[0].toString())}console.error(d+e,...t)}function x(e,t,o){return function(...n){try{let r=n[0];if(void 0===r)r="undefined";else if(null===r)r="null";else if("function"==typeof r)r=r.toString();else if("string"!=typeof r)try{r=JSON.stringify(r)}catch(e){r=r.toString()}t(o?JSON.stringify({method:e,payload:r,arguments:n.slice(1)}):[e+r,...n.slice(1)])}catch(e){m.error(`proxyConsole failed: ${e}`)}}}function j(e,t,o){f=t,g=e,m={...t};const n=`${o}/console`.replace("https://","wss://").replace("http://","ws://");u=new WebSocket(n),u.addEventListener("error",A),u.addEventListener("close",S),function(){for(const e of c)f[e]=x(`console.${e}`,T,!0)}()}function R(e){let t=30;const o=()=>{u?0==u.bufferedAmount||0==t?(e&&v(e),function(){for(const e of c)f[e]=x(`console.${e}`,m.log,!1)}(),u.removeEventListener("error",A),u.removeEventListener("close",S),u.close(1e3,e),u=void 0):(t--,globalThis.setTimeout(o,100)):e&&m&&m.log(e)};o()}function T(e){u&&u.readyState===WebSocket.OPEN?u.send(e):m.log(e)}function A(e){m.error(`[${g}] proxy console websocket error: ${e}`,e)}function S(e){m.debug(`[${g}] proxy console websocket closed: ${e}`,e)}function D(){Pe.preferredIcuAsset=O(Pe.config);let e="invariant"==Pe.config.globalizationMode;if(!e)if(Pe.preferredIcuAsset)Pe.diagnosticTracing&&b("ICU data archive(s) available, disabling invariant mode");else{if("custom"===Pe.config.globalizationMode||"all"===Pe.config.globalizationMode||"sharded"===Pe.config.globalizationMode){const e="invariant globalization mode is inactive and no ICU data archives are available";throw _(`ERROR: ${e}`),new Error(e)}Pe.diagnosticTracing&&b("ICU data archive(s) not available, using invariant globalization mode"),e=!0,Pe.preferredIcuAsset=null}const t="DOTNET_SYSTEM_GLOBALIZATION_INVARIANT",o=Pe.config.environmentVariables;if(void 0===o[t]&&e&&(o[t]="1"),void 0===o.TZ)try{const e=Intl.DateTimeFormat().resolvedOptions().timeZone||null;e&&(o.TZ=e)}catch(e){y("failed to detect timezone, will fallback to UTC")}}function O(e){var t;if((null===(t=e.resources)||void 0===t?void 0:t.icu)&&"invariant"!=e.globalizationMode){const t=e.applicationCulture||(ke?globalThis.navigator&&globalThis.navigator.languages&&globalThis.navigator.languages[0]:Intl.DateTimeFormat().resolvedOptions().locale),o=e.resources.icu;let n=null;if("custom"===e.globalizationMode){if(o.length>=1)return o[0].name}else t&&"all"!==e.globalizationMode?"sharded"===e.globalizationMode&&(n=function(e){const t=e.split("-")[0];return"en"===t||["fr","fr-FR","it","it-IT","de","de-DE","es","es-ES"].includes(e)?"icudt_EFIGS.dat":["zh","ko","ja"].includes(t)?"icudt_CJK.dat":"icudt_no_CJK.dat"}(t)):n="icudt.dat";if(n)for(let e=0;e<o.length;e++){const t=o[e];if(t.virtualPath===n)return t.name}}return e.globalizationMode="invariant",null}(new Date).valueOf();const C=class{constructor(e){this.url=e}toString(){return this.url}};async function k(e,t){try{const o="function"==typeof globalThis.fetch;if(Se){const n=e.startsWith("file://");if(!n&&o)return globalThis.fetch(e,t||{credentials:"same-origin"});p||(h=Ne.require("url"),p=Ne.require("fs")),n&&(e=h.fileURLToPath(e));const r=await p.promises.readFile(e);return{ok:!0,headers:{length:0,get:()=>null},url:e,arrayBuffer:()=>r,json:()=>JSON.parse(r),text:()=>{throw new Error("NotImplementedException")}}}if(o)return globalThis.fetch(e,t||{credentials:"same-origin"});if("function"==typeof read)return{ok:!0,url:e,headers:{length:0,get:()=>null},arrayBuffer:()=>new Uint8Array(read(e,"binary")),json:()=>JSON.parse(read(e,"utf8")),text:()=>read(e,"utf8")}}catch(t){return{ok:!1,url:e,status:500,headers:{length:0,get:()=>null},statusText:"ERR28: "+t,arrayBuffer:()=>{throw t},json:()=>{throw t},text:()=>{throw t}}}throw new Error("No fetch implementation available")}function I(e){return"string"!=typeof e&&Be(!1,"url must be a string"),!M(e)&&0!==e.indexOf("./")&&0!==e.indexOf("../")&&globalThis.URL&&globalThis.document&&globalThis.document.baseURI&&(e=new URL(e,globalThis.document.baseURI).toString()),e}const U=/^[a-zA-Z][a-zA-Z\d+\-.]*?:\/\//,P=/[a-zA-Z]:[\\/]/;function M(e){return Se||Ie?e.startsWith("/")||e.startsWith("\\")||-1!==e.indexOf("///")||P.test(e):U.test(e)}let L,N=0;const $=[],z=[],W=new Map,F={"js-module-threads":!0,"js-module-runtime":!0,"js-module-dotnet":!0,"js-module-native":!0,"js-module-diagnostics":!0},B={...F,"js-module-library-initializer":!0},V={...F,dotnetwasm:!0,heap:!0,manifest:!0},q={...B,manifest:!0},H={...B,dotnetwasm:!0},J={dotnetwasm:!0,symbols:!0},Z={...B,dotnetwasm:!0,symbols:!0},Q={symbols:!0};function G(e){return!("icu"==e.behavior&&e.name!=Pe.preferredIcuAsset)}function K(e,t,o){null!=t||(t=[]),Be(1==t.length,`Expect to have one ${o} asset in resources`);const n=t[0];return n.behavior=o,X(n),e.push(n),n}function X(e){V[e.behavior]&&W.set(e.behavior,e)}function Y(e){Be(V[e],`Unknown single asset behavior ${e}`);const t=W.get(e);if(t&&!t.resolvedUrl)if(t.resolvedUrl=Pe.locateFile(t.name),F[t.behavior]){const e=ge(t);e?("string"!=typeof e&&Be(!1,"loadBootResource response for 'dotnetjs' type should be a URL string"),t.resolvedUrl=e):t.resolvedUrl=ce(t.resolvedUrl,t.behavior)}else if("dotnetwasm"!==t.behavior)throw new Error(`Unknown single asset behavior ${e}`);return t}function ee(e){const t=Y(e);return Be(t,`Single asset for ${e} not found`),t}let te=!1;async function oe(){if(!te){te=!0,Pe.diagnosticTracing&&b("mono_download_assets");try{const e=[],t=[],o=(e,t)=>{!Z[e.behavior]&&G(e)&&Pe.expected_instantiated_assets_count++,!H[e.behavior]&&G(e)&&(Pe.expected_downloaded_assets_count++,t.push(se(e)))};for(const t of $)o(t,e);for(const e of z)o(e,t);Pe.allDownloadsQueued.promise_control.resolve(),Promise.all([...e,...t]).then((()=>{Pe.allDownloadsFinished.promise_control.resolve()})).catch((e=>{throw Pe.err("Error in mono_download_assets: "+e),Xe(1,e),e})),await Pe.runtimeModuleLoaded.promise;const n=async e=>{const t=await e;if(t.buffer){if(!Z[t.behavior]){t.buffer&&"object"==typeof t.buffer||Be(!1,"asset buffer must be array-like or buffer-like or promise of these"),"string"!=typeof t.resolvedUrl&&Be(!1,"resolvedUrl must be string");const e=t.resolvedUrl,o=await t.buffer,n=new Uint8Array(o);pe(t),await Ue.beforeOnRuntimeInitialized.promise,Ue.instantiate_asset(t,e,n)}}else J[t.behavior]?("symbols"===t.behavior&&(await Ue.instantiate_symbols_asset(t),pe(t)),J[t.behavior]&&++Pe.actual_downloaded_assets_count):(t.isOptional||Be(!1,"Expected asset to have the downloaded buffer"),!H[t.behavior]&&G(t)&&Pe.expected_downloaded_assets_count--,!Z[t.behavior]&&G(t)&&Pe.expected_instantiated_assets_count--)},r=[],i=[];for(const t of e)r.push(n(t));for(const e of t)i.push(n(e));Promise.all(r).then((()=>{Ce||Ue.coreAssetsInMemory.promise_control.resolve()})).catch((e=>{throw Pe.err("Error in mono_download_assets: "+e),Xe(1,e),e})),Promise.all(i).then((async()=>{Ce||(await Ue.coreAssetsInMemory.promise,Ue.allAssetsInMemory.promise_control.resolve())})).catch((e=>{throw Pe.err("Error in mono_download_assets: "+e),Xe(1,e),e}))}catch(e){throw Pe.err("Error in mono_download_assets: "+e),e}}}let ne=!1;function re(){if(ne)return;ne=!0;const e=Pe.config,t=[];if(e.assets)for(const t of e.assets)"object"!=typeof t&&Be(!1,`asset must be object, it was ${typeof t} : ${t}`),"string"!=typeof t.behavior&&Be(!1,"asset behavior must be known string"),"string"!=typeof t.name&&Be(!1,"asset name must be string"),t.resolvedUrl&&"string"!=typeof t.resolvedUrl&&Be(!1,"asset resolvedUrl could be string"),t.hash&&"string"!=typeof t.hash&&Be(!1,"asset resolvedUrl could be string"),t.pendingDownload&&"object"!=typeof t.pendingDownload&&Be(!1,"asset pendingDownload could be object"),t.isCore?$.push(t):z.push(t),X(t);else if(e.resources){const o=e.resources;o.wasmNative||Be(!1,"resources.wasmNative must be defined"),o.jsModuleNative||Be(!1,"resources.jsModuleNative must be defined"),o.jsModuleRuntime||Be(!1,"resources.jsModuleRuntime must be defined"),K(z,o.wasmNative,"dotnetwasm"),K(t,o.jsModuleNative,"js-module-native"),K(t,o.jsModuleRuntime,"js-module-runtime"),o.jsModuleDiagnostics&&K(t,o.jsModuleDiagnostics,"js-module-diagnostics");const n=(e,t,o)=>{const n=e;n.behavior=t,o?(n.isCore=!0,$.push(n)):z.push(n)};if(o.coreAssembly)for(let e=0;e<o.coreAssembly.length;e++)n(o.coreAssembly[e],"assembly",!0);if(o.assembly)for(let e=0;e<o.assembly.length;e++)n(o.assembly[e],"assembly",!o.coreAssembly);if(0!=e.debugLevel&&Pe.isDebuggingSupported()){if(o.corePdb)for(let e=0;e<o.corePdb.length;e++)n(o.corePdb[e],"pdb",!0);if(o.pdb)for(let e=0;e<o.pdb.length;e++)n(o.pdb[e],"pdb",!o.corePdb)}if(e.loadAllSatelliteResources&&o.satelliteResources)for(const e in o.satelliteResources)for(let t=0;t<o.satelliteResources[e].length;t++){const r=o.satelliteResources[e][t];r.culture=e,n(r,"resource",!o.coreAssembly)}if(o.coreVfs)for(let e=0;e<o.coreVfs.length;e++)n(o.coreVfs[e],"vfs",!0);if(o.vfs)for(let e=0;e<o.vfs.length;e++)n(o.vfs[e],"vfs",!o.coreVfs);const r=O(e);if(r&&o.icu)for(let e=0;e<o.icu.length;e++){const t=o.icu[e];t.name===r&&n(t,"icu",!1)}if(o.wasmSymbols)for(let e=0;e<o.wasmSymbols.length;e++)n(o.wasmSymbols[e],"symbols",!1)}if(e.appsettings)for(let t=0;t<e.appsettings.length;t++){const o=e.appsettings[t],n=he(o);"appsettings.json"!==n&&n!==`appsettings.${e.applicationEnvironment}.json`||z.push({name:o,behavior:"vfs",cache:"no-cache",useCredentials:!0})}e.assets=[...$,...z,...t]}async function ie(e){const t=await se(e);return await t.pendingDownloadInternal.response,t.buffer}async function se(e){try{return await ae(e)}catch(t){if(!Pe.enableDownloadRetry)throw t;if(Ie||Se)throw t;if(e.pendingDownload&&e.pendingDownloadInternal==e.pendingDownload)throw t;if(e.resolvedUrl&&-1!=e.resolvedUrl.indexOf("file://"))throw t;if(t&&404==t.status)throw t;e.pendingDownloadInternal=void 0,await Pe.allDownloadsQueued.promise;try{return Pe.diagnosticTracing&&b(`Retrying download '${e.name}'`),await ae(e)}catch(t){return e.pendingDownloadInternal=void 0,await new Promise((e=>globalThis.setTimeout(e,100))),Pe.diagnosticTracing&&b(`Retrying download (2) '${e.name}' after delay`),await ae(e)}}}async function ae(e){for(;L;)await L.promise;try{++N,N==Pe.maxParallelDownloads&&(Pe.diagnosticTracing&&b("Throttling further parallel downloads"),L=i());const t=await async function(e){if(e.pendingDownload&&(e.pendingDownloadInternal=e.pendingDownload),e.pendingDownloadInternal&&e.pendingDownloadInternal.response)return e.pendingDownloadInternal.response;if(e.buffer){const t=await e.buffer;return e.resolvedUrl||(e.resolvedUrl="undefined://"+e.name),e.pendingDownloadInternal={url:e.resolvedUrl,name:e.name,response:Promise.resolve({ok:!0,arrayBuffer:()=>t,json:()=>JSON.parse(new TextDecoder("utf-8").decode(t)),text:()=>{throw new Error("NotImplementedException")},headers:{get:()=>{}}})},e.pendingDownloadInternal.response}const t=e.loadRemote&&Pe.config.remoteSources?Pe.config.remoteSources:[""];let o;for(let n of t){n=n.trim(),"./"===n&&(n="");const t=le(e,n);e.name===t?Pe.diagnosticTracing&&b(`Attempting to download '${t}'`):Pe.diagnosticTracing&&b(`Attempting to download '${t}' for ${e.name}`);try{e.resolvedUrl=t;const n=fe(e);if(e.pendingDownloadInternal=n,o=await n.response,!o||!o.ok)continue;return o}catch(e){o||(o={ok:!1,url:t,status:0,statusText:""+e});continue}}const n=e.isOptional||e.name.match(/\.pdb$/)&&Pe.config.ignorePdbLoadErrors;if(o||Be(!1,`Response undefined ${e.name}`),!n){const t=new Error(`download '${o.url}' for ${e.name} failed ${o.status} ${o.statusText}`);throw t.status=o.status,t}y(`optional download '${o.url}' for ${e.name} failed ${o.status} ${o.statusText}`)}(e);return t?(J[e.behavior]||(e.buffer=await t.arrayBuffer(),++Pe.actual_downloaded_assets_count),e):e}finally{if(--N,L&&N==Pe.maxParallelDownloads-1){Pe.diagnosticTracing&&b("Resuming more parallel downloads");const e=L;L=void 0,e.promise_control.resolve()}}}function le(e,t){let o;return null==t&&Be(!1,`sourcePrefix must be provided for ${e.name}`),e.resolvedUrl?o=e.resolvedUrl:(o=""===t?"assembly"===e.behavior||"pdb"===e.behavior?e.name:"resource"===e.behavior&&e.culture&&""!==e.culture?`${e.culture}/${e.name}`:e.name:t+e.name,o=ce(Pe.locateFile(o),e.behavior)),o&&"string"==typeof o||Be(!1,"attemptUrl need to be path or url string"),o}function ce(e,t){return Pe.modulesUniqueQuery&&q[t]&&(e+=Pe.modulesUniqueQuery),e}let de=0;const ue=new Set;function fe(e){try{e.resolvedUrl||Be(!1,"Request's resolvedUrl must be set");const t=function(e){let t=e.resolvedUrl;if(Pe.loadBootResource){const o=ge(e);if(o instanceof Promise)return o;"string"==typeof o&&(t=o)}const o={};return e.cache?o.cache=e.cache:Pe.config.disableNoCacheFetch||(o.cache="no-cache"),e.useCredentials?o.credentials="include":!Pe.config.disableIntegrityCheck&&e.hash&&(o.integrity=e.hash),Pe.fetch_like(t,o)}(e),o={name:e.name,url:e.resolvedUrl,response:t};return ue.add(e.name),o.response.then((()=>{"assembly"==e.behavior&&Pe.loadedAssemblies.push(e.name),de++,Pe.onDownloadResourceProgress&&Pe.onDownloadResourceProgress(de,ue.size)})),o}catch(t){const o={ok:!1,url:e.resolvedUrl,status:500,statusText:"ERR29: "+t,arrayBuffer:()=>{throw t},json:()=>{throw t}};return{name:e.name,url:e.resolvedUrl,response:Promise.resolve(o)}}}const me={resource:"assembly",assembly:"assembly",pdb:"pdb",icu:"globalization",vfs:"configuration",manifest:"manifest",dotnetwasm:"dotnetwasm","js-module-dotnet":"dotnetjs","js-module-native":"dotnetjs","js-module-runtime":"dotnetjs","js-module-threads":"dotnetjs"};function ge(e){var t;if(Pe.loadBootResource){const o=null!==(t=e.hash)&&void 0!==t?t:"",n=e.resolvedUrl,r=me[e.behavior];if(r){const t=Pe.loadBootResource(r,e.name,n,o,e.behavior);return"string"==typeof t?I(t):t}}}function pe(e){e.pendingDownloadInternal=null,e.pendingDownload=null,e.buffer=null,e.moduleExports=null}function he(e){let t=e.lastIndexOf("/");return t>=0&&t++,e.substring(t)}async function we(e){e&&await Promise.all((null!=e?e:[]).map((e=>async function(e){try{const t=e.name;if(!e.moduleExports){const o=ce(Pe.locateFile(t),"js-module-library-initializer");Pe.diagnosticTracing&&b(`Attempting to import '${o}' for ${e}`),e.moduleExports=await import(/*! webpackIgnore: true */o)}Pe.libraryInitializers.push({scriptName:t,exports:e.moduleExports})}catch(t){E(`Failed to import library initializer '${e}': ${t}`)}}(e))))}async function be(e,t){if(!Pe.libraryInitializers)return;const o=[];for(let n=0;n<Pe.libraryInitializers.length;n++){const r=Pe.libraryInitializers[n];r.exports[e]&&o.push(ye(r.scriptName,e,(()=>r.exports[e](...t))))}await Promise.all(o)}async function ye(e,t,o){try{await o()}catch(o){throw E(`Failed to invoke '${t}' on library initializer '${e}': ${o}`),Xe(1,o),o}}function ve(e,t){if(e===t)return e;const o={...t};return void 0!==o.assets&&o.assets!==e.assets&&(o.assets=[...e.assets||[],...o.assets||[]]),void 0!==o.resources&&(o.resources=_e(e.resources||{assembly:[],jsModuleNative:[],jsModuleRuntime:[],wasmNative:[]},o.resources)),void 0!==o.environmentVariables&&(o.environmentVariables={...e.environmentVariables||{},...o.environmentVariables||{}}),void 0!==o.runtimeOptions&&o.runtimeOptions!==e.runtimeOptions&&(o.runtimeOptions=[...e.runtimeOptions||[],...o.runtimeOptions||[]]),Object.assign(e,o)}function Ee(e,t){if(e===t)return e;const o={...t};return o.config&&(e.config||(e.config={}),o.config=ve(e.config,o.config)),Object.assign(e,o)}function _e(e,t){if(e===t)return e;const o={...t};return void 0!==o.coreAssembly&&(o.coreAssembly=[...e.coreAssembly||[],...o.coreAssembly||[]]),void 0!==o.assembly&&(o.assembly=[...e.assembly||[],...o.assembly||[]]),void 0!==o.lazyAssembly&&(o.lazyAssembly=[...e.lazyAssembly||[],...o.lazyAssembly||[]]),void 0!==o.corePdb&&(o.corePdb=[...e.corePdb||[],...o.corePdb||[]]),void 0!==o.pdb&&(o.pdb=[...e.pdb||[],...o.pdb||[]]),void 0!==o.jsModuleWorker&&(o.jsModuleWorker=[...e.jsModuleWorker||[],...o.jsModuleWorker||[]]),void 0!==o.jsModuleNative&&(o.jsModuleNative=[...e.jsModuleNative||[],...o.jsModuleNative||[]]),void 0!==o.jsModuleDiagnostics&&(o.jsModuleDiagnostics=[...e.jsModuleDiagnostics||[],...o.jsModuleDiagnostics||[]]),void 0!==o.jsModuleRuntime&&(o.jsModuleRuntime=[...e.jsModuleRuntime||[],...o.jsModuleRuntime||[]]),void 0!==o.wasmSymbols&&(o.wasmSymbols=[...e.wasmSymbols||[],...o.wasmSymbols||[]]),void 0!==o.wasmNative&&(o.wasmNative=[...e.wasmNative||[],...o.wasmNative||[]]),void 0!==o.icu&&(o.icu=[...e.icu||[],...o.icu||[]]),void 0!==o.satelliteResources&&(o.satelliteResources=function(e,t){if(e===t)return e;for(const o in t)e[o]=[...e[o]||[],...t[o]||[]];return e}(e.satelliteResources||{},o.satelliteResources||{})),void 0!==o.modulesAfterConfigLoaded&&(o.modulesAfterConfigLoaded=[...e.modulesAfterConfigLoaded||[],...o.modulesAfterConfigLoaded||[]]),void 0!==o.modulesAfterRuntimeReady&&(o.modulesAfterRuntimeReady=[...e.modulesAfterRuntimeReady||[],...o.modulesAfterRuntimeReady||[]]),void 0!==o.extensions&&(o.extensions={...e.extensions||{},...o.extensions||{}}),void 0!==o.vfs&&(o.vfs=[...e.vfs||[],...o.vfs||[]]),Object.assign(e,o)}function xe(){const e=Pe.config;if(e.environmentVariables=e.environmentVariables||{},e.runtimeOptions=e.runtimeOptions||[],e.resources=e.resources||{assembly:[],jsModuleNative:[],jsModuleWorker:[],jsModuleRuntime:[],wasmNative:[],vfs:[],satelliteResources:{}},e.assets){Pe.diagnosticTracing&&b("config.assets is deprecated, use config.resources instead");for(const t of e.assets){const o={};switch(t.behavior){case"assembly":o.assembly=[t];break;case"pdb":o.pdb=[t];break;case"resource":o.satelliteResources={},o.satelliteResources[t.culture]=[t];break;case"icu":o.icu=[t];break;case"symbols":o.wasmSymbols=[t];break;case"vfs":o.vfs=[t];break;case"dotnetwasm":o.wasmNative=[t];break;case"js-module-threads":o.jsModuleWorker=[t];break;case"js-module-runtime":o.jsModuleRuntime=[t];break;case"js-module-native":o.jsModuleNative=[t];break;case"js-module-diagnostics":o.jsModuleDiagnostics=[t];break;case"js-module-dotnet":break;default:throw new Error(`Unexpected behavior ${t.behavior} of asset ${t.name}`)}_e(e.resources,o)}}e.debugLevel,e.applicationEnvironment||(e.applicationEnvironment="Production"),e.applicationCulture&&(e.environmentVariables.LANG=`${e.applicationCulture}.UTF-8`),Ue.diagnosticTracing=Pe.diagnosticTracing=!!e.diagnosticTracing,Ue.waitForDebugger=e.waitForDebugger,Pe.maxParallelDownloads=e.maxParallelDownloads||Pe.maxParallelDownloads,Pe.enableDownloadRetry=void 0!==e.enableDownloadRetry?e.enableDownloadRetry:Pe.enableDownloadRetry}let je=!1;async function Re(e){var t;if(je)return void await Pe.afterConfigLoaded.promise;let o;try{if(e.configSrc||Pe.config&&0!==Object.keys(Pe.config).length&&(Pe.config.assets||Pe.config.resources)||(e.configSrc="dotnet.boot.js"),o=e.configSrc,je=!0,o&&(Pe.diagnosticTracing&&b("mono_wasm_load_config"),await async function(e){const t=e.configSrc,o=Pe.locateFile(t);let n=null;void 0!==Pe.loadBootResource&&(n=Pe.loadBootResource("manifest",t,o,"","manifest"));let r,i=null;if(n)if("string"==typeof n)n.includes(".json")?(i=await s(I(n)),r=await Ae(i)):r=(await import(I(n))).config;else{const e=await n;"function"==typeof e.json?(i=e,r=await Ae(i)):r=e.config}else o.includes(".json")?(i=await s(ce(o,"manifest")),r=await Ae(i)):r=(await import(ce(o,"manifest"))).config;function s(e){return Pe.fetch_like(e,{method:"GET",credentials:"include",cache:"no-cache"})}Pe.config.applicationEnvironment&&(r.applicationEnvironment=Pe.config.applicationEnvironment),ve(Pe.config,r)}(e)),xe(),await we(null===(t=Pe.config.resources)||void 0===t?void 0:t.modulesAfterConfigLoaded),await be("onRuntimeConfigLoaded",[Pe.config]),e.onConfigLoaded)try{await e.onConfigLoaded(Pe.config,Le),xe()}catch(e){throw _("onConfigLoaded() failed",e),e}xe(),Pe.afterConfigLoaded.promise_control.resolve(Pe.config)}catch(t){const n=`Failed to load config file ${o} ${t} ${null==t?void 0:t.stack}`;throw Pe.config=e.config=Object.assign(Pe.config,{message:n,error:t,isError:!0}),Xe(1,new Error(n)),t}}function Te(){return!!globalThis.navigator&&(Pe.isChromium||Pe.isFirefox)}async function Ae(e){const t=Pe.config,o=await e.json();t.applicationEnvironment||o.applicationEnvironment||(o.applicationEnvironment=e.headers.get("Blazor-Environment")||e.headers.get("DotNet-Environment")||void 0),o.environmentVariables||(o.environmentVariables={});const n=e.headers.get("DOTNET-MODIFIABLE-ASSEMBLIES");n&&(o.environmentVariables.DOTNET_MODIFIABLE_ASSEMBLIES=n);const r=e.headers.get("ASPNETCORE-BROWSER-TOOLS");return r&&(o.environmentVariables.__ASPNETCORE_BROWSER_TOOLS=r),o}"function"!=typeof importScripts||globalThis.onmessage||(globalThis.dotnetSidecar=!0);const Se="object"==typeof process&&"object"==typeof process.versions&&"string"==typeof process.versions.node,De="function"==typeof importScripts,Oe=De&&"undefined"!=typeof dotnetSidecar,Ce=De&&!Oe,ke="object"==typeof window||De&&!Se,Ie=!ke&&!Se;let Ue={},Pe={},Me={},Le={},Ne={},$e=!1;const ze={},We={config:ze},Fe={mono:{},binding:{},internal:Ne,module:We,loaderHelpers:Pe,runtimeHelpers:Ue,diagnosticHelpers:Me,api:Le};function Be(e,t){if(e)return;const o="Assert failed: "+("function"==typeof t?t():t),n=new Error(o);_(o,n),Ue.nativeAbort(n)}function Ve(){return void 0!==Pe.exitCode}function qe(){return Ue.runtimeReady&&!Ve()}function He(){Ve()&&Be(!1,`.NET runtime already exited with ${Pe.exitCode} ${Pe.exitReason}. You can use runtime.runMain() which doesn't exit the runtime.`),Ue.runtimeReady||Be(!1,".NET runtime didn't start yet. Please call dotnet.create() first.")}function Je(){ke&&(globalThis.addEventListener("unhandledrejection",et),globalThis.addEventListener("error",tt))}let Ze,Qe;function Ge(e){Qe&&Qe(e),Xe(e,Pe.exitReason)}function Ke(e){Ze&&Ze(e||Pe.exitReason),Xe(1,e||Pe.exitReason)}function Xe(t,o){var n,r;const i=o&&"object"==typeof o;t=i&&"number"==typeof o.status?o.status:void 0===t?-1:t;const s=i&&"string"==typeof o.message?o.message:""+o;(o=i?o:Ue.ExitStatus?function(e,t){const o=new Ue.ExitStatus(e);return o.message=t,o.toString=()=>t,o}(t,s):new Error("Exit with code "+t+" "+s)).status=t,o.message||(o.message=s);const a=""+(o.stack||(new Error).stack);try{Object.defineProperty(o,"stack",{get:()=>a})}catch(e){}const l=!!o.silent;if(o.silent=!0,Ve())Pe.diagnosticTracing&&b("mono_exit called after exit");else{try{We.onAbort==Ke&&(We.onAbort=Ze),We.onExit==Ge&&(We.onExit=Qe),ke&&(globalThis.removeEventListener("unhandledrejection",et),globalThis.removeEventListener("error",tt)),Ue.runtimeReady?(Ue.jiterpreter_dump_stats&&Ue.jiterpreter_dump_stats(!1),0===t&&(null===(n=Pe.config)||void 0===n?void 0:n.interopCleanupOnExit)&&Ue.forceDisposeProxies(!0,!0),e&&0!==t&&(null===(r=Pe.config)||void 0===r||r.dumpThreadsOnNonZeroExit)):(Pe.diagnosticTracing&&b(`abort_startup, reason: ${o}`),function(e){Pe.allDownloadsQueued.promise_control.reject(e),Pe.allDownloadsFinished.promise_control.reject(e),Pe.afterConfigLoaded.promise_control.reject(e),Pe.wasmCompilePromise.promise_control.reject(e),Pe.runtimeModuleLoaded.promise_control.reject(e),Ue.dotnetReady&&(Ue.dotnetReady.promise_control.reject(e),Ue.afterInstantiateWasm.promise_control.reject(e),Ue.beforePreInit.promise_control.reject(e),Ue.afterPreInit.promise_control.reject(e),Ue.afterPreRun.promise_control.reject(e),Ue.beforeOnRuntimeInitialized.promise_control.reject(e),Ue.afterOnRuntimeInitialized.promise_control.reject(e),Ue.afterPostRun.promise_control.reject(e))}(o))}catch(e){E("mono_exit A failed",e)}try{l||(function(e,t){if(0!==e&&t){const e=Ue.ExitStatus&&t instanceof Ue.ExitStatus?b:_;"string"==typeof t?e(t):(void 0===t.stack&&(t.stack=(new Error).stack+""),t.message?e(Ue.stringify_as_error_with_stack?Ue.stringify_as_error_with_stack(t.message+"\n"+t.stack):t.message+"\n"+t.stack):e(JSON.stringify(t)))}!Ce&&Pe.config&&(Pe.config.logExitCode?Pe.config.forwardConsoleLogsToWS?R("WASM EXIT "+e):v("WASM EXIT "+e):Pe.config.forwardConsoleLogsToWS&&R())}(t,o),function(e){if(ke&&!Ce&&Pe.config&&Pe.config.appendElementOnExit&&document){const t=document.createElement("label");t.id="tests_done",0!==e&&(t.style.background="red"),t.innerHTML=""+e,document.body.appendChild(t)}}(t))}catch(e){E("mono_exit B failed",e)}Pe.exitCode=t,Pe.exitReason||(Pe.exitReason=o),!Ce&&Ue.runtimeReady&&We.runtimeKeepalivePop()}if(Pe.config&&Pe.config.asyncFlushOnExit&&0===t)throw(async()=>{try{await async function(){try{const e=await import(/*! webpackIgnore: true */"process"),t=e=>new Promise(((t,o)=>{e.on("error",o),e.end("","utf8",t)})),o=t(e.stderr),n=t(e.stdout);let r;const i=new Promise((e=>{r=setTimeout((()=>e("timeout")),1e3)}));await Promise.race([Promise.all([n,o]),i]),clearTimeout(r)}catch(e){_(`flushing std* streams failed: ${e}`)}}()}finally{Ye(t,o)}})(),o;Ye(t,o)}function Ye(e,t){if(Ue.runtimeReady&&Ue.nativeExit)try{Ue.nativeExit(e)}catch(e){!Ue.ExitStatus||e instanceof Ue.ExitStatus||E("set_exit_code_and_quit_now failed: "+e.toString())}if(0!==e||!ke)throw Se&&Ne.process?Ne.process.exit(e):Ue.quit&&Ue.quit(e,t),t}function et(e){ot(e,e.reason,"rejection")}function tt(e){ot(e,e.error,"error")}function ot(e,t,o){e.preventDefault();try{t||(t=new Error("Unhandled "+o)),void 0===t.stack&&(t.stack=(new Error).stack),t.stack=t.stack+"",t.silent||(_("Unhandled error:",t),Xe(1,t))}catch(e){}}!function(e){if($e)throw new Error("Loader module already loaded");$e=!0,Ue=e.runtimeHelpers,Pe=e.loaderHelpers,Me=e.diagnosticHelpers,Le=e.api,Ne=e.internal,Object.assign(Le,{INTERNAL:Ne,invokeLibraryInitializers:be}),Object.assign(e.module,{config:ve(ze,{environmentVariables:{}})});const r={mono_wasm_bindings_is_ready:!1,config:e.module.config,diagnosticTracing:!1,nativeAbort:e=>{throw e||new Error("abort")},nativeExit:e=>{throw new Error("exit:"+e)}},l={gitHash:"f7d90799ce4ef09a0bb257852a57248d2a8fb8dd",config:e.module.config,diagnosticTracing:!1,maxParallelDownloads:16,enableDownloadRetry:!0,_loaded_files:[],loadedFiles:[],loadedAssemblies:[],libraryInitializers:[],workerNextNumber:1,actual_downloaded_assets_count:0,actual_instantiated_assets_count:0,expected_downloaded_assets_count:0,expected_instantiated_assets_count:0,afterConfigLoaded:i(),allDownloadsQueued:i(),allDownloadsFinished:i(),wasmCompilePromise:i(),runtimeModuleLoaded:i(),loadingWorkers:i(),is_exited:Ve,is_runtime_running:qe,assert_runtime_running:He,mono_exit:Xe,createPromiseController:i,getPromiseController:s,assertIsControllablePromise:a,mono_download_assets:oe,resolve_single_asset_path:ee,setup_proxy_console:j,set_thread_prefix:w,installUnhandledErrorHandler:Je,retrieve_asset_download:ie,invokeLibraryInitializers:be,isDebuggingSupported:Te,exceptions:t,simd:n,relaxedSimd:o};Object.assign(Ue,r),Object.assign(Pe,l)}(Fe);let nt,rt,it,st=!1,at=!1;async function lt(e){if(!at){if(at=!0,ke&&Pe.config.forwardConsoleLogsToWS&&void 0!==globalThis.WebSocket&&j("main",globalThis.console,globalThis.location.origin),We||Be(!1,"Null moduleConfig"),Pe.config||Be(!1,"Null moduleConfig.config"),"function"==typeof e){const t=e(Fe.api);if(t.ready)throw new Error("Module.ready couldn't be redefined.");Object.assign(We,t),Ee(We,t)}else{if("object"!=typeof e)throw new Error("Can't use moduleFactory callback of createDotnetRuntime function.");Ee(We,e)}await async function(e){if(Se){const e=await import(/*! webpackIgnore: true */"process"),t=14;if(e.versions.node.split(".")[0]<t)throw new Error(`NodeJS at '${e.execPath}' has too low version '${e.versions.node}', please use at least ${t}. See also https://aka.ms/dotnet-wasm-features`)}const t=/*! webpackIgnore: true */import.meta.url,o=t.indexOf("?");var n;if(o>0&&(Pe.modulesUniqueQuery=t.substring(o)),Pe.scriptUrl=t.replace(/\\/g,"/").replace(/[?#].*/,""),Pe.scriptDirectory=(n=Pe.scriptUrl).slice(0,n.lastIndexOf("/"))+"/",Pe.locateFile=e=>"URL"in globalThis&&globalThis.URL!==C?new URL(e,Pe.scriptDirectory).toString():M(e)?e:Pe.scriptDirectory+e,Pe.fetch_like=k,Pe.out=console.log,Pe.err=console.error,Pe.onDownloadResourceProgress=e.onDownloadResourceProgress,ke&&globalThis.navigator){const e=globalThis.navigator,t=e.userAgentData&&e.userAgentData.brands;t&&t.length>0?Pe.isChromium=t.some((e=>"Google Chrome"===e.brand||"Microsoft Edge"===e.brand||"Chromium"===e.brand)):e.userAgent&&(Pe.isChromium=e.userAgent.includes("Chrome"),Pe.isFirefox=e.userAgent.includes("Firefox"))}Ne.require=Se?await import(/*! webpackIgnore: true */"module").then((e=>e.createRequire(/*! webpackIgnore: true */import.meta.url))):Promise.resolve((()=>{throw new Error("require not supported")})),void 0===globalThis.URL&&(globalThis.URL=C)}(We)}}async function ct(e){return await lt(e),Ze=We.onAbort,Qe=We.onExit,We.onAbort=Ke,We.onExit=Ge,We.ENVIRONMENT_IS_PTHREAD?async function(){(function(){const e=new MessageChannel,t=e.port1,o=e.port2;t.addEventListener("message",(e=>{var n,r;n=JSON.parse(e.data.config),r=JSON.parse(e.data.monoThreadInfo),st?Pe.diagnosticTracing&&b("mono config already received"):(ve(Pe.config,n),Ue.monoThreadInfo=r,xe(),Pe.diagnosticTracing&&b("mono config received"),st=!0,Pe.afterConfigLoaded.promise_control.resolve(Pe.config),ke&&n.forwardConsoleLogsToWS&&void 0!==globalThis.WebSocket&&Pe.setup_proxy_console("worker-idle",console,globalThis.location.origin)),t.close(),o.close()}),{once:!0}),t.start(),self.postMessage({[l]:{monoCmd:"preload",port:o}},[o])})(),await Pe.afterConfigLoaded.promise,function(){const e=Pe.config;e.assets||Be(!1,"config.assets must be defined");for(const t of e.assets)X(t),Q[t.behavior]&&z.push(t)}(),setTimeout((async()=>{try{await oe()}catch(e){Xe(1,e)}}),0);const e=dt(),t=await Promise.all(e);return await ut(t),We}():async function(){var e;await Re(We),re();const t=dt();(async function(){try{const e=ee("dotnetwasm");await se(e),e&&e.pendingDownloadInternal&&e.pendingDownloadInternal.response||Be(!1,"Can't load dotnet.native.wasm");const t=await e.pendingDownloadInternal.response,o=t.headers&&t.headers.get?t.headers.get("Content-Type"):void 0;let n;if("function"==typeof WebAssembly.compileStreaming&&"application/wasm"===o)n=await WebAssembly.compileStreaming(t);else{ke&&"application/wasm"!==o&&E('WebAssembly resource does not have the expected content type "application/wasm", so falling back to slower ArrayBuffer instantiation.');const e=await t.arrayBuffer();Pe.diagnosticTracing&&b("instantiate_wasm_module buffered"),n=Ie?await Promise.resolve(new WebAssembly.Module(e)):await WebAssembly.compile(e)}e.pendingDownloadInternal=null,e.pendingDownload=null,e.buffer=null,e.moduleExports=null,Pe.wasmCompilePromise.promise_control.resolve(n)}catch(e){Pe.wasmCompilePromise.promise_control.reject(e)}})(),setTimeout((async()=>{try{D(),await oe()}catch(e){Xe(1,e)}}),0);const o=await Promise.all(t);return await ut(o),await Ue.dotnetReady.promise,await we(null===(e=Pe.config.resources)||void 0===e?void 0:e.modulesAfterRuntimeReady),await be("onRuntimeReady",[Fe.api]),Le}()}function dt(){const e=ee("js-module-runtime"),t=ee("js-module-native");if(nt&&rt)return[nt,rt,it];"object"==typeof e.moduleExports?nt=e.moduleExports:(Pe.diagnosticTracing&&b(`Attempting to import '${e.resolvedUrl}' for ${e.name}`),nt=import(/*! webpackIgnore: true */e.resolvedUrl)),"object"==typeof t.moduleExports?rt=t.moduleExports:(Pe.diagnosticTracing&&b(`Attempting to import '${t.resolvedUrl}' for ${t.name}`),rt=import(/*! webpackIgnore: true */t.resolvedUrl));const o=Y("js-module-diagnostics");return o&&("object"==typeof o.moduleExports?it=o.moduleExports:(Pe.diagnosticTracing&&b(`Attempting to import '${o.resolvedUrl}' for ${o.name}`),it=import(/*! webpackIgnore: true */o.resolvedUrl))),[nt,rt,it]}async function ut(e){const{initializeExports:t,initializeReplacements:o,configureRuntimeStartup:n,configureEmscriptenStartup:r,configureWorkerStartup:i,setRuntimeGlobals:s,passEmscriptenInternals:a}=e[0],{default:l}=e[1],c=e[2];s(Fe),t(Fe),c&&c.setRuntimeGlobals(Fe),await n(We),Pe.runtimeModuleLoaded.promise_control.resolve(),l((e=>(Object.assign(We,{ready:e.ready,__dotnet_runtime:{initializeReplacements:o,configureEmscriptenStartup:r,configureWorkerStartup:i,passEmscriptenInternals:a}}),We))).catch((e=>{if(e.message&&e.message.toLowerCase().includes("out of memory"))throw new Error(".NET runtime has failed to start, because too much memory was requested. Please decrease the memory by adjusting EmccMaximumHeapSize. See also https://aka.ms/dotnet-wasm-features");throw e}))}const ft=new class{withModuleConfig(e){try{return Ee(We,e),this}catch(e){throw Xe(1,e),e}}withOnConfigLoaded(e){try{return Ee(We,{onConfigLoaded:e}),this}catch(e){throw Xe(1,e),e}}withConsoleForwarding(){try{return ve(ze,{forwardConsoleLogsToWS:!0}),this}catch(e){throw Xe(1,e),e}}withExitOnUnhandledError(){try{return ve(ze,{exitOnUnhandledError:!0}),Je(),this}catch(e){throw Xe(1,e),e}}withAsyncFlushOnExit(){try{return ve(ze,{asyncFlushOnExit:!0}),this}catch(e){throw Xe(1,e),e}}withExitCodeLogging(){try{return ve(ze,{logExitCode:!0}),this}catch(e){throw Xe(1,e),e}}withElementOnExit(){try{return ve(ze,{appendElementOnExit:!0}),this}catch(e){throw Xe(1,e),e}}withInteropCleanupOnExit(){try{return ve(ze,{interopCleanupOnExit:!0}),this}catch(e){throw Xe(1,e),e}}withDumpThreadsOnNonZeroExit(){try{return ve(ze,{dumpThreadsOnNonZeroExit:!0}),this}catch(e){throw Xe(1,e),e}}withWaitingForDebugger(e){try{return ve(ze,{waitForDebugger:e}),this}catch(e){throw Xe(1,e),e}}withInterpreterPgo(e,t){try{return ve(ze,{interpreterPgo:e,interpreterPgoSaveDelay:t}),ze.runtimeOptions?ze.runtimeOptions.push("--interp-pgo-recording"):ze.runtimeOptions=["--interp-pgo-recording"],this}catch(e){throw Xe(1,e),e}}withConfig(e){try{return ve(ze,e),this}catch(e){throw Xe(1,e),e}}withConfigSrc(e){try{return e&&"string"==typeof e||Be(!1,"must be file path or URL"),Ee(We,{configSrc:e}),this}catch(e){throw Xe(1,e),e}}withVirtualWorkingDirectory(e){try{return e&&"string"==typeof e||Be(!1,"must be directory path"),ve(ze,{virtualWorkingDirectory:e}),this}catch(e){throw Xe(1,e),e}}withEnvironmentVariable(e,t){try{const o={};return o[e]=t,ve(ze,{environmentVariables:o}),this}catch(e){throw Xe(1,e),e}}withEnvironmentVariables(e){try{return e&&"object"==typeof e||Be(!1,"must be dictionary object"),ve(ze,{environmentVariables:e}),this}catch(e){throw Xe(1,e),e}}withDiagnosticTracing(e){try{return"boolean"!=typeof e&&Be(!1,"must be boolean"),ve(ze,{diagnosticTracing:e}),this}catch(e){throw Xe(1,e),e}}withDebugging(e){try{return null!=e&&"number"==typeof e||Be(!1,"must be number"),ve(ze,{debugLevel:e}),this}catch(e){throw Xe(1,e),e}}withApplicationArguments(...e){try{return e&&Array.isArray(e)||Be(!1,"must be array of strings"),ve(ze,{applicationArguments:e}),this}catch(e){throw Xe(1,e),e}}withRuntimeOptions(e){try{return e&&Array.isArray(e)||Be(!1,"must be array of strings"),ze.runtimeOptions?ze.runtimeOptions.push(...e):ze.runtimeOptions=e,this}catch(e){throw Xe(1,e),e}}withMainAssembly(e){try{return ve(ze,{mainAssemblyName:e}),this}catch(e){throw Xe(1,e),e}}withApplicationArgumentsFromQuery(){try{if(!globalThis.window)throw new Error("Missing window to the query parameters from");if(void 0===globalThis.URLSearchParams)throw new Error("URLSearchParams is supported");const e=new URLSearchParams(globalThis.window.location.search).getAll("arg");return this.withApplicationArguments(...e)}catch(e){throw Xe(1,e),e}}withApplicationEnvironment(e){try{return ve(ze,{applicationEnvironment:e}),this}catch(e){throw Xe(1,e),e}}withApplicationCulture(e){try{return ve(ze,{applicationCulture:e}),this}catch(e){throw Xe(1,e),e}}withResourceLoader(e){try{return Pe.loadBootResource=e,this}catch(e){throw Xe(1,e),e}}async download(){try{await async function(){lt(We),await Re(We),re(),D(),oe(),await Pe.allDownloadsFinished.promise}()}catch(e){throw Xe(1,e),e}}async create(){try{return this.instance||(this.instance=await async function(){return await ct(We),Fe.api}()),this.instance}catch(e){throw Xe(1,e),e}}async run(){try{return We.config||Be(!1,"Null moduleConfig.config"),this.instance||await this.create(),this.instance.runMainAndExit()}catch(e){throw Xe(1,e),e}}},mt=Xe,gt=ct;Ie||"function"==typeof globalThis.URL||Be(!1,"This browser/engine doesn't support URL API. Please use a modern version. See also https://aka.ms/dotnet-wasm-features"),"function"!=typeof globalThis.BigInt64Array&&Be(!1,"This browser/engine doesn't support BigInt64Array API. Please use a modern version. See also https://aka.ms/dotnet-wasm-features"),ft.withConfig(/*json-start*/{
  "mainAssemblyName": "MyBlazorWasmApp1.Stories",
  "resources": {
    "hash": "sha256-jMFdDdncQ85VLHXi2EamGuOvXHvEHZ6rYXzK92UHkCM=",
    "jsModuleNative": [
      {
        "name": "dotnet.native.r6yxfnnghj.js"
      }
    ],
    "jsModuleRuntime": [
      {
        "name": "dotnet.runtime.web2r9gqbh.js"
      }
    ],
    "wasmNative": [
      {
        "name": "dotnet.native.mvl0uofkzq.wasm",
        "hash": "sha256-wpwKqF8/caFS+jsDXwV1FxxCS85G/JGIcPyGBqsJB1Y=",
        "cache": "force-cache"
      }
    ],
    "coreAssembly": [
      {
        "virtualPath": "System.Runtime.InteropServices.JavaScript.wasm",
        "name": "System.Runtime.InteropServices.JavaScript.1qlb6weis6.wasm",
        "hash": "sha256-RxeSFjsuk6C7ECNNbfqkoLjluC8iflL3DB2R/2ICn8I=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Private.CoreLib.wasm",
        "name": "System.Private.CoreLib.juov5x1i7w.wasm",
        "hash": "sha256-dZcjaLZIVjaf6xlQHMhiQDLIcjUdoNb5lmg3A/A9l94=",
        "cache": "force-cache"
      }
    ],
    "assembly": [
      {
        "virtualPath": "BlazingStory.wasm",
        "name": "BlazingStory.l6q64tlwjr.wasm",
        "hash": "sha256-DTITaZKiGnJAyDKNrqipADb94fw64bmwiGrtBms6Ock=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "BlazingStory.Abstractions.wasm",
        "name": "BlazingStory.Abstractions.ik2qfu1uhm.wasm",
        "hash": "sha256-gsb1qDMjwPFayHEmzhiN9/si8n5HW4zvVa9Jgl7W9zM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "BlazingStory.Addons.wasm",
        "name": "BlazingStory.Addons.g8rwyorafm.wasm",
        "hash": "sha256-ILzV2wlH/B2LYspuUwvJSKMxmfWeOHkS8KiByEkCx68=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "BlazingStory.Addons.BuiltIns.wasm",
        "name": "BlazingStory.Addons.BuiltIns.775bt5dssk.wasm",
        "hash": "sha256-EzCzZovZJ0Yc1A+LW6abIQyaT7I4xH/59j2x1JStsdo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "BlazingStory.ToolKit.wasm",
        "name": "BlazingStory.ToolKit.9eox9z8yxc.wasm",
        "hash": "sha256-wQ6h1c26DKou3e/IznJEaLCKNjJzxjC5zU9oiPOlZJo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Authorization.wasm",
        "name": "Microsoft.AspNetCore.Authorization.wp5b4xwrtz.wasm",
        "hash": "sha256-TKbWsc5gCE+04slSkrqeNGTb7K8LiRW72Tm+1gzKxqU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Components.wasm",
        "name": "Microsoft.AspNetCore.Components.4payhwuyuh.wasm",
        "hash": "sha256-qPvavUxiWPAj51uJSf4LOekNKr4akGxRwNr0tHTe7iU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Components.Forms.wasm",
        "name": "Microsoft.AspNetCore.Components.Forms.ec8b0m8vzy.wasm",
        "hash": "sha256-AqlQLnd2AUu1G5e+NO3E6tGfGEty0Vbudprc7K4DUhI=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Components.Web.wasm",
        "name": "Microsoft.AspNetCore.Components.Web.j6d7zpk03e.wasm",
        "hash": "sha256-e4X4HowGyVJz7RZ2UWcSe9mssqm1IFxF6A1+4hM6+bk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Components.WebAssembly.wasm",
        "name": "Microsoft.AspNetCore.Components.WebAssembly.jt9rzx6tc4.wasm",
        "hash": "sha256-ic64++0UYs3SvlBLdn4+qwNWqwSG407oUE8u0eu16ak=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Metadata.wasm",
        "name": "Microsoft.AspNetCore.Metadata.bhaqktdl3q.wasm",
        "hash": "sha256-j++RGo33BhgVa0g5y83+TSz6cs3qmH+A5uEXVKZhtr8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.wasm",
        "name": "Microsoft.Extensions.Configuration.s6epnzhbd8.wasm",
        "hash": "sha256-7DgCCV/LB2eNVzPeNmZ5m2thp6g2kVsit8sogYccXwQ=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.Abstractions.wasm",
        "name": "Microsoft.Extensions.Configuration.Abstractions.wa2ojg94fi.wasm",
        "hash": "sha256-qOTZIeG9FsN1IhAycfVSHdCrxcSAjUo5paRUEfUJYhI=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.Binder.wasm",
        "name": "Microsoft.Extensions.Configuration.Binder.l7o5knh4f1.wasm",
        "hash": "sha256-L5Yax0NPgj+DBfLt7lTLrH7RZVUZ/GeLukF9oyqJ7pU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.FileExtensions.wasm",
        "name": "Microsoft.Extensions.Configuration.FileExtensions.ke1t9ns7xl.wasm",
        "hash": "sha256-XJGOiagKd+9+kp4Ty7ffOtSndz0apzO8w53DJnndXmI=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.Json.wasm",
        "name": "Microsoft.Extensions.Configuration.Json.k1hilos7yb.wasm",
        "hash": "sha256-OMRxiJp+p0LFgTIPl3n6LciGXvfYIdsr/Q88sETEF4w=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.DependencyInjection.wasm",
        "name": "Microsoft.Extensions.DependencyInjection.nun950pku8.wasm",
        "hash": "sha256-AWrVpqzFnmO0tCoEXAH0O7cE33X0Q6ZAa8lm5FRsMQs=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.DependencyInjection.Abstractions.wasm",
        "name": "Microsoft.Extensions.DependencyInjection.Abstractions.q6idsb19uh.wasm",
        "hash": "sha256-rmpyVV0DOgiihi5oxsjrJb01S8m67/Z7tnu/auVgXRw=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Diagnostics.wasm",
        "name": "Microsoft.Extensions.Diagnostics.f7zn8s1thp.wasm",
        "hash": "sha256-BaaWcXtZnSfywb3RXJuUpPEO8cLvZvlVHj3RJ10hPi0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Diagnostics.Abstractions.wasm",
        "name": "Microsoft.Extensions.Diagnostics.Abstractions.s62vewhs76.wasm",
        "hash": "sha256-1HqPfTJfvXForO2lQylRMNXcKDMdF9iD4VZSdaAX5qk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.FileProviders.Abstractions.wasm",
        "name": "Microsoft.Extensions.FileProviders.Abstractions.woov9lkdcy.wasm",
        "hash": "sha256-QNT4BJu7GuqNeD1vP+7WrlS/DU4fGK5J5eUsqmFJ4us=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.FileProviders.Physical.wasm",
        "name": "Microsoft.Extensions.FileProviders.Physical.4uvrzrkcx9.wasm",
        "hash": "sha256-7GjBXcHfSzByJQUNWXPp0f32BDtR3+Ti4+rkizb4Ljo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.FileSystemGlobbing.wasm",
        "name": "Microsoft.Extensions.FileSystemGlobbing.jnewm5d55t.wasm",
        "hash": "sha256-LyxUtk8WTHguCUKCEwF2EMVVVVGlw07/472TEvDsVfE=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Logging.wasm",
        "name": "Microsoft.Extensions.Logging.xispqyz68y.wasm",
        "hash": "sha256-V9FEOzDldETmKyiW3FTnoHpCZ4AZvoexnPaEIr09WZE=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Logging.Abstractions.wasm",
        "name": "Microsoft.Extensions.Logging.Abstractions.62zefycurg.wasm",
        "hash": "sha256-/h+gE4OLQdh3yuKWe0r8/2Si4zxcgw/fbOrbjMlX/ys=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Options.wasm",
        "name": "Microsoft.Extensions.Options.bs7sofzszu.wasm",
        "hash": "sha256-a/pTUQjNwOxhBz/lZgqVyyTyXrbdvyuKnFNyaVNH0Z0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Options.ConfigurationExtensions.wasm",
        "name": "Microsoft.Extensions.Options.ConfigurationExtensions.i2sp1sqcbf.wasm",
        "hash": "sha256-pnQwodkGYFmAdSWA6bpOdTxxgWZdqCkOlv0Ku086DvE=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Primitives.wasm",
        "name": "Microsoft.Extensions.Primitives.ub9q6nskn2.wasm",
        "hash": "sha256-RgwNm087C7Ho0ns9NEvi/NHblJKJLGtNQvt9F9cpNrc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Validation.wasm",
        "name": "Microsoft.Extensions.Validation.y25xlv978u.wasm",
        "hash": "sha256-+vj2jK2O52sn5ULpvYCNDVIjlE7JxEgdA5PDj/ukV6k=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.JSInterop.wasm",
        "name": "Microsoft.JSInterop.zhfbg74p0e.wasm",
        "hash": "sha256-ydDa5CE8UQzUry68CwEPAVbu361httdsDXT2AQmuMK0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.JSInterop.WebAssembly.wasm",
        "name": "Microsoft.JSInterop.WebAssembly.vi1uw8swyy.wasm",
        "hash": "sha256-yZy5My09/M7owHVzBcnK3BPDrUtQH3/wmFTaiPscAgo=",
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
        "name": "Toolbelt.Blazor.SplitContainer.l0dpa915fg.wasm",
        "hash": "sha256-JWxXnYsXnu1lqogVRKx3jyo3/tYFIYIEu/Xw99pGz1I=",
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
        "name": "Microsoft.CSharp.6q6v6mynmi.wasm",
        "hash": "sha256-zvHf27ahVxlI2AFXaxGFeC6gsmk+KU/jayZSX/aTduc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.VisualBasic.Core.wasm",
        "name": "Microsoft.VisualBasic.Core.7ahatwz856.wasm",
        "hash": "sha256-AZ1OBnZZieuRTiDGO1izE2Hj+dIr0EJKAZci6Ip77ZA=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.VisualBasic.wasm",
        "name": "Microsoft.VisualBasic.sphnc8qe37.wasm",
        "hash": "sha256-cIXni6yabmZg2CT9PfUwYJh4QXAikeZhlBhiKNb90e4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Win32.Primitives.wasm",
        "name": "Microsoft.Win32.Primitives.x8i8s5qlfi.wasm",
        "hash": "sha256-n0FdKoueatHa5nv8Parg79didiuVV4i9dXltgpW8dQg=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Win32.Registry.wasm",
        "name": "Microsoft.Win32.Registry.vw7gs5u6x3.wasm",
        "hash": "sha256-D7z0gQYE2qYwzCqHrxaeU1ONTQE4d9nrxWJi9SyjpPs=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.AppContext.wasm",
        "name": "System.AppContext.tcwsqw4zp2.wasm",
        "hash": "sha256-zWwDUGTwmdAPAg/wyxnicWnEIbHIRhP5a0zbwQPvCxE=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Buffers.wasm",
        "name": "System.Buffers.wv1fr9u78z.wasm",
        "hash": "sha256-tOllShg6BTGCSxAMEiFJZnBzBper74Ms9lzhmEVuJJU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Collections.Concurrent.wasm",
        "name": "System.Collections.Concurrent.mjcda8szaq.wasm",
        "hash": "sha256-PpV2WEqeayibcyObR8Epqg1/MpfRS5mEOBUO3z32Vh0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Collections.Immutable.wasm",
        "name": "System.Collections.Immutable.uynhbcg8xd.wasm",
        "hash": "sha256-atEBwz5tJz7oy0K/xfppy/nEjzFsGcCEcRJ+zhf8T9I=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Collections.NonGeneric.wasm",
        "name": "System.Collections.NonGeneric.lxlhsh57kt.wasm",
        "hash": "sha256-SRkmsNF3uN58PqYIojSfut6VHMqM685aGpX1WN2Xo6w=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Collections.Specialized.wasm",
        "name": "System.Collections.Specialized.m4ce5na42h.wasm",
        "hash": "sha256-GkpbVQluCXi10nHKQPgEm8BO3UbKfyXRY+L8dxB7rB8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Collections.wasm",
        "name": "System.Collections.ggdo1btk42.wasm",
        "hash": "sha256-INftXL/eX4gvbC6W9jVLm+Dik2Qo+zrFT642kjjaKJ8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ComponentModel.Annotations.wasm",
        "name": "System.ComponentModel.Annotations.6kamo06ruz.wasm",
        "hash": "sha256-9hZsG9bVQPQITjVg++HGKgFy1dTaS4dh/BfUzMujzXM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ComponentModel.DataAnnotations.wasm",
        "name": "System.ComponentModel.DataAnnotations.xbmwog2f1v.wasm",
        "hash": "sha256-844msnT37yyqsutolJI3McEBG0NGUSUmEmIHwX8cWS8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ComponentModel.EventBasedAsync.wasm",
        "name": "System.ComponentModel.EventBasedAsync.eqv8o4kw6a.wasm",
        "hash": "sha256-pjqbMZvq40M0wwGxtsMzv6R1FbhvtZrg56PFuZCSJI8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ComponentModel.Primitives.wasm",
        "name": "System.ComponentModel.Primitives.eotivnapz7.wasm",
        "hash": "sha256-Qn6Jw4Hj4Svhr+97SclbHh8il6zpaUNl2YAw+W9zQg8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ComponentModel.TypeConverter.wasm",
        "name": "System.ComponentModel.TypeConverter.x4d9maevo7.wasm",
        "hash": "sha256-wYi1dPW6vMdZfqA5R5/8asPOTpDTYqU7nCViLbMhvLY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ComponentModel.wasm",
        "name": "System.ComponentModel.o0bt2abfmk.wasm",
        "hash": "sha256-+Agcf5ODmGOlQrZjFnA8aUvS4GrMuO+9STI9gii3Wt8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Configuration.wasm",
        "name": "System.Configuration.b12keys70e.wasm",
        "hash": "sha256-T11gpeE9bWEiaUZu1P/yt9BfQ2vUNyjIHUBKZLVddMI=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Console.wasm",
        "name": "System.Console.byghioepvx.wasm",
        "hash": "sha256-E0JTds1IkVtGA1f4mJnBrcbaS4Q2B30mL52ccMiMf8s=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Core.wasm",
        "name": "System.Core.crs2w9a4bs.wasm",
        "hash": "sha256-GIJwftQRNikWF1RvRjUn1rcS2OGesGH1tfRArkLinzs=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Data.Common.wasm",
        "name": "System.Data.Common.bgo75n328l.wasm",
        "hash": "sha256-dfdfKru3HaThBupxbYUSMrUvFowLYcClUnNCsH3jlMY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Data.DataSetExtensions.wasm",
        "name": "System.Data.DataSetExtensions.x6usn7lchy.wasm",
        "hash": "sha256-JzVzXOoOTku9XUWOO1emR9P3DqOje01K5Dhib9hNRGQ=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Data.wasm",
        "name": "System.Data.7l0iizrfot.wasm",
        "hash": "sha256-hf6YOUn9SRa9D1R6a4QfBuC8H+J/VNwC46TbP6deErM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.Contracts.wasm",
        "name": "System.Diagnostics.Contracts.zb5ovgp6rz.wasm",
        "hash": "sha256-AQBlWZgKXt+jX68NIlZiIJs7glHKv12MRp5wgMg0wzo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.Debug.wasm",
        "name": "System.Diagnostics.Debug.pzraghcsxr.wasm",
        "hash": "sha256-y59odQsgIVG+NZw2PsRsIi0GrGpMY/pV0O8PHg1v5A4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.DiagnosticSource.wasm",
        "name": "System.Diagnostics.DiagnosticSource.y8ylk8ocyk.wasm",
        "hash": "sha256-XivkJZZaTNeflPHZ4EC9GO51pCyBAXpMQFrm777ZnqM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.FileVersionInfo.wasm",
        "name": "System.Diagnostics.FileVersionInfo.qf6phldvxs.wasm",
        "hash": "sha256-Kl4M3+1TGMC+K7b4Sr6vBxTMnX19QlSbDk/ci3F4b/c=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.Process.wasm",
        "name": "System.Diagnostics.Process.j6qkqs1xs6.wasm",
        "hash": "sha256-daRDl3Mc9bv8EEzzzKyblnQB+jEUDMuPwyN1kKL7YOc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.StackTrace.wasm",
        "name": "System.Diagnostics.StackTrace.zzgq407zst.wasm",
        "hash": "sha256-j4zlxg4q78gwEUo3Qx5RlaCBBR+kNDW2sOfguhrtfoA=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.TextWriterTraceListener.wasm",
        "name": "System.Diagnostics.TextWriterTraceListener.k3ba8zyfyp.wasm",
        "hash": "sha256-UFUL+alEr9O+yhLbkv//ZXtmZ2X0Os5sKSJp6JoUnjQ=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.Tools.wasm",
        "name": "System.Diagnostics.Tools.awyyza3767.wasm",
        "hash": "sha256-KkBnCR7G5FBX8tGlCinYJ4TSPYcLMU6ueTcbleZrZQY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.TraceSource.wasm",
        "name": "System.Diagnostics.TraceSource.w5t42xm4f8.wasm",
        "hash": "sha256-pDL5LZSQ4CMsJxVWxbHLxaP4Q+MqkGjrBdMx6fJZGV0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.Tracing.wasm",
        "name": "System.Diagnostics.Tracing.zw8dj8hqv5.wasm",
        "hash": "sha256-YxFrREqUafoKZ53mdMUG3HfbQumm17/vd0TvU1vHKSo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Drawing.Primitives.wasm",
        "name": "System.Drawing.Primitives.cfbgr8gzc9.wasm",
        "hash": "sha256-KIfHUPIfDR6Ywkzl7rTWpqQSK79Audc8HtsbuHMtWfk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Drawing.wasm",
        "name": "System.Drawing.wemiyntni6.wasm",
        "hash": "sha256-+O9fNPcx8KB8jege3YahTq+/iuzNuO9UYKoQoMoyBf4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Dynamic.Runtime.wasm",
        "name": "System.Dynamic.Runtime.83wdfbg9dk.wasm",
        "hash": "sha256-TGFyS+jFkDen9z1YA9ldvjblDloMLSR0JVkIG939e2c=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Formats.Asn1.wasm",
        "name": "System.Formats.Asn1.wu3h4anva6.wasm",
        "hash": "sha256-yEnDGI2Y7GNzqmaJlxVvVTKrhOn9Fiv7zaUPLF0AEMM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Formats.Tar.wasm",
        "name": "System.Formats.Tar.hgqfxjarhi.wasm",
        "hash": "sha256-T3/MNIaoKAHoohN5fH2dQ2FzhVxogaA/TiUyec/eY6c=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Globalization.Calendars.wasm",
        "name": "System.Globalization.Calendars.ygja8e5f6z.wasm",
        "hash": "sha256-EpHMe6nGJBxqyFqYIGjdi3Hjp2wlWJYEDalWUgFyrms=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Globalization.Extensions.wasm",
        "name": "System.Globalization.Extensions.lymlf1y54s.wasm",
        "hash": "sha256-gyUasIzIw5fxDNrNSl/l2mSZwREFh9TSWjpz+SZgn+I=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Globalization.wasm",
        "name": "System.Globalization.wn8krsslap.wasm",
        "hash": "sha256-JKy0P8bL3ZXo+gNPiaH/NkV9OMY2tYcCRWKkHs2LZ24=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.Compression.Brotli.wasm",
        "name": "System.IO.Compression.Brotli.01wsvlce97.wasm",
        "hash": "sha256-3DvgyPsIGsiXu7hi4qXNzJr8iMJXoP3WSCZMfu0IC7c=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.Compression.FileSystem.wasm",
        "name": "System.IO.Compression.FileSystem.fhhysos6ne.wasm",
        "hash": "sha256-A8kP6Pr6BKlGCtTFieHlCv5LuXACTAgDNsq3Fbb5uP8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.Compression.ZipFile.wasm",
        "name": "System.IO.Compression.ZipFile.b550bh7d4o.wasm",
        "hash": "sha256-8Waqn+/oBMyLZhHMdJDpr/uGZbXrfXDNtxwUGqx9LVg=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.Compression.wasm",
        "name": "System.IO.Compression.xga7hhx237.wasm",
        "hash": "sha256-P6qVNLsMSy3fbCkBUcAdoRNN74iTNJfBw2g+5HXd+lY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.FileSystem.AccessControl.wasm",
        "name": "System.IO.FileSystem.AccessControl.gaf5qnpqkp.wasm",
        "hash": "sha256-qAKG1N95IVYn+7YBdwFlK6AXkMSGyWWr2ZlPSTUNG4E=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.FileSystem.DriveInfo.wasm",
        "name": "System.IO.FileSystem.DriveInfo.sbt0tte66b.wasm",
        "hash": "sha256-eMXm6btJQ0QFaiKzUi/3XgyFgVfes482cbVhEnDJtFw=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.FileSystem.Primitives.wasm",
        "name": "System.IO.FileSystem.Primitives.rhlpjpj7v7.wasm",
        "hash": "sha256-DyDwUHL8mFAr69NKTyG1JY+OKNWB/JMGGX5a1uONPSU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.FileSystem.Watcher.wasm",
        "name": "System.IO.FileSystem.Watcher.c0tiebun14.wasm",
        "hash": "sha256-pCK6jzW5QjGcxz9ZG2C7fd4UpJANzHMigB3St6qc4zw=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.FileSystem.wasm",
        "name": "System.IO.FileSystem.qjunxa5xpz.wasm",
        "hash": "sha256-mkxAv0gjDBDMNzIy5+IDgvmTAAxg6TMquTT7e65g1F8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.IsolatedStorage.wasm",
        "name": "System.IO.IsolatedStorage.st1kzda9co.wasm",
        "hash": "sha256-QH3skcz0NjYVmE62qspNQN3f+OQh2dtS1KnGfOJrzPo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.MemoryMappedFiles.wasm",
        "name": "System.IO.MemoryMappedFiles.dud9xjr6ji.wasm",
        "hash": "sha256-VctFxV2zTysc44Ik9MNnaVcYNGKIW1qRNBL8SDUp6Hc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.Pipelines.wasm",
        "name": "System.IO.Pipelines.7hyknmpn75.wasm",
        "hash": "sha256-i8phnvyiXxVXX07M/rZ9kqp+tT0XUh8HhUY6SEVyzNw=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.Pipes.AccessControl.wasm",
        "name": "System.IO.Pipes.AccessControl.hhnvgw5lgh.wasm",
        "hash": "sha256-W1cQpoAbaGvRrijYQSg2TyQyEVoLz/l5hRw9DqieYnA=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.Pipes.wasm",
        "name": "System.IO.Pipes.hiwotp9hyv.wasm",
        "hash": "sha256-mcsaGmJhcq4USe1jy18IW+3lrRfmIb5i4tG0UXexdZ8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.UnmanagedMemoryStream.wasm",
        "name": "System.IO.UnmanagedMemoryStream.ww8ebi8a3j.wasm",
        "hash": "sha256-oJdJc74VKlJiM9z++KKgsotHg/xbtOjsZYLtb71Z1+k=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.wasm",
        "name": "System.IO.gc5ray86oq.wasm",
        "hash": "sha256-0GUrSc5JU1BwifLiLKAvrLtnHTQ0326AwH3nWSGfBDk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Linq.AsyncEnumerable.wasm",
        "name": "System.Linq.AsyncEnumerable.07kzmng77i.wasm",
        "hash": "sha256-/KJFrMqgH1odpFXwoeOCeh3/H+BUuIRessP96epMnp0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Linq.Expressions.wasm",
        "name": "System.Linq.Expressions.hnnxjo0l2j.wasm",
        "hash": "sha256-/YgHfGfGNUNpDMDLvZkLdUm+fQJr2Z4owh+alJuNNdU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Linq.Parallel.wasm",
        "name": "System.Linq.Parallel.glk2arw06k.wasm",
        "hash": "sha256-xGr5NkI7XLhsN9qVA5cMhQhK+5PEgjwO48JxZ54Vrzc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Linq.Queryable.wasm",
        "name": "System.Linq.Queryable.9agv5eul41.wasm",
        "hash": "sha256-z15deuvyAEHCF08A9yc3pz20pHves4ClLhZPYmAH8AM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Linq.wasm",
        "name": "System.Linq.9ffwb85ft1.wasm",
        "hash": "sha256-6863YqGj4DfRgk8BzWuGVMhjf6cQhnLkOKdmgekxdL4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Memory.wasm",
        "name": "System.Memory.psbhtw4rwf.wasm",
        "hash": "sha256-+RLBpFjrVbIwTSKhINZrtdQDpOoyllgMXodgNRVBKbQ=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.Http.Json.wasm",
        "name": "System.Net.Http.Json.u38okk8pdw.wasm",
        "hash": "sha256-CmNQV8d7i/N2YUw5XpKfN1E27LtiVieOrvNahwkKC3o=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.Http.wasm",
        "name": "System.Net.Http.hprl4g0ei6.wasm",
        "hash": "sha256-hcNBh17l4O1AcN1THVeQsGfx2deOoYtCFPwcMlq3kk8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.HttpListener.wasm",
        "name": "System.Net.HttpListener.a7w2mz50g1.wasm",
        "hash": "sha256-hqGJmP41JWMkR7TG2ez2EzZ13OlLETn+PvzpFXFc5dc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.Mail.wasm",
        "name": "System.Net.Mail.0oz2x1giz0.wasm",
        "hash": "sha256-8PkwMPzbOS3W/TC69m45HCmtVxAE1uvQRC0VYWWhMRo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.NameResolution.wasm",
        "name": "System.Net.NameResolution.ofawvy3ec7.wasm",
        "hash": "sha256-LAy4wC5p8qDwFOu0prSL6MkS5g4O08aY5I0WMvIkedk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.NetworkInformation.wasm",
        "name": "System.Net.NetworkInformation.5gv80uqpl0.wasm",
        "hash": "sha256-NWlmqw8qxB0qPtlbA/GvrBctcRTgqY+Zaf6bVdfuYG4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.Ping.wasm",
        "name": "System.Net.Ping.1ac0k1f2o4.wasm",
        "hash": "sha256-KeaB0vW9oRc8hKruISyUsCSwwOQ68YFwLuMS1R6mznY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.Primitives.wasm",
        "name": "System.Net.Primitives.k1ozvs2jbp.wasm",
        "hash": "sha256-iEzW5J4i+VrLj2ofIqaj7PdebFvDYzb840eVElD08K4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.Quic.wasm",
        "name": "System.Net.Quic.irg6mf6a29.wasm",
        "hash": "sha256-XuzmMLvQZO8sKTt1ACxN/hnorHkoScx9nUq3dWseRZE=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.Requests.wasm",
        "name": "System.Net.Requests.90fz04w7mu.wasm",
        "hash": "sha256-uYZXCdg3nBdou2fVQ+d9INkYBkrVuMb/ZZjVFyukAKI=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.Security.wasm",
        "name": "System.Net.Security.jfw2oy4ger.wasm",
        "hash": "sha256-rzC9R/FFg/smWZV+ycyRnXEYTJPkc5GkmVQDgrg9H8U=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.ServerSentEvents.wasm",
        "name": "System.Net.ServerSentEvents.kyt6lsc4yl.wasm",
        "hash": "sha256-LMIsU3qDhpkP3ValJegxT6kBfpWDKcC/gQ4a/G5rUT8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.ServicePoint.wasm",
        "name": "System.Net.ServicePoint.ut6twkdfba.wasm",
        "hash": "sha256-0kd6zNt6XAhErIL/JvnVb5WhQmLHGa7O4r9mOCaCdjY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.Sockets.wasm",
        "name": "System.Net.Sockets.x8v2el0sv6.wasm",
        "hash": "sha256-T7ORwtwaDvPyvlklTX3q3RjWlvkmZ8A2HYRYK2/0KMs=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.WebClient.wasm",
        "name": "System.Net.WebClient.czcdllfidj.wasm",
        "hash": "sha256-CGTR3Az6JZ3QB1MIohRXCX0XijTRS2df6yotlJI+dQQ=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.WebHeaderCollection.wasm",
        "name": "System.Net.WebHeaderCollection.bce2110s0r.wasm",
        "hash": "sha256-G/ogYmbW5rkKKaDXZXzfYVKw/ut2mxdaAm/2uHDWLq0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.WebProxy.wasm",
        "name": "System.Net.WebProxy.5be8kwoend.wasm",
        "hash": "sha256-cQ63t0+pxlovMwoEf1eqjZCs/Fgvrc44qksU+tHkouE=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.WebSockets.Client.wasm",
        "name": "System.Net.WebSockets.Client.przg52xxg4.wasm",
        "hash": "sha256-6ymXDNwxS1yC+D4brRafFvGYCxXoQTIAqZ4b7pvxRXY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.WebSockets.wasm",
        "name": "System.Net.WebSockets.xlwui9bnh8.wasm",
        "hash": "sha256-a3n+jxPSgMa3cSRmppA6kGgWreYux0/j+j/o+SV5jmo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.wasm",
        "name": "System.Net.m966t71d0p.wasm",
        "hash": "sha256-xpFdoBA8uvHsmDNfY4Q+voDkqhoRWL/OL9ocN280cEI=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Numerics.Vectors.wasm",
        "name": "System.Numerics.Vectors.z7brvfqdcr.wasm",
        "hash": "sha256-cZP5c7btHw6ZzifSUD/sdZnk7/ClXBYaxZWVfagmZeI=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Numerics.wasm",
        "name": "System.Numerics.ag24ir3prp.wasm",
        "hash": "sha256-lgwAOy/jT/jYpbLMPsynoGm6/673OpvPVIchOAz3osA=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ObjectModel.wasm",
        "name": "System.ObjectModel.xh1vpnmjgs.wasm",
        "hash": "sha256-q1FqdH7yefTzxmYdAPicRkU2dE2tzuvMAvKBX/JcDD0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Private.DataContractSerialization.wasm",
        "name": "System.Private.DataContractSerialization.l9evwq6x3r.wasm",
        "hash": "sha256-+cHeSmTAWB13XR/X/DU4LuHBLH7FWvDN/6yrao9HpBM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Private.Uri.wasm",
        "name": "System.Private.Uri.w7q0j7zx27.wasm",
        "hash": "sha256-RJyqxBvRVgbmsbfYJ4rz7biALfDCoJ2UNIrstErC0u0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Private.Xml.Linq.wasm",
        "name": "System.Private.Xml.Linq.1ei6wtrgkp.wasm",
        "hash": "sha256-Z/mvH93p1uq4fc1w03Q8lFU7xDrlJpFSQAF5x/K7dU8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Private.Xml.wasm",
        "name": "System.Private.Xml.q1h1022344.wasm",
        "hash": "sha256-csu8wJjFJmfh9RQaxG7wdCWJYmyYrsITHzGDL20yF4o=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Reflection.DispatchProxy.wasm",
        "name": "System.Reflection.DispatchProxy.g2yz9xctp2.wasm",
        "hash": "sha256-yJsdOUe+DYOubIee0JxJiMsxjdzLOXzZMn30z7v66QE=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Reflection.Emit.ILGeneration.wasm",
        "name": "System.Reflection.Emit.ILGeneration.kwkwfmyend.wasm",
        "hash": "sha256-LKeb+EdwDOLgj5jIqAt7/1UzqUXZKoN4ccNGW0wL560=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Reflection.Emit.Lightweight.wasm",
        "name": "System.Reflection.Emit.Lightweight.2uboz65mda.wasm",
        "hash": "sha256-6vyrs19B7w9dGqSInB8X+1zDKkZjRGQXOBMPMF+whIY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Reflection.Emit.wasm",
        "name": "System.Reflection.Emit.1ir4e7atax.wasm",
        "hash": "sha256-OY62vl/YQB4MpLAWUX6Bx8vXm2f42kXNsemwVTycy7I=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Reflection.Extensions.wasm",
        "name": "System.Reflection.Extensions.ryls3wcjtu.wasm",
        "hash": "sha256-M0GewU6PblUI8DcaT7PKvFGAFy46cFUJMfKWcMs1j1I=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Reflection.Metadata.wasm",
        "name": "System.Reflection.Metadata.685kento59.wasm",
        "hash": "sha256-ik4C5OQWIByvdp3BJr4ZAvhYrVAmczfJg996oxb5iRg=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Reflection.Primitives.wasm",
        "name": "System.Reflection.Primitives.xfo3jok9qj.wasm",
        "hash": "sha256-g45DkpgeojTNQ26Ui2ivG0G4SCGpd9ul+Y7R3MfshQU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Reflection.TypeExtensions.wasm",
        "name": "System.Reflection.TypeExtensions.do7opdhcum.wasm",
        "hash": "sha256-3VlNajP6ff9vQv/QYTVh7KOvXJygqVKDmGdnrKJA6Ss=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Reflection.wasm",
        "name": "System.Reflection.rlpuhk2fom.wasm",
        "hash": "sha256-4cGtWeTBsnjb3aOzNjh6scV1i+F1ljcrgwQfFkwfG40=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Resources.Reader.wasm",
        "name": "System.Resources.Reader.4jh2hk8ou1.wasm",
        "hash": "sha256-QMaaA8pzSeI0UVcfJ2C3HpDn/pTDfk/eHXVu2jg4W1w=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Resources.ResourceManager.wasm",
        "name": "System.Resources.ResourceManager.l74nv8dd2y.wasm",
        "hash": "sha256-ESS3qolG2qdzo+jTNQA/TLzb78BsEUegO3ysF2X/Y4Q=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Resources.Writer.wasm",
        "name": "System.Resources.Writer.0w0j4onmez.wasm",
        "hash": "sha256-QF8xaglb5gAi5WxshSa01YnbT+HIuu48iOaW8ACFb/o=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.CompilerServices.Unsafe.wasm",
        "name": "System.Runtime.CompilerServices.Unsafe.psp9r1rjx3.wasm",
        "hash": "sha256-2fnX1oqwXtZiditXWXvyfeNQ991HeEhpDNCAbNsUx9o=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.CompilerServices.VisualC.wasm",
        "name": "System.Runtime.CompilerServices.VisualC.ddaiepa6v6.wasm",
        "hash": "sha256-/wivVVMBx+KQpVXpvy2D9Kg+1KiyxcXjK+yP3znQhnQ=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.Extensions.wasm",
        "name": "System.Runtime.Extensions.7mo8aanhcl.wasm",
        "hash": "sha256-YW/SHM9hXOfEWgV/EZDdu2WIw2v8BVweWJ5rBY/3TTw=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.Handles.wasm",
        "name": "System.Runtime.Handles.302mjdnyau.wasm",
        "hash": "sha256-o+pR36TOd2wxkFe3d8fVj8Pabv2VZGXCLg+VPh1IY/A=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.InteropServices.RuntimeInformation.wasm",
        "name": "System.Runtime.InteropServices.RuntimeInformation.7hdhpo4imj.wasm",
        "hash": "sha256-hcjUpnUQtPH9m4OI3sZWZuy9LnncUvAtwOYKZri6zq8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.InteropServices.wasm",
        "name": "System.Runtime.InteropServices.dgvqyfg0pe.wasm",
        "hash": "sha256-vR9sPZnV+edEk044pKra/H9KvrC3ReWea8MM6eYjTYA=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.Intrinsics.wasm",
        "name": "System.Runtime.Intrinsics.4opvn70127.wasm",
        "hash": "sha256-tMYK4S9nyrcF3OBc4a0C9kcUX6OE1qYRu/l/Wufz9EI=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.Loader.wasm",
        "name": "System.Runtime.Loader.g8rkjfh4qm.wasm",
        "hash": "sha256-IJqvBpg6PV2jr1MidEJA2NptmQBDH4PxOwnudXzF0fA=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.Numerics.wasm",
        "name": "System.Runtime.Numerics.v1ulx56ak0.wasm",
        "hash": "sha256-nZ9wPx+0oBT8ILZR72gGp89L+D3u3bVDJPT7pIQv8F8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.Serialization.Formatters.wasm",
        "name": "System.Runtime.Serialization.Formatters.rvz6whau2l.wasm",
        "hash": "sha256-Jx8GlV47mhMiNlPp3wpJQ8iezasfZy4AFS6S+r4ThNg=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.Serialization.Json.wasm",
        "name": "System.Runtime.Serialization.Json.qudur1ne02.wasm",
        "hash": "sha256-ojAOZ21pR1AzC5H27z+nt2J4mKuqSGtzHXbHMfae5Kw=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.Serialization.Primitives.wasm",
        "name": "System.Runtime.Serialization.Primitives.j92gsjj8kn.wasm",
        "hash": "sha256-iZiMFOaA0zvnRKFXWXKyGUSfMXoMlvSTDPyu/ZYxa/g=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.Serialization.Xml.wasm",
        "name": "System.Runtime.Serialization.Xml.ukymcjvxx5.wasm",
        "hash": "sha256-jkC8gqDFuNptgkezc3tz3BXUDz0LOP2hHdMXLXU4A2Q=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.Serialization.wasm",
        "name": "System.Runtime.Serialization.z7e61m03nm.wasm",
        "hash": "sha256-gfkNnDMRhtC/eV5d7S7dzKs6oGz0A95oLDx0PGmiYzU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.wasm",
        "name": "System.Runtime.tendud3ylb.wasm",
        "hash": "sha256-O2Z4wMF9THG6aFh0s5Y6DR9aNRClszGLH0SsektBg9U=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.AccessControl.wasm",
        "name": "System.Security.AccessControl.cv3mvcgnqt.wasm",
        "hash": "sha256-6FP/EmubPWfB4Ho6x0CBkmUPKAmjIloGcIYVEXitWh4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Claims.wasm",
        "name": "System.Security.Claims.vv0su8gg3e.wasm",
        "hash": "sha256-e/G5sWQc/d0vdxES+8ljo1iJA+mPXKONKZl9V1OU0gs=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Cryptography.Algorithms.wasm",
        "name": "System.Security.Cryptography.Algorithms.u07jl1h8ni.wasm",
        "hash": "sha256-TpNTuXxlGfc/5+pJYitTfDCK7AgCt0OnDj44SMn4QSs=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Cryptography.Cng.wasm",
        "name": "System.Security.Cryptography.Cng.nx9df96u5j.wasm",
        "hash": "sha256-dXkPSuaLu+/R72TqK3HP8g4NPdHFp/TwloOyWqHAzXk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Cryptography.Csp.wasm",
        "name": "System.Security.Cryptography.Csp.iv0qssebou.wasm",
        "hash": "sha256-Ek325qFgVLGIMfnhltzDbYR+iN77sxnUe9YINbIK4Ew=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Cryptography.Encoding.wasm",
        "name": "System.Security.Cryptography.Encoding.yvcfg16da3.wasm",
        "hash": "sha256-Ar3GH8OCsjvdksghGoEfblenEdgWEJLjZQOxi/cJW7A=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Cryptography.OpenSsl.wasm",
        "name": "System.Security.Cryptography.OpenSsl.1t0tq6hwlx.wasm",
        "hash": "sha256-Veuv5yjWf20hAREWWjzBwzOwwQ89oZFBTGrpLjpSSfY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Cryptography.Primitives.wasm",
        "name": "System.Security.Cryptography.Primitives.lvrn81knoi.wasm",
        "hash": "sha256-4ZhdozjdVjIT1N21cSDLRxg3N9BycjEZzFDoBafvxyk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Cryptography.X509Certificates.wasm",
        "name": "System.Security.Cryptography.X509Certificates.sjrlqg0zgl.wasm",
        "hash": "sha256-uNjKBp3dK985Qoxa/EeVMQFVO8Tty4GpS2FmXW3yo/0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Cryptography.wasm",
        "name": "System.Security.Cryptography.d6btj6o3of.wasm",
        "hash": "sha256-KxfMS6/n71XmcYW2DIVVEGfu7TdNOp7Ipht//dLLVbQ=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Principal.Windows.wasm",
        "name": "System.Security.Principal.Windows.rdh8dkjzrz.wasm",
        "hash": "sha256-//bfJMPEutxUoyncg8HhD318cKvqxBNMeDjFLf8CnIw=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Principal.wasm",
        "name": "System.Security.Principal.cb0msvc61t.wasm",
        "hash": "sha256-GGPWQO9V2/MZq00yPykTbnQhtQJthnVZOyte5kRpds8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.SecureString.wasm",
        "name": "System.Security.SecureString.kx9zmavdgn.wasm",
        "hash": "sha256-CIvvtUXtrv45XtyhoHJFfSLJT5CZeQ08JrqEWkPhU0U=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.wasm",
        "name": "System.Security.4vhqo5un91.wasm",
        "hash": "sha256-8MiHiGyFi6Au8xRdrJwRD5mdQZFzL7noN35GE7iKydk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ServiceModel.Web.wasm",
        "name": "System.ServiceModel.Web.ol7pik4njh.wasm",
        "hash": "sha256-ROvgJE3FXPPa2fX2qUEjVXD+jL1pGbKeDk9AOvp9huM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ServiceProcess.wasm",
        "name": "System.ServiceProcess.x9ceibitb3.wasm",
        "hash": "sha256-pfO/vjq/gY0nSPKJHwstdvgqHhV1rzFXb+O9NuVs0Bc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Text.Encoding.CodePages.wasm",
        "name": "System.Text.Encoding.CodePages.xr9jzwuhbs.wasm",
        "hash": "sha256-w6TbPH0ZGsXE2moaDxCXWkpHk1f6eRSadPLaZWA5Iis=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Text.Encoding.Extensions.wasm",
        "name": "System.Text.Encoding.Extensions.p7ksnf6eqq.wasm",
        "hash": "sha256-VUgVzZH1+UB5A9+N8SyKKU2kDlMc7ktp+GVVtdupM4w=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Text.Encoding.wasm",
        "name": "System.Text.Encoding.gylgsn1uaa.wasm",
        "hash": "sha256-2EQFlGI4BVCJulJd3F2h6T0EtsGSlAK4WutHAaLBSt0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Text.Encodings.Web.wasm",
        "name": "System.Text.Encodings.Web.43wbm7qo0m.wasm",
        "hash": "sha256-0JZTS0SdWRSnmJkSJLLPyMBE6ssKLcEpBC0qk9MxcWM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Text.Json.wasm",
        "name": "System.Text.Json.3fm0130zj1.wasm",
        "hash": "sha256-gUHsAryISczen4HIrfQZYvKXlvqHArroVlp47LwKsI0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Text.RegularExpressions.wasm",
        "name": "System.Text.RegularExpressions.ljb2w7634m.wasm",
        "hash": "sha256-8WLhygNpdDdZRKESO9vTJRsjVt2R0snj7hpK+nxQLcI=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.AccessControl.wasm",
        "name": "System.Threading.AccessControl.joce0p84gc.wasm",
        "hash": "sha256-TURKqENVwrbM9Of4Z24ldBYWqJ4qqwcfzgL09OMsV8Y=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.Channels.wasm",
        "name": "System.Threading.Channels.mnnp4ji3ib.wasm",
        "hash": "sha256-AtTq1bOyaqRXW3xuGVm1PheL6CibX1BnOBDgKNsXfAc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.Overlapped.wasm",
        "name": "System.Threading.Overlapped.wbup7sojf3.wasm",
        "hash": "sha256-zPxihETW4rxyYvtSqOH7eGV7sL6x+K1kXooiVIorALQ=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.Tasks.Dataflow.wasm",
        "name": "System.Threading.Tasks.Dataflow.n4t9zieonm.wasm",
        "hash": "sha256-SZnwVSHdUFrvYtzjxWYiGWiLkTfmmue94t4r468G0hg=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.Tasks.Extensions.wasm",
        "name": "System.Threading.Tasks.Extensions.wdrme7fxwz.wasm",
        "hash": "sha256-3FF6cfQYZq7UMq9eI4Vy3ToMAD1y0PsovcABxyqmDMk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.Tasks.Parallel.wasm",
        "name": "System.Threading.Tasks.Parallel.03vnuen8lc.wasm",
        "hash": "sha256-HEO4P9fPWflNhGS7S5ogWuNporpRa989CAdNPnKdT28=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.Tasks.wasm",
        "name": "System.Threading.Tasks.6fasd8si7q.wasm",
        "hash": "sha256-Pk90IHCNeRzSXG6n9nlT3ihnQGCku340uU0+ZnAXDFI=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.Thread.wasm",
        "name": "System.Threading.Thread.jil5gq8n2g.wasm",
        "hash": "sha256-K/SEm7oD2cgkqdb0FqP2OF2yURyQZxWKIrCh0bPnP6E=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.ThreadPool.wasm",
        "name": "System.Threading.ThreadPool.w7nx9n33c3.wasm",
        "hash": "sha256-NLdsXbdWt2j2Y+/30zy1oMNk356H34xyLFkCswbyTUw=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.Timer.wasm",
        "name": "System.Threading.Timer.vpz28qnb8r.wasm",
        "hash": "sha256-rf5gtbmNbLTLLcbwBqPUIeShwXl+3lP7ueMxCrWRxwo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.wasm",
        "name": "System.Threading.91g5sfq1sa.wasm",
        "hash": "sha256-kwcVWB8hw0XHrw4G8Mrm4KqJ4vSayz7vr+gX30kCgQ0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Transactions.Local.wasm",
        "name": "System.Transactions.Local.tlh6ezjhx4.wasm",
        "hash": "sha256-oUhRiyXkqQs0EiVprs0RQ+8CePbZKaFjf5RxH2xcnEs=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Transactions.wasm",
        "name": "System.Transactions.xg3z6p9c5x.wasm",
        "hash": "sha256-UVboyp/gChJBtiHJoU2syJgrqsXMBkjK3LQ+0K7bMqo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ValueTuple.wasm",
        "name": "System.ValueTuple.juzeeowza8.wasm",
        "hash": "sha256-+5oflbIHf80nBKk70uUrcDX4T1jtWztPrRb4Uzy1ppY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Web.HttpUtility.wasm",
        "name": "System.Web.HttpUtility.50ukl0vvlp.wasm",
        "hash": "sha256-5bmefWDXiyMsv2v2SXYvmh6NevqHISA/JxP8Zy8WdMs=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Web.wasm",
        "name": "System.Web.nquguxbga8.wasm",
        "hash": "sha256-YU5wgaEMuLGNtM/hJuBBMZLDAEe9XA6SO5T3QUkwQtc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Windows.wasm",
        "name": "System.Windows.7my52aupxf.wasm",
        "hash": "sha256-fwiXt1le8pwi4fZGHYYZFmGLr8hOoGddT8Cxsl962z0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Xml.Linq.wasm",
        "name": "System.Xml.Linq.mr3gbysv2k.wasm",
        "hash": "sha256-7lUvBPzNLfuFjVfyWHyiwLlk3aLGpj7lyXW28c298II=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Xml.ReaderWriter.wasm",
        "name": "System.Xml.ReaderWriter.5aewgdxgx5.wasm",
        "hash": "sha256-s3t61Bpa+mm2RwSfcVTSd7lSmOBaEkLGIsI4GWNGeOU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Xml.Serialization.wasm",
        "name": "System.Xml.Serialization.895vgbnzvk.wasm",
        "hash": "sha256-XHihnSxTt35ijX8Pyf6SGdANTNE0q1hx+N9LfRRKLQQ=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Xml.XDocument.wasm",
        "name": "System.Xml.XDocument.gawed0m0dk.wasm",
        "hash": "sha256-CNhPG60MleDV2kvMK3KewekvnzPBkvUF20eeJqstPmg=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Xml.XPath.XDocument.wasm",
        "name": "System.Xml.XPath.XDocument.8d6fv5kfwc.wasm",
        "hash": "sha256-/IC9Cu68swNOSRQp7Nmd/Tfg0kO4WTFHMd1qRICifew=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Xml.XPath.wasm",
        "name": "System.Xml.XPath.rzdhxhia4c.wasm",
        "hash": "sha256-F6ZgWE5PKQkCYDtRX09jJ4AMDZp0pTvIRHlxSi1NqBo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Xml.XmlDocument.wasm",
        "name": "System.Xml.XmlDocument.sleio7elqk.wasm",
        "hash": "sha256-cM7mVS+LOzJRS/CvfzKK7OWognXFCUxKoch4KOVAgf0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Xml.XmlSerializer.wasm",
        "name": "System.Xml.XmlSerializer.dzxtf0jve0.wasm",
        "hash": "sha256-SbB7pfP1ufQmve5TnRDHPvyZ37xbZxR96BMMm+gC1fc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Xml.wasm",
        "name": "System.Xml.dvbhh7ypsi.wasm",
        "hash": "sha256-pxzdRzNLctL4GGW80wT7UmIvwENVVwKzL2R1tkAzCnY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.wasm",
        "name": "System.9immaprbgt.wasm",
        "hash": "sha256-jyY+qefL/Zi4vaVu0D7Y7ZM+LpOdtOuuaz2zaVteAKw=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "WindowsBase.wasm",
        "name": "WindowsBase.sai7rvg6jy.wasm",
        "hash": "sha256-nFB7Od4QfJbsCJHdZ3GNVn52dUv3k1CWEcVs7gnS4UM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "mscorlib.wasm",
        "name": "mscorlib.j53nj9v82n.wasm",
        "hash": "sha256-Sek1w6zfsp+mdfTY+0a9BIJVfwMWPqO89TVmyiwd4sU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "netstandard.wasm",
        "name": "netstandard.6qjyqs9cni.wasm",
        "hash": "sha256-oo0Rj85FzuT7F2L74T+91kWWN2QGMHIrHiP/zI1NMI0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "MyBlazorWasmApp1.wasm",
        "name": "MyBlazorWasmApp1.jg9e6sgs7c.wasm",
        "hash": "sha256-nSLrSAMVblC8/8pdhgfaNJq1UVetSahnYUxRqaBrtN8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "MyBlazorWasmApp1.Stories.wasm",
        "name": "MyBlazorWasmApp1.Stories.x3tlw3xuso.wasm",
        "hash": "sha256-Y92TevMOAqmL1aUxK/Kjh9InlBls+r/v9BrdrSv53ns=",
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
