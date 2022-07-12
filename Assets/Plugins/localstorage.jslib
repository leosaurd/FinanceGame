mergeInto(LibraryManager.library, {
	SetID: function (id) {
		window.localStorage.setItem("analyticsID", UTF8ToString(id));
	},
	GetID: function () {
		return window.localStorage.getItem("analyticsID");
	},
});
