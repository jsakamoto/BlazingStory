//! Licensed to the .NET Foundation under one or more agreements.
//! The .NET Foundation licenses this file to you under the MIT license.

const e=()=>(async()=>{try{return new WebAssembly.Module(Uint8Array.from(atob("AGFzbQEAAAABBAFgAAADAgEAChABDgACaR9AAQMAAAsACxoL"),e=>e.codePointAt(0))),!0}catch(e){return!1}})(),o=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,5,1,96,0,1,123,3,2,1,0,10,15,1,13,0,65,1,253,15,65,2,253,15,253,128,2,11])),t=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,5,1,96,0,1,123,3,2,1,0,10,10,1,8,0,65,0,253,15,253,98,11])),n=Symbol.for("wasm promise_control");function r(e,o){let t=null;const r=new Promise(function(n,r){t={isDone:!1,promise:null,resolve:o=>{t.isDone||(t.isDone=!0,n(o),e&&e())},reject:e=>{t.isDone||(t.isDone=!0,r(e),o&&o())}}});t.promise=r;const s=r;return s[n]=t,{promise:s,promise_control:t}}function s(e){return e[n]}function i(e){e&&function(e){return void 0!==e[n]}(e)||We(!1,"Promise is not controllable")}const a="__mono_message__",l=["debug","log","trace","warn","info","error"],c="MONO_WASM: ";let d,u,f,m,g,p;function h(e){m=e}function b(e){if(ke.diagnosticTracing){const o="function"==typeof e?e():e;console.debug(c+o)}}function w(e,...o){console.info(c+e,...o)}function y(e,...o){console.info(e,...o)}function v(e,...o){console.warn(c+e,...o)}function _(e,...o){if(o&&o.length>0&&o[0]&&"object"==typeof o[0]){if(o[0].silent)return;if(o[0].toString)return void console.error(c+e,o[0].toString())}console.error(c+e,...o)}function A(e,o,t){return function(...n){try{let r=n[0];if(void 0===r)r="undefined";else if(null===r)r="null";else if("function"==typeof r)r=r.toString();else if("string"!=typeof r)try{r=JSON.stringify(r)}catch(e){r=r.toString()}o(t?JSON.stringify({method:e,payload:r,arguments:n.slice(1)}):[e+r,...n.slice(1)])}catch(e){f.error(`proxyConsole failed: ${e}`)}}}function x(e,o,t){u=o,m=e,f={...o};const n=`${t}/console`.replace("https://","wss://").replace("http://","ws://");d=new WebSocket(n),d.addEventListener("error",E),d.addEventListener("close",j),function(){for(const e of l)u[e]=A(`console.${e}`,R,!0)}()}function T(e){let o=30;const t=()=>{d?0==d.bufferedAmount||0==o?(e&&y(e),function(){for(const e of l)u[e]=A(`console.${e}`,f.log,!1)}(),d.removeEventListener("error",E),d.removeEventListener("close",j),d.close(1e3,e),d=void 0):(o--,globalThis.setTimeout(t,100)):e&&f&&f.log(e)};t()}function R(e){d&&d.readyState===WebSocket.OPEN?d.send(e):f.log(e)}function E(e){f.error(`[${m}] proxy console websocket error: ${e}`,e)}function j(e){f.debug(`[${m}] proxy console websocket closed: ${e}`,e)}function D(){ke.preferredIcuAsset=C(ke.config);let e="invariant"==ke.config.globalizationMode;if(!e)if(ke.preferredIcuAsset)ke.diagnosticTracing&&b("ICU data archive(s) available, disabling invariant mode");else{if("custom"===ke.config.globalizationMode||"all"===ke.config.globalizationMode||"sharded"===ke.config.globalizationMode){const e="invariant globalization mode is inactive and no ICU data archives are available";throw _(`ERROR: ${e}`),new Error(e)}ke.diagnosticTracing&&b("ICU data archive(s) not available, using invariant globalization mode"),e=!0,ke.preferredIcuAsset=null}const o="DOTNET_SYSTEM_GLOBALIZATION_INVARIANT",t=ke.config.environmentVariables;if(void 0===t[o]&&e&&(t[o]="1"),void 0===t.TZ)try{const e=Intl.DateTimeFormat().resolvedOptions().timeZone||null;e&&(t.TZ=e)}catch(e){w("failed to detect timezone, will fallback to UTC")}}function C(e){var o;if((null===(o=e.resources)||void 0===o?void 0:o.icu)&&"invariant"!=e.globalizationMode){const o=e.applicationCulture||(Me?globalThis.navigator&&globalThis.navigator.languages&&globalThis.navigator.languages[0]:Intl.DateTimeFormat().resolvedOptions().locale);e.applicationCulture||(e.applicationCulture=o);const t=e.resources.icu;let n=null;if("custom"===e.globalizationMode){if(t.length>=1)return t[0].name}else o&&"all"!==e.globalizationMode?"sharded"===e.globalizationMode&&(n=function(e){const o=e.split("-")[0];return"en"===o||["fr","fr-FR","it","it-IT","de","de-DE","es","es-ES"].includes(e)?"icudt_EFIGS.dat":["zh","ko","ja"].includes(o)?"icudt_CJK.dat":"icudt_no_CJK.dat"}(o)):n="icudt.dat";if(n)for(let e=0;e<t.length;e++){const o=t[e];if(o.virtualPath===n)return o.name}}return e.globalizationMode="invariant",null}(new Date).valueOf();const M=class{constructor(e){this.url=e}toString(){return this.url}};async function S(e){if(Se&&"function"!=typeof globalThis.atob){const e="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";globalThis.atob=o=>{const t=String(o).replace(/=+$/,"");let n="";for(let o=0,r=0,s=0;s<t.length;s++){const i=e.indexOf(t.charAt(s));-1!==i&&(r=o%4?64*r+i:i,o++%4&&(n+=String.fromCharCode(255&r>>(-2*o&6))))}return n}}if(Ee){const e=await import(/*! webpackIgnore: true */"process"),o=14;if(e.versions.node.split(".")[0]<o)throw new Error(`NodeJS at '${e.execPath}' has too low version '${e.versions.node}', please use at least ${o}.`)}const o=/*! webpackIgnore: true */import.meta.url,t=o.indexOf("?");var n;if(t>0&&(ke.modulesUniqueQuery=o.substring(t)),ke.scriptUrl=o.replace(/\\/g,"/").replace(/[?#].*/,""),ke.scriptDirectory=(n=ke.scriptUrl).slice(0,n.lastIndexOf("/"))+"/",ke.locateFile=e=>"URL"in globalThis&&globalThis.URL!==M?new URL(e,ke.scriptDirectory).toString():I(e)?e:ke.scriptDirectory+e,ke.fetch_like=U,ke.out=console.log,ke.err=console.error,ke.onDownloadResourceProgress=e.onDownloadResourceProgress,Me&&globalThis.navigator){const e=globalThis.navigator,o=e.userAgentData&&e.userAgentData.brands;o&&o.length>0?ke.isChromium=o.some(e=>"Google Chrome"===e.brand||"Microsoft Edge"===e.brand||"Chromium"===e.brand):e.userAgent&&(ke.isChromium=e.userAgent.includes("Chrome"),ke.isFirefox=e.userAgent.includes("Firefox"))}void 0===globalThis.URL&&(globalThis.URL=M)}async function U(e,o){try{const t="function"==typeof globalThis.fetch;if(Ee){const n=e.startsWith("file://");if(!n&&t)return globalThis.fetch(e,o||{credentials:"same-origin"});g||(p=await import(/*! webpackIgnore: true */"url"),g=await import(/*! webpackIgnore: true */"fs")),n&&(e=p.fileURLToPath(e));const r=await g.promises.readFile(e);return{ok:!0,headers:{length:0,get:()=>null},url:e,arrayBuffer:()=>r,json:()=>JSON.parse(r),text:()=>{throw new Error("NotImplementedException")}}}if(t)return globalThis.fetch(e,o||{credentials:"same-origin"});if("function"==typeof read)return{ok:!0,url:e,headers:{length:0,get:()=>null},arrayBuffer:()=>new Uint8Array(read(e,"binary")),json:()=>JSON.parse(read(e,"utf8")),text:()=>read(e,"utf8")}}catch(o){return{ok:!1,url:e,status:500,headers:{length:0,get:()=>null},statusText:"ERR28: "+o,arrayBuffer:()=>{throw o},json:()=>{throw o},text:()=>{throw o}}}throw new Error("No fetch implementation available")}const P=/^[a-zA-Z][a-zA-Z\d+\-.]*?:\/\//,k=/[a-zA-Z]:[\\/]/;function I(e){return Ee||Se?e.startsWith("/")||e.startsWith("\\")||-1!==e.indexOf("///")||k.test(e):P.test(e)}let O,$=0;const L=[],N=[],z=new Map,F={"js-module-runtime":!0,"js-module-dotnet":!0,"js-module-native":!0,"js-module-diagnostics":!0},W={...F,"js-module-library-initializer":!0},V={...F,dotnetwasm:!0,heap:!0,manifest:!0},B={...W,manifest:!0},J={...W,dotnetwasm:!0},H={dotnetwasm:!0,symbols:!0},Q={...W,dotnetwasm:!0,symbols:!0},q={symbols:!0};function G(e){return!("icu"==e.behavior&&e.name!=ke.preferredIcuAsset)}function Z(e,o,t){null!=o||(o=[]),We(1==o.length,`Expect to have one ${t} asset in resources`);const n=o[0];return n.behavior=t,K(n),e.push(n),n}function K(e){V[e.behavior]&&z.set(e.behavior,e)}function X(e){We(V[e],`Unknown single asset behavior ${e}`);const o=z.get(e);if(o&&!o.resolvedUrl)if(o.resolvedUrl=ke.locateFile(o.name),F[o.behavior]){const e=me(o);e?("string"!=typeof e&&We(!1,"loadBootResource response for 'dotnetjs' type should be a URL string"),o.resolvedUrl=e):o.resolvedUrl=le(o.resolvedUrl,o.behavior)}else if("dotnetwasm"!==o.behavior)throw new Error(`Unknown single asset behavior ${e}`);return o}function Y(e){const o=X(e);return We(o,`Single asset for ${e} not found`),o}let ee=!1;async function oe(){if(!ee){ee=!0,ke.diagnosticTracing&&b("mono_download_assets");try{const e=[],o=[],t=(e,o)=>{!Q[e.behavior]&&G(e)&&ke.expected_instantiated_assets_count++,!J[e.behavior]&&G(e)&&(ke.expected_downloaded_assets_count++,o.push(se(e)))};for(const o of L)t(o,e);for(const e of N)t(e,o);ke.allDownloadsQueued.promise_control.resolve(),Promise.all([...e,...o]).then(()=>{ke.allDownloadsFinished.promise_control.resolve()}).catch(e=>{throw ke.err("Error in mono_download_assets: "+e),Xe(1,e),e}),await ke.runtimeModuleLoaded.promise;const n=async e=>{const o=await e;if(H[o.behavior])return"symbols"===o.behavior&&(await Pe.instantiate_symbols_asset(o),ge(o)),void++ke.actual_downloaded_assets_count;if(o.buffer){if(!Q[o.behavior]){o.buffer&&"object"==typeof o.buffer||We(!1,"asset buffer must be array-like or buffer-like or promise of these"),"string"!=typeof o.resolvedUrl&&We(!1,"resolvedUrl must be string");const e=o.resolvedUrl,t=await o.buffer,n=new Uint8Array(t);ge(o),await Pe.beforeOnRuntimeInitialized.promise,await Pe.afterInstantiateWasm.promise,Pe.instantiate_asset(o,e,n)}}else o.isOptional||We(!1,"Expected asset to have the downloaded buffer"),!J[o.behavior]&&G(o)&&ke.expected_downloaded_assets_count--,!Q[o.behavior]&&G(o)&&ke.expected_instantiated_assets_count--},r=[],s=[];for(const o of e)r.push(n(o));for(const e of o)s.push(n(e));Promise.all(r).then(()=>{Ce||Pe.coreAssetsInMemory.promise_control.resolve()}).catch(e=>{throw ke.err("Error in mono_download_assets: "+e),Xe(1,e),e}),Promise.all(s).then(async()=>{Ce||(await Pe.coreAssetsInMemory.promise,Pe.allAssetsInMemory.promise_control.resolve())}).catch(e=>{throw ke.err("Error in mono_download_assets: "+e),Xe(1,e),e})}catch(e){throw ke.err("Error in mono_download_assets: "+e),e}}}let te=!1;function ne(){if(te)return;te=!0;const e=ke.config,o=[];if(e.assets)for(const o of e.assets)"object"!=typeof o&&We(!1,`asset must be object, it was ${typeof o} : ${o}`),"string"!=typeof o.behavior&&We(!1,"asset behavior must be known string"),"string"!=typeof o.name&&We(!1,"asset name must be string"),o.resolvedUrl&&"string"!=typeof o.resolvedUrl&&We(!1,"asset resolvedUrl could be string"),o.hash&&"string"!=typeof o.hash&&We(!1,"asset resolvedUrl could be string"),o.pendingDownload&&"object"!=typeof o.pendingDownload&&We(!1,"asset pendingDownload could be object"),o.isCore?L.push(o):N.push(o),K(o);else if(e.resources){const t=e.resources;t.wasmNative||We(!1,"resources.wasmNative must be defined"),t.jsModuleNative||We(!1,"resources.jsModuleNative must be defined"),t.jsModuleRuntime||We(!1,"resources.jsModuleRuntime must be defined"),Z(N,t.wasmNative,"dotnetwasm"),Z(o,t.jsModuleNative,"js-module-native"),Z(o,t.jsModuleRuntime,"js-module-runtime"),t.jsModuleDiagnostics&&Z(o,t.jsModuleDiagnostics,"js-module-diagnostics");const n=(e,o,t)=>{const n=e;n.behavior=o,t?(n.isCore=!0,L.push(n)):N.push(n)};if(t.coreAssembly)for(let e=0;e<t.coreAssembly.length;e++)n(t.coreAssembly[e],"assembly",!0);if(t.assembly)for(let e=0;e<t.assembly.length;e++)n(t.assembly[e],"assembly",!t.coreAssembly);if(0!=e.debugLevel&&ke.isDebuggingSupported()){if(t.corePdb)for(let e=0;e<t.corePdb.length;e++)n(t.corePdb[e],"pdb",!0);if(t.pdb)for(let e=0;e<t.pdb.length;e++)n(t.pdb[e],"pdb",!t.corePdb)}if(e.loadAllSatelliteResources&&t.satelliteResources)for(const e in t.satelliteResources)for(let o=0;o<t.satelliteResources[e].length;o++){const r=t.satelliteResources[e][o];r.culture=e,n(r,"resource",!t.coreAssembly)}if(t.coreVfs)for(let e=0;e<t.coreVfs.length;e++)n(t.coreVfs[e],"vfs",!0);if(t.vfs)for(let e=0;e<t.vfs.length;e++)n(t.vfs[e],"vfs",!t.coreVfs);const r=C(e);if(r&&t.icu)for(let e=0;e<t.icu.length;e++){const o=t.icu[e];o.name===r&&n(o,"icu",!1)}if(t.wasmSymbols)for(let e=0;e<t.wasmSymbols.length;e++)n(t.wasmSymbols[e],"symbols",!1)}if(e.appsettings)for(let o=0;o<e.appsettings.length;o++){const t=e.appsettings[o],n=pe(t);"appsettings.json"!==n&&n!==`appsettings.${e.applicationEnvironment}.json`||N.push({name:t,behavior:"vfs",cache:"no-cache",useCredentials:!0})}e.assets=[...L,...N,...o]}async function re(e){const o=await se(e);return await o.pendingDownloadInternal.response,o.buffer}async function se(e){try{return await ie(e)}catch(o){if(!ke.enableDownloadRetry)throw o;if(Se||Ee)throw o;if(e.pendingDownload&&e.pendingDownloadInternal==e.pendingDownload)throw o;if(e.resolvedUrl&&-1!=e.resolvedUrl.indexOf("file://"))throw o;if(o&&404==o.status)throw o;e.pendingDownloadInternal=void 0,await ke.allDownloadsQueued.promise;try{return ke.diagnosticTracing&&b(`Retrying download '${e.name}'`),await ie(e)}catch(o){return e.pendingDownloadInternal=void 0,await new Promise(e=>globalThis.setTimeout(e,100)),ke.diagnosticTracing&&b(`Retrying download (2) '${e.name}' after delay`),await ie(e)}}}async function ie(e){for(;O;)await O.promise;try{++$,$==ke.maxParallelDownloads&&(ke.diagnosticTracing&&b("Throttling further parallel downloads"),O=r());const o=await async function(e){if(e.pendingDownload&&(e.pendingDownloadInternal=e.pendingDownload),e.pendingDownloadInternal&&e.pendingDownloadInternal.response)return e.pendingDownloadInternal.response;if(e.buffer){const o=await e.buffer;return e.resolvedUrl||(e.resolvedUrl="undefined://"+e.name),e.pendingDownloadInternal={url:e.resolvedUrl,name:e.name,response:Promise.resolve({ok:!0,arrayBuffer:()=>o,json:()=>JSON.parse(new TextDecoder("utf-8").decode(o)),text:()=>new TextDecoder("utf-8").decode(o),headers:{get:()=>{}}})},e.pendingDownloadInternal.response}const o=e.loadRemote&&ke.config.remoteSources?ke.config.remoteSources:[""];let t;for(let n of o){n=n.trim(),"./"===n&&(n="");const o=ae(e,n);e.name===o?ke.diagnosticTracing&&b(`Attempting to download '${o}'`):ke.diagnosticTracing&&b(`Attempting to download '${o}' for ${e.name}`);try{e.resolvedUrl=o;const n=ue(e);if(e.pendingDownloadInternal=n,t=await n.response,!t||!t.ok)continue;return t}catch(e){t||(t={ok:!1,url:o,status:0,statusText:""+e});continue}}const n=e.isOptional||e.name.match(/\.pdb$/)&&ke.config.ignorePdbLoadErrors;if(t||We(!1,`Response undefined ${e.name}`),!n){const o=new Error(`download '${t.url}' for ${e.name} failed ${t.status} ${t.statusText}`);throw o.status=t.status,o}w(`optional download '${t.url}' for ${e.name} failed ${t.status} ${t.statusText}`)}(e);return o?(H[e.behavior]||(e.buffer=await o.arrayBuffer(),++ke.actual_downloaded_assets_count),e):e}finally{if(--$,O&&$==ke.maxParallelDownloads-1){ke.diagnosticTracing&&b("Resuming more parallel downloads");const e=O;O=void 0,e.promise_control.resolve()}}}function ae(e,o){let t;return null==o&&We(!1,`sourcePrefix must be provided for ${e.name}`),e.resolvedUrl?t=e.resolvedUrl:(t=""===o?"assembly"===e.behavior||"pdb"===e.behavior?e.name:"resource"===e.behavior&&e.culture&&""!==e.culture?`${e.culture}/${e.name}`:e.name:o+e.name,t=le(ke.locateFile(t),e.behavior)),t&&"string"==typeof t||We(!1,"attemptUrl need to be path or url string"),t}function le(e,o){return ke.modulesUniqueQuery&&B[o]&&(e+=ke.modulesUniqueQuery),e}let ce=0;const de=new Set;function ue(e){try{e.resolvedUrl||We(!1,"Request's resolvedUrl must be set");const o=function(e){let o=e.resolvedUrl;if(ke.loadBootResource){const t=me(e);if(t instanceof Promise)return t;"string"==typeof t&&(o=t)}const t={};return e.cache?t.cache=e.cache:ke.config.disableNoCacheFetch||(t.cache="no-cache"),e.useCredentials?t.credentials="include":!ke.config.disableIntegrityCheck&&e.hash&&(t.integrity=e.hash),ke.fetch_like(o,t)}(e),t={name:e.name,url:e.resolvedUrl,response:o};return de.add(e.name),t.response.then(()=>{"assembly"==e.behavior&&ke.loadedAssemblies.push(e.name),ce++,ke.onDownloadResourceProgress&&ke.onDownloadResourceProgress(ce,de.size)}),t}catch(o){const t={ok:!1,url:e.resolvedUrl,status:500,statusText:"ERR29: "+o,arrayBuffer:()=>{throw o},json:()=>{throw o}};return{name:e.name,url:e.resolvedUrl,response:Promise.resolve(t)}}}const fe={resource:"assembly",assembly:"assembly",pdb:"pdb",icu:"globalization",vfs:"configuration",manifest:"manifest",dotnetwasm:"dotnetwasm","js-module-dotnet":"dotnetjs","js-module-native":"dotnetjs","js-module-runtime":"dotnetjs"};function me(e){var o;if(ke.loadBootResource){const t=null!==(o=e.hash)&&void 0!==o?o:"",n=e.resolvedUrl,r=fe[e.behavior];if(r){const o=ke.loadBootResource(r,e.name,n,t,e.behavior);return"string"==typeof o?function(e){return"string"!=typeof e&&We(!1,"url must be a string"),!I(e)&&0!==e.indexOf("./")&&0!==e.indexOf("../")&&globalThis.URL&&globalThis.document&&globalThis.document.baseURI&&(e=new URL(e,globalThis.document.baseURI).toString()),e}(o):o}}}function ge(e){e.pendingDownloadInternal=null,e.pendingDownload=null,e.buffer=null,e.moduleExports=null}function pe(e){let o=e.lastIndexOf("/");return o>=0&&o++,e.substring(o)}async function he(e){e&&await Promise.all((null!=e?e:[]).map(e=>async function(e){try{const o=e.name;if(!e.moduleExports){const t=le(ke.locateFile(o),"js-module-library-initializer");ke.diagnosticTracing&&b(`Attempting to import '${t}' for ${e}`),e.moduleExports=await import(/*! webpackIgnore: true */t)}ke.libraryInitializers.push({scriptName:o,exports:e.moduleExports})}catch(o){v(`Failed to import library initializer '${e}': ${o}`)}}(e)))}async function be(e,o){if(!ke.libraryInitializers)return;const t=[];for(let n=0;n<ke.libraryInitializers.length;n++){const r=ke.libraryInitializers[n];r.exports[e]&&t.push(we(r.scriptName,e,()=>r.exports[e](...o)))}await Promise.all(t)}async function we(e,o,t){try{await t()}catch(t){throw v(`Failed to invoke '${o}' on library initializer '${e}': ${t}`),Xe(1,t),t}}function ye(e,o){if(e===o)return e;const t={...o};return void 0!==t.assets&&t.assets!==e.assets&&(t.assets=[...e.assets||[],...t.assets||[]]),void 0!==t.resources&&(t.resources=_e(e.resources||{assembly:[],jsModuleNative:[],jsModuleRuntime:[],wasmNative:[]},t.resources)),void 0!==t.environmentVariables&&(t.environmentVariables={...e.environmentVariables||{},...t.environmentVariables||{}}),void 0!==t.runtimeOptions&&t.runtimeOptions!==e.runtimeOptions&&(t.runtimeOptions=[...e.runtimeOptions||[],...t.runtimeOptions||[]]),Object.assign(e,t)}function ve(e,o){if(e===o)return e;const t={...o};return t.config&&(e.config||(e.config={}),t.config=ye(e.config,t.config)),Object.assign(e,t)}function _e(e,o){if(e===o)return e;const t={...o};return void 0!==t.coreAssembly&&(t.coreAssembly=[...e.coreAssembly||[],...t.coreAssembly||[]]),void 0!==t.assembly&&(t.assembly=[...e.assembly||[],...t.assembly||[]]),void 0!==t.lazyAssembly&&(t.lazyAssembly=[...e.lazyAssembly||[],...t.lazyAssembly||[]]),void 0!==t.corePdb&&(t.corePdb=[...e.corePdb||[],...t.corePdb||[]]),void 0!==t.pdb&&(t.pdb=[...e.pdb||[],...t.pdb||[]]),void 0!==t.jsModuleNative&&(t.jsModuleNative=[...e.jsModuleNative||[],...t.jsModuleNative||[]]),void 0!==t.jsModuleDiagnostics&&(t.jsModuleDiagnostics=[...e.jsModuleDiagnostics||[],...t.jsModuleDiagnostics||[]]),void 0!==t.jsModuleRuntime&&(t.jsModuleRuntime=[...e.jsModuleRuntime||[],...t.jsModuleRuntime||[]]),void 0!==t.wasmSymbols&&(t.wasmSymbols=[...e.wasmSymbols||[],...t.wasmSymbols||[]]),void 0!==t.wasmNative&&(t.wasmNative=[...e.wasmNative||[],...t.wasmNative||[]]),void 0!==t.icu&&(t.icu=[...e.icu||[],...t.icu||[]]),void 0!==t.satelliteResources&&(t.satelliteResources=function(e,o){if(e===o)return e;for(const t in o)e[t]=[...e[t]||[],...o[t]||[]];return e}(e.satelliteResources||{},t.satelliteResources||{})),void 0!==t.modulesAfterConfigLoaded&&(t.modulesAfterConfigLoaded=[...e.modulesAfterConfigLoaded||[],...t.modulesAfterConfigLoaded||[]]),void 0!==t.modulesAfterRuntimeReady&&(t.modulesAfterRuntimeReady=[...e.modulesAfterRuntimeReady||[],...t.modulesAfterRuntimeReady||[]]),void 0!==t.extensions&&(t.extensions={...e.extensions||{},...t.extensions||{}}),void 0!==t.vfs&&(t.vfs=[...e.vfs||[],...t.vfs||[]]),Object.assign(e,t)}function Ae(){const e=ke.config;if(e.environmentVariables=e.environmentVariables||{},e.runtimeOptions=e.runtimeOptions||[],e.resources=e.resources||{assembly:[],jsModuleNative:[],jsModuleRuntime:[],wasmNative:[],vfs:[],satelliteResources:{}},e.assets){ke.diagnosticTracing&&b("config.assets is deprecated, use config.resources instead");for(const o of e.assets){const t={};switch(o.behavior){case"assembly":t.assembly=[o];break;case"pdb":t.pdb=[o];break;case"resource":t.satelliteResources={},t.satelliteResources[o.culture]=[o];break;case"icu":t.icu=[o];break;case"symbols":t.wasmSymbols=[o];break;case"vfs":t.vfs=[o];break;case"dotnetwasm":t.wasmNative=[o];break;case"js-module-runtime":t.jsModuleRuntime=[o];break;case"js-module-native":t.jsModuleNative=[o];break;case"js-module-diagnostics":t.jsModuleDiagnostics=[o];break;case"js-module-dotnet":break;default:throw new Error(`Unexpected behavior ${o.behavior} of asset ${o.name}`)}_e(e.resources,t)}}e.debugLevel,void 0===e.virtualWorkingDirectory&&(e.virtualWorkingDirectory=Ue),e.applicationEnvironment||(e.applicationEnvironment="Production"),e.applicationCulture&&(e.environmentVariables.LANG=`${e.applicationCulture}.UTF-8`),Pe.diagnosticTracing=ke.diagnosticTracing=!!e.diagnosticTracing,Pe.waitForDebugger=e.waitForDebugger,ke.maxParallelDownloads=e.maxParallelDownloads||ke.maxParallelDownloads,ke.enableDownloadRetry=void 0!==e.enableDownloadRetry?e.enableDownloadRetry:ke.enableDownloadRetry}let xe=!1;async function Te(e){var o;if(xe)await ke.afterConfigLoaded.promise;else try{if(xe=!0,Ae(),await he(null===(o=ke.config.resources)||void 0===o?void 0:o.modulesAfterConfigLoaded),await be("onRuntimeConfigLoaded",[ke.config]),e.onConfigLoaded)try{await e.onConfigLoaded(ke.config,Oe),Ae()}catch(e){throw _("onConfigLoaded() failed",e),e}Ae(),ke.afterConfigLoaded.promise_control.resolve(ke.config)}catch(o){const t=`Failed to initialize config ${o} ${null==o?void 0:o.stack}`;throw ke.config=e.config=Object.assign(ke.config,{message:t,error:o,isError:!0}),Xe(1,new Error(t)),o}}function Re(){return!!globalThis.navigator&&(ke.isChromium||ke.isFirefox)}"function"==typeof importScripts&&(globalThis.dotnetSidecar=!0);const Ee="object"==typeof process&&"object"==typeof process.versions&&"string"==typeof process.versions.node,je="function"==typeof importScripts,De=je&&"undefined"!=typeof dotnetSidecar,Ce=je&&!De,Me="object"==typeof window||je&&!Ee,Se=!Me&&!Ee,Ue="/";let Pe={},ke={},Ie={},Oe={},$e={},Le=!1;const Ne={},ze={config:Ne},Fe={mono:{},binding:{},internal:$e,module:ze,loaderHelpers:ke,runtimeHelpers:Pe,diagnosticHelpers:Ie,api:Oe};function We(e,o){if(e)return;const t="Assert failed: "+("function"==typeof o?o():o),n=new Error(t);_(t,n),Pe.nativeAbort(n)}function Ve(){return void 0!==ke.exitCode}function Be(){return Pe.runtimeReady&&!Ve()}function Je(){Ve()&&We(!1,`.NET runtime already exited with ${ke.exitCode} ${ke.exitReason}. You can use dotnet.runMain() which doesn't exit the runtime.`),Pe.runtimeReady||We(!1,".NET runtime didn't start yet. Please call dotnet.create() first.")}function He(){Me&&(globalThis.addEventListener("unhandledrejection",eo),globalThis.addEventListener("error",oo))}let Qe,qe;function Ge(){Qe=ze.onAbort,qe=ze.onExit,ze.onAbort=Ke,ze.onExit=Ze}function Ze(e){qe&&qe(e),Xe(e,ke.exitReason)}function Ke(e){Qe&&Qe(e||ke.exitReason),Xe(1,e||ke.exitReason)}function Xe(e,o){var t;const n=o&&"object"==typeof o;e=n&&"number"==typeof o.status?o.status:void 0===e?-1:e;const r=n&&"string"==typeof o.message?o.message:""+o;(o=n?o:Pe.ExitStatus?function(e,o){const t=new Pe.ExitStatus(e);return t.message=o,t.toString=()=>o,t}(e,r):new Error("Exit with code "+e+" "+r)).status=e,o.message||(o.message=r);const s=""+(o.stack||(new Error).stack);try{Object.defineProperty(o,"stack",{get:()=>s})}catch(e){}const i=!!o.silent;if(o.silent=!0,Ve())ke.diagnosticTracing&&b("mono_exit called after exit");else{try{ze.onAbort==Ke&&(ze.onAbort=Qe),ze.onExit==Ze&&(ze.onExit=qe),Me&&(globalThis.removeEventListener("unhandledrejection",eo),globalThis.removeEventListener("error",oo)),Pe.runtimeReady?(Pe.jiterpreter_dump_stats&&Pe.jiterpreter_dump_stats(!1),0===e&&(null===(t=ke.config)||void 0===t?void 0:t.interopCleanupOnExit)&&Pe.forceDisposeProxies(!0,!0)):(ke.diagnosticTracing&&b(`abort_startup, reason: ${o}`),function(e){ke.allDownloadsQueued.promise_control.reject(e),ke.allDownloadsFinished.promise_control.reject(e),ke.afterConfigLoaded.promise_control.reject(e),ke.wasmCompilePromise.promise_control.reject(e),ke.runtimeModuleLoaded.promise_control.reject(e),Pe.dotnetReady&&(Pe.dotnetReady.promise_control.reject(e),Pe.afterInstantiateWasm.promise_control.reject(e),Pe.afterPreRun.promise_control.reject(e),Pe.beforeOnRuntimeInitialized.promise_control.reject(e),Pe.afterOnRuntimeInitialized.promise_control.reject(e),Pe.afterPostRun.promise_control.reject(e))}(o))}catch(e){v("mono_exit A failed",e)}try{i||(function(e,o){if(0!==e&&o){const e=Pe.ExitStatus&&o instanceof Pe.ExitStatus?b:_;"string"==typeof o?e(o):(void 0===o.stack&&(o.stack=(new Error).stack+""),o.message?e(Pe.stringify_as_error_with_stack?Pe.stringify_as_error_with_stack(o.message+"\n"+o.stack):o.message+"\n"+o.stack):e(JSON.stringify(o)))}!Ce&&ke.config&&(ke.config.logExitCode?ke.config.forwardConsole?T("WASM EXIT "+e):y("WASM EXIT "+e):ke.config.forwardConsole&&T())}(e,o),function(e){if(Me&&!Ce&&ke.config&&ke.config.appendElementOnExit&&document){const o=document.createElement("label");o.id="tests_done",0!==e&&(o.style.background="red"),o.innerHTML=""+e,document.body.appendChild(o)}}(e))}catch(e){v("mono_exit B failed",e)}ke.exitCode=e,ke.exitReason||(ke.exitReason=o),!Ce&&Pe.runtimeReady&&ze.runtimeKeepalivePop()}if(ke.config&&ke.config.asyncFlushOnExit&&0===e)throw(async()=>{try{await async function(){if(Ee)try{const e=await import(/*! webpackIgnore: true */"process"),o=e=>new Promise((o,t)=>{e.on("error",t),e.end("","utf8",o)}),t=o(e.stderr),n=o(e.stdout);let r;const s=new Promise(e=>{r=setTimeout(()=>e("timeout"),1e3)});await Promise.race([Promise.all([n,t]),s]),clearTimeout(r)}catch(e){_(`flushing std* streams failed: ${e}`)}}()}finally{Ye(e,o)}})(),o;Ye(e,o)}function Ye(e,o){if(Pe.runtimeReady&&Pe.nativeExit)try{Pe.nativeExit(e)}catch(e){!Pe.ExitStatus||e instanceof Pe.ExitStatus||v("set_exit_code_and_quit_now failed: "+e.toString())}if(0!==e||!Me)throw Ee?process.exit(e):Pe.quit&&Pe.quit(e,o),o}function eo(e){to(e,e.reason,"rejection")}function oo(e){to(e,e.error,"error")}function to(e,o,t){e.preventDefault();try{o||(o=new Error("Unhandled "+t)),void 0===o.stack&&(o.stack=(new Error).stack),o.stack=o.stack+"",o.silent||(_("Unhandled error:",o),Xe(1,o))}catch(e){}}!function(n){if(Le)throw new Error("Loader module already loaded");Le=!0,Pe=n.runtimeHelpers,ke=n.loaderHelpers,Ie=n.diagnosticHelpers,Oe=n.api,$e=n.internal,Object.assign(Oe,{INTERNAL:$e,invokeLibraryInitializers:be}),Object.assign(n.module,{config:ye(Ne,{environmentVariables:{}})});const a={mono_wasm_bindings_is_ready:!1,config:n.module.config,diagnosticTracing:!1,nativeAbort:e=>{throw e||new Error("abort")},nativeExit:e=>{throw new Error("exit:"+e)}},l={gitHash:"820230c386db5b0106c5430b3e5aee92a7826bf5",config:n.module.config,diagnosticTracing:!1,maxParallelDownloads:16,enableDownloadRetry:!0,_loaded_files:[],loadedFiles:[],loadedAssemblies:[],libraryInitializers:[],workerNextNumber:1,actual_downloaded_assets_count:0,actual_instantiated_assets_count:0,expected_downloaded_assets_count:0,expected_instantiated_assets_count:0,afterConfigLoaded:r(),allDownloadsQueued:r(),allDownloadsFinished:r(),wasmCompilePromise:r(),runtimeModuleLoaded:r(),loadingWorkers:r(),is_exited:Ve,is_runtime_running:Be,assert_runtime_running:Je,mono_exit:Xe,createPromiseController:r,getPromiseController:s,assertIsControllablePromise:i,mono_download_assets:oe,resolve_single_asset_path:Y,setup_proxy_console:x,set_thread_prefix:h,installUnhandledErrorHandler:He,retrieve_asset_download:re,invokeLibraryInitializers:be,isDebuggingSupported:Re,exceptionsFinal:e,simd:t,relaxedSimd:o};Object.assign(Pe,a),Object.assign(ke,l)}(Fe);let no,ro,so,io=!1,ao=!1;async function lo(e){if(!ao){if(ao=!0,Me&&ke.config.forwardConsole&&void 0!==globalThis.WebSocket&&x("main",globalThis.console,globalThis.location.origin),ze||We(!1,"Null moduleConfig"),ke.config||We(!1,"Null moduleConfig.config"),"function"==typeof e){const o=e(Fe.api);if(o.ready)throw new Error("Module.ready couldn't be redefined.");Object.assign(ze,o),ve(ze,o)}else{if("object"!=typeof e)throw new Error("Can't use moduleFactory callback of createDotnetRuntime function.");ve(ze,e)}await S(ze)}}async function co(e){return await lo(e),ke.config.exitOnUnhandledError&&He(),Ge(),async function(){var e;await Te(ze),ne();const o=uo();(async function(){try{const e=Y("dotnetwasm");await se(e),e&&e.pendingDownloadInternal&&e.pendingDownloadInternal.response||We(!1,"Can't load dotnet.native.wasm");const o=await e.pendingDownloadInternal.response,t=o.headers&&o.headers.get?o.headers.get("Content-Type"):void 0;let n;if("function"==typeof WebAssembly.compileStreaming&&"application/wasm"===t)n=await WebAssembly.compileStreaming(o);else{Me&&"application/wasm"!==t&&v('WebAssembly resource does not have the expected content type "application/wasm", so falling back to slower ArrayBuffer instantiation.');const e=await o.arrayBuffer();ke.diagnosticTracing&&b("instantiate_wasm_module buffered"),n=Se?await Promise.resolve(new WebAssembly.Module(e)):await WebAssembly.compile(e)}e.pendingDownloadInternal=null,e.pendingDownload=null,e.buffer=null,e.moduleExports=null,ke.wasmCompilePromise.promise_control.resolve(n)}catch(e){ke.wasmCompilePromise.promise_control.reject(e)}})(),setTimeout(async()=>{try{D(),await oe()}catch(e){Xe(1,e)}},0);const t=await Promise.all(o);return await fo(t),await Pe.dotnetReady.promise,await he(null===(e=ke.config.resources)||void 0===e?void 0:e.modulesAfterRuntimeReady),await be("onRuntimeReady",[Fe.api]),Oe}()}function uo(){const e=Y("js-module-runtime"),o=Y("js-module-native");if(no&&ro)return[no,ro,so];"object"==typeof e.moduleExports?no=e.moduleExports:(ke.diagnosticTracing&&b(`Attempting to import '${e.resolvedUrl}' for ${e.name}`),no=import(/*! webpackIgnore: true */e.resolvedUrl)),"object"==typeof o.moduleExports?ro=o.moduleExports:(ke.diagnosticTracing&&b(`Attempting to import '${o.resolvedUrl}' for ${o.name}`),ro=import(/*! webpackIgnore: true */o.resolvedUrl));const t=X("js-module-diagnostics");return t&&("object"==typeof t.moduleExports?so=t.moduleExports:(ke.diagnosticTracing&&b(`Attempting to import '${t.resolvedUrl}' for ${t.name}`),so=import(/*! webpackIgnore: true */t.resolvedUrl))),[no,ro,so]}async function fo(e){const{initializeExports:o,initializeReplacements:t,configureRuntimeStartup:n,configureEmscriptenStartup:r,configureWorkerStartup:s,setRuntimeGlobals:i,passEmscriptenInternals:a}=e[0],{default:l}=e[1],c=e[2];i(Fe),o(Fe),c&&c.setRuntimeGlobals(Fe),await n(ze),ke.runtimeModuleLoaded.promise_control.resolve(),l(()=>(Object.assign(ze,{__dotnet_runtime:{initializeReplacements:t,configureEmscriptenStartup:r,configureWorkerStartup:s,passEmscriptenInternals:a}}),ze)).catch(e=>{if(e.message&&e.message.toLowerCase().includes("out of memory"))throw new Error(".NET runtime has failed to start, because too much memory was requested. Please decrease the memory by adjusting EmccMaximumHeapSize.");throw e})}Ce&&async function(){(function(){const e=new MessageChannel,o=e.port1,t=e.port2;o.addEventListener("message",e=>{!function(e){const o=JSON.parse(e.config),t=JSON.parse(e.monoThreadInfo);ze.config=o,ze.wasmModule=e.wasmModule,ze.wasmMemory=e.wasmMemory,ze.handlers=e.handlers,io?ke.diagnosticTracing&&b("mono config already received"):(ye(ke.config,o),Pe.monoThreadInfo=t,Ae(),ke.diagnosticTracing&&b("mono config received"),io=!0,ke.afterConfigLoaded.promise_control.resolve(ke.config),Me&&o.forwardConsole&&void 0!==globalThis.WebSocket&&ke.setup_proxy_console("worker-idle",console,globalThis.location.origin))}(e.data),o.close(),t.close()},{once:!0}),o.start(),self.postMessage({[a]:{monoCmd:"preload",port:t}},[t])})(),await ke.afterConfigLoaded.promise,function(){const e=ke.config;e.assets||We(!1,"config.assets must be defined");for(const o of e.assets)K(o),q[o.behavior]&&N.push(o)}();const e=uo(),o=await Promise.all(e);return globalThis.name="em-pthread",await fo(o),ke.config.exitOnUnhandledError&&He(),Ge(),Me&&ke.config.forwardConsole&&void 0!==globalThis.WebSocket&&x("main",globalThis.console,globalThis.location.origin),await S(ze),await oe(),self.dispatchEvent(new MessageEvent("message",{data:{cmd:1,handlers:ze.handlers,wasmMemory:ze.wasmMemory,wasmModule:ze.wasmModule}})),ze}().catch(e=>Xe(1,e));const mo=new class{withModuleConfig(e){try{return ve(ze,e),this}catch(e){throw Xe(1,e),e}}withInterpreterPgo(e,o){try{return ye(Ne,{interpreterPgo:e,interpreterPgoSaveDelay:o}),Ne.runtimeOptions?Ne.runtimeOptions.push("--interp-pgo-recording"):Ne.runtimeOptions=["--interp-pgo-recording"],this}catch(e){throw Xe(1,e),e}}withConfig(e){try{return ye(Ne,e),this}catch(e){throw Xe(1,e),e}}withConfigSrc(e){return this}withVirtualWorkingDirectory(e){try{return e&&"string"==typeof e||We(!1,"must be directory path"),ye(Ne,{virtualWorkingDirectory:e}),this}catch(e){throw Xe(1,e),e}}withEnvironmentVariable(e,o){try{const t={};return t[e]=o,ye(Ne,{environmentVariables:t}),this}catch(e){throw Xe(1,e),e}}withEnvironmentVariables(e){try{return e&&"object"==typeof e||We(!1,"must be dictionary object"),ye(Ne,{environmentVariables:e}),this}catch(e){throw Xe(1,e),e}}withDiagnosticTracing(e){try{return"boolean"!=typeof e&&We(!1,"must be boolean"),ye(Ne,{diagnosticTracing:e}),this}catch(e){throw Xe(1,e),e}}withDebugging(e){try{return null!=e&&"number"==typeof e||We(!1,"must be number"),ye(Ne,{debugLevel:e}),this}catch(e){throw Xe(1,e),e}}withApplicationArguments(...e){try{return e&&Array.isArray(e)||We(!1,"must be array of strings"),ye(Ne,{applicationArguments:e}),this}catch(e){throw Xe(1,e),e}}withRuntimeOptions(e){try{return e&&Array.isArray(e)||We(!1,"must be array of strings"),Ne.runtimeOptions?Ne.runtimeOptions.push(...e):Ne.runtimeOptions=e,this}catch(e){throw Xe(1,e),e}}withMainAssembly(e){try{return ye(Ne,{mainAssemblyName:e}),this}catch(e){throw Xe(1,e),e}}withApplicationArgumentsFromQuery(){try{if(!globalThis.window)throw new Error("Missing window to the query parameters from");if(void 0===globalThis.URLSearchParams)throw new Error("URLSearchParams is supported");const e=new URLSearchParams(globalThis.window.location.search).getAll("arg");return this.withApplicationArguments(...e)}catch(e){throw Xe(1,e),e}}withApplicationEnvironment(e){try{return ye(Ne,{applicationEnvironment:e}),this}catch(e){throw Xe(1,e),e}}withApplicationCulture(e){try{return ye(Ne,{applicationCulture:e}),this}catch(e){throw Xe(1,e),e}}withResourceLoader(e){try{return ke.loadBootResource=e,this}catch(e){throw Xe(1,e),e}}async download(){try{await async function(){lo(ze),await Te(ze),ne(),D(),oe(),await ke.allDownloadsFinished.promise}()}catch(e){throw Xe(1,e),e}}async create(){try{return this.instance||(this.instance=await async function(){return await co(ze),Fe.api}()),this.instance}catch(e){throw Xe(1,e),e}}run(){return this.runMainAndExit()}async runMainAndExit(){try{return ze.config||We(!1,"Null moduleConfig.config"),this.instance||await this.create(),this.instance.runMainAndExit()}catch(e){throw Xe(1,e),e}}async runMain(){try{return ze.config||We(!1,"Null moduleConfig.config"),this.instance||await this.create(),this.instance.runMain()}catch(e){throw Xe(1,e),e}}},go=Xe,po=co;Se||"function"==typeof globalThis.URL||We(!1,"This browser/engine doesn't support URL API. Please use a modern version."),"function"!=typeof globalThis.BigInt64Array&&We(!1,"This browser/engine doesn't support BigInt64Array API. Please use a modern version. See also https://learn.microsoft.com/aspnet/core/blazor/supported-platforms"),globalThis.performance&&"function"==typeof globalThis.performance.now||We(!1,"This browser/engine doesn't support performance.now. Please use a modern version."),Se||globalThis.crypto&&"object"==typeof globalThis.crypto.subtle||We(!1,"This engine doesn't support crypto.subtle. Please use a modern version."),Se||globalThis.crypto&&"function"==typeof globalThis.crypto.getRandomValues||We(!1,"This engine doesn't support crypto.getRandomValues. Please use a modern version."),Ee&&"function"!=typeof process.exit&&We(!1,"This engine doesn't support process.exit. Please use a modern version."),mo.withConfig(/*json-start*/{
  "mainAssemblyName": "MyBlazorWasmApp1.Stories",
  "resources": {
    "hash": "sha256-mA2TQ8sY0ISCTpO49GUOzI9vlSNXbTV9YrBNh0KSHNg=",
    "jsModuleNative": [
      {
        "name": "dotnet.native.2yixleluw6.js"
      }
    ],
    "jsModuleRuntime": [
      {
        "name": "dotnet.runtime.okxdo5zwwj.js"
      }
    ],
    "wasmNative": [
      {
        "name": "dotnet.native.vzl365gfs0.wasm",
        "hash": "sha256-G0w0HnmOUQxd0GhQL9RG5duRzmhyFYbux95Bosv5iWg=",
        "cache": "force-cache"
      }
    ],
    "coreAssembly": [
      {
        "payloadSize": 80464,
        "virtualPath": "System.Runtime.InteropServices.JavaScript.wasm",
        "name": "System.Runtime.InteropServices.JavaScript.l7pvrvdnz0.wasm",
        "hash": "sha256-nwctoM19aMT73mduW6hkoGuUXVizKgni7iJi9xa8LWc=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5412432,
        "virtualPath": "System.Private.CoreLib.wasm",
        "name": "System.Private.CoreLib.b9r07chuqp.wasm",
        "hash": "sha256-Ac9iRF3iaPOcJC5tRKVcVNZ7WnHZgGUAe/dsbMlbcsA=",
        "cache": "force-cache"
      }
    ],
    "assembly": [
      {
        "payloadSize": 47184,
        "virtualPath": "Microsoft.AspNetCore.Authorization.wasm",
        "name": "Microsoft.AspNetCore.Authorization.l0ekhxhi5u.wasm",
        "hash": "sha256-CxfhRFYwDgzJZaQOk4auLR6fNv/srWydZVNxWlNTdI4=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 428112,
        "virtualPath": "Microsoft.AspNetCore.Components.wasm",
        "name": "Microsoft.AspNetCore.Components.6t4elacgy5.wasm",
        "hash": "sha256-2mzdf2JRv2cOTHkDzmImc7w2aok67wkq6Z4qeUXVx54=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 51280,
        "virtualPath": "Microsoft.AspNetCore.Components.Forms.wasm",
        "name": "Microsoft.AspNetCore.Components.Forms.cvjmjoj7xe.wasm",
        "hash": "sha256-6K7ey42Y/NuP/ywGCC1U+T3SN+tcVeUXa/vgdQ2Myw0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 216144,
        "virtualPath": "Microsoft.AspNetCore.Components.Web.wasm",
        "name": "Microsoft.AspNetCore.Components.Web.zc5c7rzerk.wasm",
        "hash": "sha256-IwltkaOi98AgZDu2QHKq6cj2iulrAZBbPXql6w4CCq0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 190032,
        "virtualPath": "Microsoft.AspNetCore.Components.WebAssembly.wasm",
        "name": "Microsoft.AspNetCore.Components.WebAssembly.49d2fd8d65.wasm",
        "hash": "sha256-mDKY+4xcc3Y1gWTv2rwoz/cxDsIevvOslVSDozxwd6w=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "Microsoft.AspNetCore.Metadata.wasm",
        "name": "Microsoft.AspNetCore.Metadata.cigzhxj1us.wasm",
        "hash": "sha256-rHz50sfwK2Wa3bPqFPtXtlKUamAN3Stc1xvUp1sEJrk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 34384,
        "virtualPath": "Microsoft.Extensions.Configuration.wasm",
        "name": "Microsoft.Extensions.Configuration.k7k5kyet84.wasm",
        "hash": "sha256-kHEm+9WVpbcwB4Tx5WsWPJadAbN7ZY7LzK59F7FKWKw=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 32848,
        "virtualPath": "Microsoft.Extensions.Configuration.Binder.wasm",
        "name": "Microsoft.Extensions.Configuration.Binder.ivm7tnp4x7.wasm",
        "hash": "sha256-jvhvJW0bZStkoL/C/G2ldF3MRXxHF42Au2JjhVBWwcY=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 16976,
        "virtualPath": "Microsoft.Extensions.Configuration.EnvironmentVariables.wasm",
        "name": "Microsoft.Extensions.Configuration.EnvironmentVariables.eik7b7o18a.wasm",
        "hash": "sha256-loqBAyVlXH06oflivfLsj179KgFV2EGg/+s7Zwf5OVM=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 19024,
        "virtualPath": "Microsoft.Extensions.Configuration.FileExtensions.wasm",
        "name": "Microsoft.Extensions.Configuration.FileExtensions.29zynogj2w.wasm",
        "hash": "sha256-CoKIOD9dgkS93EzUgqShkxCyD33wvtYka/7ZXnu/J5M=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 16976,
        "virtualPath": "Microsoft.Extensions.Configuration.Json.wasm",
        "name": "Microsoft.Extensions.Configuration.Json.9q48bny06e.wasm",
        "hash": "sha256-8ZBCGm5tXdNrNeD54o/0Swwi4R17+2gN6eW1wn+Q7Ug=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 86608,
        "virtualPath": "Microsoft.Extensions.DependencyInjection.wasm",
        "name": "Microsoft.Extensions.DependencyInjection.ruudh05250.wasm",
        "hash": "sha256-jaFlMtj5+KzKBGLDrGSX2/DTi0WBNkPGTIIfWzZjLJE=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 37968,
        "virtualPath": "Microsoft.Extensions.Diagnostics.wasm",
        "name": "Microsoft.Extensions.Diagnostics.9zyvx4iyqd.wasm",
        "hash": "sha256-K0wfRqvtb62d03Nr0WzExVpsDXTuwy6WJza0EzIYz1E=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 40528,
        "virtualPath": "Microsoft.Extensions.FileProviders.Physical.wasm",
        "name": "Microsoft.Extensions.FileProviders.Physical.5jcxvkjyek.wasm",
        "hash": "sha256-sbxHaIoBcHID2d8pWcR5G5Q+oLaB34W45OU+eR1uRFE=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 36944,
        "virtualPath": "Microsoft.Extensions.FileSystemGlobbing.wasm",
        "name": "Microsoft.Extensions.FileSystemGlobbing.t1z690bj4f.wasm",
        "hash": "sha256-D/Dv/O3K+j/WNE/RcLlSD25kBWOKsrwDaou5xL7UQpk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 8784,
        "virtualPath": "Microsoft.Extensions.Localization.Abstractions.wasm",
        "name": "Microsoft.Extensions.Localization.Abstractions.w2yhgql6mw.wasm",
        "hash": "sha256-OLEMkFQcoNLc5nOVMkYLfIu7/LNsflqg6Tm44ziDE4A=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 41040,
        "virtualPath": "Microsoft.Extensions.Logging.wasm",
        "name": "Microsoft.Extensions.Logging.btd356o1b4.wasm",
        "hash": "sha256-Udoqo8yReUmMD/GDsv4ilQG8eB5+abyZ6D7K4mSj6Tw=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 11344,
        "virtualPath": "Microsoft.Extensions.Options.ConfigurationExtensions.wasm",
        "name": "Microsoft.Extensions.Options.ConfigurationExtensions.ebjcrfsc5o.wasm",
        "hash": "sha256-NrxdRm0H1ejy1jqTtW15CK876ZFVZWY+KMPjJcByHM8=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 12880,
        "virtualPath": "Microsoft.Extensions.Validation.wasm",
        "name": "Microsoft.Extensions.Validation.42lijbcxhm.wasm",
        "hash": "sha256-5MXCm0k5pP6jSrDpwt7ppdhuECh8onJ4UuZGXs46P0k=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 67664,
        "virtualPath": "Microsoft.JSInterop.wasm",
        "name": "Microsoft.JSInterop.o6wt475488.wasm",
        "hash": "sha256-MJiyE+Tm3ldy2HjPmC8kGk0A6wrignxAtXGvJO2fqPo=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 13904,
        "virtualPath": "Microsoft.JSInterop.WebAssembly.wasm",
        "name": "Microsoft.JSInterop.WebAssembly.big8zqb916.wasm",
        "hash": "sha256-bV+Vuym6gKujS5b+HMPiGhXUc185EO+1Ff4nOYuIjvM=",
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
        "name": "Microsoft.CSharp.0113whnsny.wasm",
        "hash": "sha256-9JuzTkdAfsQqy3qs2/+U0K+Wb3H5APBzirJ3m1ssX2k=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 32848,
        "virtualPath": "Microsoft.Extensions.Caching.Abstractions.wasm",
        "name": "Microsoft.Extensions.Caching.Abstractions.a8w3jqbgfn.wasm",
        "hash": "sha256-Fu8MfkQ6fLqzgBrkVrndKufWES07lRwNWWVmKr6WwjU=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 16464,
        "virtualPath": "Microsoft.Extensions.Configuration.Abstractions.wasm",
        "name": "Microsoft.Extensions.Configuration.Abstractions.edgpka7p2e.wasm",
        "hash": "sha256-IstEk+qfutoEimRNaQeawxWUzC/xAaa6KjSkolkKpD8=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 57424,
        "virtualPath": "Microsoft.Extensions.DependencyInjection.Abstractions.wasm",
        "name": "Microsoft.Extensions.DependencyInjection.Abstractions.6ovh5dmoh3.wasm",
        "hash": "sha256-amzaW0wBbJS6a/jITkIVzRJJTED386xD9tz0Nr64vqo=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 17488,
        "virtualPath": "Microsoft.Extensions.Diagnostics.Abstractions.wasm",
        "name": "Microsoft.Extensions.Diagnostics.Abstractions.39skr4hx82.wasm",
        "hash": "sha256-B6fzbfSGslEuTMui19yi8yF8RAxEzjbpmcrSs22qNOc=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 11344,
        "virtualPath": "Microsoft.Extensions.FileProviders.Abstractions.wasm",
        "name": "Microsoft.Extensions.FileProviders.Abstractions.nq2vkpspfs.wasm",
        "hash": "sha256-n7fp3xOUOEgloBgJyMbgLGzzIg2DQ2f9AzLI+dQ5n2Y=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 22096,
        "virtualPath": "Microsoft.Extensions.Hosting.Abstractions.wasm",
        "name": "Microsoft.Extensions.Hosting.Abstractions.9w4lydqw73.wasm",
        "hash": "sha256-CWITgjEyR+wBGO97ZEJeHKv7pOKY3Z1ykXjjHQHx70E=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 56400,
        "virtualPath": "Microsoft.Extensions.Logging.Abstractions.wasm",
        "name": "Microsoft.Extensions.Logging.Abstractions.853or60aet.wasm",
        "hash": "sha256-7HgVvTXxRmJp/PDa3GzJHyRSTpomkkBNpzmk0I/SPYo=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 97872,
        "virtualPath": "Microsoft.Extensions.Options.wasm",
        "name": "Microsoft.Extensions.Options.s3g8uhtp7b.wasm",
        "hash": "sha256-eCp4PGXU9JeHNfwVnYbcAZA80ehLEZAsKCFQAUAZLr4=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 35408,
        "virtualPath": "Microsoft.Extensions.Primitives.wasm",
        "name": "Microsoft.Extensions.Primitives.85oej4o4oa.wasm",
        "hash": "sha256-vHSOLMc2hGYPINSZYQ7wUBTvQBZcxQoY15/5jfH9zlk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 418384,
        "virtualPath": "Microsoft.VisualBasic.Core.wasm",
        "name": "Microsoft.VisualBasic.Core.e40bj8yer5.wasm",
        "hash": "sha256-onel6HjgY8rXhV97Gjw2l7DJZWEo4S6gk5YOBIHx7VI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6736,
        "virtualPath": "Microsoft.VisualBasic.wasm",
        "name": "Microsoft.VisualBasic.xurk4bca6u.wasm",
        "hash": "sha256-CZorTD2WegoveJRGVr0zlKAlQTUPHgEWgLt3ARK5W6A=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "Microsoft.Win32.Primitives.wasm",
        "name": "Microsoft.Win32.Primitives.ltxtcae1qa.wasm",
        "hash": "sha256-eWSm5quZU0lnzuERIrBcUcCMVQGs3RoTCepEyzlXwbE=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 22608,
        "virtualPath": "Microsoft.Win32.Registry.wasm",
        "name": "Microsoft.Win32.Registry.459y6xe3ha.wasm",
        "hash": "sha256-yOyHDWCILG1ehtSsoEPU0EL6I/MKc7S9QMQvCmcApIA=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.AppContext.wasm",
        "name": "System.AppContext.nbz8pm1vpy.wasm",
        "hash": "sha256-LfqUeHcFw5yqFEfDfpJDvBXukhFdZmoY4X+Q9kdxAwM=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Buffers.wasm",
        "name": "System.Buffers.g5yaot3sti.wasm",
        "hash": "sha256-nFgLV5UKohT9PMPPrzodaSdKhjoSswlxx9dg1RfSyvE=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 53328,
        "virtualPath": "System.Collections.Concurrent.wasm",
        "name": "System.Collections.Concurrent.fw6itwtlgb.wasm",
        "hash": "sha256-kT+rKHhiedu2CqnoHsozHw1gmEPglWG073+nzl/aKPY=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 242256,
        "virtualPath": "System.Collections.Immutable.wasm",
        "name": "System.Collections.Immutable.p4vddprwqz.wasm",
        "hash": "sha256-J0C6nm3ZxzvrTy9ZmW+pLYi7qcZzUI6An+TGUKMqppw=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 37456,
        "virtualPath": "System.Collections.NonGeneric.wasm",
        "name": "System.Collections.NonGeneric.l3ifvhwphc.wasm",
        "hash": "sha256-YazR/QKgaHR+Pk8gYaEQz4R6JMLAcOEvdK4No5BhtkI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 37456,
        "virtualPath": "System.Collections.Specialized.wasm",
        "name": "System.Collections.Specialized.xf4hvwr1v8.wasm",
        "hash": "sha256-Q+Cg8mXqrErFZBX1er7xjOT3HnEwd76T4qR8oue4dsw=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 102480,
        "virtualPath": "System.Collections.wasm",
        "name": "System.Collections.k5sn9llpkp.wasm",
        "hash": "sha256-SJtfYJP8BVgKuXzhJ3f5neKqdKDgu4Zr5E2PU8NEOI0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 109136,
        "virtualPath": "System.ComponentModel.Annotations.wasm",
        "name": "System.ComponentModel.Annotations.elsmlcabm6.wasm",
        "hash": "sha256-voCkTSabEJLkGnZptd3ByC5BhBFiw94dydcZ3NxwjEM=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6224,
        "virtualPath": "System.ComponentModel.DataAnnotations.wasm",
        "name": "System.ComponentModel.DataAnnotations.amkv3f98iu.wasm",
        "hash": "sha256-IokWugfIuKV99DQP5YIUmYz31+zAogopayXX5Faqv+g=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 15952,
        "virtualPath": "System.ComponentModel.EventBasedAsync.wasm",
        "name": "System.ComponentModel.EventBasedAsync.j63qh7mkw2.wasm",
        "hash": "sha256-ZeBuLhgTYAWhr9Cum3NidjlWEr0SPMiZReSMbIUr4ec=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 31824,
        "virtualPath": "System.ComponentModel.Primitives.wasm",
        "name": "System.ComponentModel.Primitives.7eb1z2suzl.wasm",
        "hash": "sha256-EYWwRmiPa9WNp+y0Un//tOV8mRe87ONOaT0oU7uiESo=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 307280,
        "virtualPath": "System.ComponentModel.TypeConverter.wasm",
        "name": "System.ComponentModel.TypeConverter.xlzqginpyz.wasm",
        "hash": "sha256-O4HmgOQ+jNPjbioAl2ZvMdvO4bdZW++/7T5VGGzK8qg=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "System.ComponentModel.wasm",
        "name": "System.ComponentModel.xs4s4am4h1.wasm",
        "hash": "sha256-r6zR+FcmqcTASZnmRdSyN/tPfYJ7p6q2OQOkTRv6YzU=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 8784,
        "virtualPath": "System.Configuration.wasm",
        "name": "System.Configuration.cvw6ewwcna.wasm",
        "hash": "sha256-6DlMGkVjtEEYbL0F3Lc5af6SVqHbbxgbRJH+1o7t53A=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 43088,
        "virtualPath": "System.Console.wasm",
        "name": "System.Console.i745b1j0pg.wasm",
        "hash": "sha256-jmNPw9mPs1ljuWU3XIhdL1Kx2S+Kg82R/zs13k5gOhQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 12880,
        "virtualPath": "System.Core.wasm",
        "name": "System.Core.6pfcffsp3i.wasm",
        "hash": "sha256-euUdpCK4/XkQf4Wza/QggKifGF5IDhYFEzm+jjAlQMk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 1007184,
        "virtualPath": "System.Data.Common.wasm",
        "name": "System.Data.Common.cq16awo54o.wasm",
        "hash": "sha256-vCciMN7NkpaRTfwncNB0Fz2ElnQNRQLTzr5899fyeh8=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Data.DataSetExtensions.wasm",
        "name": "System.Data.DataSetExtensions.tpy913q9cd.wasm",
        "hash": "sha256-AYXolLtcm6VEqYGXGc+ZLKDuh4nSshhBbgQREVXd9/M=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 14928,
        "virtualPath": "System.Data.wasm",
        "name": "System.Data.qfx7zlu15o.wasm",
        "hash": "sha256-+sOCt3qnxrj62Z5SXoBEL3iI0oltIihKqmz3UpYVCFg=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "System.Diagnostics.Contracts.wasm",
        "name": "System.Diagnostics.Contracts.mus07n7kjg.wasm",
        "hash": "sha256-DiWih4HSlVtJDS0j3gjrGo4xcFhCizqmMDLSpt4FPfo=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Diagnostics.Debug.wasm",
        "name": "System.Diagnostics.Debug.5yt0lzs9pa.wasm",
        "hash": "sha256-Y8eZYCescc++qMPucERRsBSSseWsSOuuzWZK7iboI88=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 198224,
        "virtualPath": "System.Diagnostics.DiagnosticSource.wasm",
        "name": "System.Diagnostics.DiagnosticSource.wb1h62k2w3.wasm",
        "hash": "sha256-BBGfuWlw9AKtwilTD7EtmLJIWGt1MHZZfpLlz9JRJRQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 11856,
        "virtualPath": "System.Diagnostics.FileVersionInfo.wasm",
        "name": "System.Diagnostics.FileVersionInfo.64fu3z9hia.wasm",
        "hash": "sha256-+jhlb1PKwF+3828A2pcFnq3C8KjcU7zxJq+Nbne1W1Q=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 58448,
        "virtualPath": "System.Diagnostics.Process.wasm",
        "name": "System.Diagnostics.Process.mjs64opjv7.wasm",
        "hash": "sha256-AgodkxDYxGsiPLvfZyBDcTCfR4BHokA1m5SczfN+geU=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 15440,
        "virtualPath": "System.Diagnostics.StackTrace.wasm",
        "name": "System.Diagnostics.StackTrace.qy1vx2jt7n.wasm",
        "hash": "sha256-spTn+ndqRH5+72WzYgzVS/5EeCRuBEzJzr6ezjG1TI0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 21072,
        "virtualPath": "System.Diagnostics.TextWriterTraceListener.wasm",
        "name": "System.Diagnostics.TextWriterTraceListener.7cyqkmoy77.wasm",
        "hash": "sha256-qSPBK7KeeoOl0k/QcWc9Wd9Uixp1fWGredySDRLRJq4=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Diagnostics.Tools.wasm",
        "name": "System.Diagnostics.Tools.mm44mablje.wasm",
        "hash": "sha256-bh5tm9+FVMEs1IsgKN4+CAodz+gXE/82WzbcAv9ekJY=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 48208,
        "virtualPath": "System.Diagnostics.TraceSource.wasm",
        "name": "System.Diagnostics.TraceSource.cppgzsywmh.wasm",
        "hash": "sha256-IPkoXfVvUroYfVDtpThfl0nfxSef4o1ZuV/Ag7BcBwc=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "System.Diagnostics.Tracing.wasm",
        "name": "System.Diagnostics.Tracing.eiaw23ubm7.wasm",
        "hash": "sha256-NsvsxL9Ni21v2dbwwIf5OunOABxJ9+c9RHwOLwzYg/o=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 54352,
        "virtualPath": "System.Drawing.Primitives.wasm",
        "name": "System.Drawing.Primitives.h1fi4njhcz.wasm",
        "hash": "sha256-WzMvog6LRZ2d3VLzRmaYFmgpQKQ3nxt0t0Seh5LCfsg=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 9808,
        "virtualPath": "System.Drawing.wasm",
        "name": "System.Drawing.ri1007yaak.wasm",
        "hash": "sha256-syMTbTLiG2s6vHjHuSqTZeEEthNVtXT560pPhWHsM3E=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "System.Dynamic.Runtime.wasm",
        "name": "System.Dynamic.Runtime.haa7j273iq.wasm",
        "hash": "sha256-J2XQ+nLTt4yUxzlmgAITA/1qLdL94jpFvcxMyd8ZFro=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 96848,
        "virtualPath": "System.Formats.Asn1.wasm",
        "name": "System.Formats.Asn1.g479timp01.wasm",
        "hash": "sha256-UPGQBcTcw81r25PWeCSU287bSabNhjpqGRPpMjfUEgA=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 30800,
        "virtualPath": "System.Formats.Tar.wasm",
        "name": "System.Formats.Tar.ft2urkk5jz.wasm",
        "hash": "sha256-w7QBvUEFSmEE9DFK9eyE/ZmSaaMAihKJdiiSi00Z2/Y=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Globalization.Calendars.wasm",
        "name": "System.Globalization.Calendars.r88ekoqyhn.wasm",
        "hash": "sha256-xcpMwRx3tET5DxSqzulmhCjYVYtFFXKXU4hepVPj3G0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Globalization.Extensions.wasm",
        "name": "System.Globalization.Extensions.4umwe2g3rk.wasm",
        "hash": "sha256-nBH69OSEPO8WzTp4CkC7XJ+cZrx61yHLjNsgP+KYByE=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Globalization.wasm",
        "name": "System.Globalization.k6e5r6wv1w.wasm",
        "hash": "sha256-9JUbPrW4+qSyLvy+i+Tg5/2zxilY2ZARv04QY7BsSLM=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 17488,
        "virtualPath": "System.IO.Compression.Brotli.wasm",
        "name": "System.IO.Compression.Brotli.l4hpp7ar5t.wasm",
        "hash": "sha256-C0iCD1wz3AO7kvqfwulDR9S5NiAW0A2RgYjwfqzlV+4=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.IO.Compression.FileSystem.wasm",
        "name": "System.IO.Compression.FileSystem.za53jlny9d.wasm",
        "hash": "sha256-m5SEQiaoACBwGGpiUXCd1bImeLr6B/dWr44wRg3v3IU=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 56400,
        "virtualPath": "System.IO.Compression.ZipFile.wasm",
        "name": "System.IO.Compression.ZipFile.kyl5c0a3bw.wasm",
        "hash": "sha256-lBUMo8TsS7KShFLYMbgSGIs6p7s/Rsi8obGR/pFz+0o=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 210512,
        "virtualPath": "System.IO.Compression.wasm",
        "name": "System.IO.Compression.1feu4fqrhd.wasm",
        "hash": "sha256-g9s2YnEemBzSCal88WIrz08ywv5b84aipOoI9R76CM8=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 21584,
        "virtualPath": "System.IO.FileSystem.AccessControl.wasm",
        "name": "System.IO.FileSystem.AccessControl.bdnge32uj4.wasm",
        "hash": "sha256-TzS/PxrubkVrKsTaxwO25qEUEhGCQ7pJhm4cG5Hy3qU=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 13392,
        "virtualPath": "System.IO.FileSystem.DriveInfo.wasm",
        "name": "System.IO.FileSystem.DriveInfo.lzoeioxzz1.wasm",
        "hash": "sha256-/wfOFXzBM/ngt5kEAN6YHXUvcJKxkjuw5OTwLbweIQQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.IO.FileSystem.Primitives.wasm",
        "name": "System.IO.FileSystem.Primitives.hpwp1k4bhc.wasm",
        "hash": "sha256-Kx019+yQevz6ugqvFsXecypoE/IGmZjNmZ20qeJeqng=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 22608,
        "virtualPath": "System.IO.FileSystem.Watcher.wasm",
        "name": "System.IO.FileSystem.Watcher.21npz5rki9.wasm",
        "hash": "sha256-1ieS6Mzleo0KdadQJOOyrF9DCFEJ2a0fZXw0ywjKqNs=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.IO.FileSystem.wasm",
        "name": "System.IO.FileSystem.gbc73wagc1.wasm",
        "hash": "sha256-HD2RmaWsSiInijpj0jsnFgkPwEcq5oodJZ3IWJQsuGk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 24144,
        "virtualPath": "System.IO.IsolatedStorage.wasm",
        "name": "System.IO.IsolatedStorage.ymhwr8jip5.wasm",
        "hash": "sha256-thO0VHRmJN+cLoW2LjaJMHv/w7SS0QXja0qvklYb04Q=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 38480,
        "virtualPath": "System.IO.MemoryMappedFiles.wasm",
        "name": "System.IO.MemoryMappedFiles.muclr167ru.wasm",
        "hash": "sha256-TvM+XNNJ45FMAkQtsr5e3AKawujy2B8PXdicmdvZB9w=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 73296,
        "virtualPath": "System.IO.Pipelines.wasm",
        "name": "System.IO.Pipelines.bthhe1iaig.wasm",
        "hash": "sha256-b4YtkZll/NFAgUghEJe0BFIHNl7ER3MsvF0E54bVHlc=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 12880,
        "virtualPath": "System.IO.Pipes.AccessControl.wasm",
        "name": "System.IO.Pipes.AccessControl.rnj38cdkdl.wasm",
        "hash": "sha256-Rw7RSlCGR3kYoGjXkxFK96ZX2p5n5j7VVzcTMCotw4k=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 32336,
        "virtualPath": "System.IO.Pipes.wasm",
        "name": "System.IO.Pipes.gzngpq7ndg.wasm",
        "hash": "sha256-lJHg+JsGu/34Bnu4IcyUJtW39OY/COi5MW5meKmjuDs=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.IO.UnmanagedMemoryStream.wasm",
        "name": "System.IO.UnmanagedMemoryStream.bxan7mxuel.wasm",
        "hash": "sha256-D6Z8qWUfd/JS/GUU9PsdfH48P+uwxaZK6zFINuJ9akI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.IO.wasm",
        "name": "System.IO.84y6l2s33h.wasm",
        "hash": "sha256-OO8blmtixRYeSBAf/hR8g9+aNRogIvuCIjE59LzQp+0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 503888,
        "virtualPath": "System.Linq.AsyncEnumerable.wasm",
        "name": "System.Linq.AsyncEnumerable.x6rw06ue60.wasm",
        "hash": "sha256-qYmYnZbJp65hnzA1AF0yXhevW084nw6y9S4hH21YzzI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 565328,
        "virtualPath": "System.Linq.Expressions.wasm",
        "name": "System.Linq.Expressions.kcnqwqifz8.wasm",
        "hash": "sha256-tKAT0tZXLBswcfR6FQ0tgGsT3HlGzhivB1AhlH/cl1o=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 214608,
        "virtualPath": "System.Linq.Parallel.wasm",
        "name": "System.Linq.Parallel.ptniqneo1f.wasm",
        "hash": "sha256-HT9SkNhB0DZVJ5f3Kvb4JscZUWLFtNmzwdahUfR5cy0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 71248,
        "virtualPath": "System.Linq.Queryable.wasm",
        "name": "System.Linq.Queryable.re0691lc4q.wasm",
        "hash": "sha256-bRt3tP3MOJn3GEF0Et6fyQPU4Ln7c6mZ8fWlYd+tgBE=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 202832,
        "virtualPath": "System.Linq.wasm",
        "name": "System.Linq.euy2k23mtl.wasm",
        "hash": "sha256-5qhPE6F5wVUyLUhPMc4mAAzy3Gver8BOk0TQ87FTBzE=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 49232,
        "virtualPath": "System.Memory.wasm",
        "name": "System.Memory.ef1g3d9sq0.wasm",
        "hash": "sha256-xs1MqDaaoc6OQgxa5oB2km3/mT1OmDnPnprpsqJ/UfY=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 45648,
        "virtualPath": "System.Net.Http.Json.wasm",
        "name": "System.Net.Http.Json.va1cxuypof.wasm",
        "hash": "sha256-aRUykxJM9Miu0pZnhrCfLbWPrKLvPbElntNs9rFBh00=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 293456,
        "virtualPath": "System.Net.Http.wasm",
        "name": "System.Net.Http.ky790p52oa.wasm",
        "hash": "sha256-dJunlLch6lCzhM9GteuKZmdCjTXOhLRIcTYK2tnWgRo=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 45648,
        "virtualPath": "System.Net.HttpListener.wasm",
        "name": "System.Net.HttpListener.w09ai14p5v.wasm",
        "hash": "sha256-MMLwJpOlOp8eXD/H753v4aWlb8Ly7lDhtysfyOHeeJQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 126032,
        "virtualPath": "System.Net.Mail.wasm",
        "name": "System.Net.Mail.dszdpv1gui.wasm",
        "hash": "sha256-k2RCpy7YW+3lK+XMZy6H+YprsJwn/Nht5tByyOFz344=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 21072,
        "virtualPath": "System.Net.NameResolution.wasm",
        "name": "System.Net.NameResolution.zwue4snxaw.wasm",
        "hash": "sha256-A7jVsj2DhDRcn7+5UOfgJ+rpttgp1R4CNSTcxhoNPKU=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 31824,
        "virtualPath": "System.Net.NetworkInformation.wasm",
        "name": "System.Net.NetworkInformation.365k34op20.wasm",
        "hash": "sha256-K4olcCkU0wwiERNOJ/fW7yBVJVz1LYl34oU2gxKp+kc=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 17488,
        "virtualPath": "System.Net.Ping.wasm",
        "name": "System.Net.Ping.wdiaa1ypdw.wasm",
        "hash": "sha256-lMlTbGiZ8rJ+LWo/yqYfWA+f4tvjTap06cAAlrTdsqw=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 97872,
        "virtualPath": "System.Net.Primitives.wasm",
        "name": "System.Net.Primitives.48alnfjp7u.wasm",
        "hash": "sha256-BKptMFnwukB+D87kxeO3ZCYrtX8c8orGTmShjHXArF8=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 28752,
        "virtualPath": "System.Net.Quic.wasm",
        "name": "System.Net.Quic.801olxp69m.wasm",
        "hash": "sha256-6Ht2Ml/OiyaWIJaqKGDSPsc8tYZnuFdpmC3NOggeJgo=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 55376,
        "virtualPath": "System.Net.Requests.wasm",
        "name": "System.Net.Requests.b2bx4zqwb7.wasm",
        "hash": "sha256-vbkW5YZv+QfWbQnjb5t7kJDcwNIPt7IPd9XGVCLpe+I=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 113744,
        "virtualPath": "System.Net.Security.wasm",
        "name": "System.Net.Security.39gzxbiiz8.wasm",
        "hash": "sha256-+VXxRStfz+/8J+0tLN660QvxGR2rAFE+i0MaNb4GBKM=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 31312,
        "virtualPath": "System.Net.ServerSentEvents.wasm",
        "name": "System.Net.ServerSentEvents.b0d2mai8i8.wasm",
        "hash": "sha256-pfNMnv8Aw5ejvLSNJsr0Q3E9BbQaRqxb+aC1RGnxbLs=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Net.ServicePoint.wasm",
        "name": "System.Net.ServicePoint.bag3xxkn0i.wasm",
        "hash": "sha256-zaVDTGiJTfLAEKdnaOL11E9ktNbb35fYIAreAewoG4o=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 64080,
        "virtualPath": "System.Net.Sockets.wasm",
        "name": "System.Net.Sockets.yepkhv6ra9.wasm",
        "hash": "sha256-qmpFh0HgD6BH9lR5S7yKc83ILoXo9vbPUr+9mnbrbI8=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 46672,
        "virtualPath": "System.Net.WebClient.wasm",
        "name": "System.Net.WebClient.jpoa5pm2ng.wasm",
        "hash": "sha256-l6TATJhWcOQ0TeKfkbOcAc5cLCM41bECIein8KH3l5E=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 22608,
        "virtualPath": "System.Net.WebHeaderCollection.wasm",
        "name": "System.Net.WebHeaderCollection.ecxhgdijtz.wasm",
        "hash": "sha256-8vg3kKlOUOT5zxcmVfBKTz+/LBl1JTFnkbetmcZRj8w=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 11856,
        "virtualPath": "System.Net.WebProxy.wasm",
        "name": "System.Net.WebProxy.fkhn9e5cwp.wasm",
        "hash": "sha256-UVZJkMuoWxmBn28RQxhRiOd1a2slPIawNF1FgKvS2HM=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 41552,
        "virtualPath": "System.Net.WebSockets.Client.wasm",
        "name": "System.Net.WebSockets.Client.4n1nr5pu7v.wasm",
        "hash": "sha256-EKdgOr1REFxyRxADjXKHbHblxd2W7oebspm+C5semAI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 100432,
        "virtualPath": "System.Net.WebSockets.wasm",
        "name": "System.Net.WebSockets.35rnax297b.wasm",
        "hash": "sha256-J7dxCGUo/w5mEtDxxcHXfuMH1A5bYlXsyhDrdGzmoCA=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6736,
        "virtualPath": "System.Net.wasm",
        "name": "System.Net.5i5hezc033.wasm",
        "hash": "sha256-488UGGTMi5OzrJZDT8Fk8BlOoBtoTMaXMq4BAalVuRs=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "System.Numerics.Vectors.wasm",
        "name": "System.Numerics.Vectors.sz4qgeij65.wasm",
        "hash": "sha256-yIO5Uj/hUuJeOTHwwI6RPBWmNnCa5lE05/v1yUh1Y9s=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Numerics.wasm",
        "name": "System.Numerics.4i2883rfr8.wasm",
        "hash": "sha256-VN21JpdHM1Xb9m+V+MnSP0YAmwO9SthandoqEdM6CMc=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 30800,
        "virtualPath": "System.ObjectModel.wasm",
        "name": "System.ObjectModel.cplokk0032.wasm",
        "hash": "sha256-IBju1gSTZ3D/0LeFp8pknl3P6CpKcsJooNvGAngEySQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 848976,
        "virtualPath": "System.Private.DataContractSerialization.wasm",
        "name": "System.Private.DataContractSerialization.ey43wq3voj.wasm",
        "hash": "sha256-1vM1QzXcr+BY4gdgBp0YyKfq7LSty1vFqM4l0/ENY7w=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 92752,
        "virtualPath": "System.Private.Uri.wasm",
        "name": "System.Private.Uri.6aawlynfiz.wasm",
        "hash": "sha256-8taWZUuOf5XEZEMYy9w0jtH91t+2/Nw6MQ7evFSVcdo=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 144464,
        "virtualPath": "System.Private.Xml.Linq.wasm",
        "name": "System.Private.Xml.Linq.44861qqm58.wasm",
        "hash": "sha256-lNfY8GMkfK48OUKHghmCmmV4BG5b6E39bV7zyKPVRac=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 3109968,
        "virtualPath": "System.Private.Xml.wasm",
        "name": "System.Private.Xml.td5aoq1yaf.wasm",
        "hash": "sha256-P3aDN69w5lH8w9uzeEbjqFSqKvMRHAQQWAs4y1jXTlg=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 27728,
        "virtualPath": "System.Reflection.DispatchProxy.wasm",
        "name": "System.Reflection.DispatchProxy.pg75psa3eu.wasm",
        "hash": "sha256-Cb41MyL8zq1BJb+0RsEAf3FQVyFgz0RqKT5RItMxXqk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Reflection.Emit.ILGeneration.wasm",
        "name": "System.Reflection.Emit.ILGeneration.f9aqsivf4a.wasm",
        "hash": "sha256-jXXbY9fFxBGTp5GDqkxHPW1k3hdAwYRF6o4SvXGCYZk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Reflection.Emit.Lightweight.wasm",
        "name": "System.Reflection.Emit.Lightweight.3diqmxyie0.wasm",
        "hash": "sha256-iYa7/HrKpnalxK9J9ArMMnjSb57HUN1yyL5cr6Qg7PY=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 123984,
        "virtualPath": "System.Reflection.Emit.wasm",
        "name": "System.Reflection.Emit.3m5083ajwf.wasm",
        "hash": "sha256-a3CO2J9R0dM4d9jbnJ/FSFDNUAXcJYl2nVf/mtMrg68=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Reflection.Extensions.wasm",
        "name": "System.Reflection.Extensions.kbkkpw5gsl.wasm",
        "hash": "sha256-INN8wRXzq/iv+R3UEna0IAIRnN7mIZPEGJ3eUxnLr5A=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 494160,
        "virtualPath": "System.Reflection.Metadata.wasm",
        "name": "System.Reflection.Metadata.7irxyla1ye.wasm",
        "hash": "sha256-f79tjAs/Kl4Eq/gxzcL7buI+ICFp0pHPs02KDtSM93s=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Reflection.Primitives.wasm",
        "name": "System.Reflection.Primitives.i5t898xz0j.wasm",
        "hash": "sha256-aiHudDmi5726CLZGfNVnCK2Ug6brpfEnzaznlDiJ3N8=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 13904,
        "virtualPath": "System.Reflection.TypeExtensions.wasm",
        "name": "System.Reflection.TypeExtensions.5md00mphaa.wasm",
        "hash": "sha256-7SwHaRkp/GgVK3jccgW12cb92B0KWG1wCdnUaCzTWhY=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "System.Reflection.wasm",
        "name": "System.Reflection.fl7bt52t41.wasm",
        "hash": "sha256-zTMICfDvykS22rqVFbM6yPLrCPhUdg0513ZmGCpLrFI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Resources.Reader.wasm",
        "name": "System.Resources.Reader.bd7vjd0m86.wasm",
        "hash": "sha256-8QFPpt9O0LzbV7MVJE30z6/Be+qHO5NhtFf40q2qGQA=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Resources.ResourceManager.wasm",
        "name": "System.Resources.ResourceManager.5qcfvc7pte.wasm",
        "hash": "sha256-0zLYXTTGHjbK+hhOZ3CjBvM/slH/kyzYhfN9/8Ewzws=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 16464,
        "virtualPath": "System.Resources.Writer.wasm",
        "name": "System.Resources.Writer.4fuxgn82tx.wasm",
        "hash": "sha256-QOTc278cQdco9ftIYIkTfaCrcOpyADcPCMNmhGvbeTY=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Runtime.CompilerServices.Unsafe.wasm",
        "name": "System.Runtime.CompilerServices.Unsafe.lerzl56d86.wasm",
        "hash": "sha256-fX7cG0RmMxoUpI5LX1zwISNfRs5bot3lJSzjSRVaQbw=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6736,
        "virtualPath": "System.Runtime.CompilerServices.VisualC.wasm",
        "name": "System.Runtime.CompilerServices.VisualC.ngza2hkxlv.wasm",
        "hash": "sha256-B3hkF9JvztJi5IcyoZlf2HK9+ID5xuS/S7YgpKTSR5o=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 7248,
        "virtualPath": "System.Runtime.Extensions.wasm",
        "name": "System.Runtime.Extensions.m88lse03tp.wasm",
        "hash": "sha256-9nAt4JhThWISwzHowAMOQTjuJLTyIsw4Cs0w9L8wxmg=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Runtime.Handles.wasm",
        "name": "System.Runtime.Handles.26aspq2dx3.wasm",
        "hash": "sha256-hhxywWFFcsqPnaknYOK3jihHs/AG0OwuGR4+/S9tEeE=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Runtime.InteropServices.RuntimeInformation.wasm",
        "name": "System.Runtime.InteropServices.RuntimeInformation.095kj7f2kq.wasm",
        "hash": "sha256-bMmFarqZkqfo+51x0YethTjGg1HmDt/7t9Df8CENq7k=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 53840,
        "virtualPath": "System.Runtime.InteropServices.wasm",
        "name": "System.Runtime.InteropServices.27mzytq5zi.wasm",
        "hash": "sha256-HjH/JiKRwt4ihO5q2IDFPBbNQpbyp6gR4382MoIae3Y=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 8272,
        "virtualPath": "System.Runtime.Intrinsics.wasm",
        "name": "System.Runtime.Intrinsics.o80ipo0lun.wasm",
        "hash": "sha256-SNV9oH0Lf06DGJxIt6bLujofuxrkKPhV2Xo7ZV0U8Gw=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Runtime.Loader.wasm",
        "name": "System.Runtime.Loader.ov2p4nhco5.wasm",
        "hash": "sha256-1Cr0E7PdpChVxLxH6+X9cmLQsWWynhecFC/0xqqmX0M=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 180304,
        "virtualPath": "System.Runtime.Numerics.wasm",
        "name": "System.Runtime.Numerics.3751j13j91.wasm",
        "hash": "sha256-cajkxlIkoDjKxAkls+gA4SWQOed/lpRYftrJc4+vdx4=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 55376,
        "virtualPath": "System.Runtime.Serialization.Formatters.wasm",
        "name": "System.Runtime.Serialization.Formatters.r7r7lpi20w.wasm",
        "hash": "sha256-eaGKg0ldHuepov3XIeDC7sAh/U+pn95S6hNGqTMoJRw=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Runtime.Serialization.Json.wasm",
        "name": "System.Runtime.Serialization.Json.pwe13377b8.wasm",
        "hash": "sha256-uQAG7Ov/Ag4f390SW+08+YI2n9kN5UlTciLUYIuR6sU=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 12880,
        "virtualPath": "System.Runtime.Serialization.Primitives.wasm",
        "name": "System.Runtime.Serialization.Primitives.lpnaliwlwb.wasm",
        "hash": "sha256-iZO6lFIyZ8BTt1QFZXXyTvUKLrzOyENlZJHvN7sTw+I=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6224,
        "virtualPath": "System.Runtime.Serialization.Xml.wasm",
        "name": "System.Runtime.Serialization.Xml.ac27mgx6wc.wasm",
        "hash": "sha256-mq0ZOfEF80sJdfJZbbxh+zhzJ0humf0pBGOZgOPpjC0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6736,
        "virtualPath": "System.Runtime.Serialization.wasm",
        "name": "System.Runtime.Serialization.1pbefz8llk.wasm",
        "hash": "sha256-SzJ49kpZ8ZLGsdzfOTeUYnMFS1QqGaX+VUZhwEApmI0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 35408,
        "virtualPath": "System.Runtime.wasm",
        "name": "System.Runtime.7tzi4m9ebd.wasm",
        "hash": "sha256-yzH81QFQxo8CVF/gNpySUfIuvD4ehLt54LJApJPZdqg=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 47184,
        "virtualPath": "System.Security.AccessControl.wasm",
        "name": "System.Security.AccessControl.9hqb4043ri.wasm",
        "hash": "sha256-zd4YBufEx2c+Lg5eJhLs8f3LcbHFIx8R400l9flFSsQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 44112,
        "virtualPath": "System.Security.Claims.wasm",
        "name": "System.Security.Claims.kiljjtny40.wasm",
        "hash": "sha256-rGrcRYLsPhsk3+Iqn4gdqwXE7hvdw5DlIiUaCHZLkWE=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6736,
        "virtualPath": "System.Security.Cryptography.Algorithms.wasm",
        "name": "System.Security.Cryptography.Algorithms.g8yk7acb8n.wasm",
        "hash": "sha256-UDkyMIX3l+86P3Q4C5QpPGZMDSdiOxIuM0XHILjaQjk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "System.Security.Cryptography.Cng.wasm",
        "name": "System.Security.Cryptography.Cng.l8pr1bzi5c.wasm",
        "hash": "sha256-hRXnr4zP7BIYCTwi1uCTdx6p5Es7WRLVSwCnpnMPjnY=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "System.Security.Cryptography.Csp.wasm",
        "name": "System.Security.Cryptography.Csp.hq1azjgfhu.wasm",
        "hash": "sha256-SRYCzKb0AbNubWlIq6nsVi2Sq1jG8Km9kxyzk+1HcpU=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Security.Cryptography.Encoding.wasm",
        "name": "System.Security.Cryptography.Encoding.qyhtnb211u.wasm",
        "hash": "sha256-zhyjOzxS/n2KYM/durhx0ccu7GMxogWrpOzMuwe/hQ4=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Security.Cryptography.OpenSsl.wasm",
        "name": "System.Security.Cryptography.OpenSsl.xd7i67v35f.wasm",
        "hash": "sha256-5efffvgSR5tV7iNMwyDRB4TpO5gXlxAZUbwzYzNCy5k=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Security.Cryptography.Primitives.wasm",
        "name": "System.Security.Cryptography.Primitives.3t9fzfj9e0.wasm",
        "hash": "sha256-mRGoRvwylk/BiB9MQsI8nW1NuZqPV5Tc/H+j7lg5oNs=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6736,
        "virtualPath": "System.Security.Cryptography.X509Certificates.wasm",
        "name": "System.Security.Cryptography.X509Certificates.3lyetdrbit.wasm",
        "hash": "sha256-l6dlXgIwH9pS0tQpgW2hvYOF4Hp/lpWD/O0wSZHJpBQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 714320,
        "virtualPath": "System.Security.Cryptography.wasm",
        "name": "System.Security.Cryptography.v3a1pjxrqo.wasm",
        "hash": "sha256-lWForjNH1E+zNiVGl3Qif5XV10dB7gHL7lWwwHqy8IM=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 27216,
        "virtualPath": "System.Security.Principal.Windows.wasm",
        "name": "System.Security.Principal.Windows.znd8nivd8s.wasm",
        "hash": "sha256-LyanTsBE680lcirB4BsCDA7eyCd9EjLgzpggLrWoHOM=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Security.Principal.wasm",
        "name": "System.Security.Principal.ye179h9qav.wasm",
        "hash": "sha256-FmA/81DtDWDpwRiI0spnTs2rk58CW3tD51lZMjQs2IQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Security.SecureString.wasm",
        "name": "System.Security.SecureString.javmtvmcnb.wasm",
        "hash": "sha256-FT9F3oCEkX+qddy6sm1K0SU7TAbUOQ1p/Tax6atQd+k=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 7760,
        "virtualPath": "System.Security.wasm",
        "name": "System.Security.w66lns545t.wasm",
        "hash": "sha256-aDasdNSXIxnMz4GGwwioVG1/7cUPHbSHqvY9aBE2sTY=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6224,
        "virtualPath": "System.ServiceModel.Web.wasm",
        "name": "System.ServiceModel.Web.7aolwofx7i.wasm",
        "hash": "sha256-Lx66u3noUVRZIOshal6FUJS9lw6LYmvRYZIDozpcAaQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.ServiceProcess.wasm",
        "name": "System.ServiceProcess.rg615o187o.wasm",
        "hash": "sha256-+2tIwzcg5SgePxDtDpkP8O7PlvSFf3VeZKguMAa6dDI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 731728,
        "virtualPath": "System.Text.Encoding.CodePages.wasm",
        "name": "System.Text.Encoding.CodePages.r8vg5ncz2h.wasm",
        "hash": "sha256-K2OO6ajkFEcEBvVRPR5i/OzajoyYZU++t80jyWlHar8=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Text.Encoding.Extensions.wasm",
        "name": "System.Text.Encoding.Extensions.0gnv5gnuz9.wasm",
        "hash": "sha256-kCLyWy1IYi/UyX3C+jJW7+0VX7NKRVl9Q2riwsWvP1I=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Text.Encoding.wasm",
        "name": "System.Text.Encoding.b8cxo9w6ml.wasm",
        "hash": "sha256-1ef/7h5dHemuwjOiNV7H+62mTUZdLxCiFhcM4cX7XW4=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 55888,
        "virtualPath": "System.Text.Encodings.Web.wasm",
        "name": "System.Text.Encodings.Web.h1rxkkc9j4.wasm",
        "hash": "sha256-JWvlyOxgTVseR/en6JzthaSgz6MsRf4ylnnGXGEQjig=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 726608,
        "virtualPath": "System.Text.Json.wasm",
        "name": "System.Text.Json.043k9o56jl.wasm",
        "hash": "sha256-wCUslqhpNE8oxh7DO4zxotOlPPoArAqbhdNUi0yNygI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 384592,
        "virtualPath": "System.Text.RegularExpressions.wasm",
        "name": "System.Text.RegularExpressions.lrqhie985c.wasm",
        "hash": "sha256-P4RihR2wgsvw0wsRJU8JZKggbZoAoXtQ2xxL0ZjgEVM=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 23120,
        "virtualPath": "System.Threading.AccessControl.wasm",
        "name": "System.Threading.AccessControl.pgthlkysnu.wasm",
        "hash": "sha256-adQpFgANY01HXb0o6b6mSjJFKRQJ8eaNIWg2kcrl5y0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 55888,
        "virtualPath": "System.Threading.Channels.wasm",
        "name": "System.Threading.Channels.fbe2ex1ela.wasm",
        "hash": "sha256-+zZLMVJwZTJIjBvzXJd9JUYlOUFJUFTlKy0/fq/FqfI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Threading.Overlapped.wasm",
        "name": "System.Threading.Overlapped.54tk2l8ody.wasm",
        "hash": "sha256-m/m6fHAKxq6BatFDj5YZUMSWkyj1jlmJX0JEwNBV1k0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 175696,
        "virtualPath": "System.Threading.Tasks.Dataflow.wasm",
        "name": "System.Threading.Tasks.Dataflow.0mpxk4x7o8.wasm",
        "hash": "sha256-6PPvcsbl5b5pxCJoJGdMd8oQJB5pO0Me2RA5igY36vY=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Threading.Tasks.Extensions.wasm",
        "name": "System.Threading.Tasks.Extensions.2zf0h5snoq.wasm",
        "hash": "sha256-IkrJdDBKd8zgydsI5B/fPnm3yGJUO7v/wVVnF2+PMKg=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 51280,
        "virtualPath": "System.Threading.Tasks.Parallel.wasm",
        "name": "System.Threading.Tasks.Parallel.9xvupe3cq5.wasm",
        "hash": "sha256-4yEeAlgzzdbYpN0o7JHBXfhgG66TQrH0oe62ZNvbmsk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6224,
        "virtualPath": "System.Threading.Tasks.wasm",
        "name": "System.Threading.Tasks.xhwmavft6o.wasm",
        "hash": "sha256-+1tgvqwfHpeKflktwi9eyh6/f/xCb64fncfAC+g1BeU=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Threading.Thread.wasm",
        "name": "System.Threading.Thread.bwroa5c5oj.wasm",
        "hash": "sha256-OyGnOIDDZoss3asbo4xNfR58tQEnkYJoD5navFMJe04=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Threading.ThreadPool.wasm",
        "name": "System.Threading.ThreadPool.vf4rnn6bza.wasm",
        "hash": "sha256-xV040E6cFgezV2zoaL/kxzc81RpAr4tSM1bQI3RKDtk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Threading.Timer.wasm",
        "name": "System.Threading.Timer.aggyw7i2e4.wasm",
        "hash": "sha256-NjzPBj6mH6XgReptsJ3LDZA4BCQH0Xgro+/onPYh7TY=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 34384,
        "virtualPath": "System.Threading.wasm",
        "name": "System.Threading.eo2wsrm247.wasm",
        "hash": "sha256-cmK0Mam1ySG9elakDPAxK5CR/qFZ3IE6aXGQZvWI+CM=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 164432,
        "virtualPath": "System.Transactions.Local.wasm",
        "name": "System.Transactions.Local.zcm2w6isb3.wasm",
        "hash": "sha256-TQq1Ocm/2jmEd2jBQa4AgCS/hz6Kdyg+9rmmZrGWL4w=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 6224,
        "virtualPath": "System.Transactions.wasm",
        "name": "System.Transactions.k77nclb0ge.wasm",
        "hash": "sha256-wMs+LQ8cC9TgV1ZG7XGtVoIkCQZ8Zm9Nd9blxarTg8o=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.ValueTuple.wasm",
        "name": "System.ValueTuple.wfzy6y4bxk.wasm",
        "hash": "sha256-7HVGrQe7e+kXVjnq7i7hIyER7emANSb3jZ1y0R4UR8Q=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 19536,
        "virtualPath": "System.Web.HttpUtility.wasm",
        "name": "System.Web.HttpUtility.ogi5ilmlab.wasm",
        "hash": "sha256-SK2yzuHDyoedQswkH1eyXgm5zvXPw6mGylFwD2ZKBn0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 4688,
        "virtualPath": "System.Web.wasm",
        "name": "System.Web.83bcfir916.wasm",
        "hash": "sha256-3FVnctkCDdnS7wzxOQrqIwu7KH4ZIfeOYs7IxEos2ZY=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Windows.wasm",
        "name": "System.Windows.fgo6all3kj.wasm",
        "hash": "sha256-T48b893HCdlx4bpWRxLbTxczq7Jij70GaU9VaE9OnMY=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Xml.Linq.wasm",
        "name": "System.Xml.Linq.g38x4qav7e.wasm",
        "hash": "sha256-xMxNmjvoYCjZzQTRKwPZqSwRT55XJd0aZR16wzajbGk=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 11344,
        "virtualPath": "System.Xml.ReaderWriter.wasm",
        "name": "System.Xml.ReaderWriter.r6yiod039x.wasm",
        "hash": "sha256-k6kmutaTIDxXyGuOTFBvFEdWDmrD5VDtFztNK22TnQo=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "System.Xml.Serialization.wasm",
        "name": "System.Xml.Serialization.9e5oq9ds35.wasm",
        "hash": "sha256-r+BqGGCFM76symxaxniQAFzRVLs63oQ29Tp4fAROFGg=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "System.Xml.XDocument.wasm",
        "name": "System.Xml.XDocument.b5cr7kdgy9.wasm",
        "hash": "sha256-P33ZbSfMoZ8+9HNookwaznNOfdvygiUJDEn/jc7OOwY=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "System.Xml.XPath.XDocument.wasm",
        "name": "System.Xml.XPath.XDocument.6euu0egpou.wasm",
        "hash": "sha256-Xi3QcP2Mma9xTGvWFqt4x/AlBKO+Q0xgzHOIzFcOOyQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Xml.XPath.wasm",
        "name": "System.Xml.XPath.ldth12emrq.wasm",
        "hash": "sha256-BwhFTA4TtxCwwWpfUuZM6RiZswwmpa17fPnrd/UadT4=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5200,
        "virtualPath": "System.Xml.XmlDocument.wasm",
        "name": "System.Xml.XmlDocument.gvn5g8jkuq.wasm",
        "hash": "sha256-HGi00Z0syhZ1QYxPQ7Rd9OG10kk+nc4DK8NT0Gpe9lA=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 7248,
        "virtualPath": "System.Xml.XmlSerializer.wasm",
        "name": "System.Xml.XmlSerializer.6brrz5wicb.wasm",
        "hash": "sha256-AnqU/62+wPoGcc+cUQzXbOVrN/V3MFBMANe53Il1PiQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 12880,
        "virtualPath": "System.Xml.wasm",
        "name": "System.Xml.uccnxvcbyr.wasm",
        "hash": "sha256-skSYp+X2Uvy+hcRS4OSDeOatI74jTPj2RcMP+j26LuY=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 39504,
        "virtualPath": "System.wasm",
        "name": "System.56ze00upuh.wasm",
        "hash": "sha256-czarGtBoRVSbvpZln+Y1Vwz8W2uAvGbVlDVfsvcNxsI=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 5712,
        "virtualPath": "WindowsBase.wasm",
        "name": "WindowsBase.y9ni4442pu.wasm",
        "hash": "sha256-VhqIny+ZeLQkY0udf7jmVL8zinwQAVbSCxtxrw2MJNM=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 49232,
        "virtualPath": "mscorlib.wasm",
        "name": "mscorlib.jlj7aofoiv.wasm",
        "hash": "sha256-CQdlhCc/RPik/gkUeYG8KAELhebJxU493W1tWkEu+KQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 90704,
        "virtualPath": "netstandard.wasm",
        "name": "netstandard.5y03d987wg.wasm",
        "hash": "sha256-c6fUBrcUa3uSZfUhvFuxBN2UVukMJ0lNPvFJyfmMwWE=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 10320,
        "virtualPath": "BlazingStory.Abstractions.wasm",
        "name": "BlazingStory.Abstractions.3ha1i2nhv0.wasm",
        "hash": "sha256-ucrAiiQbgXbqW9s/m0ZUrBOU8mJ7bJ1tN5Ws4x9gSN0=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 143952,
        "virtualPath": "BlazingStory.Addons.BuiltIns.wasm",
        "name": "BlazingStory.Addons.BuiltIns.8fxil2ggdy.wasm",
        "hash": "sha256-tPYG38ReRQVaPznpQb6DDxsOg7tdJ3q41CXAEABeDvM=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 12368,
        "virtualPath": "BlazingStory.Addons.wasm",
        "name": "BlazingStory.Addons.hr2kvv15mv.wasm",
        "hash": "sha256-Awr1MRdS+2nv/YpwBCToRpKNrJ8PUMWuybjSPicnBgA=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 288336,
        "virtualPath": "BlazingStory.wasm",
        "name": "BlazingStory.rtd5jsjjdj.wasm",
        "hash": "sha256-wVeFnNMrqbmQW4ivrhedfi7UE5loo/I9R4tfK+OVcb8=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 140880,
        "virtualPath": "BlazingStory.ToolKit.wasm",
        "name": "BlazingStory.ToolKit.nivc7gsc82.wasm",
        "hash": "sha256-j5FHvkel5dQDPWIJg0FaMHR5Ve8FDdnsk5fTQKrrSOQ=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 24144,
        "virtualPath": "MyBlazorWasmApp1.wasm",
        "name": "MyBlazorWasmApp1.pwinhkbcvy.wasm",
        "hash": "sha256-efBQMgqPXKJIp3oioK9Ag8qvmLqQo6bORWsm9WkBbWs=",
        "cache": "force-cache"
      },
      {
        "payloadSize": 32336,
        "virtualPath": "MyBlazorWasmApp1.Stories.wasm",
        "name": "MyBlazorWasmApp1.Stories.2vn6eqgxef.wasm",
        "hash": "sha256-TnQxSXU1ZeE5XvyKm+TvBRBQMwHVQLC2TKx2L+LqyCY=",
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
