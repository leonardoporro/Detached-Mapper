/******/ (function(modules) { // webpackBootstrap
/******/ 	var parentHotUpdateCallback = this["webpackHotUpdate"];
/******/ 	this["webpackHotUpdate"] = 
/******/ 	function webpackHotUpdateCallback(chunkId, moreModules) { // eslint-disable-line no-unused-vars
/******/ 		hotAddUpdateChunk(chunkId, moreModules);
/******/ 		if(parentHotUpdateCallback) parentHotUpdateCallback(chunkId, moreModules);
/******/ 	}
/******/ 	
/******/ 	function hotDownloadUpdateChunk(chunkId) { // eslint-disable-line no-unused-vars
/******/ 		var head = document.getElementsByTagName("head")[0];
/******/ 		var script = document.createElement("script");
/******/ 		script.type = "text/javascript";
/******/ 		script.charset = "utf-8";
/******/ 		script.src = __webpack_require__.p + "" + chunkId + "." + hotCurrentHash + ".hot-update.js";
/******/ 		head.appendChild(script);
/******/ 	}
/******/ 	
/******/ 	function hotDownloadManifest(callback) { // eslint-disable-line no-unused-vars
/******/ 		if(typeof XMLHttpRequest === "undefined")
/******/ 			return callback(new Error("No browser support"));
/******/ 		try {
/******/ 			var request = new XMLHttpRequest();
/******/ 			var requestPath = __webpack_require__.p + "" + hotCurrentHash + ".hot-update.json";
/******/ 			request.open("GET", requestPath, true);
/******/ 			request.timeout = 10000;
/******/ 			request.send(null);
/******/ 		} catch(err) {
/******/ 			return callback(err);
/******/ 		}
/******/ 		request.onreadystatechange = function() {
/******/ 			if(request.readyState !== 4) return;
/******/ 			if(request.status === 0) {
/******/ 				// timeout
/******/ 				callback(new Error("Manifest request to " + requestPath + " timed out."));
/******/ 			} else if(request.status === 404) {
/******/ 				// no update available
/******/ 				callback();
/******/ 			} else if(request.status !== 200 && request.status !== 304) {
/******/ 				// other failure
/******/ 				callback(new Error("Manifest request to " + requestPath + " failed."));
/******/ 			} else {
/******/ 				// success
/******/ 				try {
/******/ 					var update = JSON.parse(request.responseText);
/******/ 				} catch(e) {
/******/ 					callback(e);
/******/ 					return;
/******/ 				}
/******/ 				callback(null, update);
/******/ 			}
/******/ 		};
/******/ 	}
/******/
/******/ 	
/******/ 	
/******/ 	// Copied from https://github.com/facebook/react/blob/bef45b0/src/shared/utils/canDefineProperty.js
/******/ 	var canDefineProperty = false;
/******/ 	try {
/******/ 		Object.defineProperty({}, "x", {
/******/ 			get: function() {}
/******/ 		});
/******/ 		canDefineProperty = true;
/******/ 	} catch(x) {
/******/ 		// IE will fail on defineProperty
/******/ 	}
/******/ 	
/******/ 	var hotApplyOnUpdate = true;
/******/ 	var hotCurrentHash = "e52d37073402c6e8fbc1"; // eslint-disable-line no-unused-vars
/******/ 	var hotCurrentModuleData = {};
/******/ 	var hotCurrentParents = []; // eslint-disable-line no-unused-vars
/******/ 	
/******/ 	function hotCreateRequire(moduleId) { // eslint-disable-line no-unused-vars
/******/ 		var me = installedModules[moduleId];
/******/ 		if(!me) return __webpack_require__;
/******/ 		var fn = function(request) {
/******/ 			if(me.hot.active) {
/******/ 				if(installedModules[request]) {
/******/ 					if(installedModules[request].parents.indexOf(moduleId) < 0)
/******/ 						installedModules[request].parents.push(moduleId);
/******/ 					if(me.children.indexOf(request) < 0)
/******/ 						me.children.push(request);
/******/ 				} else hotCurrentParents = [moduleId];
/******/ 			} else {
/******/ 				console.warn("[HMR] unexpected require(" + request + ") from disposed module " + moduleId);
/******/ 				hotCurrentParents = [];
/******/ 			}
/******/ 			return __webpack_require__(request);
/******/ 		};
/******/ 		for(var name in __webpack_require__) {
/******/ 			if(Object.prototype.hasOwnProperty.call(__webpack_require__, name)) {
/******/ 				if(canDefineProperty) {
/******/ 					Object.defineProperty(fn, name, (function(name) {
/******/ 						return {
/******/ 							configurable: true,
/******/ 							enumerable: true,
/******/ 							get: function() {
/******/ 								return __webpack_require__[name];
/******/ 							},
/******/ 							set: function(value) {
/******/ 								__webpack_require__[name] = value;
/******/ 							}
/******/ 						};
/******/ 					}(name)));
/******/ 				} else {
/******/ 					fn[name] = __webpack_require__[name];
/******/ 				}
/******/ 			}
/******/ 		}
/******/ 	
/******/ 		function ensure(chunkId, callback) {
/******/ 			if(hotStatus === "ready")
/******/ 				hotSetStatus("prepare");
/******/ 			hotChunksLoading++;
/******/ 			__webpack_require__.e(chunkId, function() {
/******/ 				try {
/******/ 					callback.call(null, fn);
/******/ 				} finally {
/******/ 					finishChunkLoading();
/******/ 				}
/******/ 	
/******/ 				function finishChunkLoading() {
/******/ 					hotChunksLoading--;
/******/ 					if(hotStatus === "prepare") {
/******/ 						if(!hotWaitingFilesMap[chunkId]) {
/******/ 							hotEnsureUpdateChunk(chunkId);
/******/ 						}
/******/ 						if(hotChunksLoading === 0 && hotWaitingFiles === 0) {
/******/ 							hotUpdateDownloaded();
/******/ 						}
/******/ 					}
/******/ 				}
/******/ 			});
/******/ 		}
/******/ 		if(canDefineProperty) {
/******/ 			Object.defineProperty(fn, "e", {
/******/ 				enumerable: true,
/******/ 				value: ensure
/******/ 			});
/******/ 		} else {
/******/ 			fn.e = ensure;
/******/ 		}
/******/ 		return fn;
/******/ 	}
/******/ 	
/******/ 	function hotCreateModule(moduleId) { // eslint-disable-line no-unused-vars
/******/ 		var hot = {
/******/ 			// private stuff
/******/ 			_acceptedDependencies: {},
/******/ 			_declinedDependencies: {},
/******/ 			_selfAccepted: false,
/******/ 			_selfDeclined: false,
/******/ 			_disposeHandlers: [],
/******/ 	
/******/ 			// Module API
/******/ 			active: true,
/******/ 			accept: function(dep, callback) {
/******/ 				if(typeof dep === "undefined")
/******/ 					hot._selfAccepted = true;
/******/ 				else if(typeof dep === "function")
/******/ 					hot._selfAccepted = dep;
/******/ 				else if(typeof dep === "object")
/******/ 					for(var i = 0; i < dep.length; i++)
/******/ 						hot._acceptedDependencies[dep[i]] = callback;
/******/ 				else
/******/ 					hot._acceptedDependencies[dep] = callback;
/******/ 			},
/******/ 			decline: function(dep) {
/******/ 				if(typeof dep === "undefined")
/******/ 					hot._selfDeclined = true;
/******/ 				else if(typeof dep === "number")
/******/ 					hot._declinedDependencies[dep] = true;
/******/ 				else
/******/ 					for(var i = 0; i < dep.length; i++)
/******/ 						hot._declinedDependencies[dep[i]] = true;
/******/ 			},
/******/ 			dispose: function(callback) {
/******/ 				hot._disposeHandlers.push(callback);
/******/ 			},
/******/ 			addDisposeHandler: function(callback) {
/******/ 				hot._disposeHandlers.push(callback);
/******/ 			},
/******/ 			removeDisposeHandler: function(callback) {
/******/ 				var idx = hot._disposeHandlers.indexOf(callback);
/******/ 				if(idx >= 0) hot._disposeHandlers.splice(idx, 1);
/******/ 			},
/******/ 	
/******/ 			// Management API
/******/ 			check: hotCheck,
/******/ 			apply: hotApply,
/******/ 			status: function(l) {
/******/ 				if(!l) return hotStatus;
/******/ 				hotStatusHandlers.push(l);
/******/ 			},
/******/ 			addStatusHandler: function(l) {
/******/ 				hotStatusHandlers.push(l);
/******/ 			},
/******/ 			removeStatusHandler: function(l) {
/******/ 				var idx = hotStatusHandlers.indexOf(l);
/******/ 				if(idx >= 0) hotStatusHandlers.splice(idx, 1);
/******/ 			},
/******/ 	
/******/ 			//inherit from previous dispose call
/******/ 			data: hotCurrentModuleData[moduleId]
/******/ 		};
/******/ 		return hot;
/******/ 	}
/******/ 	
/******/ 	var hotStatusHandlers = [];
/******/ 	var hotStatus = "idle";
/******/ 	
/******/ 	function hotSetStatus(newStatus) {
/******/ 		hotStatus = newStatus;
/******/ 		for(var i = 0; i < hotStatusHandlers.length; i++)
/******/ 			hotStatusHandlers[i].call(null, newStatus);
/******/ 	}
/******/ 	
/******/ 	// while downloading
/******/ 	var hotWaitingFiles = 0;
/******/ 	var hotChunksLoading = 0;
/******/ 	var hotWaitingFilesMap = {};
/******/ 	var hotRequestedFilesMap = {};
/******/ 	var hotAvailibleFilesMap = {};
/******/ 	var hotCallback;
/******/ 	
/******/ 	// The update info
/******/ 	var hotUpdate, hotUpdateNewHash;
/******/ 	
/******/ 	function toModuleId(id) {
/******/ 		var isNumber = (+id) + "" === id;
/******/ 		return isNumber ? +id : id;
/******/ 	}
/******/ 	
/******/ 	function hotCheck(apply, callback) {
/******/ 		if(hotStatus !== "idle") throw new Error("check() is only allowed in idle status");
/******/ 		if(typeof apply === "function") {
/******/ 			hotApplyOnUpdate = false;
/******/ 			callback = apply;
/******/ 		} else {
/******/ 			hotApplyOnUpdate = apply;
/******/ 			callback = callback || function(err) {
/******/ 				if(err) throw err;
/******/ 			};
/******/ 		}
/******/ 		hotSetStatus("check");
/******/ 		hotDownloadManifest(function(err, update) {
/******/ 			if(err) return callback(err);
/******/ 			if(!update) {
/******/ 				hotSetStatus("idle");
/******/ 				callback(null, null);
/******/ 				return;
/******/ 			}
/******/ 	
/******/ 			hotRequestedFilesMap = {};
/******/ 			hotAvailibleFilesMap = {};
/******/ 			hotWaitingFilesMap = {};
/******/ 			for(var i = 0; i < update.c.length; i++)
/******/ 				hotAvailibleFilesMap[update.c[i]] = true;
/******/ 			hotUpdateNewHash = update.h;
/******/ 	
/******/ 			hotSetStatus("prepare");
/******/ 			hotCallback = callback;
/******/ 			hotUpdate = {};
/******/ 			var chunkId = 0;
/******/ 			{ // eslint-disable-line no-lone-blocks
/******/ 				/*globals chunkId */
/******/ 				hotEnsureUpdateChunk(chunkId);
/******/ 			}
/******/ 			if(hotStatus === "prepare" && hotChunksLoading === 0 && hotWaitingFiles === 0) {
/******/ 				hotUpdateDownloaded();
/******/ 			}
/******/ 		});
/******/ 	}
/******/ 	
/******/ 	function hotAddUpdateChunk(chunkId, moreModules) { // eslint-disable-line no-unused-vars
/******/ 		if(!hotAvailibleFilesMap[chunkId] || !hotRequestedFilesMap[chunkId])
/******/ 			return;
/******/ 		hotRequestedFilesMap[chunkId] = false;
/******/ 		for(var moduleId in moreModules) {
/******/ 			if(Object.prototype.hasOwnProperty.call(moreModules, moduleId)) {
/******/ 				hotUpdate[moduleId] = moreModules[moduleId];
/******/ 			}
/******/ 		}
/******/ 		if(--hotWaitingFiles === 0 && hotChunksLoading === 0) {
/******/ 			hotUpdateDownloaded();
/******/ 		}
/******/ 	}
/******/ 	
/******/ 	function hotEnsureUpdateChunk(chunkId) {
/******/ 		if(!hotAvailibleFilesMap[chunkId]) {
/******/ 			hotWaitingFilesMap[chunkId] = true;
/******/ 		} else {
/******/ 			hotRequestedFilesMap[chunkId] = true;
/******/ 			hotWaitingFiles++;
/******/ 			hotDownloadUpdateChunk(chunkId);
/******/ 		}
/******/ 	}
/******/ 	
/******/ 	function hotUpdateDownloaded() {
/******/ 		hotSetStatus("ready");
/******/ 		var callback = hotCallback;
/******/ 		hotCallback = null;
/******/ 		if(!callback) return;
/******/ 		if(hotApplyOnUpdate) {
/******/ 			hotApply(hotApplyOnUpdate, callback);
/******/ 		} else {
/******/ 			var outdatedModules = [];
/******/ 			for(var id in hotUpdate) {
/******/ 				if(Object.prototype.hasOwnProperty.call(hotUpdate, id)) {
/******/ 					outdatedModules.push(toModuleId(id));
/******/ 				}
/******/ 			}
/******/ 			callback(null, outdatedModules);
/******/ 		}
/******/ 	}
/******/ 	
/******/ 	function hotApply(options, callback) {
/******/ 		if(hotStatus !== "ready") throw new Error("apply() is only allowed in ready status");
/******/ 		if(typeof options === "function") {
/******/ 			callback = options;
/******/ 			options = {};
/******/ 		} else if(options && typeof options === "object") {
/******/ 			callback = callback || function(err) {
/******/ 				if(err) throw err;
/******/ 			};
/******/ 		} else {
/******/ 			options = {};
/******/ 			callback = callback || function(err) {
/******/ 				if(err) throw err;
/******/ 			};
/******/ 		}
/******/ 	
/******/ 		function getAffectedStuff(module) {
/******/ 			var outdatedModules = [module];
/******/ 			var outdatedDependencies = {};
/******/ 	
/******/ 			var queue = outdatedModules.slice();
/******/ 			while(queue.length > 0) {
/******/ 				var moduleId = queue.pop();
/******/ 				var module = installedModules[moduleId];
/******/ 				if(!module || module.hot._selfAccepted)
/******/ 					continue;
/******/ 				if(module.hot._selfDeclined) {
/******/ 					return new Error("Aborted because of self decline: " + moduleId);
/******/ 				}
/******/ 				if(moduleId === 0) {
/******/ 					return;
/******/ 				}
/******/ 				for(var i = 0; i < module.parents.length; i++) {
/******/ 					var parentId = module.parents[i];
/******/ 					var parent = installedModules[parentId];
/******/ 					if(parent.hot._declinedDependencies[moduleId]) {
/******/ 						return new Error("Aborted because of declined dependency: " + moduleId + " in " + parentId);
/******/ 					}
/******/ 					if(outdatedModules.indexOf(parentId) >= 0) continue;
/******/ 					if(parent.hot._acceptedDependencies[moduleId]) {
/******/ 						if(!outdatedDependencies[parentId])
/******/ 							outdatedDependencies[parentId] = [];
/******/ 						addAllToSet(outdatedDependencies[parentId], [moduleId]);
/******/ 						continue;
/******/ 					}
/******/ 					delete outdatedDependencies[parentId];
/******/ 					outdatedModules.push(parentId);
/******/ 					queue.push(parentId);
/******/ 				}
/******/ 			}
/******/ 	
/******/ 			return [outdatedModules, outdatedDependencies];
/******/ 		}
/******/ 	
/******/ 		function addAllToSet(a, b) {
/******/ 			for(var i = 0; i < b.length; i++) {
/******/ 				var item = b[i];
/******/ 				if(a.indexOf(item) < 0)
/******/ 					a.push(item);
/******/ 			}
/******/ 		}
/******/ 	
/******/ 		// at begin all updates modules are outdated
/******/ 		// the "outdated" status can propagate to parents if they don't accept the children
/******/ 		var outdatedDependencies = {};
/******/ 		var outdatedModules = [];
/******/ 		var appliedUpdate = {};
/******/ 		for(var id in hotUpdate) {
/******/ 			if(Object.prototype.hasOwnProperty.call(hotUpdate, id)) {
/******/ 				var moduleId = toModuleId(id);
/******/ 				var result = getAffectedStuff(moduleId);
/******/ 				if(!result) {
/******/ 					if(options.ignoreUnaccepted)
/******/ 						continue;
/******/ 					hotSetStatus("abort");
/******/ 					return callback(new Error("Aborted because " + moduleId + " is not accepted"));
/******/ 				}
/******/ 				if(result instanceof Error) {
/******/ 					hotSetStatus("abort");
/******/ 					return callback(result);
/******/ 				}
/******/ 				appliedUpdate[moduleId] = hotUpdate[moduleId];
/******/ 				addAllToSet(outdatedModules, result[0]);
/******/ 				for(var moduleId in result[1]) {
/******/ 					if(Object.prototype.hasOwnProperty.call(result[1], moduleId)) {
/******/ 						if(!outdatedDependencies[moduleId])
/******/ 							outdatedDependencies[moduleId] = [];
/******/ 						addAllToSet(outdatedDependencies[moduleId], result[1][moduleId]);
/******/ 					}
/******/ 				}
/******/ 			}
/******/ 		}
/******/ 	
/******/ 		// Store self accepted outdated modules to require them later by the module system
/******/ 		var outdatedSelfAcceptedModules = [];
/******/ 		for(var i = 0; i < outdatedModules.length; i++) {
/******/ 			var moduleId = outdatedModules[i];
/******/ 			if(installedModules[moduleId] && installedModules[moduleId].hot._selfAccepted)
/******/ 				outdatedSelfAcceptedModules.push({
/******/ 					module: moduleId,
/******/ 					errorHandler: installedModules[moduleId].hot._selfAccepted
/******/ 				});
/******/ 		}
/******/ 	
/******/ 		// Now in "dispose" phase
/******/ 		hotSetStatus("dispose");
/******/ 		var queue = outdatedModules.slice();
/******/ 		while(queue.length > 0) {
/******/ 			var moduleId = queue.pop();
/******/ 			var module = installedModules[moduleId];
/******/ 			if(!module) continue;
/******/ 	
/******/ 			var data = {};
/******/ 	
/******/ 			// Call dispose handlers
/******/ 			var disposeHandlers = module.hot._disposeHandlers;
/******/ 			for(var j = 0; j < disposeHandlers.length; j++) {
/******/ 				var cb = disposeHandlers[j];
/******/ 				cb(data);
/******/ 			}
/******/ 			hotCurrentModuleData[moduleId] = data;
/******/ 	
/******/ 			// disable module (this disables requires from this module)
/******/ 			module.hot.active = false;
/******/ 	
/******/ 			// remove module from cache
/******/ 			delete installedModules[moduleId];
/******/ 	
/******/ 			// remove "parents" references from all children
/******/ 			for(var j = 0; j < module.children.length; j++) {
/******/ 				var child = installedModules[module.children[j]];
/******/ 				if(!child) continue;
/******/ 				var idx = child.parents.indexOf(moduleId);
/******/ 				if(idx >= 0) {
/******/ 					child.parents.splice(idx, 1);
/******/ 				}
/******/ 			}
/******/ 		}
/******/ 	
/******/ 		// remove outdated dependency from module children
/******/ 		for(var moduleId in outdatedDependencies) {
/******/ 			if(Object.prototype.hasOwnProperty.call(outdatedDependencies, moduleId)) {
/******/ 				var module = installedModules[moduleId];
/******/ 				var moduleOutdatedDependencies = outdatedDependencies[moduleId];
/******/ 				for(var j = 0; j < moduleOutdatedDependencies.length; j++) {
/******/ 					var dependency = moduleOutdatedDependencies[j];
/******/ 					var idx = module.children.indexOf(dependency);
/******/ 					if(idx >= 0) module.children.splice(idx, 1);
/******/ 				}
/******/ 			}
/******/ 		}
/******/ 	
/******/ 		// Not in "apply" phase
/******/ 		hotSetStatus("apply");
/******/ 	
/******/ 		hotCurrentHash = hotUpdateNewHash;
/******/ 	
/******/ 		// insert new code
/******/ 		for(var moduleId in appliedUpdate) {
/******/ 			if(Object.prototype.hasOwnProperty.call(appliedUpdate, moduleId)) {
/******/ 				modules[moduleId] = appliedUpdate[moduleId];
/******/ 			}
/******/ 		}
/******/ 	
/******/ 		// call accept handlers
/******/ 		var error = null;
/******/ 		for(var moduleId in outdatedDependencies) {
/******/ 			if(Object.prototype.hasOwnProperty.call(outdatedDependencies, moduleId)) {
/******/ 				var module = installedModules[moduleId];
/******/ 				var moduleOutdatedDependencies = outdatedDependencies[moduleId];
/******/ 				var callbacks = [];
/******/ 				for(var i = 0; i < moduleOutdatedDependencies.length; i++) {
/******/ 					var dependency = moduleOutdatedDependencies[i];
/******/ 					var cb = module.hot._acceptedDependencies[dependency];
/******/ 					if(callbacks.indexOf(cb) >= 0) continue;
/******/ 					callbacks.push(cb);
/******/ 				}
/******/ 				for(var i = 0; i < callbacks.length; i++) {
/******/ 					var cb = callbacks[i];
/******/ 					try {
/******/ 						cb(outdatedDependencies);
/******/ 					} catch(err) {
/******/ 						if(!error)
/******/ 							error = err;
/******/ 					}
/******/ 				}
/******/ 			}
/******/ 		}
/******/ 	
/******/ 		// Load self accepted modules
/******/ 		for(var i = 0; i < outdatedSelfAcceptedModules.length; i++) {
/******/ 			var item = outdatedSelfAcceptedModules[i];
/******/ 			var moduleId = item.module;
/******/ 			hotCurrentParents = [moduleId];
/******/ 			try {
/******/ 				__webpack_require__(moduleId);
/******/ 			} catch(err) {
/******/ 				if(typeof item.errorHandler === "function") {
/******/ 					try {
/******/ 						item.errorHandler(err);
/******/ 					} catch(err) {
/******/ 						if(!error)
/******/ 							error = err;
/******/ 					}
/******/ 				} else if(!error)
/******/ 					error = err;
/******/ 			}
/******/ 		}
/******/ 	
/******/ 		// handle errors in accept handlers and self accepted module load
/******/ 		if(error) {
/******/ 			hotSetStatus("fail");
/******/ 			return callback(error);
/******/ 		}
/******/ 	
/******/ 		hotSetStatus("idle");
/******/ 		callback(null, outdatedModules);
/******/ 	}
/******/
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId])
/******/ 			return installedModules[moduleId].exports;
/******/
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			exports: {},
/******/ 			id: moduleId,
/******/ 			loaded: false,
/******/ 			hot: hotCreateModule(moduleId),
/******/ 			parents: hotCurrentParents,
/******/ 			children: []
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, hotCreateRequire(moduleId));
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.loaded = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "/";
/******/
/******/ 	// __webpack_hash__
/******/ 	__webpack_require__.h = function() { return hotCurrentHash; };
/******/
/******/ 	// Load entry module and return exports
/******/ 	return hotCreateRequire(0)(0);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ function(module, exports, __webpack_require__) {

	__webpack_require__(1);
	module.exports = __webpack_require__(15);


/***/ },
/* 1 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(__resourceQuery, module) {/*eslint-env browser*/
	/*global __resourceQuery __webpack_public_path__*/
	
	var options = {
	  path: "/__webpack_hmr",
	  timeout: 20 * 1000,
	  overlay: true,
	  reload: false,
	  log: true,
	  warn: true,
	  name: ''
	};
	if (true) {
	  var querystring = __webpack_require__(3);
	  var overrides = querystring.parse(__resourceQuery.slice(1));
	  if (overrides.path) options.path = overrides.path;
	  if (overrides.timeout) options.timeout = overrides.timeout;
	  if (overrides.overlay) options.overlay = overrides.overlay !== 'false';
	  if (overrides.reload) options.reload = overrides.reload !== 'false';
	  if (overrides.noInfo && overrides.noInfo !== 'false') {
	    options.log = false;
	  }
	  if (overrides.name) {
	    options.name = overrides.name 
	  }
	  if (overrides.quiet && overrides.quiet !== 'false') {
	    options.log = false;
	    options.warn = false;
	  }
	  if (overrides.dynamicPublicPath) {
	    options.path = __webpack_require__.p + options.path;
	  }
	}
	
	if (typeof window === 'undefined') {
	  // do nothing
	} else if (typeof window.EventSource === 'undefined') {
	  console.warn(
	    "webpack-hot-middleware's client requires EventSource to work. " +
	    "You should include a polyfill if you want to support this browser: " +
	    "https://developer.mozilla.org/en-US/docs/Web/API/Server-sent_events#Tools"
	  );
	} else {
	  connect(window.EventSource);
	}
	
	function connect(EventSource) {
	  var source = new EventSource(options.path);
	  var lastActivity = new Date();
	
	  source.onopen = handleOnline;
	  source.onmessage = handleMessage;
	  source.onerror = handleDisconnect;
	
	  var timer = setInterval(function() {
	    if ((new Date() - lastActivity) > options.timeout) {
	      handleDisconnect();
	    }
	  }, options.timeout / 2);
	
	  function handleOnline() {
	    if (options.log) console.log("[HMR] connected");
	    lastActivity = new Date();
	  }
	
	  function handleMessage(event) {
	    lastActivity = new Date();
	    if (event.data == "\uD83D\uDC93") {
	      return;
	    }
	    try {
	      processMessage(JSON.parse(event.data));
	    } catch (ex) {
	      if (options.warn) {
	        console.warn("Invalid HMR message: " + event.data + "\n" + ex);
	      }
	    }
	  }
	
	  function handleDisconnect() {
	    clearInterval(timer);
	    source.close();
	    setTimeout(function() { connect(EventSource); }, options.timeout);
	  }
	
	}
	
	var reporter;
	// the reporter needs to be a singleton on the page
	// in case the client is being used by mutliple bundles
	// we only want to report once.
	// all the errors will go to all clients
	var singletonKey = '__webpack_hot_middleware_reporter__';
	if (typeof window !== 'undefined' && !window[singletonKey]) {
	  reporter = window[singletonKey] = createReporter();
	}
	
	function createReporter() {
	  var strip = __webpack_require__(6);
	
	  var overlay;
	  if (typeof document !== 'undefined' && options.overlay) {
	    overlay = __webpack_require__(8);
	  }
	
	
	  var previousProblems = null;
	
	  return {
	    cleanProblemsCache: function () {
	      previousProblems = null;
	    },
	    problems: function(type, obj) {
	      if (options.warn) {
	        var newProblems = obj[type].map(function(msg) { return strip(msg); }).join('\n');
	
	        if (previousProblems !== newProblems) {
	          previousProblems = newProblems;
	          console.warn("[HMR] bundle has " + type + ":\n" + newProblems);
	        }
	      }
	      if (overlay && type !== 'warnings') overlay.showProblems(type, obj[type]);
	    },
	    success: function() {
	      if (overlay) overlay.clear();
	    },
	    useCustomOverlay: function(customOverlay) {
	      overlay = customOverlay;
	    }
	  };
	}
	
	var processUpdate = __webpack_require__(14);
	
	var customHandler;
	var subscribeAllHandler;
	function processMessage(obj) {
	  switch(obj.action) {
	    case "building":
	      if (options.log) console.log("[HMR] bundle rebuilding");
	      break;
	    case "built":
	      if (options.log) {
	        console.log(
	          "[HMR] bundle " + (obj.name ? obj.name + " " : "") +
	          "rebuilt in " + obj.time + "ms"
	        );
	      }
	      // fall through
	    case "sync":
	      if (obj.name && options.name && obj.name !== options.name) {
	        return;
	      }
	      if (obj.errors.length > 0) {
	        if (reporter) reporter.problems('errors', obj);
	      } else {
	        if (reporter) {
	          if (obj.warnings.length > 0) {
	            reporter.problems('warnings', obj);
	          } else {
	            reporter.cleanProblemsCache();
	          }
	          reporter.success();
	        }
	        processUpdate(obj.hash, obj.modules, options);
	      }
	      break;
	    default:
	      if (customHandler) {
	        customHandler(obj);
	      }
	  }
	
	  if (subscribeAllHandler) {
	    subscribeAllHandler(obj);
	  }
	}
	
	if (module) {
	  module.exports = {
	    subscribeAll: function subscribeAll(handler) {
	      subscribeAllHandler = handler;
	    },
	    subscribe: function subscribe(handler) {
	      customHandler = handler;
	    },
	    useCustomOverlay: function useCustomOverlay(customOverlay) {
	      if (reporter) reporter.useCustomOverlay(customOverlay);
	    }
	  };
	}
	
	/* WEBPACK VAR INJECTION */}.call(exports, "?path=%2F__webpack_hmr", __webpack_require__(2)(module)))

