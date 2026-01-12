//! Licensed to the .NET Foundation under one or more agreements.
//! The .NET Foundation licenses this file to you under the MIT license.

var e=!1;const t=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,4,1,96,0,0,3,2,1,0,10,8,1,6,0,6,64,25,11,11])),o=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,5,1,96,0,1,123,3,2,1,0,10,15,1,13,0,65,1,253,15,65,2,253,15,253,128,2,11])),n=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,5,1,96,0,1,123,3,2,1,0,10,10,1,8,0,65,0,253,15,253,98,11])),r=Symbol.for("wasm promise_control");function i(e,t){let o=null;const n=new Promise((function(n,r){o={isDone:!1,promise:null,resolve:t=>{o.isDone||(o.isDone=!0,n(t),e&&e())},reject:e=>{o.isDone||(o.isDone=!0,r(e),t&&t())}}}));o.promise=n;const i=n;return i[r]=o,{promise:i,promise_control:o}}function s(e){return e[r]}function a(e){e&&function(e){return void 0!==e[r]}(e)||Be(!1,"Promise is not controllable")}const l="__mono_message__",c=["debug","log","trace","warn","info","error"],d="MONO_WASM: ";let u,f,m,g,p,h;function w(e){g=e}function b(e){if(Pe.diagnosticTracing){const t="function"==typeof e?e():e;console.debug(d+t)}}function y(e,...t){console.info(d+e,...t)}function v(e,...t){console.info(e,...t)}function E(e,...t){console.warn(d+e,...t)}function _(e,...t){if(t&&t.length>0&&t[0]&&"object"==typeof t[0]){if(t[0].silent)return;if(t[0].toString)return void console.error(d+e,t[0].toString())}console.error(d+e,...t)}function x(e,t,o){return function(...n){try{let r=n[0];if(void 0===r)r="undefined";else if(null===r)r="null";else if("function"==typeof r)r=r.toString();else if("string"!=typeof r)try{r=JSON.stringify(r)}catch(e){r=r.toString()}t(o?JSON.stringify({method:e,payload:r,arguments:n.slice(1)}):[e+r,...n.slice(1)])}catch(e){m.error(`proxyConsole failed: ${e}`)}}}function j(e,t,o){f=t,g=e,m={...t};const n=`${o}/console`.replace("https://","wss://").replace("http://","ws://");u=new WebSocket(n),u.addEventListener("error",A),u.addEventListener("close",S),function(){for(const e of c)f[e]=x(`console.${e}`,T,!0)}()}function R(e){let t=30;const o=()=>{u?0==u.bufferedAmount||0==t?(e&&v(e),function(){for(const e of c)f[e]=x(`console.${e}`,m.log,!1)}(),u.removeEventListener("error",A),u.removeEventListener("close",S),u.close(1e3,e),u=void 0):(t--,globalThis.setTimeout(o,100)):e&&m&&m.log(e)};o()}function T(e){u&&u.readyState===WebSocket.OPEN?u.send(e):m.log(e)}function A(e){m.error(`[${g}] proxy console websocket error: ${e}`,e)}function S(e){m.debug(`[${g}] proxy console websocket closed: ${e}`,e)}function D(){Pe.preferredIcuAsset=O(Pe.config);let e="invariant"==Pe.config.globalizationMode;if(!e)if(Pe.preferredIcuAsset)Pe.diagnosticTracing&&b("ICU data archive(s) available, disabling invariant mode");else{if("custom"===Pe.config.globalizationMode||"all"===Pe.config.globalizationMode||"sharded"===Pe.config.globalizationMode){const e="invariant globalization mode is inactive and no ICU data archives are available";throw _(`ERROR: ${e}`),new Error(e)}Pe.diagnosticTracing&&b("ICU data archive(s) not available, using invariant globalization mode"),e=!0,Pe.preferredIcuAsset=null}const t="DOTNET_SYSTEM_GLOBALIZATION_INVARIANT",o=Pe.config.environmentVariables;if(void 0===o[t]&&e&&(o[t]="1"),void 0===o.TZ)try{const e=Intl.DateTimeFormat().resolvedOptions().timeZone||null;e&&(o.TZ=e)}catch(e){y("failed to detect timezone, will fallback to UTC")}}function O(e){var t;if((null===(t=e.resources)||void 0===t?void 0:t.icu)&&"invariant"!=e.globalizationMode){const t=e.applicationCulture||(ke?globalThis.navigator&&globalThis.navigator.languages&&globalThis.navigator.languages[0]:Intl.DateTimeFormat().resolvedOptions().locale),o=e.resources.icu;let n=null;if("custom"===e.globalizationMode){if(o.length>=1)return o[0].name}else t&&"all"!==e.globalizationMode?"sharded"===e.globalizationMode&&(n=function(e){const t=e.split("-")[0];return"en"===t||["fr","fr-FR","it","it-IT","de","de-DE","es","es-ES"].includes(e)?"icudt_EFIGS.dat":["zh","ko","ja"].includes(t)?"icudt_CJK.dat":"icudt_no_CJK.dat"}(t)):n="icudt.dat";if(n)for(let e=0;e<o.length;e++){const t=o[e];if(t.virtualPath===n)return t.name}}return e.globalizationMode="invariant",null}(new Date).valueOf();const C=class{constructor(e){this.url=e}toString(){return this.url}};async function k(e,t){try{const o="function"==typeof globalThis.fetch;if(Se){const n=e.startsWith("file://");if(!n&&o)return globalThis.fetch(e,t||{credentials:"same-origin"});p||(h=Ne.require("url"),p=Ne.require("fs")),n&&(e=h.fileURLToPath(e));const r=await p.promises.readFile(e);return{ok:!0,headers:{length:0,get:()=>null},url:e,arrayBuffer:()=>r,json:()=>JSON.parse(r),text:()=>{throw new Error("NotImplementedException")}}}if(o)return globalThis.fetch(e,t||{credentials:"same-origin"});if("function"==typeof read)return{ok:!0,url:e,headers:{length:0,get:()=>null},arrayBuffer:()=>new Uint8Array(read(e,"binary")),json:()=>JSON.parse(read(e,"utf8")),text:()=>read(e,"utf8")}}catch(t){return{ok:!1,url:e,status:500,headers:{length:0,get:()=>null},statusText:"ERR28: "+t,arrayBuffer:()=>{throw t},json:()=>{throw t},text:()=>{throw t}}}throw new Error("No fetch implementation available")}function I(e){return"string"!=typeof e&&Be(!1,"url must be a string"),!M(e)&&0!==e.indexOf("./")&&0!==e.indexOf("../")&&globalThis.URL&&globalThis.document&&globalThis.document.baseURI&&(e=new URL(e,globalThis.document.baseURI).toString()),e}const U=/^[a-zA-Z][a-zA-Z\d+\-.]*?:\/\//,P=/[a-zA-Z]:[\\/]/;function M(e){return Se||Ie?e.startsWith("/")||e.startsWith("\\")||-1!==e.indexOf("///")||P.test(e):U.test(e)}let L,N=0;const $=[],z=[],W=new Map,F={"js-module-threads":!0,"js-module-runtime":!0,"js-module-dotnet":!0,"js-module-native":!0,"js-module-diagnostics":!0},B={...F,"js-module-library-initializer":!0},V={...F,dotnetwasm:!0,heap:!0,manifest:!0},q={...B,manifest:!0},H={...B,dotnetwasm:!0},J={dotnetwasm:!0,symbols:!0},Z={...B,dotnetwasm:!0,symbols:!0},Q={symbols:!0};function G(e){return!("icu"==e.behavior&&e.name!=Pe.preferredIcuAsset)}function K(e,t,o){null!=t||(t=[]),Be(1==t.length,`Expect to have one ${o} asset in resources`);const n=t[0];return n.behavior=o,X(n),e.push(n),n}function X(e){V[e.behavior]&&W.set(e.behavior,e)}function Y(e){Be(V[e],`Unknown single asset behavior ${e}`);const t=W.get(e);if(t&&!t.resolvedUrl)if(t.resolvedUrl=Pe.locateFile(t.name),F[t.behavior]){const e=ge(t);e?("string"!=typeof e&&Be(!1,"loadBootResource response for 'dotnetjs' type should be a URL string"),t.resolvedUrl=e):t.resolvedUrl=ce(t.resolvedUrl,t.behavior)}else if("dotnetwasm"!==t.behavior)throw new Error(`Unknown single asset behavior ${e}`);return t}function ee(e){const t=Y(e);return Be(t,`Single asset for ${e} not found`),t}let te=!1;async function oe(){if(!te){te=!0,Pe.diagnosticTracing&&b("mono_download_assets");try{const e=[],t=[],o=(e,t)=>{!Z[e.behavior]&&G(e)&&Pe.expected_instantiated_assets_count++,!H[e.behavior]&&G(e)&&(Pe.expected_downloaded_assets_count++,t.push(se(e)))};for(const t of $)o(t,e);for(const e of z)o(e,t);Pe.allDownloadsQueued.promise_control.resolve(),Promise.all([...e,...t]).then((()=>{Pe.allDownloadsFinished.promise_control.resolve()})).catch((e=>{throw Pe.err("Error in mono_download_assets: "+e),Xe(1,e),e})),await Pe.runtimeModuleLoaded.promise;const n=async e=>{const t=await e;if(t.buffer){if(!Z[t.behavior]){t.buffer&&"object"==typeof t.buffer||Be(!1,"asset buffer must be array-like or buffer-like or promise of these"),"string"!=typeof t.resolvedUrl&&Be(!1,"resolvedUrl must be string");const e=t.resolvedUrl,o=await t.buffer,n=new Uint8Array(o);pe(t),await Ue.beforeOnRuntimeInitialized.promise,Ue.instantiate_asset(t,e,n)}}else J[t.behavior]?("symbols"===t.behavior&&(await Ue.instantiate_symbols_asset(t),pe(t)),J[t.behavior]&&++Pe.actual_downloaded_assets_count):(t.isOptional||Be(!1,"Expected asset to have the downloaded buffer"),!H[t.behavior]&&G(t)&&Pe.expected_downloaded_assets_count--,!Z[t.behavior]&&G(t)&&Pe.expected_instantiated_assets_count--)},r=[],i=[];for(const t of e)r.push(n(t));for(const e of t)i.push(n(e));Promise.all(r).then((()=>{Ce||Ue.coreAssetsInMemory.promise_control.resolve()})).catch((e=>{throw Pe.err("Error in mono_download_assets: "+e),Xe(1,e),e})),Promise.all(i).then((async()=>{Ce||(await Ue.coreAssetsInMemory.promise,Ue.allAssetsInMemory.promise_control.resolve())})).catch((e=>{throw Pe.err("Error in mono_download_assets: "+e),Xe(1,e),e}))}catch(e){throw Pe.err("Error in mono_download_assets: "+e),e}}}let ne=!1;function re(){if(ne)return;ne=!0;const e=Pe.config,t=[];if(e.assets)for(const t of e.assets)"object"!=typeof t&&Be(!1,`asset must be object, it was ${typeof t} : ${t}`),"string"!=typeof t.behavior&&Be(!1,"asset behavior must be known string"),"string"!=typeof t.name&&Be(!1,"asset name must be string"),t.resolvedUrl&&"string"!=typeof t.resolvedUrl&&Be(!1,"asset resolvedUrl could be string"),t.hash&&"string"!=typeof t.hash&&Be(!1,"asset resolvedUrl could be string"),t.pendingDownload&&"object"!=typeof t.pendingDownload&&Be(!1,"asset pendingDownload could be object"),t.isCore?$.push(t):z.push(t),X(t);else if(e.resources){const o=e.resources;o.wasmNative||Be(!1,"resources.wasmNative must be defined"),o.jsModuleNative||Be(!1,"resources.jsModuleNative must be defined"),o.jsModuleRuntime||Be(!1,"resources.jsModuleRuntime must be defined"),K(z,o.wasmNative,"dotnetwasm"),K(t,o.jsModuleNative,"js-module-native"),K(t,o.jsModuleRuntime,"js-module-runtime"),o.jsModuleDiagnostics&&K(t,o.jsModuleDiagnostics,"js-module-diagnostics");const n=(e,t,o)=>{const n=e;n.behavior=t,o?(n.isCore=!0,$.push(n)):z.push(n)};if(o.coreAssembly)for(let e=0;e<o.coreAssembly.length;e++)n(o.coreAssembly[e],"assembly",!0);if(o.assembly)for(let e=0;e<o.assembly.length;e++)n(o.assembly[e],"assembly",!o.coreAssembly);if(0!=e.debugLevel&&Pe.isDebuggingSupported()){if(o.corePdb)for(let e=0;e<o.corePdb.length;e++)n(o.corePdb[e],"pdb",!0);if(o.pdb)for(let e=0;e<o.pdb.length;e++)n(o.pdb[e],"pdb",!o.corePdb)}if(e.loadAllSatelliteResources&&o.satelliteResources)for(const e in o.satelliteResources)for(let t=0;t<o.satelliteResources[e].length;t++){const r=o.satelliteResources[e][t];r.culture=e,n(r,"resource",!o.coreAssembly)}if(o.coreVfs)for(let e=0;e<o.coreVfs.length;e++)n(o.coreVfs[e],"vfs",!0);if(o.vfs)for(let e=0;e<o.vfs.length;e++)n(o.vfs[e],"vfs",!o.coreVfs);const r=O(e);if(r&&o.icu)for(let e=0;e<o.icu.length;e++){const t=o.icu[e];t.name===r&&n(t,"icu",!1)}if(o.wasmSymbols)for(let e=0;e<o.wasmSymbols.length;e++)n(o.wasmSymbols[e],"symbols",!1)}if(e.appsettings)for(let t=0;t<e.appsettings.length;t++){const o=e.appsettings[t],n=he(o);"appsettings.json"!==n&&n!==`appsettings.${e.applicationEnvironment}.json`||z.push({name:o,behavior:"vfs",noCache:!0,useCredentials:!0})}e.assets=[...$,...z,...t]}async function ie(e){const t=await se(e);return await t.pendingDownloadInternal.response,t.buffer}async function se(e){try{return await ae(e)}catch(t){if(!Pe.enableDownloadRetry)throw t;if(Ie||Se)throw t;if(e.pendingDownload&&e.pendingDownloadInternal==e.pendingDownload)throw t;if(e.resolvedUrl&&-1!=e.resolvedUrl.indexOf("file://"))throw t;if(t&&404==t.status)throw t;e.pendingDownloadInternal=void 0,await Pe.allDownloadsQueued.promise;try{return Pe.diagnosticTracing&&b(`Retrying download '${e.name}'`),await ae(e)}catch(t){return e.pendingDownloadInternal=void 0,await new Promise((e=>globalThis.setTimeout(e,100))),Pe.diagnosticTracing&&b(`Retrying download (2) '${e.name}' after delay`),await ae(e)}}}async function ae(e){for(;L;)await L.promise;try{++N,N==Pe.maxParallelDownloads&&(Pe.diagnosticTracing&&b("Throttling further parallel downloads"),L=i());const t=await async function(e){if(e.pendingDownload&&(e.pendingDownloadInternal=e.pendingDownload),e.pendingDownloadInternal&&e.pendingDownloadInternal.response)return e.pendingDownloadInternal.response;if(e.buffer){const t=await e.buffer;return e.resolvedUrl||(e.resolvedUrl="undefined://"+e.name),e.pendingDownloadInternal={url:e.resolvedUrl,name:e.name,response:Promise.resolve({ok:!0,arrayBuffer:()=>t,json:()=>JSON.parse(new TextDecoder("utf-8").decode(t)),text:()=>{throw new Error("NotImplementedException")},headers:{get:()=>{}}})},e.pendingDownloadInternal.response}const t=e.loadRemote&&Pe.config.remoteSources?Pe.config.remoteSources:[""];let o;for(let n of t){n=n.trim(),"./"===n&&(n="");const t=le(e,n);e.name===t?Pe.diagnosticTracing&&b(`Attempting to download '${t}'`):Pe.diagnosticTracing&&b(`Attempting to download '${t}' for ${e.name}`);try{e.resolvedUrl=t;const n=fe(e);if(e.pendingDownloadInternal=n,o=await n.response,!o||!o.ok)continue;return o}catch(e){o||(o={ok:!1,url:t,status:0,statusText:""+e});continue}}const n=e.isOptional||e.name.match(/\.pdb$/)&&Pe.config.ignorePdbLoadErrors;if(o||Be(!1,`Response undefined ${e.name}`),!n){const t=new Error(`download '${o.url}' for ${e.name} failed ${o.status} ${o.statusText}`);throw t.status=o.status,t}y(`optional download '${o.url}' for ${e.name} failed ${o.status} ${o.statusText}`)}(e);return t?(J[e.behavior]||(e.buffer=await t.arrayBuffer(),++Pe.actual_downloaded_assets_count),e):e}finally{if(--N,L&&N==Pe.maxParallelDownloads-1){Pe.diagnosticTracing&&b("Resuming more parallel downloads");const e=L;L=void 0,e.promise_control.resolve()}}}function le(e,t){let o;return null==t&&Be(!1,`sourcePrefix must be provided for ${e.name}`),e.resolvedUrl?o=e.resolvedUrl:(o=""===t?"assembly"===e.behavior||"pdb"===e.behavior?e.name:"resource"===e.behavior&&e.culture&&""!==e.culture?`${e.culture}/${e.name}`:e.name:t+e.name,o=ce(Pe.locateFile(o),e.behavior)),o&&"string"==typeof o||Be(!1,"attemptUrl need to be path or url string"),o}function ce(e,t){return Pe.modulesUniqueQuery&&q[t]&&(e+=Pe.modulesUniqueQuery),e}let de=0;const ue=new Set;function fe(e){try{e.resolvedUrl||Be(!1,"Request's resolvedUrl must be set");const t=function(e){let t=e.resolvedUrl;if(Pe.loadBootResource){const o=ge(e);if(o instanceof Promise)return o;"string"==typeof o&&(t=o)}const o={};return Pe.config.disableNoCacheFetch||(o.cache="no-cache"),e.useCredentials?o.credentials="include":!Pe.config.disableIntegrityCheck&&e.hash&&(o.integrity=e.hash),Pe.fetch_like(t,o)}(e),o={name:e.name,url:e.resolvedUrl,response:t};return ue.add(e.name),o.response.then((()=>{"assembly"==e.behavior&&Pe.loadedAssemblies.push(e.name),de++,Pe.onDownloadResourceProgress&&Pe.onDownloadResourceProgress(de,ue.size)})),o}catch(t){const o={ok:!1,url:e.resolvedUrl,status:500,statusText:"ERR29: "+t,arrayBuffer:()=>{throw t},json:()=>{throw t}};return{name:e.name,url:e.resolvedUrl,response:Promise.resolve(o)}}}const me={resource:"assembly",assembly:"assembly",pdb:"pdb",icu:"globalization",vfs:"configuration",manifest:"manifest",dotnetwasm:"dotnetwasm","js-module-dotnet":"dotnetjs","js-module-native":"dotnetjs","js-module-runtime":"dotnetjs","js-module-threads":"dotnetjs"};function ge(e){var t;if(Pe.loadBootResource){const o=null!==(t=e.hash)&&void 0!==t?t:"",n=e.resolvedUrl,r=me[e.behavior];if(r){const t=Pe.loadBootResource(r,e.name,n,o,e.behavior);return"string"==typeof t?I(t):t}}}function pe(e){e.pendingDownloadInternal=null,e.pendingDownload=null,e.buffer=null,e.moduleExports=null}function he(e){let t=e.lastIndexOf("/");return t>=0&&t++,e.substring(t)}async function we(e){e&&await Promise.all((null!=e?e:[]).map((e=>async function(e){try{const t=e.name;if(!e.moduleExports){const o=ce(Pe.locateFile(t),"js-module-library-initializer");Pe.diagnosticTracing&&b(`Attempting to import '${o}' for ${e}`),e.moduleExports=await import(/*! webpackIgnore: true */o)}Pe.libraryInitializers.push({scriptName:t,exports:e.moduleExports})}catch(t){E(`Failed to import library initializer '${e}': ${t}`)}}(e))))}async function be(e,t){if(!Pe.libraryInitializers)return;const o=[];for(let n=0;n<Pe.libraryInitializers.length;n++){const r=Pe.libraryInitializers[n];r.exports[e]&&o.push(ye(r.scriptName,e,(()=>r.exports[e](...t))))}await Promise.all(o)}async function ye(e,t,o){try{await o()}catch(o){throw E(`Failed to invoke '${t}' on library initializer '${e}': ${o}`),Xe(1,o),o}}function ve(e,t){if(e===t)return e;const o={...t};return void 0!==o.assets&&o.assets!==e.assets&&(o.assets=[...e.assets||[],...o.assets||[]]),void 0!==o.resources&&(o.resources=_e(e.resources||{assembly:[],jsModuleNative:[],jsModuleRuntime:[],wasmNative:[]},o.resources)),void 0!==o.environmentVariables&&(o.environmentVariables={...e.environmentVariables||{},...o.environmentVariables||{}}),void 0!==o.runtimeOptions&&o.runtimeOptions!==e.runtimeOptions&&(o.runtimeOptions=[...e.runtimeOptions||[],...o.runtimeOptions||[]]),Object.assign(e,o)}function Ee(e,t){if(e===t)return e;const o={...t};return o.config&&(e.config||(e.config={}),o.config=ve(e.config,o.config)),Object.assign(e,o)}function _e(e,t){if(e===t)return e;const o={...t};return void 0!==o.coreAssembly&&(o.coreAssembly=[...e.coreAssembly||[],...o.coreAssembly||[]]),void 0!==o.assembly&&(o.assembly=[...e.assembly||[],...o.assembly||[]]),void 0!==o.lazyAssembly&&(o.lazyAssembly=[...e.lazyAssembly||[],...o.lazyAssembly||[]]),void 0!==o.corePdb&&(o.corePdb=[...e.corePdb||[],...o.corePdb||[]]),void 0!==o.pdb&&(o.pdb=[...e.pdb||[],...o.pdb||[]]),void 0!==o.jsModuleWorker&&(o.jsModuleWorker=[...e.jsModuleWorker||[],...o.jsModuleWorker||[]]),void 0!==o.jsModuleNative&&(o.jsModuleNative=[...e.jsModuleNative||[],...o.jsModuleNative||[]]),void 0!==o.jsModuleDiagnostics&&(o.jsModuleDiagnostics=[...e.jsModuleDiagnostics||[],...o.jsModuleDiagnostics||[]]),void 0!==o.jsModuleRuntime&&(o.jsModuleRuntime=[...e.jsModuleRuntime||[],...o.jsModuleRuntime||[]]),void 0!==o.wasmSymbols&&(o.wasmSymbols=[...e.wasmSymbols||[],...o.wasmSymbols||[]]),void 0!==o.wasmNative&&(o.wasmNative=[...e.wasmNative||[],...o.wasmNative||[]]),void 0!==o.icu&&(o.icu=[...e.icu||[],...o.icu||[]]),void 0!==o.satelliteResources&&(o.satelliteResources=function(e,t){if(e===t)return e;for(const o in t)e[o]=[...e[o]||[],...t[o]||[]];return e}(e.satelliteResources||{},o.satelliteResources||{})),void 0!==o.modulesAfterConfigLoaded&&(o.modulesAfterConfigLoaded=[...e.modulesAfterConfigLoaded||[],...o.modulesAfterConfigLoaded||[]]),void 0!==o.modulesAfterRuntimeReady&&(o.modulesAfterRuntimeReady=[...e.modulesAfterRuntimeReady||[],...o.modulesAfterRuntimeReady||[]]),void 0!==o.extensions&&(o.extensions={...e.extensions||{},...o.extensions||{}}),void 0!==o.vfs&&(o.vfs=[...e.vfs||[],...o.vfs||[]]),Object.assign(e,o)}function xe(){const e=Pe.config;if(e.environmentVariables=e.environmentVariables||{},e.runtimeOptions=e.runtimeOptions||[],e.resources=e.resources||{assembly:[],jsModuleNative:[],jsModuleWorker:[],jsModuleRuntime:[],wasmNative:[],vfs:[],satelliteResources:{}},e.assets){Pe.diagnosticTracing&&b("config.assets is deprecated, use config.resources instead");for(const t of e.assets){const o={};switch(t.behavior){case"assembly":o.assembly=[t];break;case"pdb":o.pdb=[t];break;case"resource":o.satelliteResources={},o.satelliteResources[t.culture]=[t];break;case"icu":o.icu=[t];break;case"symbols":o.wasmSymbols=[t];break;case"vfs":o.vfs=[t];break;case"dotnetwasm":o.wasmNative=[t];break;case"js-module-threads":o.jsModuleWorker=[t];break;case"js-module-runtime":o.jsModuleRuntime=[t];break;case"js-module-native":o.jsModuleNative=[t];break;case"js-module-diagnostics":o.jsModuleDiagnostics=[t];break;case"js-module-dotnet":break;default:throw new Error(`Unexpected behavior ${t.behavior} of asset ${t.name}`)}_e(e.resources,o)}}e.debugLevel,e.applicationEnvironment||(e.applicationEnvironment="Production"),e.applicationCulture&&(e.environmentVariables.LANG=`${e.applicationCulture}.UTF-8`),Ue.diagnosticTracing=Pe.diagnosticTracing=!!e.diagnosticTracing,Ue.waitForDebugger=e.waitForDebugger,Pe.maxParallelDownloads=e.maxParallelDownloads||Pe.maxParallelDownloads,Pe.enableDownloadRetry=void 0!==e.enableDownloadRetry?e.enableDownloadRetry:Pe.enableDownloadRetry}let je=!1;async function Re(e){var t;if(je)return void await Pe.afterConfigLoaded.promise;let o;try{if(e.configSrc||Pe.config&&0!==Object.keys(Pe.config).length&&(Pe.config.assets||Pe.config.resources)||(e.configSrc="dotnet.boot.js"),o=e.configSrc,je=!0,o&&(Pe.diagnosticTracing&&b("mono_wasm_load_config"),await async function(e){const t=e.configSrc,o=Pe.locateFile(t);let n=null;void 0!==Pe.loadBootResource&&(n=Pe.loadBootResource("manifest",t,o,"","manifest"));let r,i=null;if(n)if("string"==typeof n)n.includes(".json")?(i=await s(I(n)),r=await Ae(i)):r=(await import(I(n))).config;else{const e=await n;"function"==typeof e.json?(i=e,r=await Ae(i)):r=e.config}else o.includes(".json")?(i=await s(ce(o,"manifest")),r=await Ae(i)):r=(await import(ce(o,"manifest"))).config;function s(e){return Pe.fetch_like(e,{method:"GET",credentials:"include",cache:"no-cache"})}Pe.config.applicationEnvironment&&(r.applicationEnvironment=Pe.config.applicationEnvironment),ve(Pe.config,r)}(e)),xe(),await we(null===(t=Pe.config.resources)||void 0===t?void 0:t.modulesAfterConfigLoaded),await be("onRuntimeConfigLoaded",[Pe.config]),e.onConfigLoaded)try{await e.onConfigLoaded(Pe.config,Le),xe()}catch(e){throw _("onConfigLoaded() failed",e),e}xe(),Pe.afterConfigLoaded.promise_control.resolve(Pe.config)}catch(t){const n=`Failed to load config file ${o} ${t} ${null==t?void 0:t.stack}`;throw Pe.config=e.config=Object.assign(Pe.config,{message:n,error:t,isError:!0}),Xe(1,new Error(n)),t}}function Te(){return!!globalThis.navigator&&(Pe.isChromium||Pe.isFirefox)}async function Ae(e){const t=Pe.config,o=await e.json();t.applicationEnvironment||o.applicationEnvironment||(o.applicationEnvironment=e.headers.get("Blazor-Environment")||e.headers.get("DotNet-Environment")||void 0),o.environmentVariables||(o.environmentVariables={});const n=e.headers.get("DOTNET-MODIFIABLE-ASSEMBLIES");n&&(o.environmentVariables.DOTNET_MODIFIABLE_ASSEMBLIES=n);const r=e.headers.get("ASPNETCORE-BROWSER-TOOLS");return r&&(o.environmentVariables.__ASPNETCORE_BROWSER_TOOLS=r),o}"function"!=typeof importScripts||globalThis.onmessage||(globalThis.dotnetSidecar=!0);const Se="object"==typeof process&&"object"==typeof process.versions&&"string"==typeof process.versions.node,De="function"==typeof importScripts,Oe=De&&"undefined"!=typeof dotnetSidecar,Ce=De&&!Oe,ke="object"==typeof window||De&&!Se,Ie=!ke&&!Se;let Ue={},Pe={},Me={},Le={},Ne={},$e=!1;const ze={},We={config:ze},Fe={mono:{},binding:{},internal:Ne,module:We,loaderHelpers:Pe,runtimeHelpers:Ue,diagnosticHelpers:Me,api:Le};function Be(e,t){if(e)return;const o="Assert failed: "+("function"==typeof t?t():t),n=new Error(o);_(o,n),Ue.nativeAbort(n)}function Ve(){return void 0!==Pe.exitCode}function qe(){return Ue.runtimeReady&&!Ve()}function He(){Ve()&&Be(!1,`.NET runtime already exited with ${Pe.exitCode} ${Pe.exitReason}. You can use runtime.runMain() which doesn't exit the runtime.`),Ue.runtimeReady||Be(!1,".NET runtime didn't start yet. Please call dotnet.create() first.")}function Je(){ke&&(globalThis.addEventListener("unhandledrejection",et),globalThis.addEventListener("error",tt))}let Ze,Qe;function Ge(e){Qe&&Qe(e),Xe(e,Pe.exitReason)}function Ke(e){Ze&&Ze(e||Pe.exitReason),Xe(1,e||Pe.exitReason)}function Xe(t,o){var n,r;const i=o&&"object"==typeof o;t=i&&"number"==typeof o.status?o.status:void 0===t?-1:t;const s=i&&"string"==typeof o.message?o.message:""+o;(o=i?o:Ue.ExitStatus?function(e,t){const o=new Ue.ExitStatus(e);return o.message=t,o.toString=()=>t,o}(t,s):new Error("Exit with code "+t+" "+s)).status=t,o.message||(o.message=s);const a=""+(o.stack||(new Error).stack);try{Object.defineProperty(o,"stack",{get:()=>a})}catch(e){}const l=!!o.silent;if(o.silent=!0,Ve())Pe.diagnosticTracing&&b("mono_exit called after exit");else{try{We.onAbort==Ke&&(We.onAbort=Ze),We.onExit==Ge&&(We.onExit=Qe),ke&&(globalThis.removeEventListener("unhandledrejection",et),globalThis.removeEventListener("error",tt)),Ue.runtimeReady?(Ue.jiterpreter_dump_stats&&Ue.jiterpreter_dump_stats(!1),0===t&&(null===(n=Pe.config)||void 0===n?void 0:n.interopCleanupOnExit)&&Ue.forceDisposeProxies(!0,!0),e&&0!==t&&(null===(r=Pe.config)||void 0===r||r.dumpThreadsOnNonZeroExit)):(Pe.diagnosticTracing&&b(`abort_startup, reason: ${o}`),function(e){Pe.allDownloadsQueued.promise_control.reject(e),Pe.allDownloadsFinished.promise_control.reject(e),Pe.afterConfigLoaded.promise_control.reject(e),Pe.wasmCompilePromise.promise_control.reject(e),Pe.runtimeModuleLoaded.promise_control.reject(e),Ue.dotnetReady&&(Ue.dotnetReady.promise_control.reject(e),Ue.afterInstantiateWasm.promise_control.reject(e),Ue.beforePreInit.promise_control.reject(e),Ue.afterPreInit.promise_control.reject(e),Ue.afterPreRun.promise_control.reject(e),Ue.beforeOnRuntimeInitialized.promise_control.reject(e),Ue.afterOnRuntimeInitialized.promise_control.reject(e),Ue.afterPostRun.promise_control.reject(e))}(o))}catch(e){E("mono_exit A failed",e)}try{l||(function(e,t){if(0!==e&&t){const e=Ue.ExitStatus&&t instanceof Ue.ExitStatus?b:_;"string"==typeof t?e(t):(void 0===t.stack&&(t.stack=(new Error).stack+""),t.message?e(Ue.stringify_as_error_with_stack?Ue.stringify_as_error_with_stack(t.message+"\n"+t.stack):t.message+"\n"+t.stack):e(JSON.stringify(t)))}!Ce&&Pe.config&&(Pe.config.logExitCode?Pe.config.forwardConsoleLogsToWS?R("WASM EXIT "+e):v("WASM EXIT "+e):Pe.config.forwardConsoleLogsToWS&&R())}(t,o),function(e){if(ke&&!Ce&&Pe.config&&Pe.config.appendElementOnExit&&document){const t=document.createElement("label");t.id="tests_done",0!==e&&(t.style.background="red"),t.innerHTML=""+e,document.body.appendChild(t)}}(t))}catch(e){E("mono_exit B failed",e)}Pe.exitCode=t,Pe.exitReason||(Pe.exitReason=o),!Ce&&Ue.runtimeReady&&We.runtimeKeepalivePop()}if(Pe.config&&Pe.config.asyncFlushOnExit&&0===t)throw(async()=>{try{await async function(){try{const e=await import(/*! webpackIgnore: true */"process"),t=e=>new Promise(((t,o)=>{e.on("error",o),e.end("","utf8",t)})),o=t(e.stderr),n=t(e.stdout);let r;const i=new Promise((e=>{r=setTimeout((()=>e("timeout")),1e3)}));await Promise.race([Promise.all([n,o]),i]),clearTimeout(r)}catch(e){_(`flushing std* streams failed: ${e}`)}}()}finally{Ye(t,o)}})(),o;Ye(t,o)}function Ye(e,t){if(Ue.runtimeReady&&Ue.nativeExit)try{Ue.nativeExit(e)}catch(e){!Ue.ExitStatus||e instanceof Ue.ExitStatus||E("set_exit_code_and_quit_now failed: "+e.toString())}if(0!==e||!ke)throw Se&&Ne.process?Ne.process.exit(e):Ue.quit&&Ue.quit(e,t),t}function et(e){ot(e,e.reason,"rejection")}function tt(e){ot(e,e.error,"error")}function ot(e,t,o){e.preventDefault();try{t||(t=new Error("Unhandled "+o)),void 0===t.stack&&(t.stack=(new Error).stack),t.stack=t.stack+"",t.silent||(_("Unhandled error:",t),Xe(1,t))}catch(e){}}!function(e){if($e)throw new Error("Loader module already loaded");$e=!0,Ue=e.runtimeHelpers,Pe=e.loaderHelpers,Me=e.diagnosticHelpers,Le=e.api,Ne=e.internal,Object.assign(Le,{INTERNAL:Ne,invokeLibraryInitializers:be}),Object.assign(e.module,{config:ve(ze,{environmentVariables:{}})});const r={mono_wasm_bindings_is_ready:!1,config:e.module.config,diagnosticTracing:!1,nativeAbort:e=>{throw e||new Error("abort")},nativeExit:e=>{throw new Error("exit:"+e)}},l={gitHash:"fad253f51b461736dfd3cd9c15977bb7493becef",config:e.module.config,diagnosticTracing:!1,maxParallelDownloads:16,enableDownloadRetry:!0,_loaded_files:[],loadedFiles:[],loadedAssemblies:[],libraryInitializers:[],workerNextNumber:1,actual_downloaded_assets_count:0,actual_instantiated_assets_count:0,expected_downloaded_assets_count:0,expected_instantiated_assets_count:0,afterConfigLoaded:i(),allDownloadsQueued:i(),allDownloadsFinished:i(),wasmCompilePromise:i(),runtimeModuleLoaded:i(),loadingWorkers:i(),is_exited:Ve,is_runtime_running:qe,assert_runtime_running:He,mono_exit:Xe,createPromiseController:i,getPromiseController:s,assertIsControllablePromise:a,mono_download_assets:oe,resolve_single_asset_path:ee,setup_proxy_console:j,set_thread_prefix:w,installUnhandledErrorHandler:Je,retrieve_asset_download:ie,invokeLibraryInitializers:be,isDebuggingSupported:Te,exceptions:t,simd:n,relaxedSimd:o};Object.assign(Ue,r),Object.assign(Pe,l)}(Fe);let nt,rt,it,st=!1,at=!1;async function lt(e){if(!at){if(at=!0,ke&&Pe.config.forwardConsoleLogsToWS&&void 0!==globalThis.WebSocket&&j("main",globalThis.console,globalThis.location.origin),We||Be(!1,"Null moduleConfig"),Pe.config||Be(!1,"Null moduleConfig.config"),"function"==typeof e){const t=e(Fe.api);if(t.ready)throw new Error("Module.ready couldn't be redefined.");Object.assign(We,t),Ee(We,t)}else{if("object"!=typeof e)throw new Error("Can't use moduleFactory callback of createDotnetRuntime function.");Ee(We,e)}await async function(e){if(Se){const e=await import(/*! webpackIgnore: true */"process"),t=14;if(e.versions.node.split(".")[0]<t)throw new Error(`NodeJS at '${e.execPath}' has too low version '${e.versions.node}', please use at least ${t}. See also https://aka.ms/dotnet-wasm-features`)}const t=/*! webpackIgnore: true */import.meta.url,o=t.indexOf("?");var n;if(o>0&&(Pe.modulesUniqueQuery=t.substring(o)),Pe.scriptUrl=t.replace(/\\/g,"/").replace(/[?#].*/,""),Pe.scriptDirectory=(n=Pe.scriptUrl).slice(0,n.lastIndexOf("/"))+"/",Pe.locateFile=e=>"URL"in globalThis&&globalThis.URL!==C?new URL(e,Pe.scriptDirectory).toString():M(e)?e:Pe.scriptDirectory+e,Pe.fetch_like=k,Pe.out=console.log,Pe.err=console.error,Pe.onDownloadResourceProgress=e.onDownloadResourceProgress,ke&&globalThis.navigator){const e=globalThis.navigator,t=e.userAgentData&&e.userAgentData.brands;t&&t.length>0?Pe.isChromium=t.some((e=>"Google Chrome"===e.brand||"Microsoft Edge"===e.brand||"Chromium"===e.brand)):e.userAgent&&(Pe.isChromium=e.userAgent.includes("Chrome"),Pe.isFirefox=e.userAgent.includes("Firefox"))}Ne.require=Se?await import(/*! webpackIgnore: true */"module").then((e=>e.createRequire(/*! webpackIgnore: true */import.meta.url))):Promise.resolve((()=>{throw new Error("require not supported")})),void 0===globalThis.URL&&(globalThis.URL=C)}(We)}}async function ct(e){return await lt(e),Ze=We.onAbort,Qe=We.onExit,We.onAbort=Ke,We.onExit=Ge,We.ENVIRONMENT_IS_PTHREAD?async function(){(function(){const e=new MessageChannel,t=e.port1,o=e.port2;t.addEventListener("message",(e=>{var n,r;n=JSON.parse(e.data.config),r=JSON.parse(e.data.monoThreadInfo),st?Pe.diagnosticTracing&&b("mono config already received"):(ve(Pe.config,n),Ue.monoThreadInfo=r,xe(),Pe.diagnosticTracing&&b("mono config received"),st=!0,Pe.afterConfigLoaded.promise_control.resolve(Pe.config),ke&&n.forwardConsoleLogsToWS&&void 0!==globalThis.WebSocket&&Pe.setup_proxy_console("worker-idle",console,globalThis.location.origin)),t.close(),o.close()}),{once:!0}),t.start(),self.postMessage({[l]:{monoCmd:"preload",port:o}},[o])})(),await Pe.afterConfigLoaded.promise,function(){const e=Pe.config;e.assets||Be(!1,"config.assets must be defined");for(const t of e.assets)X(t),Q[t.behavior]&&z.push(t)}(),setTimeout((async()=>{try{await oe()}catch(e){Xe(1,e)}}),0);const e=dt(),t=await Promise.all(e);return await ut(t),We}():async function(){var e;await Re(We),re();const t=dt();(async function(){try{const e=ee("dotnetwasm");await se(e),e&&e.pendingDownloadInternal&&e.pendingDownloadInternal.response||Be(!1,"Can't load dotnet.native.wasm");const t=await e.pendingDownloadInternal.response,o=t.headers&&t.headers.get?t.headers.get("Content-Type"):void 0;let n;if("function"==typeof WebAssembly.compileStreaming&&"application/wasm"===o)n=await WebAssembly.compileStreaming(t);else{ke&&"application/wasm"!==o&&E('WebAssembly resource does not have the expected content type "application/wasm", so falling back to slower ArrayBuffer instantiation.');const e=await t.arrayBuffer();Pe.diagnosticTracing&&b("instantiate_wasm_module buffered"),n=Ie?await Promise.resolve(new WebAssembly.Module(e)):await WebAssembly.compile(e)}e.pendingDownloadInternal=null,e.pendingDownload=null,e.buffer=null,e.moduleExports=null,Pe.wasmCompilePromise.promise_control.resolve(n)}catch(e){Pe.wasmCompilePromise.promise_control.reject(e)}})(),setTimeout((async()=>{try{D(),await oe()}catch(e){Xe(1,e)}}),0);const o=await Promise.all(t);return await ut(o),await Ue.dotnetReady.promise,await we(null===(e=Pe.config.resources)||void 0===e?void 0:e.modulesAfterRuntimeReady),await be("onRuntimeReady",[Fe.api]),Le}()}function dt(){const e=ee("js-module-runtime"),t=ee("js-module-native");if(nt&&rt)return[nt,rt,it];"object"==typeof e.moduleExports?nt=e.moduleExports:(Pe.diagnosticTracing&&b(`Attempting to import '${e.resolvedUrl}' for ${e.name}`),nt=import(/*! webpackIgnore: true */e.resolvedUrl)),"object"==typeof t.moduleExports?rt=t.moduleExports:(Pe.diagnosticTracing&&b(`Attempting to import '${t.resolvedUrl}' for ${t.name}`),rt=import(/*! webpackIgnore: true */t.resolvedUrl));const o=Y("js-module-diagnostics");return o&&("object"==typeof o.moduleExports?it=o.moduleExports:(Pe.diagnosticTracing&&b(`Attempting to import '${o.resolvedUrl}' for ${o.name}`),it=import(/*! webpackIgnore: true */o.resolvedUrl))),[nt,rt,it]}async function ut(e){const{initializeExports:t,initializeReplacements:o,configureRuntimeStartup:n,configureEmscriptenStartup:r,configureWorkerStartup:i,setRuntimeGlobals:s,passEmscriptenInternals:a}=e[0],{default:l}=e[1],c=e[2];s(Fe),t(Fe),c&&c.setRuntimeGlobals(Fe),await n(We),Pe.runtimeModuleLoaded.promise_control.resolve(),l((e=>(Object.assign(We,{ready:e.ready,__dotnet_runtime:{initializeReplacements:o,configureEmscriptenStartup:r,configureWorkerStartup:i,passEmscriptenInternals:a}}),We))).catch((e=>{if(e.message&&e.message.toLowerCase().includes("out of memory"))throw new Error(".NET runtime has failed to start, because too much memory was requested. Please decrease the memory by adjusting EmccMaximumHeapSize. See also https://aka.ms/dotnet-wasm-features");throw e}))}const ft=new class{withModuleConfig(e){try{return Ee(We,e),this}catch(e){throw Xe(1,e),e}}withOnConfigLoaded(e){try{return Ee(We,{onConfigLoaded:e}),this}catch(e){throw Xe(1,e),e}}withConsoleForwarding(){try{return ve(ze,{forwardConsoleLogsToWS:!0}),this}catch(e){throw Xe(1,e),e}}withExitOnUnhandledError(){try{return ve(ze,{exitOnUnhandledError:!0}),Je(),this}catch(e){throw Xe(1,e),e}}withAsyncFlushOnExit(){try{return ve(ze,{asyncFlushOnExit:!0}),this}catch(e){throw Xe(1,e),e}}withExitCodeLogging(){try{return ve(ze,{logExitCode:!0}),this}catch(e){throw Xe(1,e),e}}withElementOnExit(){try{return ve(ze,{appendElementOnExit:!0}),this}catch(e){throw Xe(1,e),e}}withInteropCleanupOnExit(){try{return ve(ze,{interopCleanupOnExit:!0}),this}catch(e){throw Xe(1,e),e}}withDumpThreadsOnNonZeroExit(){try{return ve(ze,{dumpThreadsOnNonZeroExit:!0}),this}catch(e){throw Xe(1,e),e}}withWaitingForDebugger(e){try{return ve(ze,{waitForDebugger:e}),this}catch(e){throw Xe(1,e),e}}withInterpreterPgo(e,t){try{return ve(ze,{interpreterPgo:e,interpreterPgoSaveDelay:t}),ze.runtimeOptions?ze.runtimeOptions.push("--interp-pgo-recording"):ze.runtimeOptions=["--interp-pgo-recording"],this}catch(e){throw Xe(1,e),e}}withConfig(e){try{return ve(ze,e),this}catch(e){throw Xe(1,e),e}}withConfigSrc(e){try{return e&&"string"==typeof e||Be(!1,"must be file path or URL"),Ee(We,{configSrc:e}),this}catch(e){throw Xe(1,e),e}}withVirtualWorkingDirectory(e){try{return e&&"string"==typeof e||Be(!1,"must be directory path"),ve(ze,{virtualWorkingDirectory:e}),this}catch(e){throw Xe(1,e),e}}withEnvironmentVariable(e,t){try{const o={};return o[e]=t,ve(ze,{environmentVariables:o}),this}catch(e){throw Xe(1,e),e}}withEnvironmentVariables(e){try{return e&&"object"==typeof e||Be(!1,"must be dictionary object"),ve(ze,{environmentVariables:e}),this}catch(e){throw Xe(1,e),e}}withDiagnosticTracing(e){try{return"boolean"!=typeof e&&Be(!1,"must be boolean"),ve(ze,{diagnosticTracing:e}),this}catch(e){throw Xe(1,e),e}}withDebugging(e){try{return null!=e&&"number"==typeof e||Be(!1,"must be number"),ve(ze,{debugLevel:e}),this}catch(e){throw Xe(1,e),e}}withApplicationArguments(...e){try{return e&&Array.isArray(e)||Be(!1,"must be array of strings"),ve(ze,{applicationArguments:e}),this}catch(e){throw Xe(1,e),e}}withRuntimeOptions(e){try{return e&&Array.isArray(e)||Be(!1,"must be array of strings"),ze.runtimeOptions?ze.runtimeOptions.push(...e):ze.runtimeOptions=e,this}catch(e){throw Xe(1,e),e}}withMainAssembly(e){try{return ve(ze,{mainAssemblyName:e}),this}catch(e){throw Xe(1,e),e}}withApplicationArgumentsFromQuery(){try{if(!globalThis.window)throw new Error("Missing window to the query parameters from");if(void 0===globalThis.URLSearchParams)throw new Error("URLSearchParams is supported");const e=new URLSearchParams(globalThis.window.location.search).getAll("arg");return this.withApplicationArguments(...e)}catch(e){throw Xe(1,e),e}}withApplicationEnvironment(e){try{return ve(ze,{applicationEnvironment:e}),this}catch(e){throw Xe(1,e),e}}withApplicationCulture(e){try{return ve(ze,{applicationCulture:e}),this}catch(e){throw Xe(1,e),e}}withResourceLoader(e){try{return Pe.loadBootResource=e,this}catch(e){throw Xe(1,e),e}}async download(){try{await async function(){lt(We),await Re(We),re(),D(),oe(),await Pe.allDownloadsFinished.promise}()}catch(e){throw Xe(1,e),e}}async create(){try{return this.instance||(this.instance=await async function(){return await ct(We),Fe.api}()),this.instance}catch(e){throw Xe(1,e),e}}async run(){try{return We.config||Be(!1,"Null moduleConfig.config"),this.instance||await this.create(),this.instance.runMainAndExit()}catch(e){throw Xe(1,e),e}}},mt=Xe,gt=ct;Ie||"function"==typeof globalThis.URL||Be(!1,"This browser/engine doesn't support URL API. Please use a modern version. See also https://aka.ms/dotnet-wasm-features"),"function"!=typeof globalThis.BigInt64Array&&Be(!1,"This browser/engine doesn't support BigInt64Array API. Please use a modern version. See also https://aka.ms/dotnet-wasm-features"),ft.withConfig(/*json-start*/{
  "mainAssemblyName": "MyBlazorWasmApp1.Stories",
  "resources": {
    "hash": "sha256-BK8ZyVesa5kgTDBUDru2hCCFUhpqrOz1Mq3wX0Y0Te8=",
    "jsModuleNative": [
      {
        "name": "dotnet.native.mpn1zvgcmn.js"
      }
    ],
    "jsModuleRuntime": [
      {
        "name": "dotnet.runtime.0j6ezsi0n0.js"
      }
    ],
    "wasmNative": [
      {
        "name": "dotnet.native.fg314o5euc.wasm",
        "integrity": "sha256-v3sHnU9Ewj94cNTusJN8T8e1AB5AciwFULItPV7u5Bs="
      }
    ],
    "coreAssembly": [
      {
        "virtualPath": "System.Runtime.InteropServices.JavaScript.wasm",
        "name": "System.Runtime.InteropServices.JavaScript.flbdejno7d.wasm",
        "integrity": "sha256-8+Vl/DoJkEoPx8GyRZ6Jq5VwDX4ahsWVMGPe0WJeKMk="
      },
      {
        "virtualPath": "System.Private.CoreLib.wasm",
        "name": "System.Private.CoreLib.zjh2w4p93l.wasm",
        "integrity": "sha256-X0WQmwJr9mguCUK9w8xSzHiQb3Ty4p0ckSzmNJ4ykUE="
      }
    ],
    "assembly": [
      {
        "virtualPath": "BlazingStory.wasm",
        "name": "BlazingStory.j43lho8qo0.wasm",
        "integrity": "sha256-ExRlf/M5ympT0YmcMk0we6hgN2N4uAOpVAJ7TCYx4Sc="
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Authorization.wasm",
        "name": "Microsoft.AspNetCore.Authorization.mrojqw2yhh.wasm",
        "integrity": "sha256-3oHLWA3M397uncwL6zioYvQQtAdzMJ9X/hqhQgNaJLs="
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Components.wasm",
        "name": "Microsoft.AspNetCore.Components.5yorhp2rya.wasm",
        "integrity": "sha256-DUy63iJswvt/OE3Ab/uHor+Y69Dyhrz6U9jedFr7p0Y="
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Components.Forms.wasm",
        "name": "Microsoft.AspNetCore.Components.Forms.xm4iwpaol4.wasm",
        "integrity": "sha256-+atsM3br4x9MSB/hKlbRgiYY8kgg6gy4aGYFwqbvBkM="
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Components.Web.wasm",
        "name": "Microsoft.AspNetCore.Components.Web.u0hflan0h2.wasm",
        "integrity": "sha256-7hAjr4bOvmBlPDfhWYvOen3cfaqFMND0RuQzicU77A0="
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Components.WebAssembly.wasm",
        "name": "Microsoft.AspNetCore.Components.WebAssembly.c9u7q8hb1x.wasm",
        "integrity": "sha256-ECHpZxl/Pk/Q7LYkIsDo72l5l0Qn/H4+JfTQfYSX+MY="
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Metadata.wasm",
        "name": "Microsoft.AspNetCore.Metadata.rci0o61dpz.wasm",
        "integrity": "sha256-FWM794OQzYuWpnEiTMMsEJoo9sjQS1M3lJzqb1WC+VE="
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.wasm",
        "name": "Microsoft.Extensions.Configuration.c1ftwcpjjw.wasm",
        "integrity": "sha256-oO6sepvLACDdiLyhWAmHDFUaihScdivEY9YSaF9pJ1c="
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.Abstractions.wasm",
        "name": "Microsoft.Extensions.Configuration.Abstractions.6uxnzk0ceg.wasm",
        "integrity": "sha256-Dt4/UkjhaHRiwR6pcvlRDTv7+qd0P8YQ6pW9ubYV6Hs="
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.Binder.wasm",
        "name": "Microsoft.Extensions.Configuration.Binder.fofnng4fh4.wasm",
        "integrity": "sha256-4Zm0KGUKDyjvJRn8MHKvvhn358LMQoXyLKQfeBjRmHE="
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.FileExtensions.wasm",
        "name": "Microsoft.Extensions.Configuration.FileExtensions.p30g5n9kos.wasm",
        "integrity": "sha256-eyoDkHosnHrMaKgUK+d+JOCdCA1MDS6Pj5XzHzvbQ2g="
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.Json.wasm",
        "name": "Microsoft.Extensions.Configuration.Json.by0khg80dz.wasm",
        "integrity": "sha256-07I9YcEJgFMvu9wzt4BsOFKeMeXj66HntPCE6s4U5/E="
      },
      {
        "virtualPath": "Microsoft.Extensions.DependencyInjection.wasm",
        "name": "Microsoft.Extensions.DependencyInjection.qxyrl8cayd.wasm",
        "integrity": "sha256-YoHztiNkODOScTSbcjZTeYB0wnvgzyGf9k1iYguuQLU="
      },
      {
        "virtualPath": "Microsoft.Extensions.DependencyInjection.Abstractions.wasm",
        "name": "Microsoft.Extensions.DependencyInjection.Abstractions.2omqzali0c.wasm",
        "integrity": "sha256-vs+E5VYlT/L2BAO672K94rXEMUfLnXRxnbXnrVYKvl0="
      },
      {
        "virtualPath": "Microsoft.Extensions.Diagnostics.wasm",
        "name": "Microsoft.Extensions.Diagnostics.cb9w1tnghn.wasm",
        "integrity": "sha256-KJyQvh4QCbAi4+PmDtmSfzhTv8AyguoITSG0cbb1lvc="
      },
      {
        "virtualPath": "Microsoft.Extensions.Diagnostics.Abstractions.wasm",
        "name": "Microsoft.Extensions.Diagnostics.Abstractions.dkgo23mh7v.wasm",
        "integrity": "sha256-3TfF7nt/mJsdNnYatYQohiAdRq8NVIPrlqYOTBhVa7w="
      },
      {
        "virtualPath": "Microsoft.Extensions.FileProviders.Abstractions.wasm",
        "name": "Microsoft.Extensions.FileProviders.Abstractions.liivstar2p.wasm",
        "integrity": "sha256-gwAKmkzCYSviZBLO8RffI595USj+eqI1gvicEFdI168="
      },
      {
        "virtualPath": "Microsoft.Extensions.FileProviders.Physical.wasm",
        "name": "Microsoft.Extensions.FileProviders.Physical.aiky6c5qih.wasm",
        "integrity": "sha256-UuIFzQYIWvwANyMlH0NfbPB46kAQ8x9HJcAVtqqw6dk="
      },
      {
        "virtualPath": "Microsoft.Extensions.FileSystemGlobbing.wasm",
        "name": "Microsoft.Extensions.FileSystemGlobbing.g3xufhx1sx.wasm",
        "integrity": "sha256-9FlVI/W1fgfL7qCMlPbVuhn2T8pPvMW+ddjTZtajgXo="
      },
      {
        "virtualPath": "Microsoft.Extensions.Logging.wasm",
        "name": "Microsoft.Extensions.Logging.ckhr38id6x.wasm",
        "integrity": "sha256-VAsYPEv5fVr/0wIWb/pQDirky8GQliB450Okl3GgbD4="
      },
      {
        "virtualPath": "Microsoft.Extensions.Logging.Abstractions.wasm",
        "name": "Microsoft.Extensions.Logging.Abstractions.bc5j2yr4c8.wasm",
        "integrity": "sha256-y1tmxtDc3oVKWpTgc2LvJh7qsjToAYxQq2vukEzNMTQ="
      },
      {
        "virtualPath": "Microsoft.Extensions.Options.wasm",
        "name": "Microsoft.Extensions.Options.bqvjhouolu.wasm",
        "integrity": "sha256-nefHEtEh+mG2DNa/Aka9XPKzwa0tBuj5jnVMZ+q/uZg="
      },
      {
        "virtualPath": "Microsoft.Extensions.Options.ConfigurationExtensions.wasm",
        "name": "Microsoft.Extensions.Options.ConfigurationExtensions.fl4og0ucdr.wasm",
        "integrity": "sha256-veIcgg/Q58yHHt4co+wMb9rIGoivBLh2K9meYfQUDf4="
      },
      {
        "virtualPath": "Microsoft.Extensions.Primitives.wasm",
        "name": "Microsoft.Extensions.Primitives.lla7bw3dg6.wasm",
        "integrity": "sha256-aQziTp142P5612OGVcR5uPx0SfCjdY/aSk/wbvoyqJA="
      },
      {
        "virtualPath": "Microsoft.Extensions.Validation.wasm",
        "name": "Microsoft.Extensions.Validation.iiwnap85de.wasm",
        "integrity": "sha256-WhIgBR+n1ltOl6nlBBa3PFel1YN/UVGqCmCUfj2ihKs="
      },
      {
        "virtualPath": "Microsoft.JSInterop.wasm",
        "name": "Microsoft.JSInterop.dii7u9e9mw.wasm",
        "integrity": "sha256-dRu/h+0arXFWPem4uT1d2AsGENPuyy4C0JvwAl9vShM="
      },
      {
        "virtualPath": "Microsoft.JSInterop.WebAssembly.wasm",
        "name": "Microsoft.JSInterop.WebAssembly.b45dn0ibjt.wasm",
        "integrity": "sha256-K7JpYoAeTsZWSjrqCcwXovr7B9gpokkFhVOuhEsDgNM="
      },
      {
        "virtualPath": "Toolbelt.Blazor.HotKeys2.wasm",
        "name": "Toolbelt.Blazor.HotKeys2.ydv9gxwp79.wasm",
        "integrity": "sha256-JnvhTx5LUtB6mfnbc8IIygQ4pApCYfFOMLpRtd9b9aQ="
      },
      {
        "virtualPath": "Toolbelt.Blazor.SplitContainer.wasm",
        "name": "Toolbelt.Blazor.SplitContainer.s4pm34tcwu.wasm",
        "integrity": "sha256-RJTQisTrDYqDjkinaQmVXdA/A1WeRe+Txuab6m2K98g="
      },
      {
        "virtualPath": "Toolbelt.Web.CssClassInlineBuilder.wasm",
        "name": "Toolbelt.Web.CssClassInlineBuilder.r0k6jvyfqx.wasm",
        "integrity": "sha256-21l5poJYk/1BPyr2mAfqNWLAFwQlyOuKBSrVo7sp+40="
      },
      {
        "virtualPath": "Microsoft.CSharp.wasm",
        "name": "Microsoft.CSharp.aars8n89vy.wasm",
        "integrity": "sha256-3gJ67eG9Tuze5feYA6UYbmLlqrEEzbioAgvEK+OE8DY="
      },
      {
        "virtualPath": "Microsoft.VisualBasic.Core.wasm",
        "name": "Microsoft.VisualBasic.Core.upxd87cdr1.wasm",
        "integrity": "sha256-8m+eoh3j2CkdhnDU7ragGuZyq3BwjyhlGG4L4y+UMfc="
      },
      {
        "virtualPath": "Microsoft.VisualBasic.wasm",
        "name": "Microsoft.VisualBasic.lo3va0ewdb.wasm",
        "integrity": "sha256-m7F1G6mWYfv0m+78bmZ0ZfyvPPLwAei7lQJN64m0/1k="
      },
      {
        "virtualPath": "Microsoft.Win32.Primitives.wasm",
        "name": "Microsoft.Win32.Primitives.xf0i7ifc2c.wasm",
        "integrity": "sha256-Q04hLLvVoYHZMKaK4n4wkH2dCS9V4HFK1zmVUovdvxQ="
      },
      {
        "virtualPath": "Microsoft.Win32.Registry.wasm",
        "name": "Microsoft.Win32.Registry.egcs66c38p.wasm",
        "integrity": "sha256-8rMGn/EzYH30LD6y/xwL+4RVksITbiQjQBF1Uz/Xmg8="
      },
      {
        "virtualPath": "System.AppContext.wasm",
        "name": "System.AppContext.229uy454et.wasm",
        "integrity": "sha256-WssdXF9UYOME7Yn9n9lb1Ul8zyLoMrBlt7C8V76uLxc="
      },
      {
        "virtualPath": "System.Buffers.wasm",
        "name": "System.Buffers.ffy4vk49sl.wasm",
        "integrity": "sha256-S1bP+mwrdo06J+IiAf+zz0rtwYartsBKUjeHztg5wZQ="
      },
      {
        "virtualPath": "System.Collections.Concurrent.wasm",
        "name": "System.Collections.Concurrent.vwjty21qm0.wasm",
        "integrity": "sha256-8btm7EOtpiXzGfhfwzmEpjtFwx02celUztylfjCr/F8="
      },
      {
        "virtualPath": "System.Collections.Immutable.wasm",
        "name": "System.Collections.Immutable.62ow9webvq.wasm",
        "integrity": "sha256-zgrZqD7M2kJn6O4a+olV+BXL7S4rTXt+3JMCOoA3oZw="
      },
      {
        "virtualPath": "System.Collections.NonGeneric.wasm",
        "name": "System.Collections.NonGeneric.8znze2bng9.wasm",
        "integrity": "sha256-JOoWnaAQS6hJn/RGGfWmWZhJBOI+GKDFXYzmmJ8TcA8="
      },
      {
        "virtualPath": "System.Collections.Specialized.wasm",
        "name": "System.Collections.Specialized.xek5hgzu5v.wasm",
        "integrity": "sha256-mZddTvPLwxNBWaLhU/GQGBO//1GoMWINzlkN3H5D2I0="
      },
      {
        "virtualPath": "System.Collections.wasm",
        "name": "System.Collections.0nghqyig8i.wasm",
        "integrity": "sha256-hE/sGzRTqgrQuV3Bh7WiiZL6jBvGOqKmWtVCf1OZe10="
      },
      {
        "virtualPath": "System.ComponentModel.Annotations.wasm",
        "name": "System.ComponentModel.Annotations.x5puxnf36m.wasm",
        "integrity": "sha256-5QcjYzxFcjAmRl6DmDb0f6bH+BYGwgw4JfZPH4HUhps="
      },
      {
        "virtualPath": "System.ComponentModel.DataAnnotations.wasm",
        "name": "System.ComponentModel.DataAnnotations.w951woxztj.wasm",
        "integrity": "sha256-9CCxjyp4kY8kWIu6wCjHgnZpgIZf6UI68yvIXm6YctY="
      },
      {
        "virtualPath": "System.ComponentModel.EventBasedAsync.wasm",
        "name": "System.ComponentModel.EventBasedAsync.xxfp6nuzvv.wasm",
        "integrity": "sha256-C8FNEouAC8CNin4gdVjQOdFrKEmLkx2YqtP09fY1baw="
      },
      {
        "virtualPath": "System.ComponentModel.Primitives.wasm",
        "name": "System.ComponentModel.Primitives.lfp6xvb2nc.wasm",
        "integrity": "sha256-v2zKFujiqwTb3P2cOPrXC1HEldWEI3pwgdZNFbQaGOU="
      },
      {
        "virtualPath": "System.ComponentModel.TypeConverter.wasm",
        "name": "System.ComponentModel.TypeConverter.b6pminnty5.wasm",
        "integrity": "sha256-8+Y/h8KkQLNlo4bvYZC1wzlXOPqOR8tVylHxx85xd1k="
      },
      {
        "virtualPath": "System.ComponentModel.wasm",
        "name": "System.ComponentModel.1kxs5658en.wasm",
        "integrity": "sha256-HxoU00taIL24knzyMNSGgjJgr/v+58ZN9Voi4xb+BPw="
      },
      {
        "virtualPath": "System.Configuration.wasm",
        "name": "System.Configuration.dfuzh9rwie.wasm",
        "integrity": "sha256-N1a6CdmRV9fO2aXVYOUySLRdAWOlFYYm3El247iEGJY="
      },
      {
        "virtualPath": "System.Console.wasm",
        "name": "System.Console.5bpbf67g8o.wasm",
        "integrity": "sha256-H/Sx5NlfVSeAtZeHtr7PMuQWVneNQVpCtCp7UNMrY8w="
      },
      {
        "virtualPath": "System.Core.wasm",
        "name": "System.Core.zxsh7ohdyg.wasm",
        "integrity": "sha256-OTzFvGFn5++iP4AbzhJjX2nGZVGMtjTo6gKknK2vxrw="
      },
      {
        "virtualPath": "System.Data.Common.wasm",
        "name": "System.Data.Common.f0gomia98j.wasm",
        "integrity": "sha256-8VqAXbEtUOzS+tplhOSfma2VjVA4Ktaw5vsKZDLOE+w="
      },
      {
        "virtualPath": "System.Data.DataSetExtensions.wasm",
        "name": "System.Data.DataSetExtensions.3k3coynn1w.wasm",
        "integrity": "sha256-/VKTmfp2bMHFmxYLN57I7CcU2kEdUFD4a+fUFKKT2ys="
      },
      {
        "virtualPath": "System.Data.wasm",
        "name": "System.Data.d1s9ppg3o7.wasm",
        "integrity": "sha256-z4yefYrpwUHZ9gq0MdCdTshjFcGsfybZuzyRH1ZfLUg="
      },
      {
        "virtualPath": "System.Diagnostics.Contracts.wasm",
        "name": "System.Diagnostics.Contracts.yj802auekg.wasm",
        "integrity": "sha256-TjWNA5+NGYI8MZDsoirxDZiMIpG4DVpNSRVgJ6JVxyw="
      },
      {
        "virtualPath": "System.Diagnostics.Debug.wasm",
        "name": "System.Diagnostics.Debug.d3luf33vju.wasm",
        "integrity": "sha256-SRHogrj3vV8crpFqMCG9UAeiiI+uQsrOz1RWGn+y9RI="
      },
      {
        "virtualPath": "System.Diagnostics.DiagnosticSource.wasm",
        "name": "System.Diagnostics.DiagnosticSource.4iwxfpqf71.wasm",
        "integrity": "sha256-NArlVa0x8G+wITe/2iga0/IDYX51slTbph1l/qD3Evc="
      },
      {
        "virtualPath": "System.Diagnostics.FileVersionInfo.wasm",
        "name": "System.Diagnostics.FileVersionInfo.pqffbmgf59.wasm",
        "integrity": "sha256-ccA+iO7gVZFXXl6zeZPGzgTgYKRCVwUUhiWRuqi9dqg="
      },
      {
        "virtualPath": "System.Diagnostics.Process.wasm",
        "name": "System.Diagnostics.Process.hgedtugxkh.wasm",
        "integrity": "sha256-cT9fW2eUn1cE+CZ7CLBjxieFx7SZ9xQv6I2B3v3lZPI="
      },
      {
        "virtualPath": "System.Diagnostics.StackTrace.wasm",
        "name": "System.Diagnostics.StackTrace.apl6dmc0ay.wasm",
        "integrity": "sha256-XriiYihH6QlM9+YB6QhQBxIZf7EpnvcsN4TSNOcCX6A="
      },
      {
        "virtualPath": "System.Diagnostics.TextWriterTraceListener.wasm",
        "name": "System.Diagnostics.TextWriterTraceListener.p621ruagc5.wasm",
        "integrity": "sha256-UZSunhOgYjFKFSPSVJzF3n1Kpm3TGVnqnBZ3ZiuC1cQ="
      },
      {
        "virtualPath": "System.Diagnostics.Tools.wasm",
        "name": "System.Diagnostics.Tools.f5hnfusd9n.wasm",
        "integrity": "sha256-baLr3RjKSvLNb/JDQ3OsjIfj8eZCGrt3t31aYQnw8Qw="
      },
      {
        "virtualPath": "System.Diagnostics.TraceSource.wasm",
        "name": "System.Diagnostics.TraceSource.z6jsaoha6p.wasm",
        "integrity": "sha256-K4qsHlYFQnE/+tyl4vVi3tp5bPJhFCUCtAVHLBBHwKM="
      },
      {
        "virtualPath": "System.Diagnostics.Tracing.wasm",
        "name": "System.Diagnostics.Tracing.d1w6x6c56c.wasm",
        "integrity": "sha256-T397nDWtv03LsEPL7f2EabF37N2kI7v0X53tkqEWHMw="
      },
      {
        "virtualPath": "System.Drawing.Primitives.wasm",
        "name": "System.Drawing.Primitives.l0kpbmmg5r.wasm",
        "integrity": "sha256-a2vpNVIyCKOAh7IVJE9U/VEOP3A2hVF8ZpqJFqIAy4g="
      },
      {
        "virtualPath": "System.Drawing.wasm",
        "name": "System.Drawing.ur4stg427u.wasm",
        "integrity": "sha256-1ruiHiGrBmbvLnir95mJVMTWTUjg/YmeVTypIkGvQsY="
      },
      {
        "virtualPath": "System.Dynamic.Runtime.wasm",
        "name": "System.Dynamic.Runtime.hhm1961hd1.wasm",
        "integrity": "sha256-FfnZg2GXW/4S1j6qwV70E2dmpjJDRJssZ5uT1nH2yuE="
      },
      {
        "virtualPath": "System.Formats.Asn1.wasm",
        "name": "System.Formats.Asn1.9b1kh77dim.wasm",
        "integrity": "sha256-W9/W8n9DfFSDP49YmZip6GY6HJuT0zegpJshTpFWM6o="
      },
      {
        "virtualPath": "System.Formats.Tar.wasm",
        "name": "System.Formats.Tar.pwu9oiomot.wasm",
        "integrity": "sha256-uaQsp+nl3nBqnL/D7Yb5z37CRybrB5d8zOWvH+LXAFc="
      },
      {
        "virtualPath": "System.Globalization.Calendars.wasm",
        "name": "System.Globalization.Calendars.w6686c3eg8.wasm",
        "integrity": "sha256-WPfj3mMulNgC/9qe/382WoDaKPWE6OfZUcRyp5FFi/s="
      },
      {
        "virtualPath": "System.Globalization.Extensions.wasm",
        "name": "System.Globalization.Extensions.koczvzp0tt.wasm",
        "integrity": "sha256-DFruHApFNJLjlR6F6N0DyHTX0ZKLWNc3zH/Dl6/2jeA="
      },
      {
        "virtualPath": "System.Globalization.wasm",
        "name": "System.Globalization.k8m97q3m8s.wasm",
        "integrity": "sha256-1JcNqQdI1SsK9ZxFHxWDAgIZ/cxrdjeNhJSKfFa4bl4="
      },
      {
        "virtualPath": "System.IO.Compression.Brotli.wasm",
        "name": "System.IO.Compression.Brotli.66hvl0dk85.wasm",
        "integrity": "sha256-rm3t7FvR+GtQZ86Mg24J28HoqZr79n5WSPB4S0t1uWU="
      },
      {
        "virtualPath": "System.IO.Compression.FileSystem.wasm",
        "name": "System.IO.Compression.FileSystem.jhbp0nl2b8.wasm",
        "integrity": "sha256-mW11MEGDlAS8hNlZCkbRZqUeJZEOLBu99ycr1HoxbMo="
      },
      {
        "virtualPath": "System.IO.Compression.ZipFile.wasm",
        "name": "System.IO.Compression.ZipFile.nap78ewmdw.wasm",
        "integrity": "sha256-MQTAsjsxBx/bC0GnAiMaeMsRXkKCgyhCWd4ugvt829c="
      },
      {
        "virtualPath": "System.IO.Compression.wasm",
        "name": "System.IO.Compression.wjdy7ns833.wasm",
        "integrity": "sha256-HFxXjIIxeat1OUzx2R8xaHEm9Rg7rKjl+2Lf2elQ7Xc="
      },
      {
        "virtualPath": "System.IO.FileSystem.AccessControl.wasm",
        "name": "System.IO.FileSystem.AccessControl.eod3jwbqn7.wasm",
        "integrity": "sha256-AvX28dKMjFnPq9Egaz20NWswlioY8lYL7UMsPgVm+G0="
      },
      {
        "virtualPath": "System.IO.FileSystem.DriveInfo.wasm",
        "name": "System.IO.FileSystem.DriveInfo.driq9z3dvq.wasm",
        "integrity": "sha256-eWZLHIjiBgJhvz5gAfN6SqzZ+Z7ySIWoe7tMaut3JAM="
      },
      {
        "virtualPath": "System.IO.FileSystem.Primitives.wasm",
        "name": "System.IO.FileSystem.Primitives.81fwc35czq.wasm",
        "integrity": "sha256-HNwbjHhTu3l1+TIFrDGhq+XjUHLXcHJOkmJzN7hGJKg="
      },
      {
        "virtualPath": "System.IO.FileSystem.Watcher.wasm",
        "name": "System.IO.FileSystem.Watcher.adgwo4kuza.wasm",
        "integrity": "sha256-In3DoobE9QC1SIcxRrT3ivBHgc+FbMmzlnSnHtPuC/s="
      },
      {
        "virtualPath": "System.IO.FileSystem.wasm",
        "name": "System.IO.FileSystem.bwu3sqaj6h.wasm",
        "integrity": "sha256-K2OU/ddMO+U131ff8dq7hZVlOjlHdgguqmysQBkZfl4="
      },
      {
        "virtualPath": "System.IO.IsolatedStorage.wasm",
        "name": "System.IO.IsolatedStorage.h5a2uwu0ao.wasm",
        "integrity": "sha256-GyI7mEGmDuTo491hNUqStDWSaWtxuANpZszEp7fyZ6Q="
      },
      {
        "virtualPath": "System.IO.MemoryMappedFiles.wasm",
        "name": "System.IO.MemoryMappedFiles.7rigkgafok.wasm",
        "integrity": "sha256-8/b6I9s5CvgNSnA/KCU5zLZS6QmJXZN31h5lvwCr3DA="
      },
      {
        "virtualPath": "System.IO.Pipelines.wasm",
        "name": "System.IO.Pipelines.pq56wwa14f.wasm",
        "integrity": "sha256-bx2O6FsHx5rfLrW9YgS8l5cyqft5Rya02xYnjNlxuSE="
      },
      {
        "virtualPath": "System.IO.Pipes.AccessControl.wasm",
        "name": "System.IO.Pipes.AccessControl.orawgey7bd.wasm",
        "integrity": "sha256-hOa42kowRBVd7flROVvpZCf4nQdfBrxVJFuwsNmAnDk="
      },
      {
        "virtualPath": "System.IO.Pipes.wasm",
        "name": "System.IO.Pipes.qv5hx9ne5m.wasm",
        "integrity": "sha256-Bq2o+zM9M88AbUPVDkiuApoJpsO/NfnnPAZ6zg3k+Xo="
      },
      {
        "virtualPath": "System.IO.UnmanagedMemoryStream.wasm",
        "name": "System.IO.UnmanagedMemoryStream.zjpuh62vtt.wasm",
        "integrity": "sha256-30XSWulMxTVsSH4eTh3EvERI+WAW5tnD6wtO5Yu2u3c="
      },
      {
        "virtualPath": "System.IO.wasm",
        "name": "System.IO.kdm5se4z4i.wasm",
        "integrity": "sha256-eGvuOoo6hoTWCkQpZ6+IgUZ44aMNRa4ZwRCBQMib6tc="
      },
      {
        "virtualPath": "System.Linq.AsyncEnumerable.wasm",
        "name": "System.Linq.AsyncEnumerable.24q069vvmu.wasm",
        "integrity": "sha256-Mr70upwO6JBSEkFlGzsqWhsCougBDOQSUZ7Ucpdcr/c="
      },
      {
        "virtualPath": "System.Linq.Expressions.wasm",
        "name": "System.Linq.Expressions.5vac4cfeo2.wasm",
        "integrity": "sha256-AWZYoxP0t+ICrbBXJtVSNOYLOExsSOaRN1+xUB1jjII="
      },
      {
        "virtualPath": "System.Linq.Parallel.wasm",
        "name": "System.Linq.Parallel.wec9zwybs3.wasm",
        "integrity": "sha256-GApx1/rKMYVxFzZKhqtSzgazG1ZFrF1SOBQ5f27+bk0="
      },
      {
        "virtualPath": "System.Linq.Queryable.wasm",
        "name": "System.Linq.Queryable.aacsqokaru.wasm",
        "integrity": "sha256-Mqcst10CvxJOY5rZO16yNBM6NmwnTpHZlb9r9Tjm8B8="
      },
      {
        "virtualPath": "System.Linq.wasm",
        "name": "System.Linq.xsk9czcoz7.wasm",
        "integrity": "sha256-kYmYRJKg26oh/snVKlXowSciwCDqnSA8ixdK3wnHgX8="
      },
      {
        "virtualPath": "System.Memory.wasm",
        "name": "System.Memory.av3iy8oqe6.wasm",
        "integrity": "sha256-FmbrzrgYvt8thm5f1o39K9LKKeNPn6YCkXI9ZiE5w9U="
      },
      {
        "virtualPath": "System.Net.Http.Json.wasm",
        "name": "System.Net.Http.Json.f5ti54ia1a.wasm",
        "integrity": "sha256-7UbhXREL81bSPpPCe4DxyBHUBLRuog1FZarax+8AZfY="
      },
      {
        "virtualPath": "System.Net.Http.wasm",
        "name": "System.Net.Http.xnexopyo77.wasm",
        "integrity": "sha256-g/ttnbXJx7qJQDM2tjlxxPc4YS19jpfKCptdMd1jdTA="
      },
      {
        "virtualPath": "System.Net.HttpListener.wasm",
        "name": "System.Net.HttpListener.qea3lr0ivv.wasm",
        "integrity": "sha256-jqcIIrG0ipv/+EzVdHzANNoe23LNDX80LA7O7ITekZ0="
      },
      {
        "virtualPath": "System.Net.Mail.wasm",
        "name": "System.Net.Mail.kabvwy0nwb.wasm",
        "integrity": "sha256-FA0IesoSqirr8eU4fyXNmvXSIAZyN8s7rFOBySx2n/c="
      },
      {
        "virtualPath": "System.Net.NameResolution.wasm",
        "name": "System.Net.NameResolution.r77xqyi48e.wasm",
        "integrity": "sha256-x+Rz/TsLnw1AaAz3rAjKA45sh8LEnqVx7A28h7/reFo="
      },
      {
        "virtualPath": "System.Net.NetworkInformation.wasm",
        "name": "System.Net.NetworkInformation.4y8ae4b47g.wasm",
        "integrity": "sha256-zFneucb0dssO09wgCYM+mygCgUY79ZVs5CH3FV8Zmyo="
      },
      {
        "virtualPath": "System.Net.Ping.wasm",
        "name": "System.Net.Ping.fb39s7nqmu.wasm",
        "integrity": "sha256-C4G5LxulCxldlE480n8kD5XdC9YLJoJu82NZqorL2mY="
      },
      {
        "virtualPath": "System.Net.Primitives.wasm",
        "name": "System.Net.Primitives.2oo4pve6tx.wasm",
        "integrity": "sha256-HrW4kLlKhKv1eJxLoHgBr5Sc16NXsWuQAV1YJLpLxJc="
      },
      {
        "virtualPath": "System.Net.Quic.wasm",
        "name": "System.Net.Quic.wedsu6m1ok.wasm",
        "integrity": "sha256-6IhW7a4wgNpdKcCgLnccMvYZnJhm8pAFwBfy0sEpZNQ="
      },
      {
        "virtualPath": "System.Net.Requests.wasm",
        "name": "System.Net.Requests.dd8um1v0v4.wasm",
        "integrity": "sha256-HxAKTEJm/k+VeskXeJVHaLc6O6uEjiGbQP1kai1JBh0="
      },
      {
        "virtualPath": "System.Net.Security.wasm",
        "name": "System.Net.Security.j01yc12epg.wasm",
        "integrity": "sha256-owlV6sdLRg9yetFkuF/NJeTlkcjt4x2D75W1jt4lUcM="
      },
      {
        "virtualPath": "System.Net.ServerSentEvents.wasm",
        "name": "System.Net.ServerSentEvents.n20pwunwwa.wasm",
        "integrity": "sha256-YWvgDUfszUT659eNxpGsPSPjp4dZoZ9W1MHg5+AaKkM="
      },
      {
        "virtualPath": "System.Net.ServicePoint.wasm",
        "name": "System.Net.ServicePoint.6qgdmrxct2.wasm",
        "integrity": "sha256-7sf9ZECanjhiYK5rl0nW25kL370/2X7wswsnaTCquIk="
      },
      {
        "virtualPath": "System.Net.Sockets.wasm",
        "name": "System.Net.Sockets.0jj55xk9rv.wasm",
        "integrity": "sha256-HLsnIASYq80lS16QqpRPfmz+MbyztpUpkhth1cEzlEA="
      },
      {
        "virtualPath": "System.Net.WebClient.wasm",
        "name": "System.Net.WebClient.3wc74qco9q.wasm",
        "integrity": "sha256-Ayk8X4rdnD8S2NlHmRBjkdZOtU8kb9wTvL/c/oEokug="
      },
      {
        "virtualPath": "System.Net.WebHeaderCollection.wasm",
        "name": "System.Net.WebHeaderCollection.4ybcvi6krs.wasm",
        "integrity": "sha256-hNEa+IFOoD/+JdmR0NrNVFzzAh42FRO1aWm9x5vTdAo="
      },
      {
        "virtualPath": "System.Net.WebProxy.wasm",
        "name": "System.Net.WebProxy.zxvs9bwlaq.wasm",
        "integrity": "sha256-SXogSzdjKcG5DG6UyQTcwM71XzKiPgLFJ1sYfndbWd0="
      },
      {
        "virtualPath": "System.Net.WebSockets.Client.wasm",
        "name": "System.Net.WebSockets.Client.n57nf3z1br.wasm",
        "integrity": "sha256-BbFaIYNUlwH4460WfB489UgIcAYeLh2BEPzJ6KI5pjo="
      },
      {
        "virtualPath": "System.Net.WebSockets.wasm",
        "name": "System.Net.WebSockets.i3475fl55i.wasm",
        "integrity": "sha256-gp4KiEzUX/qhvqniJZDsTc5EPRwBCRSTibR14eWoEPI="
      },
      {
        "virtualPath": "System.Net.wasm",
        "name": "System.Net.2jb9y9tls4.wasm",
        "integrity": "sha256-ntipoZNjDFpgIoZL2+cowUNaFBaWKt7eAw9Y/lYJyUU="
      },
      {
        "virtualPath": "System.Numerics.Vectors.wasm",
        "name": "System.Numerics.Vectors.tjoypuu8wa.wasm",
        "integrity": "sha256-N36JT9OQ2/L08Hf7azoiVDTukMEkI2cEHb1fktyBs3w="
      },
      {
        "virtualPath": "System.Numerics.wasm",
        "name": "System.Numerics.zb6snwiiqo.wasm",
        "integrity": "sha256-8VkwdV77IVGV8oP8jWcyFY+906chCyb8+j6rIrokgEo="
      },
      {
        "virtualPath": "System.ObjectModel.wasm",
        "name": "System.ObjectModel.qra8ipv6cl.wasm",
        "integrity": "sha256-hq6FRf6W+TsWp4e0E86ga4sziiGnapUO5l8MUFmpmHA="
      },
      {
        "virtualPath": "System.Private.DataContractSerialization.wasm",
        "name": "System.Private.DataContractSerialization.cdaesawoob.wasm",
        "integrity": "sha256-ANBBg0JT/2S2VNeD438Wf+PmlcXXKTTyBi823HZMZJw="
      },
      {
        "virtualPath": "System.Private.Uri.wasm",
        "name": "System.Private.Uri.d7c7uu4en8.wasm",
        "integrity": "sha256-iQ8wvGqvyFcs1iT1pdB2n8b/z9Mq83zK6Cods2Hwn+0="
      },
      {
        "virtualPath": "System.Private.Xml.Linq.wasm",
        "name": "System.Private.Xml.Linq.yklun9npsh.wasm",
        "integrity": "sha256-Pkx+wFIsSBbdhC8Ydrh+NaBr05//5uG9UcVGkGskulc="
      },
      {
        "virtualPath": "System.Private.Xml.wasm",
        "name": "System.Private.Xml.8fl2owujec.wasm",
        "integrity": "sha256-9BA+zqyZhjRnJDAbbfdNxsRfrgZwyDnE4/i51LAoguY="
      },
      {
        "virtualPath": "System.Reflection.DispatchProxy.wasm",
        "name": "System.Reflection.DispatchProxy.dur3huf884.wasm",
        "integrity": "sha256-S3eOzJcQvke43PsmexPGDIzWyieMbNkYb3jHx0UGAog="
      },
      {
        "virtualPath": "System.Reflection.Emit.ILGeneration.wasm",
        "name": "System.Reflection.Emit.ILGeneration.pei97jn90w.wasm",
        "integrity": "sha256-j/fA5WTsumDjxN2cKQTihwFLgsv3k792MiDWZ5F9ay4="
      },
      {
        "virtualPath": "System.Reflection.Emit.Lightweight.wasm",
        "name": "System.Reflection.Emit.Lightweight.ywsyqlj9p1.wasm",
        "integrity": "sha256-4gSOMD5HaI9rOga0ryYOIIw45X8cLcbKqfekiZr0nyc="
      },
      {
        "virtualPath": "System.Reflection.Emit.wasm",
        "name": "System.Reflection.Emit.tumhikm19o.wasm",
        "integrity": "sha256-9U/hGRr+m+4kX7koyKlAiMqz71022MlpL7IvKCqFUMU="
      },
      {
        "virtualPath": "System.Reflection.Extensions.wasm",
        "name": "System.Reflection.Extensions.z98cfn5ig8.wasm",
        "integrity": "sha256-568p4B9FblwEE8ORe6n8IXp5ypGSazKZcK78kNFNC4w="
      },
      {
        "virtualPath": "System.Reflection.Metadata.wasm",
        "name": "System.Reflection.Metadata.j4t1n1ywxj.wasm",
        "integrity": "sha256-TXuRF3bW1xqLJ/rQjBT0TgEq83MDIwsiB4/KasJL5Zo="
      },
      {
        "virtualPath": "System.Reflection.Primitives.wasm",
        "name": "System.Reflection.Primitives.p4ntci6hk7.wasm",
        "integrity": "sha256-Wc50BQPVdMIrZQVMJDCFqEdi73OmrmdH44hZpupNu04="
      },
      {
        "virtualPath": "System.Reflection.TypeExtensions.wasm",
        "name": "System.Reflection.TypeExtensions.puzzlnwex2.wasm",
        "integrity": "sha256-QREr11jd7UQohlfaM1KlTywvD7JasEb0zXEQV/oCLT0="
      },
      {
        "virtualPath": "System.Reflection.wasm",
        "name": "System.Reflection.lt32pnnkzd.wasm",
        "integrity": "sha256-J5s6vGKLVHPEEeSesDWoJfg1rK5nHKFPO8nh0+cQjaw="
      },
      {
        "virtualPath": "System.Resources.Reader.wasm",
        "name": "System.Resources.Reader.c8jfsntxbn.wasm",
        "integrity": "sha256-5IigMfJRD+jHmWBtENiG7yAucGgOB4g3SJ6B+TnGDYA="
      },
      {
        "virtualPath": "System.Resources.ResourceManager.wasm",
        "name": "System.Resources.ResourceManager.q30psrnl7u.wasm",
        "integrity": "sha256-xmSz2cgz7i1MVOIKRJZGlOstVMz6ngeaoqxl8e6HWQ0="
      },
      {
        "virtualPath": "System.Resources.Writer.wasm",
        "name": "System.Resources.Writer.wvyn0j1lcg.wasm",
        "integrity": "sha256-XIzhaSBP9mF4R0KxLLlwzMwZowWgxNA0akSKXk1v8tw="
      },
      {
        "virtualPath": "System.Runtime.CompilerServices.Unsafe.wasm",
        "name": "System.Runtime.CompilerServices.Unsafe.djrvtrz9tb.wasm",
        "integrity": "sha256-KZaFjGXwS8UKLDPsZ1Ry2s6vhdtPZewdY2F3Bn7ht6A="
      },
      {
        "virtualPath": "System.Runtime.CompilerServices.VisualC.wasm",
        "name": "System.Runtime.CompilerServices.VisualC.luekj4crl6.wasm",
        "integrity": "sha256-LYtyUyzvr89kL7xicdpxXmL9woEnZ4ft6PP2FqInGt0="
      },
      {
        "virtualPath": "System.Runtime.Extensions.wasm",
        "name": "System.Runtime.Extensions.d5mymlulmr.wasm",
        "integrity": "sha256-oQ7tJvFK6fhslwTrGQEHUQ8WPKm/ukm8D6qRyPTie8o="
      },
      {
        "virtualPath": "System.Runtime.Handles.wasm",
        "name": "System.Runtime.Handles.ih3cmfwxzd.wasm",
        "integrity": "sha256-pk573cXMJTUukX3OqQNQCbOlmUK8MSkSUIhiYdgRKxQ="
      },
      {
        "virtualPath": "System.Runtime.InteropServices.RuntimeInformation.wasm",
        "name": "System.Runtime.InteropServices.RuntimeInformation.nvwt5l6jkw.wasm",
        "integrity": "sha256-TfsdWwk+9DuqEizuk8BU4fl0Mfj40j0tMYNBM5c5/Xs="
      },
      {
        "virtualPath": "System.Runtime.InteropServices.wasm",
        "name": "System.Runtime.InteropServices.0us88a4qml.wasm",
        "integrity": "sha256-CIyJJNugh8zYFK3llYK2eFgYac6YUzVX+SwMw50IADU="
      },
      {
        "virtualPath": "System.Runtime.Intrinsics.wasm",
        "name": "System.Runtime.Intrinsics.bd7qf7pr2d.wasm",
        "integrity": "sha256-MR0joPBdJtCVIs7LNKWYcQnUpdvmew3tJShYuQ3QAHw="
      },
      {
        "virtualPath": "System.Runtime.Loader.wasm",
        "name": "System.Runtime.Loader.opo019ewpt.wasm",
        "integrity": "sha256-HMLRnfB3nllRnIJLnkvHqoPEEA4Gf2MXf19MbRh2oCg="
      },
      {
        "virtualPath": "System.Runtime.Numerics.wasm",
        "name": "System.Runtime.Numerics.opgppvbdi6.wasm",
        "integrity": "sha256-JFqanWY84LrwPzl3nMjc2/10emcUXjc83c62YaczyKY="
      },
      {
        "virtualPath": "System.Runtime.Serialization.Formatters.wasm",
        "name": "System.Runtime.Serialization.Formatters.seonqd55ks.wasm",
        "integrity": "sha256-rOajPI1BHYn4h8K+izkyPjy8YEUX0cMvrsB4W4lLPsM="
      },
      {
        "virtualPath": "System.Runtime.Serialization.Json.wasm",
        "name": "System.Runtime.Serialization.Json.zn0mp7kdmp.wasm",
        "integrity": "sha256-IZ703XHr5yuPJtjoVcOmSiuOwMS7oUnWFemeMuYtPjE="
      },
      {
        "virtualPath": "System.Runtime.Serialization.Primitives.wasm",
        "name": "System.Runtime.Serialization.Primitives.yueinvlj2j.wasm",
        "integrity": "sha256-RoHClekbaVuC/3ay0FCVI5mCkDpbRZJ4ebdWu2EbC2c="
      },
      {
        "virtualPath": "System.Runtime.Serialization.Xml.wasm",
        "name": "System.Runtime.Serialization.Xml.v8gymdawqg.wasm",
        "integrity": "sha256-v1Cvl1Bf9lBzICh5AIuWch8tdKQhdXbG7DqdhN8yK3A="
      },
      {
        "virtualPath": "System.Runtime.Serialization.wasm",
        "name": "System.Runtime.Serialization.gs7sg0u955.wasm",
        "integrity": "sha256-kLlU8Fg6mBXnrWV8AUpYvnAcfbums3BRQ/riNZvxlq0="
      },
      {
        "virtualPath": "System.Runtime.wasm",
        "name": "System.Runtime.6bp69qbwkj.wasm",
        "integrity": "sha256-XjnS5kU5PJ7mLAJfWVDsm76gDJA5zRYJXtib4kVmg+s="
      },
      {
        "virtualPath": "System.Security.AccessControl.wasm",
        "name": "System.Security.AccessControl.6rjkcrpkiw.wasm",
        "integrity": "sha256-/vaGuzoY6wS2zhrRxCFlEtOm6Am+qnB4ZJcQH714VIc="
      },
      {
        "virtualPath": "System.Security.Claims.wasm",
        "name": "System.Security.Claims.hk3ae85l96.wasm",
        "integrity": "sha256-ka7qPyTx8HQ3O6QpiObd/tLfTJ2KrVwAkngVxEQyfvg="
      },
      {
        "virtualPath": "System.Security.Cryptography.Algorithms.wasm",
        "name": "System.Security.Cryptography.Algorithms.whyyu092ah.wasm",
        "integrity": "sha256-JNEruKcOjbV3LYX7lW+6F2IByd+lUDksyL1L35Rr/3I="
      },
      {
        "virtualPath": "System.Security.Cryptography.Cng.wasm",
        "name": "System.Security.Cryptography.Cng.m6se2pcpu2.wasm",
        "integrity": "sha256-0jMJ20LIrIydx5tKNMyvrlJcK1n+4vZRGjwjDTgqEC0="
      },
      {
        "virtualPath": "System.Security.Cryptography.Csp.wasm",
        "name": "System.Security.Cryptography.Csp.hhoaoktf3s.wasm",
        "integrity": "sha256-i+2+zr0tjXXBsjfrzAI4mp02C7hs9cPDF5Im45pAToc="
      },
      {
        "virtualPath": "System.Security.Cryptography.Encoding.wasm",
        "name": "System.Security.Cryptography.Encoding.52vboi33gw.wasm",
        "integrity": "sha256-A/64Xq6L99D6Nm+M0enQPl/m3vOgStidagno4eYvlYw="
      },
      {
        "virtualPath": "System.Security.Cryptography.OpenSsl.wasm",
        "name": "System.Security.Cryptography.OpenSsl.ml0tyz68i8.wasm",
        "integrity": "sha256-StZupImYSk5LiJhK4t5L0CbPkMm6wt5urWX4gFQKnto="
      },
      {
        "virtualPath": "System.Security.Cryptography.Primitives.wasm",
        "name": "System.Security.Cryptography.Primitives.1dlu2dh7y8.wasm",
        "integrity": "sha256-pS05GqnH2PM85VghHMYs7xyGmkHcQGD/I9d7ev0MN3w="
      },
      {
        "virtualPath": "System.Security.Cryptography.X509Certificates.wasm",
        "name": "System.Security.Cryptography.X509Certificates.6uh0qispd4.wasm",
        "integrity": "sha256-ssNeg35i32Sw3cZlBmP5bAJPkFxXGzOFdGFzkNyQF7g="
      },
      {
        "virtualPath": "System.Security.Cryptography.wasm",
        "name": "System.Security.Cryptography.mbts90zzn6.wasm",
        "integrity": "sha256-juPDhNBkkR+tWofeK/InqaKq7r8hhQ4MkKORw6wGK58="
      },
      {
        "virtualPath": "System.Security.Principal.Windows.wasm",
        "name": "System.Security.Principal.Windows.rp57epmxji.wasm",
        "integrity": "sha256-UXWNkNDLTzXH92t+pdcE67GAMNjaZobYuc1m6Syh50Q="
      },
      {
        "virtualPath": "System.Security.Principal.wasm",
        "name": "System.Security.Principal.0lplkfn42e.wasm",
        "integrity": "sha256-xBQdFk8YXfkUf97UXhTzW0Nme0j1tGi6bfUx2XNtvzw="
      },
      {
        "virtualPath": "System.Security.SecureString.wasm",
        "name": "System.Security.SecureString.dhwpww24h2.wasm",
        "integrity": "sha256-sTD7LfR7tR8aPciFIFfSGb2qoRyDTRdemq+HOGHUa4Q="
      },
      {
        "virtualPath": "System.Security.wasm",
        "name": "System.Security.l9hsxb0sdk.wasm",
        "integrity": "sha256-l+wMiCYTpZCWPr/O6nj+GxL1fJgtYrGvF+0O1TpsPLI="
      },
      {
        "virtualPath": "System.ServiceModel.Web.wasm",
        "name": "System.ServiceModel.Web.zcvkay2idv.wasm",
        "integrity": "sha256-PXI//3eq/7GcJnzY7PnpSJARMqRCjzZ+pdEG5flIU7w="
      },
      {
        "virtualPath": "System.ServiceProcess.wasm",
        "name": "System.ServiceProcess.x10kdcthsy.wasm",
        "integrity": "sha256-u9V5OG0A7gfeD4P79wXU9r9Kmq55vLv2C7pIwFzSYcY="
      },
      {
        "virtualPath": "System.Text.Encoding.CodePages.wasm",
        "name": "System.Text.Encoding.CodePages.36p8aommvy.wasm",
        "integrity": "sha256-lUReuxzuMjaHtCIWpc3dCyVOrWZYd4CEjxv32BJDpyk="
      },
      {
        "virtualPath": "System.Text.Encoding.Extensions.wasm",
        "name": "System.Text.Encoding.Extensions.7s9z475qvj.wasm",
        "integrity": "sha256-R+xSscSMvYx41WPDTPNk64UQpBxmEdqEc6Oo+SvkUeE="
      },
      {
        "virtualPath": "System.Text.Encoding.wasm",
        "name": "System.Text.Encoding.ldc2rqh80i.wasm",
        "integrity": "sha256-KV5UfRvlLWt5UrI4a/z74ot1UMZpDMDLCJ4LSUP/uII="
      },
      {
        "virtualPath": "System.Text.Encodings.Web.wasm",
        "name": "System.Text.Encodings.Web.8dol48h5sw.wasm",
        "integrity": "sha256-nGIOG4uzsQEbo998Q7Zamu0rXC78HnvTtxpIfSw1L60="
      },
      {
        "virtualPath": "System.Text.Json.wasm",
        "name": "System.Text.Json.3vc5r851ef.wasm",
        "integrity": "sha256-X3vAVtULtSZRYgEshi66iHc6dxJXHsvDo1MmJ8yFSQE="
      },
      {
        "virtualPath": "System.Text.RegularExpressions.wasm",
        "name": "System.Text.RegularExpressions.fa55zq7noc.wasm",
        "integrity": "sha256-B4WyuZja/UtHg4zhOPH85pHUiniy5+3RB1hyireaysQ="
      },
      {
        "virtualPath": "System.Threading.AccessControl.wasm",
        "name": "System.Threading.AccessControl.0jx6t23d4a.wasm",
        "integrity": "sha256-POSjtb073zJHnd4FDpF1624Y2w13hciDXmHxsKrM8OI="
      },
      {
        "virtualPath": "System.Threading.Channels.wasm",
        "name": "System.Threading.Channels.5yls0a3tlo.wasm",
        "integrity": "sha256-HTZOo3BqJ2QUqsnNWx12DkExRh1BhK8cTlxgoXJ9Ae0="
      },
      {
        "virtualPath": "System.Threading.Overlapped.wasm",
        "name": "System.Threading.Overlapped.7fzdvfjpcj.wasm",
        "integrity": "sha256-bYCxenIoJmaa8lU0pUlzehe/UCqAoXnsfzXxA5MdET4="
      },
      {
        "virtualPath": "System.Threading.Tasks.Dataflow.wasm",
        "name": "System.Threading.Tasks.Dataflow.b1bop2hs5a.wasm",
        "integrity": "sha256-314zVzc8S0pI9lhDNq4EAgTl7Y+Va4pNUZeMZmZj/o4="
      },
      {
        "virtualPath": "System.Threading.Tasks.Extensions.wasm",
        "name": "System.Threading.Tasks.Extensions.w1aib1cdwa.wasm",
        "integrity": "sha256-nG3YldQlqbXqF8T29X0TfUwmXvgGldNVBPLysh6EaOU="
      },
      {
        "virtualPath": "System.Threading.Tasks.Parallel.wasm",
        "name": "System.Threading.Tasks.Parallel.k09pj8k6wz.wasm",
        "integrity": "sha256-5MyMawFkxrVtMdCaxt9SJWbDdaFkAs5TsdDUOJqZVnc="
      },
      {
        "virtualPath": "System.Threading.Tasks.wasm",
        "name": "System.Threading.Tasks.ixxtp5uj8k.wasm",
        "integrity": "sha256-+iHad5h+QbqyxQd4dCk8LAuMoH76dUrj4xQxsv5X63o="
      },
      {
        "virtualPath": "System.Threading.Thread.wasm",
        "name": "System.Threading.Thread.oikmdgabjt.wasm",
        "integrity": "sha256-YN6xDfKc00Uj33FA1RAI1dE9z2TyNB4DWvXrt7YkdAg="
      },
      {
        "virtualPath": "System.Threading.ThreadPool.wasm",
        "name": "System.Threading.ThreadPool.szctd5y8bn.wasm",
        "integrity": "sha256-CGiwSL1quoNt4LxugtDTwrREp+M4tJJESPlGdxdK4oU="
      },
      {
        "virtualPath": "System.Threading.Timer.wasm",
        "name": "System.Threading.Timer.k8rrsskw5o.wasm",
        "integrity": "sha256-XHE6vTnfUXeouXM7xp2MAr3sVNqaDj8lUsXkA3S20Gk="
      },
      {
        "virtualPath": "System.Threading.wasm",
        "name": "System.Threading.s2hrn953g9.wasm",
        "integrity": "sha256-NNqqX2vjjeUfU00pgnRj1haCraJdawgoEARioA+wgT4="
      },
      {
        "virtualPath": "System.Transactions.Local.wasm",
        "name": "System.Transactions.Local.nufsb5pyrr.wasm",
        "integrity": "sha256-wZGoSjejHXWu5w/2rlsLCLyllKh2DbnV4BCM4ZJj3Zs="
      },
      {
        "virtualPath": "System.Transactions.wasm",
        "name": "System.Transactions.3ezo8twol7.wasm",
        "integrity": "sha256-1Zoj0mgGZdKxMBQnIPNzmcnbQxD4WAgI4JhpnActsgg="
      },
      {
        "virtualPath": "System.ValueTuple.wasm",
        "name": "System.ValueTuple.lle7yi2fzv.wasm",
        "integrity": "sha256-V/Dw40uSB6rSKAZiFOGmfAZCRWvLZLlx1oILmMJl2l0="
      },
      {
        "virtualPath": "System.Web.HttpUtility.wasm",
        "name": "System.Web.HttpUtility.u5rdfpwou6.wasm",
        "integrity": "sha256-wj0yPwPNZXELkVAMQaMiUp9kO2lZVCC+VVXSQKIsUos="
      },
      {
        "virtualPath": "System.Web.wasm",
        "name": "System.Web.wygikf6la5.wasm",
        "integrity": "sha256-mAkfggK9Utiv1JIAbtn6IJaybrmam1gf4qqbX6fsfK8="
      },
      {
        "virtualPath": "System.Windows.wasm",
        "name": "System.Windows.axtn86mzz6.wasm",
        "integrity": "sha256-wgipO7KiyIOlKfYDvfGpFBiwATM63xXSVjt0ZtW7nZQ="
      },
      {
        "virtualPath": "System.Xml.Linq.wasm",
        "name": "System.Xml.Linq.j4b5ce3025.wasm",
        "integrity": "sha256-bYRQzNES2R69uuOmvHveYc8p0tT8LG1iN9FYebdpcdM="
      },
      {
        "virtualPath": "System.Xml.ReaderWriter.wasm",
        "name": "System.Xml.ReaderWriter.60z52rxro3.wasm",
        "integrity": "sha256-imEzNCgsMfKb+rhr/t/ehYoI9tBe2JbvmGkOXv4b26s="
      },
      {
        "virtualPath": "System.Xml.Serialization.wasm",
        "name": "System.Xml.Serialization.pm5yw3aolk.wasm",
        "integrity": "sha256-/wJSM0ED/FHWBp7tqT1EeDEGT54CCo/ENbT+Ydk3bkY="
      },
      {
        "virtualPath": "System.Xml.XDocument.wasm",
        "name": "System.Xml.XDocument.b4xtw4lbmx.wasm",
        "integrity": "sha256-FfNo7biuX3HWJddNcbTEIOWrSHttQHCjgZNRkRU5Bqk="
      },
      {
        "virtualPath": "System.Xml.XPath.XDocument.wasm",
        "name": "System.Xml.XPath.XDocument.p6s7bmw3ej.wasm",
        "integrity": "sha256-cU0wzV4v6Motvhc4MKUzvr1D8xbcsUkeZ4PWeNH4tL0="
      },
      {
        "virtualPath": "System.Xml.XPath.wasm",
        "name": "System.Xml.XPath.egozus3d9z.wasm",
        "integrity": "sha256-jqTPxvf1zGJ4ZjRqCIYyO3FHEkk9/RhOEscGtOTiSbQ="
      },
      {
        "virtualPath": "System.Xml.XmlDocument.wasm",
        "name": "System.Xml.XmlDocument.771pf23z5h.wasm",
        "integrity": "sha256-rYaPQZTnWjXchEjYFx1cVkbTD1Y2PFXaZxuGJTjxZwg="
      },
      {
        "virtualPath": "System.Xml.XmlSerializer.wasm",
        "name": "System.Xml.XmlSerializer.h0uogtkh03.wasm",
        "integrity": "sha256-Dy6UAhHi4CzqlUM49gXNBY2TYVEnCp3DnGLdDE3s/UM="
      },
      {
        "virtualPath": "System.Xml.wasm",
        "name": "System.Xml.95nan2l20b.wasm",
        "integrity": "sha256-rSJv2ezwYuwgh2UtNgGITSBVA9dDNZCJA6ZhDQvp4bM="
      },
      {
        "virtualPath": "System.wasm",
        "name": "System.au65xu8d95.wasm",
        "integrity": "sha256-4mrPT1s4nD5ihFhuGnNShrvZRXWUX6OJlBeSs4JEHG4="
      },
      {
        "virtualPath": "WindowsBase.wasm",
        "name": "WindowsBase.mi3hkxthj8.wasm",
        "integrity": "sha256-8gsq4MhnOZbA8BrhnJy+aXHDPPZ1GRqx4vzIebgCJAg="
      },
      {
        "virtualPath": "mscorlib.wasm",
        "name": "mscorlib.uifnjyorgo.wasm",
        "integrity": "sha256-VqkriGENEsUw3rgDWUOjRvrIziYiKYBITxpVELFv8NI="
      },
      {
        "virtualPath": "netstandard.wasm",
        "name": "netstandard.qvqeanc7qr.wasm",
        "integrity": "sha256-lgUuHy5ZKKRbgYfoW8t1tWMzuTpZuIcjJroQScqry7w="
      },
      {
        "virtualPath": "MyBlazorWasmApp1.wasm",
        "name": "MyBlazorWasmApp1.9sukhpk1p5.wasm",
        "integrity": "sha256-J/J60+11h8zmagwyo6iuWlHWx9XKwtMHVQKCGxAHSJA="
      },
      {
        "virtualPath": "MyBlazorWasmApp1.Stories.wasm",
        "name": "MyBlazorWasmApp1.Stories.bgyqguntu4.wasm",
        "integrity": "sha256-6yiCGCqEUag4F+SzwFFjZGmIFVw0Okym/FSquYxkFjk="
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
