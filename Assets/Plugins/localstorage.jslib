mergeInto(LibraryManager.library, {
	SetID: function (id) {
		window.localStorage.setItem("analyticsID", UTF8ToString(id));
	},
	GetID: function () {
		let returnStr = window.localStorage.getItem("analyticsID");
		if (returnStr === null || returnStr === undefined) return null;
		let bufferSize = lengthBytesUTF8(returnStr) + 1;
		let buffer = _malloc(bufferSize);
		stringToUTF8(returnStr, buffer, bufferSize);
		return buffer;
	},

	SetItem: function (key, value) {
		window.localStorage.setItem(UTF8ToString(key), UTF8ToString(value));
	},

	GetItem: function (key) {
		let returnStr = window.localStorage.getItem(UTF8ToString(key));
		if (returnStr === null || returnStr === undefined) return null;
		let bufferSize = lengthBytesUTF8(returnStr) + 1;
		let buffer = _malloc(bufferSize);
		stringToUTF8(returnStr, buffer, bufferSize);
		return buffer;
	},
});