/***/ },
/* 2 */
/***/ function(module, exports) {

	module.exports = function(module) {
		if(!module.webpackPolyfill) {
			module.deprecate = function() {};
			module.paths = [];
			// module.parent = undefined by default
			module.children = [];
			module.webpackPolyfill = 1;
		}
		return module;
	}


/***/ },
/* 3 */
/***/ function(module, exports, __webpack_require__) {

	'use strict';
	
	exports.decode = exports.parse = __webpack_require__(4);
	exports.encode = exports.stringify = __webpack_require__(5);


/***/ },
/* 4 */
/***/ function(module, exports) {

	// Copyright Joyent, Inc. and other Node contributors.
	//
	// Permission is hereby granted, free of charge, to any person obtaining a
	// copy of this software and associated documentation files (the
	// "Software"), to deal in the Software without restriction, including
	// without limitation the rights to use, copy, modify, merge, publish,
	// distribute, sublicense, and/or sell copies of the Software, and to permit
	// persons to whom the Software is furnished to do so, subject to the
	// following conditions:
	//
	// The above copyright notice and this permission notice shall be included
	// in all copies or substantial portions of the Software.
	//
	// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
	// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
	// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN
	// NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
	// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
	// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
	// USE OR OTHER DEALINGS IN THE SOFTWARE.
	
	'use strict';
	
	// If obj.hasOwnProperty has been overridden, then calling
	// obj.hasOwnProperty(prop) will break.
	// See: https://github.com/joyent/node/issues/1707
	function hasOwnProperty(obj, prop) {
	  return Object.prototype.hasOwnProperty.call(obj, prop);
	}
	
	module.exports = function(qs, sep, eq, options) {
	  sep = sep || '&';
	  eq = eq || '=';
	  var obj = {};
	
	  if (typeof qs !== 'string' || qs.length === 0) {
	    return obj;
	  }
	
	  var regexp = /\+/g;
	  qs = qs.split(sep);
	
	  var maxKeys = 1000;
	  if (options && typeof options.maxKeys === 'number') {
	    maxKeys = options.maxKeys;
	  }
	
	  var len = qs.length;
	  // maxKeys <= 0 means that we should not limit keys count
	  if (maxKeys > 0 && len > maxKeys) {
	    len = maxKeys;
	  }
	
	  for (var i = 0; i < len; ++i) {
	    var x = qs[i].replace(regexp, '%20'),
	        idx = x.indexOf(eq),
	        kstr, vstr, k, v;
	
	    if (idx >= 0) {
	      kstr = x.substr(0, idx);
	      vstr = x.substr(idx + 1);
	    } else {
	      kstr = x;
	      vstr = '';
	    }
	
	    k = decodeURIComponent(kstr);
	    v = decodeURIComponent(vstr);
	
	    if (!hasOwnProperty(obj, k)) {
	      obj[k] = v;
	    } else if (Array.isArray(obj[k])) {
	      obj[k].push(v);
	    } else {
	      obj[k] = [obj[k], v];
	    }
	  }
	
	  return obj;
	};


/***/ },
/* 5 */
/***/ function(module, exports) {

	// Copyright Joyent, Inc. and other Node contributors.
	//
	// Permission is hereby granted, free of charge, to any person obtaining a
	// copy of this software and associated documentation files (the
	// "Software"), to deal in the Software without restriction, including
	// without limitation the rights to use, copy, modify, merge, publish,
	// distribute, sublicense, and/or sell copies of the Software, and to permit
	// persons to whom the Software is furnished to do so, subject to the
	// following conditions:
	//
	// The above copyright notice and this permission notice shall be included
	// in all copies or substantial portions of the Software.
	//
	// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
	// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
	// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN
	// NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
	// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
	// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
	// USE OR OTHER DEALINGS IN THE SOFTWARE.
	
	'use strict';
	
	var stringifyPrimitive = function(v) {
	  switch (typeof v) {
	    case 'string':
	      return v;
	
	    case 'boolean':
	      return v ? 'true' : 'false';
	
	    case 'number':
	      return isFinite(v) ? v : '';
	
	    default:
	      return '';
	  }
	};
	
	module.exports = function(obj, sep, eq, name) {
	  sep = sep || '&';
	  eq = eq || '=';
	  if (obj === null) {
	    obj = undefined;
	  }
	
	  if (typeof obj === 'object') {
	    return Object.keys(obj).map(function(k) {
	      var ks = encodeURIComponent(stringifyPrimitive(k)) + eq;
	      if (Array.isArray(obj[k])) {
	        return obj[k].map(function(v) {
	          return ks + encodeURIComponent(stringifyPrimitive(v));
	        }).join(sep);
	      } else {
	        return ks + encodeURIComponent(stringifyPrimitive(obj[k]));
	      }
	    }).join(sep);
	
	  }
	
	  if (!name) return '';
	  return encodeURIComponent(stringifyPrimitive(name)) + eq +
	         encodeURIComponent(stringifyPrimitive(obj));
	};


/***/ },
/* 6 */
/***/ function(module, exports, __webpack_require__) {

	'use strict';
	var ansiRegex = __webpack_require__(7)();
	
	module.exports = function (str) {
		return typeof str === 'string' ? str.replace(ansiRegex, '') : str;
	};


/***/ },
/* 7 */
/***/ function(module, exports) {

	'use strict';
	module.exports = function () {
		return /[\u001b\u009b][[()#;?]*(?:[0-9]{1,4}(?:;[0-9]{0,4})*)?[0-9A-ORZcf-nqry=><]/g;
	};


/***/ },
/* 8 */
/***/ function(module, exports, __webpack_require__) {

	/*eslint-env browser*/
	
	var clientOverlay = document.createElement('div');
	var styles = {
	  background: 'rgba(0,0,0,0.85)',
	  color: '#E8E8E8',
	  lineHeight: '1.2',
	  whiteSpace: 'pre',
	  fontFamily: 'Menlo, Consolas, monospace',
	  fontSize: '13px',
	  position: 'fixed',
	  zIndex: 9999,
	  padding: '10px',
	  left: 0,
	  right: 0,
	  top: 0,
	  bottom: 0,
	  overflow: 'auto',
	  dir: 'ltr'
	};
	for (var key in styles) {
	  clientOverlay.style[key] = styles[key];
	}
	
	var ansiHTML = __webpack_require__(9);
	var colors = {
	  reset: ['transparent', 'transparent'],
	  black: '181818',
	  red: 'E36049',
	  green: 'B3CB74',
	  yellow: 'FFD080',
	  blue: '7CAFC2',
	  magenta: '7FACCA',
	  cyan: 'C3C2EF',
	  lightgrey: 'EBE7E3',
	  darkgrey: '6D7891'
	};
	ansiHTML.setColors(colors);
	
	var Entities = __webpack_require__(10).AllHtmlEntities;
	var entities = new Entities();
	
	exports.showProblems =
	function showProblems(type, lines) {
	  clientOverlay.innerHTML = '';
	  lines.forEach(function(msg) {
	    msg = ansiHTML(entities.encode(msg));
	    var div = document.createElement('div');
	    div.style.marginBottom = '26px';
	    div.innerHTML = problemType(type) + ' in ' + msg;
	    clientOverlay.appendChild(div);
	  });
	  if (document.body) {
	    document.body.appendChild(clientOverlay);
	  }
	};
	
	exports.clear =
	function clear() {
	  if (document.body && clientOverlay.parentNode) {
	    document.body.removeChild(clientOverlay);
	  }
	};
	
	var problemColors = {
	  errors: colors.red,
	  warnings: colors.yellow
	};
	
	function problemType (type) {
	  var color = problemColors[type] || colors.red;
	  return (
	    '<span style="background-color:#' + color + '; color:#fff; padding:2px 4px; border-radius: 2px">' +
	      type.slice(0, -1).toUpperCase() +
	    '</span>'
	  );
	}


/***/ },
/* 9 */
/***/ function(module, exports) {

	'use strict'
	
	module.exports = ansiHTML
	
	// Reference to https://github.com/sindresorhus/ansi-regex
	var _regANSI = /(?:(?:\u001b\[)|\u009b)(?:(?:[0-9]{1,3})?(?:(?:;[0-9]{0,3})*)?[A-M|f-m])|\u001b[A-M]/
	
	var _defColors = {
	  reset: ['fff', '000'], // [FOREGROUD_COLOR, BACKGROUND_COLOR]
	  black: '000',
	  red: 'ff0000',
	  green: '209805',
	  yellow: 'e8bf03',
	  blue: '0000ff',
	  magenta: 'ff00ff',
	  cyan: '00ffee',
	  lightgrey: 'f0f0f0',
	  darkgrey: '888'
	}
	var _styles = {
	  30: 'black',
	  31: 'red',
	  32: 'green',
	  33: 'yellow',
	  34: 'blue',
	  35: 'magenta',
	  36: 'cyan',
	  37: 'lightgrey'
	}
	var _openTags = {
	  '1': 'font-weight:bold', // bold
	  '2': 'opacity:0.8', // dim
	  '3': '<i>', // italic
	  '4': '<u>', // underscore
	  '8': 'display:none', // hidden
	  '9': '<del>' // delete
	}
	var _closeTags = {
	  '23': '</i>', // reset italic
	  '24': '</u>', // reset underscore
	  '29': '</del>' // reset delete
	}
	
	;[0, 21, 22, 27, 28, 39, 49].forEach(function (n) {
	  _closeTags[n] = '</span>'
	})
	
	/**
	 * Converts text with ANSI color codes to HTML markup.
	 * @param {String} text
	 * @returns {*}
	 */
	function ansiHTML (text) {
	  // Returns the text if the string has no ANSI escape code.
	  if (!_regANSI.test(text)) {
	    return text
	  }
	
	  // Cache opened sequence.
	  var ansiCodes = []
	  // Replace with markup.
	  var ret = text.replace(/\033\[(\d+)*m/g, function (match, seq) {
	    var ot = _openTags[seq]
	    if (ot) {
	      // If current sequence has been opened, close it.
	      if (!!~ansiCodes.indexOf(seq)) { // eslint-disable-line no-extra-boolean-cast
	        ansiCodes.pop()
	        return '</span>'
	      }
	      // Open tag.
	      ansiCodes.push(seq)
	      return ot[0] === '<' ? ot : '<span style="' + ot + ';">'
	    }
	
	    var ct = _closeTags[seq]
	    if (ct) {
	      // Pop sequence
	      ansiCodes.pop()
	      return ct
	    }
	    return ''
	  })
	
	  // Make sure tags are closed.
	  var l = ansiCodes.length
	  ;(l > 0) && (ret += Array(l + 1).join('</span>'))
	
	  return ret
	}
	
	/**
	 * Customize colors.
	 * @param {Object} colors reference to _defColors
	 */
	ansiHTML.setColors = function (colors) {
	  if (typeof colors !== 'object') {
	    throw new Error('`colors` parameter must be an Object.')
	  }
	
	  var _finalColors = {}
	  for (var key in _defColors) {
	    var hex = colors.hasOwnProperty(key) ? colors[key] : null
	    if (!hex) {
	      _finalColors[key] = _defColors[key]
	      continue
	    }
	    if ('reset' === key) {
	      if (typeof hex === 'string') {
	        hex = [hex]
	      }
	      if (!Array.isArray(hex) || hex.length === 0 || hex.some(function (h) {
	        return typeof h !== 'string'
	      })) {
	        throw new Error('The value of `' + key + '` property must be an Array and each item could only be a hex string, e.g.: FF0000')
	      }
	      var defHexColor = _defColors[key]
	      if (!hex[0]) {
	        hex[0] = defHexColor[0]
	      }
	      if (hex.length === 1 || !hex[1]) {
	        hex = [hex[0]]
	        hex.push(defHexColor[1])
	      }
	
	      hex = hex.slice(0, 2)
	    } else if (typeof hex !== 'string') {
	      throw new Error('The value of `' + key + '` property must be a hex string, e.g.: FF0000')
	    }
	    _finalColors[key] = hex
	  }
	  _setTags(_finalColors)
	}
	
	/**
	 * Reset colors.
	 */
	ansiHTML.reset = function () {
	  _setTags(_defColors)
	}
	
	/**
	 * Expose tags, including open and close.
	 * @type {Object}
	 */
	ansiHTML.tags = {}
	
	if (Object.defineProperty) {
	  Object.defineProperty(ansiHTML.tags, 'open', {
	    get: function () { return _openTags }
	  })
	  Object.defineProperty(ansiHTML.tags, 'close', {
	    get: function () { return _closeTags }
	  })
	} else {
	  ansiHTML.tags.open = _openTags
	  ansiHTML.tags.close = _closeTags
	}
	
	function _setTags (colors) {
	  // reset all
	  _openTags['0'] = 'font-weight:normal;opacity:1;color:#' + colors.reset[0] + ';background:#' + colors.reset[1]
	  // inverse
	  _openTags['7'] = 'color:#' + colors.reset[1] + ';background:#' + colors.reset[0]
	  // dark grey
	  _openTags['90'] = 'color:#' + colors.darkgrey
	
	  for (var code in _styles) {
	    var color = _styles[code]
	    var oriColor = colors[color] || '000'
	    _openTags[code] = 'color:#' + oriColor
	    code = parseInt(code)
	    _openTags[(code + 10).toString()] = 'background:#' + oriColor
	  }
	}
	
	ansiHTML.reset()


/***/ },
/* 10 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = {
	  XmlEntities: __webpack_require__(11),
	  Html4Entities: __webpack_require__(12),
	  Html5Entities: __webpack_require__(13),
	  AllHtmlEntities: __webpack_require__(13)
	};


/***/ },
/* 11 */
/***/ function(module, exports) {

	var ALPHA_INDEX = {
	    '&lt': '<',
	    '&gt': '>',
	    '&quot': '"',
	    '&apos': '\'',
	    '&amp': '&',
	    '&lt;': '<',
	    '&gt;': '>',
	    '&quot;': '"',
	    '&apos;': '\'',
	    '&amp;': '&'
	};
	
	var CHAR_INDEX = {
	    60: 'lt',
	    62: 'gt',
	    34: 'quot',
	    39: 'apos',
	    38: 'amp'
	};
	
	var CHAR_S_INDEX = {
	    '<': '&lt;',
	    '>': '&gt;',
	    '"': '&quot;',
	    '\'': '&apos;',
	    '&': '&amp;'
	};
	
	/**
	 * @constructor
	 */
	function XmlEntities() {}
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	XmlEntities.prototype.encode = function(str) {
	    if (str.length === 0) {
	        return '';
	    }
	    return str.replace(/<|>|"|'|&/g, function(s) {
	        return CHAR_S_INDEX[s];
	    });
	};
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	 XmlEntities.encode = function(str) {
	    return new XmlEntities().encode(str);
	 };
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	XmlEntities.prototype.decode = function(str) {
	    if (str.length === 0) {
	        return '';
	    }
	    return str.replace(/&#?[0-9a-zA-Z]+;?/g, function(s) {
	        if (s.charAt(1) === '#') {
	            var code = s.charAt(2).toLowerCase() === 'x' ?
	                parseInt(s.substr(3), 16) :
	                parseInt(s.substr(2));
	
	            if (isNaN(code) || code < -32768 || code > 65535) {
	                return '';
	            }
	            return String.fromCharCode(code);
	        }
	        return ALPHA_INDEX[s] || s;
	    });
	};
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	 XmlEntities.decode = function(str) {
	    return new XmlEntities().decode(str);
	 };
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	XmlEntities.prototype.encodeNonUTF = function(str) {
	    var strLength = str.length;
	    if (strLength === 0) {
	        return '';
	    }
	    var result = '';
	    var i = 0;
	    while (i < strLength) {
	        var c = str.charCodeAt(i);
	        var alpha = CHAR_INDEX[c];
	        if (alpha) {
	            result += "&" + alpha + ";";
	            i++;
	            continue;
	        }
	        if (c < 32 || c > 126) {
	            result += '&#' + c + ';';
	        } else {
	            result += str.charAt(i);
	        }
	        i++;
	    }
	    return result;
	};
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	 XmlEntities.encodeNonUTF = function(str) {
	    return new XmlEntities().encodeNonUTF(str);
	 };
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	XmlEntities.prototype.encodeNonASCII = function(str) {
	    var strLenght = str.length;
	    if (strLenght === 0) {
	        return '';
	    }
	    var result = '';
	    var i = 0;
	    while (i < strLenght) {
	        var c = str.charCodeAt(i);
	        if (c <= 255) {
	            result += str[i++];
	            continue;
	        }
	        result += '&#' + c + ';';
	        i++;
	    }
	    return result;
	};
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	 XmlEntities.encodeNonASCII = function(str) {
	    return new XmlEntities().encodeNonASCII(str);
	 };
	
	module.exports = XmlEntities;


/***/ },
/* 12 */
/***/ function(module, exports) {

	var HTML_ALPHA = ['apos', 'nbsp', 'iexcl', 'cent', 'pound', 'curren', 'yen', 'brvbar', 'sect', 'uml', 'copy', 'ordf', 'laquo', 'not', 'shy', 'reg', 'macr', 'deg', 'plusmn', 'sup2', 'sup3', 'acute', 'micro', 'para', 'middot', 'cedil', 'sup1', 'ordm', 'raquo', 'frac14', 'frac12', 'frac34', 'iquest', 'Agrave', 'Aacute', 'Acirc', 'Atilde', 'Auml', 'Aring', 'Aelig', 'Ccedil', 'Egrave', 'Eacute', 'Ecirc', 'Euml', 'Igrave', 'Iacute', 'Icirc', 'Iuml', 'ETH', 'Ntilde', 'Ograve', 'Oacute', 'Ocirc', 'Otilde', 'Ouml', 'times', 'Oslash', 'Ugrave', 'Uacute', 'Ucirc', 'Uuml', 'Yacute', 'THORN', 'szlig', 'agrave', 'aacute', 'acirc', 'atilde', 'auml', 'aring', 'aelig', 'ccedil', 'egrave', 'eacute', 'ecirc', 'euml', 'igrave', 'iacute', 'icirc', 'iuml', 'eth', 'ntilde', 'ograve', 'oacute', 'ocirc', 'otilde', 'ouml', 'divide', 'Oslash', 'ugrave', 'uacute', 'ucirc', 'uuml', 'yacute', 'thorn', 'yuml', 'quot', 'amp', 'lt', 'gt', 'oelig', 'oelig', 'scaron', 'scaron', 'yuml', 'circ', 'tilde', 'ensp', 'emsp', 'thinsp', 'zwnj', 'zwj', 'lrm', 'rlm', 'ndash', 'mdash', 'lsquo', 'rsquo', 'sbquo', 'ldquo', 'rdquo', 'bdquo', 'dagger', 'dagger', 'permil', 'lsaquo', 'rsaquo', 'euro', 'fnof', 'alpha', 'beta', 'gamma', 'delta', 'epsilon', 'zeta', 'eta', 'theta', 'iota', 'kappa', 'lambda', 'mu', 'nu', 'xi', 'omicron', 'pi', 'rho', 'sigma', 'tau', 'upsilon', 'phi', 'chi', 'psi', 'omega', 'alpha', 'beta', 'gamma', 'delta', 'epsilon', 'zeta', 'eta', 'theta', 'iota', 'kappa', 'lambda', 'mu', 'nu', 'xi', 'omicron', 'pi', 'rho', 'sigmaf', 'sigma', 'tau', 'upsilon', 'phi', 'chi', 'psi', 'omega', 'thetasym', 'upsih', 'piv', 'bull', 'hellip', 'prime', 'prime', 'oline', 'frasl', 'weierp', 'image', 'real', 'trade', 'alefsym', 'larr', 'uarr', 'rarr', 'darr', 'harr', 'crarr', 'larr', 'uarr', 'rarr', 'darr', 'harr', 'forall', 'part', 'exist', 'empty', 'nabla', 'isin', 'notin', 'ni', 'prod', 'sum', 'minus', 'lowast', 'radic', 'prop', 'infin', 'ang', 'and', 'or', 'cap', 'cup', 'int', 'there4', 'sim', 'cong', 'asymp', 'ne', 'equiv', 'le', 'ge', 'sub', 'sup', 'nsub', 'sube', 'supe', 'oplus', 'otimes', 'perp', 'sdot', 'lceil', 'rceil', 'lfloor', 'rfloor', 'lang', 'rang', 'loz', 'spades', 'clubs', 'hearts', 'diams'];
	var HTML_CODES = [39, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 34, 38, 60, 62, 338, 339, 352, 353, 376, 710, 732, 8194, 8195, 8201, 8204, 8205, 8206, 8207, 8211, 8212, 8216, 8217, 8218, 8220, 8221, 8222, 8224, 8225, 8240, 8249, 8250, 8364, 402, 913, 914, 915, 916, 917, 918, 919, 920, 921, 922, 923, 924, 925, 926, 927, 928, 929, 931, 932, 933, 934, 935, 936, 937, 945, 946, 947, 948, 949, 950, 951, 952, 953, 954, 955, 956, 957, 958, 959, 960, 961, 962, 963, 964, 965, 966, 967, 968, 969, 977, 978, 982, 8226, 8230, 8242, 8243, 8254, 8260, 8472, 8465, 8476, 8482, 8501, 8592, 8593, 8594, 8595, 8596, 8629, 8656, 8657, 8658, 8659, 8660, 8704, 8706, 8707, 8709, 8711, 8712, 8713, 8715, 8719, 8721, 8722, 8727, 8730, 8733, 8734, 8736, 8743, 8744, 8745, 8746, 8747, 8756, 8764, 8773, 8776, 8800, 8801, 8804, 8805, 8834, 8835, 8836, 8838, 8839, 8853, 8855, 8869, 8901, 8968, 8969, 8970, 8971, 9001, 9002, 9674, 9824, 9827, 9829, 9830];
	
	var alphaIndex = {};
	var numIndex = {};
	
	var i = 0;
	var length = HTML_ALPHA.length;
	while (i < length) {
	    var a = HTML_ALPHA[i];
	    var c = HTML_CODES[i];
	    alphaIndex[a] = String.fromCharCode(c);
	    numIndex[c] = a;
	    i++;
	}
	
	/**
	 * @constructor
	 */
	function Html4Entities() {}
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	Html4Entities.prototype.decode = function(str) {
	    if (str.length === 0) {
	        return '';
	    }
	    return str.replace(/&(#?[\w\d]+);?/g, function(s, entity) {
	        var chr;
	        if (entity.charAt(0) === "#") {
	            var code = entity.charAt(1).toLowerCase() === 'x' ?
	                parseInt(entity.substr(2), 16) :
	                parseInt(entity.substr(1));
	
	            if (!(isNaN(code) || code < -32768 || code > 65535)) {
	                chr = String.fromCharCode(code);
	            }
	        } else {
	            chr = alphaIndex[entity];
	        }
	        return chr || s;
	    });
	};
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	Html4Entities.decode = function(str) {
	    return new Html4Entities().decode(str);
	};
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	Html4Entities.prototype.encode = function(str) {
	    var strLength = str.length;
	    if (strLength === 0) {
	        return '';
	    }
	    var result = '';
	    var i = 0;
	    while (i < strLength) {
	        var alpha = numIndex[str.charCodeAt(i)];
	        result += alpha ? "&" + alpha + ";" : str.charAt(i);
	        i++;
	    }
	    return result;
	};
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	Html4Entities.encode = function(str) {
	    return new Html4Entities().encode(str);
	};
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	Html4Entities.prototype.encodeNonUTF = function(str) {
	    var strLength = str.length;
	    if (strLength === 0) {
	        return '';
	    }
	    var result = '';
	    var i = 0;
	    while (i < strLength) {
	        var cc = str.charCodeAt(i);
	        var alpha = numIndex[cc];
	        if (alpha) {
	            result += "&" + alpha + ";";
	        } else if (cc < 32 || cc > 126) {
	            result += "&#" + cc + ";";
	        } else {
	            result += str.charAt(i);
	        }
	        i++;
	    }
	    return result;
	};
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	Html4Entities.encodeNonUTF = function(str) {
	    return new Html4Entities().encodeNonUTF(str);
	};
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	Html4Entities.prototype.encodeNonASCII = function(str) {
	    var strLength = str.length;
	    if (strLength === 0) {
	        return '';
	    }
	    var result = '';
	    var i = 0;
	    while (i < strLength) {
	        var c = str.charCodeAt(i);
	        if (c <= 255) {
	            result += str[i++];
	            continue;
	        }
	        result += '&#' + c + ';';
	        i++;
	    }
	    return result;
	};
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	Html4Entities.encodeNonASCII = function(str) {
	    return new Html4Entities().encodeNonASCII(str);
	};
	
	module.exports = Html4Entities;


/***/ },
/* 13 */
/***/ function(module, exports) {

	var ENTITIES = [['Aacute', [193]], ['aacute', [225]], ['Abreve', [258]], ['abreve', [259]], ['ac', [8766]], ['acd', [8767]], ['acE', [8766, 819]], ['Acirc', [194]], ['acirc', [226]], ['acute', [180]], ['Acy', [1040]], ['acy', [1072]], ['AElig', [198]], ['aelig', [230]], ['af', [8289]], ['Afr', [120068]], ['afr', [120094]], ['Agrave', [192]], ['agrave', [224]], ['alefsym', [8501]], ['aleph', [8501]], ['Alpha', [913]], ['alpha', [945]], ['Amacr', [256]], ['amacr', [257]], ['amalg', [10815]], ['amp', [38]], ['AMP', [38]], ['andand', [10837]], ['And', [10835]], ['and', [8743]], ['andd', [10844]], ['andslope', [10840]], ['andv', [10842]], ['ang', [8736]], ['ange', [10660]], ['angle', [8736]], ['angmsdaa', [10664]], ['angmsdab', [10665]], ['angmsdac', [10666]], ['angmsdad', [10667]], ['angmsdae', [10668]], ['angmsdaf', [10669]], ['angmsdag', [10670]], ['angmsdah', [10671]], ['angmsd', [8737]], ['angrt', [8735]], ['angrtvb', [8894]], ['angrtvbd', [10653]], ['angsph', [8738]], ['angst', [197]], ['angzarr', [9084]], ['Aogon', [260]], ['aogon', [261]], ['Aopf', [120120]], ['aopf', [120146]], ['apacir', [10863]], ['ap', [8776]], ['apE', [10864]], ['ape', [8778]], ['apid', [8779]], ['apos', [39]], ['ApplyFunction', [8289]], ['approx', [8776]], ['approxeq', [8778]], ['Aring', [197]], ['aring', [229]], ['Ascr', [119964]], ['ascr', [119990]], ['Assign', [8788]], ['ast', [42]], ['asymp', [8776]], ['asympeq', [8781]], ['Atilde', [195]], ['atilde', [227]], ['Auml', [196]], ['auml', [228]], ['awconint', [8755]], ['awint', [10769]], ['backcong', [8780]], ['backepsilon', [1014]], ['backprime', [8245]], ['backsim', [8765]], ['backsimeq', [8909]], ['Backslash', [8726]], ['Barv', [10983]], ['barvee', [8893]], ['barwed', [8965]], ['Barwed', [8966]], ['barwedge', [8965]], ['bbrk', [9141]], ['bbrktbrk', [9142]], ['bcong', [8780]], ['Bcy', [1041]], ['bcy', [1073]], ['bdquo', [8222]], ['becaus', [8757]], ['because', [8757]], ['Because', [8757]], ['bemptyv', [10672]], ['bepsi', [1014]], ['bernou', [8492]], ['Bernoullis', [8492]], ['Beta', [914]], ['beta', [946]], ['beth', [8502]], ['between', [8812]], ['Bfr', [120069]], ['bfr', [120095]], ['bigcap', [8898]], ['bigcirc', [9711]], ['bigcup', [8899]], ['bigodot', [10752]], ['bigoplus', [10753]], ['bigotimes', [10754]], ['bigsqcup', [10758]], ['bigstar', [9733]], ['bigtriangledown', [9661]], ['bigtriangleup', [9651]], ['biguplus', [10756]], ['bigvee', [8897]], ['bigwedge', [8896]], ['bkarow', [10509]], ['blacklozenge', [10731]], ['blacksquare', [9642]], ['blacktriangle', [9652]], ['blacktriangledown', [9662]], ['blacktriangleleft', [9666]], ['blacktriangleright', [9656]], ['blank', [9251]], ['blk12', [9618]], ['blk14', [9617]], ['blk34', [9619]], ['block', [9608]], ['bne', [61, 8421]], ['bnequiv', [8801, 8421]], ['bNot', [10989]], ['bnot', [8976]], ['Bopf', [120121]], ['bopf', [120147]], ['bot', [8869]], ['bottom', [8869]], ['bowtie', [8904]], ['boxbox', [10697]], ['boxdl', [9488]], ['boxdL', [9557]], ['boxDl', [9558]], ['boxDL', [9559]], ['boxdr', [9484]], ['boxdR', [9554]], ['boxDr', [9555]], ['boxDR', [9556]], ['boxh', [9472]], ['boxH', [9552]], ['boxhd', [9516]], ['boxHd', [9572]], ['boxhD', [9573]], ['boxHD', [9574]], ['boxhu', [9524]], ['boxHu', [9575]], ['boxhU', [9576]], ['boxHU', [9577]], ['boxminus', [8863]], ['boxplus', [8862]], ['boxtimes', [8864]], ['boxul', [9496]], ['boxuL', [9563]], ['boxUl', [9564]], ['boxUL', [9565]], ['boxur', [9492]], ['boxuR', [9560]], ['boxUr', [9561]], ['boxUR', [9562]], ['boxv', [9474]], ['boxV', [9553]], ['boxvh', [9532]], ['boxvH', [9578]], ['boxVh', [9579]], ['boxVH', [9580]], ['boxvl', [9508]], ['boxvL', [9569]], ['boxVl', [9570]], ['boxVL', [9571]], ['boxvr', [9500]], ['boxvR', [9566]], ['boxVr', [9567]], ['boxVR', [9568]], ['bprime', [8245]], ['breve', [728]], ['Breve', [728]], ['brvbar', [166]], ['bscr', [119991]], ['Bscr', [8492]], ['bsemi', [8271]], ['bsim', [8765]], ['bsime', [8909]], ['bsolb', [10693]], ['bsol', [92]], ['bsolhsub', [10184]], ['bull', [8226]], ['bullet', [8226]], ['bump', [8782]], ['bumpE', [10926]], ['bumpe', [8783]], ['Bumpeq', [8782]], ['bumpeq', [8783]], ['Cacute', [262]], ['cacute', [263]], ['capand', [10820]], ['capbrcup', [10825]], ['capcap', [10827]], ['cap', [8745]], ['Cap', [8914]], ['capcup', [10823]], ['capdot', [10816]], ['CapitalDifferentialD', [8517]], ['caps', [8745, 65024]], ['caret', [8257]], ['caron', [711]], ['Cayleys', [8493]], ['ccaps', [10829]], ['Ccaron', [268]], ['ccaron', [269]], ['Ccedil', [199]], ['ccedil', [231]], ['Ccirc', [264]], ['ccirc', [265]], ['Cconint', [8752]], ['ccups', [10828]], ['ccupssm', [10832]], ['Cdot', [266]], ['cdot', [267]], ['cedil', [184]], ['Cedilla', [184]], ['cemptyv', [10674]], ['cent', [162]], ['centerdot', [183]], ['CenterDot', [183]], ['cfr', [120096]], ['Cfr', [8493]], ['CHcy', [1063]], ['chcy', [1095]], ['check', [10003]], ['checkmark', [10003]], ['Chi', [935]], ['chi', [967]], ['circ', [710]], ['circeq', [8791]], ['circlearrowleft', [8634]], ['circlearrowright', [8635]], ['circledast', [8859]], ['circledcirc', [8858]], ['circleddash', [8861]], ['CircleDot', [8857]], ['circledR', [174]], ['circledS', [9416]], ['CircleMinus', [8854]], ['CirclePlus', [8853]], ['CircleTimes', [8855]], ['cir', [9675]], ['cirE', [10691]], ['cire', [8791]], ['cirfnint', [10768]], ['cirmid', [10991]], ['cirscir', [10690]], ['ClockwiseContourIntegral', [8754]], ['CloseCurlyDoubleQuote', [8221]], ['CloseCurlyQuote', [8217]], ['clubs', [9827]], ['clubsuit', [9827]], ['colon', [58]], ['Colon', [8759]], ['Colone', [10868]], ['colone', [8788]], ['coloneq', [8788]], ['comma', [44]], ['commat', [64]], ['comp', [8705]], ['compfn', [8728]], ['complement', [8705]], ['complexes', [8450]], ['cong', [8773]], ['congdot', [10861]], ['Congruent', [8801]], ['conint', [8750]], ['Conint', [8751]], ['ContourIntegral', [8750]], ['copf', [120148]], ['Copf', [8450]], ['coprod', [8720]], ['Coproduct', [8720]], ['copy', [169]], ['COPY', [169]], ['copysr', [8471]], ['CounterClockwiseContourIntegral', [8755]], ['crarr', [8629]], ['cross', [10007]], ['Cross', [10799]], ['Cscr', [119966]], ['cscr', [119992]], ['csub', [10959]], ['csube', [10961]], ['csup', [10960]], ['csupe', [10962]], ['ctdot', [8943]], ['cudarrl', [10552]], ['cudarrr', [10549]], ['cuepr', [8926]], ['cuesc', [8927]], ['cularr', [8630]], ['cularrp', [10557]], ['cupbrcap', [10824]], ['cupcap', [10822]], ['CupCap', [8781]], ['cup', [8746]], ['Cup', [8915]], ['cupcup', [10826]], ['cupdot', [8845]], ['cupor', [10821]], ['cups', [8746, 65024]], ['curarr', [8631]], ['curarrm', [10556]], ['curlyeqprec', [8926]], ['curlyeqsucc', [8927]], ['curlyvee', [8910]], ['curlywedge', [8911]], ['curren', [164]], ['curvearrowleft', [8630]], ['curvearrowright', [8631]], ['cuvee', [8910]], ['cuwed', [8911]], ['cwconint', [8754]], ['cwint', [8753]], ['cylcty', [9005]], ['dagger', [8224]], ['Dagger', [8225]], ['daleth', [8504]], ['darr', [8595]], ['Darr', [8609]], ['dArr', [8659]], ['dash', [8208]], ['Dashv', [10980]], ['dashv', [8867]], ['dbkarow', [10511]], ['dblac', [733]], ['Dcaron', [270]], ['dcaron', [271]], ['Dcy', [1044]], ['dcy', [1076]], ['ddagger', [8225]], ['ddarr', [8650]], ['DD', [8517]], ['dd', [8518]], ['DDotrahd', [10513]], ['ddotseq', [10871]], ['deg', [176]], ['Del', [8711]], ['Delta', [916]], ['delta', [948]], ['demptyv', [10673]], ['dfisht', [10623]], ['Dfr', [120071]], ['dfr', [120097]], ['dHar', [10597]], ['dharl', [8643]], ['dharr', [8642]], ['DiacriticalAcute', [180]], ['DiacriticalDot', [729]], ['DiacriticalDoubleAcute', [733]], ['DiacriticalGrave', [96]], ['DiacriticalTilde', [732]], ['diam', [8900]], ['diamond', [8900]], ['Diamond', [8900]], ['diamondsuit', [9830]], ['diams', [9830]], ['die', [168]], ['DifferentialD', [8518]], ['digamma', [989]], ['disin', [8946]], ['div', [247]], ['divide', [247]], ['divideontimes', [8903]], ['divonx', [8903]], ['DJcy', [1026]], ['djcy', [1106]], ['dlcorn', [8990]], ['dlcrop', [8973]], ['dollar', [36]], ['Dopf', [120123]], ['dopf', [120149]], ['Dot', [168]], ['dot', [729]], ['DotDot', [8412]], ['doteq', [8784]], ['doteqdot', [8785]], ['DotEqual', [8784]], ['dotminus', [8760]], ['dotplus', [8724]], ['dotsquare', [8865]], ['doublebarwedge', [8966]], ['DoubleContourIntegral', [8751]], ['DoubleDot', [168]], ['DoubleDownArrow', [8659]], ['DoubleLeftArrow', [8656]], ['DoubleLeftRightArrow', [8660]], ['DoubleLeftTee', [10980]], ['DoubleLongLeftArrow', [10232]], ['DoubleLongLeftRightArrow', [10234]], ['DoubleLongRightArrow', [10233]], ['DoubleRightArrow', [8658]], ['DoubleRightTee', [8872]], ['DoubleUpArrow', [8657]], ['DoubleUpDownArrow', [8661]], ['DoubleVerticalBar', [8741]], ['DownArrowBar', [10515]], ['downarrow', [8595]], ['DownArrow', [8595]], ['Downarrow', [8659]], ['DownArrowUpArrow', [8693]], ['DownBreve', [785]], ['downdownarrows', [8650]], ['downharpoonleft', [8643]], ['downharpoonright', [8642]], ['DownLeftRightVector', [10576]], ['DownLeftTeeVector', [10590]], ['DownLeftVectorBar', [10582]], ['DownLeftVector', [8637]], ['DownRightTeeVector', [10591]], ['DownRightVectorBar', [10583]], ['DownRightVector', [8641]], ['DownTeeArrow', [8615]], ['DownTee', [8868]], ['drbkarow', [10512]], ['drcorn', [8991]], ['drcrop', [8972]], ['Dscr', [119967]], ['dscr', [119993]], ['DScy', [1029]], ['dscy', [1109]], ['dsol', [10742]], ['Dstrok', [272]], ['dstrok', [273]], ['dtdot', [8945]], ['dtri', [9663]], ['dtrif', [9662]], ['duarr', [8693]], ['duhar', [10607]], ['dwangle', [10662]], ['DZcy', [1039]], ['dzcy', [1119]], ['dzigrarr', [10239]], ['Eacute', [201]], ['eacute', [233]], ['easter', [10862]], ['Ecaron', [282]], ['ecaron', [283]], ['Ecirc', [202]], ['ecirc', [234]], ['ecir', [8790]], ['ecolon', [8789]], ['Ecy', [1069]], ['ecy', [1101]], ['eDDot', [10871]], ['Edot', [278]], ['edot', [279]], ['eDot', [8785]], ['ee', [8519]], ['efDot', [8786]], ['Efr', [120072]], ['efr', [120098]], ['eg', [10906]], ['Egrave', [200]], ['egrave', [232]], ['egs', [10902]], ['egsdot', [10904]], ['el', [10905]], ['Element', [8712]], ['elinters', [9191]], ['ell', [8467]], ['els', [10901]], ['elsdot', [10903]], ['Emacr', [274]], ['emacr', [275]], ['empty', [8709]], ['emptyset', [8709]], ['EmptySmallSquare', [9723]], ['emptyv', [8709]], ['EmptyVerySmallSquare', [9643]], ['emsp13', [8196]], ['emsp14', [8197]], ['emsp', [8195]], ['ENG', [330]], ['eng', [331]], ['ensp', [8194]], ['Eogon', [280]], ['eogon', [281]], ['Eopf', [120124]], ['eopf', [120150]], ['epar', [8917]], ['eparsl', [10723]], ['eplus', [10865]], ['epsi', [949]], ['Epsilon', [917]], ['epsilon', [949]], ['epsiv', [1013]], ['eqcirc', [8790]], ['eqcolon', [8789]], ['eqsim', [8770]], ['eqslantgtr', [10902]], ['eqslantless', [10901]], ['Equal', [10869]], ['equals', [61]], ['EqualTilde', [8770]], ['equest', [8799]], ['Equilibrium', [8652]], ['equiv', [8801]], ['equivDD', [10872]], ['eqvparsl', [10725]], ['erarr', [10609]], ['erDot', [8787]], ['escr', [8495]], ['Escr', [8496]], ['esdot', [8784]], ['Esim', [10867]], ['esim', [8770]], ['Eta', [919]], ['eta', [951]], ['ETH', [208]], ['eth', [240]], ['Euml', [203]], ['euml', [235]], ['euro', [8364]], ['excl', [33]], ['exist', [8707]], ['Exists', [8707]], ['expectation', [8496]], ['exponentiale', [8519]], ['ExponentialE', [8519]], ['fallingdotseq', [8786]], ['Fcy', [1060]], ['fcy', [1092]], ['female', [9792]], ['ffilig', [64259]], ['fflig', [64256]], ['ffllig', [64260]], ['Ffr', [120073]], ['ffr', [120099]], ['filig', [64257]], ['FilledSmallSquare', [9724]], ['FilledVerySmallSquare', [9642]], ['fjlig', [102, 106]], ['flat', [9837]], ['fllig', [64258]], ['fltns', [9649]], ['fnof', [402]], ['Fopf', [120125]], ['fopf', [120151]], ['forall', [8704]], ['ForAll', [8704]], ['fork', [8916]], ['forkv', [10969]], ['Fouriertrf', [8497]], ['fpartint', [10765]], ['frac12', [189]], ['frac13', [8531]], ['frac14', [188]], ['frac15', [8533]], ['frac16', [8537]], ['frac18', [8539]], ['frac23', [8532]], ['frac25', [8534]], ['frac34', [190]], ['frac35', [8535]], ['frac38', [8540]], ['frac45', [8536]], ['frac56', [8538]], ['frac58', [8541]], ['frac78', [8542]], ['frasl', [8260]], ['frown', [8994]], ['fscr', [119995]], ['Fscr', [8497]], ['gacute', [501]], ['Gamma', [915]], ['gamma', [947]], ['Gammad', [988]], ['gammad', [989]], ['gap', [10886]], ['Gbreve', [286]], ['gbreve', [287]], ['Gcedil', [290]], ['Gcirc', [284]], ['gcirc', [285]], ['Gcy', [1043]], ['gcy', [1075]], ['Gdot', [288]], ['gdot', [289]], ['ge', [8805]], ['gE', [8807]], ['gEl', [10892]], ['gel', [8923]], ['geq', [8805]], ['geqq', [8807]], ['geqslant', [10878]], ['gescc', [10921]], ['ges', [10878]], ['gesdot', [10880]], ['gesdoto', [10882]], ['gesdotol', [10884]], ['gesl', [8923, 65024]], ['gesles', [10900]], ['Gfr', [120074]], ['gfr', [120100]], ['gg', [8811]], ['Gg', [8921]], ['ggg', [8921]], ['gimel', [8503]], ['GJcy', [1027]], ['gjcy', [1107]], ['gla', [10917]], ['gl', [8823]], ['glE', [10898]], ['glj', [10916]], ['gnap', [10890]], ['gnapprox', [10890]], ['gne', [10888]], ['gnE', [8809]], ['gneq', [10888]], ['gneqq', [8809]], ['gnsim', [8935]], ['Gopf', [120126]], ['gopf', [120152]], ['grave', [96]], ['GreaterEqual', [8805]], ['GreaterEqualLess', [8923]], ['GreaterFullEqual', [8807]], ['GreaterGreater', [10914]], ['GreaterLess', [8823]], ['GreaterSlantEqual', [10878]], ['GreaterTilde', [8819]], ['Gscr', [119970]], ['gscr', [8458]], ['gsim', [8819]], ['gsime', [10894]], ['gsiml', [10896]], ['gtcc', [10919]], ['gtcir', [10874]], ['gt', [62]], ['GT', [62]], ['Gt', [8811]], ['gtdot', [8919]], ['gtlPar', [10645]], ['gtquest', [10876]], ['gtrapprox', [10886]], ['gtrarr', [10616]], ['gtrdot', [8919]], ['gtreqless', [8923]], ['gtreqqless', [10892]], ['gtrless', [8823]], ['gtrsim', [8819]], ['gvertneqq', [8809, 65024]], ['gvnE', [8809, 65024]], ['Hacek', [711]], ['hairsp', [8202]], ['half', [189]], ['hamilt', [8459]], ['HARDcy', [1066]], ['hardcy', [1098]], ['harrcir', [10568]], ['harr', [8596]], ['hArr', [8660]], ['harrw', [8621]], ['Hat', [94]], ['hbar', [8463]], ['Hcirc', [292]], ['hcirc', [293]], ['hearts', [9829]], ['heartsuit', [9829]], ['hellip', [8230]], ['hercon', [8889]], ['hfr', [120101]], ['Hfr', [8460]], ['HilbertSpace', [8459]], ['hksearow', [10533]], ['hkswarow', [10534]], ['hoarr', [8703]], ['homtht', [8763]], ['hookleftarrow', [8617]], ['hookrightarrow', [8618]], ['hopf', [120153]], ['Hopf', [8461]], ['horbar', [8213]], ['HorizontalLine', [9472]], ['hscr', [119997]], ['Hscr', [8459]], ['hslash', [8463]], ['Hstrok', [294]], ['hstrok', [295]], ['HumpDownHump', [8782]], ['HumpEqual', [8783]], ['hybull', [8259]], ['hyphen', [8208]], ['Iacute', [205]], ['iacute', [237]], ['ic', [8291]], ['Icirc', [206]], ['icirc', [238]], ['Icy', [1048]], ['icy', [1080]], ['Idot', [304]], ['IEcy', [1045]], ['iecy', [1077]], ['iexcl', [161]], ['iff', [8660]], ['ifr', [120102]], ['Ifr', [8465]], ['Igrave', [204]], ['igrave', [236]], ['ii', [8520]], ['iiiint', [10764]], ['iiint', [8749]], ['iinfin', [10716]], ['iiota', [8489]], ['IJlig', [306]], ['ijlig', [307]], ['Imacr', [298]], ['imacr', [299]], ['image', [8465]], ['ImaginaryI', [8520]], ['imagline', [8464]], ['imagpart', [8465]], ['imath', [305]], ['Im', [8465]], ['imof', [8887]], ['imped', [437]], ['Implies', [8658]], ['incare', [8453]], ['in', [8712]], ['infin', [8734]], ['infintie', [10717]], ['inodot', [305]], ['intcal', [8890]], ['int', [8747]], ['Int', [8748]], ['integers', [8484]], ['Integral', [8747]], ['intercal', [8890]], ['Intersection', [8898]], ['intlarhk', [10775]], ['intprod', [10812]], ['InvisibleComma', [8291]], ['InvisibleTimes', [8290]], ['IOcy', [1025]], ['iocy', [1105]], ['Iogon', [302]], ['iogon', [303]], ['Iopf', [120128]], ['iopf', [120154]], ['Iota', [921]], ['iota', [953]], ['iprod', [10812]], ['iquest', [191]], ['iscr', [119998]], ['Iscr', [8464]], ['isin', [8712]], ['isindot', [8949]], ['isinE', [8953]], ['isins', [8948]], ['isinsv', [8947]], ['isinv', [8712]], ['it', [8290]], ['Itilde', [296]], ['itilde', [297]], ['Iukcy', [1030]], ['iukcy', [1110]], ['Iuml', [207]], ['iuml', [239]], ['Jcirc', [308]], ['jcirc', [309]], ['Jcy', [1049]], ['jcy', [1081]], ['Jfr', [120077]], ['jfr', [120103]], ['jmath', [567]], ['Jopf', [120129]], ['jopf', [120155]], ['Jscr', [119973]], ['jscr', [119999]], ['Jsercy', [1032]], ['jsercy', [1112]], ['Jukcy', [1028]], ['jukcy', [1108]], ['Kappa', [922]], ['kappa', [954]], ['kappav', [1008]], ['Kcedil', [310]], ['kcedil', [311]], ['Kcy', [1050]], ['kcy', [1082]], ['Kfr', [120078]], ['kfr', [120104]], ['kgreen', [312]], ['KHcy', [1061]], ['khcy', [1093]], ['KJcy', [1036]], ['kjcy', [1116]], ['Kopf', [120130]], ['kopf', [120156]], ['Kscr', [119974]], ['kscr', [120000]], ['lAarr', [8666]], ['Lacute', [313]], ['lacute', [314]], ['laemptyv', [10676]], ['lagran', [8466]], ['Lambda', [923]], ['lambda', [955]], ['lang', [10216]], ['Lang', [10218]], ['langd', [10641]], ['langle', [10216]], ['lap', [10885]], ['Laplacetrf', [8466]], ['laquo', [171]], ['larrb', [8676]], ['larrbfs', [10527]], ['larr', [8592]], ['Larr', [8606]], ['lArr', [8656]], ['larrfs', [10525]], ['larrhk', [8617]], ['larrlp', [8619]], ['larrpl', [10553]], ['larrsim', [10611]], ['larrtl', [8610]], ['latail', [10521]], ['lAtail', [10523]], ['lat', [10923]], ['late', [10925]], ['lates', [10925, 65024]], ['lbarr', [10508]], ['lBarr', [10510]], ['lbbrk', [10098]], ['lbrace', [123]], ['lbrack', [91]], ['lbrke', [10635]], ['lbrksld', [10639]], ['lbrkslu', [10637]], ['Lcaron', [317]], ['lcaron', [318]], ['Lcedil', [315]], ['lcedil', [316]], ['lceil', [8968]], ['lcub', [123]], ['Lcy', [1051]], ['lcy', [1083]], ['ldca', [10550]], ['ldquo', [8220]], ['ldquor', [8222]], ['ldrdhar', [10599]], ['ldrushar', [10571]], ['ldsh', [8626]], ['le', [8804]], ['lE', [8806]], ['LeftAngleBracket', [10216]], ['LeftArrowBar', [8676]], ['leftarrow', [8592]], ['LeftArrow', [8592]], ['Leftarrow', [8656]], ['LeftArrowRightArrow', [8646]], ['leftarrowtail', [8610]], ['LeftCeiling', [8968]], ['LeftDoubleBracket', [10214]], ['LeftDownTeeVector', [10593]], ['LeftDownVectorBar', [10585]], ['LeftDownVector', [8643]], ['LeftFloor', [8970]], ['leftharpoondown', [8637]], ['leftharpoonup', [8636]], ['leftleftarrows', [8647]], ['leftrightarrow', [8596]], ['LeftRightArrow', [8596]], ['Leftrightarrow', [8660]], ['leftrightarrows', [8646]], ['leftrightharpoons', [8651]], ['leftrightsquigarrow', [8621]], ['LeftRightVector', [10574]], ['LeftTeeArrow', [8612]], ['LeftTee', [8867]], ['LeftTeeVector', [10586]], ['leftthreetimes', [8907]], ['LeftTriangleBar', [10703]], ['LeftTriangle', [8882]], ['LeftTriangleEqual', [8884]], ['LeftUpDownVector', [10577]], ['LeftUpTeeVector', [10592]], ['LeftUpVectorBar', [10584]], ['LeftUpVector', [8639]], ['LeftVectorBar', [10578]], ['LeftVector', [8636]], ['lEg', [10891]], ['leg', [8922]], ['leq', [8804]], ['leqq', [8806]], ['leqslant', [10877]], ['lescc', [10920]], ['les', [10877]], ['lesdot', [10879]], ['lesdoto', [10881]], ['lesdotor', [10883]], ['lesg', [8922, 65024]], ['lesges', [10899]], ['lessapprox', [10885]], ['lessdot', [8918]], ['lesseqgtr', [8922]], ['lesseqqgtr', [10891]], ['LessEqualGreater', [8922]], ['LessFullEqual', [8806]], ['LessGreater', [8822]], ['lessgtr', [8822]], ['LessLess', [10913]], ['lesssim', [8818]], ['LessSlantEqual', [10877]], ['LessTilde', [8818]], ['lfisht', [10620]], ['lfloor', [8970]], ['Lfr', [120079]], ['lfr', [120105]], ['lg', [8822]], ['lgE', [10897]], ['lHar', [10594]], ['lhard', [8637]], ['lharu', [8636]], ['lharul', [10602]], ['lhblk', [9604]], ['LJcy', [1033]], ['ljcy', [1113]], ['llarr', [8647]], ['ll', [8810]], ['Ll', [8920]], ['llcorner', [8990]], ['Lleftarrow', [8666]], ['llhard', [10603]], ['lltri', [9722]], ['Lmidot', [319]], ['lmidot', [320]], ['lmoustache', [9136]], ['lmoust', [9136]], ['lnap', [10889]], ['lnapprox', [10889]], ['lne', [10887]], ['lnE', [8808]], ['lneq', [10887]], ['lneqq', [8808]], ['lnsim', [8934]], ['loang', [10220]], ['loarr', [8701]], ['lobrk', [10214]], ['longleftarrow', [10229]], ['LongLeftArrow', [10229]], ['Longleftarrow', [10232]], ['longleftrightarrow', [10231]], ['LongLeftRightArrow', [10231]], ['Longleftrightarrow', [10234]], ['longmapsto', [10236]], ['longrightarrow', [10230]], ['LongRightArrow', [10230]], ['Longrightarrow', [10233]], ['looparrowleft', [8619]], ['looparrowright', [8620]], ['lopar', [10629]], ['Lopf', [120131]], ['lopf', [120157]], ['loplus', [10797]], ['lotimes', [10804]], ['lowast', [8727]], ['lowbar', [95]], ['LowerLeftArrow', [8601]], ['LowerRightArrow', [8600]], ['loz', [9674]], ['lozenge', [9674]], ['lozf', [10731]], ['lpar', [40]], ['lparlt', [10643]], ['lrarr', [8646]], ['lrcorner', [8991]], ['lrhar', [8651]], ['lrhard', [10605]], ['lrm', [8206]], ['lrtri', [8895]], ['lsaquo', [8249]], ['lscr', [120001]], ['Lscr', [8466]], ['lsh', [8624]], ['Lsh', [8624]], ['lsim', [8818]], ['lsime', [10893]], ['lsimg', [10895]], ['lsqb', [91]], ['lsquo', [8216]], ['lsquor', [8218]], ['Lstrok', [321]], ['lstrok', [322]], ['ltcc', [10918]], ['ltcir', [10873]], ['lt', [60]], ['LT', [60]], ['Lt', [8810]], ['ltdot', [8918]], ['lthree', [8907]], ['ltimes', [8905]], ['ltlarr', [10614]], ['ltquest', [10875]], ['ltri', [9667]], ['ltrie', [8884]], ['ltrif', [9666]], ['ltrPar', [10646]], ['lurdshar', [10570]], ['luruhar', [10598]], ['lvertneqq', [8808, 65024]], ['lvnE', [8808, 65024]], ['macr', [175]], ['male', [9794]], ['malt', [10016]], ['maltese', [10016]], ['Map', [10501]], ['map', [8614]], ['mapsto', [8614]], ['mapstodown', [8615]], ['mapstoleft', [8612]], ['mapstoup', [8613]], ['marker', [9646]], ['mcomma', [10793]], ['Mcy', [1052]], ['mcy', [1084]], ['mdash', [8212]], ['mDDot', [8762]], ['measuredangle', [8737]], ['MediumSpace', [8287]], ['Mellintrf', [8499]], ['Mfr', [120080]], ['mfr', [120106]], ['mho', [8487]], ['micro', [181]], ['midast', [42]], ['midcir', [10992]], ['mid', [8739]], ['middot', [183]], ['minusb', [8863]], ['minus', [8722]], ['minusd', [8760]], ['minusdu', [10794]], ['MinusPlus', [8723]], ['mlcp', [10971]], ['mldr', [8230]], ['mnplus', [8723]], ['models', [8871]], ['Mopf', [120132]], ['mopf', [120158]], ['mp', [8723]], ['mscr', [120002]], ['Mscr', [8499]], ['mstpos', [8766]], ['Mu', [924]], ['mu', [956]], ['multimap', [8888]], ['mumap', [8888]], ['nabla', [8711]], ['Nacute', [323]], ['nacute', [324]], ['nang', [8736, 8402]], ['nap', [8777]], ['napE', [10864, 824]], ['napid', [8779, 824]], ['napos', [329]], ['napprox', [8777]], ['natural', [9838]], ['naturals', [8469]], ['natur', [9838]], ['nbsp', [160]], ['nbump', [8782, 824]], ['nbumpe', [8783, 824]], ['ncap', [10819]], ['Ncaron', [327]], ['ncaron', [328]], ['Ncedil', [325]], ['ncedil', [326]], ['ncong', [8775]], ['ncongdot', [10861, 824]], ['ncup', [10818]], ['Ncy', [1053]], ['ncy', [1085]], ['ndash', [8211]], ['nearhk', [10532]], ['nearr', [8599]], ['neArr', [8663]], ['nearrow', [8599]], ['ne', [8800]], ['nedot', [8784, 824]], ['NegativeMediumSpace', [8203]], ['NegativeThickSpace', [8203]], ['NegativeThinSpace', [8203]], ['NegativeVeryThinSpace', [8203]], ['nequiv', [8802]], ['nesear', [10536]], ['nesim', [8770, 824]], ['NestedGreaterGreater', [8811]], ['NestedLessLess', [8810]], ['nexist', [8708]], ['nexists', [8708]], ['Nfr', [120081]], ['nfr', [120107]], ['ngE', [8807, 824]], ['nge', [8817]], ['ngeq', [8817]], ['ngeqq', [8807, 824]], ['ngeqslant', [10878, 824]], ['nges', [10878, 824]], ['nGg', [8921, 824]], ['ngsim', [8821]], ['nGt', [8811, 8402]], ['ngt', [8815]], ['ngtr', [8815]], ['nGtv', [8811, 824]], ['nharr', [8622]], ['nhArr', [8654]], ['nhpar', [10994]], ['ni', [8715]], ['nis', [8956]], ['nisd', [8954]], ['niv', [8715]], ['NJcy', [1034]], ['njcy', [1114]], ['nlarr', [8602]], ['nlArr', [8653]], ['nldr', [8229]], ['nlE', [8806, 824]], ['nle', [8816]], ['nleftarrow', [8602]], ['nLeftarrow', [8653]], ['nleftrightarrow', [8622]], ['nLeftrightarrow', [8654]], ['nleq', [8816]], ['nleqq', [8806, 824]], ['nleqslant', [10877, 824]], ['nles', [10877, 824]], ['nless', [8814]], ['nLl', [8920, 824]], ['nlsim', [8820]], ['nLt', [8810, 8402]], ['nlt', [8814]], ['nltri', [8938]], ['nltrie', [8940]], ['nLtv', [8810, 824]], ['nmid', [8740]], ['NoBreak', [8288]], ['NonBreakingSpace', [160]], ['nopf', [120159]], ['Nopf', [8469]], ['Not', [10988]], ['not', [172]], ['NotCongruent', [8802]], ['NotCupCap', [8813]], ['NotDoubleVerticalBar', [8742]], ['NotElement', [8713]], ['NotEqual', [8800]], ['NotEqualTilde', [8770, 824]], ['NotExists', [8708]], ['NotGreater', [8815]], ['NotGreaterEqual', [8817]], ['NotGreaterFullEqual', [8807, 824]], ['NotGreaterGreater', [8811, 824]], ['NotGreaterLess', [8825]], ['NotGreaterSlantEqual', [10878, 824]], ['NotGreaterTilde', [8821]], ['NotHumpDownHump', [8782, 824]], ['NotHumpEqual', [8783, 824]], ['notin', [8713]], ['notindot', [8949, 824]], ['notinE', [8953, 824]], ['notinva', [8713]], ['notinvb', [8951]], ['notinvc', [8950]], ['NotLeftTriangleBar', [10703, 824]], ['NotLeftTriangle', [8938]], ['NotLeftTriangleEqual', [8940]], ['NotLess', [8814]], ['NotLessEqual', [8816]], ['NotLessGreater', [8824]], ['NotLessLess', [8810, 824]], ['NotLessSlantEqual', [10877, 824]], ['NotLessTilde', [8820]], ['NotNestedGreaterGreater', [10914, 824]], ['NotNestedLessLess', [10913, 824]], ['notni', [8716]], ['notniva', [8716]], ['notnivb', [8958]], ['notnivc', [8957]], ['NotPrecedes', [8832]], ['NotPrecedesEqual', [10927, 824]], ['NotPrecedesSlantEqual', [8928]], ['NotReverseElement', [8716]], ['NotRightTriangleBar', [10704, 824]], ['NotRightTriangle', [8939]], ['NotRightTriangleEqual', [8941]], ['NotSquareSubset', [8847, 824]], ['NotSquareSubsetEqual', [8930]], ['NotSquareSuperset', [8848, 824]], ['NotSquareSupersetEqual', [8931]], ['NotSubset', [8834, 8402]], ['NotSubsetEqual', [8840]], ['NotSucceeds', [8833]], ['NotSucceedsEqual', [10928, 824]], ['NotSucceedsSlantEqual', [8929]], ['NotSucceedsTilde', [8831, 824]], ['NotSuperset', [8835, 8402]], ['NotSupersetEqual', [8841]], ['NotTilde', [8769]], ['NotTildeEqual', [8772]], ['NotTildeFullEqual', [8775]], ['NotTildeTilde', [8777]], ['NotVerticalBar', [8740]], ['nparallel', [8742]], ['npar', [8742]], ['nparsl', [11005, 8421]], ['npart', [8706, 824]], ['npolint', [10772]], ['npr', [8832]], ['nprcue', [8928]], ['nprec', [8832]], ['npreceq', [10927, 824]], ['npre', [10927, 824]], ['nrarrc', [10547, 824]], ['nrarr', [8603]], ['nrArr', [8655]], ['nrarrw', [8605, 824]], ['nrightarrow', [8603]], ['nRightarrow', [8655]], ['nrtri', [8939]], ['nrtrie', [8941]], ['nsc', [8833]], ['nsccue', [8929]], ['nsce', [10928, 824]], ['Nscr', [119977]], ['nscr', [120003]], ['nshortmid', [8740]], ['nshortparallel', [8742]], ['nsim', [8769]], ['nsime', [8772]], ['nsimeq', [8772]], ['nsmid', [8740]], ['nspar', [8742]], ['nsqsube', [8930]], ['nsqsupe', [8931]], ['nsub', [8836]], ['nsubE', [10949, 824]], ['nsube', [8840]], ['nsubset', [8834, 8402]], ['nsubseteq', [8840]], ['nsubseteqq', [10949, 824]], ['nsucc', [8833]], ['nsucceq', [10928, 824]], ['nsup', [8837]], ['nsupE', [10950, 824]], ['nsupe', [8841]], ['nsupset', [8835, 8402]], ['nsupseteq', [8841]], ['nsupseteqq', [10950, 824]], ['ntgl', [8825]], ['Ntilde', [209]], ['ntilde', [241]], ['ntlg', [8824]], ['ntriangleleft', [8938]], ['ntrianglelefteq', [8940]], ['ntriangleright', [8939]], ['ntrianglerighteq', [8941]], ['Nu', [925]], ['nu', [957]], ['num', [35]], ['numero', [8470]], ['numsp', [8199]], ['nvap', [8781, 8402]], ['nvdash', [8876]], ['nvDash', [8877]], ['nVdash', [8878]], ['nVDash', [8879]], ['nvge', [8805, 8402]], ['nvgt', [62, 8402]], ['nvHarr', [10500]], ['nvinfin', [10718]], ['nvlArr', [10498]], ['nvle', [8804, 8402]], ['nvlt', [60, 8402]], ['nvltrie', [8884, 8402]], ['nvrArr', [10499]], ['nvrtrie', [8885, 8402]], ['nvsim', [8764, 8402]], ['nwarhk', [10531]], ['nwarr', [8598]], ['nwArr', [8662]], ['nwarrow', [8598]], ['nwnear', [10535]], ['Oacute', [211]], ['oacute', [243]], ['oast', [8859]], ['Ocirc', [212]], ['ocirc', [244]], ['ocir', [8858]], ['Ocy', [1054]], ['ocy', [1086]], ['odash', [8861]], ['Odblac', [336]], ['odblac', [337]], ['odiv', [10808]], ['odot', [8857]], ['odsold', [10684]], ['OElig', [338]], ['oelig', [339]], ['ofcir', [10687]], ['Ofr', [120082]], ['ofr', [120108]], ['ogon', [731]], ['Ograve', [210]], ['ograve', [242]], ['ogt', [10689]], ['ohbar', [10677]], ['ohm', [937]], ['oint', [8750]], ['olarr', [8634]], ['olcir', [10686]], ['olcross', [10683]], ['oline', [8254]], ['olt', [10688]], ['Omacr', [332]], ['omacr', [333]], ['Omega', [937]], ['omega', [969]], ['Omicron', [927]], ['omicron', [959]], ['omid', [10678]], ['ominus', [8854]], ['Oopf', [120134]], ['oopf', [120160]], ['opar', [10679]], ['OpenCurlyDoubleQuote', [8220]], ['OpenCurlyQuote', [8216]], ['operp', [10681]], ['oplus', [8853]], ['orarr', [8635]], ['Or', [10836]], ['or', [8744]], ['ord', [10845]], ['order', [8500]], ['orderof', [8500]], ['ordf', [170]], ['ordm', [186]], ['origof', [8886]], ['oror', [10838]], ['orslope', [10839]], ['orv', [10843]], ['oS', [9416]], ['Oscr', [119978]], ['oscr', [8500]], ['Oslash', [216]], ['oslash', [248]], ['osol', [8856]], ['Otilde', [213]], ['otilde', [245]], ['otimesas', [10806]], ['Otimes', [10807]], ['otimes', [8855]], ['Ouml', [214]], ['ouml', [246]], ['ovbar', [9021]], ['OverBar', [8254]], ['OverBrace', [9182]], ['OverBracket', [9140]], ['OverParenthesis', [9180]], ['para', [182]], ['parallel', [8741]], ['par', [8741]], ['parsim', [10995]], ['parsl', [11005]], ['part', [8706]], ['PartialD', [8706]], ['Pcy', [1055]], ['pcy', [1087]], ['percnt', [37]], ['period', [46]], ['permil', [8240]], ['perp', [8869]], ['pertenk', [8241]], ['Pfr', [120083]], ['pfr', [120109]], ['Phi', [934]], ['phi', [966]], ['phiv', [981]], ['phmmat', [8499]], ['phone', [9742]], ['Pi', [928]], ['pi', [960]], ['pitchfork', [8916]], ['piv', [982]], ['planck', [8463]], ['planckh', [8462]], ['plankv', [8463]], ['plusacir', [10787]], ['plusb', [8862]], ['pluscir', [10786]], ['plus', [43]], ['plusdo', [8724]], ['plusdu', [10789]], ['pluse', [10866]], ['PlusMinus', [177]], ['plusmn', [177]], ['plussim', [10790]], ['plustwo', [10791]], ['pm', [177]], ['Poincareplane', [8460]], ['pointint', [10773]], ['popf', [120161]], ['Popf', [8473]], ['pound', [163]], ['prap', [10935]], ['Pr', [10939]], ['pr', [8826]], ['prcue', [8828]], ['precapprox', [10935]], ['prec', [8826]], ['preccurlyeq', [8828]], ['Precedes', [8826]], ['PrecedesEqual', [10927]], ['PrecedesSlantEqual', [8828]], ['PrecedesTilde', [8830]], ['preceq', [10927]], ['precnapprox', [10937]], ['precneqq', [10933]], ['precnsim', [8936]], ['pre', [10927]], ['prE', [10931]], ['precsim', [8830]], ['prime', [8242]], ['Prime', [8243]], ['primes', [8473]], ['prnap', [10937]], ['prnE', [10933]], ['prnsim', [8936]], ['prod', [8719]], ['Product', [8719]], ['profalar', [9006]], ['profline', [8978]], ['profsurf', [8979]], ['prop', [8733]], ['Proportional', [8733]], ['Proportion', [8759]], ['propto', [8733]], ['prsim', [8830]], ['prurel', [8880]], ['Pscr', [119979]], ['pscr', [120005]], ['Psi', [936]], ['psi', [968]], ['puncsp', [8200]], ['Qfr', [120084]], ['qfr', [120110]], ['qint', [10764]], ['qopf', [120162]], ['Qopf', [8474]], ['qprime', [8279]], ['Qscr', [119980]], ['qscr', [120006]], ['quaternions', [8461]], ['quatint', [10774]], ['quest', [63]], ['questeq', [8799]], ['quot', [34]], ['QUOT', [34]], ['rAarr', [8667]], ['race', [8765, 817]], ['Racute', [340]], ['racute', [341]], ['radic', [8730]], ['raemptyv', [10675]], ['rang', [10217]], ['Rang', [10219]], ['rangd', [10642]], ['range', [10661]], ['rangle', [10217]], ['raquo', [187]], ['rarrap', [10613]], ['rarrb', [8677]], ['rarrbfs', [10528]], ['rarrc', [10547]], ['rarr', [8594]], ['Rarr', [8608]], ['rArr', [8658]], ['rarrfs', [10526]], ['rarrhk', [8618]], ['rarrlp', [8620]], ['rarrpl', [10565]], ['rarrsim', [10612]], ['Rarrtl', [10518]], ['rarrtl', [8611]], ['rarrw', [8605]], ['ratail', [10522]], ['rAtail', [10524]], ['ratio', [8758]], ['rationals', [8474]], ['rbarr', [10509]], ['rBarr', [10511]], ['RBarr', [10512]], ['rbbrk', [10099]], ['rbrace', [125]], ['rbrack', [93]], ['rbrke', [10636]], ['rbrksld', [10638]], ['rbrkslu', [10640]], ['Rcaron', [344]], ['rcaron', [345]], ['Rcedil', [342]], ['rcedil', [343]], ['rceil', [8969]], ['rcub', [125]], ['Rcy', [1056]], ['rcy', [1088]], ['rdca', [10551]], ['rdldhar', [10601]], ['rdquo', [8221]], ['rdquor', [8221]], ['rdsh', [8627]], ['real', [8476]], ['realine', [8475]], ['realpart', [8476]], ['reals', [8477]], ['Re', [8476]], ['rect', [9645]], ['reg', [174]], ['REG', [174]], ['ReverseElement', [8715]], ['ReverseEquilibrium', [8651]], ['ReverseUpEquilibrium', [10607]], ['rfisht', [10621]], ['rfloor', [8971]], ['rfr', [120111]], ['Rfr', [8476]], ['rHar', [10596]], ['rhard', [8641]], ['rharu', [8640]], ['rharul', [10604]], ['Rho', [929]], ['rho', [961]], ['rhov', [1009]], ['RightAngleBracket', [10217]], ['RightArrowBar', [8677]], ['rightarrow', [8594]], ['RightArrow', [8594]], ['Rightarrow', [8658]], ['RightArrowLeftArrow', [8644]], ['rightarrowtail', [8611]], ['RightCeiling', [8969]], ['RightDoubleBracket', [10215]], ['RightDownTeeVector', [10589]], ['RightDownVectorBar', [10581]], ['RightDownVector', [8642]], ['RightFloor', [8971]], ['rightharpoondown', [8641]], ['rightharpoonup', [8640]], ['rightleftarrows', [8644]], ['rightleftharpoons', [8652]], ['rightrightarrows', [8649]], ['rightsquigarrow', [8605]], ['RightTeeArrow', [8614]], ['RightTee', [8866]], ['RightTeeVector', [10587]], ['rightthreetimes', [8908]], ['RightTriangleBar', [10704]], ['RightTriangle', [8883]], ['RightTriangleEqual', [8885]], ['RightUpDownVector', [10575]], ['RightUpTeeVector', [10588]], ['RightUpVectorBar', [10580]], ['RightUpVector', [8638]], ['RightVectorBar', [10579]], ['RightVector', [8640]], ['ring', [730]], ['risingdotseq', [8787]], ['rlarr', [8644]], ['rlhar', [8652]], ['rlm', [8207]], ['rmoustache', [9137]], ['rmoust', [9137]], ['rnmid', [10990]], ['roang', [10221]], ['roarr', [8702]], ['robrk', [10215]], ['ropar', [10630]], ['ropf', [120163]], ['Ropf', [8477]], ['roplus', [10798]], ['rotimes', [10805]], ['RoundImplies', [10608]], ['rpar', [41]], ['rpargt', [10644]], ['rppolint', [10770]], ['rrarr', [8649]], ['Rrightarrow', [8667]], ['rsaquo', [8250]], ['rscr', [120007]], ['Rscr', [8475]], ['rsh', [8625]], ['Rsh', [8625]], ['rsqb', [93]], ['rsquo', [8217]], ['rsquor', [8217]], ['rthree', [8908]], ['rtimes', [8906]], ['rtri', [9657]], ['rtrie', [8885]], ['rtrif', [9656]], ['rtriltri', [10702]], ['RuleDelayed', [10740]], ['ruluhar', [10600]], ['rx', [8478]], ['Sacute', [346]], ['sacute', [347]], ['sbquo', [8218]], ['scap', [10936]], ['Scaron', [352]], ['scaron', [353]], ['Sc', [10940]], ['sc', [8827]], ['sccue', [8829]], ['sce', [10928]], ['scE', [10932]], ['Scedil', [350]], ['scedil', [351]], ['Scirc', [348]], ['scirc', [349]], ['scnap', [10938]], ['scnE', [10934]], ['scnsim', [8937]], ['scpolint', [10771]], ['scsim', [8831]], ['Scy', [1057]], ['scy', [1089]], ['sdotb', [8865]], ['sdot', [8901]], ['sdote', [10854]], ['searhk', [10533]], ['searr', [8600]], ['seArr', [8664]], ['searrow', [8600]], ['sect', [167]], ['semi', [59]], ['seswar', [10537]], ['setminus', [8726]], ['setmn', [8726]], ['sext', [10038]], ['Sfr', [120086]], ['sfr', [120112]], ['sfrown', [8994]], ['sharp', [9839]], ['SHCHcy', [1065]], ['shchcy', [1097]], ['SHcy', [1064]], ['shcy', [1096]], ['ShortDownArrow', [8595]], ['ShortLeftArrow', [8592]], ['shortmid', [8739]], ['shortparallel', [8741]], ['ShortRightArrow', [8594]], ['ShortUpArrow', [8593]], ['shy', [173]], ['Sigma', [931]], ['sigma', [963]], ['sigmaf', [962]], ['sigmav', [962]], ['sim', [8764]], ['simdot', [10858]], ['sime', [8771]], ['simeq', [8771]], ['simg', [10910]], ['simgE', [10912]], ['siml', [10909]], ['simlE', [10911]], ['simne', [8774]], ['simplus', [10788]], ['simrarr', [10610]], ['slarr', [8592]], ['SmallCircle', [8728]], ['smallsetminus', [8726]], ['smashp', [10803]], ['smeparsl', [10724]], ['smid', [8739]], ['smile', [8995]], ['smt', [10922]], ['smte', [10924]], ['smtes', [10924, 65024]], ['SOFTcy', [1068]], ['softcy', [1100]], ['solbar', [9023]], ['solb', [10692]], ['sol', [47]], ['Sopf', [120138]], ['sopf', [120164]], ['spades', [9824]], ['spadesuit', [9824]], ['spar', [8741]], ['sqcap', [8851]], ['sqcaps', [8851, 65024]], ['sqcup', [8852]], ['sqcups', [8852, 65024]], ['Sqrt', [8730]], ['sqsub', [8847]], ['sqsube', [8849]], ['sqsubset', [8847]], ['sqsubseteq', [8849]], ['sqsup', [8848]], ['sqsupe', [8850]], ['sqsupset', [8848]], ['sqsupseteq', [8850]], ['square', [9633]], ['Square', [9633]], ['SquareIntersection', [8851]], ['SquareSubset', [8847]], ['SquareSubsetEqual', [8849]], ['SquareSuperset', [8848]], ['SquareSupersetEqual', [8850]], ['SquareUnion', [8852]], ['squarf', [9642]], ['squ', [9633]], ['squf', [9642]], ['srarr', [8594]], ['Sscr', [119982]], ['sscr', [120008]], ['ssetmn', [8726]], ['ssmile', [8995]], ['sstarf', [8902]], ['Star', [8902]], ['star', [9734]], ['starf', [9733]], ['straightepsilon', [1013]], ['straightphi', [981]], ['strns', [175]], ['sub', [8834]], ['Sub', [8912]], ['subdot', [10941]], ['subE', [10949]], ['sube', [8838]], ['subedot', [10947]], ['submult', [10945]], ['subnE', [10955]], ['subne', [8842]], ['subplus', [10943]], ['subrarr', [10617]], ['subset', [8834]], ['Subset', [8912]], ['subseteq', [8838]], ['subseteqq', [10949]], ['SubsetEqual', [8838]], ['subsetneq', [8842]], ['subsetneqq', [10955]], ['subsim', [10951]], ['subsub', [10965]], ['subsup', [10963]], ['succapprox', [10936]], ['succ', [8827]], ['succcurlyeq', [8829]], ['Succeeds', [8827]], ['SucceedsEqual', [10928]], ['SucceedsSlantEqual', [8829]], ['SucceedsTilde', [8831]], ['succeq', [10928]], ['succnapprox', [10938]], ['succneqq', [10934]], ['succnsim', [8937]], ['succsim', [8831]], ['SuchThat', [8715]], ['sum', [8721]], ['Sum', [8721]], ['sung', [9834]], ['sup1', [185]], ['sup2', [178]], ['sup3', [179]], ['sup', [8835]], ['Sup', [8913]], ['supdot', [10942]], ['supdsub', [10968]], ['supE', [10950]], ['supe', [8839]], ['supedot', [10948]], ['Superset', [8835]], ['SupersetEqual', [8839]], ['suphsol', [10185]], ['suphsub', [10967]], ['suplarr', [10619]], ['supmult', [10946]], ['supnE', [10956]], ['supne', [8843]], ['supplus', [10944]], ['supset', [8835]], ['Supset', [8913]], ['supseteq', [8839]], ['supseteqq', [10950]], ['supsetneq', [8843]], ['supsetneqq', [10956]], ['supsim', [10952]], ['supsub', [10964]], ['supsup', [10966]], ['swarhk', [10534]], ['swarr', [8601]], ['swArr', [8665]], ['swarrow', [8601]], ['swnwar', [10538]], ['szlig', [223]], ['Tab', [9]], ['target', [8982]], ['Tau', [932]], ['tau', [964]], ['tbrk', [9140]], ['Tcaron', [356]], ['tcaron', [357]], ['Tcedil', [354]], ['tcedil', [355]], ['Tcy', [1058]], ['tcy', [1090]], ['tdot', [8411]], ['telrec', [8981]], ['Tfr', [120087]], ['tfr', [120113]], ['there4', [8756]], ['therefore', [8756]], ['Therefore', [8756]], ['Theta', [920]], ['theta', [952]], ['thetasym', [977]], ['thetav', [977]], ['thickapprox', [8776]], ['thicksim', [8764]], ['ThickSpace', [8287, 8202]], ['ThinSpace', [8201]], ['thinsp', [8201]], ['thkap', [8776]], ['thksim', [8764]], ['THORN', [222]], ['thorn', [254]], ['tilde', [732]], ['Tilde', [8764]], ['TildeEqual', [8771]], ['TildeFullEqual', [8773]], ['TildeTilde', [8776]], ['timesbar', [10801]], ['timesb', [8864]], ['times', [215]], ['timesd', [10800]], ['tint', [8749]], ['toea', [10536]], ['topbot', [9014]], ['topcir', [10993]], ['top', [8868]], ['Topf', [120139]], ['topf', [120165]], ['topfork', [10970]], ['tosa', [10537]], ['tprime', [8244]], ['trade', [8482]], ['TRADE', [8482]], ['triangle', [9653]], ['triangledown', [9663]], ['triangleleft', [9667]], ['trianglelefteq', [8884]], ['triangleq', [8796]], ['triangleright', [9657]], ['trianglerighteq', [8885]], ['tridot', [9708]], ['trie', [8796]], ['triminus', [10810]], ['TripleDot', [8411]], ['triplus', [10809]], ['trisb', [10701]], ['tritime', [10811]], ['trpezium', [9186]], ['Tscr', [119983]], ['tscr', [120009]], ['TScy', [1062]], ['tscy', [1094]], ['TSHcy', [1035]], ['tshcy', [1115]], ['Tstrok', [358]], ['tstrok', [359]], ['twixt', [8812]], ['twoheadleftarrow', [8606]], ['twoheadrightarrow', [8608]], ['Uacute', [218]], ['uacute', [250]], ['uarr', [8593]], ['Uarr', [8607]], ['uArr', [8657]], ['Uarrocir', [10569]], ['Ubrcy', [1038]], ['ubrcy', [1118]], ['Ubreve', [364]], ['ubreve', [365]], ['Ucirc', [219]], ['ucirc', [251]], ['Ucy', [1059]], ['ucy', [1091]], ['udarr', [8645]], ['Udblac', [368]], ['udblac', [369]], ['udhar', [10606]], ['ufisht', [10622]], ['Ufr', [120088]], ['ufr', [120114]], ['Ugrave', [217]], ['ugrave', [249]], ['uHar', [10595]], ['uharl', [8639]], ['uharr', [8638]], ['uhblk', [9600]], ['ulcorn', [8988]], ['ulcorner', [8988]], ['ulcrop', [8975]], ['ultri', [9720]], ['Umacr', [362]], ['umacr', [363]], ['uml', [168]], ['UnderBar', [95]], ['UnderBrace', [9183]], ['UnderBracket', [9141]], ['UnderParenthesis', [9181]], ['Union', [8899]], ['UnionPlus', [8846]], ['Uogon', [370]], ['uogon', [371]], ['Uopf', [120140]], ['uopf', [120166]], ['UpArrowBar', [10514]], ['uparrow', [8593]], ['UpArrow', [8593]], ['Uparrow', [8657]], ['UpArrowDownArrow', [8645]], ['updownarrow', [8597]], ['UpDownArrow', [8597]], ['Updownarrow', [8661]], ['UpEquilibrium', [10606]], ['upharpoonleft', [8639]], ['upharpoonright', [8638]], ['uplus', [8846]], ['UpperLeftArrow', [8598]], ['UpperRightArrow', [8599]], ['upsi', [965]], ['Upsi', [978]], ['upsih', [978]], ['Upsilon', [933]], ['upsilon', [965]], ['UpTeeArrow', [8613]], ['UpTee', [8869]], ['upuparrows', [8648]], ['urcorn', [8989]], ['urcorner', [8989]], ['urcrop', [8974]], ['Uring', [366]], ['uring', [367]], ['urtri', [9721]], ['Uscr', [119984]], ['uscr', [120010]], ['utdot', [8944]], ['Utilde', [360]], ['utilde', [361]], ['utri', [9653]], ['utrif', [9652]], ['uuarr', [8648]], ['Uuml', [220]], ['uuml', [252]], ['uwangle', [10663]], ['vangrt', [10652]], ['varepsilon', [1013]], ['varkappa', [1008]], ['varnothing', [8709]], ['varphi', [981]], ['varpi', [982]], ['varpropto', [8733]], ['varr', [8597]], ['vArr', [8661]], ['varrho', [1009]], ['varsigma', [962]], ['varsubsetneq', [8842, 65024]], ['varsubsetneqq', [10955, 65024]], ['varsupsetneq', [8843, 65024]], ['varsupsetneqq', [10956, 65024]], ['vartheta', [977]], ['vartriangleleft', [8882]], ['vartriangleright', [8883]], ['vBar', [10984]], ['Vbar', [10987]], ['vBarv', [10985]], ['Vcy', [1042]], ['vcy', [1074]], ['vdash', [8866]], ['vDash', [8872]], ['Vdash', [8873]], ['VDash', [8875]], ['Vdashl', [10982]], ['veebar', [8891]], ['vee', [8744]], ['Vee', [8897]], ['veeeq', [8794]], ['vellip', [8942]], ['verbar', [124]], ['Verbar', [8214]], ['vert', [124]], ['Vert', [8214]], ['VerticalBar', [8739]], ['VerticalLine', [124]], ['VerticalSeparator', [10072]], ['VerticalTilde', [8768]], ['VeryThinSpace', [8202]], ['Vfr', [120089]], ['vfr', [120115]], ['vltri', [8882]], ['vnsub', [8834, 8402]], ['vnsup', [8835, 8402]], ['Vopf', [120141]], ['vopf', [120167]], ['vprop', [8733]], ['vrtri', [8883]], ['Vscr', [119985]], ['vscr', [120011]], ['vsubnE', [10955, 65024]], ['vsubne', [8842, 65024]], ['vsupnE', [10956, 65024]], ['vsupne', [8843, 65024]], ['Vvdash', [8874]], ['vzigzag', [10650]], ['Wcirc', [372]], ['wcirc', [373]], ['wedbar', [10847]], ['wedge', [8743]], ['Wedge', [8896]], ['wedgeq', [8793]], ['weierp', [8472]], ['Wfr', [120090]], ['wfr', [120116]], ['Wopf', [120142]], ['wopf', [120168]], ['wp', [8472]], ['wr', [8768]], ['wreath', [8768]], ['Wscr', [119986]], ['wscr', [120012]], ['xcap', [8898]], ['xcirc', [9711]], ['xcup', [8899]], ['xdtri', [9661]], ['Xfr', [120091]], ['xfr', [120117]], ['xharr', [10231]], ['xhArr', [10234]], ['Xi', [926]], ['xi', [958]], ['xlarr', [10229]], ['xlArr', [10232]], ['xmap', [10236]], ['xnis', [8955]], ['xodot', [10752]], ['Xopf', [120143]], ['xopf', [120169]], ['xoplus', [10753]], ['xotime', [10754]], ['xrarr', [10230]], ['xrArr', [10233]], ['Xscr', [119987]], ['xscr', [120013]], ['xsqcup', [10758]], ['xuplus', [10756]], ['xutri', [9651]], ['xvee', [8897]], ['xwedge', [8896]], ['Yacute', [221]], ['yacute', [253]], ['YAcy', [1071]], ['yacy', [1103]], ['Ycirc', [374]], ['ycirc', [375]], ['Ycy', [1067]], ['ycy', [1099]], ['yen', [165]], ['Yfr', [120092]], ['yfr', [120118]], ['YIcy', [1031]], ['yicy', [1111]], ['Yopf', [120144]], ['yopf', [120170]], ['Yscr', [119988]], ['yscr', [120014]], ['YUcy', [1070]], ['yucy', [1102]], ['yuml', [255]], ['Yuml', [376]], ['Zacute', [377]], ['zacute', [378]], ['Zcaron', [381]], ['zcaron', [382]], ['Zcy', [1047]], ['zcy', [1079]], ['Zdot', [379]], ['zdot', [380]], ['zeetrf', [8488]], ['ZeroWidthSpace', [8203]], ['Zeta', [918]], ['zeta', [950]], ['zfr', [120119]], ['Zfr', [8488]], ['ZHcy', [1046]], ['zhcy', [1078]], ['zigrarr', [8669]], ['zopf', [120171]], ['Zopf', [8484]], ['Zscr', [119989]], ['zscr', [120015]], ['zwj', [8205]], ['zwnj', [8204]]];
	
	var alphaIndex = {};
	var charIndex = {};
	
	createIndexes(alphaIndex, charIndex);
	
	/**
	 * @constructor
	 */
	function Html5Entities() {}
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	Html5Entities.prototype.decode = function(str) {
	    if (str.length === 0) {
	        return '';
	    }
	    return str.replace(/&(#?[\w\d]+);?/g, function(s, entity) {
	        var chr;
	        if (entity.charAt(0) === "#") {
	            var code = entity.charAt(1) === 'x' ?
	                parseInt(entity.substr(2).toLowerCase(), 16) :
	                parseInt(entity.substr(1));
	
	            if (!(isNaN(code) || code < -32768 || code > 65535)) {
	                chr = String.fromCharCode(code);
	            }
	        } else {
	            chr = alphaIndex[entity];
	        }
	        return chr || s;
	    });
	};
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	 Html5Entities.decode = function(str) {
	    return new Html5Entities().decode(str);
	 };
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	Html5Entities.prototype.encode = function(str) {
	    var strLength = str.length;
	    if (strLength === 0) {
	        return '';
	    }
	    var result = '';
	    var i = 0;
	    while (i < strLength) {
	        var charInfo = charIndex[str.charCodeAt(i)];
	        if (charInfo) {
	            var alpha = charInfo[str.charCodeAt(i + 1)];
	            if (alpha) {
	                i++;
	            } else {
	                alpha = charInfo[''];
	            }
	            if (alpha) {
	                result += "&" + alpha + ";";
	                i++;
	                continue;
	            }
	        }
	        result += str.charAt(i);
	        i++;
	    }
	    return result;
	};
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	 Html5Entities.encode = function(str) {
	    return new Html5Entities().encode(str);
	 };
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	Html5Entities.prototype.encodeNonUTF = function(str) {
	    var strLength = str.length;
	    if (strLength === 0) {
	        return '';
	    }
	    var result = '';
	    var i = 0;
	    while (i < strLength) {
	        var c = str.charCodeAt(i);
	        var charInfo = charIndex[c];
	        if (charInfo) {
	            var alpha = charInfo[str.charCodeAt(i + 1)];
	            if (alpha) {
	                i++;
	            } else {
	                alpha = charInfo[''];
	            }
	            if (alpha) {
	                result += "&" + alpha + ";";
	                i++;
	                continue;
	            }
	        }
	        if (c < 32 || c > 126) {
	            result += '&#' + c + ';';
	        } else {
	            result += str.charAt(i);
	        }
	        i++;
	    }
	    return result;
	};
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	 Html5Entities.encodeNonUTF = function(str) {
	    return new Html5Entities().encodeNonUTF(str);
	 };
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	Html5Entities.prototype.encodeNonASCII = function(str) {
	    var strLength = str.length;
	    if (strLength === 0) {
	        return '';
	    }
	    var result = '';
	    var i = 0;
	    while (i < strLength) {
	        var c = str.charCodeAt(i);
	        if (c <= 255) {
	            result += str[i++];
	            continue;
	        }
	        result += '&#' + c + ';';
	        i++
	    }
	    return result;
	};
	
	/**
	 * @param {String} str
	 * @returns {String}
	 */
	 Html5Entities.encodeNonASCII = function(str) {
	    return new Html5Entities().encodeNonASCII(str);
	 };
	
	/**
	 * @param {Object} alphaIndex Passed by reference.
	 * @param {Object} charIndex Passed by reference.
	 */
	function createIndexes(alphaIndex, charIndex) {
	    var i = ENTITIES.length;
	    var _results = [];
	    while (i--) {
	        var e = ENTITIES[i];
	        var alpha = e[0];
	        var chars = e[1];
	        var chr = chars[0];
	        var addChar = (chr < 32 || chr > 126) || chr === 62 || chr === 60 || chr === 38 || chr === 34 || chr === 39;
	        var charInfo;
	        if (addChar) {
	            charInfo = charIndex[chr] = charIndex[chr] || {};
	        }
	        if (chars[1]) {
	            var chr2 = chars[1];
	            alphaIndex[alpha] = String.fromCharCode(chr) + String.fromCharCode(chr2);
	            _results.push(addChar && (charInfo[chr2] = alpha));
	        } else {
	            alphaIndex[alpha] = String.fromCharCode(chr);
	            _results.push(addChar && (charInfo[''] = alpha));
	        }
	    }
	}
	
	module.exports = Html5Entities;


/***/ },
/* 14 */
/***/ function(module, exports, __webpack_require__) {

	/**
	 * Based heavily on https://github.com/webpack/webpack/blob/
	 *  c0afdf9c6abc1dd70707c594e473802a566f7b6e/hot/only-dev-server.js
	 * Original copyright Tobias Koppers @sokra (MIT license)
	 */
	
	/* global window __webpack_hash__ */
	
	if (false) {
	  throw new Error("[HMR] Hot Module Replacement is disabled.");
	}
	
	var hmrDocsUrl = "http://webpack.github.io/docs/hot-module-replacement-with-webpack.html"; // eslint-disable-line max-len
	
	var lastHash;
	var failureStatuses = { abort: 1, fail: 1 };
	var applyOptions = { ignoreUnaccepted: true };
	
	function upToDate(hash) {
	  if (hash) lastHash = hash;
	  return lastHash == __webpack_require__.h();
	}
	
	module.exports = function(hash, moduleMap, options) {
	  var reload = options.reload;
	  if (!upToDate(hash) && module.hot.status() == "idle") {
	    if (options.log) console.log("[HMR] Checking for updates on the server...");
	    check();
	  }
	
	  function check() {
	    var cb = function(err, updatedModules) {
	      if (err) return handleError(err);
	
	      if(!updatedModules) {
	        if (options.warn) {
	          console.warn("[HMR] Cannot find update (Full reload needed)");
	          console.warn("[HMR] (Probably because of restarting the server)");
	        }
	        performReload();
	        return null;
	      }
	
	      var applyCallback = function(applyErr, renewedModules) {
	        if (applyErr) return handleError(applyErr);
	
	        if (!upToDate()) check();
	
	        logUpdates(updatedModules, renewedModules);
	      };
	
	      var applyResult = module.hot.apply(applyOptions, applyCallback);
	      // webpack 2 promise
	      if (applyResult && applyResult.then) {
	        // HotModuleReplacement.runtime.js refers to the result as `outdatedModules`
	        applyResult.then(function(outdatedModules) {
	          applyCallback(null, outdatedModules);
	        });
	        applyResult.catch(applyCallback);
	      }
	
	    };
	
	    var result = module.hot.check(false, cb);
	    // webpack 2 promise
	    if (result && result.then) {
	        result.then(function(updatedModules) {
	            cb(null, updatedModules);
	        });
	        result.catch(cb);
	    }
	  }
	
	  function logUpdates(updatedModules, renewedModules) {
	    var unacceptedModules = updatedModules.filter(function(moduleId) {
	      return renewedModules && renewedModules.indexOf(moduleId) < 0;
	    });
	
	    if(unacceptedModules.length > 0) {
	      if (options.warn) {
	        console.warn(
	          "[HMR] The following modules couldn't be hot updated: " +
	          "(Full reload needed)\n" +
	          "This is usually because the modules which have changed " +
	          "(and their parents) do not know how to hot reload themselves. " +
	          "See " + hmrDocsUrl + " for more details."
	        );
	        unacceptedModules.forEach(function(moduleId) {
	          console.warn("[HMR]  - " + moduleMap[moduleId]);
	        });
	      }
	      performReload();
	      return;
	    }
	
	    if (options.log) {
	      if(!renewedModules || renewedModules.length === 0) {
	        console.log("[HMR] Nothing hot updated.");
	      } else {
	        console.log("[HMR] Updated modules:");
	        renewedModules.forEach(function(moduleId) {
	          console.log("[HMR]  - " + moduleMap[moduleId]);
	        });
	      }
	
	      if (upToDate()) {
	        console.log("[HMR] App is up to date.");
	      }
	    }
	  }
	
	  function handleError(err) {
	    if (module.hot.status() in failureStatuses) {
	      if (options.warn) {
	        console.warn("[HMR] Cannot check for update (Full reload needed)");
	        console.warn("[HMR] " + err.stack || err.message);
	      }
	      performReload();
	      return;
	    }
	    if (options.warn) {
	      console.warn("[HMR] Update check failed: " + err.stack || err.message);
	    }
	  }
	
	  function performReload() {
	    if (reload) {
	      if (options.warn) console.warn("[HMR] Reloading page");
	      window.location.reload();
	    }
	  }
	};


/***/ },
/* 15 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {"use strict";
	__webpack_require__(16);
	__webpack_require__(18);
	__webpack_require__(19);
	var platform_browser_dynamic_1 = __webpack_require__(20);
	var core_1 = __webpack_require__(21);
	var app_module_1 = __webpack_require__(22);
	var platform = platform_browser_dynamic_1.platformBrowserDynamic();
	if (module['hot']) {
	    module['hot'].accept();
	    module['hot'].dispose(function () { platform.destroy(); });
	}
	else {
	    core_1.enableProdMode();
	}
	platform.bootstrapModule(app_module_1.AppModule);
	
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(2)(module)))

/***/ },
/* 16 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = (__webpack_require__(17))(273);

/***/ },
/* 17 */
/***/ function(module, exports) {

	module.exports = vendor_0ab6c82201444f6c8074;

/***/ },
/* 18 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = (__webpack_require__(17))(275);

/***/ },
/* 19 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = (__webpack_require__(17))(629);

/***/ },
/* 20 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = (__webpack_require__(17))(297);

/***/ },
/* 21 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = (__webpack_require__(17))(279);

/***/ },
/* 22 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	// angular 2
	var core_1 = __webpack_require__(21);
	var platform_browser_1 = __webpack_require__(23);
	var forms_1 = __webpack_require__(24);
	var http_1 = __webpack_require__(25);
	var material_1 = __webpack_require__(26);
	var flex_layout_1 = __webpack_require__(27);
	var angular2localization_1 = __webpack_require__(31);
	// app
	var core_module_1 = __webpack_require__(32);
	var app_routing_module_1 = __webpack_require__(56);
	var app_nav_component_1 = __webpack_require__(66);
	var app_component_1 = __webpack_require__(68);
	// home
	var home_component_1 = __webpack_require__(58);
	// user
	var user_list_component_1 = __webpack_require__(60);
	var user_edit_component_1 = __webpack_require__(63);
	__webpack_require__(70);
	var AppModule = (function () {
	    function AppModule() {
	    }
	    AppModule = __decorate([
	        core_1.NgModule({
	            imports: [
	                platform_browser_1.BrowserModule,
	                forms_1.FormsModule,
	                http_1.HttpModule,
	                flex_layout_1.FlexLayoutModule.forRoot(),
	                material_1.MaterialModule.forRoot(),
	                angular2localization_1.LocaleModule.forRoot(),
	                angular2localization_1.LocalizationModule.forRoot(),
	                core_module_1.CoreModule,
	                app_routing_module_1.AppRoutingModule,
	            ],
	            declarations: [
	                app_component_1.AppComponent,
	                app_nav_component_1.AppNavComponent,
	                home_component_1.HomeComponent,
	                user_list_component_1.UserListComponent,
	                user_edit_component_1.UserEditComponent
	            ],
	            bootstrap: [app_component_1.AppComponent]
	        }), 
	        __metadata('design:paramtypes', [])
	    ], AppModule);
	    return AppModule;
	}());
	exports.AppModule = AppModule;


/***/ },
/* 23 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = (__webpack_require__(17))(277);

/***/ },
/* 24 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = (__webpack_require__(17))(299);

/***/ },
/* 25 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = (__webpack_require__(17))(303);

/***/ },
/* 26 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = (__webpack_require__(17))(334);

/***/ },
/* 27 */
/***/ function(module, exports, __webpack_require__) {

	(function (global, factory) {
	     true ? factory(exports, __webpack_require__(28), __webpack_require__(29), __webpack_require__(21), __webpack_require__(30)) :
	    typeof define === 'function' && define.amd ? define(['exports', 'rxjs/add/operator/map', 'rxjs/add/operator/filter', '@angular/core', 'rxjs/BehaviorSubject'], factory) :
	    (factory((global.ng = global.ng || {}, global.ng.flexLayout = global.ng.flexLayout || {}),global.Rx.Observable.prototype,global.Rx.Observable.prototype,global.ng.core,global.Rx));
	}(this, (function (exports,rxjs_add_operator_map,rxjs_add_operator_filter,_angular_core,rxjs_BehaviorSubject) { 'use strict';
	
	/** @internal Applies CSS prefixes to appropriate style keys.*/
	function applyCssPrefixes(target) {
	    for (var key in target) {
	        var value = target[key];
	        switch (key) {
	            case 'display':
	                target['display'] = value;
	                // also need 'display : -webkit-box' and 'display : -ms-flexbox;'
	                break;
	            case 'flex':
	                target['-ms-flex'] = value;
	                target['-webkit-box-flex'] = value.split(" ")[0];
	                break;
	            case 'flex-direction':
	                value = value || "row";
	                target['flex-direction'] = value;
	                target['-ms-flex-direction'] = value;
	                target['-webkit-box-orient'] = toBoxOrient(value);
	                target['-webkit-box-direction'] = toBoxDirection(value);
	                break;
	            case 'flex-wrap':
	                target['-ms-flex-wrap'] = value;
	                break;
	            case 'order':
	                if (isNaN(value)) {
	                    value = "0";
	                }
	                target['order'] = value;
	                target['-ms-flex-order'] = value;
	                target['-webkit-box-ordinal-group'] = toBoxOrdinal(value);
	                break;
	            case 'justify-content':
	                target['-ms-flex-pack'] = toBoxValue(value);
	                target['-webkit-box-pack'] = toBoxValue(value);
	                break;
	            case 'align-items':
	                target['-ms-flex-align'] = toBoxValue(value);
	                target['-webkit-box-align'] = toBoxValue(value);
	                break;
	            case 'align-self':
	                target['-ms-flex-item-align'] = toBoxValue(value);
	                break;
	            case 'align-content':
	                target['-ms-align-content'] = toAlignContentValue(value);
	                target['-ms-flex-line-pack'] = toAlignContentValue(value);
	                break;
	        }
	    }
	    return target;
	}
	function toAlignContentValue(value) {
	    switch (value) {
	        case "space-between": return "justify";
	        case "space-around": return "distribute";
	        default:
	            return toBoxValue(value);
	    }
	}
	/** @internal Convert flex values flex-start, flex-end to start, end. */
	function toBoxValue(value) {
	    if (value === void 0) { value = ""; }
	    return (value == 'flex-start') ? 'start' : ((value == 'flex-end') ? 'end' : value);
	}
	/** @internal Convert flex Direction to Box orientation */
	function toBoxOrient(flexDirection) {
	    if (flexDirection === void 0) { flexDirection = 'row'; }
	    return flexDirection.indexOf('column') === -1 ? 'horizontal' : 'vertical';
	}
	/** @internal Convert flex Direction to Box direction type */
	function toBoxDirection(flexDirection) {
	    if (flexDirection === void 0) { flexDirection = 'row'; }
	    return flexDirection.indexOf('reverse') !== -1 ? 'reverse' : 'normal';
	}
	/** @internal Convert flex order to Box ordinal group */
	function toBoxOrdinal(order) {
	    if (order === void 0) { order = '0'; }
	    var value = order ? parseInt(order) + 1 : 1;
	    return isNaN(value) ? "0" : value.toString();
	}
	
	/**
	 * @internal
	 *
	 * Extends an object with the *enumerable* and *own* properties of one or more source objects,
	 * similar to Object.assign.
	 *
	 * @param dest The object which will have properties copied to it.
	 * @param sources The source objects from which properties will be copied.
	 */
	function extendObject(dest) {
	    var sources = [];
	    for (var _i = 1; _i < arguments.length; _i++) {
	        sources[_i - 1] = arguments[_i];
	    }
	    if (dest == null) {
	        throw TypeError('Cannot convert undefined or null to object');
	    }
	    for (var _a = 0, sources_1 = sources; _a < sources_1.length; _a++) {
	        var source = sources_1[_a];
	        if (source != null) {
	            for (var key in source) {
	                if (source.hasOwnProperty(key)) {
	                    dest[key] = source[key];
	                }
	            }
	        }
	    }
	    return dest;
	}
	
	/** @internal  */
	var KeyOptions = (function () {
	    function KeyOptions(baseKey, defaultValue, inputKeys) {
	        this.baseKey = baseKey;
	        this.defaultValue = defaultValue;
	        this.inputKeys = inputKeys;
	    }
	    return KeyOptions;
	}());
	/**
	 * @internal
	 *
	 * ResponsiveActivation acts as a proxy between the MonitorMedia service (which emits mediaQuery changes)
	 * and the fx API directives. The MQA proxies mediaQuery change events and notifies the directive
	 * via the specified callback.
	 *
	 * - The MQA also determines which directive property should be used to determine the
	 *   current change 'value'... BEFORE the original `onMediaQueryChanges()` method is called.
	 * - The `ngOnDestroy()` method is also head-hooked to enable auto-unsubscribe from the MediaQueryServices.
	 *
	 * NOTE: these interceptions enables the logic in the fx API directives to remain terse and clean.
	 */
	var ResponsiveActivation = (function () {
	    /**
	     * Constructor
	     */
	    function ResponsiveActivation(_options, _mediaMonitor, _onMediaChanges) {
	        this._options = _options;
	        this._mediaMonitor = _mediaMonitor;
	        this._onMediaChanges = _onMediaChanges;
	        this._subscribers = [];
	        this._subscribers = this._configureChangeObservers();
	    }
	    Object.defineProperty(ResponsiveActivation.prototype, "mediaMonitor", {
	        /**
	         * Accessor to the DI'ed directive property
	         * Each directive instance has a reference to the MediaMonitor which is
	         * used HERE to subscribe to mediaQuery change notifications.
	         */
	        get: function () {
	            return this._mediaMonitor;
	        },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(ResponsiveActivation.prototype, "activatedInputKey", {
	        /**
	         * Determine which directive @Input() property is currently active (for the viewport size):
	         * The key must be defined (in use) or fallback to the 'closest' overlapping property key
	         * that is defined; otherwise the default property key will be used.
	         * e.g.
	         *      if `<div fxHide fxHide.gt-sm="false">` is used but the current activated mediaQuery alias
	         *      key is `.md` then `.gt-sm` should be used instead
	         */
	        get: function () {
	            return this._activatedInputKey || this._options.baseKey;
	        },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(ResponsiveActivation.prototype, "activatedInput", {
	        /**
	         * Get the currently activated @Input value or the fallback default @Input value
	         */
	        get: function () {
	            var key = this.activatedInputKey;
	            return this._hasKeyValue(key) ? this._lookupKeyValue(key) : this._options.defaultValue;
	        },
	        enumerable: true,
	        configurable: true
	    });
	    /**
	     * Remove interceptors, restore original functions, and forward the onDestroy() call
	     */
	    ResponsiveActivation.prototype.destroy = function () {
	        this._subscribers.forEach(function (link) {
	            link.unsubscribe();
	        });
	        this._subscribers = [];
	    };
	    /**
	     * For each *defined* API property, register a callback to `_onMonitorEvents( )`
	     * Cache 1..n subscriptions for internal auto-unsubscribes when the the directive destructs
	     */
	    ResponsiveActivation.prototype._configureChangeObservers = function () {
	        var _this = this;
	        var subscriptions = [];
	        this._buildRegistryMap().forEach(function (bp) {
	            if (_this._keyInUse(bp.key)) {
	                // Inject directive default property key name: to let onMediaChange() calls
	                // know which property is being triggered...
	                var buildChanges = function (change) {
	                    change.property = _this._options.baseKey;
	                    return change;
	                };
	                subscriptions.push(_this.mediaMonitor.observe(bp.alias)
	                    .map(buildChanges)
	                    .subscribe(function (change) {
	                    _this._onMonitorEvents(change);
	                }));
	            }
	        });
	        return subscriptions;
	    };
	    /**
	     * Build mediaQuery key-hashmap; only for the directive properties that are actually defined/used
	     * in the HTML markup
	     */
	    ResponsiveActivation.prototype._buildRegistryMap = function () {
	        var _this = this;
	        return this.mediaMonitor.breakpoints
	            .map(function (bp) {
	            return extendObject({}, bp, {
	                baseKey: _this._options.baseKey,
	                key: _this._options.baseKey + bp.suffix // e.g.  layoutGtSm, layoutMd, layoutGtLg
	            });
	        })
	            .filter(function (bp) { return _this._keyInUse(bp.key); });
	    };
	    /**
	     * Synchronizes change notifications with the current mq-activated @Input and calculates the
	     * mq-activated input value or the default value
	     */
	    ResponsiveActivation.prototype._onMonitorEvents = function (change) {
	        if (change.property == this._options.baseKey) {
	            change.value = this._calculateActivatedValue(change);
	            this._onMediaChanges(change);
	        }
	    };
	    /**
	     * Has the key been specified in the HTML markup and thus is intended
	     * to participate in activation processes.
	     */
	    ResponsiveActivation.prototype._keyInUse = function (key) {
	        return this._lookupKeyValue(key) !== undefined;
	    };
	    /**
	     *  Map input key associated with mediaQuery activation to closest defined input key
	     *  then return the values associated with the targeted input property
	     *
	     *  !! change events may arrive out-of-order (activate before deactivate)
	     *     so make sure the deactivate is used ONLY when the keys match
	     *     (since a different activate may be in use)
	     */
	    ResponsiveActivation.prototype._calculateActivatedValue = function (current) {
	        var currentKey = this._options.baseKey + current.suffix; // e.g. suffix == 'GtSm', _baseKey == 'hide'
	        var newKey = this._activatedInputKey; // e.g. newKey == hideGtSm
	        newKey = current.matches ? currentKey : ((newKey == currentKey) ? null : newKey);
	        this._activatedInputKey = this._validateInputKey(newKey);
	        return this.activatedInput;
	    };
	    /**
	     * For the specified input property key, validate it is defined (used in the markup)
	     * If not see if a overlapping mediaQuery-related input key fallback has been defined
	     *
	     * NOTE: scans in the order defined by activeOverLaps (largest viewport ranges -> smallest ranges)
	     */
	    ResponsiveActivation.prototype._validateInputKey = function (inputKey) {
	        var _this = this;
	        var items = this.mediaMonitor.activeOverlaps;
	        var isMissingKey = function (key) { return !_this._keyInUse(key); };
	        if (isMissingKey(inputKey)) {
	            items.some(function (bp) {
	                var key = _this._options.baseKey + bp.suffix;
	                if (!isMissingKey(key)) {
	                    inputKey = key;
	                    return true; // exit .some()
	                }
	                return false;
	            });
	        }
	        return inputKey;
	    };
	    /**
	     * Get the value (if any) for the directive instances @Input property (aka key)
	     */
	    ResponsiveActivation.prototype._lookupKeyValue = function (key) {
	        return this._options.inputKeys[key];
	    };
	    ResponsiveActivation.prototype._hasKeyValue = function (key) {
	        var value = this._options.inputKeys[key];
	        return typeof value !== 'undefined';
	    };
	    return ResponsiveActivation;
	}());
	
	/** Abstract base class for the Layout API styling directives. */
	var BaseFxDirective = (function () {
	    /**
	     *
	     */
	    function BaseFxDirective(_mediaMonitor, _elementRef, _renderer) {
	        this._mediaMonitor = _mediaMonitor;
	        this._elementRef = _elementRef;
	        this._renderer = _renderer;
	        /**
	         *  Dictionary of input keys with associated values
	         */
	        this._inputMap = {};
	    }
	    // *********************************************
	    // Accessor Methods
	    // *********************************************
	    /**
	     * Access the current value (if any) of the @Input property.
	     */
	    BaseFxDirective.prototype._queryInput = function (key) {
	        return this._inputMap[key];
	    };
	    // *********************************************
	    // Lifecycle Methods
	    // *********************************************
	    BaseFxDirective.prototype.ngOnDestroy = function () {
	        if (this._mqActivation) {
	            this._mqActivation.destroy();
	        }
	        this._mediaMonitor = null;
	    };
	    // *********************************************
	    // Protected Methods
	    // *********************************************
	    /**
	     * Applies styles given via string pair or object map to the directive element.
	     */
	    BaseFxDirective.prototype._applyStyleToElement = function (style, value, nativeElement) {
	        var styles = {};
	        var element = nativeElement || this._elementRef.nativeElement;
	        if (typeof style === 'string') {
	            styles[style] = value;
	            style = styles;
	        }
	        styles = applyCssPrefixes(style);
	        // Iterate all properties in hashMap and set styles
	        for (var key in styles) {
	            this._renderer.setElementStyle(element, key, styles[key]);
	        }
	    };
	    /**
	     * Applies styles given via string pair or object map to the directive element.
	     */
	    BaseFxDirective.prototype._applyStyleToElements = function (style, elements) {
	        var _this = this;
	        var styles = applyCssPrefixes(style);
	        elements.forEach(function (el) {
	            // Iterate all properties in hashMap and set styles
	            for (var key in styles) {
	                _this._renderer.setElementStyle(el, key, styles[key]);
	            }
	        });
	    };
	    /**
	     *  Save the property value; which may be a complex object.
	     *  Complex objects support property chains
	     */
	    BaseFxDirective.prototype._cacheInput = function (key, source) {
	        if (typeof source === 'object') {
	            for (var prop in source) {
	                this._inputMap[prop] = source[prop];
	            }
	        }
	        else {
	            this._inputMap[key] = source;
	        }
	    };
	    /**
	     *  Build a ResponsiveActivation object used to manage subscriptions to mediaChange notifications
	     *  and intelligent lookup of the directive's property value that corresponds to that mediaQuery
	     *  (or closest match).
	     */
	    BaseFxDirective.prototype._listenForMediaQueryChanges = function (key, defaultValue, onMediaQueryChange) {
	        var _this = this;
	        var keyOptions = new KeyOptions(key, defaultValue, this._inputMap);
	        return this._mqActivation = new ResponsiveActivation(keyOptions, this._mediaMonitor, function (change) { return onMediaQueryChange.call(_this, change); });
	    };
	    Object.defineProperty(BaseFxDirective.prototype, "childrenNodes", {
	        /**
	         * Special accessor to query for all child 'element' nodes regardless of type, class, etc.
	         */
	        get: function () {
	            var obj = this._elementRef.nativeElement.childNodes;
	            var array = [];
	            // iterate backwards ensuring that length is an UInt32
	            for (var i = obj.length; i--;) {
	                array[i] = obj[i];
	            }
	            return array;
	        },
	        enumerable: true,
	        configurable: true
	    });
	    return BaseFxDirective;
	}());
	
	var RESPONSIVE_ALIASES = ['xs', 'gt-xs', 'sm', 'gt-sm', 'md', 'gt-md', 'lg', 'gt-lg', 'xl'];
	var RAW_DEFAULTS = [
	    {
	        alias: 'xs',
	        suffix: 'Xs',
	        overlapping: false,
	        mediaQuery: 'screen and (max-width: 599px)'
	    },
	    {
	        alias: 'gt-xs',
	        suffix: 'GtXs',
	        overlapping: true,
	        mediaQuery: 'screen and (min-width: 600px)'
	    },
	    {
	        alias: 'sm',
	        suffix: 'Sm',
	        overlapping: false,
	        mediaQuery: 'screen and (min-width: 600px) and (max-width: 959px)'
	    },
	    {
	        alias: 'gt-sm',
	        suffix: 'GtSm',
	        overlapping: true,
	        mediaQuery: 'screen and (min-width: 960px)'
	    },
	    {
	        alias: 'md',
	        suffix: 'Md',
	        overlapping: false,
	        mediaQuery: 'screen and (min-width: 960px) and (max-width: 1279px)'
	    },
	    {
	        alias: 'gt-md',
	        suffix: 'GtMd',
	        overlapping: true,
	        mediaQuery: 'screen and (min-width: 1280px)'
	    },
	    {
	        alias: 'lg',
	        suffix: 'Lg',
	        overlapping: false,
	        mediaQuery: 'screen and (min-width: 1280px) and (max-width: 1919px)'
	    },
	    {
	        alias: 'gt-lg',
	        suffix: 'GtLg',
	        overlapping: true,
	        mediaQuery: 'screen and (min-width: 1920px)'
	    },
	    {
	        alias: 'xl',
	        suffix: 'Xl',
	        overlapping: false,
	        mediaQuery: 'screen and (min-width: 1921px)' // should be distinct from 'gt-lg' range
	    }
	];
	/**
	 *  Opaque Token unique to the flex-layout library.
	 *  Use this token when build a custom provider (see below).
	 */
	var BREAKPOINTS = new _angular_core.OpaqueToken('fxRawBreakpoints');
	/**
	 *  Provider to return observable to ALL known BreakPoint(s)
	 *  Developers should build custom providers to override this default BreakPointRegistry dataset provider
	 *  NOTE: !! custom breakpoints lists MUST contain the following aliases & suffixes:
	 *        [xs, gt-xs, sm, gt-sm, md, gt-md, lg, gt-lg, xl]
	 */
	var BreakPointsProvider = {
	    provide: BREAKPOINTS,
	    useValue: RAW_DEFAULTS
	};
	
	var __decorate$2 = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata$2 = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var __param = (this && this.__param) || function (paramIndex, decorator) {
	    return function (target, key) { decorator(target, key, paramIndex); }
	};
	/**
	 * @internal
	 *
	 * Registry of 1..n MediaQuery breakpoint ranges
	 * This is published as a provider and may be overriden from custom, application-specific ranges
	 *
	 */
	var BreakPointRegistry = (function () {
	    function BreakPointRegistry(_registry) {
	        this._registry = _registry;
	    }
	    Object.defineProperty(BreakPointRegistry.prototype, "items", {
	        /**
	         * Accessor to raw list
	         */
	        get: function () {
	            return this._registry.slice();
	        },
	        enumerable: true,
	        configurable: true
	    });
	    /**
	     * Search breakpoints by alias (e.g. gt-xs)
	     */
	    BreakPointRegistry.prototype.findByAlias = function (alias) {
	        return this._registry.find(function (bp) { return bp.alias == alias; });
	    };
	    BreakPointRegistry.prototype.findByQuery = function (query) {
	        return this._registry.find(function (bp) { return bp.mediaQuery == query; });
	    };
	    Object.defineProperty(BreakPointRegistry.prototype, "overlappings", {
	        /**
	         * Get all the breakpoints whose ranges could overlapping `normal` ranges;
	         * e.g. gt-sm overlaps md, lg, and xl
	         */
	        get: function () {
	            return this._registry.filter(function (it) { return it.overlapping == true; });
	        },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(BreakPointRegistry.prototype, "aliases", {
	        /**
	         * Get list of all registered (non-empty) breakpoint aliases
	         */
	        get: function () {
	            return this._registry.map(function (it) { return it.alias; });
	        },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(BreakPointRegistry.prototype, "suffixes", {
	        /**
	         * Aliases are mapped to properties using suffixes
	         * e.g.  'gt-sm' for property 'layout'  uses suffix 'GtSm'
	         * for property layoutGtSM.
	         */
	        get: function () {
	            return this._registry.map(function (it) { return it.suffix; });
	        },
	        enumerable: true,
	        configurable: true
	    });
	    BreakPointRegistry = __decorate$2([
	        _angular_core.Injectable(),
	        __param(0, _angular_core.Inject(BREAKPOINTS)), 
	        __metadata$2('design:paramtypes', [Array])
	    ], BreakPointRegistry);
	    return BreakPointRegistry;
	}());
	
	/**
	 * Class instances emitted [to observers] for each mql notification
	 */
	var MediaChange = (function () {
	    function MediaChange(matches, // Is the mq currently activated
	        mediaQuery, // e.g.   screen and (min-width: 600px) and (max-width: 959px)
	        mqAlias, // e.g.   gt-sm, md, gt-lg
	        suffix // e.g.   GtSM, Md, GtLg
	        ) {
	        if (matches === void 0) { matches = false; }
	        if (mediaQuery === void 0) { mediaQuery = 'all'; }
	        if (mqAlias === void 0) { mqAlias = ''; }
	        if (suffix === void 0) { suffix = ''; }
	        this.matches = matches;
	        this.mediaQuery = mediaQuery;
	        this.mqAlias = mqAlias;
	        this.suffix = suffix;
	    }
	    return MediaChange;
	}());
	
	var __decorate$3 = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata$3 = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	/**
	 *  Opaque Token unique to the flex-layout library.
	 *  Note: Developers must use this token when building their own custom `MatchMediaObservableProvider`
	 *  provider.
	 *
	 *  @see ./providers/match-media-observable-provider.ts
	 */
	var MatchMediaObservable = new _angular_core.OpaqueToken('fxObservableMatchMedia');
	/**
	 * MediaMonitor configures listeners to mediaQuery changes and publishes an Observable facade to convert
	 * mediaQuery change callbacks to subscriber notifications. These notifications will be performed within the
	 * ng Zone to trigger change detections and component updates.
	 *
	 * NOTE: both mediaQuery activations and de-activations are announced in notifications
	 */
	var MatchMedia = (function () {
	    function MatchMedia(_zone) {
	        this._zone = _zone;
	        this._registry = new Map();
	        this._source = new rxjs_BehaviorSubject.BehaviorSubject(new MediaChange(true));
	        this._observable$ = this._source.asObservable();
	    }
	    /**
	     * For the specified mediaQuery?
	     */
	    MatchMedia.prototype.isActive = function (mediaQuery) {
	        if (this._registry.has(mediaQuery)) {
	            var mql = this._registry.get(mediaQuery);
	            return mql.matches;
	        }
	        return false;
	    };
	    /**
	     * External observers can watch for all (or a specific) mql changes.
	     * Typically used by the MediaQueryAdaptor; optionally available to components
	     * who wish to use the MediaMonitor as mediaMonitor$ observable service.
	     *
	     * NOTE: if a mediaQuery is not specified, then ALL mediaQuery activations will
	     *       be announced.
	     */
	    MatchMedia.prototype.observe = function (mediaQuery) {
	        this.registerQuery(mediaQuery);
	        return this._observable$.filter(function (change) {
	            return mediaQuery ? (change.mediaQuery === mediaQuery) : true;
	        });
	    };
	    /**
	     * Based on the BreakPointRegistry provider, register internal listeners for each unique mediaQuery
	     * Each listener emits specific MediaChange data to observers
	     */
	    MatchMedia.prototype.registerQuery = function (mediaQuery) {
	        var _this = this;
	        if (mediaQuery) {
	            var mql = this._registry.get(mediaQuery);
	            var onMQLEvent = function (mql) {
	                _this._zone.run(function () {
	                    var change = new MediaChange(mql.matches, mediaQuery);
	                    _this._source.next(change);
	                });
	            };
	            if (!mql) {
	                mql = this._buildMQL(mediaQuery);
	                mql.addListener(onMQLEvent);
	                this._registry.set(mediaQuery, mql);
	            }
	            if (mql.matches) {
	                onMQLEvent(mql); // Announce activate range for initial subscribers
	            }
	        }
	    };
	    /**
	     * Call window.matchMedia() to build a MediaQueryList; which
	     * supports 0..n listeners for activation/deactivation
	     */
	    MatchMedia.prototype._buildMQL = function (query) {
	        prepareQueryCSS(query);
	        var canListen = !!window.matchMedia('all').addListener;
	        return canListen ? window.matchMedia(query) : {
	            matches: query === 'all' || query === '',
	            media: query,
	            addListener: function () { },
	            removeListener: function () { }
	        };
	    };
	    MatchMedia = __decorate$3([
	        _angular_core.Injectable(), 
	        __metadata$3('design:paramtypes', [_angular_core.NgZone])
	    ], MatchMedia);
	    return MatchMedia;
	}());
	/**
	 * Private global registry for all dynamically-created, injected style tags
	 * @see prepare(query)
	 */
	var ALL_STYLES = {};
	/**
	 * For Webkit engines that only trigger the MediaQueryListListener
	 * when there is at least one CSS selector for the respective media query.
	 *
	 * @param query string The mediaQuery used to create a faux CSS selector
	 *
	 */
	function prepareQueryCSS(query) {
	    if (!ALL_STYLES[query]) {
	        try {
	            var style = document.createElement('style');
	            style.setAttribute('type', 'text/css');
	            if (!style['styleSheet']) {
	                var cssText = "@media " + query + " {.fx-query-test{ }}";
	                style.appendChild(document.createTextNode(cssText));
	            }
	            document.getElementsByTagName('head')[0].appendChild(style);
	            // Store in private global registry
	            ALL_STYLES[query] = style;
	        }
	        catch (e) {
	            console.error(e);
	        }
	    }
	}
	
	/**
	 * @internal
	 *
	 * For the specified MediaChange, make sure it contains the breakpoint alias
	 * and suffix (if available).
	 */
	function mergeAlias(dest, source) {
	    return extendObject(dest, source ? {
	        mqAlias: source.alias,
	        suffix: source.suffix
	    } : {});
	}
	
	var __decorate$1 = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata$1 = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	/**
	 * MediaMonitor uses the MatchMedia service to observe mediaQuery changes (both activations and
	 * deactivations). These changes are are published as MediaChange notifications.
	 *
	 * Note: all notifications will be performed within the
	 * ng Zone to trigger change detections and component updates.
	 *
	 * It is the MediaMonitor that:
	 *  - auto registers all known breakpoints
	 *  - injects alias information into each raw MediaChange event
	 *  - provides accessor to the currently active BreakPoint
	 *  - publish list of overlapping BreakPoint(s); used by ResponsiveActivation
	 */
	var MediaMonitor = (function () {
	    function MediaMonitor(_breakpoints, _matchMedia) {
	        this._breakpoints = _breakpoints;
	        this._matchMedia = _matchMedia;
	        this._registerBreakpoints();
	    }
	    Object.defineProperty(MediaMonitor.prototype, "breakpoints", {
	        /**
	         * Read-only accessor to the list of breakpoints configured in the BreakPointRegistry provider
	         */
	        get: function () {
	            return this._breakpoints.items.slice();
	        },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(MediaMonitor.prototype, "activeOverlaps", {
	        get: function () {
	            var _this = this;
	            var items = this._breakpoints.overlappings.reverse();
	            return items.filter(function (bp) {
	                return _this._matchMedia.isActive(bp.mediaQuery);
	            });
	        },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(MediaMonitor.prototype, "active", {
	        get: function () {
	            var _this = this;
	            var found = null, items = this.breakpoints.reverse();
	            items.forEach(function (bp) {
	                if (bp.alias !== '') {
	                    if (!found && _this._matchMedia.isActive(bp.mediaQuery)) {
	                        found = bp;
	                    }
	                }
	            });
	            var first = this.breakpoints[0];
	            return found || (this._matchMedia.isActive(first.mediaQuery) ? first : null);
	        },
	        enumerable: true,
	        configurable: true
	    });
	    /**
	     * For the specified mediaQuery alias, is the mediaQuery range active?
	     */
	    MediaMonitor.prototype.isActive = function (alias) {
	        var bp = this._breakpoints.findByAlias(alias) || this._breakpoints.findByQuery(alias);
	        return this._matchMedia.isActive(bp ? bp.mediaQuery : alias);
	    };
	    /**
	     * External observers can watch for all (or a specific) mql changes.
	     * If specific breakpoint is observed, only return *activated* events
	     * otherwise return all events for BOTH activated + deactivated changes.
	     */
	    MediaMonitor.prototype.observe = function (alias) {
	        var bp = this._breakpoints.findByAlias(alias) || this._breakpoints.findByQuery(alias);
	        var hasAlias = function (change) { return (bp ? change.mqAlias !== "" : true); };
	        // Note: the raw MediaChange events [from MatchMedia] do not contain important alias information
	        return this._matchMedia
	            .observe(bp ? bp.mediaQuery : alias)
	            .map(function (change) { return mergeAlias(change, bp); })
	            .filter(hasAlias);
	    };
	    /**
	     * Immediate calls to matchMedia() to establish listeners
	     * and prepare for immediate subscription notifications
	     */
	    MediaMonitor.prototype._registerBreakpoints = function () {
	        var _this = this;
	        this._breakpoints.items.forEach(function (bp) {
	            _this._matchMedia.registerQuery(bp.mediaQuery);
	        });
	    };
	    MediaMonitor = __decorate$1([
	        _angular_core.Injectable(), 
	        __metadata$1('design:paramtypes', [BreakPointRegistry, MatchMedia])
	    ], MediaMonitor);
	    return MediaMonitor;
	}());
	
	/**
	 * This factory uses the BreakPoint Registry only to inject alias information into the raw MediaChange
	 * notification. For custom mediaQuery notifications, alias information will not be injected and those
	 * fields will be ''.
	 *
	 * !! Only activation mediaChange notifications are publised by the MatchMediaObservable
	 */
	function instanceOfMatchMediaObservable(mediaWatcher, breakpoints) {
	    var onlyActivations = function (change) { return change.matches === true; };
	    var findBreakpoint = function (mediaQuery) { return breakpoints.findByQuery(mediaQuery); };
	    var injectAlias = function (change) { return mergeAlias(change, findBreakpoint(change.mediaQuery)); };
	    // Note: the raw MediaChange events [from MatchMedia] do not contain important alias information
	    //       these must be injected into the MediaChange
	    return mediaWatcher.observe().filter(onlyActivations).map(injectAlias);
	}
	
	/**
	 *  Provider to return observable to ALL MediaQuery events
	 *  Developers should build custom providers to override this default MediaQuery Observable
	 */
	var MatchMediaObservableProvider = {
	    provide: MatchMediaObservable,
	    deps: [MatchMedia, BreakPointRegistry],
	    useFactory: instanceOfMatchMediaObservable
	};
	
	var __decorate$4 = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata$4 = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	/**
	 * *****************************************************************
	 * Define module for the MediaQuery API
	 * *****************************************************************
	 */
	var MediaQueriesModule = (function () {
	    function MediaQueriesModule() {
	    }
	    MediaQueriesModule.forRoot = function () {
	        return {
	            ngModule: MediaQueriesModule
	        };
	    };
	    MediaQueriesModule = __decorate$4([
	        _angular_core.NgModule({
	            providers: [
	                MatchMedia,
	                MediaMonitor,
	                BreakPointRegistry,
	                BreakPointsProvider,
	                MatchMediaObservableProvider // Allows easy subscription to the injectable `media$` matchMedia observable
	            ]
	        }), 
	        __metadata$4('design:paramtypes', [])
	    ], MediaQueriesModule);
	    return MediaQueriesModule;
	}());
	
	var __extends$1 = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var __decorate$6 = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata$6 = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var LAYOUT_VALUES = ['row', 'column', 'row-reverse', 'column-reverse'];
	/**
	 * 'layout' flexbox styling directive
	 * Defines the positioning flow direction for the child elements: row or column
	 * Optional values: column or row (default)
	 * @see https://css-tricks.com/almanac/properties/f/flex-direction/
	 *
	 */
	var LayoutDirective = (function (_super) {
	    __extends$1(LayoutDirective, _super);
	    /**
	     *
	     */
	    function LayoutDirective(monitor, elRef, renderer) {
	        _super.call(this, monitor, elRef, renderer);
	        this._announcer = new rxjs_BehaviorSubject.BehaviorSubject("row");
	        this.layout$ = this._announcer.asObservable();
	    }
	    Object.defineProperty(LayoutDirective.prototype, "layout", {
	        set: function (val) { this._cacheInput("layout", val); },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(LayoutDirective.prototype, "layoutXs", {
	        set: function (val) { this._cacheInput('layoutXs', val); },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(LayoutDirective.prototype, "layoutGtXs", {
	        set: function (val) { this._cacheInput('layoutGtXs', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutDirective.prototype, "layoutSm", {
	        set: function (val) { this._cacheInput('layoutSm', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutDirective.prototype, "layoutGtSm", {
	        set: function (val) { this._cacheInput('layoutGtSm', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutDirective.prototype, "layoutMd", {
	        set: function (val) { this._cacheInput('layoutMd', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutDirective.prototype, "layoutGtMd", {
	        set: function (val) { this._cacheInput('layoutGtMd', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutDirective.prototype, "layoutLg", {
	        set: function (val) { this._cacheInput('layoutLg', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutDirective.prototype, "layoutGtLg", {
	        set: function (val) { this._cacheInput('layoutGtLg', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutDirective.prototype, "layoutXl", {
	        set: function (val) { this._cacheInput('layoutXl', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    // *********************************************
	    // Lifecycle Methods
	    // *********************************************
	    /**
	     * On changes to any @Input properties...
	     * Default to use the non-responsive Input value ('fxLayout')
	     * Then conditionally override with the mq-activated Input's current value
	     */
	    LayoutDirective.prototype.ngOnChanges = function (changes) {
	        if (changes['layout'] != null || this._mqActivation) {
	            this._updateWithDirection();
	        }
	    };
	    /**
	     * After the initial onChanges, build an mqActivation object that bridges
	     * mql change events to onMediaQueryChange handlers
	     */
	    LayoutDirective.prototype.ngOnInit = function () {
	        var _this = this;
	        this._listenForMediaQueryChanges('layout', 'row', function (changes) {
	            _this._updateWithDirection(changes.value);
	        });
	        this._updateWithDirection();
	    };
	    // *********************************************
	    // Protected methods
	    // *********************************************
	    /**
	     * Validate the direction value and then update the host's inline flexbox styles
	     */
	    LayoutDirective.prototype._updateWithDirection = function (direction) {
	        direction = direction || this._queryInput("layout") || 'row';
	        if (this._mqActivation) {
	            direction = this._mqActivation.activatedInput;
	        }
	        direction = this._validateValue(direction);
	        // Update styles and announce to subscribers the *new* direction
	        this._applyStyleToElement(this._buildCSS(direction));
	        this._announcer.next(direction);
	    };
	    /**
	     * Build the CSS that should be assigned to the element instance
	     * BUG:
	     *
	     *   1) min-height on a column flex container won’t apply to its flex item children in IE 10-11.
	     *      Use height instead if possible; height : <xxx>vh;
	     *
	     * @todo - update all child containers to have "box-sizing: border-box"
	     *         This way any padding or border specified on the child elements are
	     *         laid out and drawn inside that element's specified width and height.
	     *
	     */
	    LayoutDirective.prototype._buildCSS = function (value) {
	        return { 'display': 'flex', 'box-sizing': 'border-box', 'flex-direction': value };
	    };
	    /**
	     * Validate the value to be one of the acceptable value options
	     * Use default fallback of "row"
	     */
	    LayoutDirective.prototype._validateValue = function (value) {
	        value = value ? value.toLowerCase() : '';
	        return LAYOUT_VALUES.find(function (x) { return x === value; }) ? value : LAYOUT_VALUES[0]; // "row"
	    };
	    __decorate$6([
	        _angular_core.Input('fxLayout'), 
	        __metadata$6('design:type', Object), 
	        __metadata$6('design:paramtypes', [Object])
	    ], LayoutDirective.prototype, "layout", null);
	    __decorate$6([
	        _angular_core.Input('fxLayout.xs'), 
	        __metadata$6('design:type', Object), 
	        __metadata$6('design:paramtypes', [Object])
	    ], LayoutDirective.prototype, "layoutXs", null);
	    __decorate$6([
	        _angular_core.Input('fxLayout.gt-xs'), 
	        __metadata$6('design:type', Object), 
	        __metadata$6('design:paramtypes', [Object])
	    ], LayoutDirective.prototype, "layoutGtXs", null);
	    __decorate$6([
	        _angular_core.Input('fxLayout.sm'), 
	        __metadata$6('design:type', Object), 
	        __metadata$6('design:paramtypes', [Object])
	    ], LayoutDirective.prototype, "layoutSm", null);
	    __decorate$6([
	        _angular_core.Input('fxLayout.gt-sm'), 
	        __metadata$6('design:type', Object), 
	        __metadata$6('design:paramtypes', [Object])
	    ], LayoutDirective.prototype, "layoutGtSm", null);
	    __decorate$6([
	        _angular_core.Input('fxLayout.md'), 
	        __metadata$6('design:type', Object), 
	        __metadata$6('design:paramtypes', [Object])
	    ], LayoutDirective.prototype, "layoutMd", null);
	    __decorate$6([
	        _angular_core.Input('fxLayout.gt-md'), 
	        __metadata$6('design:type', Object), 
	        __metadata$6('design:paramtypes', [Object])
	    ], LayoutDirective.prototype, "layoutGtMd", null);
	    __decorate$6([
	        _angular_core.Input('fxLayout.lg'), 
	        __metadata$6('design:type', Object), 
	        __metadata$6('design:paramtypes', [Object])
	    ], LayoutDirective.prototype, "layoutLg", null);
	    __decorate$6([
	        _angular_core.Input('fxLayout.gt-lg'), 
	        __metadata$6('design:type', Object), 
	        __metadata$6('design:paramtypes', [Object])
	    ], LayoutDirective.prototype, "layoutGtLg", null);
	    __decorate$6([
	        _angular_core.Input('fxLayout.xl'), 
	        __metadata$6('design:type', Object), 
	        __metadata$6('design:paramtypes', [Object])
	    ], LayoutDirective.prototype, "layoutXl", null);
	    LayoutDirective = __decorate$6([
	        _angular_core.Directive({ selector: "\n  [fxLayout],\n  [fxLayout.xs]\n  [fxLayout.gt-xs],\n  [fxLayout.sm],\n  [fxLayout.gt-sm]\n  [fxLayout.md],\n  [fxLayout.gt-md]\n  [fxLayout.lg],\n  [fxLayout.gt-lg],\n  [fxLayout.xl]\n" }), 
	        __metadata$6('design:paramtypes', [MediaMonitor, _angular_core.ElementRef, _angular_core.Renderer])
	    ], LayoutDirective);
	    return LayoutDirective;
	}(BaseFxDirective));
	
	var __extends$2 = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var __decorate$7 = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata$7 = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var __param$2 = (this && this.__param) || function (paramIndex, decorator) {
	    return function (target, key) { decorator(target, key, paramIndex); }
	};
	/**
	 * 'layout-wrap' flexbox styling directive
	 * Defines wrapping of child elements in layout container
	 * Optional values: reverse, wrap-reverse, none, nowrap, wrap (default)]
	 * @see https://css-tricks.com/almanac/properties/f/flex-wrap/
	 */
	var LayoutWrapDirective = (function (_super) {
	    __extends$2(LayoutWrapDirective, _super);
	    function LayoutWrapDirective(monitor, elRef, renderer, container) {
	        _super.call(this, monitor, elRef, renderer);
	        this._layout = 'row'; // default flex-direction
	        if (container) {
	            this._layoutWatcher = container.layout$.subscribe(this._onLayoutChange.bind(this));
	        }
	    }
	    Object.defineProperty(LayoutWrapDirective.prototype, "wrap", {
	        set: function (val) { this._cacheInput("wrap", val); },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(LayoutWrapDirective.prototype, "wrapXs", {
	        set: function (val) { this._cacheInput('wrapXs', val); },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(LayoutWrapDirective.prototype, "wrapGtXs", {
	        set: function (val) { this._cacheInput('wrapGtXs', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutWrapDirective.prototype, "wrapSm", {
	        set: function (val) { this._cacheInput('wrapSm', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutWrapDirective.prototype, "wrapGtSm", {
	        set: function (val) { this._cacheInput('wrapGtSm', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutWrapDirective.prototype, "wrapMd", {
	        set: function (val) { this._cacheInput('wrapMd', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutWrapDirective.prototype, "wrapGtMd", {
	        set: function (val) { this._cacheInput('wrapGtMd', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutWrapDirective.prototype, "wrapLg", {
	        set: function (val) { this._cacheInput('wrapLg', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutWrapDirective.prototype, "wrapGtLg", {
	        set: function (val) { this._cacheInput('wrapGtLg', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutWrapDirective.prototype, "wrapXl", {
	        set: function (val) { this._cacheInput('wrapXl', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    // *********************************************
	    // Lifecycle Methods
	    // *********************************************
	    LayoutWrapDirective.prototype.ngOnChanges = function (changes) {
	        if (changes['wrap'] != null || this._mqActivation) {
	            this._updateWithValue();
	        }
	    };
	    /**
	     * After the initial onChanges, build an mqActivation object that bridges
	     * mql change events to onMediaQueryChange handlers
	     */
	    LayoutWrapDirective.prototype.ngOnInit = function () {
	        var _this = this;
	        this._listenForMediaQueryChanges('wrap', 'wrap', function (changes) {
	            _this._updateWithValue(changes.value);
	        });
	        this._updateWithValue();
	    };
	    // *********************************************
	    // Protected methods
	    // *********************************************
	    /**
	     * Cache the parent container 'flex-direction' and update the 'flex' styles
	     */
	    LayoutWrapDirective.prototype._onLayoutChange = function (direction) {
	        var _this = this;
	        this._layout = (direction || '').toLowerCase().replace('-reverse', '');
	        if (!LAYOUT_VALUES.find(function (x) { return x === _this._layout; })) {
	            this._layout = 'row';
	        }
	        this._updateWithValue();
	    };
	    LayoutWrapDirective.prototype._updateWithValue = function (value) {
	        value = value || this._queryInput("wrap") || 'wrap';
	        if (this._mqActivation) {
	            value = this._mqActivation.activatedInput;
	        }
	        value = this._validateValue(value);
	        this._applyStyleToElement(this._buildCSS(value));
	    };
	    /**
	     * Build the CSS that should be assigned to the element instance
	     */
	    LayoutWrapDirective.prototype._buildCSS = function (value) {
	        return extendObject({ 'flex-wrap': value }, {
	            'display': 'flex',
	            'flex-direction': this._layout || 'row'
	        });
	    };
	    /**
	     * Convert layout-wrap="<value>" to expected flex-wrap style
	     */
	    LayoutWrapDirective.prototype._validateValue = function (value) {
	        switch (value.toLowerCase()) {
	            case 'reverse':
	            case 'wrap-reverse':
	                value = 'wrap-reverse';
	                break;
	            case 'no':
	            case 'none':
	            case 'nowrap':
	                value = 'nowrap';
	                break;
	            // All other values fallback to "wrap"
	            default:
	                value = 'wrap';
	                break;
	        }
	        return value;
	    };
	    __decorate$7([
	        _angular_core.Input('fxLayoutWrap'), 
	        __metadata$7('design:type', Object), 
	        __metadata$7('design:paramtypes', [Object])
	    ], LayoutWrapDirective.prototype, "wrap", null);
	    __decorate$7([
	        _angular_core.Input('fxLayoutWrap.xs'), 
	        __metadata$7('design:type', Object), 
	        __metadata$7('design:paramtypes', [Object])
	    ], LayoutWrapDirective.prototype, "wrapXs", null);
	    __decorate$7([
	        _angular_core.Input('fxLayoutWrap.gt-xs'), 
	        __metadata$7('design:type', Object), 
	        __metadata$7('design:paramtypes', [Object])
	    ], LayoutWrapDirective.prototype, "wrapGtXs", null);
	    __decorate$7([
	        _angular_core.Input('fxLayoutWrap.sm'), 
	        __metadata$7('design:type', Object), 
	        __metadata$7('design:paramtypes', [Object])
	    ], LayoutWrapDirective.prototype, "wrapSm", null);
	    __decorate$7([
	        _angular_core.Input('fxLayoutWrap.gt-sm'), 
	        __metadata$7('design:type', Object), 
	        __metadata$7('design:paramtypes', [Object])
	    ], LayoutWrapDirective.prototype, "wrapGtSm", null);
	    __decorate$7([
	        _angular_core.Input('fxLayoutWrap.md'), 
	        __metadata$7('design:type', Object), 
	        __metadata$7('design:paramtypes', [Object])
	    ], LayoutWrapDirective.prototype, "wrapMd", null);
	    __decorate$7([
	        _angular_core.Input('fxLayoutWrap.gt-md'), 
	        __metadata$7('design:type', Object), 
	        __metadata$7('design:paramtypes', [Object])
	    ], LayoutWrapDirective.prototype, "wrapGtMd", null);
	    __decorate$7([
	        _angular_core.Input('fxLayoutWrap.lg'), 
	        __metadata$7('design:type', Object), 
	        __metadata$7('design:paramtypes', [Object])
	    ], LayoutWrapDirective.prototype, "wrapLg", null);
	    __decorate$7([
	        _angular_core.Input('fxLayoutWrap.gt-lg'), 
	        __metadata$7('design:type', Object), 
	        __metadata$7('design:paramtypes', [Object])
	    ], LayoutWrapDirective.prototype, "wrapGtLg", null);
	    __decorate$7([
	        _angular_core.Input('fxLayoutWrap.xl'), 
	        __metadata$7('design:type', Object), 
	        __metadata$7('design:paramtypes', [Object])
	    ], LayoutWrapDirective.prototype, "wrapXl", null);
	    LayoutWrapDirective = __decorate$7([
	        _angular_core.Directive({ selector: "\n  [fxLayoutWrap],\n  [fxLayoutWrap.xs]\n  [fxLayoutWrap.gt-xs],\n  [fxLayoutWrap.sm],\n  [fxLayoutWrap.gt-sm]\n  [fxLayoutWrap.md],\n  [fxLayoutWrap.gt-md]\n  [fxLayoutWrap.lg],\n  [fxLayoutWrap.gt-lg],\n  [fxLayoutWrap.xl]\n" }),
	        __param$2(3, _angular_core.Optional()),
	        __param$2(3, _angular_core.Self()), 
	        __metadata$7('design:paramtypes', [MediaMonitor, _angular_core.ElementRef, _angular_core.Renderer, LayoutDirective])
	    ], LayoutWrapDirective);
	    return LayoutWrapDirective;
	}(BaseFxDirective));
	
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var __decorate$5 = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata$5 = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var __param$1 = (this && this.__param) || function (paramIndex, decorator) {
	    return function (target, key) { decorator(target, key, paramIndex); }
	};
	/**
	 * Directive to control the size of a flex item using flex-basis, flex-grow, and flex-shrink.
	 * Corresponds to the css `flex` shorthand property.
	 *
	 * @see https://css-tricks.com/snippets/css/a-guide-to-flexbox/
	 */
	var FlexDirective = (function (_super) {
	    __extends(FlexDirective, _super);
	    // Explicitly @SkipSelf on LayoutDirective and LayoutWrapDirective because we want the
	    // parent flex container for this flex item.
	    function FlexDirective(monitor, elRef, renderer, _container, _wrap) {
	        var _this = this;
	        _super.call(this, monitor, elRef, renderer);
	        this._container = _container;
	        this._wrap = _wrap;
	        /** The flex-direction of this element's flex container. Defaults to 'row'. */
	        this._layout = 'row';
	        this._cacheInput("flex", "");
	        this._cacheInput("shrink", 1);
	        this._cacheInput("grow", 1);
	        if (_container) {
	            // If this flex item is inside of a flex container marked with
	            // Subscribe to layout immediate parent direction changes
	            this._layoutWatcher = _container.layout$.subscribe(function (direction) {
	                // `direction` === null if parent container does not have a `fxLayout`
	                _this._onLayoutChange(direction);
	            });
	        }
	    }
	    Object.defineProperty(FlexDirective.prototype, "flex", {
	        set: function (val) { this._cacheInput("flex", val); },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(FlexDirective.prototype, "shrink", {
	        set: function (val) { this._cacheInput("shrink", val); },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(FlexDirective.prototype, "grow", {
	        set: function (val) { this._cacheInput("grow", val); },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(FlexDirective.prototype, "flexXs", {
	        set: function (val) { this._cacheInput('flexXs', val); },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(FlexDirective.prototype, "flexGtXs", {
	        set: function (val) { this._cacheInput('flexGtXs', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexDirective.prototype, "flexSm", {
	        set: function (val) { this._cacheInput('flexSm', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexDirective.prototype, "flexGtSm", {
	        set: function (val) { this._cacheInput('flexGtSm', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexDirective.prototype, "flexMd", {
	        set: function (val) { this._cacheInput('flexMd', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexDirective.prototype, "flexGtMd", {
	        set: function (val) { this._cacheInput('flexGtMd', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexDirective.prototype, "flexLg", {
	        set: function (val) { this._cacheInput('flexLg', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexDirective.prototype, "flexGtLg", {
	        set: function (val) { this._cacheInput('flexGtLg', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexDirective.prototype, "flexXl", {
	        set: function (val) { this._cacheInput('flexXl', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    /**
	     * For @Input changes on the current mq activation property, see onMediaQueryChanges()
	     */
	    FlexDirective.prototype.ngOnChanges = function (changes) {
	        if (changes['flex'] != null || this._mqActivation) {
	            this._onLayoutChange();
	        }
	    };
	    /**
	     * After the initial onChanges, build an mqActivation object that bridges
	     * mql change events to onMediaQueryChange handlers
	     */
	    FlexDirective.prototype.ngOnInit = function () {
	        var _this = this;
	        this._listenForMediaQueryChanges('flex', '', function (changes) {
	            _this._updateStyle(changes.value);
	        });
	        this._onLayoutChange();
	    };
	    FlexDirective.prototype.ngOnDestroy = function () {
	        _super.prototype.ngOnDestroy.call(this);
	        if (this._layoutWatcher) {
	            this._layoutWatcher.unsubscribe();
	        }
	    };
	    /**
	     * Caches the parent container's 'flex-direction' and updates the element's style.
	     * Used as a handler for layout change events from the parent flex container.
	     */
	    FlexDirective.prototype._onLayoutChange = function (direction) {
	        this._layout = direction || this._layout || "row";
	        this._updateStyle();
	    };
	    FlexDirective.prototype._updateStyle = function (value) {
	        var flexBasis = value || this._queryInput("flex") || '';
	        if (this._mqActivation) {
	            flexBasis = this._mqActivation.activatedInput;
	        }
	        this._applyStyleToElement(this._validateValue.apply(this, this._parseFlexParts(String(flexBasis))));
	    };
	    /**
	     * If the used the short-form `fxFlex="1 0 37%"`, then parse the parts
	     */
	    FlexDirective.prototype._parseFlexParts = function (basis) {
	        basis = basis.replace(";", "");
	        var hasCalc = basis && basis.indexOf("calc") > -1;
	        var matches = !hasCalc ? basis.split(" ") : this._getPartsWithCalc(basis.trim());
	        return (matches.length === 3) ? matches : [this._queryInput("grow"), this._queryInput("shrink"), basis];
	    };
	    /**
	     * Extract more complicated short-hand versions.
	     * e.g.
	     * fxFlex="3 3 calc(15em + 20px)"
	     */
	    FlexDirective.prototype._getPartsWithCalc = function (value) {
	        debugger;
	        var parts = [this._queryInput("grow"), this._queryInput("shrink"), value];
	        var j = value.indexOf('calc');
	        if (j > 0) {
	            parts[2] = value.substring(j);
	            var matches = value.substr(0, j).trim().split(" ");
	            if (matches.length == 2) {
	                parts[0] = matches[0];
	                parts[1] = matches[1];
	            }
	        }
	        return parts;
	    };
	    /**
	     * Validate the value to be one of the acceptable value options
	     * Use default fallback of "row"
	     */
	    FlexDirective.prototype._validateValue = function (grow, shrink, basis) {
	        var css;
	        var direction = (this._layout === 'column') || (this._layout == 'column-reverse') ?
	            'column' :
	            'row';
	        // flex-basis allows you to specify the initial/starting main-axis size of the element,
	        // before anything else is computed. It can either be a percentage or an absolute value.
	        // It is, however, not the breaking point for flex-grow/shrink properties
	        //
	        // flex-grow can be seen as this:
	        //   0: Do not stretch. Either size to element's content width, or obey 'flex-basis'.
	        //   1: (Default value). Stretch; will be the same size to all other flex items on
	        //       the same row since they have a default value of 1.
	        //   ≥2 (integer n): Stretch. Will be n times the size of other elements
	        //      with 'flex-grow: 1' on the same row.
	        // Use `null` to clear existing styles.
	        var clearStyles = {
	            'max-width': null,
	            'max-height': null,
	            'min-width': null,
	            'min-height': null
	        };
	        switch (basis || '') {
	            case '':
	                css = extendObject(clearStyles, { 'flex': '1 1 0.000000001px' });
	                break;
	            case 'grow':
	                css = extendObject(clearStyles, { 'flex': '1 1 100%' });
	                break;
	            case 'initial':
	                css = extendObject(clearStyles, { 'flex': '0 1 auto' });
	                break; // default
	            case 'auto':
	                css = extendObject(clearStyles, { 'flex': '1 1 auto' });
	                break;
	            case 'none':
	                css = extendObject(clearStyles, { 'flex': '0 0 auto' });
	                break;
	            case 'nogrow':
	                css = extendObject(clearStyles, { 'flex': '0 1 auto' });
	                break;
	            case 'none':
	                css = extendObject(clearStyles, { 'flex': 'none' });
	                break;
	            case 'noshrink':
	                css = extendObject(clearStyles, { 'flex': '1 0 auto' });
	                break;
	            default:
	                var isPercent = String(basis).indexOf('%') > -1;
	                var isValue = String(basis).indexOf('px') > -1 ||
	                    String(basis).indexOf('calc') > -1 ||
	                    String(basis).indexOf('em') > -1 ||
	                    String(basis).indexOf('vw') > -1 ||
	                    String(basis).indexOf('vh') > -1;
	                // Defaults to percentage sizing unless `px` is explicitly set
	                if (!isValue && !isPercent && !isNaN(basis))
	                    basis = basis + '%';
	                if (basis === '0px')
	                    basis = '0%';
	                // Set max-width = basis if using layout-wrap
	                // @see https://github.com/philipwalton/flexbugs#11-min-and-max-size-declarations-are-ignored-when-wrappifl-flex-items
	                css = extendObject(clearStyles, {
	                    'flex': grow + " " + shrink + " " + ((isValue || this._wrap) ? basis : '100%'),
	                });
	                break;
	        }
	        var max = (direction === 'row') ? 'max-width' : 'max-height';
	        var min = (direction === 'row') ? 'min-width' : 'min-height';
	        var usingCalc = String(basis).indexOf('calc') > -1;
	        css[min] = (basis == '0%') ? 0 : null;
	        css[max] = (basis == '0%') ? 0 : usingCalc ? null : basis;
	        return extendObject(css, { 'box-sizing': 'border-box' });
	    };
	    __decorate$5([
	        _angular_core.Input('fxFlex'), 
	        __metadata$5('design:type', Object), 
	        __metadata$5('design:paramtypes', [Object])
	    ], FlexDirective.prototype, "flex", null);
	    __decorate$5([
	        _angular_core.Input('fxShrink'), 
	        __metadata$5('design:type', Object), 
	        __metadata$5('design:paramtypes', [Object])
	    ], FlexDirective.prototype, "shrink", null);
	    __decorate$5([
	        _angular_core.Input('fxGrow'), 
	        __metadata$5('design:type', Object), 
	        __metadata$5('design:paramtypes', [Object])
	    ], FlexDirective.prototype, "grow", null);
	    __decorate$5([
	        _angular_core.Input('fxFlex.xs'), 
	        __metadata$5('design:type', Object), 
	        __metadata$5('design:paramtypes', [Object])
	    ], FlexDirective.prototype, "flexXs", null);
	    __decorate$5([
	        _angular_core.Input('fxFlex.gt-xs'), 
	        __metadata$5('design:type', Object), 
	        __metadata$5('design:paramtypes', [Object])
	    ], FlexDirective.prototype, "flexGtXs", null);
	    __decorate$5([
	        _angular_core.Input('fxFlex.sm'), 
	        __metadata$5('design:type', Object), 
	        __metadata$5('design:paramtypes', [Object])
	    ], FlexDirective.prototype, "flexSm", null);
	    __decorate$5([
	        _angular_core.Input('fxFlex.gt-sm'), 
	        __metadata$5('design:type', Object), 
	        __metadata$5('design:paramtypes', [Object])
	    ], FlexDirective.prototype, "flexGtSm", null);
	    __decorate$5([
	        _angular_core.Input('fxFlex.md'), 
	        __metadata$5('design:type', Object), 
	        __metadata$5('design:paramtypes', [Object])
	    ], FlexDirective.prototype, "flexMd", null);
	    __decorate$5([
	        _angular_core.Input('fxFlex.gt-md'), 
	        __metadata$5('design:type', Object), 
	        __metadata$5('design:paramtypes', [Object])
	    ], FlexDirective.prototype, "flexGtMd", null);
	    __decorate$5([
	        _angular_core.Input('fxFlex.lg'), 
	        __metadata$5('design:type', Object), 
	        __metadata$5('design:paramtypes', [Object])
	    ], FlexDirective.prototype, "flexLg", null);
	    __decorate$5([
	        _angular_core.Input('fxFlex.gt-lg'), 
	        __metadata$5('design:type', Object), 
	        __metadata$5('design:paramtypes', [Object])
	    ], FlexDirective.prototype, "flexGtLg", null);
	    __decorate$5([
	        _angular_core.Input('fxFlex.xl'), 
	        __metadata$5('design:type', Object), 
	        __metadata$5('design:paramtypes', [Object])
	    ], FlexDirective.prototype, "flexXl", null);
	    FlexDirective = __decorate$5([
	        _angular_core.Directive({ selector: "\n  [fxFlex],\n  [fxFlex.xs]\n  [fxFlex.gt-xs],\n  [fxFlex.sm],\n  [fxFlex.gt-sm]\n  [fxFlex.md],\n  [fxFlex.gt-md]\n  [fxFlex.lg],\n  [fxFlex.gt-lg],\n  [fxFlex.xl]\n"
	        }),
	        __param$1(3, _angular_core.Optional()),
	        __param$1(3, _angular_core.SkipSelf()),
	        __param$1(4, _angular_core.Optional()),
	        __param$1(4, _angular_core.SkipSelf()), 
	        __metadata$5('design:paramtypes', [MediaMonitor, _angular_core.ElementRef, _angular_core.Renderer, LayoutDirective, LayoutWrapDirective])
	    ], FlexDirective);
	    return FlexDirective;
	}(BaseFxDirective));
	
	var __extends$4 = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var __decorate$9 = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata$9 = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var __param$4 = (this && this.__param) || function (paramIndex, decorator) {
	    return function (target, key) { decorator(target, key, paramIndex); }
	};
	var FALSY$1 = ['false', false, 0];
	/**
	 * 'show' Layout API directive
	 *
	 */
	var ShowDirective = (function (_super) {
	    __extends$4(ShowDirective, _super);
	    /**
	     *
	     */
	    function ShowDirective(monitor, _layout, _hideDirective, elRef, renderer) {
	        var _this = this;
	        _super.call(this, monitor, elRef, renderer);
	        this._layout = _layout;
	        this._hideDirective = _hideDirective;
	        this.elRef = elRef;
	        this.renderer = renderer;
	        /**
	         * Original dom Elements CSS display style
	         */
	        this._display = 'flex';
	        if (_layout) {
	            /**
	             * The Layout can set the display:flex (and incorrectly affect the Hide/Show directives.
	             * Whenever Layout [on the same element] resets its CSS, then update the Hide/Show CSS
	             */
	            this._layoutWatcher = _layout.layout$.subscribe(function () { return _this._updateWithValue(); });
	        }
	    }
	    Object.defineProperty(ShowDirective.prototype, "show", {
	        set: function (val) { this._cacheInput("show", val); },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(ShowDirective.prototype, "showXs", {
	        set: function (val) { this._cacheInput('showXs', val); },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(ShowDirective.prototype, "showGtXs", {
	        set: function (val) { this._cacheInput('showGtXs', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(ShowDirective.prototype, "showSm", {
	        set: function (val) { this._cacheInput('showSm', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(ShowDirective.prototype, "showGtSm", {
	        set: function (val) { this._cacheInput('showGtSm', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(ShowDirective.prototype, "showMd", {
	        set: function (val) { this._cacheInput('showMd', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(ShowDirective.prototype, "showGtMd", {
	        set: function (val) { this._cacheInput('showGtMd', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(ShowDirective.prototype, "showLg", {
	        set: function (val) { this._cacheInput('showLg', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(ShowDirective.prototype, "showGtLg", {
	        set: function (val) { this._cacheInput('showGtLg', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(ShowDirective.prototype, "showXl", {
	        set: function (val) { this._cacheInput('showXl', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(ShowDirective.prototype, "usesHideAPI", {
	        /**
	          * Does the current element also use the fxShow API ?
	          */
	        get: function () {
	            return !!this._hideDirective;
	        },
	        enumerable: true,
	        configurable: true
	    });
	    // *********************************************
	    // Lifecycle Methods
	    // *********************************************
	    /**
	     * On changes to any @Input properties...
	     * Default to use the non-responsive Input value ('fxShow')
	     * Then conditionally override with the mq-activated Input's current value
	     */
	    ShowDirective.prototype.ngOnChanges = function (changes) {
	        if (changes['show'] != null || this._mqActivation) {
	            this._updateWithValue();
	        }
	    };
	    /**
	     * After the initial onChanges, build an mqActivation object that bridges
	     * mql change events to onMediaQueryChange handlers
	     */
	    ShowDirective.prototype.ngOnInit = function () {
	        var _this = this;
	        this._listenForMediaQueryChanges('show', true, function (changes) {
	            _this._updateWithValue(changes.value);
	        });
	        this._updateWithValue();
	    };
	    ShowDirective.prototype.ngOnDestroy = function () {
	        _super.prototype.ngOnDestroy.call(this);
	        if (this._layoutWatcher) {
	            this._layoutWatcher.unsubscribe();
	        }
	    };
	    // *********************************************
	    // Protected methods
	    // *********************************************
	    /** Validate the visibility value and then update the host's inline display style */
	    ShowDirective.prototype._updateWithValue = function (value) {
	        value = value || this._queryInput("show") || true;
	        if (this._mqActivation) {
	            value = this._mqActivation.activatedInput;
	        }
	        var shouldShow = this._validateTruthy(value);
	        if (shouldShow || !this.usesHideAPI) {
	            this._applyStyleToElement(this._buildCSS(shouldShow));
	        }
	    };
	    /** Build the CSS that should be assigned to the element instance */
	    ShowDirective.prototype._buildCSS = function (show) {
	        return { 'display': show ? this._display : 'none' };
	    };
	    /**  Validate the to be not FALSY */
	    ShowDirective.prototype._validateTruthy = function (show) {
	        return (FALSY$1.indexOf(show) == -1);
	    };
	    __decorate$9([
	        _angular_core.Input('fxShow'), 
	        __metadata$9('design:type', Object), 
	        __metadata$9('design:paramtypes', [Object])
	    ], ShowDirective.prototype, "show", null);
	    __decorate$9([
	        _angular_core.Input('fxShow.xs'), 
	        __metadata$9('design:type', Object), 
	        __metadata$9('design:paramtypes', [Object])
	    ], ShowDirective.prototype, "showXs", null);
	    __decorate$9([
	        _angular_core.Input('fxShow.gt-xs'), 
	        __metadata$9('design:type', Object), 
	        __metadata$9('design:paramtypes', [Object])
	    ], ShowDirective.prototype, "showGtXs", null);
	    __decorate$9([
	        _angular_core.Input('fxShow.sm'), 
	        __metadata$9('design:type', Object), 
	        __metadata$9('design:paramtypes', [Object])
	    ], ShowDirective.prototype, "showSm", null);
	    __decorate$9([
	        _angular_core.Input('fxShow.gt-sm'), 
	        __metadata$9('design:type', Object), 
	        __metadata$9('design:paramtypes', [Object])
	    ], ShowDirective.prototype, "showGtSm", null);
	    __decorate$9([
	        _angular_core.Input('fxShow.md'), 
	        __metadata$9('design:type', Object), 
	        __metadata$9('design:paramtypes', [Object])
	    ], ShowDirective.prototype, "showMd", null);
	    __decorate$9([
	        _angular_core.Input('fxShow.gt-md'), 
	        __metadata$9('design:type', Object), 
	        __metadata$9('design:paramtypes', [Object])
	    ], ShowDirective.prototype, "showGtMd", null);
	    __decorate$9([
	        _angular_core.Input('fxShow.lg'), 
	        __metadata$9('design:type', Object), 
	        __metadata$9('design:paramtypes', [Object])
	    ], ShowDirective.prototype, "showLg", null);
	    __decorate$9([
	        _angular_core.Input('fxShow.gt-lg'), 
	        __metadata$9('design:type', Object), 
	        __metadata$9('design:paramtypes', [Object])
	    ], ShowDirective.prototype, "showGtLg", null);
	    __decorate$9([
	        _angular_core.Input('fxShow.xl'), 
	        __metadata$9('design:type', Object), 
	        __metadata$9('design:paramtypes', [Object])
	    ], ShowDirective.prototype, "showXl", null);
	    ShowDirective = __decorate$9([
	        _angular_core.Directive({ selector: "\n  [fxShow],\n  [fxShow.xs]\n  [fxShow.gt-xs],\n  [fxShow.sm],\n  [fxShow.gt-sm]\n  [fxShow.md],\n  [fxShow.gt-md]\n  [fxShow.lg],\n  [fxShow.gt-lg],\n  [fxShow.xl]\n" }),
	        __param$4(1, _angular_core.Optional()),
	        __param$4(1, _angular_core.Self()),
	        __param$4(2, _angular_core.Inject(_angular_core.forwardRef(function () { return HideDirective; }))),
	        __param$4(2, _angular_core.Optional()),
	        __param$4(2, _angular_core.Self()), 
	        __metadata$9('design:paramtypes', [MediaMonitor, LayoutDirective, Object, _angular_core.ElementRef, _angular_core.Renderer])
	    ], ShowDirective);
	    return ShowDirective;
	}(BaseFxDirective));
	
	var __extends$3 = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var __decorate$8 = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata$8 = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var __param$3 = (this && this.__param) || function (paramIndex, decorator) {
	    return function (target, key) { decorator(target, key, paramIndex); }
	};
	/**
	 * 'show' Layout API directive
	 *
	 */
	var HideDirective = (function (_super) {
	    __extends$3(HideDirective, _super);
	    /**
	     *
	     */
	    function HideDirective(monitor, _layout, _showDirective, elRef, renderer) {
	        var _this = this;
	        _super.call(this, monitor, elRef, renderer);
	        this._layout = _layout;
	        this._showDirective = _showDirective;
	        this.elRef = elRef;
	        this.renderer = renderer;
	        /**
	         * Original dom Elements CSS display style
	         */
	        this._display = 'flex';
	        if (_layout) {
	            /**
	             * The Layout can set the display:flex (and incorrectly affect the Hide/Show directives.
	             * Whenever Layout [on the same element] resets its CSS, then update the Hide/Show CSS
	             */
	            this._layoutWatcher = _layout.layout$.subscribe(function () { return _this._updateWithValue(); });
	        }
	    }
	    Object.defineProperty(HideDirective.prototype, "hide", {
	        set: function (val) { this._cacheInput("hide", val); },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(HideDirective.prototype, "hideXs", {
	        set: function (val) { this._cacheInput('hideXs', val); },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(HideDirective.prototype, "hideGtXs", {
	        set: function (val) { this._cacheInput('hideGtXs', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(HideDirective.prototype, "hideSm", {
	        set: function (val) { this._cacheInput('hideSm', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(HideDirective.prototype, "hideGtSm", {
	        set: function (val) { this._cacheInput('hideGtSm', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(HideDirective.prototype, "hideMd", {
	        set: function (val) { this._cacheInput('hideMd', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(HideDirective.prototype, "hideGtMd", {
	        set: function (val) { this._cacheInput('hideGtMd', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(HideDirective.prototype, "hideLg", {
	        set: function (val) { this._cacheInput('hideLg', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(HideDirective.prototype, "hideGtLg", {
	        set: function (val) { this._cacheInput('hideGtLg', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(HideDirective.prototype, "hideXl", {
	        set: function (val) { this._cacheInput('hideXl', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(HideDirective.prototype, "usesShowAPI", {
	        /**
	         * Does the current element also use the fxShow API ?
	         */
	        get: function () {
	            return !!this._showDirective;
	        },
	        enumerable: true,
	        configurable: true
	    });
	    // *********************************************
	    // Lifecycle Methods
	    // *********************************************
	    /**
	     * On changes to any @Input properties...
	     * Default to use the non-responsive Input value ('fxHide')
	     * Then conditionally override with the mq-activated Input's current value
	     */
	    HideDirective.prototype.ngOnChanges = function (changes) {
	        if (changes['hide'] != null || this._mqActivation) {
	            this._updateWithValue();
	        }
	    };
	    /**
	     * After the initial onChanges, build an mqActivation object that bridges
	     * mql change events to onMediaQueryChange handlers
	     */
	    HideDirective.prototype.ngOnInit = function () {
	        var _this = this;
	        this._listenForMediaQueryChanges('hide', true, function (changes) {
	            _this._updateWithValue(changes.value);
	        });
	        this._updateWithValue();
	    };
	    HideDirective.prototype.ngOnDestroy = function () {
	        _super.prototype.ngOnDestroy.call(this);
	        if (this._layoutWatcher) {
	            this._layoutWatcher.unsubscribe();
	        }
	    };
	    // *********************************************
	    // Protected methods
	    // *********************************************
	    /**
	     * Validate the visibility value and then update the host's inline display style
	     */
	    HideDirective.prototype._updateWithValue = function (value) {
	        value = value || this._queryInput("hide") || true;
	        if (this._mqActivation) {
	            value = this._mqActivation.activatedInput;
	        }
	        var shouldHide = this._validateTruthy(value);
	        if (shouldHide || !this.usesShowAPI) {
	            this._applyStyleToElement(this._buildCSS(shouldHide));
	        }
	    };
	    /**
	     * Build the CSS that should be assigned to the element instance
	     */
	    HideDirective.prototype._buildCSS = function (value) {
	        return { 'display': value ? 'none' : this._display };
	    };
	    /**
	     * Validate the value to NOT be FALSY
	     */
	    HideDirective.prototype._validateTruthy = function (value) {
	        return FALSY.indexOf(value) === -1;
	    };
	    __decorate$8([
	        _angular_core.Input('fxHide'), 
	        __metadata$8('design:type', Object), 
	        __metadata$8('design:paramtypes', [Object])
	    ], HideDirective.prototype, "hide", null);
	    __decorate$8([
	        _angular_core.Input('fxHide.xs'), 
	        __metadata$8('design:type', Object), 
	        __metadata$8('design:paramtypes', [Object])
	    ], HideDirective.prototype, "hideXs", null);
	    __decorate$8([
	        _angular_core.Input('fxHide.gt-xs'), 
	        __metadata$8('design:type', Object), 
	        __metadata$8('design:paramtypes', [Object])
	    ], HideDirective.prototype, "hideGtXs", null);
	    __decorate$8([
	        _angular_core.Input('fxHide.sm'), 
	        __metadata$8('design:type', Object), 
	        __metadata$8('design:paramtypes', [Object])
	    ], HideDirective.prototype, "hideSm", null);
	    __decorate$8([
	        _angular_core.Input('fxHide.gt-sm'), 
	        __metadata$8('design:type', Object), 
	        __metadata$8('design:paramtypes', [Object])
	    ], HideDirective.prototype, "hideGtSm", null);
	    __decorate$8([
	        _angular_core.Input('fxHide.md'), 
	        __metadata$8('design:type', Object), 
	        __metadata$8('design:paramtypes', [Object])
	    ], HideDirective.prototype, "hideMd", null);
	    __decorate$8([
	        _angular_core.Input('fxHide.gt-md'), 
	        __metadata$8('design:type', Object), 
	        __metadata$8('design:paramtypes', [Object])
	    ], HideDirective.prototype, "hideGtMd", null);
	    __decorate$8([
	        _angular_core.Input('fxHide.lg'), 
	        __metadata$8('design:type', Object), 
	        __metadata$8('design:paramtypes', [Object])
	    ], HideDirective.prototype, "hideLg", null);
	    __decorate$8([
	        _angular_core.Input('fxHide.gt-lg'), 
	        __metadata$8('design:type', Object), 
	        __metadata$8('design:paramtypes', [Object])
	    ], HideDirective.prototype, "hideGtLg", null);
	    __decorate$8([
	        _angular_core.Input('fxHide.xl'), 
	        __metadata$8('design:type', Object), 
	        __metadata$8('design:paramtypes', [Object])
	    ], HideDirective.prototype, "hideXl", null);
	    HideDirective = __decorate$8([
	        _angular_core.Directive({ selector: "\n  [fxHide],\n  [fxHide.xs]\n  [fxHide.gt-xs],\n  [fxHide.sm],\n  [fxHide.gt-sm]\n  [fxHide.md],\n  [fxHide.gt-md]\n  [fxHide.lg],\n  [fxHide.gt-lg],\n  [fxHide.xl]\n" }),
	        __param$3(1, _angular_core.Optional()),
	        __param$3(1, _angular_core.Self()),
	        __param$3(2, _angular_core.Optional()),
	        __param$3(2, _angular_core.Self()), 
	        __metadata$8('design:paramtypes', [MediaMonitor, LayoutDirective, ShowDirective, _angular_core.ElementRef, _angular_core.Renderer])
	    ], HideDirective);
	    return HideDirective;
	}(BaseFxDirective));
	var FALSY = ['false', false, 0];
	
	var __extends$5 = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var __decorate$10 = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata$10 = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	/**
	 * 'flex-align' flexbox styling directive
	 * Allows element-specific overrides for cross-axis alignments in a layout container
	 * @see https://css-tricks.com/almanac/properties/a/align-self/
	 */
	var FlexAlignDirective = (function (_super) {
	    __extends$5(FlexAlignDirective, _super);
	    function FlexAlignDirective(monitor, elRef, renderer) {
	        _super.call(this, monitor, elRef, renderer);
	    }
	    Object.defineProperty(FlexAlignDirective.prototype, "align", {
	        set: function (val) {
	            this._cacheInput('align', val);
	        },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(FlexAlignDirective.prototype, "alignXs", {
	        set: function (val) {
	            this._cacheInput('alignXs', val);
	        },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(FlexAlignDirective.prototype, "alignGtXs", {
	        set: function (val) {
	            this._cacheInput('alignGtXs', val);
	        },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexAlignDirective.prototype, "alignSm", {
	        set: function (val) {
	            this._cacheInput('alignSm', val);
	        },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexAlignDirective.prototype, "alignGtSm", {
	        set: function (val) {
	            this._cacheInput('alignGtSm', val);
	        },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexAlignDirective.prototype, "alignMd", {
	        set: function (val) {
	            this._cacheInput('alignMd', val);
	        },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexAlignDirective.prototype, "alignGtMd", {
	        set: function (val) {
	            this._cacheInput('alignGtMd', val);
	        },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexAlignDirective.prototype, "alignLg", {
	        set: function (val) {
	            this._cacheInput('alignLg', val);
	        },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexAlignDirective.prototype, "alignGtLg", {
	        set: function (val) {
	            this._cacheInput('alignGtLg', val);
	        },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexAlignDirective.prototype, "alignXl", {
	        set: function (val) {
	            this._cacheInput('alignXl', val);
	        },
	        enumerable: true,
	        configurable: true
	    });
	    
	    // *********************************************
	    // Lifecycle Methods
	    // *********************************************
	    /**
	     * For @Input changes on the current mq activation property, see onMediaQueryChanges()
	     */
	    FlexAlignDirective.prototype.ngOnChanges = function (changes) {
	        if (changes['align'] != null || this._mqActivation) {
	            this._updateWithValue();
	        }
	    };
	    /**
	     * After the initial onChanges, build an mqActivation object that bridges
	     * mql change events to onMediaQueryChange handlers
	     */
	    FlexAlignDirective.prototype.ngOnInit = function () {
	        var _this = this;
	        this._listenForMediaQueryChanges('align', 'stretch', function (changes) {
	            _this._updateWithValue(changes.value);
	        });
	        this._updateWithValue();
	    };
	    // *********************************************
	    // Protected methods
	    // *********************************************
	    FlexAlignDirective.prototype._updateWithValue = function (value) {
	        value = value || this._queryInput("align") || 'stretch';
	        if (this._mqActivation) {
	            value = this._mqActivation.activatedInput;
	        }
	        this._applyStyleToElement(this._buildCSS(value));
	    };
	    FlexAlignDirective.prototype._buildCSS = function (align) {
	        var css = {};
	        // Cross-axis
	        switch (align) {
	            case 'start':
	                css['align-self'] = 'flex-start';
	                break;
	            case 'end':
	                css['align-self'] = 'flex-end';
	                break;
	            default:
	                css['align-self'] = align;
	                break;
	        }
	        return css;
	    };
	    __decorate$10([
	        _angular_core.Input('fxFlexAlign'), 
	        __metadata$10('design:type', Object), 
	        __metadata$10('design:paramtypes', [Object])
	    ], FlexAlignDirective.prototype, "align", null);
	    __decorate$10([
	        _angular_core.Input('fxFlexAlign.xs'), 
	        __metadata$10('design:type', Object), 
	        __metadata$10('design:paramtypes', [Object])
	    ], FlexAlignDirective.prototype, "alignXs", null);
	    __decorate$10([
	        _angular_core.Input('fxFlexAlign.gt-xs'), 
	        __metadata$10('design:type', Object), 
	        __metadata$10('design:paramtypes', [Object])
	    ], FlexAlignDirective.prototype, "alignGtXs", null);
	    __decorate$10([
	        _angular_core.Input('fxFlexAlign.sm'), 
	        __metadata$10('design:type', Object), 
	        __metadata$10('design:paramtypes', [Object])
	    ], FlexAlignDirective.prototype, "alignSm", null);
	    __decorate$10([
	        _angular_core.Input('fxFlexAlign.gt-sm'), 
	        __metadata$10('design:type', Object), 
	        __metadata$10('design:paramtypes', [Object])
	    ], FlexAlignDirective.prototype, "alignGtSm", null);
	    __decorate$10([
	        _angular_core.Input('fxFlexAlign.md'), 
	        __metadata$10('design:type', Object), 
	        __metadata$10('design:paramtypes', [Object])
	    ], FlexAlignDirective.prototype, "alignMd", null);
	    __decorate$10([
	        _angular_core.Input('fxFlexAlign.gt-md'), 
	        __metadata$10('design:type', Object), 
	        __metadata$10('design:paramtypes', [Object])
	    ], FlexAlignDirective.prototype, "alignGtMd", null);
	    __decorate$10([
	        _angular_core.Input('fxFlexAlign.lg'), 
	        __metadata$10('design:type', Object), 
	        __metadata$10('design:paramtypes', [Object])
	    ], FlexAlignDirective.prototype, "alignLg", null);
	    __decorate$10([
	        _angular_core.Input('fxFlexAlign.gt-lg'), 
	        __metadata$10('design:type', Object), 
	        __metadata$10('design:paramtypes', [Object])
	    ], FlexAlignDirective.prototype, "alignGtLg", null);
	    __decorate$10([
	        _angular_core.Input('fxFlexAlign.xl'), 
	        __metadata$10('design:type', Object), 
	        __metadata$10('design:paramtypes', [Object])
	    ], FlexAlignDirective.prototype, "alignXl", null);
	    FlexAlignDirective = __decorate$10([
	        _angular_core.Directive({
	            selector: "\n  [fxFlexAlign],\n  [fxFlexAlign.xs]\n  [fxFlexAlign.gt-xs],\n  [fxFlexAlign.sm],\n  [fxFlexAlign.gt-sm]\n  [fxFlexAlign.md],\n  [fxFlexAlign.gt-md]\n  [fxFlexAlign.lg],\n  [fxFlexAlign.gt-lg],\n  [fxFlexAlign.xl]\n"
	        }), 
	        __metadata$10('design:paramtypes', [MediaMonitor, _angular_core.ElementRef, _angular_core.Renderer])
	    ], FlexAlignDirective);
	    return FlexAlignDirective;
	}(BaseFxDirective));
	
	var __extends$6 = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var __decorate$11 = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata$11 = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var FLEX_FILL_CSS = {
	    'margin': 0,
	    'width': '100%',
	    'height': '100%',
	    'min-width': '100%',
	    'min-height': '100%'
	};
	/**
	 * 'fxFill' flexbox styling directive
	 *  Maximizes width and height of element in a layout container
	 *
	 *  NOTE: fxFill is NOT responsive API!!
	 */
	var FlexFillDirective = (function (_super) {
	    __extends$6(FlexFillDirective, _super);
	    function FlexFillDirective(monitor, elRef, renderer) {
	        _super.call(this, monitor, elRef, renderer);
	        this.elRef = elRef;
	        this.renderer = renderer;
	        this._applyStyleToElement(FLEX_FILL_CSS);
	    }
	    FlexFillDirective = __decorate$11([
	        _angular_core.Directive({ selector: "\n  [fxFill],\n  [fxFill.xs]\n  [fxFill.gt-xs],\n  [fxFill.sm],\n  [fxFill.gt-sm]\n  [fxFill.md],\n  [fxFill.gt-md]\n  [fxFill.lg],\n  [fxFill.gt-lg],\n  [fxFill.xl]\n" }), 
	        __metadata$11('design:paramtypes', [MediaMonitor, _angular_core.ElementRef, _angular_core.Renderer])
	    ], FlexFillDirective);
	    return FlexFillDirective;
	}(BaseFxDirective));
	
	var __extends$7 = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var __decorate$12 = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata$12 = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	/**
	 * 'flex-offset' flexbox styling directive
	 * Configures the 'margin-left' of the element in a layout container
	 */
	var FlexOffsetDirective = (function (_super) {
	    __extends$7(FlexOffsetDirective, _super);
	    function FlexOffsetDirective(monitor, elRef, renderer) {
	        _super.call(this, monitor, elRef, renderer);
	    }
	    Object.defineProperty(FlexOffsetDirective.prototype, "offset", {
	        set: function (val) { this._cacheInput('offset', val); },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(FlexOffsetDirective.prototype, "offsetXs", {
	        set: function (val) { this._cacheInput('offsetXs', val); },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(FlexOffsetDirective.prototype, "offsetGtXs", {
	        set: function (val) { this._cacheInput('offsetGtXs', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexOffsetDirective.prototype, "offsetSm", {
	        set: function (val) { this._cacheInput('offsetSm', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexOffsetDirective.prototype, "offsetGtSm", {
	        set: function (val) { this._cacheInput('offsetGtSm', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexOffsetDirective.prototype, "offsetMd", {
	        set: function (val) { this._cacheInput('offsetMd', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexOffsetDirective.prototype, "offsetGtMd", {
	        set: function (val) { this._cacheInput('offsetGtMd', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexOffsetDirective.prototype, "offsetLg", {
	        set: function (val) { this._cacheInput('offsetLg', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexOffsetDirective.prototype, "offsetGtLg", {
	        set: function (val) { this._cacheInput('offsetGtLg', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexOffsetDirective.prototype, "offsetXl", {
	        set: function (val) { this._cacheInput('offsetXl', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    // *********************************************
	    // Lifecycle Methods
	    // *********************************************
	    /**
	     * For @Input changes on the current mq activation property, see onMediaQueryChanges()
	     */
	    FlexOffsetDirective.prototype.ngOnChanges = function (changes) {
	        if (changes['offset'] != null || this._mqActivation) {
	            this._updateWithValue();
	        }
	    };
	    /**
	     * After the initial onChanges, build an mqActivation object that bridges
	     * mql change events to onMediaQueryChange handlers
	     */
	    FlexOffsetDirective.prototype.ngOnInit = function () {
	        var _this = this;
	        this._listenForMediaQueryChanges('offset', 0, function (changes) {
	            _this._updateWithValue(changes.value);
	        });
	    };
	    // *********************************************
	    // Protected methods
	    // *********************************************
	    FlexOffsetDirective.prototype._updateWithValue = function (value) {
	        value = value || this._queryInput("offset") || 0;
	        if (this._mqActivation) {
	            value = this._mqActivation.activatedInput;
	        }
	        this._applyStyleToElement(this._buildCSS(value));
	    };
	    FlexOffsetDirective.prototype._buildCSS = function (offset) {
	        var isPercent = String(offset).indexOf('%') > -1;
	        var isPx = String(offset).indexOf('px') > -1;
	        if (!isPx && !isPercent && !isNaN(offset))
	            offset = offset + '%';
	        return { 'margin-left': "" + offset };
	    };
	    __decorate$12([
	        _angular_core.Input('fxFlexOffset'), 
	        __metadata$12('design:type', Object), 
	        __metadata$12('design:paramtypes', [Object])
	    ], FlexOffsetDirective.prototype, "offset", null);
	    __decorate$12([
	        _angular_core.Input('fxFlexOffset.xs'), 
	        __metadata$12('design:type', Object), 
	        __metadata$12('design:paramtypes', [Object])
	    ], FlexOffsetDirective.prototype, "offsetXs", null);
	    __decorate$12([
	        _angular_core.Input('fxFlexOffset.gt-xs'), 
	        __metadata$12('design:type', Object), 
	        __metadata$12('design:paramtypes', [Object])
	    ], FlexOffsetDirective.prototype, "offsetGtXs", null);
	    __decorate$12([
	        _angular_core.Input('fxFlexOffset.sm'), 
	        __metadata$12('design:type', Object), 
	        __metadata$12('design:paramtypes', [Object])
	    ], FlexOffsetDirective.prototype, "offsetSm", null);
	    __decorate$12([
	        _angular_core.Input('fxFlexOffset.gt-sm'), 
	        __metadata$12('design:type', Object), 
	        __metadata$12('design:paramtypes', [Object])
	    ], FlexOffsetDirective.prototype, "offsetGtSm", null);
	    __decorate$12([
	        _angular_core.Input('fxFlexOffset.md'), 
	        __metadata$12('design:type', Object), 
	        __metadata$12('design:paramtypes', [Object])
	    ], FlexOffsetDirective.prototype, "offsetMd", null);
	    __decorate$12([
	        _angular_core.Input('fxFlexOffset.gt-md'), 
	        __metadata$12('design:type', Object), 
	        __metadata$12('design:paramtypes', [Object])
	    ], FlexOffsetDirective.prototype, "offsetGtMd", null);
	    __decorate$12([
	        _angular_core.Input('fxFlexOffset.lg'), 
	        __metadata$12('design:type', Object), 
	        __metadata$12('design:paramtypes', [Object])
	    ], FlexOffsetDirective.prototype, "offsetLg", null);
	    __decorate$12([
	        _angular_core.Input('fxFlexOffset.gt-lg'), 
	        __metadata$12('design:type', Object), 
	        __metadata$12('design:paramtypes', [Object])
	    ], FlexOffsetDirective.prototype, "offsetGtLg", null);
	    __decorate$12([
	        _angular_core.Input('fxFlexOffset.xl'), 
	        __metadata$12('design:type', Object), 
	        __metadata$12('design:paramtypes', [Object])
	    ], FlexOffsetDirective.prototype, "offsetXl", null);
	    FlexOffsetDirective = __decorate$12([
	        _angular_core.Directive({ selector: "\n  [fxFlexOffset],\n  [fxFlexOffset.xs]\n  [fxFlexOffset.gt-xs],\n  [fxFlexOffset.sm],\n  [fxFlexOffset.gt-sm]\n  [fxFlexOffset.md],\n  [fxFlexOffset.gt-md]\n  [fxFlexOffset.lg],\n  [fxFlexOffset.gt-lg],\n  [fxFlexOffset.xl]\n" }), 
	        __metadata$12('design:paramtypes', [MediaMonitor, _angular_core.ElementRef, _angular_core.Renderer])
	    ], FlexOffsetDirective);
	    return FlexOffsetDirective;
	}(BaseFxDirective));
	
	var __extends$8 = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var __decorate$13 = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata$13 = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	/**
	 * 'flex-order' flexbox styling directive
	 * Configures the positional ordering of the element in a sorted layout container
	 * @see https://css-tricks.com/almanac/properties/o/order/
	 */
	var FlexOrderDirective = (function (_super) {
	    __extends$8(FlexOrderDirective, _super);
	    function FlexOrderDirective(monitor, elRef, renderer) {
	        _super.call(this, monitor, elRef, renderer);
	    }
	    Object.defineProperty(FlexOrderDirective.prototype, "order", {
	        set: function (val) { this._cacheInput('order', val); },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(FlexOrderDirective.prototype, "orderXs", {
	        set: function (val) { this._cacheInput('orderXs', val); },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(FlexOrderDirective.prototype, "orderGtXs", {
	        set: function (val) { this._cacheInput('orderGtXs', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexOrderDirective.prototype, "orderSm", {
	        set: function (val) { this._cacheInput('orderSm', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexOrderDirective.prototype, "orderGtSm", {
	        set: function (val) { this._cacheInput('orderGtSm', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexOrderDirective.prototype, "orderMd", {
	        set: function (val) { this._cacheInput('orderMd', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexOrderDirective.prototype, "orderGtMd", {
	        set: function (val) { this._cacheInput('orderGtMd', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexOrderDirective.prototype, "orderLg", {
	        set: function (val) { this._cacheInput('orderLg', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexOrderDirective.prototype, "orderGtLg", {
	        set: function (val) { this._cacheInput('orderGtLg', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(FlexOrderDirective.prototype, "orderXl", {
	        set: function (val) { this._cacheInput('orderXl', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    // *********************************************
	    // Lifecycle Methods
	    // *********************************************
	    /**
	     * For @Input changes on the current mq activation property, see onMediaQueryChanges()
	     */
	    FlexOrderDirective.prototype.ngOnChanges = function (changes) {
	        if (changes['order'] != null || this._mqActivation) {
	            this._updateWithValue();
	        }
	    };
	    /**
	     * After the initial onChanges, build an mqActivation object that bridges
	     * mql change events to onMediaQueryChange handlers
	     */
	    FlexOrderDirective.prototype.ngOnInit = function () {
	        var _this = this;
	        this._listenForMediaQueryChanges('order', '0', function (changes) {
	            _this._updateWithValue(changes.value);
	        });
	        this._updateWithValue();
	    };
	    // *********************************************
	    // Protected methods
	    // *********************************************
	    FlexOrderDirective.prototype._updateWithValue = function (value) {
	        value = value || this._queryInput("order") || '0';
	        if (this._mqActivation) {
	            value = this._mqActivation.activatedInput;
	        }
	        this._applyStyleToElement(this._buildCSS(value));
	    };
	    FlexOrderDirective.prototype._buildCSS = function (value) {
	        value = parseInt(value, 10);
	        return { order: isNaN(value) ? 0 : value };
	    };
	    __decorate$13([
	        _angular_core.Input('fxFlexOrder'), 
	        __metadata$13('design:type', Object), 
	        __metadata$13('design:paramtypes', [Object])
	    ], FlexOrderDirective.prototype, "order", null);
	    __decorate$13([
	        _angular_core.Input('fxFlexOrder.xs'), 
	        __metadata$13('design:type', Object), 
	        __metadata$13('design:paramtypes', [Object])
	    ], FlexOrderDirective.prototype, "orderXs", null);
	    __decorate$13([
	        _angular_core.Input('fxFlexOrder.gt-xs'), 
	        __metadata$13('design:type', Object), 
	        __metadata$13('design:paramtypes', [Object])
	    ], FlexOrderDirective.prototype, "orderGtXs", null);
	    __decorate$13([
	        _angular_core.Input('fxFlexOrder.sm'), 
	        __metadata$13('design:type', Object), 
	        __metadata$13('design:paramtypes', [Object])
	    ], FlexOrderDirective.prototype, "orderSm", null);
	    __decorate$13([
	        _angular_core.Input('fxFlexOrder.gt-sm'), 
	        __metadata$13('design:type', Object), 
	        __metadata$13('design:paramtypes', [Object])
	    ], FlexOrderDirective.prototype, "orderGtSm", null);
	    __decorate$13([
	        _angular_core.Input('fxFlexOrder.md'), 
	        __metadata$13('design:type', Object), 
	        __metadata$13('design:paramtypes', [Object])
	    ], FlexOrderDirective.prototype, "orderMd", null);
	    __decorate$13([
	        _angular_core.Input('fxFlexOrder.gt-md'), 
	        __metadata$13('design:type', Object), 
	        __metadata$13('design:paramtypes', [Object])
	    ], FlexOrderDirective.prototype, "orderGtMd", null);
	    __decorate$13([
	        _angular_core.Input('fxFlexOrder.lg'), 
	        __metadata$13('design:type', Object), 
	        __metadata$13('design:paramtypes', [Object])
	    ], FlexOrderDirective.prototype, "orderLg", null);
	    __decorate$13([
	        _angular_core.Input('fxFlexOrder.gt-lg'), 
	        __metadata$13('design:type', Object), 
	        __metadata$13('design:paramtypes', [Object])
	    ], FlexOrderDirective.prototype, "orderGtLg", null);
	    __decorate$13([
	        _angular_core.Input('fxFlexOrder.xl'), 
	        __metadata$13('design:type', Object), 
	        __metadata$13('design:paramtypes', [Object])
	    ], FlexOrderDirective.prototype, "orderXl", null);
	    FlexOrderDirective = __decorate$13([
	        _angular_core.Directive({ selector: "\n  [fxFlexOrder],\n  [fxFlexOrder.xs]\n  [fxFlexOrder.gt-xs],\n  [fxFlexOrder.sm],\n  [fxFlexOrder.gt-sm]\n  [fxFlexOrder.md],\n  [fxFlexOrder.gt-md]\n  [fxFlexOrder.lg],\n  [fxFlexOrder.gt-lg],\n  [fxFlexOrder.xl]\n" }), 
	        __metadata$13('design:paramtypes', [MediaMonitor, _angular_core.ElementRef, _angular_core.Renderer])
	    ], FlexOrderDirective);
	    return FlexOrderDirective;
	}(BaseFxDirective));
	
	var __extends$9 = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var __decorate$14 = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata$14 = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var __param$5 = (this && this.__param) || function (paramIndex, decorator) {
	    return function (target, key) { decorator(target, key, paramIndex); }
	};
	/**
	 * 'layout-align' flexbox styling directive
	 *  Defines positioning of child elements along main and cross axis in a layout container
	 *  Optional values: {main-axis} values or {main-axis cross-axis} value pairs
	 *
	 *  @see https://css-tricks.com/almanac/properties/j/justify-content/
	 *  @see https://css-tricks.com/almanac/properties/a/align-items/
	 *  @see https://css-tricks.com/almanac/properties/a/align-content/
	 */
	var LayoutAlignDirective = (function (_super) {
	    __extends$9(LayoutAlignDirective, _super);
	    function LayoutAlignDirective(monitor, elRef, renderer, container) {
	        _super.call(this, monitor, elRef, renderer);
	        this._layout = 'row'; // default flex-direction
	        if (container) {
	            this._layoutWatcher = container.layout$.subscribe(this._onLayoutChange.bind(this));
	        }
	    }
	    Object.defineProperty(LayoutAlignDirective.prototype, "align", {
	        set: function (val) { this._cacheInput('align', val); },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(LayoutAlignDirective.prototype, "alignXs", {
	        set: function (val) { this._cacheInput('alignXs', val); },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(LayoutAlignDirective.prototype, "alignGtXs", {
	        set: function (val) { this._cacheInput('alignGtXs', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutAlignDirective.prototype, "alignSm", {
	        set: function (val) { this._cacheInput('alignSm', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutAlignDirective.prototype, "alignGtSm", {
	        set: function (val) { this._cacheInput('alignGtSm', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutAlignDirective.prototype, "alignMd", {
	        set: function (val) { this._cacheInput('alignMd', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutAlignDirective.prototype, "alignGtMd", {
	        set: function (val) { this._cacheInput('alignGtMd', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutAlignDirective.prototype, "alignLg", {
	        set: function (val) { this._cacheInput('alignLg', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutAlignDirective.prototype, "alignGtLg", {
	        set: function (val) { this._cacheInput('alignGtLg', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutAlignDirective.prototype, "alignXl", {
	        set: function (val) { this._cacheInput('alignXl', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    // *********************************************
	    // Lifecycle Methods
	    // *********************************************
	    LayoutAlignDirective.prototype.ngOnChanges = function (changes) {
	        if (changes['align'] != null || this._mqActivation) {
	            this._updateWithValue();
	        }
	    };
	    /**
	     * After the initial onChanges, build an mqActivation object that bridges
	     * mql change events to onMediaQueryChange handlers
	     */
	    LayoutAlignDirective.prototype.ngOnInit = function () {
	        var _this = this;
	        this._listenForMediaQueryChanges('align', 'start stretch', function (changes) {
	            _this._updateWithValue(changes.value);
	        });
	        this._updateWithValue();
	    };
	    LayoutAlignDirective.prototype.ngOnDestroy = function () {
	        _super.prototype.ngOnDestroy.call(this);
	        if (this._layoutWatcher) {
	            this._layoutWatcher.unsubscribe();
	        }
	    };
	    // *********************************************
	    // Protected methods
	    // *********************************************
	    /**
	     *
	     */
	    LayoutAlignDirective.prototype._updateWithValue = function (value) {
	        value = value || this._queryInput("align") || 'start stretch';
	        if (this._mqActivation) {
	            value = this._mqActivation.activatedInput;
	        }
	        this._applyStyleToElement(this._buildCSS(value));
	        this._allowStretching(value, !this._layout ? "row" : this._layout);
	    };
	    /**
	     * Cache the parent container 'flex-direction' and update the 'flex' styles
	     */
	    LayoutAlignDirective.prototype._onLayoutChange = function (direction) {
	        var _this = this;
	        this._layout = (direction || '').toLowerCase().replace('-reverse', '');
	        if (!LAYOUT_VALUES.find(function (x) { return x === _this._layout; }))
	            this._layout = 'row';
	        var value = this._queryInput("align") || 'start stretch';
	        if (this._mqActivation) {
	            value = this._mqActivation.activatedInput;
	        }
	        this._allowStretching(value, this._layout || "row");
	    };
	    LayoutAlignDirective.prototype._buildCSS = function (align) {
	        var css = {}, _a = align.split(' '), main_axis = _a[0], cross_axis = _a[1];
	        css['justify-content'] = 'flex-start'; // default main axis
	        css['align-items'] = 'stretch'; // default cross axis
	        css['align-content'] = 'stretch'; // default cross axis
	        // Main axis
	        switch (main_axis) {
	            case 'center':
	                css['justify-content'] = 'center';
	                break;
	            case 'space-around':
	                css['justify-content'] = 'space-around';
	                break;
	            case 'space-between':
	                css['justify-content'] = 'space-between';
	                break;
	            case 'end':
	                css['justify-content'] = 'flex-end';
	                break;
	        }
	        // Cross-axis
	        switch (cross_axis) {
	            case 'start':
	                css['align-items'] = css['align-content'] = 'flex-start';
	                break;
	            case 'baseline':
	                css['align-items'] = 'baseline';
	                break;
	            case 'center':
	                css['align-items'] = css['align-content'] = 'center';
	                break;
	            case 'end':
	                css['align-items'] = css['align-content'] = 'flex-end';
	                break;
	            default:
	                break;
	        }
	        return extendObject(css, {
	            'display': 'flex',
	            'flex-direction': this._layout || "row",
	            'box-sizing': 'border-box'
	        });
	    };
	    /**
	     * Update container element to 'stretch' as needed...
	     * NOTE: this is only done if the crossAxis is explicitly set to 'stretch'
	     */
	    LayoutAlignDirective.prototype._allowStretching = function (align, layout) {
	        var _a = align.split(' '), cross_axis = _a[1];
	        if (cross_axis == 'stretch') {
	            // Use `null` values to remove style
	            this._applyStyleToElement({
	                'box-sizing': 'border-box',
	                'max-width': (layout === 'column') ? '100%' : null,
	                'max-height': (layout === 'row') ? '100%' : null
	            });
	        }
	    };
	    __decorate$14([
	        _angular_core.Input('fxLayoutAlign'), 
	        __metadata$14('design:type', Object), 
	        __metadata$14('design:paramtypes', [Object])
	    ], LayoutAlignDirective.prototype, "align", null);
	    __decorate$14([
	        _angular_core.Input('fxLayoutAlign.xs'), 
	        __metadata$14('design:type', Object), 
	        __metadata$14('design:paramtypes', [Object])
	    ], LayoutAlignDirective.prototype, "alignXs", null);
	    __decorate$14([
	        _angular_core.Input('fxLayoutAlign.gt-xs'), 
	        __metadata$14('design:type', Object), 
	        __metadata$14('design:paramtypes', [Object])
	    ], LayoutAlignDirective.prototype, "alignGtXs", null);
	    __decorate$14([
	        _angular_core.Input('fxLayoutAlign.sm'), 
	        __metadata$14('design:type', Object), 
	        __metadata$14('design:paramtypes', [Object])
	    ], LayoutAlignDirective.prototype, "alignSm", null);
	    __decorate$14([
	        _angular_core.Input('fxLayoutAlign.gt-sm'), 
	        __metadata$14('design:type', Object), 
	        __metadata$14('design:paramtypes', [Object])
	    ], LayoutAlignDirective.prototype, "alignGtSm", null);
	    __decorate$14([
	        _angular_core.Input('fxLayoutAlign.md'), 
	        __metadata$14('design:type', Object), 
	        __metadata$14('design:paramtypes', [Object])
	    ], LayoutAlignDirective.prototype, "alignMd", null);
	    __decorate$14([
	        _angular_core.Input('fxLayoutAlign.gt-md'), 
	        __metadata$14('design:type', Object), 
	        __metadata$14('design:paramtypes', [Object])
	    ], LayoutAlignDirective.prototype, "alignGtMd", null);
	    __decorate$14([
	        _angular_core.Input('fxLayoutAlign.lg'), 
	        __metadata$14('design:type', Object), 
	        __metadata$14('design:paramtypes', [Object])
	    ], LayoutAlignDirective.prototype, "alignLg", null);
	    __decorate$14([
	        _angular_core.Input('fxLayoutAlign.gt-lg'), 
	        __metadata$14('design:type', Object), 
	        __metadata$14('design:paramtypes', [Object])
	    ], LayoutAlignDirective.prototype, "alignGtLg", null);
	    __decorate$14([
	        _angular_core.Input('fxLayoutAlign.xl'), 
	        __metadata$14('design:type', Object), 
	        __metadata$14('design:paramtypes', [Object])
	    ], LayoutAlignDirective.prototype, "alignXl", null);
	    LayoutAlignDirective = __decorate$14([
	        _angular_core.Directive({ selector: "\n  [fxLayoutAlign],\n  [fxLayoutAlign.xs]\n  [fxLayoutAlign.gt-xs],\n  [fxLayoutAlign.sm],\n  [fxLayoutAlign.gt-sm]\n  [fxLayoutAlign.md],\n  [fxLayoutAlign.gt-md]\n  [fxLayoutAlign.lg],\n  [fxLayoutAlign.gt-lg],\n  [fxLayoutAlign.xl]\n" }),
	        __param$5(3, _angular_core.Optional()),
	        __param$5(3, _angular_core.Self()), 
	        __metadata$14('design:paramtypes', [MediaMonitor, _angular_core.ElementRef, _angular_core.Renderer, LayoutDirective])
	    ], LayoutAlignDirective);
	    return LayoutAlignDirective;
	}(BaseFxDirective));
	
	var __extends$10 = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var __decorate$15 = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata$15 = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	/**
	 * 'layout-padding' styling directive
	 *  Defines padding of child elements in a layout container
	 */
	var LayoutGapDirective = (function (_super) {
	    __extends$10(LayoutGapDirective, _super);
	    function LayoutGapDirective(monitor, elRef, renderer) {
	        _super.call(this, monitor, elRef, renderer);
	    }
	    Object.defineProperty(LayoutGapDirective.prototype, "gap", {
	        set: function (val) { this._cacheInput('gap', val); },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(LayoutGapDirective.prototype, "gapXs", {
	        set: function (val) { this._cacheInput('gapXs', val); },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(LayoutGapDirective.prototype, "gapGtXs", {
	        set: function (val) { this._cacheInput('gapGtXs', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutGapDirective.prototype, "gapSm", {
	        set: function (val) { this._cacheInput('gapSm', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutGapDirective.prototype, "gapGtSm", {
	        set: function (val) { this._cacheInput('gapGtSm', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutGapDirective.prototype, "gapMd", {
	        set: function (val) { this._cacheInput('gapMd', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutGapDirective.prototype, "gapGtMd", {
	        set: function (val) { this._cacheInput('gapGtMd', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutGapDirective.prototype, "gapLg", {
	        set: function (val) { this._cacheInput('gapLg', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutGapDirective.prototype, "gapGtLg", {
	        set: function (val) { this._cacheInput('gapGtLg', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    Object.defineProperty(LayoutGapDirective.prototype, "gapXl", {
	        set: function (val) { this._cacheInput('gapXl', val); },
	        enumerable: true,
	        configurable: true
	    });
	    
	    // *********************************************
	    // Lifecycle Methods
	    // *********************************************
	    LayoutGapDirective.prototype.ngOnChanges = function (changes) {
	        if (changes['gap'] != null || this._mqActivation) {
	            this._updateWithValue();
	        }
	    };
	    /**
	     * After the initial onChanges, build an mqActivation object that bridges
	     * mql change events to onMediaQueryChange handlers
	     */
	    LayoutGapDirective.prototype.ngAfterContentInit = function () {
	        var _this = this;
	        this._listenForMediaQueryChanges('gap', '0', function (changes) {
	            _this._updateWithValue(changes.value);
	        });
	        this._updateWithValue();
	    };
	    // *********************************************
	    // Protected methods
	    // *********************************************
	    /**
	     *
	     */
	    LayoutGapDirective.prototype._updateWithValue = function (value) {
	        value = value || this._queryInput("padding") || '0';
	        if (this._mqActivation) {
	            value = this._mqActivation.activatedInput;
	        }
	        // For each `element` child, set the padding styles...
	        var items = this.childrenNodes
	            .filter(function (el) { return (el.nodeType === 1); }) // only Element types
	            .filter(function (el, j) { return j > 0; }); // skip first element since gaps are needed
	        this._applyStyleToElements(this._buildCSS(value), items);
	    };
	    LayoutGapDirective.prototype._buildCSS = function (value) {
	        return { 'margin-left': value };
	    };
	    __decorate$15([
	        _angular_core.Input('fxLayoutGap'), 
	        __metadata$15('design:type', Object), 
	        __metadata$15('design:paramtypes', [Object])
	    ], LayoutGapDirective.prototype, "gap", null);
	    __decorate$15([
	        _angular_core.Input('fxLayoutGap.xs'), 
	        __metadata$15('design:type', Object), 
	        __metadata$15('design:paramtypes', [Object])
	    ], LayoutGapDirective.prototype, "gapXs", null);
	    __decorate$15([
	        _angular_core.Input('fxLayoutGap.gt-xs'), 
	        __metadata$15('design:type', Object), 
	        __metadata$15('design:paramtypes', [Object])
	    ], LayoutGapDirective.prototype, "gapGtXs", null);
	    __decorate$15([
	        _angular_core.Input('fxLayoutGap.sm'), 
	        __metadata$15('design:type', Object), 
	        __metadata$15('design:paramtypes', [Object])
	    ], LayoutGapDirective.prototype, "gapSm", null);
	    __decorate$15([
	        _angular_core.Input('fxLayoutGap.gt-sm'), 
	        __metadata$15('design:type', Object), 
	        __metadata$15('design:paramtypes', [Object])
	    ], LayoutGapDirective.prototype, "gapGtSm", null);
	    __decorate$15([
	        _angular_core.Input('fxLayoutGap.md'), 
	        __metadata$15('design:type', Object), 
	        __metadata$15('design:paramtypes', [Object])
	    ], LayoutGapDirective.prototype, "gapMd", null);
	    __decorate$15([
	        _angular_core.Input('fxLayoutGap.gt-md'), 
	        __metadata$15('design:type', Object), 
	        __metadata$15('design:paramtypes', [Object])
	    ], LayoutGapDirective.prototype, "gapGtMd", null);
	    __decorate$15([
	        _angular_core.Input('fxLayoutGap.lg'), 
	        __metadata$15('design:type', Object), 
	        __metadata$15('design:paramtypes', [Object])
	    ], LayoutGapDirective.prototype, "gapLg", null);
	    __decorate$15([
	        _angular_core.Input('fxLayoutGap.gt-lg'), 
	        __metadata$15('design:type', Object), 
	        __metadata$15('design:paramtypes', [Object])
	    ], LayoutGapDirective.prototype, "gapGtLg", null);
	    __decorate$15([
	        _angular_core.Input('fxLayoutGap.xl'), 
	        __metadata$15('design:type', Object), 
	        __metadata$15('design:paramtypes', [Object])
	    ], LayoutGapDirective.prototype, "gapXl", null);
	    LayoutGapDirective = __decorate$15([
	        _angular_core.Directive({ selector: "\n  [fxLayoutGap],\n  [fxLayoutGap.xs]\n  [fxLayoutGap.gt-xs],\n  [fxLayoutGap.sm],\n  [fxLayoutGap.gt-sm]\n  [fxLayoutGap.md],\n  [fxLayoutGap.gt-md]\n  [fxLayoutGap.lg],\n  [fxLayoutGap.gt-lg],\n  [fxLayoutGap.xl]\n" }), 
	        __metadata$15('design:paramtypes', [MediaMonitor, _angular_core.ElementRef, _angular_core.Renderer])
	    ], LayoutGapDirective);
	    return LayoutGapDirective;
	}(BaseFxDirective));
	
	var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	/**
	 * Since the equivalent results are easily achieved with a css class attached to each
	 * layout child, these have been deprecated and removed from the API.
	 *
	 *  import {LayoutPaddingDirective} from './api/layout-padding';
	 *  import {LayoutMarginDirective} from './api/layout-margin';
	 */
	var ALL_DIRECTIVES = [
	    LayoutDirective,
	    LayoutWrapDirective,
	    LayoutGapDirective,
	    LayoutAlignDirective,
	    FlexDirective,
	    FlexOrderDirective,
	    FlexOffsetDirective,
	    FlexFillDirective,
	    FlexAlignDirective,
	    ShowDirective,
	    HideDirective,
	];
	/**
	 *
	 */
	var FlexLayoutModule = (function () {
	    function FlexLayoutModule() {
	    }
	    FlexLayoutModule.forRoot = function () {
	        return { ngModule: FlexLayoutModule, providers: [MediaMonitor] };
	    };
	    FlexLayoutModule = __decorate([
	        _angular_core.NgModule({
	            declarations: ALL_DIRECTIVES,
	            imports: [MediaQueriesModule],
	            exports: [MediaQueriesModule].concat(ALL_DIRECTIVES),
	            providers: []
	        }), 
	        __metadata('design:paramtypes', [])
	    ], FlexLayoutModule);
	    return FlexLayoutModule;
	}());
	
	exports.BaseFxDirective = BaseFxDirective;
	exports.KeyOptions = KeyOptions;
	exports.ResponsiveActivation = ResponsiveActivation;
	exports.FlexLayoutModule = FlexLayoutModule;
	exports.BreakPointRegistry = BreakPointRegistry;
	exports.RESPONSIVE_ALIASES = RESPONSIVE_ALIASES;
	exports.RAW_DEFAULTS = RAW_DEFAULTS;
	exports.BREAKPOINTS = BREAKPOINTS;
	exports.BreakPointsProvider = BreakPointsProvider;
	exports.MatchMediaObservable = MatchMediaObservable;
	exports.MatchMedia = MatchMedia;
	exports.MediaChange = MediaChange;
	exports.MediaMonitor = MediaMonitor;
	exports.MediaQueriesModule = MediaQueriesModule;
	exports.applyCssPrefixes = applyCssPrefixes;
	exports.toAlignContentValue = toAlignContentValue;
	exports.toBoxValue = toBoxValue;
	exports.toBoxOrient = toBoxOrient;
	exports.toBoxDirection = toBoxDirection;
	exports.toBoxOrdinal = toBoxOrdinal;
	exports.extendObject = extendObject;
	exports.mergeAlias = mergeAlias;
	
	Object.defineProperty(exports, '__esModule', { value: true });
	
	})));


/***/ },
/* 28 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = (__webpack_require__(17))(342);

/***/ },
/* 29 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = (__webpack_require__(17))(343);

/***/ },
/* 30 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = (__webpack_require__(17))(305);

/***/ },
/* 31 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = (__webpack_require__(17))(354);

/***/ },
/* 32 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	function __export(m) {
	    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
	}
	// angular.
	var core_1 = __webpack_require__(21);
	var common_1 = __webpack_require__(33);
	var forms_1 = __webpack_require__(24);
	var flex_layout_1 = __webpack_require__(27);
	var material_1 = __webpack_require__(26);
	var angular2localization_1 = __webpack_require__(31);
	// components.
	var form_errormsg_directive_1 = __webpack_require__(34);
	var content_directive_1 = __webpack_require__(35);
	var column_component_1 = __webpack_require__(36);
	var table_component_1 = __webpack_require__(37);
	var list_component_1 = __webpack_require__(47);
	__webpack_require__(51);
	var CoreModule = (function () {
	    function CoreModule() {
	    }
	    CoreModule.forRoot = function () {
	        return {
	            ngModule: CoreModule,
	            providers: []
	        };
	    };
	    CoreModule = __decorate([
	        core_1.NgModule({
	            imports: [
	                common_1.CommonModule,
	                forms_1.FormsModule,
	                flex_layout_1.FlexLayoutModule,
	                material_1.MaterialModule,
	                angular2localization_1.LocaleModule,
	                angular2localization_1.LocalizationModule
	            ],
	            exports: [
	                form_errormsg_directive_1.FormErrorMessageDirective,
	                content_directive_1.ContentDirective,
	                column_component_1.ColumnComponent,
	                table_component_1.TableComponent,
	                list_component_1.ListComponent
	            ],
	            declarations: [
	                form_errormsg_directive_1.FormErrorMessageDirective,
	                content_directive_1.ContentDirective,
	                column_component_1.ColumnComponent,
	                table_component_1.TableComponent,
	                list_component_1.ListComponent
	            ],
	            providers: []
	        }), 
	        __metadata('design:paramtypes', [])
	    ], CoreModule);
	    return CoreModule;
	}());
	exports.CoreModule = CoreModule;
	__export(__webpack_require__(52));
	__export(__webpack_require__(55));


/***/ },
/* 33 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = (__webpack_require__(17))(278);

/***/ },
/* 34 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var core_1 = __webpack_require__(21);
	var forms_1 = __webpack_require__(24);
	var angular2localization_1 = __webpack_require__(31);
	var FormErrorMessageDirective = (function () {
	    function FormErrorMessageDirective(element, localizationService) {
	        this.element = element;
	        this.localizationService = localizationService;
	    }
	    Object.defineProperty(FormErrorMessageDirective.prototype, "errormsg", {
	        get: function () {
	            return this._formControl;
	        },
	        set: function (value) {
	            this._formControl = value;
	            this._formControl.statusChanges.subscribe(this.onStatusChange.bind(this));
	        },
	        enumerable: true,
	        configurable: true
	    });
	    FormErrorMessageDirective.prototype.onStatusChange = function () {
	        if (!(this._formControl.pristine || this._formControl.valid)) {
	            for (var errorName in this._formControl.errors) {
	                var errorValue = this._formControl.errors[errorName];
	                if (errorValue) {
	                    if (errorName == "server") {
	                        this.element.nativeElement.innerHTML = errorValue;
	                    }
	                    else {
	                        var args = {
	                            field: this.fieldName
	                        };
	                        var errorKey = "core.validation." + errorName;
	                        var errorText = this.localizationService.translate(errorKey, args);
	                        this.element.nativeElement.innerHTML = errorText;
	                    }
	                }
	            }
	        }
	        else {
	            this.element.nativeElement.innerHTML = null;
	        }
	    };
	    __decorate([
	        core_1.Input(), 
	        __metadata('design:type', forms_1.FormControl)
	    ], FormErrorMessageDirective.prototype, "errormsg", null);
	    __decorate([
	        core_1.Input(), 
	        __metadata('design:type', String)
	    ], FormErrorMessageDirective.prototype, "fieldName", void 0);
	    FormErrorMessageDirective = __decorate([
	        core_1.Directive({
	            selector: "[errormsg]"
	        }), 
	        __metadata('design:paramtypes', [core_1.ElementRef, angular2localization_1.LocalizationService])
	    ], FormErrorMessageDirective);
	    return FormErrorMessageDirective;
	}());
	exports.FormErrorMessageDirective = FormErrorMessageDirective;


/***/ },
/* 35 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var core_1 = __webpack_require__(21);
	var ContentDirective = (function () {
	    function ContentDirective(_viewContainer) {
	        this._viewContainer = _viewContainer;
	        this._hasView = false;
	    }
	    Object.defineProperty(ContentDirective.prototype, "condition", {
	        set: function (value) {
	            if (value)
	                this.show();
	            else
	                this.hide();
	        },
	        enumerable: true,
	        configurable: true
	    });
	    ContentDirective.prototype.show = function () {
	        if (!this._hasView) {
	            this._hasView = true;
	            var view = this._viewContainer.createEmbeddedView(this.template);
	            view.context.model = this.model;
	            view.context.index = this.index;
	        }
	    };
	    ContentDirective.prototype.hide = function () {
	        if (this._hasView) {
	            this._hasView = false;
	            this._viewContainer.clear();
	        }
	    };
	    __decorate([
	        core_1.Input(), 
	        __metadata('design:type', Object)
	    ], ContentDirective.prototype, "model", void 0);
	    __decorate([
	        core_1.Input(), 
	        __metadata('design:type', Number)
	    ], ContentDirective.prototype, "index", void 0);
	    __decorate([
	        core_1.Input(), 
	        __metadata('design:type', core_1.TemplateRef)
	    ], ContentDirective.prototype, "template", void 0);
	    __decorate([
	        core_1.Input(), 
	        __metadata('design:type', Boolean), 
	        __metadata('design:paramtypes', [Boolean])
	    ], ContentDirective.prototype, "condition", null);
	    ContentDirective = __decorate([
	        core_1.Directive({
	            selector: "d-content"
	        }), 
	        __metadata('design:paramtypes', [core_1.ViewContainerRef])
	    ], ContentDirective);
	    return ContentDirective;
	}());
	exports.ContentDirective = ContentDirective;


/***/ },
/* 36 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var core_1 = __webpack_require__(21);
	var ColumnComponent = (function () {
	    function ColumnComponent(viewContainer) {
	        this.viewContainer = viewContainer;
	        this.type = "text";
	        this.canSort = false;
	    }
	    __decorate([
	        core_1.Input(), 
	        __metadata('design:type', String)
	    ], ColumnComponent.prototype, "title", void 0);
	    __decorate([
	        core_1.Input(), 
	        __metadata('design:type', String)
	    ], ColumnComponent.prototype, "type", void 0);
	    __decorate([
	        core_1.Input(), 
	        __metadata('design:type', String)
	    ], ColumnComponent.prototype, "property", void 0);
	    __decorate([
	        core_1.Input(), 
	        __metadata('design:type', Boolean)
	    ], ColumnComponent.prototype, "canSort", void 0);
	    __decorate([
	        core_1.Input(), 
	        __metadata('design:type', String)
	    ], ColumnComponent.prototype, "width", void 0);
	    __decorate([
	        core_1.ContentChild(core_1.TemplateRef), 
	        __metadata('design:type', core_1.TemplateRef)
	    ], ColumnComponent.prototype, "template", void 0);
	    ColumnComponent = __decorate([
	        core_1.Directive({
	            selector: "d-column"
	        }), 
	        __metadata('design:paramtypes', [core_1.ViewContainerRef])
	    ], ColumnComponent);
	    return ColumnComponent;
	}());
	exports.ColumnComponent = ColumnComponent;


/***/ },
/* 37 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var core_1 = __webpack_require__(21);
	var column_component_1 = __webpack_require__(36);
	var collection_component_1 = __webpack_require__(38);
	var TableComponent = (function (_super) {
	    __extends(TableComponent, _super);
	    function TableComponent() {
	        _super.apply(this, arguments);
	        this.columns = [];
	    }
	    TableComponent.prototype.toggleSort = function (canSort, propertyName) {
	        if (canSort) {
	            this.dataSource.toggleSort(propertyName);
	        }
	    };
	    __decorate([
	        core_1.ContentChildren(column_component_1.ColumnComponent), 
	        __metadata('design:type', Array)
	    ], TableComponent.prototype, "columns", void 0);
	    TableComponent = __decorate([
	        core_1.Component({
	            selector: 'd-table',
	            template: __webpack_require__(39),
	            encapsulation: core_1.ViewEncapsulation.None,
	            styles: [__webpack_require__(40)],
	        }), 
	        __metadata('design:paramtypes', [])
	    ], TableComponent);
	    return TableComponent;
	}(collection_component_1.CollectionComponent));
	exports.TableComponent = TableComponent;


/***/ },
/* 38 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var core_1 = __webpack_require__(21);
	var CollectionItem = (function () {
	    function CollectionItem(model) {
	        this.model = model;
	        this._selected = false;
	        this.selectedChange = new core_1.EventEmitter();
	    }
	    Object.defineProperty(CollectionItem.prototype, "selected", {
	        get: function () {
	            return this._selected;
	        },
	        set: function (value) {
	            if (this._selected !== value) {
	                this._selected = value;
	                this.selectedChange.emit(this);
	            }
	        },
	        enumerable: true,
	        configurable: true
	    });
	    return CollectionItem;
	}());
	exports.CollectionItem = CollectionItem;
	var CollectionComponent = (function () {
	    function CollectionComponent() {
	        this._suspendFlag = false;
	        this._items = [];
	        this._selection = [];
	        this.keyProperty = "id";
	        this.displayProperty = "name";
	        this.itemsChange = new core_1.EventEmitter();
	        this.dataSourceChanged = new core_1.EventEmitter();
	        this.selectionChange = new core_1.EventEmitter();
	        this.allSelectedChange = new core_1.EventEmitter();
	    }
	    Object.defineProperty(CollectionComponent.prototype, "items", {
	        //items.
	        get: function () {
	            return this._items;
	        },
	        set: function (value) {
	            if (this._items !== value) {
	                if (this._items) {
	                    for (var _i = 0, _a = this._items; _i < _a.length; _i++) {
	                        var item = _a[_i];
	                        item.selectedChange.unsubscribe();
	                    }
	                }
	                this._items = value;
	                if (this._items) {
	                    for (var _b = 0, _c = this._items; _b < _c.length; _b++) {
	                        var item = _c[_b];
	                        item.selectedChange.subscribe(this.onItemSelectedChange.bind(this));
	                    }
	                }
	                this.syncSelection();
	                this.itemsChange.emit(this._items);
	            }
	        },
	        enumerable: true,
	        configurable: true
	    });
	    Object.defineProperty(CollectionComponent.prototype, "dataSource", {
	        //item source.
	        get: function () {
	            return this._dataSource;
	        },
	        set: function (value) {
	            if (this._dataSource != value) {
	                if (this._dataSource) {
	                    this._dataSource.itemsChange.unsubscribe();
	                }
	                this._dataSource = value;
	                if (this._dataSource) {
	                    this._dataSource.itemsChange.subscribe(this.onDataSourceItemsChange.bind(this));
	                    this.onDataSourceItemsChange();
	                }
	                this.dataSourceChanged.emit(this.dataSource);
	            }
	        },
	        enumerable: true,
	        configurable: true
	    });
	    CollectionComponent.prototype.onDataSourceItemsChange = function () {
	        if (this._dataSource && this._dataSource.items) {
	            this.items = this._dataSource.items.map(function (i) { return new CollectionItem(i); });
	        }
	    };
	    Object.defineProperty(CollectionComponent.prototype, "selection", {
	        // selection.
	        get: function () {
	            return this._selection;
	        },
	        set: function (value) {
	            if (this._selection !== value) {
	                this._selection = value;
	                this.syncSelection();
	                this.selectionChange.emit(this._selection);
	            }
	        },
	        enumerable: true,
	        configurable: true
	    });
	    CollectionComponent.prototype.syncSelection = function () {
	        var _this = this;
	        this._suspendFlag = true;
	        if (this._items && this._selection) {
	            var _loop_1 = function(item) {
	                item.selected = this_1._selection.find(function (s) { return item.model[_this.keyProperty] == s[_this.keyProperty]; }) !== undefined;
	            };
	            var this_1 = this;
	            for (var _i = 0, _a = this._items; _i < _a.length; _i++) {
	                var item = _a[_i];
	                _loop_1(item);
	            }
	            this.syncAllSelected();
	        }
	        this._suspendFlag = false;
	    };
	    CollectionComponent.prototype.onItemSelectedChange = function (item) {
	        var _this = this;
	        if (!this._suspendFlag) {
	            if (!this._selection) {
	                this._selection = [];
	            }
	            var existing = this._selection.findIndex(function (s) { return s[_this.keyProperty] == item.model[_this.keyProperty]; });
	            if (item.selected && existing < 0)
	                this._selection.push(item.model);
	            else if (!item.selected && existing >= 0)
	                this._selection.splice(existing, 1);
	            this.selectionChange.emit(this._selection);
	            this.syncAllSelected();
	        }
	    };
	    Object.defineProperty(CollectionComponent.prototype, "allSelected", {
	        // all selected.
	        get: function () {
	            return this._allSelected;
	        },
	        set: function (value) {
	            if (this._allSelected !== value) {
	                this._allSelected = value;
	                this._suspendFlag = true;
	                this._selection = [];
	                if (value) {
	                    for (var _i = 0, _a = this._items; _i < _a.length; _i++) {
	                        var item = _a[_i];
	                        item.selected = true;
	                        this._selection.push(item.model);
	                    }
	                }
	                else {
	                    for (var _b = 0, _c = this._items; _b < _c.length; _b++) {
	                        var item = _c[_b];
	                        item.selected = false;
	                    }
	                }
	                this.selectionChange.emit(this._selection);
	                this.allSelectedChange.emit(this._allSelected);
	                this._suspendFlag = false;
	            }
	        },
	        enumerable: true,
	        configurable: true
	    });
	    CollectionComponent.prototype.syncAllSelected = function () {
	        var allSelected = this._items.length == this._selection.length && this._items.length > 0;
	        if (this._allSelected !== allSelected) {
	            this._allSelected = allSelected;
	            this.allSelectedChange.emit(allSelected);
	        }
	    };
	    __decorate([
	        core_1.Input(), 
	        __metadata('design:type', String)
	    ], CollectionComponent.prototype, "keyProperty", void 0);
	    __decorate([
	        core_1.Input(), 
	        __metadata('design:type', String)
	    ], CollectionComponent.prototype, "displayProperty", void 0);
	    __decorate([
	        core_1.Input(), 
	        __metadata('design:type', Array)
	    ], CollectionComponent.prototype, "items", null);
	    __decorate([
	        core_1.Output(), 
	        __metadata('design:type', Object)
	    ], CollectionComponent.prototype, "itemsChange", void 0);
	    __decorate([
	        core_1.Input(), 
	        __metadata('design:type', Object)
	    ], CollectionComponent.prototype, "dataSource", null);
	    __decorate([
	        core_1.Output(), 
	        __metadata('design:type', Object)
	    ], CollectionComponent.prototype, "dataSourceChanged", void 0);
	    __decorate([
	        core_1.Input(), 
	        __metadata('design:type', Array)
	    ], CollectionComponent.prototype, "selection", null);
	    __decorate([
	        core_1.Output(), 
	        __metadata('design:type', Object)
	    ], CollectionComponent.prototype, "selectionChange", void 0);
	    __decorate([
	        core_1.Input(), 
	        __metadata('design:type', Boolean)
	    ], CollectionComponent.prototype, "allSelected", null);
	    __decorate([
	        core_1.Output(), 
	        __metadata('design:type', core_1.EventEmitter)
	    ], CollectionComponent.prototype, "allSelectedChange", void 0);
	    return CollectionComponent;
	}());
	exports.CollectionComponent = CollectionComponent;


/***/ },
/* 39 */
/***/ function(module, exports) {

	module.exports = "<table>\n    <thead>\n        <tr>\n            <th class=\"d-selector\">\n                <md-checkbox [(ngModel)]=\"allSelected\"></md-checkbox>\n            </th>\n            <th *ngFor=\"let column of columns\" (click)=\"toggleSort(column.canSort, column.property)\"\n                [ngClass]=\"{ 'd-sortable': column.canSort,\n                             'd-sorted': dataSource.sortBy==column.property,\n                             'd-sorted-asc': dataSource.sortBy==column.property && dataSource.sortDirection == 1,\n                             'd-type-number': column.type == 'number',\n                             'd-type-text': column.type == 'text' }\">\n                <span>{{column.title}}</span>\n            </th>\n        </tr>\n    </thead>\n    <tbody>\n        <tr *ngFor=\"let item of items; let index = index;\" [ngClass]=\"{'d-selected': item.selected }\">\n            <td class=\"d-selector\">\n                <md-checkbox [(ngModel)]=\"item.selected\"></md-checkbox>\n            </td>\n            <td *ngFor=\"let column of columns\" [ngClass]=\"{ 'd-type-number': column.type == 'number', 'd-type-text': column.type == 'text' }\" [ngStyle]=\"{ 'width' : column.width }\">\n                <d-content [condition]=\"column.template\" [template]=\"column.template\" [model]=\"item.model\" [index]=\"index\"></d-content>\n                <span *ngIf=\"!column.template\">{{item.model[column.property]}}</span>\n            </td>\n        </tr>\n    </tbody>\n</table>"

/***/ },
/* 40 */
/***/ function(module, exports, __webpack_require__) {

	
	        var result = __webpack_require__(41);
	
	        if (typeof result === "string") {
	            module.exports = result;
	        } else {
	            module.exports = result.toString();
	        }
	    

/***/ },
/* 41 */
/***/ function(module, exports, __webpack_require__) {

	exports = module.exports = __webpack_require__(42)();
	// imports
	
	
	// module
	exports.push([module.id, "/**\n * Applies styles for users in high contrast mode. Note that this only applies\n * to Microsoft browsers. Chrome can be included by checking for the `html[hc]`\n * attribute, however Chrome handles high contrast differently.\n */\n/* fallback */\n@font-face {\n  font-family: 'Material Icons';\n  font-style: normal;\n  font-weight: 400;\n  src: url(" + __webpack_require__(43) + ");\n  /* For IE6-8 */\n  src: local(\"Material Icons\"), local(\"MaterialIcons-Regular\"), url(" + __webpack_require__(44) + ") format(\"woff2\"), url(" + __webpack_require__(45) + ") format(\"woff\"), url(" + __webpack_require__(46) + ") format(\"truetype\"); }\n\n.material-icons, d-table table thead tr th.d-sorted:after {\n  font-family: 'Material Icons';\n  font-weight: normal;\n  font-style: normal;\n  font-size: 24px;\n  line-height: 1;\n  letter-spacing: normal;\n  text-transform: none;\n  display: inline-block;\n  white-space: nowrap;\n  word-wrap: normal;\n  direction: ltr;\n  -webkit-font-feature-settings: 'liga';\n  -webkit-font-smoothing: antialiased; }\n\n/*header*/\n/*body*/\n/*checkbox*/\nd-table table {\n  position: relative;\n  white-space: nowrap;\n  border-collapse: collapse;\n  background-color: #ffffff;\n  font-family: Roboto, \"Helvetica Neue\", sans-serif;\n  box-shadow: 0px 3px 5px -1px rgba(0, 0, 0, 0.2), 0px 6px 10px 0px rgba(0, 0, 0, 0.14), 0px 1px 18px 0px rgba(0, 0, 0, 0.12); }\n  d-table table thead tr {\n    height: 56px;\n    font-size: 12px;\n    color: rgba(0, 0, 0, 0.54); }\n    d-table table thead tr th {\n      padding-left: 12px;\n      padding-right: 44px;\n      vertical-align: middle;\n      text-overflow: ellipsis;\n      box-sizing: border-box; }\n      d-table table thead tr th:first-of-type {\n        padding-left: 24px;\n        padding-right: 12px; }\n      d-table table thead tr th:last-of-type {\n        padding-right: 24px; }\n    d-table table thead tr th.d-sorted {\n      color: rgba(0, 0, 0, 0.87); }\n      d-table table thead tr th.d-sorted:after {\n        content: 'arrow_downward';\n        font-size: 16px;\n        display: inline;\n        vertical-align: text-bottom; }\n    d-table table thead tr th.d-sorted-asc:after {\n      content: 'arrow_upward'; }\n    d-table table thead tr th.d-sortable {\n      cursor: pointer; }\n    d-table table thead tr th.d-type-number {\n      text-align: right; }\n    d-table table thead tr th.d-type-text {\n      text-align: left; }\n    d-table table thead tr th.d-selector {\n      text-align: left;\n      width: 42px; }\n  d-table table tbody tr {\n    height: 48px;\n    font-size: 13px;\n    color: rgba(0, 0, 0, 0.87); }\n    d-table table tbody tr.is-selected {\n      background-color: #f5f5f5; }\n    d-table table tbody tr:hover {\n      background-color: #eeeeee; }\n    d-table table tbody tr td {\n      border-top: 1px solid rgba(0, 0, 0, 0.12);\n      padding-left: 12px;\n      padding-right: 44px;\n      vertical-align: middle;\n      box-sizing: border-box; }\n      d-table table tbody tr td:first-of-type {\n        padding-left: 24px;\n        padding-right: 12px; }\n      d-table table tbody tr td:last-of-type {\n        padding-right: 24px; }\n    d-table table tbody tr td.d-type-number {\n      text-align: right; }\n    d-table table tbody tr td.d-type-text {\n      text-align: left; }\n    d-table table tbody tr th.d-selector {\n      text-align: left;\n      width: 42px; }\n  d-table table tbody tr.d-selected {\n    background-color: #f5f5f5; }\n\nd-table .md-checkbox-inner-container {\n  width: 18px;\n  height: 18px;\n  color: rgba(0, 0, 0, 0.54); }\n", ""]);
	
	// exports


/***/ },
/* 42 */
/***/ function(module, exports) {

	/*
		MIT License http://www.opensource.org/licenses/mit-license.php
		Author Tobias Koppers @sokra
	*/
	// css base code, injected by the css-loader
	module.exports = function() {
		var list = [];
	
		// return the list of modules as css string
		list.toString = function toString() {
			var result = [];
			for(var i = 0; i < this.length; i++) {
				var item = this[i];
				if(item[2]) {
					result.push("@media " + item[2] + "{" + item[1] + "}");
				} else {
					result.push(item[1]);
				}
			}
			return result.join("");
		};
	
		// import a list of modules into the list
		list.i = function(modules, mediaQuery) {
			if(typeof modules === "string")
				modules = [[null, modules, ""]];
			var alreadyImportedModules = {};
			for(var i = 0; i < this.length; i++) {
				var id = this[i][0];
				if(typeof id === "number")
					alreadyImportedModules[id] = true;
			}
			for(i = 0; i < modules.length; i++) {
				var item = modules[i];
				// skip already imported module
				// this implementation is not 100% perfect for weird media query combinations
				//  when a module is imported multiple times with different media queries.
				//  I hope this will never occur (Hey this way we have smaller bundles)
				if(typeof item[0] !== "number" || !alreadyImportedModules[item[0]]) {
					if(mediaQuery && !item[2]) {
						item[2] = mediaQuery;
					} else if(mediaQuery) {
						item[2] = "(" + item[2] + ") and (" + mediaQuery + ")";
					}
					list.push(item);
				}
			}
		};
		return list;
	};


/***/ },
/* 43 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = __webpack_require__.p + "../fonts/MaterialIcons-Regular.eot";

/***/ },
/* 44 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = __webpack_require__.p + "../fonts/MaterialIcons-Regular.woff2";

/***/ },
/* 45 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = __webpack_require__.p + "../fonts/MaterialIcons-Regular.woff";

/***/ },
/* 46 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = __webpack_require__.p + "../fonts/MaterialIcons-Regular.ttf";

/***/ },
/* 47 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var core_1 = __webpack_require__(21);
	var collection_component_1 = __webpack_require__(38);
	var ListComponent = (function (_super) {
	    __extends(ListComponent, _super);
	    function ListComponent() {
	        _super.apply(this, arguments);
	    }
	    __decorate([
	        core_1.ContentChild(core_1.TemplateRef), 
	        __metadata('design:type', core_1.TemplateRef)
	    ], ListComponent.prototype, "template", void 0);
	    __decorate([
	        core_1.Input(), 
	        __metadata('design:type', String)
	    ], ListComponent.prototype, "placeholder", void 0);
	    ListComponent = __decorate([
	        core_1.Component({
	            selector: 'd-list',
	            template: __webpack_require__(48),
	            encapsulation: core_1.ViewEncapsulation.None,
	            styles: [__webpack_require__(49)],
	        }), 
	        __metadata('design:paramtypes', [])
	    ], ListComponent);
	    return ListComponent;
	}(collection_component_1.CollectionComponent));
	exports.ListComponent = ListComponent;


/***/ },
/* 48 */
/***/ function(module, exports) {

	module.exports = "<div class=\"d-list-wrapper\">\r\n    <div class=\"d-placeholder\">\r\n        <label>{{placeholder}}</label>\r\n    </div>\r\n    <div class=\"d-list-item\" *ngFor=\"let item of items\">\r\n        <md-checkbox [(ngModel)]=\"item.selected\">\r\n\r\n            <d-content [condition]=\"template\"\r\n                       [template]=\"template\"\r\n                       [model]=\"item.model\"\r\n                       [index]=\"index\"></d-content>\r\n\r\n            <span *ngIf=\"!template\">{{item.model[displayProperty]}}</span>\r\n        </md-checkbox>\r\n    </div>\r\n</div>"

/***/ },
/* 49 */
/***/ function(module, exports, __webpack_require__) {

	
	        var result = __webpack_require__(50);
	
	        if (typeof result === "string") {
	            module.exports = result;
	        } else {
	            module.exports = result.toString();
	        }
	    

/***/ },
/* 50 */
/***/ function(module, exports, __webpack_require__) {

	exports = module.exports = __webpack_require__(42)();
	// imports
	
	
	// module
	exports.push([module.id, "/**\n * Applies styles for users in high contrast mode. Note that this only applies\n * to Microsoft browsers. Chrome can be included by checking for the `html[hc]`\n * attribute, however Chrome handles high contrast differently.\n */\n/* fallback */\n@font-face {\n  font-family: 'Material Icons';\n  font-style: normal;\n  font-weight: 400;\n  src: url(" + __webpack_require__(43) + ");\n  /* For IE6-8 */\n  src: local(\"Material Icons\"), local(\"MaterialIcons-Regular\"), url(" + __webpack_require__(44) + ") format(\"woff2\"), url(" + __webpack_require__(45) + ") format(\"woff\"), url(" + __webpack_require__(46) + ") format(\"truetype\"); }\n\n.material-icons {\n  font-family: 'Material Icons';\n  font-weight: normal;\n  font-style: normal;\n  font-size: 24px;\n  line-height: 1;\n  letter-spacing: normal;\n  text-transform: none;\n  display: inline-block;\n  white-space: nowrap;\n  word-wrap: normal;\n  direction: ltr;\n  -webkit-font-feature-settings: 'liga';\n  -webkit-font-smoothing: antialiased; }\n\nd-list .d-list-wrapper {\n  font-family: Roboto, \"Helvetica Neue\", sans-serif;\n  font-size: 100%;\n  margin-top: 5px;\n  margin-bottom: 5px; }\n  d-list .d-list-wrapper .d-placeholder {\n    margin-bottom: 3px;\n    font-size: 75%;\n    color: rgba(0, 0, 0, 0.38); }\n  d-list .d-list-wrapper .d-list-item {\n    padding-top: 5px;\n    padding-bottom: 5px; }\n  d-list .d-list-wrapper .md-checkbox-inner-container {\n    width: 18px;\n    height: 18px;\n    color: rgba(0, 0, 0, 0.54); }\n", ""]);
	
	// exports


/***/ },
/* 51 */
/***/ function(module, exports) {

	// removed by extract-text-webpack-plugin

/***/ },
/* 52 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var core_1 = __webpack_require__(21);
	var base_datasource_1 = __webpack_require__(53);
	(function (SortDirection) {
	    SortDirection[SortDirection["Asc"] = 0] = "Asc";
	    SortDirection[SortDirection["Desc"] = 1] = "Desc";
	})(exports.SortDirection || (exports.SortDirection = {}));
	var SortDirection = exports.SortDirection;
	var DataPage = (function () {
	    function DataPage() {
	    }
	    return DataPage;
	}());
	exports.DataPage = DataPage;
	var HttpRestCollectionDataSource = (function (_super) {
	    __extends(HttpRestCollectionDataSource, _super);
	    function HttpRestCollectionDataSource(http, baseUrl) {
	        _super.call(this, http);
	        this.baseUrl = baseUrl;
	        this.sortDirection = SortDirection.Asc;
	        this.pageIndex = 1;
	        this._items = [];
	        this.itemsChange = new core_1.EventEmitter();
	    }
	    Object.defineProperty(HttpRestCollectionDataSource.prototype, "items", {
	        get: function () {
	            return this._items;
	        },
	        set: function (value) {
	            if (value != this._items) {
	                this._items = value;
	                this.itemsChange.emit(this._items);
	            }
	        },
	        enumerable: true,
	        configurable: true
	    });
	    HttpRestCollectionDataSource.prototype.load = function () {
	        var _this = this;
	        var params = Object.assign({}, this.requestParams);
	        params.searchText = this.searchText;
	        params.sortBy = this.sortBy;
	        params.sortDirection = this.sortDirection;
	        params.pageIndex = this.pageIndex;
	        params.pageSize = this.pageSize;
	        params.noCount = this.noCount;
	        var request = this.execute(this.baseUrl, this.requestParams, "get", null);
	        request.subscribe(function (data) { return _this.items = data; }, function (error) { });
	        return request;
	    };
	    HttpRestCollectionDataSource.prototype.toggleSort = function (propertyName) {
	        if (this.sortBy != propertyName) {
	            this.sortDirection = SortDirection.Asc;
	        }
	        else {
	            switch (this.sortDirection) {
	                case SortDirection.Desc:
	                    this.sortDirection = SortDirection.Asc;
	                    break;
	                case SortDirection.Asc:
	                    this.sortDirection = SortDirection.Desc;
	                    break;
	            }
	        }
	        this.sortBy = propertyName;
	        return this.load();
	    };
	    return HttpRestCollectionDataSource;
	}(base_datasource_1.HttpRestBaseDataSource));
	exports.HttpRestCollectionDataSource = HttpRestCollectionDataSource;


/***/ },
/* 53 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var http_1 = __webpack_require__(25);
	var ReplaySubject_1 = __webpack_require__(54);
	var HttpRestBaseDataSource = (function () {
	    function HttpRestBaseDataSource(http) {
	        this.http = http;
	        this.keyProperty = "id";
	        this.requestParams = {};
	    }
	    HttpRestBaseDataSource.prototype.execute = function (baseUrl, params, verb, data) {
	        var _this = this;
	        var result = new ReplaySubject_1.ReplaySubject();
	        this.isBusy = true;
	        var searchParams = new http_1.URLSearchParams();
	        for (var paramProp in params) {
	            searchParams.append(paramProp, params[paramProp]);
	        }
	        var url = baseUrl.replace(/:[a-zA-z0-9]+/, function (match, args) {
	            var paramName = match.substr(1);
	            var value = searchParams.get(paramName);
	            if (!value)
	                throw new URIError("parameter " + match + " in url '" + this.url + "' is not defined.");
	            searchParams.delete(paramName); //don't send duplicated parameters.
	            return value;
	        }.bind(this));
	        var request;
	        switch (verb) {
	            case "get":
	                request = this.http.get(url, { search: searchParams });
	                break;
	            case "post":
	                request = this.http.post(url, data, { search: searchParams });
	                break;
	            case "put":
	                request = this.http.put(url, data, { search: searchParams });
	                break;
	            case "delete":
	                request = this.http.delete(url, { search: searchParams });
	                break;
	        }
	        request.subscribe(function (data) { return result.next(_this.handleData(data)); }, function (error) { return result.error(_this.handleError(error)); }, function () {
	            result.complete();
	            _this.isBusy = false;
	        });
	        return result;
	    };
	    HttpRestBaseDataSource.prototype.handleData = function (data) {
	        return data.json();
	    };
	    HttpRestBaseDataSource.prototype.handleError = function (error) {
	        return error.json();
	    };
	    return HttpRestBaseDataSource;
	}());
	exports.HttpRestBaseDataSource = HttpRestBaseDataSource;
	var ApiError = (function () {
	    function ApiError() {
	    }
	    return ApiError;
	}());
	exports.ApiError = ApiError;


/***/ },
/* 54 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = (__webpack_require__(17))(429);

/***/ },
/* 55 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var core_1 = __webpack_require__(21);
	var base_datasource_1 = __webpack_require__(53);
	var HttpRestModelDataSource = (function (_super) {
	    __extends(HttpRestModelDataSource, _super);
	    function HttpRestModelDataSource(http, baseUrl) {
	        _super.call(this, http);
	        this.baseUrl = baseUrl;
	        this.model = {};
	        this.modelChange = new core_1.EventEmitter();
	        this.modelErrors = {};
	        this.modelErrorsChange = new core_1.EventEmitter();
	        this.isNew = true;
	        this.isNewChange = new core_1.EventEmitter();
	        this.loadUrl = baseUrl;
	        this.saveUrl = baseUrl.replace("/:" + this.keyProperty, "");
	        this.deleteUrl = baseUrl;
	    }
	    HttpRestModelDataSource.prototype.load = function (key) {
	        var _this = this;
	        var params = Object.assign({}, this.requestParams);
	        params[this.keyProperty] = key;
	        var request = this.execute(this.loadUrl, params, "get", null);
	        request.subscribe(function (data) {
	            _this.model = data;
	        }, function (error) { });
	        return request;
	    };
	    HttpRestModelDataSource.prototype.save = function () {
	        var _this = this;
	        var request = this.execute(this.saveUrl, this.requestParams, "post", this.model);
	        request.subscribe(function (data) {
	            _this.model = data;
	        }, function (error) { });
	        return request;
	    };
	    HttpRestModelDataSource.prototype.delete = function () {
	        return null;
	    };
	    return HttpRestModelDataSource;
	}(base_datasource_1.HttpRestBaseDataSource));
	exports.HttpRestModelDataSource = HttpRestModelDataSource;


/***/ },
/* 56 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var core_1 = __webpack_require__(21);
	var router_1 = __webpack_require__(57);
	var home_component_1 = __webpack_require__(58);
	var user_list_component_1 = __webpack_require__(60);
	var user_edit_component_1 = __webpack_require__(63);
	var appRoutes = [
	    { path: "", redirectTo: "home", pathMatch: "full" },
	    { path: "home", component: home_component_1.HomeComponent },
	    { path: "security/users",
	        children: [
	            { path: "", component: user_list_component_1.UserListComponent },
	            { path: ":id", component: user_edit_component_1.UserEditComponent },
	            { path: "new", component: user_edit_component_1.UserEditComponent }
	        ]
	    }
	];
	var AppRoutingModule = (function () {
	    function AppRoutingModule() {
	    }
	    AppRoutingModule = __decorate([
	        core_1.NgModule({
	            imports: [
	                router_1.RouterModule.forRoot(appRoutes)
	            ],
	            exports: [
	                router_1.RouterModule
	            ]
	        }), 
	        __metadata('design:paramtypes', [])
	    ], AppRoutingModule);
	    return AppRoutingModule;
	}());
	exports.AppRoutingModule = AppRoutingModule;


/***/ },
/* 57 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = (__webpack_require__(17))(304);

/***/ },
/* 58 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var core_1 = __webpack_require__(21);
	var HomeComponent = (function () {
	    function HomeComponent() {
	    }
	    HomeComponent = __decorate([
	        core_1.Component({
	            selector: "app-home",
	            template: __webpack_require__(59)
	        }), 
	        __metadata('design:paramtypes', [])
	    ], HomeComponent);
	    return HomeComponent;
	}());
	exports.HomeComponent = HomeComponent;


/***/ },
/* 59 */
/***/ function(module, exports) {

	module.exports = "<md-toolbar color=\"primary\">\n    <button md-button (click)=\"sidenav.open()\"><span class=\"material-icons\">menu</span></button>\n    <span>Home</span>\n</md-toolbar>\n\n<md-sidenav-container>\n    <md-sidenav #sidenav>\n        <app-nav></app-nav>\n    </md-sidenav>\n    <p></p>\n    \n    Home!\n    Select an option from the left panel...\n\n</md-sidenav-container>"

/***/ },
/* 60 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var core_1 = __webpack_require__(21);
	var users_datasource_1 = __webpack_require__(61);
	var UserListComponent = (function () {
	    function UserListComponent(users) {
	        this.users = users;
	    }
	    UserListComponent.prototype.ngOnInit = function () {
	        this.users.load();
	    };
	    UserListComponent = __decorate([
	        core_1.Component({
	            selector: "user-list",
	            template: __webpack_require__(62),
	            providers: [users_datasource_1.UserCollectionDataSource]
	        }), 
	        __metadata('design:paramtypes', [users_datasource_1.UserCollectionDataSource])
	    ], UserListComponent);
	    return UserListComponent;
	}());
	exports.UserListComponent = UserListComponent;


/***/ },
/* 61 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var core_1 = __webpack_require__(21);
	var http_1 = __webpack_require__(25);
	var core_module_1 = __webpack_require__(32);
	var core_module_2 = __webpack_require__(32);
	var User = (function () {
	    function User() {
	    }
	    return User;
	}());
	exports.User = User;
	var UserQuery = (function () {
	    function UserQuery() {
	    }
	    return UserQuery;
	}());
	exports.UserQuery = UserQuery;
	var UserCollectionDataSource = (function (_super) {
	    __extends(UserCollectionDataSource, _super);
	    function UserCollectionDataSource(http) {
	        _super.call(this, http, "/api/users");
	        this.sortBy = "name";
	    }
	    UserCollectionDataSource = __decorate([
	        core_1.Injectable(), 
	        __metadata('design:paramtypes', [http_1.Http])
	    ], UserCollectionDataSource);
	    return UserCollectionDataSource;
	}(core_module_1.HttpRestCollectionDataSource));
	exports.UserCollectionDataSource = UserCollectionDataSource;
	var UserModelDataSource = (function (_super) {
	    __extends(UserModelDataSource, _super);
	    function UserModelDataSource(http) {
	        _super.call(this, http, "api/users/:id");
	        this.requestParams.culture = "es-AR";
	    }
	    UserModelDataSource = __decorate([
	        core_1.Injectable(), 
	        __metadata('design:paramtypes', [http_1.Http])
	    ], UserModelDataSource);
	    return UserModelDataSource;
	}(core_module_2.HttpRestModelDataSource));
	exports.UserModelDataSource = UserModelDataSource;


/***/ },
/* 62 */
/***/ function(module, exports) {

	module.exports = "<md-toolbar color=\"primary\">\n    <button md-button (click)=\"sidenav.open()\"><md-icon>menu</md-icon></button>\n    <span>Users</span>\n    <div fxFlex></div>\n    <button md-button [routerLink]=\"['/security/users/new']\">\n        <md-icon>add</md-icon>\n        <span>New</span>\n    </button>\n    <button md-button>\n        <md-icon>delete</md-icon>\n        <span>Delete</span>\n    </button>\n    <button md-button>\n        <md-icon>more_vertical</md-icon>\n    </button>\n</md-toolbar>\n\n<md-sidenav-container>\n    <md-sidenav #sidenav>\r\n        <app-nav></app-nav>\r\n    </md-sidenav>\n    <div class=\"flex-container\" fxLayout=\"row\" fxLayoutAlign=\"center start\">\n        <div class=\"flex-item\">\n            <d-table [dataSource]=\"users\">\n                <d-column property=\"id\" type=\"number\" title=\"Key\" canSort=\"true\"></d-column>\n                <d-column property=\"name\" title=\"Name\" type=\"text\" canSort=\"true\"></d-column>\n                <d-column property=\"email\" title=\"E-Mail\" type=\"text\" canSort=\"true\"></d-column>\n                <d-column>\n                    <template let-model=\"model\">\n                        <button md-icon-button [routerLink]=\"['/security/users/' + model.id]\"><md-icon>edit</md-icon></button>\n                    </template>\n                </d-column>\n            </d-table>\n        </div>\n    </div>\n</md-sidenav-container>"

/***/ },
/* 63 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var core_1 = __webpack_require__(21);
	var router_1 = __webpack_require__(57);
	var users_datasource_1 = __webpack_require__(61);
	var roles_datasource_1 = __webpack_require__(64);
	var angular2localization_1 = __webpack_require__(31);
	var UserEditComponent = (function (_super) {
	    __extends(UserEditComponent, _super);
	    function UserEditComponent(userSource, rolesSource, activatedRoute, localization) {
	        _super.call(this, null, localization);
	        this.userSource = userSource;
	        this.rolesSource = rolesSource;
	        this.activatedRoute = activatedRoute;
	        this.localization = localization;
	    }
	    UserEditComponent.prototype.ngOnInit = function () {
	        var key = this.activatedRoute.snapshot.params["id"];
	        if (key !== "new") {
	            this.userSource.load(key);
	        }
	        this.rolesSource.load();
	    };
	    UserEditComponent.prototype.save = function (frm) {
	        var _this = this;
	        if (frm.valid) {
	            this.userSource.save()
	                .subscribe(function (ok) { return _this.close(); }, function (error) {
	                if (error.memberErrors) {
	                    for (var member in error.memberErrors) {
	                        var ctrl = frm.controls[member];
	                        ctrl.setErrors({ server: error.memberErrors[member] });
	                    }
	                }
	            });
	        }
	    };
	    UserEditComponent.prototype.delete = function () {
	        var _this = this;
	        this.userSource.delete()
	            .subscribe(function (ok) { return _this.close(); });
	    };
	    UserEditComponent.prototype.close = function () {
	        window.history.back();
	    };
	    UserEditComponent = __decorate([
	        core_1.Component({
	            selector: "user-edit",
	            template: __webpack_require__(65),
	            providers: [users_datasource_1.UserModelDataSource, roles_datasource_1.RoleCollectionDataSource]
	        }), 
	        __metadata('design:paramtypes', [users_datasource_1.UserModelDataSource, roles_datasource_1.RoleCollectionDataSource, router_1.ActivatedRoute, angular2localization_1.LocalizationService])
	    ], UserEditComponent);
	    return UserEditComponent;
	}(angular2localization_1.Locale));
	exports.UserEditComponent = UserEditComponent;


/***/ },
/* 64 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var core_1 = __webpack_require__(21);
	var http_1 = __webpack_require__(25);
	var core_module_1 = __webpack_require__(32);
	var Role = (function () {
	    function Role() {
	    }
	    return Role;
	}());
	exports.Role = Role;
	var RoleQuery = (function () {
	    function RoleQuery() {
	    }
	    return RoleQuery;
	}());
	exports.RoleQuery = RoleQuery;
	var RoleCollectionDataSource = (function (_super) {
	    __extends(RoleCollectionDataSource, _super);
	    function RoleCollectionDataSource(http) {
	        _super.call(this, http, "/api/roles");
	        this.sortBy = "name";
	    }
	    RoleCollectionDataSource = __decorate([
	        core_1.Injectable(), 
	        __metadata('design:paramtypes', [http_1.Http])
	    ], RoleCollectionDataSource);
	    return RoleCollectionDataSource;
	}(core_module_1.HttpRestCollectionDataSource));
	exports.RoleCollectionDataSource = RoleCollectionDataSource;


/***/ },
/* 65 */
/***/ function(module, exports) {

	module.exports = "<md-toolbar color=\"primary\">\r\n    <span *ngIf=\"userSource.isNew\" translate=\"yes\">security.users.new.title</span>\r\n    <span *ngIf=\"!userSource.isNew\">{{ 'security.users.edit.title' | translate:lang:{ user: userSource.model.name } }}</span>\r\n    <div fxFlex></div>\r\n    <button md-button type=\"submit\" form=\"userForm\">\r\n        <md-icon>save</md-icon>\r\n        <span>Save</span>\r\n    </button>\r\n    <button md-button (click)=\"delete();\">\r\n        <md-icon>delete</md-icon>\r\n        <span>Delete</span>\r\n    </button>\r\n    <button md-button>\r\n        <md-icon>more_vertical</md-icon>\r\n    </button>\r\n</md-toolbar>\r\n\r\n<md-card>\r\n    <form id=\"userForm\" (ngSubmit)=\"save(frm)\" #frm=\"ngForm\" novalidate>\r\n        <md-input-container>\r\n            <input required md-input name=\"Name\" #nameCtrl=\"ngModel\" [(ngModel)]=\"userSource.model.name\" placeholder=\"{{ 'security.users.user.name' | translate:lang }}\" />\r\n            <md-hint [errormsg]=\"nameCtrl\" fieldName=\"Name\"></md-hint>\r\n        </md-input-container>\r\n\r\n        <md-input-container>\r\n            <input md-input name=\"Email\" #mailCtrl=\"ngModel\" [(ngModel)]=\"userSource.model.email\" placeholder=\"{{ 'security.users.user.mail' | translate:lang }}\" />\r\n            <md-hint [errormsg]=\"mailCtrl\" fieldName=\"Mail\"></md-hint>\r\n        </md-input-container>\r\n\r\n        <d-list [dataSource]=\"rolesSource\" [selection]=\"userSource.model.roles\" placeholder=\"roles\">\r\n            <template let-model=\"model\" let-index=\"index\">\r\n                {{model.name}} \r\n            </template>\r\n        </d-list>\r\n    </form>\r\n</md-card>"

/***/ },
/* 66 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var core_1 = __webpack_require__(21);
	var AppNavComponent = (function () {
	    function AppNavComponent() {
	    }
	    AppNavComponent = __decorate([
	        core_1.Component({
	            selector: "app-nav",
	            template: __webpack_require__(67)
	        }), 
	        __metadata('design:paramtypes', [])
	    ], AppNavComponent);
	    return AppNavComponent;
	}());
	exports.AppNavComponent = AppNavComponent;


/***/ },
/* 67 */
/***/ function(module, exports) {

	module.exports = "<ul>\n    <li>\n        <a [routerLink]=\"['/home']\">Home</a>\n    </li>\n    <li>\n        <a [routerLink]=\"['/security/users']\">Users example</a>\n    </li>\n</ul>"

/***/ },
/* 68 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	var __extends = (this && this.__extends) || function (d, b) {
	    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
	    function __() { this.constructor = d; }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
	    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
	    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
	    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
	    return c > 3 && r && Object.defineProperty(target, key, r), r;
	};
	var __metadata = (this && this.__metadata) || function (k, v) {
	    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
	};
	var core_1 = __webpack_require__(21);
	var angular2localization_1 = __webpack_require__(31);
	var AppComponent = (function (_super) {
	    __extends(AppComponent, _super);
	    function AppComponent(locale, localization) {
	        _super.call(this, locale, localization);
	        this.locale = locale;
	        this.localization = localization;
	        this.locale.addLanguages(["en", "es"]);
	        this.locale.definePreferredLocale("en", "US", 30);
	        this.locale.definePreferredCurrency("USD");
	        this.localization.translationProvider("./lang/res_");
	        this.localization.updateTranslation();
	    }
	    AppComponent = __decorate([
	        core_1.Component({
	            selector: "app",
	            template: __webpack_require__(69)
	        }), 
	        __metadata('design:paramtypes', [angular2localization_1.LocaleService, angular2localization_1.LocalizationService])
	    ], AppComponent);
	    return AppComponent;
	}(angular2localization_1.Locale));
	exports.AppComponent = AppComponent;


/***/ },
/* 69 */
/***/ function(module, exports) {

	module.exports = "<router-outlet></router-outlet>\n"

/***/ },
/* 70 */
/***/ function(module, exports) {

	// removed by extract-text-webpack-plugin

/***/ }
/******/ ]);
//# sourceMappingURL=app.js.map